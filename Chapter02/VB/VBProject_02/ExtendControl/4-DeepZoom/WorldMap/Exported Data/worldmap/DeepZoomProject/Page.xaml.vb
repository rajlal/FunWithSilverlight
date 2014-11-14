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

    Public Partial class Page
        Inherits UserControl

        '
        '  Based on prior work done by Lutz Gerhard, Peter Blois, and Scott Hanselman
        '
        Dim BoundryFlag As Boolean  = true
        Dim ZoomBoundFlag As Boolean  = true
        Dim zoom As Double  = 1
        Dim duringDrag As Boolean  = false
        Dim mouseDown As Boolean  = false
        Dim lastMouseDownPos As Point  = New Point()
        Dim lastMousePos As Point  = New Point()
        Dim lastMouseViewPort As Point  = New Point()

        Public Property ZoomFactor() As Double
            Get
                return zoom
            End Get
            Set
                zoom = value
            End Sub , Me.MouseLeftButtonDown
        End Property

        Public Sub New()

            InitializeComponent()

            '
            '  Firing an event when the MultiScaleImage is Loaded
            '
            AddHandler RoutedEventHandler(msi_Loaded), New msi.Loaded

            '
            '  Firing an event when all of the images have been Loaded
            '
            AddHandler RoutedEventHandler(msi_ImageOpenSucceeded), New msi.ImageOpenSucceeded

            '
            '  Handling all of the mouse and keyboard functionality
            '
            AddHandler Public Delegate Sub (object sender, MouseButtonEventArgs e)
                lastMouseDownPos = e.GetPosition(msi)
                lastMouseViewPort = msi.ViewportOrigin

                mouseDown = true

                msi.CaptureMouse()
            End Sub , Me.MouseLeftButtonUp

            AddHandler Public Delegate Sub (object sender, MouseButtonEventArgs e)

                If ( Not duringDrag) Then
                    (Keyboard.Modifiers & ModifierKeys.Shift) = ModifierKeys.Shift
                    Dim shiftDown As Boolean  = ModifierKeys.Shift

                    Dim newzoom As Double  = zoom


                    If (shiftDown) Then
                        newzoom /= 2
                    else
                        newzoom *= 2
                    End If

                    Zoom(newzoom, msi.ElementToLogicalPoint(Me.lastMousePos))
                End If
                duringDrag = false
                mouseDown = false

                msi.ReleaseMouseCapture()
                updateStatus()
            End Sub , Me.MouseMove

            AddHandler Public Delegate Sub (object sender, MouseEventArgs e)
                lastMousePos = e.GetPosition(msi)

                If (mouseDown  AndAlso   Not duringDrag) Then
                    duringDrag = true

                    Dim w As Double  = msi.ViewportWidth
                    Dim o As Point  = New Point(msi.ViewportOrigin.X, msi.ViewportOrigin.Y)

                    msi.UseSprings = CType(UseSprings.IsChecked, Boolean)
                    msi.ViewportOrigin = New Point(o.X, o.Y)
                    msi.ViewportWidth = w
                    zoom = 1/w
                    msi.UseSprings = CType(UseSprings.IsChecked, Boolean)
                    updateStatus()
                End If


                If (duringDrag) Then

                    Dim newPoint As Point  = lastMouseViewPort

                    newPoint.X += (lastMouseDownPos.X - lastMousePos.X) / msi.ActualWidth * msi.ViewportWidth
                    newPoint.Y += (lastMouseDownPos.Y - lastMousePos.Y) / msi.ActualWidth * msi.ViewportWidth


                    If (BoundryFlag) Then
                        If (newPoint.X < 0) Then
                            newPoint.X = 0
                        End If
                        If (newPoint.Y < 0) Then
                            newPoint.Y = 0
                        End If

                        Dim MaxX As Double  = 1 - (1 / Me.zoom)
                        '  Since Height = 1/2 Width
                        Dim MaxY As Double  = (1 - (1 / Me.zoom))/2.0

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
            End Sub , New MouseWheelHelper(this).Moved

            AddHandler Public Delegate Sub (object sender, MouseWheelEventArgs e)
                e.Handled = true

                Dim newzoom As Double  = zoom


                If (e.Delta < 0) Then
                    newzoom /= 1.3
                else
                    newzoom *= 1.3
                End If

                Zoom(newzoom, msi.ElementToLogicalPoint(Me.lastMousePos))
                msi.CaptureMouse()
            End Set
        End Sub '   New

        Sub msi_ImageOpenSucceeded(sender As Object, e As Routedeventargs)

            ' If collection, this gets you a list of all of the MultiScaleSubImages
            '
            ' foreach (MultiScaleSubImage subImage in msi.SubImages)
            ' {
            '     '  Do something
            ' }

            msi.ViewportWidth = 1
        End Sub '   msi_ImageOpenSucceeded

        Sub msi_Loaded(sender As Object, e As Routedeventargs)

            '  Hook up any events you want when the image has successfully been opened
        End Sub '   msi_Loaded

        private Sub Zoom(newzoom As Double, p As Point)


            If (ZoomBoundFlag) Then

                If (newzoom < 1) Then
                    newzoom = 1
                End If

                If (newzoom > 11) Then
                    newzoom = 11
                End If
            End If

                msi.ZoomAboutLogicalPoint(newzoom / zoom, p.X, p.Y)
                zoom = newzoom
                updateStatus()
        End Sub '   Zoom

        private Sub ZoomInClick(sender As Object, e As System.windows.routedeventargs)

            Zoom(zoom * 1.3, msi.ElementToLogicalPoint(new Point(.5 * msi.ActualWidth, .5 * msi.ActualHeight)))
        End Sub '   ZoomInClick

        private Sub ZoomOutClick(sender As Object, e As System.windows.routedeventargs)

            Zoom(zoom / 1.3, msi.ElementToLogicalPoint(new Point(.5 * msi.ActualWidth, .5 * msi.ActualHeight)))
        End Sub '   ZoomOutClick

        private Sub GoHomeClick(sender As Object, e As System.windows.routedeventargs)

        	Me.msi.ViewportWidth = 1.0
			Me.msi.ViewportOrigin = New Point(0.0,0.0)
            ZoomFactor = 1
            ResetStatus()
        End Sub '   GoHomeClick

        private Sub ResetStatus()

                StatusZoom.Text = "Zoom:100%"
                StatusViewport.Text = "Viewport Origin:0.00/0.00"
                StatusViewportWidth.Text = "ViewportWidth:1"
        End Sub '   ResetStatus

        private Sub updateStatus()

                Dim ZoomFactorPercentage As Double  = ZoomFactor * 100

                StatusZoom.Text = "Zoom:" + string.Format("{0:N}", ZoomFactorPercentage) + "%"
                StatusViewport.Text = "Viewport Origin:" + string.Format("{0:N}", Me.msi.ViewportOrigin.X) + "/" + string.Format("{0:N}", Me.msi.ViewportOrigin.Y)
                StatusViewportWidth.Text = "ViewportWidth:" + string.Format("{0:N}", Me.msi.ViewportWidth)
        End Sub '   updateStatus

        private Sub GoFullScreenClick(sender As Object, e As System.windows.routedeventargs)


            If ( Not Application.Current.Host.Content.IsFullScreen) Then
                Application.Current.Host.Content.IsFullScreen = true
                msi.Width = Application.Current.Host.Content.ActualWidth
                msi.Height= Application.Current.Host.Content.ActualHeight
            else
                Application.Current.Host.Content.IsFullScreen = false
                msi.Width = 600
                msi.Height = 300
            End If
        End Sub '   GoFullScreenClick

        '  Handling the VSM states
        private Sub LeaveMovie(sender As Object, e As System.windows.input.mouseeventargs)

            ' VisualStateManager.GoToState(this, "FadeOut", true)
            buttonCanvas.Opacity = 0
        End Sub '   LeaveMovie

        private Sub EnterMovie(sender As Object, e As System.windows.input.mouseeventargs)

            buttonCanvas.Opacity = 1
            ' VisualStateManager.GoToState(this, "FadeIn", true)
        End Sub '   EnterMovie

        '  unused functions that show the inner math of Deep Zoom
        public Function getImageRect()

            return New Rect(-msi.ViewportOrigin.X / msi.ViewportWidth, -msi.ViewportOrigin.Y / msi.ViewportWidth, 1 / msi.ViewportWidth, 1 / msi.ViewportWidth * msi.AspectRatio)
        End Function  '   getImageRect

        public Function ZoomAboutPoint(img As Rect, zAmount As Double, pt As Point) As Rect

            return New Rect(pt.X + (img.X - pt.X) / zAmount, pt.Y + (img.Y - pt.Y) / zAmount, img.Width / zAmount, img.Height / zAmount)
        End Function  '   ZoomAboutPoint

        public Sub LayoutDZI(rect As Rect)

            Dim ar As Double  = msi.AspectRatio

            msi.ViewportWidth = 1 / rect.Width
            msi.ViewportOrigin = New Point(-rect.Left / rect.Width, -rect.Top / rect.Width)
        End Sub '   LayoutDZI

        private Sub CheckBound_Click(sender As Object, e As Routedeventargs)

            BoundryFlag =CType( CheckBound.IsChecked, Boolean)
        End Sub '   CheckBound_Click

        private Sub TextCanvas_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

            e.Handled = true
        End Sub '   TextCanvas_MouseLeftButtonUp

        private Sub ZoomBound_Click(sender As Object, e As Routedeventargs)

            ZoomBoundFlag = CType(ZoomBound.IsChecked, Boolean)
        End Sub '   ZoomBound_Click

        private Sub UseSprings_Click(sender As Object, e As Routedeventargs)

            Me.msi.UseSprings = CType(UseSprings.IsChecked, Boolean)
        End Sub '   UseSprings_Click

        private Sub LeftInClick(sender As Object, e As Routedeventargs)

            Me.msi.ViewportOrigin = New Point(Me.msi.ViewportOrigin.X + .05,Me.msi.ViewportOrigin.Y)
        End Sub '   LeftInClick

        private Sub RightInClick(sender As Object, e As Routedeventargs)

            Me.msi.ViewportOrigin = New Point(Me.msi.ViewportOrigin.X - .05, Me.msi.ViewportOrigin.Y)
        End Sub '   RightInClick
    End Class   '   Page
' End Namespace
' ..\ExtendControl\E\ExtendControl\ExtendControl\4-DeepZoom\WorldMap\Exported Data\worldmap\DeepZoomProject\Page.xaml.cs
