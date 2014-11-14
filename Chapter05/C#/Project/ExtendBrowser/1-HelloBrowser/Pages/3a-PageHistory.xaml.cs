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
using System.Windows.Browser;

namespace HelloBrowser
{
    public partial class PageHistory : UserControl
    {
        
        public PageHistory()
        {
            InitializeComponent();
        }
        private void URL_Click(object sender, RoutedEventArgs e)
        {
            HtmlWindow h = HtmlPage.Window;
            HyperlinkButton hp = (HyperlinkButton)sender;

            string documentUri = HtmlPage.Document.DocumentUri.OriginalString;
            h.Navigate(new Uri(documentUri + "?Page=Category&Category=" + hp.Content.ToString().Substring(hp.Content.ToString().Length - 1)));
        }
  
        private void URL_Category_MouseEnter(object sender, MouseEventArgs e)
        {
            HyperlinkButton hp = (HyperlinkButton)sender;
            StatusBar.Text = "?Page=Category&Category=" + hp.Content.ToString().Substring(hp.Content.ToString().Length - 1);
        }

        private void URL_Category_MouseLeave(object sender, MouseEventArgs e)
        {
            StatusBar.Text = "";
        }

       
    }
}
