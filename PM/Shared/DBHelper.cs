using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public enum LookupTypesEnum
        {
            CustomerType,
            ContactItemType,
            TypeOfCompany,
            ServiceType
        }

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


        private List<m.LookupItem> _LookupsRepository;
        public List<m.LookupItem> LookupsRepository
        {
            get
            {
                if (_LookupsRepository == null)
                    _LookupsRepository = (from x in _cx.Lookups
                                          select new m.LookupItem
                                          {
                                              LookupName = x.LookupName,
                                              LookupType = x.LookupType,
                                              SortOrder = x.SortOrder
                                          }).ToList();
                return _LookupsRepository;
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

        public List<m.AppSetting> AppSettingsRepository
        {
            get
            {
                return
                (
                    from x in _cx.AppSettings
                    select new m.AppSetting
                    {
                        ID = x.ID,
                        SettingsName = x.SettingsName,
                        SettingsValue = x.SettingsValue
                    }
                ).ToList();
            }
        }

        #endregion



        #region Save Methods

        public m.DocumentFolder SaveDocumentFolder(m.DocumentFolder f)
        {
            var id = _cx.sp_SaveDocumentFolders(f.ID, f.CustomerID, f.ParentID, f.FolderName, f.IsStarred, f.IsHidden);
            if (id != 0)
                f.ID = id;
            return f;
        }

        public void DeleteDocumentFolder(m.DocumentFolder f)
        {
            _cx.sp_DeleteDocumentFolder(f.ID);
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


        public ObservableCollection<string> GetLookups(LookupTypesEnum LookupType)
        {
            return
                new ObservableCollection<string>
                (
                    from x in LookupsRepository
                    where x.LookupType == LookupType.ToString()
                    orderby x.SortOrder
                    select x.LookupName
                );
        }

        public void CreateDefaultDocumentFolders(int customerID) { _cx.sp_CreateDefaultDocumentFolders(customerID); }

        public int GetDocumentFolderCountForCustomer(int customerID) { return _cx.fn_GetDocumentFolderCountForCustomer(customerID) ?? 0; }

        public ObservableCollection<m.DocumentFolder> GetDocumentFoldersForCustomer(int customerID)
        {
            var folders = from x in _cx.DocumentFolders
                          where x.CustomerID == customerID
                          where x.IsActive == true
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

        public ObservableCollection<m.Document> GetDocumentsForCustomer(int customerID)
        {
            var docs = from x in _cx.Documents
                       where x.CustomerID == customerID
                       where x.IsActive == true
                       select new m.Document
                       {
                           ID = x.ID,
                           CustomerID = x.CustomerID,
                           DocumentFolderID = x.DocumentFolderID,
                           DocumentFileName = x.DocumentFileName,
                           DocumentType = x.DocumentType,
                           UploadDate = x.UploadDate,
                           ExpirationDate = x.ExpirationDate,
                           Comments = x.Comments
                       };
            return new ObservableCollection<m.Document>(docs);
        }

        public string GetAppSetting(string settingName)
        {
            return (from x in AppSettingsRepository where x.SettingsName == settingName select x.SettingsValue).FirstOrDefault();
        }


    }
}
