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

namespace DeepZoomProject
{
    public partial class Page : UserControl
    {
        //
        // Based on prior work done by Lutz Gerhard, Peter Blois, and Scott Hanselman
        //
        bool BoundryFlag = true;
        bool ZoomBoundFlag = true;
        double zoom = 1;
        bool duringDrag = false;
        bool mouseDown = false;
        Point lastMouseDownPos = new Point();
        Point lastMousePos = new Point();
        Point lastMouseViewPort = new Point();


        public double ZoomFactor
        {
            get { return zoom; }
            set { 
                zoom = value;
            }
        }

        public Page()
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
                }
                duringDrag = false;
                mouseDown = false;

                msi.ReleaseMouseCapture();
                updateStatus();
            };

            this.MouseMove += delegate(object sender, MouseEventArgs e)
            {
                lastMousePos = e.GetPosition(msi);
                if (mouseDown && !duringDrag)
                {
                    duringDrag = true;
                    double w = msi.ViewportWidth;
                    Point o = new Point(msi.ViewportOrigin.X, msi.ViewportOrigin.Y);
                    msi.UseSprings = (bool)UseSprings.IsChecked; 
                    msi.ViewportOrigin = new Point(o.X, o.Y);
                    msi.ViewportWidth = w;
                    zoom = 1/w;
                    msi.UseSprings = (bool)UseSprings.IsChecked; 
                    updateStatus();
                }

                if (duringDrag)
                {
                    Point newPoint = lastMouseViewPort;
                    newPoint.X += (lastMouseDownPos.X - lastMousePos.X) / msi.ActualWidth * msi.ViewportWidth;
                    newPoint.Y += (lastMouseDownPos.Y - lastMousePos.Y) / msi.ActualWidth * msi.ViewportWidth;

                  

                    if (BoundryFlag)
                    {
                        if (newPoint.X < 0) newPoint.X = 0;
                        if (newPoint.Y < 0) newPoint.Y = 0;

                        double MaxX = 1 - (1 / this.zoom);
                        double MaxY = (1 - (1 / this.zoom))/2.0; // Since Height = 1/2 Width
                        if (newPoint.X > MaxX) newPoint.X = MaxX;
                        if (newPoint.Y > MaxY) newPoint.Y = MaxY;

                    }
                  
                   

                    msi.ViewportOrigin = newPoint;
                    updateStatus();
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

        void msi_ImageOpenSucceeded(object sender, RoutedEventArgs e)
        {
            //If collection, this gets you a list of all of the MultiScaleSubImages
            //
            //foreach (MultiScaleSubImage subImage in msi.SubImages)
            //{
            //    // Do something
            //}

            msi.ViewportWidth = 1;
        }

        void msi_Loaded(object sender, RoutedEventArgs e)
        {
            // Hook up any events you want when the image has successfully been opened
        }

        private void Zoom(double newzoom, Point p)
        {
            if (ZoomBoundFlag)
            {
                if (newzoom < 1)
                {
                    newzoom = 1;
                }
                if (newzoom > 11)
                {
                    newzoom = 11;
                }
            }
              

                msi.ZoomAboutLogicalPoint(newzoom / zoom, p.X, p.Y);
                zoom = newzoom;
                updateStatus();
            
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
            ResetStatus();
        }
         private void ResetStatus()
        {
            
                StatusZoom.Text = "Zoom:100%";
                StatusViewport.Text = "Viewport Origin:0.00/0.00";
                StatusViewportWidth.Text = "ViewportWidth:1";
            }
        private void updateStatus()
        {
                double ZoomFactorPercentage = ZoomFactor * 100;
                StatusZoom.Text = "Zoom:" + string.Format("{0:N}", ZoomFactorPercentage) + "%";
                StatusViewport.Text = "Viewport Origin:" + string.Format("{0:N}", this.msi.ViewportOrigin.X) + "/" + string.Format("{0:N}", this.msi.ViewportOrigin.Y);
                StatusViewportWidth.Text = "ViewportWidth:" + string.Format("{0:N}", this.msi.ViewportWidth);
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
                msi.Height = 300;
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

        private void CheckBound_Click(object sender, RoutedEventArgs e)
        {
            BoundryFlag =(bool) CheckBound.IsChecked;
        }

        private void TextCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void ZoomBound_Click(object sender, RoutedEventArgs e)
        {
            ZoomBoundFlag = (bool)ZoomBound.IsChecked;
        }

        private void UseSprings_Click(object sender, RoutedEventArgs e)
        {
            this.msi.UseSprings = (bool)UseSprings.IsChecked;
        }

        private void LeftInClick(object sender, RoutedEventArgs e)
        {
            this.msi.ViewportOrigin = new Point(this.msi.ViewportOrigin.X + .05,this.msi.ViewportOrigin.Y);
        }

        private void RightInClick(object sender, RoutedEventArgs e)
        {
            this.msi.ViewportOrigin = new Point(this.msi.ViewportOrigin.X - .05, this.msi.ViewportOrigin.Y);
  
        }
    }
}