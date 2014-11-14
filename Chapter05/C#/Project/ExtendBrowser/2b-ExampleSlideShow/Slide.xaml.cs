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
using System.Windows.Browser;

namespace SlideShow
{
    public partial class Slide : UserControl
    {
        private int slideCount;
        private int currentSlide;
        private double slideWidth;
        private double slideHeight;

        public Slide(double w, double h)
        {
            InitializeComponent();
            Application.Current.Host.Content.Resized += new EventHandler(Content_Resized);
            this.SlideWidth = w;
            this.SlideHeight = h;
        }
        private void Content_Resized(object sender, EventArgs e) 
        {
            double SWidth = 0;
            double SHeight = 0;
            if (Application.Current.Host.Content.IsFullScreen)
            {
                SWidth = Application.Current.Host.Content.ActualWidth;
                SHeight = Application.Current.Host.Content.ActualHeight;
            }
            else
            {
                SWidth = slideWidth;
                SHeight = slideHeight;

            }
            SlideImage.Width = SWidth;
            SlideGrid.Width = SWidth;
            SlideBorder.Width = SWidth;

            SlideImage.Height = SHeight - 35;
            SlideGrid.Height = SHeight;
            SlideBorder.Height = SHeight;
        }
        private void SetupSlideShow()
        {
            SlideImagesList mySlides = DataContext as SlideImagesList;
            SlideCount = mySlides.Count;
            CurrentSlide = 0;
            SetToolTip(); 

        }
        private void SetToolTip()
        {
            int CurrentSlideIndexed = CurrentSlide + 1;
            MainTitle = CurrentSlideIndexed + " / " + SlideCount ;
        }

        public int SlideCount
        {
            get {
                SlideImagesList mySlides = DataContext as SlideImagesList;
                slideCount = mySlides.Count;
                return slideCount; 
            }
            set
            {
                slideCount =value;
            }
        }
        public int CurrentSlide
        {
            get { return currentSlide; }
            set
            {
                currentSlide = value;
            }
        }
        public Double SlideWidth
        {
            get { return slideWidth; }
            set 
            {
                slideWidth = value;
                
            }

        }
        public Double SlideHeight
        {
            get { return slideHeight; }
            set
            {
                slideHeight = value ;
              
            }

        }
        public string MainImage
        {
            get { return SlideImage.Source.ToString(); }
            set
            {
                SlideImage.Source = new BitmapImage(new Uri(value, UriKind.Relative));
             }

        }
        public string MainTitle
        {
            get { return SlideTitle.Text; }
            set
            {
                SlideTitle.Text = value;
            }

        }
        private void SlideImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            NextSlide();
        }
        private void RightButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            NextSlide();
        }
        private void LeftButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PreviousSlide();
        }
        private void FullscreenButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double SWidth = 0;
            double SHeight = 0;

           Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
           if (Application.Current.Host.Content.IsFullScreen)
            {
                SWidth = Application.Current.Host.Content.ActualWidth;
                SHeight = Application.Current.Host.Content.ActualHeight;
            }
            else
            {
                SWidth = slideWidth;
                SHeight = slideHeight;

            }

           SlideImage.Width = SWidth;
           SlideGrid.Width = SWidth;
           SlideBorder.Width = SWidth;

           SlideImage.Height = SHeight - 35;
           SlideGrid.Height = SHeight;
           SlideBorder.Height = SHeight;
        }
        private void NextSlide()
        {
            if (CurrentSlide == SlideCount - 1)
                CurrentSlide = 0;
            else
                CurrentSlide++;
            SlideImagesList mySlides = DataContext as SlideImagesList;
            MainImage = mySlides[CurrentSlide].ImageUri;
            SetToolTip();
        }
        private void PreviousSlide()
        {
            if (CurrentSlide == 0)
                CurrentSlide = SlideCount - 1;
            else
                CurrentSlide--;
            SlideImagesList mySlides = DataContext as SlideImagesList;
            MainImage = mySlides[CurrentSlide].ImageUri;
            SetToolTip();
        }
        private void SlideImage_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MainImage = "Images/_ErrorImage.jpg";
        }

    }

    public class Slides
    {
        public string ImageUri { get; set; }
        public string Title { get; set; }
    }

    public class SlideImagesList : List<Slides>
    {
        Slides si;
        public Slides Val { get { return si; } set { si = value; } }
        public SlideImagesList()
        {
        }
    }

}
