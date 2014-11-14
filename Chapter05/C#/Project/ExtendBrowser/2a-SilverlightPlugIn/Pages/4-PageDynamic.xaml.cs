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

namespace SilverlightPlugIn
{
    public partial class PageDynamic : UserControl
    {
        public PageDynamic(string RectangleColor, string VariableCustom)
        {
            InitializeComponent();
            txtParameter.Text = "Custom Value: " + VariableCustom;
            recParameter.Fill = new SolidColorBrush(getColorFromHex(RectangleColor));
   
        }
        public Color getColorFromHex(string s)
        {
            byte a = System.Convert.ToByte(s.Substring(0, 2), 16);
            byte r = System.Convert.ToByte(s.Substring(2, 2), 16);
            byte g = System.Convert.ToByte(s.Substring(4, 2), 16);
            byte b = System.Convert.ToByte(s.Substring(6, 2), 16);
            return Color.FromArgb(a, r, g, b);
        }

    }
}
