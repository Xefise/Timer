using System;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Media;


namespace WPFtest
{
    public partial class MainWindow : Window
    {
        uint Time;
        uint LastEnteredNumber;
        uint PausedTime;
        bool IsStopped;
        MediaPlayer player = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            if(!Properties.Settings.Default.DarkTheme)
            {
                Windoww.Background = Brushes.White;
                TBlock_TimeLeft.Foreground = Brushes.Black;
                TBlock_EnterTime.Foreground = Brushes.Black;
                TBox_Time.Background = Brushes.LightGray;
                TBox_Time.Foreground = Brushes.Black;
                Bt_Start.Background = Brushes.White;
                Bt_Start.Foreground = Brushes.Black;
                Bt_Pause.BorderBrush = Brushes.Gray;
                Bt_Pause.Background = Brushes.White;
                Bt_Pause.Foreground = Brushes.Black;
                Bt_Pause.BorderBrush = Brushes.Gray;
                Bt_Restart.Background = Brushes.White;
                Bt_Restart.Foreground = Brushes.Black;
                Bt_Restart.BorderBrush = Brushes.Gray;
                Bt_Stop.Background = Brushes.White;
                Bt_Stop.Foreground = Brushes.Black;
                Bt_Stop.BorderBrush = Brushes.Gray;
                Bt_Resume.Background = Brushes.White;
                Bt_Resume.Foreground = Brushes.Black;
                Bt_Resume.BorderBrush = Brushes.Gray;
                Bt_Settings.Background = Brushes.White;
                Bt_Settings.Foreground = Brushes.Black;
                Bt_Settings.BorderBrush = Brushes.Gray;
            }

            if(!Properties.Settings.Default.UnitInSeconds)
            {
                TBlock_EnterTime.Text = "Time in mins:";
            }
        }

        private async void Bt_Start_Click(object sender, RoutedEventArgs e)
        {
            IsStopped = false;
            ButtonsSwitch(true);
            try
            {
                LastEnteredNumber = Convert.ToUInt32(TBox_Time.Text);
                Time = LastEnteredNumber;
                await Task.Run(() => WTimer());
            }
            catch (Exception)
            {
                TBlock_TimeLeft.Foreground = Brushes.Red;
                TBlock_TimeLeft.Text = "Enter positive number!";
                ButtonsSwitch(false);
            }
        }

        private void Bt_Stop_Click(object sender, RoutedEventArgs e)
        {
            IsStopped = true;
            Time = 0;
            Dispatcher.Invoke(new Action(() => TBlock_TimeLeft.Text = $"Time Left: 0 : 0 : 0"));
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
            Dispatcher.Invoke(new Action(() => TBlock_TimeLeft.Foreground = Brushes.White));
            long STime; int h; int m; int s;
            while (Time != 0)
            {
                Time -= 1;
                STime = Time;
                h = (int)(STime / 3600); STime -= 3600 * h;
                m = (int)(STime / 60); STime -= 60 * m;
                s = (int)(STime);
                Dispatcher.Invoke(new Action(() => TBlock_TimeLeft.Text = $"Time Left: {h} : {m} : {s}"));
                Thread.Sleep(1000);
            }

            Dispatcher.Invoke(new Action(() => TBlock_TimeLeft.Foreground = Brushes.Orange));
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
                }
                catch (Exception)
                {
                    MessageBox.Show("Error: Bell.mp3 not found!");
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
