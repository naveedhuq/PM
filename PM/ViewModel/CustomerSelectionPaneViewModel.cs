using DevExpress.Mvvm;
using PM.Model;
using PM.Shared;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using static PM.Model.Enumerators;

namespace PM.ViewModel
{
    public class CustomerSelectionPaneViewModel : ViewModelBase
    {
        public ObservableCollection<Customer> Customers
        {
            get { return GetProperty(() => Customers); }
            set { SetProperty(() => Customers, value); }
        }
        public Customer SelectedCustomer
        {
            get { return GetProperty(() => SelectedCustomer); }
            set
            {
                SetProperty(() => SelectedCustomer, value);
                Messenger.Default.Send(value, MessageTokenEnum.SelectedCustomerChanged);
            }
        }

        public CustomerSelectionPaneViewModel()
        {
            if (IsInDesignMode)
            {
                SelectedCustomer = new Customer();
                return;
            }
            RefreshData();
        }

        void RefreshData()
        {
            Customers = DBHelper.Instance.GetCustomers(true);
        }

    }
}
