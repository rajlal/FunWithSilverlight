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

' Namespace HTMLBridge

Partial Public Class PageAccessHTML
    Inherits UserControl
    Public Sub New()
        InitializeComponent()
    End Sub '   New

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        ' ''HtmlPage.Document.DocumentElement
        Dim elementDoc As HtmlDocument = HtmlPage.Document
        Dim elementBody As HtmlElement = CType(HtmlPage.Document.Body, HtmlElement)

        txtHTMLDocument.Text += "Title: " + elementDoc.GetProperty("title").ToString() + Environment.NewLine
        txtHTMLDocument.Text += "Anchors: " + CType(elementDoc.GetProperty("anchors"), ScriptObjectCollection).Count.ToString() + Environment.NewLine
        txtHTMLDocument.Text += "Applets: " + CType(elementDoc.GetProperty("applets"), ScriptObjectCollection).Count.ToString() + Environment.NewLine
        txtHTMLDocument.Text += "Domain: " + elementDoc.GetProperty("domain").ToString() + Environment.NewLine
        txtHTMLDocument.Text += "Forms: " + CType(elementDoc.GetProperty("forms"), ScriptObjectCollection).Count.ToString() + Environment.NewLine
        txtHTMLDocument.Text += "Images: " + CType(elementDoc.GetProperty("images"), ScriptObjectCollection).Count.ToString() + Environment.NewLine
        txtHTMLDocument.Text += "Links: " + CType(elementDoc.GetProperty("links"), ScriptObjectCollection).Count.ToString() + Environment.NewLine
        txtHTMLDocument.Text += "Referrer: " + elementDoc.GetProperty("referrer").ToString() + Environment.NewLine

        txtHTMLBody.Text += "Background: " + elementBody.GetProperty("background").ToString() + Environment.NewLine
        txtHTMLBody.Text += "BG Color: " + elementBody.GetProperty("bgColor").ToString() + Environment.NewLine
        txtHTMLBody.Text += "Text Color: " + elementBody.GetProperty("text").ToString() + Environment.NewLine
        txtHTMLBody.Text += "Link color: " + elementBody.GetProperty("link").ToString() + Environment.NewLine
        txtHTMLBody.Text += "Visited Link: " + elementBody.GetProperty("vLink").ToString() + Environment.NewLine
        txtHTMLBody.Text += "Active Link: " + elementBody.GetProperty("aLink").ToString() + Environment.NewLine
    End Sub '   LayoutRoot_Loaded
End Class   '   PageAccessHTML
' End Namespace   '   HTMLBridge
' ..\Project_05\ExtendBrowser\4-HTMLBridge\Pages\1-PageAccessHTML.xaml.cs
