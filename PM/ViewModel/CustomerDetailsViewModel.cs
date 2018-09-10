using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using PM.Model;
using PM.Shared;
using static PM.Model.Enumerators;

namespace PM.ViewModel
{
    public class CustomerDetailsViewModel : ViewModelBase
    {
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

        public ObservableCollection<string> TypesOfCompany
        {
            get { return GetProperty(() => TypesOfCompany); }
            set { SetProperty(() => TypesOfCompany, value); }
        }

        public ObservableCollection<string> ServiceTypes
        {
            get { return GetProperty(() => ServiceTypes); }
            set { SetProperty(() => ServiceTypes, value); }
        }

        public CustomerDetailsViewModel()
        {
            if (IsInDesignMode)
            {
                SelectedCustomer = new Customer();
                return;
            }
            RefreshData();

            Messenger.Default.Register<Customer>(
                recipient: this,
                token: MessageTokenEnum.SelectedCustomerChanged,
                action: customer => SelectedCustomer = customer);
        }

        void RefreshData()
        {
            CustomerTypes = new ObservableCollection<string>(DBHelper.Instance.GetLookups(DBHelper.LookupTypesEnum.CustomerType));
            TypesOfCompany = new ObservableCollection<string>(DBHelper.Instance.GetLookups(DBHelper.LookupTypesEnum.TypeOfCompany));
            ServiceTypes = new ObservableCollection<string>(DBHelper.Instance.GetLookups(DBHelper.LookupTypesEnum.ServiceType));
        }
    }
}
