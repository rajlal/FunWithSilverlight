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
Imports System.Windows.Media.Imaging

' Namespace WCFClient

Partial Public Class MainPage
    Inherits UserControl

    Dim localService As SilverlightResourceClient = New SilverlightResourceClient()

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)

        ButtonGet.Content = "Calling..."
        ButtonGet.IsEnabled = False

        Try
            AddHandler localService.GetResourceCompleted, AddressOf localService_GetResourceCompleted ')EventHandler(Of GetResourceCompletedEventArgs)(
            localService.GetResourceAsync(Convert.ToInt32(txtNum.Value))
        Catch ex As Exception
            txtMessage.Text = ex.Message.ToString()
        End Try
    End Sub '   Button_Click

    Private Sub localService_GetResourceCompleted(sender As Object, e As GetResourceCompletedEventArgs)

        If (e.Result.Title IsNot Nothing) Then
            If (e.Result.Title.ToString() = "Error: No data for that ID") Then
                txtMessage.Text = "No data for that ID"
                txtAuthor.Text = "Author: n/a"
                txtTitle.Text = e.Result.Title.ToString()
                txtType.Text = "Type: n/a"
                lnkWeb.NavigateUri = New Uri(e.Result.URL.ToString())
                imgResource.Source = New BitmapImage(New Uri(e.Result.Image.ToString(), UriKind.Absolute))
            Else
                txtMessage.Text = ""
                txtAuthor.Text = "Author: " + e.Result.Author.ToString()
                txtTitle.Text = e.Result.Title.ToString()
                txtType.Text = "Type: " + e.Result.Type.ToString()
                lnkWeb.NavigateUri = New Uri(e.Result.URL.ToString())
                imgResource.Source = New BitmapImage(New Uri(e.Result.Image.ToString(), UriKind.Absolute))
            End If
        Else
            txtMessage.Text = "Error."
        End If

        ButtonGet.Content = "Call WCF Service"
        ButtonGet.IsEnabled = True
    End Sub '   localService_GetResourceCompleted
End Class   '   MainPage
' End Namespace
' ..\Project_08\WebServices\3-WCFService\WCFClient\MainPage.xaml.cs
