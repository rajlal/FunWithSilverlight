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
    public partial class PageBrowserInfo : UserControl
    {
        public PageBrowserInfo()
        {
            InitializeComponent();
        }

        private void GetBrowser(object sender, MouseButtonEventArgs e)
        {
            txtBrowserInfo.Text = "Name:" + HtmlPage.BrowserInformation.Name;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void GetOS(object sender, MouseButtonEventArgs e)
        {
            txtBrowserInfo.Text = "Platform:" + HtmlPage.BrowserInformation.Platform;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void GetBrowserVersion(object sender, MouseButtonEventArgs e)
        {
            txtBrowserInfo.Text = "Version:" + HtmlPage.BrowserInformation.BrowserVersion.ToString();
            txtBrowserInfo.Text += "\n\nMajor:" + HtmlPage.BrowserInformation.BrowserVersion.Major.ToString() ;
            txtBrowserInfo.Text += "\nMinor:" + HtmlPage.BrowserInformation.BrowserVersion.Minor.ToString() ;
            txtBrowserInfo.Text += "\nBuild:" + HtmlPage.BrowserInformation.BrowserVersion.Build.ToString();
            txtBrowserInfo.Text += "\nRevision:" + HtmlPage.BrowserInformation.BrowserVersion.Revision.ToString();
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void CheckCookieEnabled(object sender, MouseButtonEventArgs e)
        {
            txtBrowserInfo.Text = "Cookies:" + HtmlPage.BrowserInformation.CookiesEnabled.ToString() ;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }

        private void GetAddressURL(object sender, MouseButtonEventArgs e)
        {
            txtBrowserInfo.Text = "URL:" + HtmlPage.Document.DocumentUri.ToString();
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
        private void CheckUserAgent(object sender, MouseButtonEventArgs e)
        {
            txtBrowserInfo.Text = "UserAgent:" + HtmlPage.BrowserInformation.UserAgent;
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();     
        }

        private void CheckPopupWindow(object sender, MouseButtonEventArgs e)
        {
            txtBrowserInfo.Text = "Popup Window:" + HtmlPage.IsPopupWindowAllowed.ToString();
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }

        private void GetHost(object sender, MouseButtonEventArgs e)
        {
            txtBrowserInfo.Text = "Host:" + HtmlPage.Document.DocumentUri.Host.ToString();
            StatusBar.Text = ToolTipService.GetToolTip((TextBlock)sender).ToString();
        }
    }
}
