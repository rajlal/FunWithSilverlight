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

' Namespace DeepZoom

Partial Public Class PageSingleImage
    Inherits UserControl

    '
    '  Based on prior work done by Lutz Gerhard, Peter Blois, and Scott Hanselman
    '
    Dim BoundryFlag As Boolean = True
    Dim ZoomBoundFlag As Boolean = True
    Dim m_dZoom As Double = 1.0
    Dim duringDrag As Boolean = False
    Dim mouseDown As Boolean = False
    Dim lastMouseDownPos As Point = New Point()
    Dim lastMousePos As Point = New Point()
    Dim lastMouseViewPort As Point = New Point()

    Public Property ZoomFactor() As Double
        Get
            Return m_dZoom
        End Get
        Set(value As Double)
            m_dZoom = value
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
        AddHandler msi.ImageOpenSucceeded, AddressOf msi_ImageOpenSucceeded
        '
        '  Handling all of the mouse and keyboard functionality
        '
        AddHandler Me.MouseLeftButtonDown, Sub(sender As Object, e As MouseButtonEventArgs)
                                               lastMouseDownPos = e.GetPosition(msi)
                                               lastMouseViewPort = msi.ViewportOrigin

                                               mouseDown = True

                                               msi.CaptureMouse()
                                           End Sub

        AddHandler Me.MouseLeftButtonUp, Sub(sender As Object, e As MouseButtonEventArgs)

                                             If (Not duringDrag) Then
                                                 Dim shiftDown As Boolean = ((Keyboard.Modifiers And ModifierKeys.Shift) = ModifierKeys.Shift)

                                                 Dim newzoom As Double = m_dZoom

                                                 If (shiftDown) Then
                                                     newzoom /= 2
                                                 Else
                                                     newzoom *= 2
                                                 End If

                                                 Zoom(newzoom, msi.ElementToLogicalPoint(Me.lastMousePos))
                                             End If

                                             duringDrag = False
                                             mouseDown = False

                                             msi.ReleaseMouseCapture()
                                             updateStatus()
                                         End Sub

        AddHandler Me.MouseMove, Sub(sender As Object, e As MouseEventArgs)
                                     lastMousePos = e.GetPosition(msi)

                                     If (mouseDown AndAlso Not duringDrag) Then
                                         duringDrag = True

                                         Dim w As Double = msi.ViewportWidth
                                         Dim o As Point = New Point(msi.ViewportOrigin.X, msi.ViewportOrigin.Y)

                                         msi.UseSprings = CType(UseSprings.IsChecked, Boolean)
                                         msi.ViewportOrigin = New Point(o.X, o.Y)
                                         msi.ViewportWidth = w
                                         m_dZoom = 1 / w
                                         msi.UseSprings = CType(UseSprings.IsChecked, Boolean)
                                         updateStatus()
                                     End If

                                     If (duringDrag) Then

                                         Dim newPoint As Point = lastMouseViewPort

                                         newPoint.X += (lastMouseDownPos.X - lastMousePos.X) / msi.ActualWidth * msi.ViewportWidth
                                         newPoint.Y += (lastMouseDownPos.Y - lastMousePos.Y) / msi.ActualWidth * msi.ViewportWidth

                                         If (BoundryFlag) Then
                                             If (newPoint.X < 0) Then
                                                 newPoint.X = 0
                                             End If
                                             If (newPoint.Y < 0) Then
                                                 newPoint.Y = 0
                                             End If

                                             Dim MaxX As Double = 1 - (1 / Me.m_dZoom)
                                             '  Since Height = 1/2 Width
                                             Dim MaxY As Double = (1 - (1 / Me.m_dZoom)) / 2.0

                                             If (newPoint.X > MaxX) Then
                                                 newPoint.X = MaxX
                                             End If
                                             If (newPoint.Y > MaxY) Then
                                                 newPoint.Y = MaxY
                                             End If
                                         End If
                                         msi.ViewportOrigin = newPoint
                                         updateStatus()
                                     End If
                                 End Sub

        AddHandler New MouseWheelHelper(Me).Moved, Sub(sender As Object, e As MouseWheelEventArgs)
                                                       e.Handled = True

                                                       Dim newzoom As Double = m_dZoom


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

        msi.ViewportWidth = 1
    End Sub '   msi_ImageOpenSucceeded

    Sub msi_Loaded(sender As Object, e As Routedeventargs)

        '  Hook up any events you want when the image has successfully been opened
    End Sub '   msi_Loaded

    Private Sub Zoom(newzoom As Double, p As Point)

        If (ZoomBoundFlag) Then
            If (newzoom < 1) Then
                newzoom = 1
            End If

            If (newzoom > 11) Then
                newzoom = 11
            End If
        End If

        msi.ZoomAboutLogicalPoint(newzoom / m_dZoom, p.X, p.Y)
        m_dZoom = newzoom
        updateStatus()
    End Sub '   Zoom

    Private Sub ZoomInClick(sender As Object, e As System.windows.routedeventargs)

        Zoom(m_dZoom * 1.3, msi.ElementToLogicalPoint(New Point(0.5 * msi.ActualWidth, 0.5 * msi.ActualHeight)))
    End Sub '   ZoomInClick

    Private Sub ZoomOutClick(sender As Object, e As System.windows.routedeventargs)

        Zoom(m_dZoom / 1.3, msi.ElementToLogicalPoint(New Point(0.5 * msi.ActualWidth, 0.5 * msi.ActualHeight)))
    End Sub '   ZoomOutClick

    Private Sub GoHomeClick(sender As Object, e As System.windows.routedeventargs)

        Me.msi.ViewportWidth = 1.0
        Me.msi.ViewportOrigin = New Point(0.0, 0.0)
        ZoomFactor = 1
        ResetStatus()
    End Sub '   GoHomeClick

    Private Sub ResetStatus()

        StatusZoom.Text = "Zoom:100%"
        StatusViewport.Text = "Viewport Origin:0.00/0.00"
        StatusViewportWidth.Text = "ViewportWidth:1"
    End Sub '   ResetStatus

    Private Sub updateStatus()

        Dim ZoomFactorPercentage As Double = ZoomFactor * 100

        StatusZoom.Text = "Zoom:" + String.Format("{0:N}", ZoomFactorPercentage) + "%"
        StatusViewport.Text = "Viewport Origin:" + String.Format("{0:N}", Me.msi.ViewportOrigin.X) + "/" + String.Format("{0:N}", Me.msi.ViewportOrigin.Y)
        StatusViewportWidth.Text = "ViewportWidth:" + String.Format("{0:N}", Me.msi.ViewportWidth)
    End Sub '   updateStatus

    Private Sub GoFullScreenClick(sender As Object, e As System.windows.routedeventargs)

        If (Not Application.Current.Host.Content.IsFullScreen) Then
            Application.Current.Host.Content.IsFullScreen = True
            msi.Width = Application.Current.Host.Content.ActualWidth
            msi.Height = Application.Current.Host.Content.ActualHeight
        Else
            Application.Current.Host.Content.IsFullScreen = False
            msi.Width = 600
            msi.Height = 300
        End If
    End Sub '   GoFullScreenClick

    '  Handling the VSM states
    Private Sub LeaveMovie(sender As Object, e As System.windows.input.mouseeventargs)

        ' VisualStateManager.GoToState(this, "FadeOut", true)
        buttonCanvas.Opacity = 0
    End Sub '   LeaveMovie

    Private Sub EnterMovie(sender As Object, e As System.windows.input.mouseeventargs)

        buttonCanvas.Opacity = 1
        ' VisualStateManager.GoToState(this, "FadeIn", true)
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

        msi.ViewportWidth = 1 / rect.Width
        msi.ViewportOrigin = New Point(-rect.Left / rect.Width, -rect.Top / rect.Width)
    End Sub '   LayoutDZI

    Private Sub CheckBound_Click(sender As Object, e As Routedeventargs)

        BoundryFlag = CType(CheckBound.IsChecked, Boolean)
    End Sub '   CheckBound_Click

    Private Sub TextCanvas_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        e.Handled = True
    End Sub '   TextCanvas_MouseLeftButtonUp

    Private Sub ZoomBound_Click(sender As Object, e As Routedeventargs)

        ZoomBoundFlag = CType(ZoomBound.IsChecked, Boolean)
    End Sub '   ZoomBound_Click

    Private Sub UseSprings_Click(sender As Object, e As Routedeventargs)

        Me.msi.UseSprings = CType(UseSprings.IsChecked, Boolean)
    End Sub '   UseSprings_Click

    Private Sub LeftInClick(sender As Object, e As Routedeventargs)

        Me.msi.ViewportOrigin = New Point(Me.msi.ViewportOrigin.X + 0.05, Me.msi.ViewportOrigin.Y)
    End Sub '   LeftInClick

    Private Sub RightInClick(sender As Object, e As Routedeventargs)

        Me.msi.ViewportOrigin = New Point(Me.msi.ViewportOrigin.X - 0.05, Me.msi.ViewportOrigin.Y)
    End Sub '   RightInClick
End Class   '   PageSingleImage
' End Namespace
' ..\ExtendControl\E\ExtendControl\ExtendControl\4-DeepZoom\1-SingleImage.xaml.cs
