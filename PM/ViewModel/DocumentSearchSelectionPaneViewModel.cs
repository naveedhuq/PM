using System;
using System.Collections.ObjectModel;
using System.Linq;
using DevExpress.Mvvm;
using PM.Model;
using PM.Shared;

namespace PM.ViewModel
{
    public class DocumentSearchSelectionPaneViewModel : ViewModelBase
    {
        protected IMessageBoxService MessageBoxService { get { return GetService<IMessageBoxService>(); } }
        protected INotificationService AppNotificationService { get { return GetService<INotificationService>(); } }
        private readonly ISplashScreenService _WaitIndicatorService;


        #region Customer Filter Properties
        public ObservableCollection<Customer> Customers
        {
            get { return GetProperty(() => Customers); }
            set { SetProperty(() => Customers, value); }
        }
        public Customer SelectedCustomer
        {
            get { return GetProperty(() => SelectedCustomer); }
            set { SetProperty(() => SelectedCustomer, value); }
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
        public bool HiddenFoldersOnly
        {
            get { return GetProperty(() => HiddenFoldersOnly); }
            set { SetProperty(() => HiddenFoldersOnly, value); }
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
            get { return new DelegateCommand(() => { ClearData(); }); }
        }

        public DelegateCommand ApplyFilterCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    _WaitIndicatorService.ShowSplashScreen();
                    var docs = from doc in DBHelper.Instance.GetAllDocuments(!IncludeDeletedDocuments);

                    _WaitIndicatorService.HideSplashScreen();
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
            SelectedCustomer = null;
            ShowInactiveCustomers = false;
            SelectedFolderName = null;
            BookmarkedFoldersOnly = false;
            HiddenFoldersOnly = false;
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
}
}
