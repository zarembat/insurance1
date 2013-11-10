using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insurance_company.Models;
using System.Windows.Input;
using Insurance_company.Helpers;
using Insurance_company.Views;
using System.Windows;


namespace Insurance_company.ViewModels
{
    class EditClientViewModel : BaseViewModel
    {

        private InsuranceCompanyEntities db = new InsuranceCompanyEntities();
        private ClientSet _client;
        public ClientSet Client
        {
            get { return _client; }
            set
            {
                if (value != _client)
                {
                    _client = value;
                    RaisePropertyChanged(() => "_client");
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
                    RaisePropertyChanged(() => "_address");
                };
            }
        }

        public ICommand SaveClientCommand { get { return new DelegateCommand(OnCustomerSave); } }

        public EditClientViewModel()
        {
        }

        public EditClientViewModel(ClientSet client) // This is called from ClientsViewModel when an item is clicked twice
        {
            _client = client;
            using (var db = new InsuranceCompanyEntities())
            {
                var address = db.AdressSet.Where(a => a.AdressId == client.AdressAdressId).FirstOrDefault(); // Looking for an Address
                if (address == null)
                    MessageBox.Show("No address...");
                else
                {
                    _address = address;
                }

            }
        }

        private void OnCustomerSave(object parameter)
        {
            using (var db = new InsuranceCompanyEntities()) {
                var client = db.ClientSet.Find(Client.ClientId); // Looking for an old entry in the database
                var address = db.AdressSet.Find(Address.AdressId);
                db.Entry(client).CurrentValues.SetValues(Client); // Updating values
                db.Entry(address).CurrentValues.SetValues(Address);
                db.SaveChanges(); // Saving changes
                MessageBox.Show("Saved!");
            }
        }


    }
}
