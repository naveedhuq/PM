using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DevExpress.Mvvm;
using PM.Model;
using PM.Shared;


namespace PM.ViewModel
{
    public class CustomerEntryViewModel : ViewModelBase
    {
        public ObservableCollection<Customer> Customers
        {
            get { return GetProperty(() => Customers); }
            set { SetProperty(() => Customers, value); }
        }

        public Customer SelectedCustomer
        {
            get { return GetProperty(() => SelectedCustomer); }
            set { SetProperty(() => SelectedCustomer, value); }
        }
        public Contact SelectedContact
        {
            get { return GetProperty(() => SelectedContact); }
            set { SetProperty(() => SelectedContact, value); }
        }



        public ObservableCollection<string> CustomerTypes
        {
            get { return GetProperty(() => CustomerTypes); }
            set { SetProperty(() => CustomerTypes, value); }
        }

        public ObservableCollection<string> TypesOfCompany
        {
            get { return GetProperty(() => TypesOfCompany); }
            set { SetProperty(() => TypesOfCompany, value); }
        }

        public ObservableCollection<string> ContactItemTypes
        {
            get { return GetProperty(() => ContactItemTypes); }
            set { SetProperty(() => ContactItemTypes, value); }
        }

        public ObservableCollection<string> ServiceTypes
        {
            get { return GetProperty(() => ServiceTypes); }
            set { SetProperty(() => ServiceTypes, value); }
        }

        public CustomerEntryViewModel()
        {
            if (IsInDesignMode)
            {
                SelectedCustomer = new Customer();
                return;
            }
            RefreshData();
        }


        private void RefreshData()
        {
            Customers = new ObservableCollection<Customer>(DBHelper.Instance.GetCustomers(true));
            CustomerTypes = new ObservableCollection<string>(DBHelper.Instance.GetLookups(DBHelper.LookupTypesEnum.CustomerType));
            TypesOfCompany = new ObservableCollection<string>(DBHelper.Instance.GetLookups(DBHelper.LookupTypesEnum.TypeOfCompany));
            ContactItemTypes = new ObservableCollection<string>(DBHelper.Instance.GetLookups(DBHelper.LookupTypesEnum.ContactItemType));
            ServiceTypes = new ObservableCollection<string>(DBHelper.Instance.GetLookups(DBHelper.LookupTypesEnum.ServiceType));
        }
        
    }
}
