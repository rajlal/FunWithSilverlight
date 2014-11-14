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
using System.Windows.Ink;

namespace InputEvents
{
    public partial class PageInk : UserControl
    {
        Stroke InkStroke;
        Color InkColor;
        double InkWidth;
        double InkHeight;
        public PageInk()
        {
            InitializeComponent();
            SetDefaults();
        }
        private void InkPresenterControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            InkPresenterControl.CaptureMouse();
            StylusPointCollection MyStylusPointCollection = new StylusPointCollection();
            MyStylusPointCollection.Add(e.StylusDevice.GetStylusPoints(InkPresenterControl));
            InkStroke = new Stroke(MyStylusPointCollection);
            InkPresenterControl.Strokes.Add(InkStroke);

        }
        private void InkPresenterControl_LostMouseCapture(object sender, MouseEventArgs e)
        {
            InkStroke = null;

        }
        private void InkPresenterControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (InkStroke != null)
            {
                InkStroke.DrawingAttributes.Color = InkColor;
                InkStroke.DrawingAttributes.Width = InkWidth;
                InkStroke.DrawingAttributes.Height = InkHeight;
                InkStroke.StylusPoints.Add(e.StylusDevice.GetStylusPoints(InkPresenterControl));
            }
        }
        //Set the Clip property of the inkpresenter so that the strokes
        //are contained within the boundary of the inkpresenter
        private void SetDefaults()
        {
            RectangleGeometry MyRectangleGeometry = new RectangleGeometry();
            MyRectangleGeometry.Rect = new Rect(0, 0,InkPresenterControl.ActualWidth, InkPresenterControl.ActualHeight);
            InkPresenterControl.Clip = MyRectangleGeometry;
            InkColor = Colors.Black;
            UpdateStatus("Color: Black");
            InkWidth = 2.0;
            InkHeight = 2.0;
            StatusThickness.Text = "Stroke thickness: " + InkWidth;
        }
        private void RedBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            InkColor = Colors.Red;
            UpdateStatus("Color: Red");
        }
        private void BlueBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            InkColor = Colors.Blue;
            UpdateStatus("Color: Blue");
        }
        private void GreenBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            InkColor = Colors.Green;
            UpdateStatus("Color: Green");
        }
        private void YellowBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            InkColor = Colors.Yellow;
            UpdateStatus("Color: Yellow");
        }
        private void WhiteBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            InkColor = Colors.White;
            UpdateStatus("Color: White/Erase");
        }
        private void UpdateStatus(string status)
        {
            StatusBar.Text = status;
        }
        private void BlackBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            InkColor = Colors.Black;
            UpdateStatus("Color: Black");
        }

        private void EraseBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            InkPresenterControl.Strokes.Clear();
        }
        private void IncreaseThickness_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (InkWidth < 20)
                InkWidth = InkHeight = InkWidth + 1.0;
            StatusThickness.Text = "Stroke thickness: " + InkWidth;
        }
        private void DecreaseThickness_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (InkWidth > 2)
                InkWidth = InkHeight = InkWidth - 1.0;
            StatusThickness.Text = "Stroke thickness: " + InkWidth;
        }
        private void Mirror_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StrokeCollection InkStrokeCollection = new StrokeCollection();
            foreach (Stroke stroke in InkPresenterControl.Strokes)
            {
                StylusPointCollection newcollection = new StylusPointCollection();
                foreach (StylusPoint p in stroke.StylusPoints)
                {
                    //Create the mirror image
                    StylusPoint newpoint = new StylusPoint();
                    newpoint.X = InkPresenterControl.ActualWidth - p.X;
                    newpoint.Y = p.Y;
                    newcollection.Add(newpoint);
                }
                //Add the mirror image to InkStrokeCollection
                Stroke newStroke = new Stroke(newcollection);
                newStroke.DrawingAttributes = stroke.DrawingAttributes;
                InkStrokeCollection.Add(newStroke);
            }
            foreach (Stroke s in InkStrokeCollection)
            {
                InkPresenterControl.Strokes.Add(s);
            }
        }


    }
}
