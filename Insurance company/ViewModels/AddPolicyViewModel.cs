using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insurance_company.Models;
using System.Windows.Input;
using Insurance_company.Helpers;
using System.Windows;
using System.Data.Entity.Validation;

namespace Insurance_company.ViewModels
{
    class AddPolicyViewModel : BaseViewModel
    {
        private PolicySet _policy;

        private const string CAR = "Car";
        private const string HOUSE = "House";

        public PolicySet Policy
        {
            get { return _policy; }
            set
            {
                if (value != _policy)
                {
                    _policy = value;
                    RaisePropertyChanged(() => Policy);
                };
            }
        }

        private HouseSet _house;
        public HouseSet House
        {
            get { return _house; }
            set
            {
                if (value != _house)
                {
                    _house = value;
                    RaisePropertyChanged(() => House);
                };
            }
        }

        private AdressSet _address;
        public AdressSet Address
        {
            get { return _address; }
            set
            {
                if (value != _address)
                {
                    _address = value;
                    RaisePropertyChanged(() => Address);
                };
            }
        }

        private CarSet _car;
        public CarSet Car
        {
            get { return _car; }
            set
            {
                if (value != _car)
                {
                    _car = value;
                    RaisePropertyChanged(() => Car);
                };
            }
        }

        public ICommand SavePolicyCommand { get { return new DelegateCommand(OnPolicySave); } }

        public AddPolicyViewModel() {
            setEntities();
        }

        private void setEntities()
        {
            _policy = new PolicySet();
            _car = new CarSet();
            _house = new HouseSet();
            _address = new AdressSet();
        }

        private void OnPolicySave(object parameter)
        {
            using (var db = new InsuranceCompanyEntities())
            {
                ClientSet client = db.ClientSet.Find(Policy.ClientClientId); // Checking if the client exists
                if (client == null)
                {
                    MessageBox.Show("Client with given ID was not found!");
                    return;
                }
                DateTime now = DateTime.Now;
                Policy.StartDate = now; // Setting StartDate
                Policy.EndDate = now.AddYears(Policy.Duration); // Setting EndDate
                db.PolicySet.Add(Policy);
                if (Policy.ObjectType.Equals(CAR)) // If we are adding car policy
                {
                    db.CarSet.Add(Car);
                }
                else if (Policy.ObjectType.Equals(HOUSE)) // If we are adding house policy
                {
                    db.AdressSet.Add(Address);
                    House.AdressSet = Address; // Assign Address to the house
                    db.HouseSet.Add(House);
                }
                try
                {
                    db.SaveChanges();   // Save changes to the database                 
                    MessageBox.Show("Policy added successfully!");
                    // Clear the form:
                    Policy = new PolicySet();
                    Address = new AdressSet();
                    House = new HouseSet();
                    Car = new CarSet();
                }
                catch (DbEntityValidationException e)
                {
                    MessageBox.Show("Adding a new policy caused an error: " + e.Message);
                }
            }
        }
    }
}
