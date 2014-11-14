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

Partial Public Class PageQueryString
    Inherits UserControl

    Public Sub New()
        InitializeComponent()
    End Sub '   New

    Private Sub GetAddressURL()

        If (HtmlPage.Document.QueryString.Count > 0) Then
            txtQueryString.Text = "Query String Count:" + HtmlPage.Document.QueryString.Count.ToString() + "\n"
        End If

        If (HtmlPage.Document.QueryString.Keys.Contains("Category")) Then
            txtQueryString.Text += "Category: " + HtmlPage.Document.QueryString("Category").ToString() + "\n"
        End If


        If (HtmlPage.Document.QueryString.Keys.Contains("SubCategory")) Then
            txtQueryString.Text += "Subcategory: " + HtmlPage.Document.QueryString("SubCategory").ToString() + "\n"
        End If
    End Sub '   GetAddressURL

    Private Sub Canvas_Loaded(sender As Object, e As RoutedEventArgs)

        GetAddressURL()
    End Sub '   Canvas_Loaded

    Private Sub URL_Click(sender As Object, e As RoutedEventArgs)

        Dim h As HtmlWindow = HtmlPage.Window

        Dim documentUri As String = HtmlPage.Document.DocumentUri.OriginalString

        documentUri = documentUri.Split({"?"c})(0)

        h.Navigate(New Uri(documentUri))
    End Sub '   URL_Click

    Private Sub URL_Category_Click(sender As Object, e As RoutedEventArgs)

        Dim h As HtmlWindow = HtmlPage.Window

        Dim documentUri As String = HtmlPage.Document.DocumentUri.OriginalString

        documentUri = documentUri.Split({"?"c})(0)
        h.Navigate(New Uri(documentUri + "?Category=Books"))
    End Sub '   URL_Category_Click

    Private Sub URL_SubCategory_Click(sender As Object, e As RoutedEventArgs)

        Dim h As HtmlWindow = HtmlPage.Window
        Dim documentUri As String = HtmlPage.Document.DocumentUri.OriginalString

        documentUri = documentUri.Split({"?"c})(0)
        h.Navigate(New Uri(documentUri + "?Category=Books&SubCategory=SilverlightHowTo"))
    End Sub '   URL_SubCategory_Click

    Private Sub URL_MouseLeave(sender As Object, e As MouseEventArgs)

        StatusBar.Text = ""
    End Sub '   URL_MouseLeave

    Private Sub URL_MouseEnter(sender As Object, e As MouseEventArgs)

        Dim documentUri As String = HtmlPage.Document.DocumentUri.OriginalString

        documentUri = documentUri.Split({"?"c})(0)
        StatusBar.Text = "Home"
    End Sub '   URL_MouseEnter

    Private Sub URL_Category_MouseEnter(sender As Object, e As MouseEventArgs)

        Dim documentUri As String = HtmlPage.Document.DocumentUri.OriginalString

        documentUri = documentUri.Split({"?"c})(0)
        StatusBar.Text = "?Category=Books"
    End Sub '   URL_Category_MouseEnter

    Private Sub URL_Category_MouseLeave(sender As Object, e As MouseEventArgs)

        StatusBar.Text = ""
    End Sub '   URL_Category_MouseLeave

    Private Sub URL_SubCategory_MouseLeave(sender As Object, e As MouseEventArgs)

        StatusBar.Text = ""
    End Sub '   URL_SubCategory_MouseLeave

    Private Sub URL_SubCategory_MouseEnter(sender As Object, e As MouseEventArgs)

        Dim documentUri As String = HtmlPage.Document.DocumentUri.OriginalString

        documentUri = documentUri.Split({"?"c})(0)
        StatusBar.Text = "?Category=Books&SubCategory=SilverlightHowTo"
    End Sub '   URL_SubCategory_MouseEnter
End Class   '   PageQueryString
' End Namespace   '   HelloBrowser
' ..\Project_05\ExtendBrowser\1-HelloBrowser\Pages\2-PageQueryString.xaml.cs
