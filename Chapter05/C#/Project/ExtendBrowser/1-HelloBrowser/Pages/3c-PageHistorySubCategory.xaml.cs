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
    public partial class PageHistorySubCategory : UserControl
    {
        
        public PageHistorySubCategory()
        {
            InitializeComponent();
        }


        private void Home_Click(object sender, RoutedEventArgs e)
        {
            string documentUri = HtmlPage.Document.DocumentUri.OriginalString;
            documentUri = documentUri.Split('?')[0];
            HtmlWindow h = HtmlPage.Window;
            h.Navigate(new Uri(documentUri));
        }
        private void Category_Click(object sender, RoutedEventArgs e)
        {
             
            string documentUri = HtmlPage.Document.DocumentUri.OriginalString;
            documentUri = documentUri.Split('?')[0];
            HtmlWindow h = HtmlPage.Window;
            h.Navigate(new Uri(documentUri + "?Page=Category&Category=" + HtmlPage.Document.QueryString["Category"].ToString()));
   
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

        private void URL_Category_MouseEnter(object sender, MouseEventArgs e)
        {
            string documentUri = HtmlPage.Document.DocumentUri.OriginalString;
            documentUri = documentUri.Split('?')[0];
            StatusBar.Text = "?Page=Category&Category=" + HtmlPage.Document.QueryString["Category"].ToString();
        }

        private void URL_Category_MouseLeave(object sender, MouseEventArgs e)
        {
            StatusBar.Text = "";
        }


        private void On_Loaded(object sender, RoutedEventArgs e)
        {
            if (HtmlPage.Document.QueryString.Keys.Contains("SubCategory"))
            {
                txtSubCategory.Text += " " + HtmlPage.Document.QueryString["SubCategory"].ToString();
                txtCategory.Content += " " + HtmlPage.Document.QueryString["Category"].ToString();
            }
        }

     
      
    }
}
