using Unity;


namespace PM.ViewModel
{
    public class ViewModelLocator
    {
        private static IUnityContainer _container = new UnityContainer();

        public static MasterViewModel MasterViewModel { get { return _container.Resolve<MasterViewModel>(); } }
        public static CustomerEntryViewModel CustomerEntryViewModel { get { return _container.Resolve<CustomerEntryViewModel>(); } }
        public static CustomerSelectionPaneViewModel CustomerSelectionPaneViewModel { get { return _container.Resolve<CustomerSelectionPaneViewModel>(); } }
        public static CustomerDetailsViewModel CustomerDetailsViewModel { get { return _container.Resolve<CustomerDetailsViewModel>(); } }
        public static CustomerContactsViewModel CustomerContactsViewModel { get { return _container.Resolve<CustomerContactsViewModel>(); } }
        public static CustomerDocumentsViewModel CustomerDocumentsViewModel { get { return _container.Resolve<CustomerDocumentsViewModel>(); } }
        public static DocumentSearchViewModel DocumentSearchViewModel { get { return _container.Resolve<DocumentSearchViewModel>(); } }
        public static DocumentSearchSelectionPaneViewModel DocumentSearchSelectionPaneViewModel { get { return _container.Resolve<DocumentSearchSelectionPaneViewModel>(); } }
        public static ReminderCalendarViewModel ReminderCalendarViewModel { get { return _container.Resolve<ReminderCalendarViewModel>(); } }
        public static DocumentExpiryCalendarViewModel DocumentExpiryCalendarViewModel { get { return _container.Resolve<DocumentExpiryCalendarViewModel>(); } }
        public static WordDocumentViewModel WordDocumentViewModel { get { return _container.Resolve<WordDocumentViewModel>(); } }
        public static ExcelDocumentViewModel ExcelDocumentViewModel { get { return _container.Resolve<ExcelDocumentViewModel>(); } }
        public static RelatedPartyViewModel RelatedPartyViewModel { get { return _container.Resolve<RelatedPartyViewModel>(); } }


        static ViewModelLocator()
        {
            _container.RegisterType<MasterViewModel>();
            _container.RegisterType<CustomerEntryViewModel>();
            _container.RegisterType<CustomerSelectionPaneViewModel>();
            _container.RegisterType<CustomerDetailsViewModel>();
            _container.RegisterType<CustomerContactsViewModel>();
            _container.RegisterType<CustomerDocumentsViewModel>();
            _container.RegisterType<DocumentSearchViewModel>();
            _container.RegisterType<DocumentSearchSelectionPaneViewModel>();
            _container.RegisterType<ReminderCalendarViewModel>();
            _container.RegisterType<DocumentExpiryCalendarViewModel>();
            _container.RegisterType<WordDocumentViewModel>();
            _container.RegisterType<ExcelDocumentViewModel>();
            _container.RegisterType<RelatedPartyViewModel>();
        }
    }
}
