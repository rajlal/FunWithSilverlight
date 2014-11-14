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

namespace HTMLBridge
{
    public partial class PageAccessHTML : UserControl
    {
        public PageAccessHTML()
        {
            InitializeComponent();
        }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            //''HtmlPage.Document.DocumentElement
            HtmlDocument elementDoc = HtmlPage.Document;
            HtmlElement elementBody = (HtmlElement)HtmlPage.Document.Body;

            txtHTMLDocument.Text += "Title: " + elementDoc.GetProperty("title").ToString() + "\n";
            txtHTMLDocument.Text += "Anchors: " + ((ScriptObjectCollection)elementDoc.GetProperty("anchors")).Count.ToString() + "\n";
            txtHTMLDocument.Text += "Applets: " + ((ScriptObjectCollection)elementDoc.GetProperty("applets")).Count.ToString() + "\n";
            txtHTMLDocument.Text += "Domain: " + elementDoc.GetProperty("domain").ToString() + "\n";
            txtHTMLDocument.Text += "Forms: " + ((ScriptObjectCollection)elementDoc.GetProperty("forms")).Count.ToString() + "\n";
            txtHTMLDocument.Text += "Images: " + ((ScriptObjectCollection)elementDoc.GetProperty("images")).Count.ToString() + "\n";
            txtHTMLDocument.Text += "Links: " + ((ScriptObjectCollection)elementDoc.GetProperty("links")).Count.ToString() + "\n";
            txtHTMLDocument.Text += "Referrer: " + elementDoc.GetProperty("referrer").ToString() + "\n";

            txtHTMLBody.Text += "Background: " + elementBody.GetProperty("background").ToString() + "\n";
            txtHTMLBody.Text += "BG Color: " + elementBody.GetProperty("bgColor").ToString() + "\n";
            txtHTMLBody.Text += "Text Color: " + elementBody.GetProperty("text").ToString() + "\n";
            txtHTMLBody.Text += "Link color: " + elementBody.GetProperty("link").ToString() + "\n";
            txtHTMLBody.Text += "Visited Link: " + elementBody.GetProperty("vLink").ToString() + "\n";
            txtHTMLBody.Text += "Active Link: " + elementBody.GetProperty("aLink").ToString() + "\n";

        }
    }
}
