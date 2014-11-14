Imports System
Imports System.Diagnostics
Imports System.Text.RegularExpressions
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Markup
Imports System.Windows.Media.Imaging
Imports System.Collections.Generic

' Namespace SlideshowCustomControl

    Public class Slideshow
        Inherits Control

        Private layoutRoot As Canvas
        Private SlideImage As Image
        Private SlideBorder As Border
        Private SlideGrid As Grid
        Private SlideTitle As TextBlock
        Private LeftButton As Image
        Private RightButton As Image
        Private FullscreenButton As Image

    Public Sub New()
        MyBase.New()

        DefaultStyleKey = GetType(Slideshow)

        AddHandler Application.Current.Host.Content.Resized, AddressOf Content_Resized
    End Sub '   New

    Public ReadOnly SlideStyleProperty As DependencyProperty = DependencyProperty.Register("SlideStyle", GetType(Style), GetType(Slideshow), Nothing)
    Public ReadOnly TextProperty As DependencyProperty = DependencyProperty.Register("Text", GetType(String), GetType(Slideshow), Nothing)
    Public ReadOnly SlideWidthProperty As DependencyProperty = DependencyProperty.Register("SlideWidth", GetType(Double), GetType(Slideshow), Nothing)
    Public ReadOnly SlideHeightProperty As DependencyProperty = DependencyProperty.Register("SlideHeight", GetType(Double), GetType(Slideshow), Nothing)
    Public ReadOnly MainTitleProperty As DependencyProperty = DependencyProperty.Register("MainTitle", GetType(String), GetType(Slideshow), Nothing)
    Public ReadOnly MainImageProperty As DependencyProperty = DependencyProperty.Register("MainImage", GetType(String), GetType(Slideshow), Nothing)
    Public ReadOnly CurrentSlideProperty As DependencyProperty = DependencyProperty.Register("CurrentSlide", GetType(Integer), GetType(Slideshow), Nothing)
    Public ReadOnly SlideCountProperty As DependencyProperty = DependencyProperty.Register("SlideCount", GetType(Integer), GetType(Slideshow), Nothing)
     ''' <summary>
     ''' Gets or sets the style of the added TextBlock controls
     ''' </summary>
    Public Property SlideStyle() As Style
        Get
            Return CType(Me.GetValue(SlideStyleProperty), Style)
        End Get

        Set(value As Style)
            MyBase.SetValue(SlideStyleProperty, CType(value, DependencyObject))
        End Set
    End Property
    ''' <summary>
    ''' Gets or sets the text of the LinkLabel control
    ''' </summary>
    Public Property Text() As String
        Get
            Return CType(Me.GetValue(TextProperty), String)
        End Get

        Set(value As String)
            MyBase.SetValue(TextProperty, value)
        End Set
    End Property

    Public Property SlideCount() As Integer
        Get
            Return CType(GetValue(SlideCountProperty), Integer)
        End Get

        Set(value As Integer)
            SetValue(SlideCountProperty, value)
        End Set
    End Property

    Public Property CurrentSlide() As Integer
        Get
            Return CType(GetValue(CurrentSlideProperty), Integer)
        End Get

        Set(value As Integer)
            SetValue(CurrentSlideProperty, value)
        End Set
    End Property

    Public Property SlideWidth() As Double
        Get
            Return CType(GetValue(SlideWidthProperty), Double)
        End Get

        Set(value As Double)
            SetValue(SlideWidthProperty, value)
        End Set
    End Property

    Public Property SlideHeight() As Double
        Get
            Return CType(GetValue(SlideHeightProperty), Double)
        End Get
        Set(value As Double)
            SetValue(SlideHeightProperty, value)
        End Set
    End Property

    Public Property MainImage() As String
        Get
            Return CType(GetValue(MainImageProperty), String)
        End Get

        Set(value As String)
            SetValue(MainImageProperty, value)
        End Set
    End Property

    Public Property MainTitle() As String
        Get
            Return CType(GetValue(MainTitleProperty), String)
        End Get

        Set(value As String)
            SetValue(MainTitleProperty, value)
        End Set
    End Property

    Public Overrides Sub OnApplyTemplate()

        Me.layoutRoot = CType(Me.GetTemplateChild("LayoutRoot"), Canvas)
        Me.SlideImage = CType(Me.GetTemplateChild("SlideImage"), Image)
        Me.SlideGrid = CType(Me.GetTemplateChild("SlideGrid"), Grid)
        Me.SlideBorder = CType(Me.GetTemplateChild("SlideBorder"), Border)
        Me.SlideTitle = CType(Me.GetTemplateChild("SlideTitle"), TextBlock)
        Me.LeftButton = CType(Me.GetTemplateChild("LeftButton"), Image)
        Me.RightButton = CType(Me.GetTemplateChild("RightButton"), Image)
        Me.FullscreenButton = CType(Me.GetTemplateChild("FullscreenButton"), Image)

        AddHandler SlideImage.MouseLeftButtonDown, AddressOf Me.NextSlide
        AddHandler SlideImage.ImageFailed, AddressOf Me.ImageFailed
        AddHandler RightButton.MouseLeftButtonDown, AddressOf Me.NextSlide
        AddHandler LeftButton.MouseLeftButtonDown, AddressOf Me.PreviousSlide
        AddHandler FullscreenButton.MouseLeftButtonUp, AddressOf Me.FullScreen

        MyBase.OnApplyTemplate()
        LoadItems()
    End Sub '   OnApplyTemplate

    Private Sub Content_Resized(sender As Object, e As Eventargs)

        Try
            If (Application.Current.Host.Content.IsFullScreen) Then

                SlideWidth = Application.Current.Host.Content.ActualWidth
                SlideHeight = Application.Current.Host.Content.ActualHeight
            Else
                SlideWidth = 400
                SlideHeight = 300
            End If

            Me.SlideImage.Width = SlideWidth
            Me.SlideGrid.Width = SlideWidth
            Me.SlideBorder.Width = SlideWidth

            Me.SlideImage.Height = SlideHeight - 35
            Me.SlideGrid.Height = SlideHeight
            Me.SlideBorder.Height = SlideHeight
        Catch ex As Exception
        End Try
    End Sub '   Content_Resized

    Private Sub FullScreen(sender As Object, e As Eventargs)

        Application.Current.Host.Content.IsFullScreen = Not Application.Current.Host.Content.IsFullScreen

        If (Application.Current.Host.Content.IsFullScreen) Then
            SlideWidth = Application.Current.Host.Content.ActualWidth
            SlideHeight = Application.Current.Host.Content.ActualHeight
        Else
            SlideWidth = 400
            SlideHeight = 300
        End If

        Me.SlideImage.Width = SlideWidth
        Me.SlideGrid.Width = SlideWidth
        Me.SlideBorder.Width = SlideWidth

        Me.SlideImage.Height = SlideHeight - 35
        Me.SlideGrid.Height = SlideHeight
        Me.SlideBorder.Height = SlideHeight
    End Sub '   FullScreen

    Private Sub ImageFailed(sender As Object, e As Eventargs)

        MainImage = "Images/_ErrorImage.jpg"
        SlideImage.Source = New BitmapImage(New Uri(MainImage, UriKind.Relative))
    End Sub '   ImageFailed

    Private Sub NextSlide(sender As Object, e As Routedeventargs)

        If (CurrentSlide = SlideCount - 1) Then
            CurrentSlide = 0
        Else
            CurrentSlide += 1
        End If

        Dim mySlides As SlideImagesList = CType(DataContext, SlideImagesList)

        MainImage = mySlides(CurrentSlide).ImageUri

        Dim CurrentSlideIndexed As Integer = CurrentSlide + 1

        MainTitle = CStr(CurrentSlideIndexed) + " / " + CStr(SlideCount)
        SlideImage.Source = New BitmapImage(New Uri(MainImage, UriKind.Relative))
        SlideTitle.Text = MainTitle
    End Sub '   NextSlide

    Private Sub PreviousSlide(sender As Object, e As Routedeventargs)

        If (CurrentSlide = 0) Then
            CurrentSlide = SlideCount - 1
        Else
            CurrentSlide -= 1
        End If

        Dim mySlides As SlideImagesList = CType(DataContext, SlideImagesList)

        MainImage = mySlides(CurrentSlide).ImageUri

        Dim CurrentSlideIndexed As Integer = CurrentSlide + 1

        MainTitle = CStr(CurrentSlideIndexed) + " / " + CStr(SlideCount)
        SlideImage.Source = New BitmapImage(New Uri(MainImage, UriKind.Relative))
        SlideTitle.Text = MainTitle
    End Sub '   PreviousSlide

    Private Sub LoadItems()

        Dim mySlides As SlideImagesList = CType(DataContext, SlideImagesList)

        SlideCount = mySlides.Count
        SlideImage.Source = New BitmapImage(New Uri(MainImage, UriKind.Relative))
        SlideTitle.Text = MainTitle

        Me.SlideImage.Width = SlideWidth
        Me.SlideGrid.Width = SlideWidth
        Me.SlideBorder.Width = SlideWidth

        Me.SlideImage.Height = SlideHeight - 35
        Me.SlideGrid.Height = SlideHeight
        Me.SlideBorder.Height = SlideHeight
    End Sub '   LoadItems
End Class   '   Slideshow

Public Class Slides

    Public Property ImageUri As String
    Public Property Title As String
End Class   '   Slides

Public Class SlideImagesList
    Inherits List(Of Slides)

    Dim si As Slides

    Public Property Val() As Slides
        Get
            Return si
        End Get
        Set(value As Slides)
            si = value
        End Set
    End Property

    Public Sub New()
    End Sub '   New
End Class   '   SlideImagesList
' End Namespace
' ..\ExtendControl\E\ExtendControl\ExtendControl\3-CustomControl\SlideshowCustomControl\Slideshow.cs
