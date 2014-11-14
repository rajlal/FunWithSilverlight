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
    public partial class PageLayout : UserControl
    {
        private bool flagPaused;
        private string SelectedAnim = "Border";
        private Storyboard myStoryboard = new Storyboard();
        public PageLayout()
        {
            InitializeComponent();
        }


       

        private void showMargin(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasMargin.Visibility = Visibility.Visible;
            SelectedAnim = "Margin";
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }

        private void showBorder(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasBorder.Visibility = Visibility.Visible;
            SelectedAnim = "Border";
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }

        private void showPadding(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasPadding.Visibility = Visibility.Visible;
            SelectedAnim = "Padding";
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
     
        private void CollapseAll()
        {
             CanvasMargin.Visibility = Visibility.Collapsed;
            CanvasBorder.Visibility = Visibility.Collapsed;
            CanvasPadding.Visibility = Visibility.Collapsed;
        }
     

        

      


    }
}
