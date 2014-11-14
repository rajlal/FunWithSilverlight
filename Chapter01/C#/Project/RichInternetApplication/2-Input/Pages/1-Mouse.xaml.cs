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
    public partial class PageMouse : UserControl
    {
        public PageMouse()
        {
            InitializeComponent();
            this.YellowBox.MouseLeftButtonDown += new MouseButtonEventHandler(DynamicYellowBox_MouseLeftButtonDown);
            this.YellowBox.MouseLeftButtonUp += new MouseButtonEventHandler(DynamicYellowBox_MouseLeftButtonUp);

        }
        private void WhiteCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            UpdateStatus("MouseEnter: WhiteBlock");
        }
        private void WhiteCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            UpdateStatus("MouseLeave: WhiteBlock");
        }
        private void WhiteCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            StatusXPosition.Text = e.GetPosition((UIElement)sender).X.ToString();// MouseButtonEventArgs position relative to the Item
            StatusYPosition.Text = e.GetPosition((UIElement)sender).Y.ToString();

        }
        private void WhiteCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateLogTable("LeftButtonDown: WhiteBlock");
        }
        private void WhiteCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateLogTable("LeftButtonUp: WhiteBlock");
            LogTable.Text = LogTable.Text  + "\n----------------------------" ;
        }

        private void RedBox_MouseEnter(object sender, MouseEventArgs e)
        {
            UpdateStatus("MouseEnter: RedBox");
        }
        private void RedBox_MouseLeave(object sender, MouseEventArgs e)
        {
            UpdateStatus("MouseLeave: RedBox"); 
        }
        private void RedBox_MouseMove(object sender, MouseEventArgs e)
        {
            StatusXPosition.Text = e.GetPosition(null).X.ToString();// MouseButtonEventArgs position relative to the Silverlight Plug-in
            StatusYPosition.Text = e.GetPosition(null).Y.ToString();
        }
        private void RedBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateLogTable("LeftButtonDown: RedBox");
        }
        private void RedBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateLogTable("LeftButtonUp: RedBox");
        }

        private void GreenBox_MouseEnter(object sender, MouseEventArgs e)
        {
            UpdateStatus("MouseEnter: GreenBox");
        }
        private void GreenBox_MouseLeave(object sender, MouseEventArgs e)
        {
            UpdateStatus("MouseLeave: GreenBox");
            
        }
        private void GreenBox_MouseMove(object sender, MouseEventArgs e)
        {
            StatusXPosition.Text = e.GetPosition(null).X.ToString();// MouseButtonEventArgs position relative to the Silverlight Plug-in
            StatusYPosition.Text = e.GetPosition(null).Y.ToString();
        }
        private void GreenBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateLogTable("LeftButtonDown: GreenBox");
            e.Handled = true;      
        }
        private void GreenBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateLogTable("LeftButtonUp: GreenBox");
            e.Handled = true;
            LogTable.Text = LogTable.Text + "\n----------------------------";
        }

        private void BlueBox_MouseEnter(object sender, MouseEventArgs e)
        {
            UpdateStatus("MouseEnter: BlueBox");
        }
        private void BlueBox_MouseLeave(object sender, MouseEventArgs e)
        {
            UpdateStatus("MouseLeave: BlueBox");
        }
        private void YellowBox_MouseEnter(object sender, MouseEventArgs e)
        {
            UpdateStatus("MouseEnter: YellowBox");
        }
        private void YellowBox_MouseLeave(object sender, MouseEventArgs e)
        {
            UpdateStatus("MouseLeave: YellowBox");

        }

        private void DynamicYellowBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateLogTable("LeftButtonDown: YellowBox");
        }
        private void DynamicYellowBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UpdateLogTable("LeftButtonUp: YellowBox");
        }
        
        private void ResetLogTable()
        {
            LogTable.Text = "";
        }
        private void UpdateStatus(string EventName)
        {
            StatusBar.Text = EventName ;
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
