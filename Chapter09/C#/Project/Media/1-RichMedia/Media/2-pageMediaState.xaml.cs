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

namespace Media
{
    public partial class pageMediaState : UserControl
    {
        DispatcherTimer timer = new DispatcherTimer();
        private bool MediaRewindState = false;
        private bool MediaFastForwardState = false;
   

        private Storyboard myStoryboard = new Storyboard();
        public pageMediaState()
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
                               StatusPosition.Text = Media.Position.Hours.ToString() + ":" + Media.Position.Minutes.ToString() + ":" + Media.Position.Seconds.ToString() + " / " + Media.NaturalDuration.TimeSpan.Hours.ToString() + ":" + Media.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + Media.NaturalDuration.TimeSpan.Seconds.ToString();
                           };
            timer.Interval = new TimeSpan(0, 0, 0, 1); // one second
            timer.Start();

            Media.Play();
        }
        private void UpdateMediaState()
        {
            btnPlaybig.Visibility = Visibility.Collapsed;
            Media.Visibility = Visibility.Visible;
   

            timer.Stop();

                if (MediaRewindState || MediaFastForwardState)
            {
                MediaRewindState = false;
                MediaFastForwardState = false;
                Media.Volume = 0.5;
                txtVolume.Text = String.Format("{0:0.0}", Media.Volume);
            }
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
        private void btnRewind_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (timer.IsEnabled) timer.Stop(); else timer.Start();

            Media.Volume = 0;
            txtVolume.Text = String.Format("{0:0.0}", Media.Volume);
            MediaRewindState = true;
            timer.Tick +=
                           delegate(object s, EventArgs args)
                           {
                               if (Media.CanSeek)
                               Media.Position = Media.Position.Subtract(new TimeSpan(0, 0, 0, 1, 0));
                               StatusPosition.Text = Media.Position.Hours.ToString() + ":" + Media.Position.Minutes.ToString() + ":" + Media.Position.Seconds.ToString() + " / " + Media.NaturalDuration.TimeSpan.Hours.ToString() + ":" + Media.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + Media.NaturalDuration.TimeSpan.Seconds.ToString();
                           };

            timer.Interval = new TimeSpan(0, 0, 0, 0, 200); // one second
            timer.Start();
        }

        private void btnFastForward_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           if (timer.IsEnabled) timer.Stop(); else timer.Start();

            Media.Volume = 0;
            txtVolume.Text = String.Format("{0:0.0}", Media.Volume);
            MediaRewindState = true;
            timer.Tick +=
                           delegate(object s, EventArgs args)
                           {
                               if (Media.CanSeek)
                                   Media.Position = Media.Position.Add(new TimeSpan(0, 0, 0, 1, 0));
                               StatusPosition.Text = Media.Position.Hours.ToString() + ":" + Media.Position.Minutes.ToString() + ":" + Media.Position.Seconds.ToString() + " / " + Media.NaturalDuration.TimeSpan.Hours.ToString() + ":" + Media.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + Media.NaturalDuration.TimeSpan.Seconds.ToString();
        
                           };

            timer.Interval = new TimeSpan(0,0,0,0,200); // one second
            timer.Start();
        }

        private void btnFullscreen_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MakeFullScreen();
        }

        private void btnVolumeDown_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Media.Volume = Media.Volume - 0.1;
            txtVolume.Text = String.Format("{0:0.0}", Media.Volume);
        }

        private void btnVolumeup_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Media.Volume = Media.Volume + 0.1;
            txtVolume.Text = String.Format("{0:0.0}", Media.Volume);
        }

        private void Media_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MakeFullScreen();
        }

        private void MakeFullScreen()
        {
            System.Windows.Interop.SilverlightHost host = Application.Current.Host;
            host.Content.FullScreenChanged += new EventHandler(Content_FullScreenChanged);
            host.Content.IsFullScreen = true;

        }

        void Content_FullScreenChanged(object sender, EventArgs e)
        {
            System.Windows.Interop.SilverlightHost host = Application.Current.Host;
            if (host.Content.IsFullScreen)
            {
                Media.Width = Application.Current.Host.Content.ActualWidth-80; // because canvasMedia holding the media has canvas.left property=40 so equal gap on both sides=80
                Media.Height = Application.Current.Host.Content.ActualHeight-60; // canvasMedia has the canvas.top property=30 so equal space in top and bottom
                borderMediastates.Visibility = Visibility.Collapsed;
                borderStatus.Visibility = Visibility.Collapsed; 
            }
            else
            {
                Media.Width = 320;
                Media.Height = 200;
                borderMediastates.Visibility = Visibility.Visible;
                borderStatus.Visibility = Visibility.Visible;
            }
        }

        private void Media_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            StatusBar.Text = Media.CurrentState.ToString();
            StatusPosition.Text = Media.Position.Hours.ToString() + ":" + Media.Position.Minutes.ToString() + ":" + Media.Position.Seconds.ToString() + " / " + Media.NaturalDuration.TimeSpan.Hours.ToString() + ":" + Media.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + Media.NaturalDuration.TimeSpan.Seconds.ToString();
                  
        }

        private void Media_BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            StatusBar.Text = Media.CurrentState.ToString();
            StatusPosition.Text = Media.Position.Hours.ToString() + ":" + Media.Position.Minutes.ToString() + ":" + Media.Position.Seconds.ToString() + " / " + Media.NaturalDuration.TimeSpan.Hours.ToString() + ":" + Media.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + Media.NaturalDuration.TimeSpan.Seconds.ToString();
         
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
                   PlayMedia();
        }
    }
}
