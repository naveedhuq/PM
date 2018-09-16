using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.DragDrop;
using PM.Model;
using PM.Shared;
using static PM.Model.Enumerators;

namespace PM.ViewModel
{
    public class CustomerDocumentsViewModel : ViewModelBase
    {
        [ServiceProperty(Key = "InputDialog")]
        protected virtual IDialogService DialogService { get { return GetService<IDialogService>(); } }
        protected virtual IMessageBoxService MessageBoxService { get { return GetService<IMessageBoxService>(); } }


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

        public DelegateCommand<TreeListDragOverEventArgs> DragCommand
        {
            get
            {
                return new DelegateCommand<TreeListDragOverEventArgs>(args =>
                {
                    var target = (DocumentFolder)args.TargetNode.Content;
                    if (target.IsDefault)
                        args.Manager.AllowDrop = false;
                    else if (SelectedDocumentFolder.IsRoot)
                        args.Manager.AllowDrop = false;
                    else if (SelectedDocumentFolder.IsDefault)
                        args.Manager.AllowDrop = false;
                    else
                        args.Manager.AllowDrop = true;
                    //else
                    //{
                    //    TreeListView view = args.Manager.TreeListView;
                    //    var draggedRow = view.GetNodeByRowHandle(args.HitInfo.RowHandle);
                    //}                    
                });
            }
        }

        public DelegateCommand<TreeListDropEventArgs> DropCommand
        {
            get
            {
                return new DelegateCommand<TreeListDropEventArgs>(args =>
                {
                    var target = args.TargetNode.Content as DocumentFolder;
                    SelectedDocumentFolder.ParentID = target.ID;
                    SelectedDocumentFolder.SaveChanges();
                });
            }
        }



        public CustomerDocumentsViewModel()
        {
            if (IsInDesignMode)
            {
                SelectedCustomer = new Customer();
                return;
            }

            Messenger.Default.Register<Customer>(
                recipient: this,
                token: MessageTokenEnum.SelectedCustomerChanged,
                action: customer => SelectedCustomer = customer);
        }

        private void RefreshFolderTree()
        {
            if (_SelectedCustomer != null)
                DocumentFolders = DocumentFolder.GetCustomerDocumentFolders(_SelectedCustomer.ID, ShowHiddenFolders);
        }

        private void RefreshDocumentGrid()
        {
            if (_SelectedCustomer != null && _SelectedDocumentFolder != null)
                Documents = Document.GetCustomerDocuments(_SelectedCustomer.ID, _SelectedDocumentFolder.ID);
        }

    }
}
