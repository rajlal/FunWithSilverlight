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
    public partial class pageVideoBrush : UserControl
    {
        public pageVideoBrush()
        {
            InitializeComponent();
            CollapseAll();
            FillRectangle();
        }
        private void CollapseAll()
        {
            media.Stop();
            mediaReflect.Stop();      
            mediaText.Stop();
            mediaRotate.Stop();
            mediaRotateBG.Stop();
            canvasTextRotate.Visibility = Visibility.Collapsed;
            canvasTextBlock.Visibility = Visibility.Collapsed;
            canvasMedia.Visibility = Visibility.Collapsed;
                 canvasReflection.Visibility = Visibility.Collapsed;
        }
        private void ShowMedia(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
            FillRectangle();
        }

        private void FillRectangle()
        {
            canvasMedia.Visibility = Visibility.Visible;
            VideoBrush vb = new VideoBrush();
            vb.SourceName = "media";
            vb.Stretch = Stretch.UniformToFill;
            myRectangle.Fill = vb;
            media.Play();
        }
        private void ShowReflection(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            canvasReflection.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
            mediaReflect.Play();
        }

        private void ShowText(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            canvasTextBlock.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
            mediaText.Play();
 
        }

        private void ShowTextRotate(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            canvasTextRotate.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
            mediaRotate.Play();
            mediaRotateBG.Play();
 
        }

      
    }
}
