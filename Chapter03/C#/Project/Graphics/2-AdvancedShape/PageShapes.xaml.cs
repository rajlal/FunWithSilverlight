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
using System.Windows.Markup;

namespace AdvancedShapes
{
    public partial class PageShapes : UserControl
    {
        public PageShapes()
        {
            InitializeComponent();
      
        }
        private void DynamicItemContainer_MouseMove(object sender, MouseEventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.Content = "X: " + e.GetPosition(DynamicItemContainer).X.ToString() + " Y:" + e.GetPosition(DynamicItemContainer).Y.ToString();//"
            ToolTipService.SetToolTip(DynamicItemContainer, tt);
            StatusXY.Text = tt.Content.ToString();
        }
        private void Line_MouseEnter(object sender, MouseEventArgs e)
        {
            Line l = (Line)sender;

            if (l.Opacity == 1)
            {
                l.Stroke = new SolidColorBrush(Colors.Blue);
                StatusBar.Text = ToolTipService.GetToolTip(l).ToString();
            }
        }
        private void Line_MouseLeave(object sender, MouseEventArgs e)
        {
            Line l = (Line)sender;
            if (l.Opacity == 1)
            {
                l.Stroke = new SolidColorBrush(getColorFromHex("FFCCCCFF"));
            }
        }
        private void gridCheck_Click(object sender, RoutedEventArgs e)
        {
            updateGrid();
        }
        private void updateGrid()
        {
            if ((bool)chkGrid.IsChecked)
            {
                CanvasGridLine.Visibility=Visibility.Visible ;
                StatusBar.Text = "Gridlines On: 1 unit = 20 px";
            }
            else
            {
                CanvasGridLine.Visibility = Visibility.Collapsed;
                StatusBar.Text = "Gridlines Off";
            }
        }
        public Color getColorFromHex(string s)
        {
            byte a = System.Convert.ToByte(s.Substring(0, 2), 16);
            byte r = System.Convert.ToByte(s.Substring(2, 2), 16);
            byte g = System.Convert.ToByte(s.Substring(4, 2), 16);
            byte b = System.Convert.ToByte(s.Substring(6, 2), 16);
            return Color.FromArgb(a, r, g, b);
        }


        private void Geometry_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClearAll();
            CanvasGeometry.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }

        private void Curve_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClearAll();
            CanvasCurve.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void Arc_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClearAll();
            CanvasArc.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void YinYan_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClearAll();
            CanvasYinYan.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void Lines_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClearAll();
            CanvasPolyLine.Visibility = Visibility.Visible;
            CanvasLinePath.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void Clear_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ClearAll();

        }
        private void ClearAll()
        {
            CanvasLinePath.Visibility = Visibility.Collapsed;
            CanvasPolyLine.Visibility = Visibility.Collapsed;
            CanvasGeometry.Visibility = Visibility.Collapsed;
            CanvasCurve.Visibility = Visibility.Collapsed;
            CanvasArc.Visibility = Visibility.Collapsed;
            CanvasYinYan.Visibility = Visibility.Collapsed;

        }
          
        private void Path_Mousemove(object sender, MouseEventArgs e)
        {
            StatusBar.Text = ToolTipService.GetToolTip((Path)sender).ToString();
        }
        private void Curve_MouseMove(object sender, MouseEventArgs e)
        {
            StatusBar.Text = ToolTipService.GetToolTip((Path)sender).ToString();
        }
        private void Polyline_Mousemove(object sender, MouseEventArgs e)
        {
            StatusBar.Text = ToolTipService.GetToolTip((Polyline)sender).ToString();
        }
        private void CG_Path_MouseMove(object sender, MouseEventArgs e)
        {
            StatusBar.Text = "Geometry Group with FillRule=EvenOdd";
        }
        private void EG_Path_MouseMove(object sender, MouseEventArgs e)
        {
            StatusBar.Text = "Center=80,140 rX=50 rY=50";
        }
        private void RG_Path_MouseMove(object sender, MouseEventArgs e)
        {
            StatusBar.Text = "Rect=40,20,80,60";
        }

        private void Ellipse_MouseEnter(object sender, MouseEventArgs e)
        {
            MainE.Opacity = .5;
        }

        private void Ellipse_MouseLeave(object sender, MouseEventArgs e)
        {
            MainE.Opacity = 1.0;
        }

        private void YYp1_MouseEnter(object sender, MouseEventArgs e)
        {
            YYp1.Opacity = .5;
        }

        private void YYp1_MouseLeave(object sender, MouseEventArgs e)
        {
            YYp1.Opacity = 1.0;
        }

        private void YYp2_MouseEnter(object sender, MouseEventArgs e)
        {
            YYp2.Opacity = .5;
        }

        private void YYp2_MouseLeave(object sender, MouseEventArgs e)
        {
            YYp2.Opacity = 1.0;
        }

        private void YYp3_MouseEnter(object sender, MouseEventArgs e)
        {
            YYp3.Opacity = 0.5;
        }

        private void YYp3_MouseLeave(object sender, MouseEventArgs e)
        {
            YYp3.Opacity = 1.0;
        }

  

       
    }
}
