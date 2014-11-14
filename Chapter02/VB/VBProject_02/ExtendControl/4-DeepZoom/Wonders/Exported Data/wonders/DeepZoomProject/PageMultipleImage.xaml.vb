Imports System
Imports System.Collections.Generic
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes

' Namespace DeepZoomProject

Partial Public Class PageMultipleImage
    Inherits UserControl
    '
    '  Based on prior work done by Lutz Gerhard, Peter Blois, and Scott Hanselman
    '
    Dim _zoom As Double = 1.0
    Dim duringDrag As Boolean = False
    Dim mouseDown As Boolean = False
    Dim lastMouseDownPos As Point = New Point()
    Dim lastMousePos As Point = New Point()
    Dim lastMouseViewPort As Point = New Point()

    Public Property ZoomFactor() As Double
        Get
            Return _zoom
        End Get
        Set(value As Double)
            _zoom = value
        End Set
    End Property

    Public Sub New()

        InitializeComponent()
        '
        '  Firing an event when the MultiScaleImage is Loaded
        '
        AddHandler msi.Loaded, AddressOf msi_Loaded
        '
        '  Firing an event when all of the images have been Loaded
        '
        AddHandler msi.ImageOpenSucceeded, AddressOf msi_ImageOpenSucceeded 'RoutedEventHandler()
        '
        '  Handling all of the mouse and keyboard functionality
        '
        AddHandler Me.MouseLeftButtonDown,
            Sub(sender As Object, e As MouseButtonEventArgs)
                lastMouseDownPos = e.GetPosition(msi)
                lastMouseViewPort = msi.ViewportOrigin
                mouseDown = True
                msi.CaptureMouse()
            End Sub

        AddHandler Me.MouseLeftButtonUp,
            Sub(sender As Object, e As MouseButtonEventArgs)
                If (Not duringDrag) Then
                    Dim shiftDown As Boolean = ((Keyboard.Modifiers And ModifierKeys.Shift) = ModifierKeys.Shift)
                    Dim newzoom As Double = _zoom

                    If (shiftDown) Then
                        newzoom /= 2.0
                    Else
                        newzoom *= 2.0
                    End If

                    Zoom(newzoom, msi.ElementToLogicalPoint(Me.lastMousePos))
                End If

                duringDrag = False
                mouseDown = False
                msi.ReleaseMouseCapture()
            End Sub

        AddHandler Me.MouseMove,
           Sub(sender As Object, e As MouseEventArgs)
               lastMousePos = e.GetPosition(msi)

               If (mouseDown AndAlso (Not duringDrag)) Then
                   duringDrag = True

                   Dim w As Double = msi.ViewportWidth
                   Dim o As Point = New Point(msi.ViewportOrigin.X, msi.ViewportOrigin.Y)

                   msi.UseSprings = False
                   msi.ViewportOrigin = New Point(o.X, o.Y)
                   msi.ViewportWidth = w
                   _zoom = 1.0 / w
                   msi.UseSprings = True
               End If

               If (duringDrag) Then

                   Dim newPoint As Point = lastMouseViewPort

                   newPoint.X += (lastMouseDownPos.X - lastMousePos.X) / msi.ActualWidth * msi.ViewportWidth
                   newPoint.Y += (lastMouseDownPos.Y - lastMousePos.Y) / msi.ActualWidth * msi.ViewportWidth
                   msi.ViewportOrigin = newPoint
               End If
           End Sub

        AddHandler New MouseWheelHelper(Me).Moved,
           Sub(sender As Object, e As MouseWheelEventArgs)
               e.Handled = True

               Dim newzoom As Double = _zoom

               If (e.Delta < 0) Then
                   newzoom /= 1.3
               Else
                   newzoom *= 1.3
               End If

               Zoom(newzoom, msi.ElementToLogicalPoint(Me.lastMousePos))
               msi.CaptureMouse()
           End Sub
    End Sub '   New

    Sub msi_ImageOpenSucceeded(sender As Object, e As Routedeventargs)

        ' If collection, this gets you a list of all of the MultiScaleSubImages
        '
        ' foreach (MultiScaleSubImage subImage in msi.SubImages)
        ' {
        '     '  Do something
        ' }

        msi.ViewportWidth = 1.0
    End Sub '   msi_ImageOpenSucceeded

    Sub msi_Loaded(sender As Object, e As Routedeventargs)

        '  Hook up any events you want when the image has successfully been opened
    End Sub '   msi_Loaded

    Private Sub Zoom(newzoom As Double, p As Point)


        If (newzoom < 0.5) Then
            newzoom = 0.5
        End If

        msi.ZoomAboutLogicalPoint(newzoom / _zoom, p.X, p.Y)
        _zoom = newzoom
    End Sub '   Zoom

    Private Sub ZoomInClick(sender As Object, e As System.windows.routedeventargs)

        Zoom(_zoom * 1.3, msi.ElementToLogicalPoint(New Point(0.5 * msi.ActualWidth, 0.5 * msi.ActualHeight)))
    End Sub '   ZoomInClick

    Private Sub ZoomOutClick(sender As Object, e As System.windows.routedeventargs)

        Zoom(_zoom / 1.3, msi.ElementToLogicalPoint(New Point(0.5 * msi.ActualWidth, 0.5 * msi.ActualHeight)))
    End Sub '   ZoomOutClick

    Private Sub GoHomeClick(sender As Object, e As System.windows.routedeventargs)

        Me.msi.ViewportWidth = 1.0
        Me.msi.ViewportOrigin = New Point(0, 0)
        ZoomFactor = 1.0
    End Sub '   GoHomeClick

    Private Sub GoFullScreenClick(sender As Object, e As System.windows.routedeventargs)

        If (Not Application.Current.Host.Content.IsFullScreen) Then
            Application.Current.Host.Content.IsFullScreen = True
        Else
            Application.Current.Host.Content.IsFullScreen = False
        End If
    End Sub '   GoFullScreenClick

    '  Handling the VSM states
    Private Sub LeaveMovie(sender As Object, e As System.windows.input.mouseeventargs)

        VisualStateManager.GoToState(Me, "FadeOut", True)
    End Sub '   LeaveMovie

    Private Sub EnterMovie(sender As Object, e As System.windows.input.mouseeventargs)

        VisualStateManager.GoToState(Me, "FadeIn", True)
    End Sub '   EnterMovie

    '  unused functions that show the inner math of Deep Zoom
    Public Function getImageRect() As Rect

        Return New Rect(-msi.ViewportOrigin.X / msi.ViewportWidth, -msi.ViewportOrigin.Y / msi.ViewportWidth, 1 / msi.ViewportWidth, 1 / msi.ViewportWidth * msi.AspectRatio)
    End Function  '   getImageRect

    Public Function ZoomAboutPoint(img As Rect, zAmount As Double, pt As Point) As Rect

        Return New Rect(pt.X + (img.X - pt.X) / zAmount, pt.Y + (img.Y - pt.Y) / zAmount, img.Width / zAmount, img.Height / zAmount)
    End Function  '   ZoomAboutPoint

    Public Sub LayoutDZI(rect As Rect)

        Dim ar As Double = msi.AspectRatio

        msi.ViewportWidth = 1.0 / rect.Width
        msi.ViewportOrigin = New Point(-rect.Left / rect.Width, -rect.Top / rect.Width)
    End Sub '   LayoutDZI
End Class   '   PageMultipleImage
' End Namespace
' ..\ExtendControl\E\ExtendControl\ExtendControl\4-DeepZoom\Wonders\Exported Data\wonders\DeepZoomProject\PageMultipleImage.xaml.cs
