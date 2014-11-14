using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace InputEvents
{
    public partial class PageKeyboard : UserControl
    {
        public PageKeyboard()
        {
            InitializeComponent();
        }
        private void WaterMarkTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            WaterMarkTitle.Text = "";
            WaterMarkTitle.Foreground = new SolidColorBrush(Colors.Black);
            UpdateLogTable("GotFocus: WaterMarkTitle");
        }
        private void NumberTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            UpdateLogTable("GotFocus: NumberTextBox");
        }
        private void Button_GotFocus(object sender, RoutedEventArgs e)
        {
            UpdateLogTable("GotFocus: Button");
        }
        private void WaterMarkTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            if (WaterMarkTitle.Text == "")
            { 
                WaterMarkTitle.Text = "Enter Title...";
                WaterMarkTitle.Foreground = new SolidColorBrush(Colors.Gray);
            }
            UpdateLogTable("LostFocus: WaterMarkTitle");
        }
        private void NumberTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateLogTable("LostFocus: NumberTextBox");
        }
        private void Button_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateLogTable("LostFocus: Button");
        }

        private void NumberTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key.ToString()=="D0")||(e.Key.ToString()=="D1")||(e.Key.ToString()=="D2")||(e.Key.ToString()=="D3")||(e.Key.ToString()=="D4")||(e.Key.ToString()=="D5")||(e.Key.ToString()=="D6")||(e.Key.ToString()=="D7")||(e.Key.ToString()=="D8")||(e.Key.ToString()=="D9"))
                e.Handled = false;
            else
            e.Handled = true;
        }
        private void KeyBox_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateLogTable("Keyup: " + e.Key.ToString());
            if (Keyboard.Modifiers.ToString() != "none")
                UpdateLogTable("Keyboard: " + Keyboard.Modifiers.ToString());
            LogTable.Text = LogTable.Text + "\n----------------------------";
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
