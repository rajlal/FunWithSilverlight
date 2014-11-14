Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media

' Namespace ReusableControl

Partial Public Class PageThumbnails
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
        tn2.DataContext = New ThumbnailData() With {
                            .ImageUri = "Images/leonardo.jpg",
                            .Title = "Da Vinci"}
        tn3.DataContext = New ThumbnailData() With {
                            .ImageUri = "Images/Newton.jpg",
                            .Title = "Newton"}
        tn4.DataContext = New ThumbnailData() With {
                            .ImageUri = "Images/Galileo.jpg",
                            .Title = "Galileo"}
        tn5.DataContext = New ThumbnailData() With {
                            .ImageUri = "Images/Darwin.jpg",
                            .Title = "Darwin"}
        tn6.DataContext = New ThumbnailData() With {
                            .ImageUri = "Images/Edison.jpg",
                            .Title = "Edison"}

    End Sub '   DefaultData
    Private Sub SwitchData()


        tn6.DataContext = New ThumbnailData() With {
                            .ImageUri = "Images/Einstein.jpg",
                            .Title = "Einstein"}
        tn5.DataContext = New ThumbnailData() With {
                            .ImageUri = "Images/leonardo.jpg",
                            .Title = "Da Vinci"}
        tn4.DataContext = New ThumbnailData() With {
                            .ImageUri = "Images/Newton.jpg",
                            .Title = "Newton"}
        tn3.DataContext = New ThumbnailData() With {
                            .ImageUri = "Images/Galileo.jpg",
                            .Title = "Galileo"}
        tn2.DataContext = New ThumbnailData() With {
                            .ImageUri = "Images/Darwin.jpg",
                            .Title = "Darwin"}
        tn1.DataContext = New ThumbnailData() With {
                            .ImageUri = "Images/Edison.jpg",
                            .Title = "Edison"}
    End Sub '   SwitchData

    Private Sub Re_SizeThumb(w As Double, h As Double)

        tn6.ThumbWidth = w
        tn5.ThumbWidth = w
        tn4.ThumbWidth = w
        tn3.ThumbWidth = w
        tn2.ThumbWidth = w
        tn1.ThumbWidth = w

        tn6.ThumbHeight = h
        tn5.ThumbHeight = h
        tn4.ThumbHeight = h
        tn3.ThumbHeight = h
        tn2.ThumbHeight = h
        tn1.ThumbHeight = h
    End Sub '   Re_SizeThumb

    Private Sub ResizeThumb_Click(sender As Object, e As Routedeventargs)

        If (tn1.ThumbWidth = 52) Then
            Re_SizeThumb(80, 100)
        ElseIf (tn1.ThumbWidth = 80) Then
            Re_SizeThumb(70, 84)
        ElseIf (tn1.ThumbWidth = 70) Then
            Re_SizeThumb(52, 65)
        End If
    End Sub '   ResizeThumb_Click
End Class   '   PageThumbnails
' End Namespace
' ..\ExtendControl\E\ExtendControl\ExtendControl\2-ReusableControl\Pages\1-UserControl-Thumbnail\PageThumbnails.xaml.cs
