using DevExpress.Xpf.Core;
using System.Windows;


namespace PM.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ApplicationThemeHelper.ApplicationThemeName = UI.Properties.Settings.Default.ApplicationThemeName;
            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
