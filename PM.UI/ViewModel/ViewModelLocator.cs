using DevExpress.Mvvm;
using Unity;
using PM.UI.Model;


namespace PM.UI.ViewModel
{
    public class ViewModelLocator
    {
        private static IUnityContainer _container = new UnityContainer();

        public static MasterViewModel MasterViewModel { get { return _container.Resolve<MasterViewModel>(); } }
        public static IMasterModel MasterModel { get { return _container.Resolve<IMasterModel>(); } }


        static ViewModelLocator()
        {
            // Registering ViewModels
            _container.RegisterType<MasterViewModel>();

            // Registering View DataModel (for Run and Design Time)
            if (!ViewModelBase.IsInDesignMode)
            {
                _container.RegisterType<IMasterModel, MasterModel>();
            }
            else
            {
                _container.RegisterType<IMasterModel, Design.MasterModel>();
            }

        }
    }
}
