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

' Namespace ReusableControl

Partial Public Class PageSlide
    Inherits UserControl

    Private SwitchFlag As Boolean

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub Button_Click(sender As Object, e As Routedeventargs)

        If (Not SwitchFlag) Then
            SwitchData()
            SwitchFlag = True
        Else
            DefaultData()
            SwitchFlag = False
        End If
    End Sub '   Button_Click

    Private Sub DefaultData()

        tn1.DataContext = New ThumbnailData() With {
                .ImageUri = "Images/Einstein.jpg",
                .Title = "Einstein"}
    End Sub '   DefaultData

    Private Sub SwitchData()

        tn1.DataContext = New ThumbnailData() With {
                .ImageUri = "Images/Edison.jpg",
                .Title = "Edison"}
    End Sub '   SwitchData

    Private Sub Re_SizeThumb(w As Double, h As Double)

        ' tn1.ThumbWidth = tn2.ThumbWidth = tn3.ThumbWidth = tn4.ThumbWidth = tn5.ThumbWidth = tn6.ThumbWidth = w
        ' tn1.ThumbHeight = tn2.ThumbHeight = tn3.ThumbHeight = tn4.ThumbHeight = tn5.ThumbHeight = tn6.ThumbHeight = h
    End Sub '   Re_SizeThumb

    Private Sub ResizeThumb_Click(sender As Object, e As Routedeventargs)

        ' if (tn1.ThumbWidth =52)
        ' {
        '     Re_SizeThumb(80,100)
        ' }
        ' else if (tn1.ThumbWidth = 80)
        ' {
        '     Re_SizeThumb(70, 84)
        ' }
        ' else if (tn1.ThumbWidth = 70)
        ' {
        '     Re_SizeThumb(52, 65)
        ' }
    End Sub '   ResizeThumb_Click
End Class   '   PageSlide
' End Namespace
' ..\ExtendControl\E\ExtendControl\ExtendControl\2-ReusableControl\Pages\2-UserControl-Slideshow\PageSlide.xaml.cs
