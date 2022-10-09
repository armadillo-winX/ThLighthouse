global using System;
global using System.IO;

global using ThLighthouse.Handler;

using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ThLighthouse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AboutDialog? _aboutDialog;

        private readonly string? _appName = VersionInfo.AppName;

        public MainWindow()
        {
            InitializeComponent();

            this.Title = _appName;

            try
            {
                SettingsSerializer.ConfigureSettings();
            }
            catch (Exception)
            {
                Environment.Exit(-1);
            }

            ThtkPathBox.Text = Settings.ThtkDirectory;
        }

        private string GetSelectedGameId()
        {
            ComboBoxItem item = (ComboBoxItem)GameComboBox.SelectedItem;
            return item.Uid.ToString();
        }

        private void EnableLimitationMode(bool option)
        {
            ArchiveBrowseButton.IsEnabled = !option;
            ArchiveFilePathBox.IsEnabled = !option;
            OutputDirectoryBrowseButton.IsEnabled = !option;
            OutputPathBox.IsEnabled = !option;
            ExtractButton.IsEnabled = !option;

            ThtkBrowseButton.IsEnabled = !option;
            ThtkPathBox.IsEnabled = !option;
        }

        private void ThtkBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new();
            folderBrowserDialog.Description = "thtkがインストールされているフォルダを選択してください";
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath;
                ThtkPathBox.Text = path;
            }
        }

        private void ThtkPathBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Settings.ThtkDirectory = ThtkPathBox.Text;
        }

        private void ArchiveBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.FileName = "*.dat";
            openFileDialog.Filter = "アーカイブデータファイル|*.dat|すべてのファイル|*.*";
            openFileDialog.Title = "アーカイブファイルを選択してください";

            if (openFileDialog.ShowDialog(this) == true)
            {
                string path = openFileDialog.FileName;
                ArchiveFilePathBox.Text = path;
            }
        }

        private void OutputDirectoryBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new();
            folderBrowserDialog.Description = "アーカイブの展開先フォルダを選択してください";
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath;
                OutputPathBox.Text = path;
            }
        }

        private async void ExtractButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GameComboBox.SelectedIndex > -1 &&
                ArchiveFilePathBox.Text.Length > 0 &&
                OutputPathBox.Text.Length > 0)
                {
                    string gameId = GetSelectedGameId();
                    string archivePath = ArchiveFilePathBox.Text;
                    string outputPath = OutputPathBox.Text;

                    EnableLimitationMode(true);
                    string output = await Task.Run(() => ThdatHandler.Extract(gameId, archivePath, outputPath));
                    EnableLimitationMode(false);

                    OutputBox.Text = output;
                    MessageBox.Show(this, "展開が完了しました。", "処理の完了",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (GameComboBox.SelectedIndex == -1)
                {
                    MessageBox.Show(this, "ゲームを選択してください。", "エラー",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (ArchiveFilePathBox.Text.Length == 0 || OutputPathBox.Text.Length == 0)
                {
                    MessageBox.Show(this, "アーカイブファイルのパス、あるいは展開先ディレクトリが指定されていません。", "エラー",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "エラー",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                SettingsSerializer.SaveSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "エラー",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AboutHelpMenu_Click(object sender, RoutedEventArgs e)
        {
            if (_aboutDialog == null || !_aboutDialog.IsLoaded)
            {
                _aboutDialog = new();
                _aboutDialog.Owner = this;
                _aboutDialog.Show();
            }
            else
            {
                _aboutDialog.WindowState = WindowState.Normal;
                _ = _aboutDialog.Activate();
            }
        }
    }
}
