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

' Namespace PaintBrushes

Partial Public Class PageMediaBrush
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub ShowImageBrush(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        CanvasText.Visibility = Visibility.Visible

        Dim tbShadow As TextBlock = New TextBlock()

        tbShadow.Text = "Silverlight"
        tbShadow.FontSize = 42
        tbShadow.Foreground = New SolidColorBrush(Colors.Gray)
        tbShadow.SetValue(Canvas.TopProperty, 22.0)
        tbShadow.SetValue(Canvas.LeftProperty, 22.0)

        Dim tb As TextBlock = New TextBlock()

        tb.Text = "Silverlight"
        tb.FontSize = 42
        tb.SetValue(Canvas.TopProperty, 20.0)
        tb.SetValue(Canvas.LeftProperty, 20.0)

        Dim ib As ImageBrush = New ImageBrush()
        ib.ImageSource = New BitmapImage(New Uri("files/silverlighticon.png", UriKind.Relative))
        tb.Foreground = ib

        CanvasText.Children.Add(tbShadow)
        CanvasText.Children.Add(tb)

        ShowVideoBrush(sender, e)
    End Sub '   ShowImageBrush

    Private Sub ShowVideoBrush(sender As Object, e As MouseButtonEventArgs)

        Dim tbShadow As TextBlock = New TextBlock()

        tbShadow.Text = "Silverlight"
        tbShadow.FontSize = 42
        tbShadow.Foreground = New SolidColorBrush(Colors.Gray)
        tbShadow.SetValue(Canvas.TopProperty, 102.0)
        tbShadow.SetValue(Canvas.LeftProperty, 72.0)

        Dim tb As TextBlock = New TextBlock()

        tb.Text = "Silverlight"
        tb.FontSize = 42
        tb.SetValue(Canvas.TopProperty, 100.0)
        tb.SetValue(Canvas.LeftProperty, 70.0)

        Dim vb As VideoBrush = New VideoBrush()

        vb.SetSource(myMedia)
        vb.Stretch = Stretch.UniformToFill

        tb.Foreground = vb
        CanvasText.Children.Add(tbShadow)
        CanvasText.Children.Add(tb)
        myMedia.Play()
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   ShowVideoBrush

    Private Sub ShowBorder(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        CanvasText.Children.Clear()
        myMedia.Play()
        CanvasBorder.Visibility = Visibility.Visible
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   ShowBorder

    Private Sub ShowShapeBrush(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        CanvasShape.Visibility = Visibility.Visible
        myMedia.Play()
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   ShowShapeBrush

    Private Sub CollapseAll()

        myMedia.Stop()
        CanvasText.Visibility = Visibility.Collapsed
        CanvasText.Children.Clear()
        CanvasBorder.Visibility = Visibility.Collapsed

        CanvasShape.Visibility = Visibility.Collapsed
        CanvasControl.Visibility = Visibility.Collapsed
    End Sub '   CollapseAll

    Private Sub ShowControlBrush(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        CanvasControl.Visibility = Visibility.Visible
        myMedia.Play()

        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   ShowControlBrush
End Class   '   PageMediaBrush

' End Namespace 
' ..\Graphics\G\Graphics\Graphics\3-PaintBrush\Pages\4-PageMediaBrush.xaml.cs
