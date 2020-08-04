using System.Windows;

namespace Timer
{
    public partial class SettingsWindow : Window
    {
        static Window instance; // Singleton

        public SettingsWindow()
        {
            if (instance != null)
            {
                instance.Focus();
            }
            else
            {
                this.Closing += RemoveInstance;

                instance = this;
                InitializeComponent();
                this.Show();

                CB_Theme.IsChecked = Properties.Settings.Default.DarkTheme;
                CB_UnitInSeconds.IsChecked = Properties.Settings.Default.UnitInSeconds;
            }
        }

        private void Bt_Save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.DarkTheme = CB_Theme.IsChecked ?? false;
            Properties.Settings.Default.UnitInSeconds = CB_UnitInSeconds.IsChecked ?? false;
            Properties.Settings.Default.Save();

            MainWindow.UpdateTheme();
            MainWindow.UnitInSeconds = CB_UnitInSeconds.IsChecked ?? false;
        }

        void RemoveInstance(object sender, System.ComponentModel.CancelEventArgs e) => instance = null; 
    }
}
