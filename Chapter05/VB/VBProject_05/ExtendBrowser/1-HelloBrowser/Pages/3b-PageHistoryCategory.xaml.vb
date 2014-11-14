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

Partial Public Class PageHistoryCategory
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub URL_Click(sender As Object, e As RoutedEventArgs)

        Dim h As HtmlWindow = HtmlPage.Window

        Dim hp As HyperlinkButton = CType(sender, HyperlinkButton)

        Dim documentUri As String = HtmlPage.Document.DocumentUri.OriginalString

        documentUri = documentUri.Split("?"c)(0)

        h.Navigate(New Uri(documentUri +
                           "?Page=SubCategory&Category=" +
                           HtmlPage.Document.QueryString("Category").ToString() +
                           "&SubCategory=" +
                           hp.Content.ToString().Substring(hp.Content.ToString().Length - 1)))
    End Sub '   URL_Click

    Private Sub Home_Click(sender As Object, e As RoutedEventArgs)

        Dim documentUri As String = HtmlPage.Document.DocumentUri.OriginalString

        documentUri = documentUri.Split("?"c)(0)

        Dim h As HtmlWindow = HtmlPage.Window

        h.Navigate(New Uri(documentUri))
    End Sub '   Home_Click

    Private Sub URL_MouseLeave(sender As Object, e As MouseEventArgs)

        StatusBar.Text = ""
    End Sub '   URL_MouseLeave

    Private Sub URL_MouseEnter(sender As Object, e As MouseEventArgs)

        Dim documentUri As String = HtmlPage.Document.DocumentUri.OriginalString

        documentUri = documentUri.Split("?"c)(0)
        StatusBar.Text = "Home"
    End Sub '   URL_MouseEnter

    Private Sub URL_SubCategory_MouseLeave(sender As Object, e As MouseEventArgs)

        StatusBar.Text = ""
    End Sub '   URL_SubCategory_MouseLeave

    Private Sub URL_SubCategory_MouseEnter(sender As Object, e As MouseEventArgs)

        Dim documentUri As String = HtmlPage.Document.DocumentUri.OriginalString

        Dim hp As HyperlinkButton = CType(sender, HyperlinkButton)

        documentUri = documentUri.Split("?"c)(0)
        StatusBar.Text = "?Page=SubCategory&Category=" + HtmlPage.Document.QueryString("Category").ToString() + "&SubCategory=" + hp.Content.ToString().Substring(hp.Content.ToString().Length - 1)
    End Sub '   URL_SubCategory_MouseEnter

    Private Sub On_Loaded(sender As Object, e As RoutedEventArgs)

        If (HtmlPage.Document.QueryString.Keys.Contains("Category")) Then
            txtCategory.Text += " " + HtmlPage.Document.QueryString("Category").ToString()
        End If
    End Sub '   On_Loaded
End Class   '   PageHistoryCategory
' End Namespace   '   HelloBrowser
' ..\Project_05\ExtendBrowser\1-HelloBrowser\Pages\3b-PageHistoryCategory.xaml.cs
