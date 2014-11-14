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
namespace InputEvents
{
    public partial class PageMouseCapture : UserControl
    {
        bool isMouseCaptured;
        double mouseTop;
        double mouseLeft;
        public PageMouseCapture()
        {
            InitializeComponent();
        }
        private void ResetLogTable()
        {
            LogTable.Text = "";
        }
        private void Box_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateLogTable("LeftButtonDown: Box");
            Rectangle myBox = sender as Rectangle;
            mouseTop = e.GetPosition(null).Y; // null to get the position relative to the Siverlght Plug-in
            mouseLeft = e.GetPosition(null).X; // null to get the position relative to the Siverlght Plug-in
            isMouseCaptured = true;
            UpdateLogTable("MouseCaptured: Box");
            myBox.CaptureMouse();
            UpdateLogTable("isMouseCaptured: True");
            UpdateLogTable("CaptureMouse()");
        }
        private void Box_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateLogTable("LeftButtonUp: Box");
            Rectangle myBox = sender as Rectangle;
            isMouseCaptured = false;
            myBox.ReleaseMouseCapture();

            UpdateLogTable("isMouseCaptured: False");
            UpdateLogTable("ReleaseMouseCapture()");
            LogTable.Text = LogTable.Text + "\n----------------------------";
            double newTop = e.GetPosition(null).Y; 
            double newLeft = e.GetPosition(null).X;
            if ((newTop  > 100)&&(newLeft>100))
            {
                myBox.SetValue(Canvas.LeftProperty, 100.0);
                myBox.SetValue(Canvas.TopProperty, 100.0);
                 }
            else if ((newTop > 100) && (newLeft < 70))
            {
                myBox.SetValue(Canvas.LeftProperty, 20.0);
                myBox.SetValue(Canvas.TopProperty, 100.0);
                  }
            else if ((newTop < 70) && (newLeft > 100))
            {
                myBox.SetValue(Canvas.LeftProperty, 100.0);
                myBox.SetValue(Canvas.TopProperty, 20.0);
                }
            else
            {
                myBox.SetValue(Canvas.LeftProperty, 20.0);
                myBox.SetValue(Canvas.TopProperty, 20.0);
            }
                mouseTop = -1;
                mouseLeft = -1;

        }
        private void Box_MouseMove(object sender, MouseEventArgs e)
        {
            StatusXPosition.Text = e.GetPosition(null).X.ToString();
            StatusYPosition.Text = e.GetPosition(null).Y.ToString();

            Rectangle myBox = sender as Rectangle;
            if (isMouseCaptured)
            {
                // Calculate the current position of the object.
                double CurrentTop = e.GetPosition(null).Y - mouseTop;
                double CurrentLeft= e.GetPosition(null).X - mouseLeft;
                double newTop = CurrentTop + (double)myBox.GetValue(Canvas.TopProperty);
                double newLeft = CurrentLeft+ (double)myBox.GetValue(Canvas.LeftProperty);
                // Set new position for the Box .
                myBox.SetValue(Canvas.TopProperty, newTop);
                myBox.SetValue(Canvas.LeftProperty, newLeft);
                // Update global top/left positions .
                mouseTop = e.GetPosition(null).Y;
                mouseLeft = e.GetPosition(null).X;
            }
        }
        private void UpdateLogTable(string EventName)
        {
            if (LogTable.Text == "")
                LogTable.Text = EventName;
            else
                LogTable.Text = LogTable.Text + "\n" + EventName;
        }
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            LogTable.Text = "";
        }
    }
}
