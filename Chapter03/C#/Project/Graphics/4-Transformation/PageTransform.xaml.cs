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

namespace Transformation
{
    public partial class PageTransform : UserControl
    {
        public PageTransform()
        {
            InitializeComponent();
        }

        private void sliderRotate_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            transformRotate.Angle = sliderRotate.Value;
            ToolTip t = new ToolTip();
            t.Content = "Rotate at an Angle:" + sliderRotate.Value;
            ToolTipService.SetToolTip(sliderRotate, t);
            RotateAngle.Text = "Angle: " + string.Format("{0:0}", sliderRotate.Value);
        }

        private void sliderScaleX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                valueTransformScale.ScaleX = sliderScaleX.Value / 100.0;
                ToolTip t = new ToolTip();
                t.Content = "Scale percentage:" + sliderScaleX.Value;
                ToolTipService.SetToolTip(sliderScaleX, t);
                ScaleX.Text = "ScaleX: " + string.Format("{0:N}", valueTransformScale.ScaleX);
            }
            catch(Exception)
            {}
        }
        private void sliderScaleY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
            valueTransformScale.ScaleY = sliderScaleY.Value / 100.0;
            ToolTip t = new ToolTip();
            t.Content = "Scale percentage:" + sliderScaleY.Value;
            ToolTipService.SetToolTip(sliderScaleY, t);
            ScaleY.Text = "ScaleY: " + string.Format("{0:N}", valueTransformScale.ScaleY);
            }
            catch(Exception)
            {}
        }

        private void sliderSkewX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                valueSkewTransform.AngleX= sliderSkewX.Value ;
                ToolTip t = new ToolTip();
                t.Content = "Skew Angle:" + sliderSkewX.Value;
                ToolTipService.SetToolTip(sliderSkewX, t);
                SkewX.Text = "AngleX: " + string.Format("{0:N}", valueSkewTransform.AngleX);
            }
            catch (Exception)
            { }
        }
        private void sliderSkewY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                valueSkewTransform.AngleY = sliderSkewY.Value;
                ToolTip t = new ToolTip();
                t.Content = "Skew Angle:" + sliderSkewY.Value;
                ToolTipService.SetToolTip(sliderSkewY, t);
                SkewY.Text = "AngleY: " + string.Format("{0:N}", valueSkewTransform.AngleY);
            }
            catch (Exception)
            { }
        }

        private void sliderTranslateX_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            valueTranslateTransform.X = sliderTranslateX.Value;
            ToolTip t = new ToolTip();
            t.Content = "Move X:" + sliderTranslateX.Value;
            ToolTipService.SetToolTip(sliderTranslateX, t);
            TranslateX.Text = "Move X: " + string.Format("{0:N}", valueTranslateTransform.X);

        }
        private void sliderTranslateY_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            valueTranslateTransform.Y= sliderTranslateY.Value;
            ToolTip t = new ToolTip();
            t.Content = "Move Y:" + sliderTranslateY.Value;
            ToolTipService.SetToolTip(sliderTranslateY, t);
            TranslateY.Text = "Move Y: " + string.Format("{0:N}", valueTranslateTransform.Y);
        }



        private void ShowScale(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasScale.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void ShowSkew(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasSkew.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void ShowRotate(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasRotate.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void ShowTranslate(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasTranslate.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void CollapseAll()
        {
            CanvasRotate.Visibility = Visibility.Collapsed;
            CanvasScale.Visibility = Visibility.Collapsed;
            CanvasSkew.Visibility = Visibility.Collapsed;
            CanvasTranslate.Visibility = Visibility.Collapsed;
        }

    }
}
