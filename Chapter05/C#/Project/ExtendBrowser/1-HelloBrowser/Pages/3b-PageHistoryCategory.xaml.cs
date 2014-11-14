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
    public partial class PageHistoryCategory : UserControl
    {
        
        public PageHistoryCategory()
        {
            InitializeComponent();
        }

      

        private void URL_Click(object sender, RoutedEventArgs e)
        {
            HtmlWindow h = HtmlPage.Window;
            HyperlinkButton hp = (HyperlinkButton)sender;

            string documentUri = HtmlPage.Document.DocumentUri.OriginalString;
            documentUri = documentUri.Split('?')[0];

            h.Navigate(new Uri(documentUri + "?Page=SubCategory&Category=" + HtmlPage.Document.QueryString["Category"].ToString() + "&SubCategory=" + hp.Content.ToString().Substring(hp.Content.ToString().Length-1)));

        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            string documentUri = HtmlPage.Document.DocumentUri.OriginalString;
            documentUri = documentUri.Split('?')[0];
            HtmlWindow h = HtmlPage.Window;
            h.Navigate(new Uri(documentUri));
        }
        private void URL_MouseLeave(object sender, MouseEventArgs e)
        {

            StatusBar.Text = "";
        }

        private void URL_MouseEnter(object sender, MouseEventArgs e)
        {
            string documentUri = HtmlPage.Document.DocumentUri.OriginalString;
            documentUri = documentUri.Split('?')[0];
            StatusBar.Text = "Home";
        }



        private void URL_SubCategory_MouseLeave(object sender, MouseEventArgs e)
        {
            StatusBar.Text = "";
        }

        private void URL_SubCategory_MouseEnter(object sender, MouseEventArgs e)
        {
            string documentUri = HtmlPage.Document.DocumentUri.OriginalString;
            HyperlinkButton hp = (HyperlinkButton)sender;

            documentUri = documentUri.Split('?')[0];
            StatusBar.Text = "?Page=SubCategory&Category=" + HtmlPage.Document.QueryString["Category"].ToString() + "&SubCategory=" + hp.Content.ToString().Substring(hp.Content.ToString().Length - 1);
        }


        private void On_Loaded(object sender, RoutedEventArgs e)
        {
            if (HtmlPage.Document.QueryString.Keys.Contains("Category"))
            {
                txtCategory.Text += " " + HtmlPage.Document.QueryString["Category"].ToString();
            }
            
        }

    }
}
