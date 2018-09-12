using PM.Shared;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media;


namespace PM.Model
{
    public class CustomerDocument : ModelBase<CustomerDocument>
    {
        private string _FileName;
        public string FileName
        {
            get { return _FileName; }
            set
            {
                if (_FileName == value)
                    return;
                _FileName = value;
                NotifyPropertyChanged(m => m.FileName);
                ChangeFileImage();
            }
        }

        private DateTime? _ExpiryDate;
        public DateTime? ExpiryDate
        {
            get { return _ExpiryDate; }
            set
            {
                if (_ExpiryDate == value)
                    return;
                _ExpiryDate = value;
                NotifyPropertyChanged(m => m.ExpiryDate);
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

        private ImageSource _FileImage;
        public ImageSource FileImage
        {
            get { return _FileImage; }
            set
            {
                if (_FileImage == value)
                    return;
                _FileImage = value;
                NotifyPropertyChanged(m => m.FileImage);
            }
        }


        private void ChangeFileImage()
        {
            string imageResourceName = "IconUnknown";
            if (FileName != null)
            {
                var fileExtension = Path.GetExtension(FileName.ToLower());
                string lookupValue = (from x in DBHelper.Instance.LookupsRepository
                                      where x.LookupType == "ExtensionToImageMapping"
                                      where x.LookupName.Contains(fileExtension)
                                      select x.LookupName).FirstOrDefault();
                if (lookupValue.Contains("|"))
                    imageResourceName = lookupValue.Split('|')[1];
            }
            FileImage = SharedUtils.Instance.ConvertBitmapToImageSource(Properties.Resources.ResourceManager.GetObject(imageResourceName) as Bitmap);
        }


        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }

        protected override void Populate(CustomerDocument item)
        {
            throw new NotImplementedException();
        }


    }
}
