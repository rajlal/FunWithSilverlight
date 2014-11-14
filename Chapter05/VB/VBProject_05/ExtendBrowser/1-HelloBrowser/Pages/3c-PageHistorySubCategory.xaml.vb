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

Partial Public Class PageHistorySubCategory
    Inherits UserControl

    Public Sub New()
        InitializeComponent()
    End Sub '   New

    Private Sub Home_Click(sender As Object, e As RoutedEventArgs)

        Dim documentUri As String = HtmlPage.Document.DocumentUri.OriginalString

        documentUri = documentUri.Split("?"c)(0)

        Dim h As HtmlWindow = HtmlPage.Window

        h.Navigate(New Uri(documentUri))
    End Sub '   Home_Click

    Private Sub Category_Click(sender As Object, e As RoutedEventArgs)

        Dim documentUri As String = HtmlPage.Document.DocumentUri.OriginalString

        documentUri = documentUri.Split("?"c)(0)

        Dim h As HtmlWindow = HtmlPage.Window

        h.Navigate(New Uri(documentUri + "?Page=Category&Category=" + HtmlPage.Document.QueryString("Category").ToString()))
    End Sub '   Category_Click

    Private Sub URL_MouseLeave(sender As Object, e As MouseEventArgs)

        StatusBar.Text = ""
    End Sub '   URL_MouseLeave

    Private Sub URL_MouseEnter(sender As Object, e As MouseEventArgs)

        Dim documentUri As String = HtmlPage.Document.DocumentUri.OriginalString

        documentUri = documentUri.Split("?"c)(0)
        StatusBar.Text = "Home"
    End Sub '   URL_MouseEnter

    Private Sub URL_Category_MouseEnter(sender As Object, e As MouseEventArgs)

        Dim documentUri As String = HtmlPage.Document.DocumentUri.OriginalString

        documentUri = documentUri.Split("?"c)(0)
        StatusBar.Text = "?Page=Category&Category=" + HtmlPage.Document.QueryString("Category").ToString()
    End Sub '   URL_Category_MouseEnter

    Private Sub URL_Category_MouseLeave(sender As Object, e As MouseEventArgs)

        StatusBar.Text = ""
    End Sub '   URL_Category_MouseLeave

    Private Sub On_Loaded(sender As Object, e As RoutedEventArgs)

        If (HtmlPage.Document.QueryString.Keys.Contains("SubCategory")) Then
            txtSubCategory.Text += " " + HtmlPage.Document.QueryString("SubCategory").ToString()
            txtCategory.Content = CType(txtCategory.Content, String) + " " + HtmlPage.Document.QueryString("Category").ToString()
        End If
    End Sub '   On_Loaded
End Class   '   PageHistorySubCategory
' End Namespace   '   HelloBrowser
' ..\Project_05\ExtendBrowser\1-HelloBrowser\Pages\3c-PageHistorySubCategory.xaml.cs
