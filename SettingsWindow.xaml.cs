using System.Windows;
using System.Windows.Media;
using WPFtest.ViewModels;

namespace WPFtest
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            this.DataContext = new SettingsWindowViewModel();

            CB_Theme.IsChecked = Properties.Settings.Default.DarkTheme;
            CB_UnitInSeconds.IsChecked = Properties.Settings.Default.UnitInSeconds;
        }

        private void Bt_Save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DarkTheme = CB_Theme.IsChecked ?? false;
            Properties.Settings.Default.UnitInSeconds = CB_UnitInSeconds.IsChecked ?? false;

            Properties.Settings.Default.Save();
            MessageBox.Show("Restart the program to apply the settings.");
        }
    }
}
