using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Media;
using PM.Shared;

namespace PM.Model
{
    public class DocumentFilter : ModelBase<DocumentFilter>
    {

        private int _DocumentID;
        public int DocumentID
        {
            get { return _DocumentID; }
            set
            {
                if (_DocumentID == value)
                    return;
                _DocumentID = value;
                NotifyPropertyChanged(m => m.DocumentID);
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

        private int? _DocumentFolderID;
        public int? DocumentFolderID
        {
            get { return _DocumentFolderID; }
            set
            {
                if (_DocumentFolderID == value)
                    return;
                _DocumentFolderID = value;
                NotifyPropertyChanged(m => m.DocumentFolderID);
            }
        }

        private bool? _IsDocumentDeleted;
        public bool? IsDocumentDeleted
        {
            get { return _IsDocumentDeleted; }
            set
            {
                if (_IsDocumentDeleted == value)
                    return;
                _IsDocumentDeleted = value;
                NotifyPropertyChanged(m => m.IsDocumentDeleted);
            }
        }

        private string _DocumentFileName;
        public string DocumentFileName
        {
            get { return _DocumentFileName; }
            set
            {
                if (_DocumentFileName == value)
                    return;
                _DocumentFileName = value;
                NotifyPropertyChanged(m => m.DocumentFileName);
            }
        }

        private string _FileType;
        public string FileType
        {
            get { return _FileType; }
            set
            {
                if (_FileType == value)
                    return;
                _FileType = value;
                NotifyPropertyChanged(m => m.FileType);
                ChangeFileImage();
            }
        }

        private string _DocumentType;
        public string DocumentType
        {
            get { return _DocumentType; }
            set
            {
                if (_DocumentType == value)
                    return;
                _DocumentType = value;
                NotifyPropertyChanged(m => m.DocumentType);
            }
        }

        private DateTime? _FileTimestamp;
        public DateTime? FileTimestamp
        {
            get { return _FileTimestamp; }
            set
            {
                if (_FileTimestamp == value)
                    return;
                _FileTimestamp = value;
                NotifyPropertyChanged(m => m.FileTimestamp);
            }
        }

        private DateTime? _UploadDate;
        public DateTime? UploadDate
        {
            get { return _UploadDate; }
            set
            {
                if (_UploadDate == value)
                    return;
                _UploadDate = value;
                NotifyPropertyChanged(m => m.UploadDate);
            }
        }

        private DateTime? _ExpirationDate;
        public DateTime? ExpirationDate
        {
            get { return _ExpirationDate; }
            set
            {
                if (_ExpirationDate == value)
                    return;
                _ExpirationDate = value;
                NotifyPropertyChanged(m => m.ExpirationDate);
            }
        }

        private string _Comments;
        public string Comments
        {
            get { return _Comments; }
            set
            {
                if (_Comments == value)
                    return;
                _Comments = value;
                NotifyPropertyChanged(m => m.Comments);
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

        private bool? _IsCustomerActive;
        public bool? IsCustomerActive
        {
            get { return _IsCustomerActive; }
            set
            {
                if (_IsCustomerActive == value)
                    return;
                _IsCustomerActive = value;
                NotifyPropertyChanged(m => m.IsCustomerActive);
            }
        }

        private string _FolderName;
        public string FolderName
        {
            get { return _FolderName; }
            set
            {
                if (_FolderName == value)
                    return;
                _FolderName = value;
                NotifyPropertyChanged(m => m.FolderName);
            }
        }

        private string _FolderTree;
        public string FolderTree
        {
            get { return _FolderTree; }
            set
            {
                if (_FolderTree == value)
                    return;
                _FolderTree = value;
                NotifyPropertyChanged(m => m.FolderTree);
            }
        }

        private bool? _IsFolderHidden;
        public bool? IsFolderHidden
        {
            get { return _IsFolderHidden; }
            set
            {
                if (_IsFolderHidden == value)
                    return;
                _IsFolderHidden = value;
                NotifyPropertyChanged(m => m.IsFolderHidden);
                if (_IsFolderHidden != null && (bool)_IsFolderHidden)
                    FolderImage = SharedUtils.Instance.ConvertBitmapToImageSource(Properties.Resources.ResourceManager.GetObject("HiddenFolder") as Bitmap);

            }
        }

        private bool? _IsFolderBookmarked;
        public bool? IsFolderBookmarked
        {
            get { return _IsFolderBookmarked; }
            set
            {
                if (_IsFolderBookmarked == value)
                    return;
                _IsFolderBookmarked = value;
                NotifyPropertyChanged(m => m.IsFolderBookmarked);
                if (_IsFolderBookmarked != null && (bool)_IsFolderBookmarked)
                    FolderImage = SharedUtils.Instance.ConvertBitmapToImageSource(Properties.Resources.ResourceManager.GetObject("StarredFolder") as Bitmap);
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

        private ImageSource _FolderImage = SharedUtils.Instance.ConvertBitmapToImageSource(Properties.Resources.ResourceManager.GetObject("Folder") as Bitmap);
        public ImageSource FolderImage
        {
            get { return _FolderImage; }
            set
            {
                if (_FolderImage == value)
                    return;
                _FolderImage = value;
                NotifyPropertyChanged(m => m.FolderImage);
            }
        }




        public static ObservableCollection<DocumentFilter> GetDocumentsForFilter()
        {
            return DBHelper.Instance.GetDocumentsForFilter();
        }

        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }

        protected override void Populate(DocumentFilter item)
        {
            throw new NotImplementedException();
        }

        private void ChangeFileImage()
        {
            try
            {
                if (FileType == null)
                    return;
                FileImage = SharedUtils.Instance.ConvertBitmapToImageSource(Properties.Resources.ResourceManager.GetObject($"Icon{FileType}") as Bitmap);
            }
            catch { FileImage = SharedUtils.Instance.ConvertBitmapToImageSource(Properties.Resources.ResourceManager.GetObject("IconOther") as Bitmap); }
        }
    }
}
