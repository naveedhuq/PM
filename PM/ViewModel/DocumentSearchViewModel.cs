using System;
using System.Linq;
using System.Collections.ObjectModel;
using DevExpress.Mvvm;
using PM.Model;
using static PM.Model.Enumerators;

namespace PM.ViewModel
{
    public class DocumentSearchViewModel : ViewModelBase
    {
        public ObservableCollection<DocumentFilter> FilteredDocuments
        {
            get { return GetProperty(() => FilteredDocuments); }
            set { SetProperty(() => FilteredDocuments, value); }
        }

        public DocumentFilter SelectedDocument
        {
            get { return GetProperty(() => SelectedDocument); }
            set { SetProperty(() => SelectedDocument, value); }
        }



        public DocumentSearchViewModel()
        {
            Messenger.Default.Register<ObservableCollection<DocumentFilter>>(
                recipient: this,
                token: MessageTokenEnum.ApplyFilterInvoked,
                action: documents => FilteredDocuments = documents);
        }
    }
}
