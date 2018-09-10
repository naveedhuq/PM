using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using PM.Model;
using PM.Shared;
using static PM.Model.Enumerators;

namespace PM.ViewModel
{
    public class CustomerDocumentsViewModel : ViewModelBase
    {
        private Customer _SelectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _SelectedCustomer; }
            set
            {
                if (_SelectedCustomer == value)
                    return;
                _SelectedCustomer = value;
                RaisePropertyChanged("SelectedCustomer");
                if (_SelectedCustomer != null)
                    DocumentFolders = DBHelper.Instance.GetCustomerDocumentFolders(_SelectedCustomer.ID);
            }
        }

        private ObservableCollection<DocumentFolder> _DocumntFolders;
        public ObservableCollection<DocumentFolder> DocumentFolders
        {
            get { return _DocumntFolders; }
            set
            {
                if (_DocumntFolders == value)
                    return;
                _DocumntFolders = value;
                RaisePropertyChanged("DocumentFolders");
            }
        }



        public CustomerDocumentsViewModel()
        {
            if (IsInDesignMode)
            {
                SelectedCustomer = new Customer();
                return;
            }

            Messenger.Default.Register<Customer>(
                recipient: this,
                token: MessageTokenEnum.SelectedCustomerChanged,
                action: customer => SelectedCustomer = customer);
        }

    }
}
