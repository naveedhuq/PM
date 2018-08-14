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
            ApplicationThemeHelper.ApplicationThemeName = PM.Properties.Settings.Default.ApplicationThemeName;
            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
