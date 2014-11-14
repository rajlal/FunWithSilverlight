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
    public partial class PageSolidBrush : UserControl
    {
        public PageSolidBrush()
        {
            InitializeComponent();
        }

        private void GetSolidColorBrush()
        {
            ClearAll(); 
            canvasSolidColor.Visibility = Visibility.Visible;
            StatusBar.Text = "SolidColor Brush support 16x16x16x16=65536 Colors";
        }


        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
            string colorValueA = string.Format("{0:0#}", sliderA.Value); 
            string colorValueR = string.Format("{0:0#}", sliderR.Value) ;
            string colorValueG = string.Format("{0:0#}", sliderG.Value);
            string colorValueB = string.Format("{0:0#}", sliderB.Value);

            string cValueA = ConvertHex(colorValueA) ;
            string cValueR = ConvertHex(colorValueR) ;
            string cValueG = ConvertHex(colorValueG) ;
            string cValueB = ConvertHex(colorValueB);
            string colorValue = cValueA + cValueR + cValueG + cValueB;
            txtColorA.Text = cValueA;
            txtColorR.Text = cValueR;
            txtColorG.Text = cValueG;
            txtColorB.Text = cValueB;

            txtColor.Text = "#" + colorValue;
            rectangleColor.Fill = new SolidColorBrush(getColorFromHex(colorValue));
        }
        private string ConvertHex(string s)
        {
            string rHex = s;
            if (s=="10") rHex= "AA";
            if (s == "11") rHex = "BB";
            if (s == "12") rHex = "CC";
            if (s == "13") rHex = "DD";
            if (s == "14") rHex = "EE";
            if (s == "15") rHex = "FF";

            return rHex;
        }
        public Color getColorFromHex(string s)
        {
            byte a = System.Convert.ToByte(s.Substring(0, 2), 16);
            byte r = System.Convert.ToByte(s.Substring(2, 2), 16);
            byte g = System.Convert.ToByte(s.Substring(4, 2), 16);
            byte b = System.Convert.ToByte(s.Substring(6, 2), 16);
            return Color.FromArgb(a, r, g, b);
        }

        private void CreateWebSafeColors(object sender, MouseButtonEventArgs e)
        {
            ClearAll();
            string[] wscolors = new string[6] { "00", "33", "66", "99", "CC", "FF" };

            double topv = 0;
            for (int i = 0; i < 6; i++)
            {
                topv += 25;
                double leftv = 6;
                for (int j = 0; j < 6; j++)
                {
                    for (int k = 0; k < 6; k++)
                    {
                        string currentcolor = wscolors[i] + wscolors[j] + wscolors[k];
                        Rectangle rc = new Rectangle();
                        rc.SetValue(Canvas.LeftProperty, leftv);
                        rc.SetValue(Canvas.TopProperty, topv);
                        rc.Fill = new SolidColorBrush(getColorFromHex("FF" + currentcolor));
                        rc.Width = 7;
                        rc.Height = 24;
                        rc.Cursor = Cursors.Hand;
                        ToolTip t = new ToolTip();
                        t.Content = "FF" + currentcolor;
                        ToolTipService.SetToolTip(rc, t);
                        leftv += 8;
                        CanvasWS.Children.Add(rc);
                    }
                }
            }
            CanvasWS.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }

        private void ShowPreDefined(object sender, MouseButtonEventArgs e)
        {
            ClearAll();
            canvasPreDefColor.Visibility = Visibility.Visible;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }

        private void ClearAll()
        {
            canvasSolidColor.Visibility = Visibility.Collapsed;
            canvasPreDefColor.Visibility = Visibility.Collapsed;
            CanvasWS.Visibility = Visibility.Collapsed;

        }

        private void Colors_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            GetSolidColorBrush();

        }
    }
}
