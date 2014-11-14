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

' Namespace SlideShow

Partial Public Class PageSlide
    Inherits UserControl

    Public Sub New(ImageArray As String(), myWidth As Double, myHeight As Double)

        InitializeComponent()
        LoadItems(ImageArray, myWidth, myHeight)
    End Sub '   New

    Private Sub LoadItems(listImages As String(), myWidth As Double, myHeight As Double)

        Dim mySlides As SlideImagesList = New SlideImagesList()


        For i As Integer = 0 To listImages.Length - 1

            Dim s1 As Slides = New Slides()

            s1.ImageUri = listImages(i)
            mySlides.Add(s1)
        Next    '   i

        mySlides.Capacity = listImages.Length

        Dim mySlideShow As Slide = New Slide(myWidth, myHeight)
        mySlideShow.Name = "tn1"

        mySlideShow.MainImage = listImages(0)
        mySlideShow.MainTitle = "1 / " + CStr(listImages.Length)
        mySlideShow.DataContext = mySlides
        mySlideShow.SlideCount = mySlides.Count
        LayoutRoot.Children.Add(mySlideShow)
    End Sub '   LoadItems
End Class   '   PageSlide
' End Namespace   '   SlideShow
' ..\Project_05\ExtendBrowser\2b-ExampleSlideShow\PageSlide.xaml.cs
