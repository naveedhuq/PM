using System;
using System.Windows;
using DevExpress.Xpf.Core;

namespace PM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Shared.EventLog.AddEventLog(Shared.EventLog.LogEventType.Login, "User started the application");
                ApplicationThemeHelper.ApplicationThemeName = PM.Properties.Settings.Default.ApplicationThemeName;
                DevExpress.Xpf.Grid.DataControlBase.AllowInfiniteGridSize = true;

                MainWindow window = new MainWindow();
                window.Show();
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
