using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid.DragDrop;
using PM.Model;
using PM.Shared;
using static PM.Model.Enumerators;

namespace PM.ViewModel
{
    public class CustomerDocumentsViewModel : ViewModelBase
    {
        [ServiceProperty(Key = "InputDialog")]
        protected IDialogService DialogService { get { return GetService<IDialogService>(); } }
        protected IMessageBoxService MessageBoxService { get { return GetService<IMessageBoxService>(); } }
        protected IOpenFileDialogService OpenFileDialogService { get { return GetService<IOpenFileDialogService>(); } }


        


        private Customer _SelectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _SelectedCustomer; }
            set
            {
                if (_SelectedCustomer == value)
                    return;
                _SelectedCustomer = value;
                RaisePropertyChanged("SelectedCustomer");
                RefreshFolderTree();
                SelectedDocumentFolder = null;
                RefreshDocumentGrid();
            }
        }

        private ObservableCollection<DocumentFolder> _DocumntFolders;
        public ObservableCollection<DocumentFolder> DocumentFolders
        {
            get { return _DocumntFolders; }
            set
            {
                if (_DocumntFolders == value)
                    return;
                _DocumntFolders = value;
                RaisePropertyChanged("DocumentFolders");
            }
        }

        private DocumentFolder _SelectedDocumentFolder;
        public DocumentFolder SelectedDocumentFolder
        {
            get { return _SelectedDocumentFolder; }
            set
            {
                if (_SelectedDocumentFolder == value)
                    return;
                _SelectedDocumentFolder = value;
                RaisePropertyChanged("SelectedDocumentFolder");
                RefreshDocumentGrid();
            }
        }

        private ObservableCollection<Document> _Documents;
        public ObservableCollection<Document> Documents
        {
            get { return _Documents; }
            set
            {
                if (_Documents == value)
                    return;
                _Documents = value;
                RaisePropertyChanged("Documents");
            }
        }

        private Document _SelectedDocument;
        public Document SelectedDocument
        {
            get { return _SelectedDocument; }
            set
            {
                if (_SelectedDocument == value)
                    return;
                _SelectedDocument = value;
                RaisePropertyChanged("SelectedDocument");
                IsDocumentSelectionValid = (_SelectedDocument != null);
            }
        }

        private string _InputDialogText;
        public string InputDialogText
        {
            get { return _InputDialogText; }
            set
            {
                if (_InputDialogText == value)
                    return;
                _InputDialogText = value;
                RaisePropertyChanged("InputDialogText");
            }
        }

        private bool _ShowHiddenFolders;
        public bool ShowHiddenFolders
        {
            get { return _ShowHiddenFolders; }
            set
            {
                if (_ShowHiddenFolders == value)
                    return;
                _ShowHiddenFolders = value;
                RaisePropertyChanged("ShowHiddenFolders");
                RefreshFolderTree();
            }
        }

        private bool _IsDocumentSelectionValid;
        public bool IsDocumentSelectionValid
        {
            get { return _IsDocumentSelectionValid; }
            set
            {
                if (_IsDocumentSelectionValid == value)
                    return;
                _IsDocumentSelectionValid = value;
                RaisePropertyChanged("IsDocumentSelectionValid");
            }
        }

        private ObservableCollection<string> _DocumentTypes = new ObservableCollection<string>();
        public ObservableCollection<string> DocumentTypes
        {
            get { return _DocumentTypes; }
            set
            {
                if (_DocumentTypes == value)
                    return;
                _DocumentTypes = value;
                RaisePropertyChanged("DocumentTypes");
            }
        }




        public DelegateCommand RenameFolderCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        InputDialogText = SelectedDocumentFolder?.FolderName;
                        var ret = DialogService.ShowDialog(dialogButtons: MessageButton.OKCancel, title: "Enter Folder Name", viewModel: this);
                        if (ret != MessageResult.OK)
                            return;
                        SelectedDocumentFolder.FolderName = InputDialogText;
                        SelectedDocumentFolder.SaveChanges();
                    }
                    catch (Exception ex) { MessageBoxService.ShowMessage(messageBoxText: ex.Message, caption: "Error", button: MessageButton.OK, icon: MessageIcon.Error); }
                }, () => SelectedDocumentFolder?.IsDefault == false && SelectedDocumentFolder?.IsRoot == false);
            }
        }

        public DelegateCommand HideFolderCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        SelectedDocumentFolder.IsHidden = !SelectedDocumentFolder.IsHidden;
                        SelectedDocumentFolder.SaveChanges();
                        RefreshFolderTree();
                    }
                    catch (Exception ex) { MessageBoxService.ShowMessage(messageBoxText: ex.Message, caption: "Error", button: MessageButton.OK, icon: MessageIcon.Error); }
                }, () => SelectedDocumentFolder?.IsDefault == false && SelectedDocumentFolder?.IsRoot == false);
            }
        }

        public DelegateCommand BookMarkFolderCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        SelectedDocumentFolder.IsStarred = !SelectedDocumentFolder.IsStarred;
                        SelectedDocumentFolder.SaveChanges();
                    }
                    catch (Exception ex) { MessageBoxService.ShowMessage(messageBoxText: ex.Message, caption: "Error", button: MessageButton.OK, icon: MessageIcon.Error); }
                }, () => SelectedDocumentFolder?.IsDefault == false && SelectedDocumentFolder?.IsRoot == false);
            }
        }

        public DelegateCommand NewFolderCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        InputDialogText = "";
                        var ret = DialogService.ShowDialog(dialogButtons: MessageButton.OKCancel, title: "Enter Folder Name", viewModel: this);
                        if (ret != MessageResult.OK || InputDialogText == "")
                            return;

                        var folder = new DocumentFolder()
                        {
                            CustomerID = SelectedCustomer.ID,
                            FolderName = InputDialogText,
                            ParentID = 0
                        };
                        folder.SaveChanges();
                        DocumentFolders.Add(folder);
                        SelectedDocumentFolder = folder;
                    }
                    catch (Exception ex) { MessageBoxService.ShowMessage(messageBoxText: ex.Message, caption: "Error", button: MessageButton.OK, icon: MessageIcon.Error); }
                });
            }
        }

        public DelegateCommand NewSubFolderCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        InputDialogText = "";
                        var ret = DialogService.ShowDialog(dialogButtons: MessageButton.OKCancel, title: "Enter New SubFolder Name", viewModel: this);
                        if (ret != MessageResult.OK || InputDialogText == "")
                            return;

                        var folder = new DocumentFolder()
                        {
                            CustomerID = SelectedCustomer.ID,
                            FolderName = InputDialogText,
                            ParentID = SelectedDocumentFolder?.ID
                        };
                        folder.SaveChanges();
                        DocumentFolders.Add(folder);
                        SelectedDocumentFolder = folder;
                    }
                    catch (Exception ex) { MessageBoxService.ShowMessage(messageBoxText: ex.Message, caption: "Error", button: MessageButton.OK, icon: MessageIcon.Error); }
                }, () => SelectedDocumentFolder?.IsDefault == false && SelectedDocumentFolder?.IsRoot == false);
            }
        }

        public DelegateCommand DeleteFolderCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        if (MessageBoxService.ShowMessage("Are you sure you want to Delete this folder (All existing document(s) will move to Un-Categorized folder)", "Confirmation", MessageButton.OKCancel, MessageIcon.Exclamation) == MessageResult.Cancel)
                            return;
                        SelectedDocumentFolder.Delete();
                        DocumentFolders.Remove(SelectedDocumentFolder);
                    }
                    catch (Exception ex) { MessageBoxService.ShowMessage(messageBoxText: ex.Message, caption: "Error", button: MessageButton.OK, icon: MessageIcon.Error); }
                }, () => SelectedDocumentFolder?.IsDefault == false && SelectedDocumentFolder?.IsRoot == false);
            }
        }

        public DelegateCommand<KeyEventArgs> OnPreviewKeyDownCommand
        {
            get
            {
                return new DelegateCommand<KeyEventArgs>(args =>
                {
                    switch (args.Key)
                    {
                        case Key.F2:
                            RenameFolderCommand.Execute(null);
                            break;
                        case Key.Insert:
                            NewFolderCommand.Execute(null);
                            break;
                        case Key.Delete:
                            DeleteFolderCommand.Execute(null);
                            break;
                        case Key.Add:
                            NewSubFolderCommand.Execute(null);
                            break;
                    }
                }, (args) => SelectedDocumentFolder?.IsDefault == false && SelectedDocumentFolder?.IsRoot == false);
            }
        }

        public DelegateCommand<TreeListDragOverEventArgs> FolderDragCommand
        {
            get
            {
                return new DelegateCommand<TreeListDragOverEventArgs>(args =>
                {
                    try
                    {
                        if (args.TargetNode == null)
                            return;
                        var target = (DocumentFolder)args.TargetNode.Content;
                        if (target.IsDefault)
                            args.Manager.AllowDrop = false;
                        else if (SelectedDocumentFolder.IsRoot)
                            args.Manager.AllowDrop = false;
                        else if (SelectedDocumentFolder.IsDefault)
                            args.Manager.AllowDrop = false;
                        else
                            args.Manager.AllowDrop = true;
                    }
                    catch (Exception ex) { MessageBoxService.ShowMessage(messageBoxText: ex.Message, caption: "Error", button: MessageButton.OK, icon: MessageIcon.Error); }
                });
            }
        }

        public DelegateCommand<TreeListDropEventArgs> FolderDropCommand
        {
            get
            {
                return new DelegateCommand<TreeListDropEventArgs>(args =>
                {
                    try
                    {
                        var target = args.TargetNode.Content as DocumentFolder;
                        SelectedDocumentFolder.ParentID = target.ID;
                        SelectedDocumentFolder.SaveChanges();
                    }
                    catch (Exception ex) { MessageBoxService.ShowMessage(messageBoxText: ex.Message, caption: "Error", button: MessageButton.OK, icon: MessageIcon.Error); }
                });
            }
        }
        
        public DelegateCommand ImportFileCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        if (!OpenFileDialogService.ShowDialog())
                            return;
                        foreach (var file in OpenFileDialogService.Files)
                            ImportFileIntoFolder(file);
                        RefreshDocumentGrid();
                    }
                    catch (Exception ex) { MessageBoxService.ShowMessage(messageBoxText: ex.Message, caption: "Error", button: MessageButton.OK, icon: MessageIcon.Error); }
                }, () => SelectedDocumentFolder != null);
            }
        }

        public DelegateCommand SaveDocumentCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try { SelectedDocument.SaveChanges(); }
                    catch (Exception ex) { MessageBoxService.ShowMessage(messageBoxText: ex.Message, caption: "Error", button: MessageButton.OK, icon: MessageIcon.Error); }
                }, () => SelectedDocument != null && SelectedDocument.IsDirty);
            }
        }




        public CustomerDocumentsViewModel()
        {
            if (IsInDesignMode)
            {
                SelectedCustomer = new Customer();
                return;
            }

            DocumentTypes = LookupItem.GetLookupStrings(LookupItem.LookupTypesEnum.DocumentType);
            Messenger.Default.Register<Customer>(
                recipient: this,
                token: MessageTokenEnum.SelectedCustomerChanged,
                action: customer => SelectedCustomer = customer);
        }

        private void RefreshFolderTree()
        {
            if (_SelectedCustomer != null)
                DocumentFolders = DocumentFolder.GetCustomerDocumentFolders(_SelectedCustomer.ID, ShowHiddenFolders);
            else
                DocumentFolders = null;
        }

        private void RefreshDocumentGrid()
        {
            if (_SelectedCustomer != null && _SelectedDocumentFolder != null)
                Documents = Document.GetCustomerDocuments(_SelectedCustomer.ID, _SelectedDocumentFolder.ID);
            else
                Documents = null;
        }

        private void ImportFileIntoFolder(IFileInfo file)
        {
            Document doc = new Document
            {
                CustomerID = SelectedDocumentFolder.CustomerID,
                DocumentFolderID = SelectedDocumentFolder.ID,
                DocumentFileName = file.Name,
                FileTimestamp = File.GetLastWriteTime(file.GetFullName()),
                UploadDate = DateTime.Today
            };
            doc.SaveChanges();
            //TODO: Import BLOB
        }

    }
}
