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

' Namespace Media

Partial Public Class pageMedia
    Inherits UserControl

    Private myStoryboard As Storyboard = New Storyboard()

    Public Sub New()
        InitializeComponent()
    End Sub '   New

    Private Sub CollapseAll()

        mediaFadein.Opacity = 0
        mediaFadein.Stop()
        storyboardFadein.Stop()

        mediaSlidein.Width = 0
        mediaSlidein.Stop()
        storyboardSlidein.Stop()

        mediaRotate.Stop()
        storyboardRotate.Stop()
        Media.Stop()
        canvasMedia.Visibility = Visibility.Collapsed
        canvasFadein.Visibility = Visibility.Collapsed
        canvasSlidein.Visibility = Visibility.Collapsed
        canvasRotate.Visibility = Visibility.Collapsed
    End Sub '   CollapseAll

    Private Sub ShowMedia(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        canvasMedia.Visibility = Visibility.Visible
        Media.Source = New Uri("Butterfly.wmv", UriKind.Relative)
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   ShowMedia

    Private Sub ShowFadein(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        canvasFadein.Visibility = Visibility.Visible
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        myStoryboard = storyboardFadein
        myStoryboard.Begin()
    End Sub '   ShowFadein

    Private Sub ShowSlidein(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        canvasSlidein.Visibility = Visibility.Visible
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        myStoryboard = storyboardSlidein
        myStoryboard.Begin()
    End Sub '   ShowSlidein

    Private Sub ShowRotate(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        canvasRotate.Visibility = Visibility.Visible
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        storyboardRotate.Begin()
        mediaRotate.Play()
    End Sub '   ShowRotate

    Private Sub storyboardFadein_Completed(sender As Object, e As EventArgs)

        mediaFadein.Play()
    End Sub '   storyboardFadein_Completed

    Private Sub canvasFadein_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        myStoryboard = storyboardFadein
        myStoryboard.Begin()
    End Sub '   canvasFadein_MouseLeftButtonUp

    Private Sub storyboardSlidein_Completed(sender As Object, e As EventArgs)

        mediaSlidein.Play()
    End Sub '   storyboardSlidein_Completed

    Private Sub canvasRotate_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        storyboardRotate.Begin()
        mediaRotate.Play()
    End Sub '   canvasRotate_MouseLeftButtonUp

    Private Sub canvasSlidein_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        myStoryboard = storyboardSlidein
        myStoryboard.Begin()
    End Sub '   canvasSlidein_MouseLeftButtonUp

    Private Sub storyboardRotate_Completed(sender As Object, e As EventArgs)

        mediaRotate.Play()
    End Sub '   storyboardRotate_Completed
End Class   '   pageMedia
' End Namespace   '   Media
' ..\Project_09\Media\1-RichMedia\Media\1-pageMedia.xaml.cs
