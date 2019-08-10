namespace WPFtest.ViewModels
{
    public class SettingsWindowViewModel
    {
        public string Background
        {
            get
            {
                if (Properties.Settings.Default.DarkTheme) return "#FF232323"; else return "#FFEEEEEE";
            }
        }
        public string BtCB_Background
        {
            get
            {
                if (Properties.Settings.Default.DarkTheme) return "#FF323232"; else return "#FFFFFFFF";
            }
        }
        public string BtCB_Foreground
        {
            get
            {
                if (!Properties.Settings.Default.DarkTheme) return "#FF323232"; else return "#FFFFFFFF";
            }
        }
    }
}
