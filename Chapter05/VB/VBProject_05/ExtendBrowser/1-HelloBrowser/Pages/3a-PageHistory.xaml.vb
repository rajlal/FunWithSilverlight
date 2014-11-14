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

Partial Public Class PageHistory
    Inherits UserControl

    Public Sub New()
        InitializeComponent()
    End Sub '   New

    Private Sub URL_Click(sender As Object, e As RoutedEventArgs)

        Dim h As HtmlWindow = HtmlPage.Window

        Dim hp As HyperlinkButton = CType(sender, HyperlinkButton)

        Dim documentUri As String = HtmlPage.Document.DocumentUri.OriginalString

        h.Navigate(New Uri(documentUri + "?Page=Category&Category=" + hp.Content.ToString().Substring(hp.Content.ToString().Length - 1)))
    End Sub '   URL_Click

    Private Sub URL_Category_MouseEnter(sender As Object, e As MouseEventArgs)

        Dim hp As HyperlinkButton = CType(sender, HyperlinkButton)
        StatusBar.Text = "?Page=Category&Category=" + hp.Content.ToString().Substring(hp.Content.ToString().Length - 1)
    End Sub '   URL_Category_MouseEnter

    Private Sub URL_Category_MouseLeave(sender As Object, e As MouseEventArgs)

        StatusBar.Text = ""
    End Sub '   URL_Category_MouseLeave
End Class   '   PageHistory
' End Namespace   '   HelloBrowser
' ..\Project_05\ExtendBrowser\1-HelloBrowser\Pages\3a-PageHistory.xaml.cs
