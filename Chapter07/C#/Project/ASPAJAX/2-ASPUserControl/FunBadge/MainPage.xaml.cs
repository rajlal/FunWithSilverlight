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

namespace FunBadge
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }
        public MainPage(int startValue, string backColor)
        {
            InitializeComponent();
            Face.Source = new BitmapImage(new Uri("images/smiley/" + startValue.ToString() + ".png", UriKind.Relative));

            // to get the color
            String xamlString = "<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Background=\"" + backColor + "\"/>";
            Canvas c = (Canvas)System.Windows.Markup.XamlReader.Load(xamlString);
            SolidColorBrush myParameterBrush = (SolidColorBrush)c.Background;
            LayoutRoot.Background = myParameterBrush;
        }
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            string facestring = ((BitmapImage)(((Image)(sender)).Source)).UriSource.OriginalString;
            facestring = facestring.ToLower().Replace(".png", "");
            facestring = facestring.Substring(14);
            int count = Convert.ToInt32(facestring );
            count++;
            if (count == 11) count = 0;
            Image i = new Image();
            Face.Source = new BitmapImage(new Uri("images/smiley/" + count.ToString() + ".png", UriKind.Relative));
        }
    }
}
