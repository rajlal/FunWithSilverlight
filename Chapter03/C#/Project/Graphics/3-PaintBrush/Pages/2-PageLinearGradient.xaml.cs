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
    public partial class PageLinearGradient : UserControl
    {
        public PageLinearGradient()
        {
            InitializeComponent();
        }
        private void ShowLinearGradient(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasLinear.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void ShowSun(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasSun.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();

        }
        private void ShowShape(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasShape.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void ShowPrism(object sender, MouseButtonEventArgs e)
        {
            CollapseAll();
            CanvasPrism.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void CollapseAll()
        {
            CanvasLinear.Visibility = Visibility.Collapsed;
            CanvasPrism.Visibility = Visibility.Collapsed;
            CanvasShape.Visibility = Visibility.Collapsed;
            CanvasSun.Visibility = Visibility.Collapsed;
        }
    }
}
