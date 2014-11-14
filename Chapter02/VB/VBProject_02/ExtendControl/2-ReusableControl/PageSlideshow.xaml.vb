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

    Public Partial class PageSlideshow
        Inherits UserControl

        Private SwitchFlag As Boolean
        Public Sub New()

            InitializeComponent()
        End Sub '   New

        private Sub Button_Click(sender As Object, e As Routedeventargs)


            If ( Not SwitchFlag) Then
                SwitchData()
                SwitchFlag = true
            else
                DefaultData()
                SwitchFlag = false
            End If
        End Sub '   Button_Click
        private Sub DefaultData()
        End Sub '   DefaultData
        private Sub SwitchData()

            ' tn6.DataContext = New ThumbnailData() With {
                    .ImageUri = "Images/Einstein.jpg",
            ' tn4.DataContext = New ThumbnailData() With {
                    .ImageUri = "Images/Newton.jpg",
            ' tn2.DataContext = New ThumbnailData() With {
                    .ImageUri = "Images/Darwin.jpg",
        End Sub '   SwitchData

        private Sub ReSizeThumb(w As Double, h As Double)

            ' tn1.ThumbWidth = tn2.ThumbWidth = tn3.ThumbWidth = tn4.ThumbWidth = tn5.ThumbWidth = tn6.ThumbWidth = w
            ' tn1.ThumbHeight = tn2.ThumbHeight = tn3.ThumbHeight = tn4.ThumbHeight = tn5.ThumbHeight = tn6.ThumbHeight = h
        End Sub '   ReSizeThumb

        private Sub ResizeThumb_Click(sender As Object, e As Routedeventargs)

            ' if (tn1.ThumbWidth =52)
            ' {
            '     ReSizeThumb(80,100)
            ' }
            ' else if (tn1.ThumbWidth = 80)
            ' {
            '     ReSizeThumb(70, 84)
            ' }
            ' else if (tn1.ThumbWidth = 70)
            ' {
            '     ReSizeThumb(52, 65)
            ' }
        End Sub '   ResizeThumb_Click
    End Class   '   PageSlideshow
' End Namespace
' ..\ExtendControl\E\ExtendControl\ExtendControl\2-ReusableControl\PageSlideshow.xaml.cs
