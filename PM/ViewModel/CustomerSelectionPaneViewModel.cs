using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
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
        [ServiceProperty(Key = "InputDialog")]
        protected IDialogService DialogService { get { return GetService<IDialogService>(); } }
        protected IMessageBoxService MessageBoxService { get { return GetService<IMessageBoxService>(); } }

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
                Messenger.Default.Send(
                    value,
                    MessageTokenEnum.SelectedCustomerChanged);
            }
        }
        public bool ShowInactiveCustomers
        {
            get { return GetProperty(() => ShowInactiveCustomers); }
            set { SetProperty(() => ShowInactiveCustomers, value); }
        }

        public string InputDialogText
        {
            get { return GetProperty(() => InputDialogText); }
            set { SetProperty(() => InputDialogText, value); }
        }


        public DelegateCommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try { RefreshData(); }
                    catch (Exception ex) { MessageBoxService.ShowMessage(messageBoxText: ex.Message, caption: "Error", button: MessageButton.OK, icon: MessageIcon.Error); }
                });
            }
        }

        public DelegateCommand NewCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    InputDialogText = null;
                    var ret = DialogService.ShowDialog(dialogButtons: MessageButton.OKCancel, title: "Enter Customer Name", viewModel: this);
                    if (ret != MessageResult.OK)
                        return;

                    var customer = new Customer()
                    {
                        CustomerName = InputDialogText,
                        CustomerType = SelectedCustomer?.CustomerType ?? "Business",
                        IsActive = true,
                        OpeningDate = DateTime.Today
                    };
                    customer.SaveChanges();
                    Customers.Add(customer);
                    SelectedCustomer = customer;
                });
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

        private void RefreshData()
        {
            Customers = DBHelper.Instance.GetCustomers(ActiveOnly: !ShowInactiveCustomers);
        }

    }
}
