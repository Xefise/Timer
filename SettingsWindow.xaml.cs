using System.Windows;
using System.Windows.Media;

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
            CB_Theme.IsChecked = Properties.Settings.Default.DarkTheme;
            CB_UnitInSeconds.IsChecked = Properties.Settings.Default.UnitInSeconds;
            if(!Properties.Settings.Default.DarkTheme)
            {
                Windoww.Background = Brushes.White;
                CB_Theme.Foreground = Brushes.Black;
                CB_UnitInSeconds.Foreground = Brushes.Black;
                Bt_Save.Background = Brushes.White;
                Bt_Save.Foreground = Brushes.Black;
            }
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
