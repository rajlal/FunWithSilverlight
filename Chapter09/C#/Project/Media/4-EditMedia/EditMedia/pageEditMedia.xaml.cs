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

namespace EditMedia
{
    public partial class pageEditMedia: UserControl
    {
        private Storyboard myStoryboard = new Storyboard();
        public pageEditMedia()
        {
            InitializeComponent();
        }
        private void CollapseAll()
        {
            mediaProgressive.Opacity = 0;
            mediaTraditional.Opacity = 0;
            mediaSmooth.Opacity = 0;
            Media.Opacity = 0;

            mediaProgressive.Stop();
            mediaTraditional.Stop();
            mediaSmooth.Stop();
            Media.Stop();

            canvasMedia.Visibility = Visibility.Collapsed;
            canvasProgressive.Visibility = Visibility.Collapsed;
            canvasTraditional.Visibility = Visibility.Collapsed;
            canvasSmooth.Visibility = Visibility.Collapsed;
        }
        private void ShowMedia(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            Media.Opacity = 1;

            canvasMedia.Visibility = Visibility.Visible;
            Media.Source = new Uri("Butterfly.wmv", UriKind.Relative);
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void ShowProgressive(object sender, MouseButtonEventArgs e)
        {
            
            CollapseAll();
            mediaProgressive.Opacity = 1;
            canvasProgressive.Visibility = Visibility.Visible;
            mediaProgressive.Source = new Uri("http://addrating.com/Silverlight/Media/Robotica_1080.wmv", UriKind.Absolute);
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
            
        }
        private void ShowTraditional(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            canvasTraditional.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void ShowSmooth(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            canvasSmooth.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
            mediaSmooth.Play();
        }
       
        private void canvasProgressive_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }
        private void canvasSmooth_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mediaSmooth.Play();
        }
        private void canvasTraditional_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
         }
        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
