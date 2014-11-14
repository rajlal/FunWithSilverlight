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
using System.Windows.Controls.Primitives;

namespace CommonControls
{
    public partial class PagePopup : UserControl
    {
        Popup gridPopup; 
        public PagePopup()
        {
            InitializeComponent();
      
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
        }
        private void ShowPopup()
        {
            // Create a popup. 
            gridPopup = new Popup();
            // Set the Child property of Popup to an instance of MyControl. 
            gridPopup.Child = new TestPopup();
            // Set where the popup will show up on the screen. 
            gridPopup.VerticalOffset = 100;
            gridPopup.HorizontalOffset = 100;
            // Open the popup. 
            gridPopup.IsOpen = true;
        }
        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            ShowPopup();
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            gridPopup.IsOpen = false;
        }
      


    }
}
