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

namespace PaintBrushes
{
    public partial class PageMediaBrush : UserControl
    {
        public PageMediaBrush()
        {
            InitializeComponent();
        }

       

        private void ShowImageBrush(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasText.Visibility = Visibility.Visible;
            

            TextBlock tbShadow = new TextBlock();
            tbShadow.Text = "Silverlight";
            tbShadow.FontSize = 42;
            tbShadow.Foreground = new SolidColorBrush( Colors.Gray);
            tbShadow.SetValue(Canvas.TopProperty, 22.0);
            tbShadow.SetValue(Canvas.LeftProperty, 22.0);
            
            TextBlock tb = new TextBlock();
            tb.Text = "Silverlight";
            tb.FontSize = 42;
            tb.SetValue(Canvas.TopProperty, 20.0);
            tb.SetValue(Canvas.LeftProperty, 20.0);
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri("files/silverlighticon.png", UriKind.Relative));
            tb.Foreground = ib;


            CanvasText.Children.Add(tbShadow);
            CanvasText.Children.Add(tb);
           
                  ShowVideoBrush(sender, e);
        }

        private void ShowVideoBrush(object sender, MouseButtonEventArgs e)
        {
         
            TextBlock tbShadow = new TextBlock();
            tbShadow.Text = "Silverlight";
            tbShadow.FontSize = 42;
            tbShadow.Foreground = new SolidColorBrush( Colors.Gray); 
            tbShadow.SetValue(Canvas.TopProperty, 102.0);
            tbShadow.SetValue(Canvas.LeftProperty, 72.0);

            TextBlock tb = new TextBlock();
            tb.Text = "Silverlight";
            tb.FontSize = 42;
            tb.SetValue(Canvas.TopProperty, 100.0);
            tb.SetValue(Canvas.LeftProperty, 70.0);
           
           
            VideoBrush vb = new VideoBrush();
            vb.SetSource(myMedia);
            vb.Stretch = Stretch.UniformToFill;

            tb.Foreground = vb;
            CanvasText.Children.Add(tbShadow);
            CanvasText.Children.Add(tb);
            myMedia.Play();
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }

        private void ShowBorder(object sender, MouseButtonEventArgs e)
        {
            CollapseAll(); 
            CanvasText.Children.Clear();
            myMedia.Play();
            CanvasBorder.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();


        }

        private void ShowShapeBrush(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasShape.Visibility = Visibility.Visible;
            myMedia.Play();
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void CollapseAll()
        {
            myMedia.Stop();
            CanvasText.Visibility = Visibility.Collapsed;
            CanvasText.Children.Clear();
            CanvasBorder.Visibility = Visibility.Collapsed;

            CanvasShape.Visibility = Visibility.Collapsed;
            CanvasControl.Visibility = Visibility.Collapsed;


        }

        private void ShowControlBrush(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasControl.Visibility = Visibility.Visible;
            myMedia.Play();

            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();

        }
    }
}
