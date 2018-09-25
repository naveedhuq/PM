using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using PM.Model;
using PM.Shared;

namespace PM.ViewModel
{
    public class DocumentSearchSelectionPaneViewModel : ViewModelBase
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

        public DocumentSearchSelectionPaneViewModel()
        {
            Customers = DBHelper.Instance.GetCustomers();
        }
    }
}
