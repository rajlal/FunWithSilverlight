using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AdvMediaPlayer
{
    public partial class pagePlayerProgress: UserControl
    {
        DispatcherTimer timer = new DispatcherTimer();

        public pagePlayerProgress()
        {
            InitializeComponent();
        }
        private void ShowMedia(object sender, MouseButtonEventArgs e)
        {
            UpdateMediaState();
            Media.Stop();
            btnPlaybig.Visibility = Visibility.Visible;
            Media.Visibility = Visibility.Collapsed;

        }

        private void btnPlay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PlayMedia();
        }
        private void PlayMedia()
        {
            UpdateMediaState();

            if (timer.IsEnabled) timer.Stop(); else timer.Start();

            timer.Tick +=
                           delegate(object s, EventArgs args)
                           {
                              // StatusPosition.Text = Media.Position.Hours.ToString() + ":" + Media.Position.Minutes.ToString() + ":" + Media.Position.Seconds.ToString() + " / " + Media.NaturalDuration.TimeSpan.Hours.ToString() + ":" + Media.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + Media.NaturalDuration.TimeSpan.Seconds.ToString();

                               StatusPosition.Text =  string.Format("{0:D2}", Media.Position.Minutes) + ":" +
                                                        string.Format("{0:D2}", Media.Position.Seconds) + " / " +
                                                        string.Format("{0:D2}", Media.NaturalDuration.TimeSpan.Minutes) + ":" +
                                                        string.Format("{0:D2}", Media.NaturalDuration.TimeSpan.Seconds);

                               // update progress bar
                               double current = Media.Position.TotalMilliseconds;
                               double total = Media.NaturalDuration.TimeSpan.TotalMilliseconds;
                               ProgressBar.Value = current / total * 240;
                               
                               Canvas.SetLeft(scrubBar, ProgressBar.Value);
                           };
            timer.Interval = new TimeSpan(0, 0, 0, 0,10); // ten millisecond
            timer.Start();

            Media.Play();
        }
        private void UpdateMediaState()
        {
            btnPlaybig.Visibility = Visibility.Collapsed;
            Media.Visibility = Visibility.Visible;


            timer.Stop();

         
        }
        private void btnPause_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateMediaState();
            Media.Pause();
        }
        private void btnStop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateMediaState();
            Media.Stop();


        }
        private void Media_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            StatusPosition.Text = string.Format("{0:D2}", Media.Position.Minutes) + ":" +
                                                       string.Format("{0:D2}", Media.Position.Seconds) + " / " +
                                                        string.Format("{0:D2}", Media.NaturalDuration.TimeSpan.Minutes) + ":" +
                                                       string.Format("{0:D2}", Media.NaturalDuration.TimeSpan.Seconds);
        }

        private void Media_BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PlayMedia();
        }

        private void ProgressBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double offset = e.GetPosition(ProgressBar).X;
            double barWidth = ProgressBar.ActualWidth;
            double ratio = offset / barWidth;

            TimeSpan totalTime = Media.NaturalDuration.TimeSpan;
            TimeSpan target = new TimeSpan(0, 0, (int)(totalTime.TotalSeconds * ratio));
            Media.Position = target;
        }

    }
}
