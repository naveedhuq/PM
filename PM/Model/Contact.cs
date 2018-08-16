using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM.Model
{
    public class Contact : ModelBase<Contact>
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


        private string _ContactItemType;
        public string ContactItemType
        {
            get { return _ContactItemType; }
            set
            {
                if (_ContactItemType == value)
                    return;
                _ContactItemType = value;
                NotifyPropertyChanged(m => m.ContactItemType);
            }
        }

        private string _ContactItemValue;
        public string ContactItemValue
        {
            get { return _ContactItemValue; }
            set
            {
                if (_ContactItemValue == value)
                    return;
                _ContactItemValue = value;
                NotifyPropertyChanged(m => m.ContactItemValue);
            }
        }


        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }

        protected override void Populate(Contact item)
        {
            throw new NotImplementedException();
        }
    }
}
