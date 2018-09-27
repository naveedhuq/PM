using DevExpress.Mvvm;
using PM.Model;
using static PM.Model.Enumerators;

namespace PM.ViewModel
{
    public class CustomerEntryViewModel : ViewModelBase
    {
        Shared.ILogger _logger;
        public Customer SelectedCustomer
        {
            get { return GetProperty(() => SelectedCustomer); }
            set { SetProperty(() => SelectedCustomer, value); }
        }


        public CustomerEntryViewModel()
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
