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
using System.Windows.Media.Imaging;

namespace Animation
{
    public partial class PagePointAnimation : UserControl
    {
        private bool flagPaused;
        private string SelectedAnim = "Basic";
        private Storyboard myStoryboard = new Storyboard();

        public PagePointAnimation()
        {
            InitializeComponent();
        }

        private void btnPlay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GetStoryboard();
            myStoryboard.Stop();
            myStoryboard.Begin();

        }
        private void btnPause_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GetStoryboard();
            if (flagPaused)
            {
                myStoryboard.Resume();
                flagPaused = false;
            }
            else
            {
                myStoryboard.Pause();
                flagPaused = true;
            }

        }
        private void btnStop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GetStoryboard();
            myStoryboard.Stop();
        }




        private void ShowBasic(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasBasic.Visibility = Visibility.Visible;
            SelectedAnim = "Basic";
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void ShowEclipse(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasEclipse.Visibility = Visibility.Visible;
            SelectedAnim = "Eclipse";
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();

        }

        private void ShowSunrise(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasSunrise.Visibility = Visibility.Visible;
            SelectedAnim = "Sunrise";
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();

        }
        private void CollapseAll()
        {
            myStoryboard.Stop();
            CanvasBasic.Visibility = Visibility.Collapsed;
            CanvasEclipse.Visibility = Visibility.Collapsed;
            CanvasSunrise.Visibility = Visibility.Collapsed;
        }
        private Storyboard GetStoryboard()
        {
            if (SelectedAnim == "Basic")
            {
                myStoryboard = myBasicStoryboard;
            }
            else if (SelectedAnim == "Eclipse")
            {
                myStoryboard = myEclipseStoryboard;
            }
            else 
            {
                myStoryboard = mySunriseStoryboard;
            }
            
            return myStoryboard;
        }

       
      


    }
}
