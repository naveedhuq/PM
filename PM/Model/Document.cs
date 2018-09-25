using PM.Shared;
using System;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Media;


namespace PM.Model
{
    public class Document : ModelBase<Document>
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
            try
            {
                string imageResourceName = "IconUnknown";
                if (DocumentFileName != null)
                {
                    var fileExtension = Path.GetExtension(DocumentFileName.ToLower());
                    string lookupValue = (from x in DBHelper.Instance.LookupsRepository
                                          where x.LookupType == "ExtensionToImageMapping"
                                          where x.LookupName.Contains(fileExtension)
                                          select x.LookupName).FirstOrDefault();
                    if (lookupValue != null && lookupValue.Contains("|"))
                        imageResourceName = lookupValue.Split('|')[1];
                }
                FileImage = SharedUtils.Instance.ConvertBitmapToImageSource(Properties.Resources.ResourceManager.GetObject(imageResourceName) as Bitmap);
            }
            catch { FileImage = SharedUtils.Instance.ConvertBitmapToImageSource(Properties.Resources.ResourceManager.GetObject("IconUnknown") as Bitmap); }
        }


        public override void SaveChanges()
        {
            var ret = DBHelper.Instance.SaveDocument(this);
            DBHelper.Instance.SyncDocumentType();
            IsDirty = false;
        }

        protected override void Populate(Document item)
        {
            throw new NotImplementedException();
        }
        public static ObservableCollection<Document> GetCustomerDocuments(int customerID, int folderID, bool activeOnly = true)
        {
            var docs = from f in DBHelper.Instance.GetDocumentsForCustomer(customerID)
                       where (folderID == 0 ? true : ((f.DocumentFolderID ?? -1) == folderID)) == true
                       select f;
            return new ObservableCollection<Document>(docs);
        }

        public void Delete()
        {
            DBHelper.Instance.DeleteDocument(this);
            IsActive = false;
        }

        public void UploadRawDataFromFile(string fileName)
        {
            DBHelper.Instance.UploadDocumentData(ID, fileName);            
        }

        public Binary GetRawDocumentData()
        {
            return DBHelper.Instance.GetRawDocumentData(ID);
        }

    }
}
