using DevExpress.Mvvm;
using PM.Model;
using PM.Shared;
using System;
using System.Collections.ObjectModel;
using static PM.Model.Enumerators;

namespace PM.ViewModel
{
    public class CustomerContactsViewModel : ViewModelBase
    {
        ILogger _logger;
        protected IMessageBoxService MessageBoxService { get { return GetService<IMessageBoxService>(); } }

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

        
        public DelegateCommand NewCommand
        {
            get
            {
                return new DelegateCommand(() => { SelectedContact = new Contact() { CustomerID = SelectedCustomer.ID }; });
            }
        }
        public DelegateCommand SaveCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        SelectedContact.SaveChanges();
                        SelectedContact = null;
                        SelectedCustomer.RefreshContacts();
                        SelectedCustomer.IsDirty = false;
                    }
                    catch (Exception ex) { ShowError(ex); }
                }, () => SelectedContact != null && 
                         SelectedContact.IsDirty &&
                         SelectedContact.ContactItemType != null &&
                         SelectedContact.ContactItemValue != null);
            }
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

        void ShowError(Exception ex)
        {
            MessageBoxService.ShowMessage(messageBoxText: ex.Message, caption: "Error", button: MessageButton.OK, icon: MessageIcon.Error);
            _logger.Error(ex.Message, ex);
        }
    }
}
