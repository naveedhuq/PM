using System;
using System.Collections.ObjectModel;
using System.Linq;
using DevExpress.Mvvm;
using PM.Model;
using PM.Shared;
using static PM.Model.Enumerators;

namespace PM.ViewModel
{
    public class DocumentSearchSelectionPaneViewModel : ViewModelBase
    {
        Shared.ILogger _logger;
        protected IMessageBoxService MessageBoxService { get { return GetService<IMessageBoxService>(); } }
        protected INotificationService AppNotificationService { get { return GetService<INotificationService>(); } }
        private readonly ISplashScreenService _WaitIndicatorService;


        #region Customer Filter Properties
        public ObservableCollection<Customer> Customers
        {
            get { return GetProperty(() => Customers); }
            set { SetProperty(() => Customers, value); }
        }
        public string CustomerName
        {
            get { return GetProperty(() => CustomerName); }
            set { SetProperty(() => CustomerName, value); }
        }
        public bool ShowInactiveCustomers
        {
            get { return GetProperty(() => ShowInactiveCustomers); }
            set
            {
                SetProperty(() => ShowInactiveCustomers, value);
                Customers = DBHelper.Instance.GetCustomers(ActiveOnly: !value);
            }
        }
        #endregion


        #region Folder Filter Properties
        public ObservableCollection<string> AllFolderNames
        {
            get { return GetProperty(() => AllFolderNames); }
            set { SetProperty(() => AllFolderNames, value); }
        }
        public string SelectedFolderName
        {
            get { return GetProperty(() => SelectedFolderName); }
            set { SetProperty(() => SelectedFolderName, value); }
        }
        public bool BookmarkedFoldersOnly
        {
            get { return GetProperty(() => BookmarkedFoldersOnly); }
            set { SetProperty(() => BookmarkedFoldersOnly, value); }
        }
        public bool IncludeHiddenFolders
        {
            get { return GetProperty(() => IncludeHiddenFolders); }
            set { SetProperty(() => IncludeHiddenFolders, value); }
        }
        #endregion


        #region Document Filter Properties
        public string DocumentFilename
        {
            get { return GetProperty(() => DocumentFilename); }
            set { SetProperty(() => DocumentFilename, value); }
        }
        public ObservableCollection<string> FileTypes
        {
            get { return GetProperty(() => FileTypes); }
            set { SetProperty(() => FileTypes, value); }
        }
        public string SelectedFileType
        {
            get { return GetProperty(() => SelectedFileType); }
            set { SetProperty(() => SelectedFileType, value); }
        }
        public ObservableCollection<string> DocumentTypes
        {
            get { return GetProperty(() => DocumentTypes); }
            set { SetProperty(() => DocumentTypes, value); }
        }
        public string SelectedDocumentType
        {
            get { return GetProperty(() => SelectedDocumentType); }
            set { SetProperty(() => SelectedDocumentType, value); }
        }
        public DateTime? FileCreationDateFrom
        {
            get { return GetProperty(() => FileCreationDateFrom); }
            set { SetProperty(() => FileCreationDateFrom, value); }
        }
        public DateTime? FileCreationDateTo
        {
            get { return GetProperty(() => FileCreationDateTo); }
            set { SetProperty(() => FileCreationDateTo, value); }
        }
        public DateTime? FileUploadDateFrom
        {
            get { return GetProperty(() => FileUploadDateFrom); }
            set { SetProperty(() => FileUploadDateFrom, value); }
        }
        public DateTime? FileUploadDateTo
        {
            get { return GetProperty(() => FileUploadDateTo); }
            set { SetProperty(() => FileUploadDateTo, value); }
        }
        public DateTime? FileExpiryDateFrom
        {
            get { return GetProperty(() => FileExpiryDateFrom); }
            set { SetProperty(() => FileExpiryDateFrom, value); }
        }
        public DateTime? FileExpiryDateTo
        {
            get { return GetProperty(() => FileExpiryDateTo); }
            set { SetProperty(() => FileExpiryDateTo, value); }
        }
        public string Comments
        {
            get { return GetProperty(() => Comments); }
            set { SetProperty(() => Comments, value); }
        }
        public bool IncludeDeletedDocuments
        {
            get { return GetProperty(() => IncludeDeletedDocuments); }
            set { SetProperty(() => IncludeDeletedDocuments, value); }
        }
        #endregion


        #region Commands
        public DelegateCommand ClearFilterCommand
        {
            get { return new DelegateCommand(() =>
            {
                ClearData();
                Messenger.Default.Send(
                        message: new ObservableCollection<DocumentFilter>(new ObservableCollection<DocumentFilter>()),
                        token: MessageTokenEnum.ApplyFilterInvoked);
            }); }
        }

