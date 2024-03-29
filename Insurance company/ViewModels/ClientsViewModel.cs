﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Insurance_company.Models;
using System.Windows.Input;
using Insurance_company.Helpers;
using Insurance_company.Views;
using System.Windows;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace Insurance_company.ViewModels
{

    class ClientsViewModel : BaseViewModel
    {

        private ObservableCollection<ClientSet> _clients;

        public ObservableCollection<ClientSet> Clients
        {
            get { return _clients; }
            set
            {
                if (_clients != value)
                {
                    _clients = value;
                    RaisePropertyChanged(() => Clients);
                }
            }

        }

        public ICommand ClientsGridLeftDoubleClickCommand { get { return new DelegateCommand(OnClientsGridLeftDoubleClick); } }

        private void OnClientsGridLeftDoubleClick(object parameter) // Clicking twice on a DataGrid item opens Edit Window
        {
            if (!(parameter is ClientSet))
                return;
            EditClient EditClientWindow = new EditClient(); // Creating Edit Window object
            EditClientWindow.DataContext = new EditClientViewModel(parameter as ClientSet); // Passing client object to the Edit Window
            EditClientWindow.Show();
        }

        public void refresh(System.Data.Entity.DbSet<ClientSet> clients) // Refreshing the list of clients in the DataGrid
        {
            // Clients = new ObservableCollection<ClientSet>(clients);
            ObservableCollection<ClientSet> clientss = new ObservableCollection<ClientSet>(clients);
            for (int i = Clients.Count; i < clients.Count(); i++)
            {
                Clients.Add(clientss[i]);
            }
        }

        public ClientsViewModel()
        {
            using (var db = new InsuranceCompanyEntities())
            {
                _clients = new ObservableCollection<ClientSet>(db.ClientSet);
            }
        }

        public ClientsViewModel(ObservableCollection<ClientSet> clients)
        {
            _clients = clients;
        }

        public ClientsViewModel(DbSet<ClientSet> clients)
        {
            _clients = new ObservableCollection<ClientSet>(clients);
        }

    }
}
