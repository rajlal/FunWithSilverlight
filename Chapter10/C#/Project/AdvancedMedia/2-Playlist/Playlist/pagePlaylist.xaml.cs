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

namespace Playlist
{
    public partial class pagePlaylist: UserControl
    {
        private Storyboard myStoryboard = new Storyboard();
        public pagePlaylist()
        {
            InitializeComponent();
        }
        private void CollapseAll()
        {
            mediaServerPlaylist.Opacity = 0;
            mediaWebPlaylist.Opacity = 0;
            Media.Opacity = 0;

            mediaServerPlaylist.Stop();
            mediaWebPlaylist.Stop();
            Media.Stop();

            canvasMedia.Visibility = Visibility.Collapsed;
            canvasServer.Visibility = Visibility.Collapsed;
            canvasWeb .Visibility = Visibility.Collapsed;
        }
        private void ShowMedia(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            Media.Opacity = 1;

            canvasMedia.Visibility = Visibility.Visible;
            Media.Source = new Uri("Butterfly.wmv",UriKind.Relative) ;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void ShowServerPlaylist(object sender, MouseButtonEventArgs e)
        {
            
            CollapseAll();
            mediaServerPlaylist.Opacity = 1;
            canvasServer.Visibility = Visibility.Visible;
            mediaServerPlaylist.Source = new Uri("http://addrating.com:100/SilverlightSSPL", UriKind.Absolute);
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void ShowWebPlaylist(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            canvasWeb.Visibility = Visibility.Visible;
            mediaWebPlaylist.Opacity = 1;
            mediaWebPlaylist.Source = new Uri("http://addrating.com:80/ServerPlaylist.isx", UriKind.Absolute);
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
    }
}
     //mediaWebPlaylist.Source = new Uri("http://addrating.com:100/Sample_Broadcast", UriKind.Absolute);
       