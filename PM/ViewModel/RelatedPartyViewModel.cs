using DevExpress.Mvvm;
using PM.Model;
using PM.Shared;
using System;
using System.Collections.ObjectModel;
using static PM.Model.Enumerators;


namespace PM.ViewModel
{
    public class RelatedPartyViewModel : ViewModelBase
    {
        ILogger _logger;
        protected IMessageBoxService MessageBoxService { get { return GetService<IMessageBoxService>(); } }
        public Customer SelectedCustomer
        {
            get { return GetProperty(() => SelectedCustomer); }
            set { SetProperty(() => SelectedCustomer, value); }
        }

        public RelatedParty SelectedRelatedParty
        {
            get { return GetProperty(() => SelectedRelatedParty); }
            set { SetProperty(() => SelectedRelatedParty, value); }
        }

        public ObservableCollection<string> EntityTypes
        {
            get { return GetProperty(() => EntityTypes); }
            set { SetProperty(() => EntityTypes, value); }
        }

        public DelegateCommand NewCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    SelectedRelatedParty = new RelatedParty()
                    {
                        CustomerID = SelectedCustomer.ID
                    };
                });
            }
        }


        public RelatedPartyViewModel()
        {
            if (IsInDesignMode)
            {
                SelectedCustomer = new Customer();
                return;
            }
            _logger = LogManager.GetLogger(GetType());
            EntityTypes = LookupItem.GetLookupStrings(LookupItem.LookupTypesEnum.EntityType);

            Messenger.Default.Register<Customer>(
                recipient: this,
                token: MessageTokenEnum.SelectedCustomerChanged,
                action: customer => SelectedCustomer = customer);
        }
    }
}
