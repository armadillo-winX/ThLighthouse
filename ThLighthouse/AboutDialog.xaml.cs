using System.Windows;

namespace ThLighthouse
{
    /// <summary>
    /// AboutDialog.xaml の相互作用ロジック
    /// </summary>
    public partial class AboutDialog : Window
    {
        public AboutDialog()
        {
            InitializeComponent();

            AppNameBlock.Text = VersionInfo.AppName;
            AppVersionBlock.Text = $"Version {VersionInfo.AppVersion}";
            DeveloperBlock.Text = $"Developer {VersionInfo.Developer}";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Activate();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
