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
    public partial class PageKeyframeAnimation : UserControl
    {
        private bool flagPaused;
        private string SelectedAnim = "Linear";
        private Storyboard myStoryboard = new Storyboard();
        public PageKeyframeAnimation()
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

       

        private void showDiscrete(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasDiscrete.Visibility = Visibility.Visible;
            SelectedAnim = "Discrete";
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }

        private void showLinear(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasLinear.Visibility = Visibility.Visible;
            SelectedAnim = "Linear";
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }

        private void showSplined(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasSplined.Visibility = Visibility.Visible;
            SelectedAnim = "Splined";
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
     
        private void CollapseAll()
        {
            myStoryboard.Stop();
            CanvasDiscrete.Visibility = Visibility.Collapsed;
            CanvasLinear.Visibility = Visibility.Collapsed;
            CanvasSplined.Visibility = Visibility.Collapsed;
        }
        private Storyboard GetStoryboard()
        {
            if (SelectedAnim == "Linear")
            {
                myStoryboard = myLinearStoryboard;
            }
            else if (SelectedAnim == "Discrete")
            {
                myStoryboard = myDiscreteStoryboard;
            }
            else
            {
                myStoryboard = mySplinedStoryboard;
            }
            return myStoryboard;
        }

        

      


    }
}
