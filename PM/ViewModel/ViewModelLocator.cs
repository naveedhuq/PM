using DevExpress.Mvvm;
using Unity;


namespace PM.ViewModel
{
    public class ViewModelLocator
    {
        private static IUnityContainer _container = new UnityContainer();

        public static MasterViewModel MasterViewModel { get { return _container.Resolve<MasterViewModel>(); } }
        public static CustomerEntryViewModel CustomerEntryViewModel { get { return _container.Resolve<CustomerEntryViewModel>(); } }
        


        static ViewModelLocator()
        {
            _container.RegisterType<MasterViewModel>();
            _container.RegisterType<CustomerEntryViewModel>();
        }
    }
}
