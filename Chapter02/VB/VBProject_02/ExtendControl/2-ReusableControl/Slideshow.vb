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

    Public Partial class Slideshow
        Inherits Control

        Private slideCount As Integer
        Private currentSlide As Integer
        Public Sub New()

            AddHandler RoutedEventHandler(Control_Loaded), New Loaded
            Application.Current.Host.Content.Resized += New EventHandler(Content_Resized)
        End Sub '   New
        private Sub Control_Loaded(sender As Object, e As Eventargs)
        End Sub '   Control_Loaded
        private Sub Content_Resized(sender As Object, e As Eventargs)


            If (Application.Current.Host.Content.IsFullScreen) Then
                SlideWidth = Application.Current.Host.Content.ActualWidth
                SlideHeight = Application.Current.Host.Content.ActualHeight
            else
                SlideWidth = 400
                SlideHeight = 300
            End If
        End Sub '   Content_Resized
        private Sub SetupSlideShow()

            Dim mySlides As SlideImagesList  = CType(DataContext,  SlideImagesList)

            SlideCount = mySlides.Count
            CurrentSlide = 0
            SetToolTip()
        End Sub '   SetupSlideShow
        private Sub SetToolTip()

            Dim CurrentSlideIndexed As Integer  = CurrentSlide + 1

            MainTitle = CurrentSlideIndexed + " / " + SlideCount
        End Sub '   SetToolTip

        Public Property SlideCount() As Integer
            get

                Dim mySlides As SlideImagesList  = CType(DataContext,  SlideImagesList)

                slideCount = mySlides.Count
                return slideCount
            End Get
            set
                slideCount = value
            End Set
        End Property
        Public Property CurrentSlide() As Integer
            Get
                return currentSlide
            End Get
            set
                currentSlide = value
            End Set
        End Property
        Public Property SlideWidth() As Double
            Get
                return SlideImage.Width
            End Get
            set
                SlideImage.Width = value
                SlideGrid.Width = value
                SlideBorder.Width = value
            End Set
        End Property
        Public Property SlideHeight() As Double
            Get
                return SlideImage.Height
            End Get
            set
                SlideImage.Height = value - 35
                SlideGrid.Height = value
                SlideBorder.Height = value
            End Set
        End Property
        Public Property MainImage() As String
            Get
                return SlideImage.Source.ToString()
            End Get
            set
                SlideImage.Source = New BitmapImage(new Uri(value, UriKind.Relative))
            End Set
        End Property
        Public Property MainTitle() As String
            Get
                return SlideTitle.Text
            End Get
            set
                SlideTitle.Text = value
            End Set
        End Property
        private Sub SlideImage_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

            NextSlide()
        End Sub '   SlideImage_MouseLeftButtonUp
        private Sub RightButton_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

            NextSlide()
        End Sub '   RightButton_MouseLeftButtonUp
        private Sub LeftButton_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

            PreviousSlide()
        End Sub '   LeftButton_MouseLeftButtonUp
        private Sub FullscreenButton_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

            Application.Current.Host.Content.IsFullScreen =  Not Application.Current.Host.Content.IsFullScreen

            If (Application.Current.Host.Content.IsFullScreen) Then
                SlideWidth = Application.Current.Host.Content.ActualWidth
                SlideHeight = Application.Current.Host.Content.ActualHeight
            else
                SlideWidth = 400
                SlideHeight = 300
            End If
        End Sub '   FullscreenButton_MouseLeftButtonUp
        private Sub NextSlide()


            If (CurrentSlide = SlideCount - 1) Then
                CurrentSlide = 0
            else
                CurrentSlide += 1
            End If

            Dim mySlides As SlideImagesList  = CType(DataContext,  SlideImagesList)

            MainImage = mySlides(CurrentSlide).ImageUri
            SetToolTip()
        End Sub '   NextSlide
        private Sub PreviousSlide()


            If (CurrentSlide = 0) Then
                CurrentSlide = SlideCount - 1
            else
                CurrentSlide -= 1
            End If

            Dim mySlides As SlideImagesList  = CType(DataContext,  SlideImagesList)

            MainImage = mySlides(CurrentSlide).ImageUri
            SetToolTip()
        End Sub '   PreviousSlide
        private Sub SlideImage_ImageFailed(sender As Object, e As Exceptionroutedeventargs)

            MainImage = "Images/_ErrorImage.jpg"
        End Sub '   SlideImage_ImageFailed
    End Class   '   Slideshow

    Public class Slides

            Public Property ImageUri As String
            Public Property Title As String
    End Class   '   Slides

    Public class SlideImagesList
        Inherits List<Slides>

        Dim si As Slides

        End Property
        End Sub '   New
    End Class   '   SlideImagesList

' End Namespace
' ..\ExtendControl\E\ExtendControl\ExtendControl\2-ReusableControl\Slideshow.cs
