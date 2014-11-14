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
    public partial class PageQueryString : UserControl
    {
        
        public PageQueryString()
        {
            InitializeComponent();
        }

        private void GetAddressURL()
        {
            if(HtmlPage.Document.QueryString.Count>0)
             txtQueryString.Text = "Query String Count:" + HtmlPage.Document.QueryString.Count.ToString() + "\n";

            if (HtmlPage.Document.QueryString.Keys.Contains("Category"))
            {
                txtQueryString.Text += "Category: " + HtmlPage.Document.QueryString["Category"].ToString() + "\n";
            }
            if (HtmlPage.Document.QueryString.Keys.Contains("SubCategory"))
            {
                txtQueryString.Text += "Subcategory: " + HtmlPage.Document.QueryString["SubCategory"].ToString() + "\n";
            }   
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            GetAddressURL();
        }

        private void URL_Click(object sender, RoutedEventArgs e)
        {
            HtmlWindow h = HtmlPage.Window;

            string documentUri = HtmlPage.Document.DocumentUri.OriginalString;
            documentUri = documentUri.Split('?')[0];

            h.Navigate(new Uri(documentUri));
        }

        private void URL_Category_Click(object sender, RoutedEventArgs e)
        {
            HtmlWindow h = HtmlPage.Window;

                        string documentUri = HtmlPage.Document.DocumentUri.OriginalString;
                        documentUri = documentUri.Split('?')[0];
                        h.Navigate(new Uri(documentUri + "?Category=Books"));

        }

        private void URL_SubCategory_Click(object sender, RoutedEventArgs e)
        {
            HtmlWindow h = HtmlPage.Window;
            string documentUri = HtmlPage.Document.DocumentUri.OriginalString;
            documentUri = documentUri.Split('?')[0];
            h.Navigate(new Uri(documentUri + "?Category=Books&SubCategory=SilverlightHowTo"));
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
            StatusBar.Text =  "?Category=Books";
        }

        private void URL_Category_MouseLeave(object sender, MouseEventArgs e)
        {
            StatusBar.Text = "";
        }

        private void URL_SubCategory_MouseLeave(object sender, MouseEventArgs e)
        {
            StatusBar.Text = "";
        }

        private void URL_SubCategory_MouseEnter(object sender, MouseEventArgs e)
        {
        string documentUri = HtmlPage.Document.DocumentUri.OriginalString;
        documentUri = documentUri.Split('?')[0];
        StatusBar.Text = "?Category=Books&SubCategory=SilverlightHowTo";
        }
    }
}
