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

        public ObservableCollection<string> CustomerTypes
        {
            get { return GetProperty(() => CustomerTypes); }
            set { SetProperty(() => CustomerTypes, value); }
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
        }
        
    }
}
