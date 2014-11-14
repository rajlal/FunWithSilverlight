using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace DeepZoom
{
    public partial class PageMultipleImage : UserControl
    {
        //
        // Based on prior work done by Lutz Gerhard, Peter Blois, and Scott Hanselman
        //
        double zoom = 1;
        bool duringDrag = false;
        bool mouseDown = false;
        Point lastMouseDownPos = new Point();
        Point lastMousePos = new Point();
        Point lastMouseViewPort = new Point();
        int subImageIndex = 7;

        public double ZoomFactor
        {
            get { return zoom; }
            set { 
                zoom = value;
            }
        }

        public PageMultipleImage()
        {
            InitializeComponent();
            //
            // Firing an event when the MultiScaleImage is Loaded
            //
            this.msi.Loaded += new RoutedEventHandler(msi_Loaded);

            //
            // Firing an event when all of the images have been Loaded
            //
            this.msi.ImageOpenSucceeded += new RoutedEventHandler(msi_ImageOpenSucceeded);

            //
            // Handling all of the mouse and keyboard functionality
            //
            this.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
            {
                lastMouseDownPos = e.GetPosition(msi);
                lastMouseViewPort = msi.ViewportOrigin;

                mouseDown = true;

                msi.CaptureMouse();
            };

            this.MouseLeftButtonUp += delegate(object sender, MouseButtonEventArgs e)
            {
                if (!duringDrag)
                {
                    bool shiftDown = (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
                    double newzoom = zoom;

                    if (shiftDown)
                    {
                        newzoom /= 2;
                    }
                    else
                    {
                        newzoom *= 2;
                    }

                    Zoom(newzoom, msi.ElementToLogicalPoint(this.lastMousePos));

                    Point p = this.msi.ElementToLogicalPoint(e.GetPosition(this.msi));
                    subImageIndex = SubImageHitTest(p);
                    if (subImageIndex >= 0)
                    {

                       if (ZoomFactor > 1)
                        {
                            subImageTitle.Text = GetNameofWonder(subImageIndex);
                            subImageThumb.Source = new BitmapImage(new Uri("Images/thumb" + subImageIndex + ".jpg", UriKind.Relative));
                            subImageDesc.Text = GetDescriptionWonder(subImageIndex); ;
                        }
                    }
                }
                duringDrag = false;
                mouseDown = false;

                msi.ReleaseMouseCapture();
            };

            this.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                lastMousePos = e.GetPosition(msi);
                if (mouseDown && !duringDrag)
                {
                    duringDrag = true;
                    double w = msi.ViewportWidth;
                    Point o = new Point(msi.ViewportOrigin.X, msi.ViewportOrigin.Y);
                    msi.UseSprings = false; 
                    msi.ViewportOrigin = new Point(o.X, o.Y);
                    msi.ViewportWidth = w;
                    zoom = 1/w;
                    msi.UseSprings = true; 
                }

                if (duringDrag)
                {
                    Point newPoint = lastMouseViewPort;
                    newPoint.X += (lastMouseDownPos.X - lastMousePos.X) / msi.ActualWidth * msi.ViewportWidth;
                    newPoint.Y += (lastMouseDownPos.Y - lastMousePos.Y) / msi.ActualWidth * msi.ViewportWidth;

                    msi.ViewportOrigin = newPoint;
                }
            };

            new MouseWheelHelper(this).Moved += delegate(object sender, MouseWheelEventArgs e)
            {
                e.Handled = true;

                double newzoom = zoom;

                if (e.Delta < 0)
                    newzoom /= 1.3;
                else
                    newzoom *= 1.3;

                Zoom(newzoom, msi.ElementToLogicalPoint(this.lastMousePos));
                msi.CaptureMouse();
            };
        }
        int SubImageHitTest(Point p)
        {
            for (int i = 0; i < this.msi.SubImages.Count; i++)
            {
                Rect subImageRect = GetSubImageRect(i);
                if (subImageRect.Contains(p))
                    return i;
            } return -1;
        }
        Rect GetSubImageRect(int indexSubImage)
        {
            if (indexSubImage < 0 || indexSubImage >= this.msi.SubImages.Count)
                return Rect.Empty;
            MultiScaleSubImage subImage = this.msi.SubImages[indexSubImage];
            double scaleBy = 1 / subImage.ViewportWidth;
            return new Rect(-subImage.ViewportOrigin.X * scaleBy, -subImage.ViewportOrigin.Y * scaleBy, 1 * scaleBy, (1 / subImage.AspectRatio) * scaleBy);
        }
        void DisplaySubImageCentered(int indexSubImage)
        {
            if (indexSubImage < 0 || indexSubImage >= msi.SubImages.Count)
                return;

            Rect subImageRect = GetSubImageRect(indexSubImage);
            double msiAspectRatio = msi.ActualWidth / msi.ActualHeight;

            Point newOrigin = new Point(subImageRect.X - (msi.ViewportWidth / 2) + (subImageRect.Width / 2),
                                        subImageRect.Y - ((msi.ViewportWidth / msiAspectRatio) / 2) + (subImageRect.Height / 2));

            msi.ViewportOrigin = newOrigin;
        }
        void msi_ImageOpenSucceeded(object sender, RoutedEventArgs e)
        {
            //If collection, this gets you a list of all of the MultiScaleSubImages
            //
            foreach (MultiScaleSubImage subImage in msi.SubImages)
            {
              //  System.Windows.Browser.HtmlPage.Window.Alert(subImage.ToString());

            }

            msi.ViewportWidth = 1;
        }

        void msi_Loaded(object sender, RoutedEventArgs e)
        {
            // Hook up any events you want when the image has successfully been opened
        }

        private void Zoom(double newzoom, Point p)
        {
            
                msi.ZoomAboutLogicalPoint(newzoom / zoom, p.X, p.Y);
                zoom = newzoom;
            
        }

        private void ZoomInClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Zoom(zoom * 1.3, msi.ElementToLogicalPoint(new Point(.5 * msi.ActualWidth, .5 * msi.ActualHeight)));
        }

        private void ZoomOutClick(object sender, System.Windows.RoutedEventArgs e)
        {
            Zoom(zoom / 1.3, msi.ElementToLogicalPoint(new Point(.5 * msi.ActualWidth, .5 * msi.ActualHeight)));
        }

        private void GoHomeClick(object sender, System.Windows.RoutedEventArgs e)
        {
        	this.msi.ViewportWidth = 1.0;
			this.msi.ViewportOrigin = new Point(0.0,0.0);
            ZoomFactor = 1;
        }


        private void GoFullScreenClick(object sender, System.Windows.RoutedEventArgs e)
        {
        	if (!Application.Current.Host.Content.IsFullScreen) 
			{ 
				Application.Current.Host.Content.IsFullScreen = true;
                msi.Width = Application.Current.Host.Content.ActualWidth;
                msi.Height= Application.Current.Host.Content.ActualHeight;
            } 
			else 
			{ 
				Application.Current.Host.Content.IsFullScreen = false;
                msi.Width = 600;
                msi.Height = 500;
			} 
        }

        // Handling the VSM states
        private void LeaveMovie(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //VisualStateManager.GoToState(this, "FadeOut", true);
            buttonCanvas.Opacity = 0; 
        }

        private void EnterMovie(object sender, System.Windows.Input.MouseEventArgs e)
        {
            buttonCanvas.Opacity = 1;
            //VisualStateManager.GoToState(this, "FadeIn", true);
        }


        // unused functions that show the inner math of Deep Zoom
        public Rect getImageRect()
        {
            return new Rect(-msi.ViewportOrigin.X / msi.ViewportWidth, -msi.ViewportOrigin.Y / msi.ViewportWidth, 1 / msi.ViewportWidth, 1 / msi.ViewportWidth * msi.AspectRatio);
        }

        public Rect ZoomAboutPoint(Rect img, double zAmount, Point pt)
        {
            return new Rect(pt.X + (img.X - pt.X) / zAmount, pt.Y + (img.Y - pt.Y) / zAmount, img.Width / zAmount, img.Height / zAmount);
        }

        public void LayoutDZI(Rect rect)
        {
            double ar = msi.AspectRatio;
            msi.ViewportWidth = 1 / rect.Width;
            msi.ViewportOrigin = new Point(-rect.Left / rect.Width, -rect.Top / rect.Width);
        }

        private void LeftInClick(object sender, RoutedEventArgs e)
        {
            this.msi.ViewportOrigin = new Point(this.msi.ViewportOrigin.X + .05,this.msi.ViewportOrigin.Y);
        }

        private void RightInClick(object sender, RoutedEventArgs e)
        {
            this.msi.ViewportOrigin = new Point(this.msi.ViewportOrigin.X - .05, this.msi.ViewportOrigin.Y);
  
        }

        private void UpInClick(object sender, RoutedEventArgs e)
        {
            this.msi.ViewportOrigin = new Point(this.msi.ViewportOrigin.X, this.msi.ViewportOrigin.Y + .05);
  
        }

        private void DownInClick(object sender, RoutedEventArgs e)
        {
            this.msi.ViewportOrigin = new Point(this.msi.ViewportOrigin.X, this.msi.ViewportOrigin.Y - .05);
  
        }

        private void closeDesc_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void closeDesc_Click(object sender, RoutedEventArgs e)
        {
            canvasDescription.Opacity = 0;
        }
        private string GetNameofWonder(int index)
        {
            string nameWonder="";
            if (index == 0)
                nameWonder = "Colosseum in Rome";
            else if (index == 1)
                nameWonder = "Golden Gate Bridge";
            else if (index == 2)
                nameWonder = "Grand Canyon";
            else if (index == 3)
                nameWonder = "The Great Wall of China";
            else if (index == 4)
                nameWonder = "Great Pyramid of Giza";
            else if (index == 5)
                nameWonder = "Mount Everest";
            else if (index == 6)
                nameWonder = "Taj Mahal";
            else if (index == 7)
                nameWonder = "Wonders of the World";

            return nameWonder;

        }

        private void GoInfoClick(object sender, RoutedEventArgs e)
        {
            if (canvasDescription.Opacity == .75)
                canvasDescription.Opacity = 0;
            else
            {
                
                canvasDescription.Opacity = .75;
            }
        }

        private string GetDescriptionWonder(int index)
        {
            string descWonder = "";
            if (index == 0)
                descWonder = "The Colosseum or Roman Coliseum, originally the Flavian Amphitheatre (Latin: Amphitheatrum Flavium, Italian Anfiteatro Flavio or Colosseo), is an elliptical amphitheatre in the center of the city of Rome, Italy, the largest ever built in the Roman Empire. It is one of the greatest works of Roman architecture and Roman engineering.";
            else if (index == 1)
                descWonder = "The Golden Gate Bridge is a suspension bridge spanning the Golden Gate, the opening of the San Francisco Bay onto the Pacific Ocean. As part of both U.S. Route 101 and State Route 1, it connects the city of San Francisco on the northern tip of the San Francisco Peninsula to Marin County.";
            else if (index == 2)
                descWonder = "The Grand Canyon is a steep-sided gorge carved by the Colorado River in the United States state of Arizona. It is largely contained within the Grand Canyon National Park — one of the first national parks in the United States. President Theodore Roosevelt was a major proponent of preservation of the Grand Canyon area, and visited on numerous occasions to hunt and enjoy the scenery.";
            else if (index == 3)
                descWonder = "The Great Wall of China (pinyin: Chángchéng; literally 'long city/fortress') or (pinyin: Wànlǐ Chángchéng; literally 'The long wall of 10,000 Li (里)') is a series of stone and earthen fortifications in China, built, rebuilt, and maintained between the 5th century BC and the 16th century to protect the northern borders of the Chinese Empire from Xiongnu attacks during the rule of successive dynasties. ";
            else if (index == 4)
                descWonder = "The Great Pyramid of Giza, also called Khufu's Pyramid or the Pyramid of Khufu, and Pyramid of Cheops, is the oldest and largest of the three pyramids in the Giza Necropolis bordering what is now Cairo, Egypt, and is the only remaining member of the Seven Wonders of the Ancient World.";
            else if (index == 5)
                descWonder = "Mount Everest, also called Sagarmatha (meaning Head of the Sky) or Chomolungma, Qomolangma or Zhumulangma ( in Chinese: 珠穆朗玛峰 Zhūmùlǎngmǎ Fēng) is the highest mountain on Earth, as measured by the height of its summit above sea level, which is 8,848 metres (29,029 ft). The mountain, which is part of the Himalaya range in High Asia, is located on the border between Sagarmatha Zone, Nepal, and Tibet, China.";
            else if (index == 6)
                descWonder = "The Taj Mahal (pronounced /tɑdʒ mə'hɑl/ Persian/Urdu: تاج محل) is a mausoleum located in Agra, India, built by Mughal Emperor Shah Jahan in memory of his favorite wife, Mumtaz Mahal.";
            else if (index == 7)
                descWonder = "Colosseum in Rome \nGrand Canyon\nTaj Mahal\nThe Great Wall of China\nGreat Pyramid of Giza\nGolden Gate Bridge\nMount Everest";

            return descWonder;
        }
           
    }
}