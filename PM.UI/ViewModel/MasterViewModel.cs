using PM.UI.Model;

namespace PM.UI.ViewModel
{
    public class MasterViewModel : VMBase
    {
        private IMasterModel _model = ViewModelLocator.MasterModel;

        public string UserName { get { return _model.UserName; } }

        public string AppDirectory { get { return _model.AppDirectory; } }

        



        public MasterViewModel(IMasterModel model)
        {
            _model = model;
        }
        

    }
}
