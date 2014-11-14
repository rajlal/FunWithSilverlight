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
Imports System.Windows.Media.Imaging

' Namespace DeepZoom

Partial Public Class PageMultipleImage
    Inherits UserControl

    '
    '  Based on prior work done by Lutz Gerhard, Peter Blois, and Scott Hanselman
    '
    Dim m_dZoom As Double = 1
    Dim duringDrag As Boolean = False
    Dim mouseDown As Boolean = False
    Dim lastMouseDownPos As Point = New Point()
    Dim lastMousePos As Point = New Point()
    Dim lastMouseViewPort As Point = New Point()
    Dim subImageIndex As Integer = 7

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

                                                 Dim p As Point = Me.msi.ElementToLogicalPoint(e.GetPosition(Me.msi))

                                                 subImageIndex = SubImageHitTest(p)

                                                 If (subImageIndex >= 0) Then
                                                     If (ZoomFactor > 1) Then
                                                         subImageTitle.Text = GetNameofWonder(subImageIndex)
                                                         subImageThumb.Source = New BitmapImage(New Uri("Images/thumb" + CStr(subImageIndex) + ".jpg", UriKind.Relative))
                                                         subImageDesc.Text = GetDescriptionWonder(subImageIndex)
                                                     End If
                                                 End If
                                             End If

                                             duringDrag = False
                                             mouseDown = False

                                             msi.ReleaseMouseCapture()
                                         End Sub

        AddHandler Me.MouseMove, Sub(sender As Object, e As MouseEventArgs)
                                     lastMousePos = e.GetPosition(msi)

                                     If (mouseDown AndAlso Not duringDrag) Then
                                         duringDrag = True

                                         Dim w As Double = msi.ViewportWidth
                                         Dim o As Point = New Point(msi.ViewportOrigin.X, msi.ViewportOrigin.Y)

                                         msi.UseSprings = False
                                         msi.ViewportOrigin = New Point(o.X, o.Y)
                                         msi.ViewportWidth = w
                                         m_dZoom = 1 / w
                                         msi.UseSprings = True
                                     End If

                                     If (duringDrag) Then
                                         Dim newPoint As Point = lastMouseViewPort

                                         newPoint.X += (lastMouseDownPos.X - lastMousePos.X) / msi.ActualWidth * msi.ViewportWidth
                                         newPoint.Y += (lastMouseDownPos.Y - lastMousePos.Y) / msi.ActualWidth * msi.ViewportWidth

                                         msi.ViewportOrigin = newPoint
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
    End Sub
        '  no scope
    Public Function SubImageHitTest(p As Point) As Integer

        Dim nReturn As Integer = -1

        For i As Integer = 0 To Me.msi.SubImages.Count - 1

            Dim subImageRect As Rect = GetSubImageRect(i)

            If (subImageRect.Contains(p)) Then
                nReturn = i
                i = Me.msi.SubImages.Count
            End If
        Next    '   i

        Return nReturn
    End Function  '   SubImageHitTest

    '  no scope
    Public Function GetSubImageRect(indexSubImage As Integer) As Rect


        If (indexSubImage < 0 OrElse indexSubImage >= Me.msi.SubImages.Count) Then
            Return Rect.Empty
        Else
            Dim subImage As MultiScaleSubImage = Me.msi.SubImages(indexSubImage)

            Dim scaleBy As Double = 1 / subImage.ViewportWidth

            Return New Rect(-subImage.ViewportOrigin.X * scaleBy, -subImage.ViewportOrigin.Y * scaleBy, 1 * scaleBy, (1 / subImage.AspectRatio) * scaleBy)
        End If
    End Function  '   GetSubImageRect

    '  no scope
    Public Sub DisplaySubImageCentered(indexSubImage As Integer)


        If (indexSubImage >= 0 AndAlso indexSubImage < msi.SubImages.Count) Then

            Dim subImageRect As Rect = GetSubImageRect(indexSubImage)
            Dim msiAspectRatio As Double = msi.ActualWidth / msi.ActualHeight

            Dim newOrigin As Point = New Point(subImageRect.X - (msi.ViewportWidth / 2) + (subImageRect.Width / 2),
                                            subImageRect.Y - ((msi.ViewportWidth / msiAspectRatio) / 2) + (subImageRect.Height / 2))

            msi.ViewportOrigin = newOrigin
        End If
    End Sub '   DisplaySubImageCentered

    '  no scope
    Public Sub msi_ImageOpenSucceeded(sender As Object, e As Routedeventargs)

        ' If collection, this gets you a list of all of the MultiScaleSubImages
        '

        For Each subImage As MultiScaleSubImage In msi.SubImages
            '   System.Windows.Browser.HtmlPage.Window.Alert(subImage.ToString())
        Next    '   MultiScaleSubImage

        msi.ViewportWidth = 1
    End Sub '   msi_ImageOpenSucceeded

    '  no scope
    Public Sub msi_Loaded(sender As Object, e As Routedeventargs)

        '  Hook up any events you want when the image has successfully been opened
    End Sub '   msi_Loaded

    Private Sub Zoom(newzoom As Double, p As Point)

        msi.ZoomAboutLogicalPoint(newzoom / m_dZoom, p.X, p.Y)
        m_dZoom = newzoom
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
    End Sub '   GoHomeClick

    Private Sub GoFullScreenClick(sender As Object, e As System.windows.routedeventargs)

        If (Not Application.Current.Host.Content.IsFullScreen) Then
            Application.Current.Host.Content.IsFullScreen = True
            msi.Width = Application.Current.Host.Content.ActualWidth
            msi.Height = Application.Current.Host.Content.ActualHeight
        Else
            Application.Current.Host.Content.IsFullScreen = False
            msi.Width = 600
            msi.Height = 500
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

    Private Sub LeftInClick(sender As Object, e As Routedeventargs)

        Me.msi.ViewportOrigin = New Point(Me.msi.ViewportOrigin.X + 0.05, Me.msi.ViewportOrigin.Y)
    End Sub '   LeftInClick

    Private Sub RightInClick(sender As Object, e As Routedeventargs)

        Me.msi.ViewportOrigin = New Point(Me.msi.ViewportOrigin.X - 0.05, Me.msi.ViewportOrigin.Y)
    End Sub '   RightInClick

    Private Sub UpInClick(sender As Object, e As Routedeventargs)

        Me.msi.ViewportOrigin = New Point(Me.msi.ViewportOrigin.X, Me.msi.ViewportOrigin.Y + 0.05)
    End Sub '   UpInClick

    Private Sub DownInClick(sender As Object, e As Routedeventargs)

        Me.msi.ViewportOrigin = New Point(Me.msi.ViewportOrigin.X, Me.msi.ViewportOrigin.Y - 0.05)
    End Sub '   DownInClick

    Private Sub closeDesc_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)
    End Sub '   closeDesc_MouseLeftButtonUp

    Private Sub closeDesc_Click(sender As Object, e As Routedeventargs)

        canvasDescription.Opacity = 0
    End Sub '   closeDesc_Click

    Private Function GetNameofWonder(index As Integer) As String

        Dim nameWonder As String = ""

        If (index = 0) Then
            nameWonder = "Colosseum in Rome"
        ElseIf (index = 1) Then
            nameWonder = "Golden Gate Bridge"
        ElseIf (index = 2) Then
            nameWonder = "Grand Canyon"
        ElseIf (index = 3) Then
            nameWonder = "The Great Wall of China"
        ElseIf (index = 4) Then
            nameWonder = "Great Pyramid of Giza"
        ElseIf (index = 5) Then
            nameWonder = "Mount Everest"
        ElseIf (index = 6) Then
            nameWonder = "Taj Mahal"
        ElseIf (index = 7) Then
            nameWonder = "Wonders of the World"
        End If

        Return nameWonder
    End Function  '   GetNameofWonder

    Private Sub GoInfoClick(sender As Object, e As Routedeventargs)

        If (canvasDescription.Opacity = 0.75) Then
            canvasDescription.Opacity = 0
        Else
            canvasDescription.Opacity = 0.75
        End If
     End Sub '   GoInfoClick

    Private Function GetDescriptionWonder(index As Integer) As String

        Dim descWonder As String = ""


        If (index = 0) Then
            descWonder = "The Colosseum or Roman Coliseum, originally the Flavian Amphitheatre (Latin: Amphitheatrum Flavium, Italian Anfiteatro Flavio or Colosseo), is an elliptical amphitheatre in the center of the city of Rome, Italy, the largest ever built in the Roman Empire. It is one of the greatest works of Roman architecture and Roman engineering."
        ElseIf (index = 1) Then
            descWonder = "The Golden Gate Bridge is a suspension bridge spanning the Golden Gate, the opening of the San Francisco Bay onto the Pacific Ocean. As part of both U.S. Route 101 and State Route 1, it connects the city of San Francisco on the northern tip of the San Francisco Peninsula to Marin County."
        ElseIf (index = 2) Then
            descWonder = "The Grand Canyon is a steep-sided gorge carved by the Colorado River in the United States state of Arizona. It is largely contained within the Grand Canyon National Park — one of the first national parks in the United States. President Theodore Roosevelt was a major proponent of preservation of the Grand Canyon area, and visited on numerous occasions to hunt and enjoy the scenery."
        ElseIf (index = 3) Then
            descWonder = "The Great Wall of China (pinyin: Chángchéng; literally 'long city/fortress') or (pinyin: Wànlǐ Chángchéng; literally 'The long wall of 10,000 Li (里)') is a series of stone and earthen fortifications in China, built, rebuilt, and maintained between the 5th century BC and the 16th century to protect the northern borders of the Chinese Empire from Xiongnu attacks during the rule of successive dynasties. "
        ElseIf (index = 4) Then
            descWonder = "The Great Pyramid of Giza, also called Khufu's Pyramid or the Pyramid of Khufu, and Pyramid of Cheops, is the oldest and largest of the three pyramids in the Giza Necropolis bordering what is now Cairo, Egypt, and is the only remaining member of the Seven Wonders of the Ancient World."
        ElseIf (index = 5) Then
            descWonder = "Mount Everest, also called Sagarmatha (meaning Head of the Sky) or Chomolungma, Qomolangma or Zhumulangma ( in Chinese: 珠穆朗玛峰 Zhūmùlǎngmǎ Fēng) is the highest mountain on Earth, as measured by the height of its summit above sea level, which is 8,848 metres (29,029 ft). The mountain, which is part of the Himalaya range in High Asia, is located on the border between Sagarmatha Zone, Nepal, and Tibet, China."
        ElseIf (index = 6) Then
            descWonder = "The Taj Mahal (pronounced /tɑdʒ mə'hɑl/ Persian/Urdu: تاج محل) is a mausoleum located in Agra, India, built by Mughal Emperor Shah Jahan in memory of his favorite wife, Mumtaz Mahal."
        ElseIf (index = 7) Then
            descWonder = "Colosseum in Rome \nGrand Canyon\nTaj Mahal\nThe Great Wall of China\nGreat Pyramid of Giza\nGolden Gate Bridge\nMount Everest"
        End If

        Return descWonder
    End Function  '   GetDescriptionWonder
End Class   '   PageMultipleImage
' End Namespace
' ..\ExtendControl\E\ExtendControl\ExtendControl\4-DeepZoom\2-MultipleImage.xaml.cs
