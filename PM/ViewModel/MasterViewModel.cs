using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using DevExpress.Mvvm;
using PM.Shared;


namespace PM.ViewModel
{
    public class MasterViewModel : ViewModelBase
    {
        IMessageBoxService MessageBoxService { get { return GetService<IMessageBoxService>(); } }
        private ILogger _logger;


        public string UserName { get { return SharedUtils.Instance.UserName; } }

        public string AppDirectory { get { return SharedUtils.Instance.AppDirectory; } }

        public UIElement ChildView
        {
            get { return GetProperty(() => ChildView); }
            set { SetProperty(() => ChildView, value); }
        }

        



        public DelegateCommand<string> OpenViewCommand
        {
            get
            {
                return new DelegateCommand<string>(viewName =>
                {
                    try
                    {
                        _logger.Debug($"Opening {viewName}");
                        var assembly = Assembly.GetExecutingAssembly();
                        var type = assembly.GetTypes().First(t => t.Name == viewName);
                        var view = (UIElement)Activator.CreateInstance(type);

                        ChildView = view;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message, ex);
                        MessageBoxService.ShowMessage(ex.Message, "Error", MessageButton.OK, MessageIcon.Error);
                    }
                });
            }
        }

        public MasterViewModel()
        {
            _logger = LogManager.GetLogger(GetType());
        }

    }
}
