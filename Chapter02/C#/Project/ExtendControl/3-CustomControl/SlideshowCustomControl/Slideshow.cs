using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace SlideshowCustomControl
{
    public class Slideshow : Control
    {
        private Canvas layoutRoot;
        private Image SlideImage;
        private Border SlideBorder;
        private Grid SlideGrid;
        private TextBlock SlideTitle;
        private Image LeftButton;
        private Image RightButton;
        private Image FullscreenButton;

         public Slideshow() : base()
          {
              DefaultStyleKey = typeof(Slideshow);
              Application.Current.Host.Content.Resized += new EventHandler(Content_Resized);
          }
     public static readonly DependencyProperty SlideStyleProperty = DependencyProperty.Register("SlideStyle", typeof(Style), typeof(Slideshow), null);
     public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(Slideshow), null);
     public static readonly DependencyProperty SlideWidthProperty = DependencyProperty.Register("SlideWidth", typeof(double), typeof(Slideshow), null);
     public static readonly DependencyProperty SlideHeightProperty = DependencyProperty.Register("SlideHeight", typeof(double), typeof(Slideshow), null);
     public static readonly DependencyProperty MainTitleProperty = DependencyProperty.Register("MainTitle", typeof(string), typeof(Slideshow), null);
     public static readonly DependencyProperty MainImageProperty = DependencyProperty.Register("MainImage", typeof(string), typeof(Slideshow), null);
     public static readonly DependencyProperty CurrentSlideProperty = DependencyProperty.Register("CurrentSlide", typeof(int), typeof(Slideshow), null);
     public static readonly DependencyProperty SlideCountProperty = DependencyProperty.Register("SlideCount", typeof(int), typeof(Slideshow), null);
     /// <summary>
     /// Gets or sets the style of the added TextBlock controls
     /// </summary>
     public Style SlideStyle
     {
         get
         {
             return (Style)this.GetValue(SlideStyleProperty);
         }

         set
         {
             base.SetValue(SlideStyleProperty, (DependencyObject)value);
         }
     }
     /// <summary>
     /// Gets or sets the text of the LinkLabel control
     /// </summary>
     public string Text
     {
         get
         {
             return (string)this.GetValue(TextProperty);
         }

         set
         {
             base.SetValue(TextProperty, value);
         }
     }
     public int SlideCount
     {
         get
         {
             return (int)GetValue(SlideCountProperty);
         }

         set
         {
             SetValue(SlideCountProperty, value);
         }

     }
     public int CurrentSlide
     {
         get
         {
             return (int)GetValue(CurrentSlideProperty);
         }

         set
         {
             SetValue(CurrentSlideProperty, value);
         }
     }
     public Double SlideWidth
     {
         get
         {
             return (double)GetValue(SlideWidthProperty);
         }

         set
         {
             SetValue(SlideWidthProperty, value);
           
         }
     }
     public Double SlideHeight
     {
         get
         {
             return (double)GetValue(SlideHeightProperty);
         }
         set
         {
             SetValue(SlideHeightProperty, value);
         }
     }
     public string MainImage
     {
         get
         {
             return (string)GetValue(MainImageProperty);
         }

         set
         {
             SetValue(MainImageProperty, value);
         }
     }
     public string MainTitle
     {
         get
         {
             return (string)GetValue(MainTitleProperty);
         }

         set
         {
             SetValue(MainTitleProperty, value);
         }
     }
     public override void OnApplyTemplate()
        {
           this.layoutRoot = this.GetTemplateChild("LayoutRoot") as Canvas;
           this.SlideImage = this.GetTemplateChild("SlideImage") as Image;
           this.SlideGrid = this.GetTemplateChild("SlideGrid") as Grid;
           this.SlideBorder = this.GetTemplateChild("SlideBorder") as Border;
           this.SlideTitle = this.GetTemplateChild("SlideTitle") as TextBlock;
           this.LeftButton = this.GetTemplateChild("LeftButton") as Image;
           this.RightButton = this.GetTemplateChild("RightButton") as Image;
           this.FullscreenButton = this.GetTemplateChild("FullscreenButton") as Image;

           this.SlideImage.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.NextSlide);
           this.SlideImage.ImageFailed += new EventHandler<ExceptionRoutedEventArgs> (this.ImageFailed);         
           this.RightButton.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.NextSlide);
           this.LeftButton.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.PreviousSlide);
           this.FullscreenButton.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.FullScreen);

           base.OnApplyTemplate();
           LoadItems();
        }
     private void Content_Resized(object sender, EventArgs e)
     {
         try
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
             this.SlideImage.Width = SlideWidth;
             this.SlideGrid.Width = SlideWidth;
             this.SlideBorder.Width = SlideWidth;

             this.SlideImage.Height = SlideHeight - 35;
             this.SlideGrid.Height = SlideHeight;
             this.SlideBorder.Height = SlideHeight;
         }
         catch (Exception)
         { }
     }
     private void FullScreen(object sender, EventArgs e)
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
         this.SlideImage.Width = SlideWidth;
         this.SlideGrid.Width = SlideWidth;
         this.SlideBorder.Width = SlideWidth;

         this.SlideImage.Height = SlideHeight - 35;
         this.SlideGrid.Height = SlideHeight;
         this.SlideBorder.Height = SlideHeight;
     }
     private void ImageFailed(object sender, EventArgs e)
     {
         MainImage = "Images/_ErrorImage.jpg";
         SlideImage.Source = new BitmapImage(new Uri(MainImage, UriKind.Relative));
     }
     private void NextSlide(object sender, RoutedEventArgs e)
     {
         if (CurrentSlide == SlideCount - 1)
             CurrentSlide = 0;
         else
             CurrentSlide++;
         SlideImagesList mySlides = DataContext as SlideImagesList;
         MainImage = mySlides[CurrentSlide].ImageUri;
         int CurrentSlideIndexed = CurrentSlide + 1;
         MainTitle = CurrentSlideIndexed + " / " + SlideCount;
         SlideImage.Source = new BitmapImage(new Uri(MainImage, UriKind.Relative));
         SlideTitle.Text = MainTitle;
     }

     private void PreviousSlide(object sender, RoutedEventArgs e)
     {
         if (CurrentSlide == 0)
             CurrentSlide = SlideCount - 1;
         else
             CurrentSlide--;
         SlideImagesList mySlides = DataContext as SlideImagesList;
         MainImage = mySlides[CurrentSlide].ImageUri;
         int CurrentSlideIndexed = CurrentSlide + 1;
         MainTitle = CurrentSlideIndexed + " / " + SlideCount;
         SlideImage.Source = new BitmapImage(new Uri(MainImage, UriKind.Relative));
         SlideTitle.Text = MainTitle;
     }
     private void LoadItems()
     {
         SlideImagesList mySlides = DataContext as SlideImagesList;
         SlideCount = mySlides.Count;
         SlideImage.Source = new BitmapImage(new Uri(MainImage, UriKind.Relative));
         SlideTitle.Text = MainTitle;

         this.SlideImage.Width = SlideWidth;
         this.SlideGrid.Width = SlideWidth;
         this.SlideBorder.Width = SlideWidth;

         this.SlideImage.Height = SlideHeight- 35;
         this.SlideGrid.Height = SlideHeight;
         this.SlideBorder.Height = SlideHeight;
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


