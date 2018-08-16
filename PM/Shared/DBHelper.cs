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


        private List<CustomerDocument> _CustomerDocumentRepository;
        public List<CustomerDocument> CustomerDocumentRepository
        {
            get
            {
                if (_CustomerDocumentRepository == null)
                    _CustomerDocumentRepository = LoadFakeCustomerDocumentRepository();
                return _CustomerDocumentRepository;
            }
        }

        private List<CustomerDocument> LoadFakeCustomerDocumentRepository()
        {
            List<CustomerDocument> ret = new List<CustomerDocument>();
            ret.Add(new CustomerDocument() { FileName = "W2 Tax Document.doc", ExpiryDate = DateTime.Parse("10/15/2019"), Notes = "Customer's W2 tax document for 2017" });
            ret.Add(new CustomerDocument() { FileName = "License.pdf", ExpiryDate = DateTime.Parse("1/3/2022"), Notes = "NY Drive's license" });
            ret.Add(new CustomerDocument() { FileName = "PowerOfAttorny.docx", ExpiryDate = null, Notes = "Power of Attorny document" });
            ret.Add(new CustomerDocument() { FileName = "Bills.xls", ExpiryDate = null, Notes = "List of bills submitted" });
            ret.Add(new CustomerDocument() { FileName = "passport.jpeg", ExpiryDate = DateTime.Parse("05/12/2023"), Notes = "Copy of passport" });
            return ret;
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
                           Contacts = new ObservableCollection<Contact>(from y in contacts where y.CustomerID == x.ID select y),
                           CustomerDocuments = new ObservableCollection<CustomerDocument>(CustomerDocumentRepository)
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

        
    }
}
