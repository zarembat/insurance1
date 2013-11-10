//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Insurance_company.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text.RegularExpressions;

    public partial class PolicySet : IDataErrorInfo
    {
        public PolicySet()
        {
            this.CarSet = new HashSet<CarSet>();
            this.HouseSet = new HashSet<HouseSet>();
        }
    
        public int PolicyId { get; set; }
        public int Duration { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string ObjectType { get; set; }
        public int? ClientClientId { get; set; }
    
        public virtual ICollection<CarSet> CarSet { get; set; }
        public virtual ClientSet ClientSet { get; set; }
        public virtual ICollection<HouseSet> HouseSet { get; set; }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                string result = string.Empty;
                switch (columnName)
                {
                    case "ClientClientId":
                        {
                            if (ClientClientId == null)
                            {
                                result = "Cliend ID is required!";
                                break;
                            }

                            string clientid = ClientClientId.ToString();
                            if (!Regex.IsMatch(clientid, @"^[0-9]+$"))
                                result = "Only letters!";

                            break;
                        }
                    case "ObjectType":
                        {
                            if (ObjectType == null)
                                result = "Object type is required!";

                            break;
                        }
                    case "Duration":
                        {
                            if (!(Duration == 1 || Duration == 2 || Duration == 3 || Duration == 4 || Duration == 5))
                                result = "Choose duration!";

                            break;
                        }
                };
                return result;
            }
        }
    }
}