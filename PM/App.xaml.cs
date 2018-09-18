using DevExpress.Xpf.Core;
using System.Windows;


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
            DevExpress.Xpf.Grid.DataControlBase.AllowInfiniteGridSize = true;

            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
