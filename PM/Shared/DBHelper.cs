using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PM.Model;


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

        public List<Customer> CustomerRepository
        {
            get
            {
                return
                (
                    from x in _cx.Customer
                    select new Customer
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

        public List<Contact> ContactRepository
        {
            get
            {
                return
                (
                    from x in _cx.Contacts
                    select new Contact
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


        private List<LookupItem> _LookupsRepository;
        public List<LookupItem> LookupsRepository
        {
            get
            {
                if (_LookupsRepository == null)
                    _LookupsRepository = (from x in _cx.Lookups
                                          select new LookupItem()
                                          {
                                              LookupName = x.LookupName,
                                              LookupType = x.LookupType,
                                              SortOrder = x.SortOrder
                                          }).ToList();
                return _LookupsRepository;
            }
        }


        public List<DocumentFolder> DocumentFolderRepository
        {
            get
            {
                return
                (
                    from x in _cx.DocumentFolders
                    select new DocumentFolder()
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

        public ObservableCollection<Customer> GetCustomers(bool ActiveOnly = true, string CustomerType = null)
        {
            var customers = CustomerRepository;
            var contacts = ContactRepository;

            var ret = (from x in customers
                       where (ActiveOnly ? x.IsActive : true)
                       && (CustomerType == null ? true : (x.CustomerType == CustomerType))
                       select new Customer()
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
                           Contacts = new ObservableCollection<Contact>(from y in contacts where y.CustomerID == x.ID select y)
                       }).ToList();
            return new ObservableCollection<Customer>(ret);
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

        public ObservableCollection<DocumentFolder> GetCustomerDocumentFolders(int customerID)
        {
            if ((_cx.fn_GetDocumentFolderCountForCustomer(customerID) ?? 0) == 0)
                SaveDefaultDocumentFolder(customerID);

            var folders = from x in DocumentFolderRepository
                          where x.CustomerID == customerID
                          orderby x.ID
                          select new DocumentFolder()
                          {
                              ID = x.ID,
                              ParentID = x.ParentID,
                              CustomerID = x.CustomerID,
                              FolderName = x.FolderName,
                              IsStarred = x.IsStarred,
                              IsHidden = x.IsHidden
                          };
            return new ObservableCollection<DocumentFolder>(folders);
        }



        private void SaveDefaultDocumentFolder(int customerID)
        {
            _cx.sp_CreateDefaultDocumentFolders(customerID);
        }
    }
}
