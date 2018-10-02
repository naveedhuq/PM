using PM.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM.Model
{
    public class RelatedParty : ModelBase<RelatedParty>
    {
        private int _ID;
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

        private bool _IsActive = true;
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

        private int _CustomerID;
        public int CustomerID
        {
            get { return _CustomerID; }
            set
            {
                if (_CustomerID == value)
                    return;
                _CustomerID = value;
                NotifyPropertyChanged(m => m.CustomerID);
            }
        }

        private string _PartyName;
        public string PartyName
        {
            get { return _PartyName; }
            set
            {
                if (_PartyName == value)
                    return;
                _PartyName = value;
                NotifyPropertyChanged(m => m.PartyName);
            }
        }

        private string _EntityType;
        public string EntityType
        {
            get { return _EntityType; }
            set
            {
                if (_EntityType == value)
                    return;
                _EntityType = value;
                NotifyPropertyChanged(m => m.EntityType);
            }
        }

        private string _Gender;
        public string Gender
        {
            get { return _Gender; }
            set
            {
                if (_Gender == value)
                    return;
                _Gender = value;
                NotifyPropertyChanged(m => m.Gender);
            }
        }

        private DateTime? _BirthDate;
        public DateTime? BirthDate
        {
            get { return _BirthDate; }
            set
            {
                if (_BirthDate == value)
                    return;
                _BirthDate = value;
                NotifyPropertyChanged(m => m.BirthDate);
            }
        }

        private string _SSN;
        public string SSN
        {
            get { return _SSN; }
            set
            {
                if (_SSN == value)
                    return;
                _SSN = value;
                NotifyPropertyChanged(m => m.SSN);
            }
        }

        private string _LicenseID;
        public string LicenseID
        {
            get { return _LicenseID; }
            set
            {
                if (_LicenseID == value)
                    return;
                _LicenseID = value;
                NotifyPropertyChanged(m => m.LicenseID);
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



        public override void SaveChanges()
        {
            var ret = DBHelper.Instance.SaveRelatedParty(this);
            ID = ret.ID;
            IsDirty = false;
        }

        protected override void Populate(RelatedParty item)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return PartyName;
        }
    }
}
