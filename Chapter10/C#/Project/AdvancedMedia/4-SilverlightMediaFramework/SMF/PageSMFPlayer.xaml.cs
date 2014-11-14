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
using Microsoft.SilverlightMediaFramework.Core.Logging;
using Microsoft.SilverlightMediaFramework.Plugins.Primitives;

namespace SMF
{
    public partial class PageSMFPlayer : UserControl
    {
        public PageSMFPlayer()
        {
            InitializeComponent();
         
        }
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            //Monitor the state of the content to determine the right action to take on this button being clicked
            //and then change the text to reflect the next action
            switch (smfMediaPlayer.PlayState)
            {
                case MediaPluginState.Playing:
                    smfMediaPlayer.Pause();
                    PlayButton.Content = "Play";
                    break;
                case MediaPluginState.Stopped:
                case MediaPluginState.Paused:
                    smfMediaPlayer.Play();
                    PlayButton.Content = "Pause";
                    break;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            //This should simply stop the playback
            smfMediaPlayer.Stop();
            //We should also reflect the chang on the play button
            PlayButton.Content = "Play";
        }

        private void PlayButton_Loaded(object sender, RoutedEventArgs e)
        {
            //We need to prepopulate the value of Play/Pause button content, we need to check AutoPlay
            switch (smfMediaPlayer.AutoPlay)
            {
                case false:
                    PlayButton.Content = "Play";
                    break;
                case true:
                    PlayButton.Content = "Pause";
                    break;
            }
        }
        
    }
}
