using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Images;
using PM.Shared;


namespace PM.Model
{
    public class DocumentFolder : ModelBase<DocumentFolder>
    {
        private static string _SpecialFolderName_All = DBHelper.Instance.GetAppSetting("SPECIAL_FOLDERNAME_ALL");
        private static string _SpecialFolderName_UnCategorized = DBHelper.Instance.GetAppSetting("SPECIAL_FOLDERNAME_UNCATEGORIZED");


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
        private bool _IsDefault;
        public bool IsDefault
        {
            get { return _IsDefault; }
            set
            {
                if (_IsDefault == value)
                    return;
                _IsDefault = value;
                NotifyPropertyChanged(m => m.IsDefault);
                ChangeFolderImage();
            }
        }

        private bool _IsRoot;
        public bool IsRoot
        {
            get { return _IsRoot; }
            set
            {
                if (_IsRoot == value)
                    return;
                _IsRoot = value;
                NotifyPropertyChanged(m => m.IsRoot);
                ChangeFolderImage();
            }
        }

        public bool IsDraggableFolder
        {
            get { return !(IsRoot || IsDefault); }
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

        public override void SaveChanges()
        {
            var ret = DBHelper.Instance.SaveDocumentFolder(this);
            IsDirty = false;
        }

        public void Delete()
        {
            DBHelper.Instance.DeleteDocumentFolder(this);
        }

        protected override void Populate(DocumentFolder item)
        {
            throw new NotImplementedException();
        }

        public static ObservableCollection<DocumentFolder> GetCustomerDocumentFolders(int customerID, bool showHiddenFolder = true)
        {
            if (DBHelper.Instance.GetDocumentFolderCountForCustomer(customerID) == 0)
                DBHelper.Instance.CreateDefaultDocumentFolders(customerID);

            var folders = (from x in DBHelper.Instance.GetDocumentFoldersForCustomer(customerID)
                           where (showHiddenFolder ? true : !x.IsHidden) == true
                           orderby x.ID
                           select new DocumentFolder()
                           {
                               ID = x.ID,
                               ParentID = x.ParentID ?? 0,
                               CustomerID = x.CustomerID,
                               FolderName = x.FolderName,
                               IsStarred = x.IsStarred,
                               IsHidden = x.IsHidden
                           }).ToList();
            folders.Insert(0, new DocumentFolder { ID = -1, FolderName = _SpecialFolderName_UnCategorized, IsDefault = true, CustomerID = customerID, ParentID = 0 });
            folders.Insert(0, new DocumentFolder { ID = 0, FolderName = _SpecialFolderName_All, IsRoot = true, CustomerID = customerID });
            return new ObservableCollection<DocumentFolder>(folders);
        }

        public static ObservableCollection<string> GetAllFolderNames()
        {
            return DBHelper.Instance.GetAllFolderNames();
        }


        private void ChangeFolderImage()
        {
            string imageResourceName = "Folder";
            if (IsRoot)
                imageResourceName = "AllFolders";
            if (IsDefault)
                imageResourceName = "UnCategorized";

            if (IsHidden)
                imageResourceName = "HiddenFolder";
            else if (IsStarred)
                imageResourceName = "StarredFolder";
            FolderImage = SharedUtils.Instance.ConvertBitmapToImageSource(Properties.Resources.ResourceManager.GetObject(imageResourceName) as Bitmap);
        }
    }
}
