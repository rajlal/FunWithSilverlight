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

' Namespace Encode

Partial Public Class pageDeliver
    Inherits UserControl

    Private myStoryboard As Storyboard = New Storyboard()

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub CollapseAll()

        mediaProgressive.Opacity = 0
        mediaTraditional.Opacity = 0
        mediaSmooth.Opacity = 0
        Media.Opacity = 0

        mediaProgressive.Stop()
        mediaTraditional.Stop()
        mediaSmooth.Stop()
        Media.Stop()

        canvasMedia.Visibility = Visibility.Collapsed
        canvasProgressive.Visibility = Visibility.Collapsed
        canvasTraditional.Visibility = Visibility.Collapsed
        canvasSmooth.Visibility = Visibility.Collapsed
    End Sub '   CollapseAll

    Private Sub ShowMedia(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        Media.Opacity = 1

        canvasMedia.Visibility = Visibility.Visible
        Media.Source = New Uri("Butterfly.wmv", UriKind.Relative)
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   ShowMedia

    Private Sub ShowProgressive(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        mediaProgressive.Opacity = 1
        canvasProgressive.Visibility = Visibility.Visible
        mediaProgressive.Source = New Uri("http://addrating.com/Silverlight/Media/WMV/Robotica_1080.wmv", UriKind.Absolute)
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   ShowProgressive

    Private Sub ShowTraditional(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        canvasTraditional.Visibility = Visibility.Visible
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   ShowTraditional

    Private Sub ShowSmooth(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        canvasSmooth.Visibility = Visibility.Visible
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        mediaSmooth.Play()
    End Sub '   ShowSmooth


    Private Sub canvasProgressive_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)
    End Sub '   canvasProgressive_MouseLeftButtonUp

    Private Sub canvasSmooth_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        mediaSmooth.Play()
    End Sub '   canvasSmooth_MouseLeftButtonUp

    Private Sub canvasTraditional_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

    End Sub '   canvasTraditional_MouseLeftButtonUp

    Private Sub TextBlock_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

    End Sub '   TextBlock_MouseLeftButtonUp
End Class   '   pageDeliver
' End Namespace   '   Encode
' ..\Project_09\Media\2-Encode\Encode\pageDeliver.xaml.cs
