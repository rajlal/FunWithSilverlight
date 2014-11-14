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

' Namespace AdoDataServiceClient

Partial Public Class Toc
    Inherits UserControl

    Dim h As HtmlWindow = HtmlPage.Window

    Public Sub New()
        InitializeComponent()
    End Sub '   New

    Private Sub Create_Click(sender As Object, e As RoutedEventArgs)

        h.Navigate(New Uri("1-Create.aspx", UriKind.Relative))
    End Sub '   Create_Click

    Private Sub Read_Click(sender As Object, e As RoutedEventArgs)

        h.Navigate(New Uri("2-Read.aspx", UriKind.Relative))
    End Sub '   Read_Click


    Private Sub Update_Click(sender As Object, e As RoutedEventArgs)

        h.Navigate(New Uri("3-Update.aspx", UriKind.Relative))
    End Sub '   Update_Click


    Private Sub Delete_Click(sender As Object, e As RoutedEventArgs)

        h.Navigate(New Uri("4-Delete.aspx", UriKind.Relative))
    End Sub '   Delete_Click
End Class   '   Toc
' End Namespace   '   AdoDataServiceClient
' ..\Project_07\ASPAJAX\4-AdoDataService\AdoDataServiceClient\Toc.xaml.cs
