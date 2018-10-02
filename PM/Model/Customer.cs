using System;
using System.Collections.ObjectModel;
using PM.Shared;

namespace PM.Model
{
    public class Customer: ModelBase<Customer>
    {
        private int _ID = 0;
        public int ID
        {
            get { return _ID; }
            set
            {
                if (_ID == value)
                    return;
                _ID = value;
                NotifyPropertyChanged(m => m.ID);
            }
        }

        private bool _IsActive;
        public bool IsActive
        {
            get { return _IsActive; }
            set
            {
                if (_IsActive == value)
                    return;
                _IsActive = value;
                NotifyPropertyChanged(m => m.IsActive);
            }
        }

        private DateTime? _OpeningDate;
        public DateTime? OpeningDate
        {
            get { return _OpeningDate; }
            set
            {
                if (_OpeningDate == value)
                    return;
                _OpeningDate = value;
                NotifyPropertyChanged(m => m.OpeningDate);
            }
        }

        private string _CustomerType = "Personal";
        public string CustomerType
        {
            get { return _CustomerType; }
            set
            {
                if (_CustomerType == value)
                    return;
                _CustomerType = value;
                NotifyPropertyChanged(m => m.CustomerType);
            }
        }

        private string _CustomerName;
        public string CustomerName
        {
            get { return _CustomerName; }
            set
            {
                if (_CustomerName == value)
                    return;
                _CustomerName = value;
                NotifyPropertyChanged(m => m.CustomerName);
            }
        }

        private string _Personal_Gender;
        public string Personal_Gender
        {
            get { return _Personal_Gender; }
            set
            {
                if (_Personal_Gender == value)
                    return;
                _Personal_Gender = value;
                NotifyPropertyChanged(m => m.Personal_Gender);
            }
        }

        private DateTime? _Personal_BirthDate;
        public DateTime? Personal_BirthDate
        {
            get { return _Personal_BirthDate; }
            set
            {
                if (_Personal_BirthDate == value)
                    return;
                _Personal_BirthDate = value;
                NotifyPropertyChanged(m => m.Personal_BirthDate);
            }
        }

        private string _Personal_SSN;
        public string Personal_SSN
        {
            get { return _Personal_SSN; }
            set
            {
                if (_Personal_SSN == value)
                    return;
                _Personal_SSN = value;
                NotifyPropertyChanged(m => m.Personal_SSN);
            }
        }

        private string _Personal_LicenseID;
        public string Personal_LicenseID
        {
            get { return _Personal_LicenseID; }
            set
            {
                if (_Personal_LicenseID == value)
                    return;
                _Personal_LicenseID = value;
                NotifyPropertyChanged(m => m.Personal_LicenseID);
            }
        }

        private string _Business_TypeOfCompany;
        public string Business_TypeOfCompany
        {
            get { return _Business_TypeOfCompany; }
            set
            {
                if (_Business_TypeOfCompany == value)
                    return;
                _Business_TypeOfCompany = value;
                NotifyPropertyChanged(m => m.Business_TypeOfCompany);
            }
        }

        private string _Business_TaxID;
        public string Business_TaxID
        {
            get { return _Business_TaxID; }
            set
            {
                if (_Business_TaxID == value)
                    return;
                _Business_TaxID = value;
                NotifyPropertyChanged(m => m.Business_TaxID);
            }
        }

        private string _Notes;
        public string Notes
        {
            get { return _Notes; }
            set
            {
                if (_Notes == value)
                    return;
                _Notes = value;
                NotifyPropertyChanged(m => m.Notes);
            }
        }

        private ObservableCollection<Contact> _Contacts;
        public ObservableCollection<Contact> Contacts
        {
            get { return _Contacts; }
            set
            {
                if (_Contacts == value)
                    return;
                _Contacts = value;
                NotifyPropertyChanged(m => m.Contacts);
            }
        }

        private ObservableCollection<RelatedParty> _RelatedParties;
        public ObservableCollection<RelatedParty> RelatedParties
        {
            get { return _RelatedParties; }
            set
            {
                if (_RelatedParties == value)
                    return;
                _RelatedParties = value;
                NotifyPropertyChanged(m => m.RelatedParties);
            }
        }



        public bool IsPersonalCustomer
        {
            get { return (CustomerType ?? "") == "Personal"; }
        }

        public bool IsBusinessCustomer
        {
            get { return (CustomerType ?? "") == "Business"; }
        }


        public override void SaveChanges()
        {
            var ret = DBHelper.Instance.SaveCustomer(this);
            ID = ret.ID;
            IsDirty = false;
        }

        protected override void Populate(Customer item)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return CustomerName;
        }
        public static bool CustomerNameExists(string customerName)
        {
            return DBHelper.Instance.CustomerNameExists(customerName);
        }

        public void RefreshContacts()
        {
            Contacts = DBHelper.Instance.GetContactsForCustomer(ID);
        }
    }
}
