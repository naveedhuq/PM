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
        protected IMessageBoxService MessageBoxService { get { return GetService<IMessageBoxService>(); } }
        protected INotificationService AppNotificationService { get { return GetService<INotificationService>(); } }

        private readonly ISplashScreenService _WaitIndicatorService;


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
                        _WaitIndicatorService.ShowSplashScreen();
                        _logger.Debug($"Opening {viewName}");
                        var assembly = Assembly.GetExecutingAssembly();
                        var type = assembly.GetTypes().First(t => t.Name == viewName);
                        var view = (UIElement)Activator.CreateInstance(type);
                        ChildView = view;
                        _WaitIndicatorService.HideSplashScreen();
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
            _WaitIndicatorService = DevExpress.Mvvm.ServiceContainer.Default.GetService<ISplashScreenService>("WaitIndicatorService");
        }


        private async void ShowNotification(string message)
        {
            INotification notification = AppNotificationService.CreatePredefinedNotification(message, null, null, null);
            await notification.ShowAsync();
        }

    }
}
