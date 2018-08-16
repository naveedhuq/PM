using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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


        public ImageSource ImageSource
        {
            get
            {
                if (FileName == null)
                    return GetImage("/Images/text.ico");

                if (FileName.EndsWith(".pdf"))
                    return GetImage("/Images/pdf.ico");

                if (FileName.EndsWith(".xls"))
                    return GetImage("/Images/excel.ico");
                if (FileName.EndsWith(".xlsx"))
                    return GetImage("/Images/excel.ico");

                if (FileName.EndsWith(".doc"))
                    return GetImage("/Images/word.ico");
                if (FileName.EndsWith(".docx"))
                    return GetImage("/Images/word.ico");
                if (FileName.EndsWith(".rtf"))
                    return GetImage("/Images/word.ico");

                if (FileName.EndsWith(".png"))
                    return GetImage("/Images/image.ico");
                if (FileName.EndsWith(".jpg"))
                    return GetImage("/Images/image.ico");
                if (FileName.EndsWith(".jpeg"))
                    return GetImage("/Images/image.ico");
                if (FileName.EndsWith(".bmp"))
                    return GetImage("/Images/image.ico");

                /*
                switch (Path.GetExtension(FileName)?.ToLower())
                {
                    case "pdf":
                        return GetImage("/Images/pdf.ico");
                    case "xls":
                    case "xlsx":
                        return GetImage("/Images/excel.ico");
                    case "doc":
                    case "docx":
                    case "rtf":
                        return GetImage("/Images/word.ico");
                    case "png":
                    case "jpg":
                    case "jpeg":
                    case "tiff":
                    case "bmp":
                        return GetImage("/Images/image.ico");                 
                }
                */
                return GetImage("/Images/text.ico");
            }
        }


        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }

        protected override void Populate(CustomerDocument item)
        {
            throw new NotImplementedException();
        }

        ImageSource GetImage(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }

    }
}
