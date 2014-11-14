using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CommonControls
{
    public partial class PageRepeatButton : UserControl
    {
        static int Clicks = 0;
        public PageRepeatButton()
        {
            InitializeComponent();
      
        }     
private void RepeatButton_Click(object sender, RoutedEventArgs e)
{
    Clicks += 1;
    textValue.Text = "Value: " + Clicks.ToString();
}

private void RepeatButton_Click_Increase(object sender, RoutedEventArgs e)
{
    Clicks += 1;
    textValue.Text = "Value: " + Clicks.ToString();
}

private void RepeatButton_Click_Decrease(object sender, RoutedEventArgs e)
{
    Clicks -= 1;
    textValue.Text =  "Value: " +Clicks.ToString();
}
}
}
