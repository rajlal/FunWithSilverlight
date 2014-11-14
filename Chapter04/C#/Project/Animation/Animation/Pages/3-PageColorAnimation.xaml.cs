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
    public partial class PageColorAnimation : UserControl
    {
        private bool flagPaused;
        private string SelectedAnim = "Basic";
        private Storyboard myStoryboard = new Storyboard();

        public PageColorAnimation()
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
            StatusInfo.Text = "Animation TargetProperty: Color";
        }
        private void ShowGradient(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasGradient.Visibility = Visibility.Visible;
            SelectedAnim = "Gradient";
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
            StatusInfo.Text = "Animation Target: GradientStop";
        }
        private void ShowRainbow(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasRainbow.Visibility = Visibility.Visible;
            SelectedAnim = "Rainbow";
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
            StatusInfo.Text = "Animation TargetProperty: Ellipse.Fill";
        }


        private void CollapseAll()
        {
            myStoryboard.Stop();
            CanvasBasic.Visibility = Visibility.Collapsed;
            CanvasGradient.Visibility = Visibility.Collapsed;
            CanvasRainbow.Visibility = Visibility.Collapsed;
        }
        private Storyboard GetStoryboard()
        {
            if (SelectedAnim == "Basic")
            {
                myStoryboard = myBasicStoryboard;
            }
            else if (SelectedAnim == "Gradient")
            {
                myStoryboard = myGradientStoryboard;
            }
            else
            {
                myStoryboard = myRainbowStoryboard;
            }

            return myStoryboard;
        }      
        private void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StatusBar.Text = ToolTipService.GetToolTip((Ellipse)sender).ToString();
        }

       
    }
}
