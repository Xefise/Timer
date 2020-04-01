using System.Windows;

namespace Timer
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();

            CB_Theme.IsChecked = Properties.Settings.Default.DarkTheme;
            CB_UnitInSeconds.IsChecked = Properties.Settings.Default.UnitInSeconds;
        }

        private void Bt_Save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DarkTheme = CB_Theme.IsChecked ?? false;
            Properties.Settings.Default.UnitInSeconds = CB_UnitInSeconds.IsChecked ?? false;
            Properties.Settings.Default.Save();

            MainWindow.UpdateTheme();
            MainWindow.UnitInSeconds = CB_UnitInSeconds.IsChecked ?? false;
        }
    }
}
