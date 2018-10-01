using PM.Shared;
using System;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PM.Model
{
    public class Contact : ModelBase<Contact>
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
                ContactItemTypeImage = GetContactItemTypeImage();
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

        private ImageSource _ContactItemTypeImage;
        public ImageSource ContactItemTypeImage
        {
            get { return _ContactItemTypeImage; }
            set
            {
                if (_ContactItemTypeImage == value)
                    return;
                _ContactItemTypeImage = value;
                NotifyPropertyChanged(m => m.ContactItemTypeImage);
            }
        }


        public override void SaveChanges()
        {
            var ret = DBHelper.Instance.SaveContact(this);
            ID = ret.ID;
            IsDirty = false;
        }

        protected override void Populate(Contact item)
        {
            throw new NotImplementedException();
        }

        ImageSource GetContactItemTypeImage()
        {
            var defaultImage = SharedUtils.Instance.ConvertBitmapToImageSource(Properties.Resources.ResourceManager.GetObject("OtherContacts") as Bitmap);
            if (ContactItemType == null)
                return null;

            var image = Properties.Resources.ResourceManager.GetObject(ContactItemType.Replace(" ", ""));
            if (image != null)
                return SharedUtils.Instance.ConvertBitmapToImageSource(image as Bitmap);
            else
                return defaultImage;
        }
    }
}
