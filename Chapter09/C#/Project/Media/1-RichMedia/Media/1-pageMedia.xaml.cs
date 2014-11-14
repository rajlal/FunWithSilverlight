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

namespace Media
{
    public partial class pageMedia : UserControl
    {
        private Storyboard myStoryboard = new Storyboard();
        public pageMedia()
        {
            InitializeComponent();
        }
        private void CollapseAll()
        {
            mediaFadein.Opacity = 0;
            mediaFadein.Stop();
            storyboardFadein.Stop();

            mediaSlidein.Width = 0;
            mediaSlidein.Stop();
            storyboardSlidein.Stop(); 
            
            mediaRotate.Stop();
            storyboardRotate.Stop();
            Media.Stop();
            canvasMedia.Visibility = Visibility.Collapsed;
            canvasFadein.Visibility = Visibility.Collapsed;
            canvasSlidein.Visibility = Visibility.Collapsed;
            canvasRotate.Visibility = Visibility.Collapsed;
        }
        private void ShowMedia(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            canvasMedia.Visibility = Visibility.Visible;
           Media.Source = new Uri("Butterfly.wmv", UriKind.Relative);
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void ShowFadein(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            canvasFadein.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
            myStoryboard = storyboardFadein;
            myStoryboard.Begin(); 
        }
        private void ShowSlidein(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            canvasSlidein.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
            myStoryboard = storyboardSlidein;
            myStoryboard.Begin();
        }
        private void ShowRotate(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            canvasRotate.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
            storyboardRotate.Begin();
            mediaRotate.Play();
        }
       
        private void storyboardFadein_Completed(object sender, EventArgs e)
        {
            mediaFadein.Play();
        }
        private void canvasFadein_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            myStoryboard = storyboardFadein;
            myStoryboard.Begin(); 
        }
        private void storyboardSlidein_Completed(object sender, EventArgs e)
        {
            mediaSlidein.Play();
        }
        private void canvasRotate_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            storyboardRotate.Begin();
            mediaRotate.Play();
        }
        private void canvasSlidein_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            myStoryboard = storyboardSlidein;
            myStoryboard.Begin();
         }
        private void storyboardRotate_Completed(object sender, EventArgs e)
        {
            mediaRotate.Play();
        }
    }
}