        public DelegateCommand ApplyFilterCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        _WaitIndicatorService.ShowSplashScreen();
                        var docs = from x in DBHelper.Instance.GetDocumentsForFilter()
                                   select x;

                        // Applying Filters
                        if (!ShowInactiveCustomers)
                            docs = docs.Where(x => x.IsCustomerActive == true);

                        if (CustomerName != null)
                            docs = docs.Where(x => x.CustomerName.ToLower().Contains(CustomerName.ToLower()));

                        if (SelectedFolderName != null)
                            docs = docs.Where(x => x.FolderTree.ToLower().Contains(SelectedFolderName.ToLower()));

                        if (BookmarkedFoldersOnly)
                            docs = docs.Where(x => x.IsFolderBookmarked == true);

                        if (!IncludeHiddenFolders)
                            docs = docs.Where(x => x.IsFolderHidden == false);

                        if (DocumentFilename != null)
                            docs = docs.Where(x => x.DocumentFileName.ToLower().Contains(DocumentFilename.ToLower()));

                        if (SelectedFileType != null)
                            docs = docs.Where(x => x.FileType == SelectedFileType);

                        if (SelectedDocumentType != null)
                            docs = docs.Where(x => x.DocumentType == SelectedDocumentType);

                        if (FileCreationDateFrom != null)
                            docs = docs.Where(x => (x.FileTimestamp >= FileCreationDateFrom));
                        if (FileCreationDateTo != null)
                            docs = docs.Where(x => (x.FileTimestamp <= FileCreationDateTo));

                        if (FileUploadDateFrom != null)
                            docs = docs.Where(x => (x.UploadDate >= FileUploadDateFrom));
                        if (FileUploadDateTo != null)
                            docs = docs.Where(x => (x.UploadDate <= FileUploadDateTo));

                        if (FileExpiryDateFrom != null)
                            docs = docs.Where(x => (x.ExpirationDate != null && x.ExpirationDate >= FileExpiryDateFrom));
                        if (FileExpiryDateTo != null)
                            docs = docs.Where(x => (x.ExpirationDate != null && x.ExpirationDate <= FileExpiryDateTo));

                        if (Comments != null)
                            docs = docs.Where(x => x.Comments.ToLower().Contains(Comments.ToLower()));

                        if (!IncludeDeletedDocuments)
                            docs = docs.Where(x => x.IsDocumentDeleted == false);

                        // Send filtered document list
                        Messenger.Default.Send(
                            message: new ObservableCollection<DocumentFilter>(docs),
                            token: MessageTokenEnum.ApplyFilterInvoked);
                        _WaitIndicatorService.HideSplashScreen();
                    }
                    catch (Exception ex )
                    {
                        _WaitIndicatorService.HideSplashScreen();
                        ShowError(ex);
                    }
                });
            }
        }

        #endregion


        public DocumentSearchSelectionPaneViewModel()
        {
            RefreshData();
            _WaitIndicatorService = DevExpress.Mvvm.ServiceContainer.Default.GetService<ISplashScreenService>("WaitIndicatorService");
        }

        private void RefreshData()
        {
            Customers = DBHelper.Instance.GetCustomers();
            AllFolderNames = DocumentFolder.GetAllFolderNames();
            FileTypes = LookupItem.GetLookupStrings(LookupItem.LookupTypesEnum.FileType);
            FileTypes.Insert(0, string.Empty);
            DocumentTypes = LookupItem.GetLookupStrings(LookupItem.LookupTypesEnum.DocumentType);
            DocumentTypes.Insert(0, string.Empty);            
        }

        private void ClearData()
        {
            CustomerName = null;
            ShowInactiveCustomers = false;
            SelectedFolderName = null;
            BookmarkedFoldersOnly = false;
            IncludeHiddenFolders = false;
            DocumentFilename = null;
            SelectedFileType = null;
            SelectedDocumentType = null;
            FileCreationDateFrom = null;
            FileCreationDateTo = null;
            FileUploadDateFrom = null;
            FileUploadDateTo = null;
            FileExpiryDateFrom = null;
            FileExpiryDateTo = null;
            Comments = null;
            IncludeDeletedDocuments = false;
        }

        private void ShowError(Exception ex)
        {
            MessageBoxService.ShowMessage(messageBoxText: ex.Message, caption: "Error", button: MessageButton.OK, icon: MessageIcon.Error);
            _logger.Error(ex.Message, ex);
        }
    }
}
