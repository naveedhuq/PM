using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using DevExpress.Mvvm;
using PM.Model;
using PM.Shared;
using static PM.Model.Enumerators;

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

        public string ClipboardContent
        {
            get { return GetProperty(() => ClipboardContent ); }
            set { SetProperty(() => ClipboardContent, value); }
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
            Messenger.Default.Register<Document>(
                recipient: this,
                token: MessageTokenEnum.DocumentClipboardChanged,
                action: doc => ClipboardContent = doc?.DocumentFileName );
        }

    }
}
