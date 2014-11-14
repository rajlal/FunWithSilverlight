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

' Namespace ReusableControl

Partial Public Class Slide
    Inherits UserControl

    Private m_nslideCount As Integer
    Private m_currentSlide As Integer

    Public Sub New()

        InitializeComponent()
        AddHandler Application.Current.Host.Content.Resized, AddressOf Content_Resized
    End Sub '   New

    Private Sub Content_Resized(sender As Object, e As Eventargs)


        If (Application.Current.Host.Content.IsFullScreen) Then
            SlideWidth = Application.Current.Host.Content.ActualWidth
            SlideHeight = Application.Current.Host.Content.ActualHeight
        Else
            SlideWidth = 400
            SlideHeight = 300
        End If
    End Sub '   Content_Resized

    Private Sub SetupSlideShow()

        Dim mySlides As SlideImagesList = CType(DataContext, SlideImagesList)

        SlideCount = mySlides.Count
        CurrentSlide = 0
        SetToolTip()
    End Sub '   SetupSlideShow

    Private Sub SetToolTip()

        Dim CurrentSlideIndexed As Integer = CurrentSlide + 1

        MainTitle = CStr(CurrentSlideIndexed) + " / " + CStr(SlideCount)
    End Sub '   SetToolTip

    Public Property SlideCount() As Integer
        Get

            Dim mySlides As SlideImagesList = CType(DataContext, SlideImagesList)

            m_nslideCount = mySlides.Count
            Return m_nslideCount
        End Get
        Set(value As Integer)
            m_nslideCount = value
        End Set
    End Property

    Public Property CurrentSlide() As Integer
        Get
            Return m_currentSlide
        End Get
        Set(value As Integer)
            m_currentSlide = value
        End Set
    End Property

    Public Property SlideWidth() As Double
        Get
            Return SlideImage.Width
        End Get
        Set(value As Double)
            SlideImage.Width = value
            SlideGrid.Width = value
            SlideBorder.Width = value
        End Set
    End Property

    Public Property SlideHeight() As Double
        Get
            Return SlideImage.Height
        End Get
        Set(value As Double)
            SlideImage.Height = value - 35
            SlideGrid.Height = value
            SlideBorder.Height = value
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

    Private Sub SlideImage_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        NextSlide()
    End Sub '   SlideImage_MouseLeftButtonUp

    Private Sub RightButton_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        NextSlide()
    End Sub '   RightButton_MouseLeftButtonUp

    Private Sub LeftButton_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        PreviousSlide()
    End Sub '   LeftButton_MouseLeftButtonUp

    Private Sub FullscreenButton_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        Application.Current.Host.Content.IsFullScreen = Not Application.Current.Host.Content.IsFullScreen

        If (Application.Current.Host.Content.IsFullScreen) Then
            SlideWidth = Application.Current.Host.Content.ActualWidth
            SlideHeight = Application.Current.Host.Content.ActualHeight
        Else
            SlideWidth = 400
            SlideHeight = 300
        End If
    End Sub '   FullscreenButton_MouseLeftButtonUp

    Private Sub NextSlide()


        If (CurrentSlide = SlideCount - 1) Then
            CurrentSlide = 0
        Else
            CurrentSlide += 1
        End If

        Dim mySlides As SlideImagesList = CType(DataContext, SlideImagesList)

        MainImage = mySlides(CurrentSlide).ImageUri
        SetToolTip()
    End Sub '   NextSlide

    Private Sub PreviousSlide()


        If (CurrentSlide = 0) Then
            CurrentSlide = SlideCount - 1
        Else
            CurrentSlide -= 1
        End If

        Dim mySlides As SlideImagesList = CType(DataContext, SlideImagesList)

        MainImage = mySlides(CurrentSlide).ImageUri
        SetToolTip()
    End Sub '   PreviousSlide

    Private Sub SlideImage_ImageFailed(sender As Object, e As Exceptionroutedeventargs)

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

    Public Sub New()

    End Sub '   New

    Public Property Val() As Slides
        Get
            Return si
        End Get
        Set(value As Slides)
            si = value
        End Set
    End Property

    Public Sub SlideImagesList()

    End Sub
End Class   '   SlideImagesList

' End Namespace
' ..\ExtendControl\E\ExtendControl\ExtendControl\2-ReusableControl\Pages\2-UserControl-Slideshow\Slide.xaml.cs
