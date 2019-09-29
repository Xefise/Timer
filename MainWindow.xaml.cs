using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Forms;

namespace WPFtest
{
    public partial class MainWindow : Window
    {
        private NotifyIcon _notifyIcon;
        uint Time;
        uint LastEnteredNumber;
        uint PausedTime;
        bool IsStopped;
        public static bool UnitInSeconds = true;
        public static string Theme = "Dark";
        MediaPlayer player = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            UpdateTheme();

            if (!Properties.Settings.Default.UnitInSeconds) UnitInSeconds = false; else  UnitInSeconds = true;
        }

        public static void UpdateTheme()
        {
            if (!Properties.Settings.Default.DarkTheme) Theme = "Light"; else Theme = "Dark";
            ResourceDictionary resourceDict = System.Windows.Application.LoadComponent(new Uri($"{Theme}.xaml", UriKind.Relative)) as ResourceDictionary;
            System.Windows.Application.Current.Resources.Clear();
            System.Windows.Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }

        private async void Bt_Start_Click(object sender, RoutedEventArgs e)
        {
            IsStopped = false;
            ButtonsSwitch(true);
            try
            {
                LastEnteredNumber = Convert.ToUInt32(TBox_Time.Text);
                if (!UnitInSeconds) LastEnteredNumber *= 60;
                Time = LastEnteredNumber;
                await Task.Run(() => WTimer());
            }
            catch (Exception)
            {
                TBlock_TimeLeft.Text = "Enter positive number!";
                ButtonsSwitch(false);
            }
        }

        private void Bt_Stop_Click(object sender, RoutedEventArgs e)
        {
            IsStopped = true;
            Time = 0;
            Dispatcher.Invoke(new Action(() => TBlock_TimeLeft.Text = $"Time left: 00 : 00 : 00"));
            ButtonsSwitch(false);
        }

        private void Bt_Pause_Click(object sender, RoutedEventArgs e)
        {
            PausedTime = Time;
            IsStopped = true;
            Time = 0;
            PauseSwitch(true);
        }

        private async void Bt_Restart_Click(object sender, RoutedEventArgs e)
        {
            ButtonsSwitch(true);
            IsStopped = false;
            Time = LastEnteredNumber;
            await Task.Run(() => WTimer());
        }

        private async void Bt_Resume_Click(object sender, RoutedEventArgs e)
        {
            IsStopped = false;
            PauseSwitch(false);
            Time = PausedTime;
            await Task.Run(() => WTimer());
        }

        private void Bt_Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }

        private void WTimer()
        {
            long STime; byte h; byte m; byte s; int hh; byte mm; byte ss;
            while (Time != 0)
            {
                Time -= 1;
                STime = Time;
                hh = (byte)(STime / 36000); STime -= 36000 * hh;
                h = (byte)(STime / 3600); STime -= 3600 * h;
                mm = (byte)(STime / 600); STime -= 600 * mm;
                m = (byte)(STime / 60); STime -= 60 * m;
                ss = (byte)(STime / 10); STime -= 10 * ss;
                s = (byte)(STime);
                Dispatcher.Invoke(new Action(() => TBlock_TimeLeft.Text = $"Time left: {hh}{h} : {mm}{m} : {ss}{s}"));
                Thread.Sleep(1000);
            }

            if (IsStopped == false)
            {
                try
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        player.Open(new Uri($"{Environment.CurrentDirectory}\\Resources\\Bell.mp3"));
                        player.Volume = 1d;
                        player.Play();
                    }));
                    _notifyIcon = new NotifyIcon
                    {
                        BalloonTipText = @"Time is passed!",
                        Text = @"Timer",
                        Icon = new Icon("Resources//Icon.ico"),
                        Visible = true
                    };
                    _notifyIcon.ShowBalloonTip(1000);
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show("Error: Bell.mp3 not found!\n " + e);
                }
                finally
                {
                    ButtonsSwitch(false);
                }
            }
            IsStopped = false;
        }
        
        /// <param name="TimerIsWorking">Is the timer working?</param>
        private void ButtonsSwitch(bool TimerIsWorking)
        {
            if(TimerIsWorking)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    Bt_Start.IsEnabled = false;
                    Bt_Start.Visibility = Visibility.Hidden;
                    Bt_Pause.IsEnabled = true;
                    Bt_Pause.Visibility = Visibility.Visible;
                    Bt_Restart.IsEnabled = false;
                    Bt_Restart.Visibility = Visibility.Hidden;
                    Bt_Stop.IsEnabled = true;
                    Bt_Stop.Visibility = Visibility.Visible;
                    Bt_Resume.IsEnabled = false;
                    Bt_Resume.Visibility = Visibility.Hidden;
                }));
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    Bt_Start.IsEnabled = true;
                    Bt_Start.Visibility = Visibility.Visible;
                    Bt_Pause.IsEnabled = false;
                    Bt_Pause.Visibility = Visibility.Hidden;
                    Bt_Restart.IsEnabled = true;
                    Bt_Restart.Visibility = Visibility.Visible;
                    Bt_Stop.IsEnabled = false;
                    Bt_Stop.Visibility = Visibility.Hidden;
                    Bt_Resume.IsEnabled = false;
                    Bt_Resume.Visibility = Visibility.Hidden;
                }));
            }
        }

        /// <param name="IsEnabled">Is the pause enabled?</param>
        private void PauseSwitch(bool IsEnabled)
        {
            if(IsEnabled)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    Bt_Start.IsEnabled = false;
                    Bt_Start.Visibility = Visibility.Hidden;
                    Bt_Pause.IsEnabled = false;
                    Bt_Pause.Visibility = Visibility.Hidden;
                    Bt_Restart.IsEnabled = false;
                    Bt_Restart.Visibility = Visibility.Hidden;
                    Bt_Stop.IsEnabled = true;
                    Bt_Stop.Visibility = Visibility.Visible;
                    Bt_Resume.IsEnabled = true;
                    Bt_Resume.Visibility = Visibility.Visible;
                }));
            }
            else
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    Bt_Start.IsEnabled = false;
                    Bt_Start.Visibility = Visibility.Hidden;
                    Bt_Pause.IsEnabled = true;
                    Bt_Pause.Visibility = Visibility.Visible;
                    Bt_Restart.IsEnabled = false;
                    Bt_Restart.Visibility = Visibility.Hidden;
                    Bt_Stop.IsEnabled = true;
                    Bt_Stop.Visibility = Visibility.Visible;
                    Bt_Resume.IsEnabled = false;
                    Bt_Resume.Visibility = Visibility.Hidden;
                }));
            }
        }
    }
}
