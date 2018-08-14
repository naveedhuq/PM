using System;
using DevExpress.Mvvm;
using log4net;
using PM.Backend.Shared;
using PM.UI.Model;


namespace PM.UI.ViewModel
{
    public class MasterViewModel : VMBase
    {
        readonly ILog _logger = Logger.GetLogger();

        private IMasterModel _model = ViewModelLocator.MasterModel;
        IMessageBoxService MessageBoxService { get { return GetService<IMessageBoxService>(); } }


        public string UserName { get { return _model.UserName; } }

        public string AppDirectory { get { return _model.AppDirectory; } }

        public DelegateCommand<string> OpenViewCommand
        {
            get
            {
                return new DelegateCommand<string>(viewName =>
                {
                    try
                    {
                        _logger.Info($"Opening {viewName}");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message, ex);
                        MessageBoxService.ShowMessage(ex.Message, "Error", MessageButton.OK, MessageIcon.Error);
                    }
                });
            }
        }



        public MasterViewModel(IMasterModel model)
        {
            _model = model;
        }
        

    }
}
