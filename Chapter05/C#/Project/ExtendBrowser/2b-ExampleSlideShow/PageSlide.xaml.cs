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

namespace SlideShow
{
    public partial class PageSlide : UserControl
    {
        
        public PageSlide(String[] ImageArray, double myWidth, double myHeight)
        {
            
            InitializeComponent();
            LoadItems(ImageArray, myWidth, myHeight);
        }
        private void LoadItems(string[] listImages, double myWidth, double myHeight)
        {
            SlideImagesList mySlides = new SlideImagesList();

            for (int i = 0; i < listImages.Length; i++)
            {
                Slides s1 = new Slides();
                s1.ImageUri = listImages[i];
                mySlides.Add(s1);
            }
            mySlides.Capacity = listImages.Length;


            Slide mySlideShow = new Slide(myWidth, myHeight);
            mySlideShow.Name = "tn1";
            
            mySlideShow.MainImage = listImages[0];
            mySlideShow.MainTitle = "1 / " + listImages.Length;
            mySlideShow.DataContext = mySlides;
            mySlideShow.SlideCount = mySlides.Count;
            LayoutRoot.Children.Add(mySlideShow);
        }
    }
}
