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

namespace ReusableControl
{
    public partial class Slideshow : Control
    {
        private int slideCount;
        private int currentSlide;
        public Slideshow()
        {
            this.Loaded += new RoutedEventHandler(Control_Loaded);
            Application.Current.Host.Content.Resized += new EventHandler(Content_Resized);

        }
        private void Control_Loaded(object sender, EventArgs e)
        {

        }
        private void Content_Resized(object sender, EventArgs e)
        {
            if (Application.Current.Host.Content.IsFullScreen)
            {
                SlideWidth = Application.Current.Host.Content.ActualWidth;
                SlideHeight = Application.Current.Host.Content.ActualHeight;
            }
            else
            {
                SlideWidth = 400;
                SlideHeight = 300;

            }
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
            MainTitle = CurrentSlideIndexed + " / " + SlideCount;
        }

        public int SlideCount
        {
            get
            {
                SlideImagesList mySlides = DataContext as SlideImagesList;
                slideCount = mySlides.Count;
                return slideCount;
            }
            set
            {
                slideCount = value;
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
            get { return SlideImage.Width; }
            set
            {
                SlideImage.Width = value;
                SlideGrid.Width = value;
                SlideBorder.Width = value;
            }

        }
        public Double SlideHeight
        {
            get { return SlideImage.Height; }
            set
            {
                SlideImage.Height = value - 35;
                SlideGrid.Height = value;
                SlideBorder.Height = value;
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
            Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
            if (Application.Current.Host.Content.IsFullScreen)
            {
                SlideWidth = Application.Current.Host.Content.ActualWidth;
                SlideHeight = Application.Current.Host.Content.ActualHeight;
            }
            else
            {
                SlideWidth = 400;
                SlideHeight = 300;

            }
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
