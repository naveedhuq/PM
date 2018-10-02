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

        public DB.PMDataContext DataContext { get; } = new DB.PMDataContext(Properties.Settings.Default.PMConnectionString);


        #region Repositories

        public List<m.Customer> CustomerRepository
        {
            get
            {
                return
                (
                    from x in DataContext.Customer
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
                    from x in DataContext.Contacts
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
                    from x in DataContext.Lookups
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
                    from x in DataContext.DocumentFolders
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

        public List<m.RelatedParty> RelatedPartyRepository
        {
            get
            {
                return
                (
                    from x in DataContext.RelatedParties
                    select new m.RelatedParty
                    {
                        ID = x.ID,
                        IsActive = x.IsActive,
                        CustomerID = x.CustomerID,
                        PartyName = x.PartyName,
                        EntityType = x.EntityType,
                        Gender = x.Gender,
                        BirthDate = x.BirthDate,
                        SSN = x.SSN,
                        LicenseID = x.LicenseID,
                        Notes = x.Notes
                    }
                ).ToList();
            }
        }

        
        #endregion



        #region DML Methods

        public m.DocumentFolder SaveDocumentFolder(m.DocumentFolder f)
        {
            var id = DataContext.sp_SaveDocumentFolder(f.ID, f.CustomerID, f.ParentID, f.FolderName, f.IsStarred, f.IsHidden);
            if (id != 0)
                f.ID = id;
            return f;
        }

        public void DeleteDocumentFolder(m.DocumentFolder f)
        {
            DataContext.sp_DeleteDocumentFolder(f.ID);
        }

        public m.Document SaveDocument(m.Document d)
        {
            var id = DataContext.sp_SaveDocument(d.ID, d.CustomerID, d.DocumentFolderID, d.DocumentFileName, d.DocumentType, d.FileType, d.FileTimestamp, d.UploadDate, d.ExpirationDate, d.Comments);
            if (id != 0)
                d.ID = id;
            return d;
        }

        public void DeleteDocument(m.Document d)
        {
            DataContext.sp_DeleteDocument(d.ID);
        }

        public void UploadDocumentData(int documentID, string fileName)
        {
            DataContext.sp_UploadDocumentData(documentID, fileName);
        }

        public void SaveDocumentData(int documentID, Binary rawData)
        {
            DataContext.sp_SaveDocumentData(documentID, rawData);
        }

        public void SyncDocumentType()
        {
            DataContext.sp_SyncDocumentType();
        }
        public void AddEventLog(EventLog.LogEventType eventType, string message)
        {
            DataContext.sp_AddEventLog(eventType.ToString(), message);
        }

        public void AddDocumentActivityLog(EventLog.LogEventType eventType, string customerName, string documentFileName, string folderName)
        {
            DataContext.sp_AddDocumentActivityLog(eventType.ToString(), customerName, documentFileName, folderName);
        }

        public m.Customer SaveCustomer(m.Customer c)
        {
            var id = DataContext.sp_SaveCustomer(
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
            var id = DataContext.sp_SaveContact(
                c.ID,
                c.IsActive,
                c.CustomerID,
                c.ContactItemType,
                c.ContactItemValue);
            if (id != 0)
                c.ID = id;
            return c;
        }

        public m.RelatedParty SaveRelatedParty(m.RelatedParty r)
        {
            var id = DataContext.sp_SaveRelatedParty(
                r.ID,
                r.IsActive,
                r.CustomerID,
                r.PartyName,
                r.EntityType,
                r.Gender,
                r.BirthDate,
                r.SSN,
                r.LicenseID,
                r.Notes);
            if (id != 0)
                r.ID = id;
            return r;
        }

        #endregion



        #region Utility Selection Functions


        public ObservableCollection<m.Customer> GetCustomers(bool ActiveOnly = true, string CustomerType = null)
        {
            var customers = CustomerRepository;
            var contacts = ContactRepository;
            var related = RelatedPartyRepository;

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
                           Contacts = new ObservableCollection<m.Contact>(from y in contacts where y.CustomerID == x.ID select y),
                           RelatedParties = new ObservableCollection<m.RelatedParty>(from r in related where r.CustomerID == x.ID select r)
                       }).ToList();
            return new ObservableCollection<m.Customer>(ret);
        }

        public void CreateDefaultDocumentFolders(int customerID) { DataContext.sp_CreateDefaultDocumentFolders(customerID); }

        public int GetDocumentFolderCountForCustomer(int customerID) { return DataContext.fn_GetDocumentFolderCountForCustomer(customerID) ?? 0; }

        public ObservableCollection<m.DocumentFolder> GetDocumentFoldersForCustomer(int customerID)
        {
            var folders = from x in DataContext.fn_GetDocumentFoldersForCustomer(customerID)
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
            var docs = from x in DataContext.fn_GetDocumentsForCustomer(customerID, activeOnly)
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
            return DataContext.fn_GetRawDocumentData(documentID);
        }

        public ObservableCollection<string> GetAllFolderNames(bool activeOnly = true)
        {
            var folders = from x in DataContext.fn_GetAllFolderNames(activeOnly)
                          orderby x.FolderName
                          select x.FolderName;
            return new ObservableCollection<string>(folders);
        }

        public ObservableCollection<m.Document> GetAllDocuments(bool activeOnly = true)
        {
            var docs = from x in DataContext.Documents
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
            return DataContext.fn_GetFolderTreeForDocumentFolderID(documentFolderID);
        }

        public ObservableCollection<m.DocumentFilter> GetDocumentsForFilter()
        {
            var docs = from x in DataContext.fn_GetDocumentsForFilter()
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


        public bool CustomerNameExists(string customerName) { return (bool)DataContext.fn_CustomerNameExists(customerName); }


        public ObservableCollection<m.Contact> GetContactsForCustomer(int customerID)
        {
            var ret = from c in ContactRepository
                      where c.CustomerID == customerID
                      select c;
            return new ObservableCollection<m.Contact>(ret);
        }

        public ObservableCollection<m.RelatedParty> GetRelatedPartiesForCustomer(int customerID)
        {
            var ret = from r in RelatedPartyRepository
                      where r.CustomerID == customerID
                      select r;
            return new ObservableCollection<m.RelatedParty>(ret);
        }

        #endregion
    }
}
