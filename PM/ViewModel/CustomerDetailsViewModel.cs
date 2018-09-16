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

        private void RefreshData()
        {
            CustomerTypes =LookupItem.GetLookupStrings(LookupItem.LookupTypesEnum.CustomerType);
            TypesOfCompany = LookupItem.GetLookupStrings(LookupItem.LookupTypesEnum.TypeOfCompany);
            ServiceTypes = LookupItem.GetLookupStrings(LookupItem.LookupTypesEnum.ServiceType);
        }
    }
}
