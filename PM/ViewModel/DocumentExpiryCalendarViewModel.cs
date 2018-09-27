using System;
using System.Collections.ObjectModel;
using System.Linq;
using DevExpress.Mvvm;
using PM.Model;
using static PM.Model.Enumerators;

namespace PM.ViewModel
{
    public class DocumentExpiryCalendarViewModel : ViewModelBase
    {
        Shared.ILogger _logger;
        protected IMessageBoxService MessageBoxService { get { return GetService<IMessageBoxService>(); } }


        public ObservableCollection<DocumentFilter> Documents
        {
            get { return GetProperty(() => Documents); }
            set { SetProperty(() => Documents, value); }
        }

        public DateTime FocusedDate
        {
            get { return GetProperty(() => FocusedDate); }
            set
            {
                SetProperty(() => FocusedDate, value);                
            }
        }


        public DelegateCommand CloseCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Messenger.Default.Send<object>(
                        message: this,
                        token: MessageTokenEnum.CloseUserControl);
                });                
            }
        }

        public DocumentExpiryCalendarViewModel()
        {
            FocusedDate = DateTime.Today;
            Documents = new ObservableCollection<DocumentFilter>
                                (
                                    from x in Shared.DBHelper.Instance.GetDocumentsForFilter()
                                    where x.ExpirationDate != null
                                    select x
                                );
        }
    }
}
