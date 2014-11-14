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

namespace PaintBrushes
{
    public partial class PageRadialGradient : UserControl
    {
        public PageRadialGradient()
        {
            InitializeComponent();
        }

        private void ShowMultiple(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasMultiple.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }

        private void ShowRadial(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasRadial.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();

        }

        private void ShowSun(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasSun.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
      

        private void ShowOrb(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasOrb.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }

        private void ShowSunrise(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasSunrise.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void CollapseAll()
        {
            CanvasRadial.Visibility = Visibility.Collapsed;
            CanvasMultiple.Visibility = Visibility.Collapsed;
            CanvasSun.Visibility = Visibility.Collapsed;
            CanvasOrb.Visibility = Visibility.Collapsed;
            CanvasSunrise.Visibility = Visibility.Collapsed;

        }

        private void sliderOrigin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                double sliderValue = sliderOrgin.Value / 100.0;
                RadialOrigin.GradientOrigin = new Point(sliderValue, sliderValue);
                ToolTip t = new ToolTip();
                t.Content = "Gradient Origin:" + sliderValue + "/" + sliderValue;
                ToolTipService.SetToolTip(sliderOrgin, t);
                radialOriginText.Text = "Origin:" + string.Format("{0:N}", sliderValue) + "," + string.Format("{0:N}", sliderValue);
            }
            catch (Exception)
            { }
        }

        private void sliderCenter_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                double sliderValue = sliderCenter.Value / 100.0;
                RadialCenter.Center = new Point(sliderValue, sliderValue);
                ToolTip t = new ToolTip();
                t.Content = "Gradient Center:" + sliderValue + "/" + sliderValue;
                ToolTipService.SetToolTip(sliderCenter, t);
                radialCenterText.Text = "Center:" + string.Format("{0:N}", sliderValue) + "," + string.Format("{0:N}", sliderValue);
            }
            catch (Exception)
            { }
        }

        private void sliderRadius_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                double sliderValue = sliderRadius.Value / 100.0;
                RadialRadius.RadiusX = sliderValue;
                RadialRadius.RadiusY = sliderValue;
                ToolTip t = new ToolTip();
                t.Content = "Radius:" + sliderValue + "," + sliderValue;
                ToolTipService.SetToolTip(sliderRadius, t);
                radialRadiusXText.Text = "RadiusX:" + string.Format("{0:N}", sliderValue);
                radialRadiusYText.Text = "RadiusY:" + string.Format("{0:N}", sliderValue);
            }
            catch (Exception)
            { }
        }
    }
}
