using System;
using System.Windows;
using System.Windows.Media;

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

            if (!CB_Theme.IsChecked ?? false) MainWindow.Theme = "Themes/Light"; else MainWindow.Theme = "Themes/Dark";
            ResourceDictionary resourceDict = Application.LoadComponent(new Uri($"{MainWindow.Theme}.xaml", UriKind.Relative)) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);

            MainWindow.UpdateTheme();
            MainWindow.UnitInSeconds = CB_UnitInSeconds.IsChecked ?? false;
        }
    }
}
