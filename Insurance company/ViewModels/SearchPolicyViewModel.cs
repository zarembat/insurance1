﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Insurance_company.Helpers;
using Insurance_company.Models;
using Insurance_company.Views;
using System.Windows.Data;
using System.Data.Entity.Infrastructure;
using System.Collections;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;

namespace Insurance_company.ViewModels
{
    class SearchPolicyViewModel : BaseViewModel
    {

        private ObservableCollection<PolicySet> _policies;

        public ObservableCollection<PolicySet> Policies
        {
            get { return _policies; }
            set
            {
                if (_policies != value)
                {
                    _policies = value;
                    RaisePropertyChanged(() => "Policies");
                }
            }

        }

        List<EntityParameter> PolicyParameters = new List<EntityParameter>();

        private PolicySet _policy;

        public PolicySet Policy
        {
            get { return _policy; }
            set
            {
                if (value != _policy)
                {
                    _policy = value;
                    RaisePropertyChanged(() => "_policy");
                };
            }
        }

        public ICommand SearchPolicyCommand { get { return new DelegateCommand(OnPolicySearch); } }

        public SearchPolicyViewModel() {
            _policy = new PolicySet();
        }

        private string CreatePolicySearchQuery()
        {

            string esqlQuery = null;

            if (Policy.Duration > 0 || Policy.ObjectType != null || Policy.ClientClientId > 0) // If at least one search field is not empty, we create a query
                esqlQuery += @"SELECT VALUE Policy FROM InsuranceCompanyEntities.PolicySet AS Policy WHERE ";
               
            if (Policy.Duration > 0) {
                EntityParameter param = new EntityParameter(); // Creating a parameter
                param.ParameterName = "Duration"; // Setting parameter name
                param.Value = Policy.Duration; // Setting parameter value
                PolicyParameters.Add(param); // Adding parameter to collection so that it can be used when executing the query
                esqlQuery += "Policy.Duration = @Duration AND ";
            }
            if (Policy.ObjectType != null && !Policy.ObjectType.Equals("")) {
                EntityParameter param = new EntityParameter();
                param.ParameterName = "ObjectType";
                param.Value = Policy.ObjectType;
                PolicyParameters.Add(param);
                esqlQuery += "Policy.ObjectType = @ObjectType AND ";
            }
            if (Policy.ClientClientId > 0) {
                EntityParameter param = new EntityParameter();
                param.ParameterName = "ClientClientId";
                param.Value = Policy.ClientClientId;
                PolicyParameters.Add(param);
                esqlQuery += "Policy.ClientClientId = @ClientClientId AND ";
            }
            if (esqlQuery != null)
                esqlQuery = esqlQuery.Remove(esqlQuery.Length - 5);
            return esqlQuery;
        }

        /*public ICommand PoliciesGridLeftDoubleClickCommand { get { return new DelegateCommand(OnPoliciesGridLeftDoubleClick); } }

        private void OnPoliciesGridLeftDoubleClick(object parameter)
        {
            if (!(parameter is PolicySet))
                return;
            EditPolicy EditPolicyWindow = new EditPolicy();
            EditPolicyWindow.DataContext = new EditPolicyViewModel(parameter as PolicySet);
            EditPolicyWindow.Show();
        }*/

        private void OnPolicySearch(object parameter)
        {

            string policyQuery = CreatePolicySearchQuery(); // Creating query

            if (policyQuery == null) // If nothing was specified, return
                return;

            _policies = new ObservableCollection<PolicySet>();

            using (EntityConnection conn = new EntityConnection("name=InsuranceCompanyEntities"))
            {
                conn.Open(); // Opening database connection

                using (EntityCommand cmd = new EntityCommand(policyQuery, conn)) // Creating a command
                {

                    foreach (EntityParameter param in PolicyParameters) // Adding parameters which we added before to the command
                    {
                        cmd.Parameters.Add(param);
                    }

                    using (DbDataReader rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess)) // Executing query
                    {
                        while (rdr.Read()) // Reading records found
                        {
                            PolicySet policy = new PolicySet();
                            policy.PolicyId = (int)rdr["PolicyId"];
                            policy.Duration = (int)rdr["Duration"];                            
                            policy.StartDate = (DateTime)rdr["StartDate"];   
                            policy.EndDate = (DateTime)rdr["EndDate"];
                            policy.ObjectType = rdr["ObjectType"].ToString();
                            policy.ClientClientId = (int)rdr["ClientClientId"];                                                                                 
                            _policies.Add(policy);
                        }
                    }
                }

                conn.Close();
                if (_policies.Count > 0) // If something was found, we display the results
                {
                    PoliciesWindow pw = new PoliciesWindow();
                    pw.DataContext = new PoliciesViewModel(_policies);
                    pw.Show();
                }
                else
                    MessageBox.Show("No policies matching these criteria were found!");

            }

            PolicyParameters = new List<EntityParameter>(); // Zeroing the parameters
        }
        
    }
}
