Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes
Imports System.Windows.Browser

' Namespace HelloBrowser

    Public Partial Class PageBrowserInfo
        Inherits UserControl
        Public Sub New()
            InitializeComponent()
        End Sub '   New


        private Sub GetBrowser(sender As Object, e As MouseButtonEventArgs)

            txtBrowserInfo.Text = "Name:" + HtmlPage.BrowserInformation.Name
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   GetBrowser

        private Sub GetOS(sender As Object, e As MouseButtonEventArgs)

            txtBrowserInfo.Text = "Platform:" + HtmlPage.BrowserInformation.Platform
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   GetOS

        private Sub GetBrowserVersion(sender As Object, e As MouseButtonEventArgs)

            txtBrowserInfo.Text = "Version:" + HtmlPage.BrowserInformation.BrowserVersion.ToString()
            txtBrowserInfo.Text += "\n\nMajor:" + HtmlPage.BrowserInformation.BrowserVersion.Major.ToString()
            txtBrowserInfo.Text += "\nMinor:" + HtmlPage.BrowserInformation.BrowserVersion.Minor.ToString()
            txtBrowserInfo.Text += "\nBuild:" + HtmlPage.BrowserInformation.BrowserVersion.Build.ToString()
            txtBrowserInfo.Text += "\nRevision:" + HtmlPage.BrowserInformation.BrowserVersion.Revision.ToString()
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   GetBrowserVersion

        private Sub CheckCookieEnabled(sender As Object, e As MouseButtonEventArgs)

            txtBrowserInfo.Text = "Cookies:" + HtmlPage.BrowserInformation.CookiesEnabled.ToString()
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   CheckCookieEnabled


        private Sub GetAddressURL(sender As Object, e As MouseButtonEventArgs)

            txtBrowserInfo.Text = "URL:" + HtmlPage.Document.DocumentUri.ToString()
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   GetAddressURL

        private Sub CheckUserAgent(sender As Object, e As MouseButtonEventArgs)

            txtBrowserInfo.Text = "UserAgent:" + HtmlPage.BrowserInformation.UserAgent
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   CheckUserAgent


        private Sub CheckPopupWindow(sender As Object, e As MouseButtonEventArgs)

            txtBrowserInfo.Text = "Popup Window:" + HtmlPage.IsPopupWindowAllowed.ToString()
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   CheckPopupWindow


        private Sub GetHost(sender As Object, e As MouseButtonEventArgs)

            txtBrowserInfo.Text = "Host:" + HtmlPage.Document.DocumentUri.Host.ToString()
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   GetHost
    End Class   '   PageBrowserInfo
' End Namespace   '   HelloBrowser
' ..\Project_05\ExtendBrowser\1-HelloBrowser\Pages\1-PageBrowserInfo.xaml.cs
