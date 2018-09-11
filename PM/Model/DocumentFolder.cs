using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Images;
using PM.Shared;


namespace PM.Model
{
    public class DocumentFolder : ModelBase<DocumentFolder>
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
        private int? _ParentID;
        public int? ParentID
        {
            get { return _ParentID; }
            set
            {
                if (_ParentID == value)
                    return;
                _ParentID = value;
                NotifyPropertyChanged(m => m.ParentID);
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
        private bool _IsEmpty;
        public bool IsEmpty
        {
            get { return _IsEmpty; }
            private set
            {
                if (_IsEmpty == value)
                    return;
                _IsEmpty = value;
                NotifyPropertyChanged(m => m.IsEmpty);
            }
        }
        private bool _IsStarred;
        public bool IsStarred
        {
            get { return _IsStarred; }
            set
            {
                if (_IsStarred == value)
                    return;
                _IsStarred = value;
                NotifyPropertyChanged(m => m.IsStarred);
                ChangeFolderImage();
            }
        }
        private bool _IsHidden;
        public bool IsHidden
        {
            get { return _IsHidden; }
            set
            {
                if (_IsHidden == value)
                    return;
                _IsHidden = value;
                NotifyPropertyChanged(m => m.IsHidden);
                ChangeFolderImage();
            }
        }

        private ImageSource _FolderImage = SharedUtils.Instance.ConvertBitmapToImageSource(Properties.Resources.ResourceManager.GetObject("Folder") as Bitmap);
        public ImageSource FolderImage
        {
            get { return _FolderImage; }
            private set
            {
                if (_FolderImage == value)
                    return;
                _FolderImage = value;
                NotifyPropertyChanged(m => m.FolderImage);
            }
        }


        private ObservableCollection<CustomerDocument> _CustomerDocuments;
        public ObservableCollection<CustomerDocument> CustomerDocuments
        {
            get { return _CustomerDocuments; }
            set
            {
                if (_CustomerDocuments == value)
                    return;
                _CustomerDocuments = value;
                NotifyPropertyChanged(m => m.CustomerDocuments);
                IsEmpty = _CustomerDocuments?.Count > 0;
            }
        }


        public override void SaveChanges()
        {
            throw new NotImplementedException();
        }

        protected override void Populate(DocumentFolder item)
        {
            throw new NotImplementedException();
        }

        void ChangeFolderImage()
        {
            string imageResourceName = "Folder";
            if (IsHidden)
                imageResourceName = "HiddenFolder";
            else if (IsStarred)
                imageResourceName = "StarredFolder";
            FolderImage = SharedUtils.Instance.ConvertBitmapToImageSource(Properties.Resources.ResourceManager.GetObject(imageResourceName) as Bitmap);
        }
    }
}
