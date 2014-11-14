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
Imports System.Windows.Browser

' Namespace SlideShow

Partial Public Class Slide
    Inherits UserControl
    Private _slideCount As Integer
    Private _currentSlide As Integer
    Private _slideWidth As Double
    Private _slideHeight As Double

    Public Sub New(w As Double, h As Double)
        InitializeComponent()

        AddHandler Application.Current.Host.Content.Resized, AddressOf Content_Resized

        Me._slideWidth = w
        Me._slideHeight = h
    End Sub '   New

    Private Sub Content_Resized(sender As Object, e As EventArgs)

        Dim SHeight As Double = 0.0
        Dim SWidth As Double = 0.0

        If (Application.Current.Host.Content.IsFullScreen) Then
            SWidth = Application.Current.Host.Content.ActualWidth
            SHeight = Application.Current.Host.Content.ActualHeight
        Else
            SWidth = _slideWidth
            SHeight = _slideHeight
        End If

        SlideImage.Width = SWidth
        SlideGrid.Width = SWidth
        SlideBorder.Width = SWidth

        SlideImage.Height = SHeight - 112.0
        SlideGrid.Height = SHeight
        SlideBorder.Height = SHeight
    End Sub '   Content_Resized

    Private Sub SetupSlideShow()

        Dim mySlides As SlideImagesList = CType(DataContext, SlideImagesList)

        _slideCount = mySlides.Count
        _currentSlide = 0
        SetToolTip()
    End Sub '   SetupSlideShow

    Private Sub SetToolTip()

        Dim CurrentSlideIndexed As Integer = CurrentSlide + 1

        MainTitle = CStr(CurrentSlideIndexed) + " / " + CStr(_slideCount)
    End Sub '   SetToolTip

    Public Property SlideCount() As Integer
        Get
            Dim mySlides As SlideImagesList = CType(DataContext, SlideImagesList)

            _slideCount = mySlides.Count
            Return _slideCount
        End Get
        Set(value As Integer)

            _slideCount = value
        End Set
    End Property

    Public Property CurrentSlide() As Integer
        Get
            Return _currentSlide
        End Get

        Set(value As Integer)

            _currentSlide = value
        End Set
    End Property

    Public Property SlideWidth() As Double
        Get
            Return _slideWidth
        End Get
        Set(value As Double)
            _slideWidth = value
        End Set
    End Property

    Public Property SlideHeight() As Double
        Get
            Return _slideHeight
        End Get
        Set(value As Double)

            _slideHeight = value
        End Set
    End Property

    Public Property MainImage() As String
        Get
            Return SlideImage.Source.ToString()
        End Get
        Set(value As String)
            SlideImage.Source = New BitmapImage(New Uri(value, UriKind.Relative))
        End Set
    End Property

    Public Property MainTitle() As String
        Get
            Return SlideTitle.Text
        End Get
        Set(value As String)
            SlideTitle.Text = value
        End Set
    End Property

    Private Sub SlideImage_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        NextSlide()
    End Sub '   SlideImage_MouseLeftButtonUp

    Private Sub RightButton_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        NextSlide()
    End Sub '   RightButton_MouseLeftButtonUp

    Private Sub LeftButton_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        PreviousSlide()
    End Sub '   LeftButton_MouseLeftButtonUp

    Private Sub FullscreenButton_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        Dim SWidth As Double = 0.0
        Dim SHeight As Double = 0.0

        Application.Current.Host.Content.IsFullScreen = Not Application.Current.Host.Content.IsFullScreen

        If (Application.Current.Host.Content.IsFullScreen) Then
            SWidth = Application.Current.Host.Content.ActualWidth
            SHeight = Application.Current.Host.Content.ActualHeight
        Else
            SWidth = _slideWidth
            SHeight = _slideHeight
        End If


        SlideImage.Width = SWidth
        SlideGrid.Width = SWidth
        SlideBorder.Width = SWidth

        SlideImage.Height = SHeight - 112
        SlideGrid.Height = SHeight
        SlideBorder.Height = SHeight
    End Sub '   FullscreenButton_MouseLeftButtonUp

    Private Sub NextSlide()

        If (_currentSlide >= (_slideCount - 1)) Then
            _currentSlide = 0
        Else
            _currentSlide += 1
        End If

        Dim mySlides As SlideImagesList = CType(DataContext, SlideImagesList)

        MainImage = mySlides(CurrentSlide).ImageUri
        SetToolTip()
    End Sub '   NextSlide

    Private Sub PreviousSlide()

        If (_currentSlide = 0) Then
            _currentSlide = _slideCount - 1
        Else
            _currentSlide -= 1
        End If

        Dim mySlides As SlideImagesList = CType(DataContext, SlideImagesList)

        MainImage = mySlides(_currentSlide).ImageUri
        SetToolTip()
    End Sub '   PreviousSlide

    Private Sub SlideImage_ImageFailed(sender As Object, e As ExceptionRoutedEventArgs)

        MainImage = "Images/_ErrorImage.jpg"
    End Sub '   SlideImage_ImageFailed
End Class   '   Slide

Public Class Slides

    Public Property ImageUri As String
    Public Property Title As String
End Class   '   Slides

Public Class SlideImagesList
    Inherits List(Of Slides)

    Dim si As Slides

    Public Property val() As Slides
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
' End Namespace   '   SlideShow
' ..\Project_05\ExtendBrowser\2b-ExampleSlideShow\Slide.xaml.cs
