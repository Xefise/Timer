namespace WPFtest.ViewModels
{
    public class MainWindowViewModel
    {
        public string Background
        {
            get
            {
                if (Properties.Settings.Default.DarkTheme) return "#FF232323"; else return "#FFEEEEEE";
            }
        }
        public string Bt_Background
        {
            get
            {
                if (Properties.Settings.Default.DarkTheme) return "#FF323232"; else return "#FFFFFFFF";
            }
        }
        public string Bt_Foreground
        {
            get
            {
                if (!Properties.Settings.Default.DarkTheme) return "#FF323232"; else return "#FFFFFFFF";
            }
        }
    }
}
