﻿using System;
using System.Collections.ObjectModel;
using System.Drawing;
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
        protected INotificationService AppNotificationService { get { return GetService<INotificationService>(); ; } }


        private Document _ClipboardCopyDocument = null;
        private Document _ClipboardCutDocument = null;


        public Customer SelectedCustomer
        {
            get { return GetProperty(() => SelectedCustomer); }
            set
            {
                SetProperty(() => SelectedCustomer, value);
                RefreshFolderTree();
                SelectedDocumentFolder = null;
                RefreshDocumentGrid();
            }
        }

        public ObservableCollection<DocumentFolder> DocumentFolders
        {
            get { return GetProperty(() => DocumentFolders); }
            set { SetProperty(() => DocumentFolders, value); }
        }

        public DocumentFolder SelectedDocumentFolder
        {
            get { return GetProperty(() => SelectedDocumentFolder); }
            set
            {
                SetProperty(() => SelectedDocumentFolder, value);
                RefreshDocumentGrid();
            }
        }

        public ObservableCollection<Document> Documents
        {
            get { return GetProperty(() => Documents); }
            set { SetProperty(() => Documents, value); }
        }

        public Document SelectedDocument
        {
            get { return GetProperty(() => SelectedDocument); }
            set
            {
                SetProperty(() => SelectedDocument, value);
                IsDocumentSelectionValid = (value != null);
            }
        }

        public string InputDialogText
        {
            get { return GetProperty(() => InputDialogText); }
            set { SetProperty(() => InputDialogText, value); }
        }

        public bool ShowHiddenFolders
        {
            get { return GetProperty(() => ShowHiddenFolders); }
            set
            {
                SetProperty(() => ShowHiddenFolders, value);
                RefreshFolderTree();
            }
        }

        public bool IsDocumentSelectionValid
        {
            get { return GetProperty(() => IsDocumentSelectionValid); }
            set { SetProperty(() => IsDocumentSelectionValid, value); }
        }

        public ObservableCollection<string> DocumentTypes
        {
            get { return GetProperty(() => DocumentTypes); }
            set { SetProperty(() => DocumentTypes, value); }
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
                        case Key.V:
                            if (Keyboard.Modifiers == ModifierKeys.Control)
                                PasteDocumentCommand.Execute(null);
                            break;
                    }
                }, (args) => SelectedDocumentFolder?.IsDefault == false && SelectedDocumentFolder?.IsRoot == false);
            }
        }

        public DelegateCommand<KeyEventArgs> OnDocumentGrid_PreviewKeyDownCommand
        {
            get
            {
                return new DelegateCommand<KeyEventArgs>(args =>
                {
                    bool ctrlKey = Keyboard.Modifiers == ModifierKeys.Control;
                    if (args.Key == Key.C && ctrlKey)
                        CopyDocumentCommand.Execute(null);
                    else if (args.Key == Key.X && ctrlKey)
                        CutDocumentCommand.Execute(null);
                });
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
                        var target = args.TargetNode.Content as DocumentFolder;
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

        public DelegateCommand CopyDocumentCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ClearClipboard();
                    _ClipboardCopyDocument = SelectedDocument;
                    ShowNotification($"File [{_ClipboardCopyDocument.DocumentFileName}] copied to clipboard.");
                    Messenger.Default.Send(message: SelectedDocument, token: MessageTokenEnum.DocumentClipboardChanged);
                }, () => SelectedDocument != null);
            }
        }

        public DelegateCommand CutDocumentCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    ClearClipboard();
                    _ClipboardCutDocument = SelectedDocument;
                    ShowNotification($"File [{_ClipboardCutDocument.DocumentFileName}] cut to clipboard.");
                    Messenger.Default.Send(message: SelectedDocument, token: MessageTokenEnum.DocumentClipboardChanged);
                }, () => SelectedDocument != null);
            }
        }

        public DelegateCommand PasteDocumentCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        if (_ClipboardCopyDocument != null)
                        {
                            var newdoc = new Document()
                            {
                                CustomerID = SelectedCustomer.ID,                                
                                DocumentFileName = _ClipboardCopyDocument.DocumentFileName,
                                DocumentType = _ClipboardCopyDocument.DocumentType,
                                UploadDate = DateTime.Today,
                                FileTimestamp = _ClipboardCopyDocument.FileTimestamp,
                                ExpirationDate = _ClipboardCopyDocument.ExpirationDate,
                                Comments = _ClipboardCopyDocument.Comments
                            };
                            if (!SelectedDocumentFolder.IsDefault)
                                newdoc.DocumentFolderID = SelectedDocumentFolder.ID;
                            newdoc.SaveChanges();
                            // TODO: Import BLOB
                        }
                        else if (_ClipboardCutDocument != null)
                        {
                            _ClipboardCutDocument.CustomerID = SelectedCustomer.ID;
                            if (!SelectedDocumentFolder.IsDefault)
                                _ClipboardCutDocument.DocumentFolderID = SelectedDocumentFolder.ID;
                            else
                                _ClipboardCutDocument.DocumentFolderID = null;
                            _ClipboardCutDocument.SaveChanges();
                        }
                        ClearClipboard();
                        RefreshDocumentGrid();
                    }
                    catch (Exception ex) { MessageBoxService.ShowMessage(messageBoxText: ex.Message, caption: "Error", button: MessageButton.OK, icon: MessageIcon.Error); }

                }, () => SelectedCustomer != null && SelectedDocumentFolder != null && !IsClipboardEmpty() && SelectedDocumentFolder.ID != 0);
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
            if (SelectedCustomer != null)
                DocumentFolders = DocumentFolder.GetCustomerDocumentFolders(SelectedCustomer.ID, ShowHiddenFolders);
            else
                DocumentFolders = null;
        }

        private void RefreshDocumentGrid()
        {
            if (SelectedCustomer != null && SelectedDocumentFolder != null)
                Documents = Document.GetCustomerDocuments(SelectedCustomer.ID, SelectedDocumentFolder.ID);
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

        private void ShowNotification(string text)
        {
            INotification n = AppNotificationService.CreatePredefinedNotification(text, null, null);
            n.ShowAsync();
        }
        private void ClearClipboard()
        {
            _ClipboardCopyDocument = null;
            _ClipboardCutDocument = null;
            Messenger.Default.Send<Document>(message: null, token: MessageTokenEnum.DocumentClipboardChanged);
        }

        private bool IsClipboardEmpty()
        {
            return (_ClipboardCopyDocument == null && _ClipboardCutDocument == null);
        }

    }
}
