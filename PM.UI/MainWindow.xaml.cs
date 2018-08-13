using DevExpress.Xpf.Core;


namespace PM.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ThemedWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.ApplicationThemeName = ApplicationThemeHelper.ApplicationThemeName;
            Properties.Settings.Default.Save();
        }
    }
}
