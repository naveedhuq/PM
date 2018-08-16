using System;
using System.Collections.Generic;
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
            TypeOfCompany
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

        #endregion

        public List<Customer> GetCustomers(bool ActiveOnly = true, string CustomerType = null)
        {
            return
                (from x in CustomerRepository
                 where (ActiveOnly ? x.IsActive : true)
                 && (CustomerType == null ? true : (x.CustomerType == CustomerType ))
                 select x).ToList();
        }


        public List<string> GetLookups(LookupTypesEnum LookupType)
        {
            return
                (
                    from x in LookupsRepository
                    where x.LookupType == LookupType.ToString()
                    orderby x.SortOrder
                    select x.LookupName
                ).ToList();
        }
    }
}
