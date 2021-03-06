﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Forms;

namespace Timer
{
    public partial class MainWindow : Window
    {
        private NotifyIcon _notifyIcon;
        ulong Time;
        ulong PausedTime; // This is time when timer stopped
        bool IsWorking = false; // The timer is working now?
        public static bool UnitInSeconds;
        public static string Theme;

        // For accuracy
        DateTime TimePassedAt = new DateTime();
        TimeSpan Span = new TimeSpan();

        MediaPlayer player = new MediaPlayer(); // For the alarm

        public MainWindow()
        {
            InitializeComponent();
            UpdateTheme();

            if (Properties.Settings.Default.UnitInSeconds) UnitInSeconds = true; else UnitInSeconds = false;
        }

        public static void UpdateTheme()
        {
            if (!Properties.Settings.Default.DarkTheme) Theme = "Themes/Light"; else Theme = "Themes/Dark";
            ResourceDictionary resourceDict = System.Windows.Application.LoadComponent(new Uri($"{Theme}.xaml", UriKind.Relative)) as ResourceDictionary;
            System.Windows.Application.Current.Resources.Clear();
            System.Windows.Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }

        private async void Bt_Start_Click(object sender, RoutedEventArgs e)
        {
            FreezeButtons();
            await Task.Run(() =>
            {
                Thread.Sleep(255);
                ButtonsSwitch(true);
            });
            try
            {
                Time = Convert.ToUInt32(TBox_Time.Text);
                if (!UnitInSeconds) Time *= 60;
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
            IsWorking = false;
            Time = 0;
            Dispatcher.Invoke(new Action(() => TBlock_TimeLeft.Text = $"Time left: 00 : 00 : 00"));
            Thread.Sleep(255);
            ButtonsSwitch(false);
        }

        private void Bt_Pause_Click(object sender, RoutedEventArgs e)
        {
            PausedTime = Time;
            IsWorking = false;
            Time = 0;
            Thread.Sleep(255);
            PauseSwitch(true);
        }

        private async void Bt_Resume_Click(object sender, RoutedEventArgs e)
        {
            FreezeButtons();
            await Task.Run(() =>
            {
                Thread.Sleep(255);
                PauseSwitch(false);
            });
            IsWorking = true;
            Time = PausedTime;
            await Task.Run(() => WTimer());
        }

        private void Bt_Settings_Click(object sender, RoutedEventArgs e) => _ = new SettingsWindow();

        private void WTimer()
        {
            TimePassedAt = DateTime.Now.AddSeconds(Convert.ToDouble(Time));
            IsWorking = true;
            ulong Working = 0;
            ulong STime; byte h; byte m; byte s; int hh; byte mm; byte ss;
            while (Time != 0)
            {
                Time -= 1;
                ++Working;
                STime = Time;
                hh = (byte)(STime / 36000); STime -= Convert.ToUInt64(36000 * hh);
                h = (byte)(STime / 3600); STime -= Convert.ToUInt64(3600 * h);
                mm = (byte)(STime / 600); STime -= Convert.ToUInt64(600 * mm);
                m = (byte)(STime / 60); STime -= Convert.ToUInt64(60 * m);
                ss = (byte)(STime / 10); STime -= Convert.ToUInt64(10 * ss);
                s = (byte)(STime);
                Dispatcher.Invoke(new Action(() => TBlock_TimeLeft.Text = $"Time left: {hh}{h} : {mm}{m} : {ss}{s}"));
                Thread.Sleep(250);
                if (!IsWorking) break;
                Thread.Sleep(250);
                if (!IsWorking) break;
                Thread.Sleep(250);
                if (!IsWorking) break;
                Thread.Sleep(250); // Немного говнокода))
                if (Working > 200)
                {
                    Working = 0;
                    Span = TimePassedAt - DateTime.Now;
                    Time = Convert.ToUInt64(Span.TotalSeconds);
                }
            }

            if (IsWorking)
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
            IsWorking = false;
            Dispatcher.Invoke(new Action(() => TBlock_TimeLeft.Text = $"Time left: 00 : 00 : 00"));
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
                    Bt_Stop.IsEnabled = false;
                    Bt_Stop.Visibility = Visibility.Hidden;
                    Bt_Resume.IsEnabled = false;
                    Bt_Resume.Visibility = Visibility.Hidden;
                }));
            }
        }

        /// <param>Turn off all buttons</param>
        private void FreezeButtons()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                Bt_Start.IsEnabled = false;
                Bt_Start.Visibility = Visibility.Hidden;
                Bt_Pause.IsEnabled = false;
                Bt_Pause.Visibility = Visibility.Hidden;
                Bt_Stop.IsEnabled = false;
                Bt_Stop.Visibility = Visibility.Hidden;
                Bt_Resume.IsEnabled = false;
                Bt_Resume.Visibility = Visibility.Hidden;
            }));
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
                    Bt_Stop.IsEnabled = true;
                    Bt_Stop.Visibility = Visibility.Visible;
                    Bt_Resume.IsEnabled = false;
                    Bt_Resume.Visibility = Visibility.Hidden;
                }));
            }
        }
    }
}
