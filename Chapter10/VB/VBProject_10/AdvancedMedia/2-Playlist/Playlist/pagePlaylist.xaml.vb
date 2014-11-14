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

' Namespace Playlist

Partial Public Class pagePlaylist
    Inherits UserControl

    Private myStoryboard As Storyboard = New Storyboard()

    Public Sub New()

        InitializeComponent()
    End Sub

    Private Sub CollapseAll()

        mediaServerPlaylist.Opacity = 0
        mediaWebPlaylist.Opacity = 0
        Media.Opacity = 0

        mediaServerPlaylist.Stop()
        mediaWebPlaylist.Stop()
        Media.Stop()

        canvasMedia.Visibility = Visibility.Collapsed
        canvasServer.Visibility = Visibility.Collapsed
        canvasWeb.Visibility = Visibility.Collapsed
    End Sub '   CollapseAll

    Private Sub ShowMedia(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        Media.Opacity = 1

        canvasMedia.Visibility = Visibility.Visible
        Media.Source = New Uri("Butterfly.wmv", UriKind.Relative)
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   ShowMedia

    Private Sub ShowServerPlaylist(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        mediaServerPlaylist.Opacity = 1
        canvasServer.Visibility = Visibility.Visible
        mediaServerPlaylist.Source = New Uri("http://addrating.com:100/SilverlightSSPL", UriKind.Absolute)
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   ShowServerPlaylist

    Private Sub ShowWebPlaylist(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        canvasWeb.Visibility = Visibility.Visible
        mediaWebPlaylist.Opacity = 1
        mediaWebPlaylist.Source = New Uri("http://addrating.com:80/ServerPlaylist.isx", UriKind.Absolute)
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   ShowWebPlaylist
End Class   '   pagePlaylist:
' End Namespace   '   Playlist
' mediaWebPlaylist.Source = new Uri("http:' addrating.com:100/Sample_Broadcast", UriKind.Absolute)
' ..\Project_10\AdvancedMedia\2-Playlist\Playlist\pagePlaylist.xaml.cs
