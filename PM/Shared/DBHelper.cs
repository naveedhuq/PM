using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Linq;
using m = PM.Model;


namespace PM.Shared
{
    public class DBHelper
    {
        #region Singleton
        private static DBHelper _Instance;
        public static DBHelper Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new DBHelper();
                return _Instance;
            }
        }
        #endregion

        private DB.PMDataContext _cx = new DB.PMDataContext(Properties.Settings.Default.PMConnectionString);
        public DB.PMDataContext DataContext { get { return _cx; } }


        #region Repositories

        public List<m.Customer> CustomerRepository
        {
            get
            {
                return
                (
                    from x in _cx.Customer
                    select new m.Customer
                    {
                        ID = x.ID,
                        IsActive = x.IsActive,
                        OpeningDate = x.OpeningDate,
                        CustomerType = x.CustomerType,
                        CustomerName = x.CustomerName,
                        Personal_Gender = x.Personal_Gender,
                        Personal_BirthDate = x.Personal_BirthDate,
                        Personal_SSN = x.Personal_SSN,
                        Personal_LicenseID = x.Personal_LicenseID,
                        Business_TypeOfCompany = x.Business_TypeOfCompany,
                        Business_TaxID = x.Business_TaxID,
                        Notes = x.Notes
                    }
                ).ToList();
            }
        }

        public List<m.Contact> ContactRepository
        {
            get
            {
                return
                (
                    from x in _cx.Contacts
                    select new m.Contact
                    {
                        ID = x.ID,
                        IsActive = x.IsActive,
                        CustomerID = x.CustomerID,
                        ContactItemType = x.ContactItemType,
                        ContactItemValue = x.ContactItemValue
                    }
                ).ToList();
            }
        }


        public List<m.LookupItem> LookupsRepository
        {
            get
            {
                return
                (
                    from x in _cx.Lookups
                    orderby x.SortOrder
                    select new m.LookupItem
                    {
                        LookupName = x.LookupName,
                        LookupType = x.LookupType,
                        SortOrder = x.SortOrder
                    }
                ).ToList();
            }
        }


        public List<m.DocumentFolder> DocumentFolderRepository
        {
            get
            {
                return
                (
                    from x in _cx.DocumentFolders
                    where x.IsActive == true
                    select new m.DocumentFolder
                    {
                        ID = x.ID,
                        ParentID = x.ParentID,
                        CustomerID = x.CustomerID,
                        FolderName = x.FolderName,
                        IsStarred = x.IsStarred,
                        IsHidden = x.IsHidden
                    }
                ).ToList();
            }
        }

        #endregion



        #region DML Methods

        public m.DocumentFolder SaveDocumentFolder(m.DocumentFolder f)
        {
            var id = _cx.sp_SaveDocumentFolder(f.ID, f.CustomerID, f.ParentID, f.FolderName, f.IsStarred, f.IsHidden);
            if (id != 0)
                f.ID = id;
            return f;
        }

        public void DeleteDocumentFolder(m.DocumentFolder f)
        {
            _cx.sp_DeleteDocumentFolder(f.ID);
        }

        public m.Document SaveDocument(m.Document d)
        {
            var id = _cx.sp_SaveDocument(d.ID, d.CustomerID, d.DocumentFolderID, d.DocumentFileName, d.DocumentType, d.FileType, d.FileTimestamp, d.UploadDate, d.ExpirationDate, d.Comments);
            if (id != 0)
                d.ID = id;
            return d;
        }

        public void DeleteDocument(m.Document d)
        {
            _cx.sp_DeleteDocument(d.ID);
        }

        public void UploadDocumentData(int documentID, string fileName)
        {
            _cx.sp_UploadDocumentData(documentID, fileName);
        }

        public void SaveDocumentData(int documentID, Binary rawData)
        {
            _cx.sp_SaveDocumentData(documentID, rawData);
        }

        public void SyncDocumentType()
        {
            _cx.sp_SyncDocumentType();
        }
        public void AddEventLog(EventLog.LogEventType eventType, string message)
        {
            _cx.sp_AddEventLog(eventType.ToString(), message);
        }

        public void AddDocumentActivityLog(EventLog.LogEventType eventType, string customerName, string documentFileName, string folderName)
        {
            _cx.sp_AddDocumentActivityLog(eventType.ToString(), customerName, documentFileName, folderName);
        }

        public m.Customer SaveCustomer(m.Customer c)
        {
            var id = _cx.sp_SaveCustomer(
                c.ID,
                c.IsActive,
                c.OpeningDate,
                c.CustomerType,
                c.CustomerName,
                c.Personal_Gender,
                c.Personal_BirthDate,
                c.Personal_SSN,
                c.Personal_LicenseID,
                c.Business_TypeOfCompany,
                c.Business_TaxID,
                c.Notes);
            if (id != 0)
                c.ID = id;
            return c;
        }

        public m.Contact SaveContact(m.Contact c)
        {
            var id = _cx.sp_SaveContact(
                c.ID,
                c.IsActive,
                c.CustomerID,
                c.ContactItemType,
                c.ContactItemValue);
            if (id != 0)
                c.ID = id;
            return c;
        }

        #endregion


        public ObservableCollection<m.Customer> GetCustomers(bool ActiveOnly = true, string CustomerType = null)
        {
            var customers = CustomerRepository;
            var contacts = ContactRepository;

            var ret = (from x in customers
                       where (ActiveOnly ? x.IsActive : true)
                       && (CustomerType == null ? true : (x.CustomerType == CustomerType))
                       select new m.Customer
                       {
                           ID = x.ID,
                           IsActive = x.IsActive,
                           OpeningDate = x.OpeningDate,
                           CustomerType = x.CustomerType,
                           CustomerName = x.CustomerName,
                           Personal_Gender = x.Personal_Gender,
                           Personal_BirthDate = x.Personal_BirthDate,
                           Personal_SSN = x.Personal_SSN,
                           Personal_LicenseID = x.Personal_LicenseID,
                           Business_TypeOfCompany = x.Business_TypeOfCompany,
                           Business_TaxID = x.Business_TaxID,
                           Notes = x.Notes,
                           Contacts = new ObservableCollection<m.Contact>(from y in contacts where y.CustomerID == x.ID select y)
                       }).ToList();
            return new ObservableCollection<m.Customer>(ret);
        }

        public void CreateDefaultDocumentFolders(int customerID) { _cx.sp_CreateDefaultDocumentFolders(customerID); }

        public int GetDocumentFolderCountForCustomer(int customerID) { return _cx.fn_GetDocumentFolderCountForCustomer(customerID) ?? 0; }

        public ObservableCollection<m.DocumentFolder> GetDocumentFoldersForCustomer(int customerID)
        {
            var folders = from x in _cx.fn_GetDocumentFoldersForCustomer(customerID)
                          select new m.DocumentFolder
                          {
                              ID = x.ID,
                              ParentID = x.ParentID,
                              CustomerID = x.CustomerID,
                              FolderName = x.FolderName,
                              IsStarred = x.IsStarred,
                              IsHidden = x.IsHidden
                          };
            return new ObservableCollection<m.DocumentFolder>(folders);
        }

        public ObservableCollection<m.Document> GetDocumentsForCustomer(int customerID, bool activeOnly = true)
        {
            var docs = from x in _cx.fn_GetDocumentsForCustomer(customerID, activeOnly)
                       select new m.Document
                       {
                           ID = x.ID,
                           IsActive = x.IsActive,
                           CustomerID = x.CustomerID,
                           DocumentFolderID = x.DocumentFolderID,
                           DocumentFileName = x.DocumentFileName,
                           DocumentType = x.DocumentType,
                           FileType = x.FileType,
                           FileTimestamp = x.FileTimestamp,
                           UploadDate = x.UploadDate,
                           ExpirationDate = x.ExpirationDate,
                           Comments = x.Comments
                       };
            return new ObservableCollection<m.Document>(docs);
        }

        public Binary GetRawDocumentData(int documentID)
        {
            return _cx.fn_GetRawDocumentData(documentID);
        }

        public ObservableCollection<string> GetAllFolderNames(bool activeOnly = true)
        {
            var folders = from x in _cx.fn_GetAllFolderNames(activeOnly)
                          orderby x.FolderName
                          select x.FolderName;
            return new ObservableCollection<string>(folders);
        }

        public ObservableCollection<m.Document> GetAllDocuments(bool activeOnly = true)
        {
            var docs = from x in _cx.Documents
                       where (activeOnly ? x.IsActive : true)
                       select new m.Document
                       {
                           ID = x.ID,
                           IsActive = x.IsActive,
                           CustomerID = x.CustomerID,
                           DocumentFolderID = x.DocumentFolderID,
                           DocumentFileName = x.DocumentFileName,
                           DocumentType = x.DocumentType,
                           FileType = x.FileType,
                           FileTimestamp = x.FileTimestamp,
                           UploadDate = x.UploadDate,
                           ExpirationDate = x.ExpirationDate,
                           Comments = x.Comments
                       };
            return new ObservableCollection<m.Document>(docs);
        }

        public string GetDocumentFolderTree(int documentFolderID)
        {
            return _cx.fn_GetFolderTreeForDocumentFolderID(documentFolderID);
        }

        public ObservableCollection<m.DocumentFilter> GetDocumentsForFilter()
        {
            var docs = from x in _cx.fn_GetDocumentsForFilter()
                       select new m.DocumentFilter()
                       {
                           DocumentID = x.DocumentID,
                           CustomerID = x.CustomerID,
                           DocumentFolderID = x.DocumentFolderID,
                           IsDocumentDeleted = x.IsDocumentDeleted,
                           DocumentFileName = x.DocumentFileName,
                           FileType = x.FileType,
                           DocumentType = x.DocumentType,
                           FileTimestamp = x.FileTimestamp,
                           UploadDate = x.UploadDate,
                           ExpirationDate = x.ExpirationDate,
                           Comments = x.Comments,
                           CustomerName = x.CustomerName,
                           IsCustomerActive = x.IsCustomerActive,
                           FolderName = x.FolderName,
                           FolderTree = x.FolderTree,
                           IsFolderHidden = x.IsFolderHidden,
                           IsFolderBookmarked = x.IsFolderBookmarked
                       };
            return new ObservableCollection<m.DocumentFilter>(docs);
        }

        public bool CustomerNameExists(string customerName) { return (bool)_cx.fn_CustomerNameExists(customerName); }

        public ObservableCollection<m.Contact> GetContactsForCustomer(int customerID)
        {
            var ret = from c in ContactRepository
                      where c.CustomerID == customerID
                      select c;
            return new ObservableCollection<m.Contact>(ret);
        }
    }
}
