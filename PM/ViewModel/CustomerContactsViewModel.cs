using DevExpress.Mvvm;
using PM.Model;
using PM.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PM.Model.Enumerators;

namespace PM.ViewModel
{
    public class CustomerContactsViewModel : ViewModelBase
    {
        ILogger _logger;
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
        public ObservableCollection<string> ContactItemTypes
        {
            get { return GetProperty(() => ContactItemTypes); }
            set { SetProperty(() => ContactItemTypes, value); }
        }

        public CustomerContactsViewModel()
        {
            if (IsInDesignMode)
            {
                SelectedCustomer = new Customer();
                return;
            }
            _logger = LogManager.GetLogger(GetType());
            ContactItemTypes = LookupItem.GetLookupStrings(LookupItem.LookupTypesEnum.ContactItemType);

            Messenger.Default.Register<Customer>(
                recipient: this,
                token: MessageTokenEnum.SelectedCustomerChanged,
                action: customer => SelectedCustomer = customer);
        }
    }
}
