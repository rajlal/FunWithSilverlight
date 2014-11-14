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

namespace ReusableControl
{
    public partial class PageSlideshow : UserControl
    {
        private bool SwitchFlag;
        public PageSlideshow()
        {
            InitializeComponent();  
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!SwitchFlag)
            {
                SwitchData();
                SwitchFlag = true;
            }
            else
            {
                DefaultData();
                SwitchFlag = false;
            }
        }
        private void DefaultData()
        {
         
        }
        private void SwitchData()
        {
            //tn6.DataContext = new ThumbnailData() { ImageUri = "Images/Einstein.jpg", Title = "Einstein" };
            //tn5.DataContext = new ThumbnailData() { ImageUri = "Images/leonardo.jpg", Title = "Da Vinci" };
            //tn4.DataContext = new ThumbnailData() { ImageUri = "Images/Newton.jpg", Title = "Newton" };
            //tn3.DataContext = new ThumbnailData() { ImageUri = "Images/Galileo.jpg", Title = "Galileo" };
            //tn2.DataContext = new ThumbnailData() { ImageUri = "Images/Darwin.jpg", Title = "Darwin" };
            //tn1.DataContext = new ThumbnailData() { ImageUri = "Images/Edison.jpg", Title = "Edison" };
       
        }
    
        private void ReSizeThumb(double w, double h)
        {
            //tn1.ThumbWidth = tn2.ThumbWidth = tn3.ThumbWidth = tn4.ThumbWidth = tn5.ThumbWidth = tn6.ThumbWidth = w;
            //tn1.ThumbHeight = tn2.ThumbHeight = tn3.ThumbHeight = tn4.ThumbHeight = tn5.ThumbHeight = tn6.ThumbHeight = h;

        }

        private void ResizeThumb_Click(object sender, RoutedEventArgs e)
        {
            //if (tn1.ThumbWidth ==52)
            //{
            //    ReSizeThumb(80,100);
            //}
            //else if (tn1.ThumbWidth == 80)
            //{
            //    ReSizeThumb(70, 84);
            //}
            //else if (tn1.ThumbWidth == 70)
            //{
            //    ReSizeThumb(52, 65);
            //}
        }
    }
}
