Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media

' Namespace ReusableControl

Partial Public Class Thumbnail
    Inherits UserControl

    Private ToolTipFlag As Boolean

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub ThumbImage_MouseEnter(sender As Object, e As Mouseeventargs)

        ImageSetToolTip()
    End Sub '   ThumbImage_MouseEnter

    Private Sub ImageSetToolTip()


        If (Not ToolTipFlag) Then

            Dim tt As ToolTip = New ToolTip()

            tt.Template = CType(Resources("ToolTipTemplate"), ControlTemplate)

            Dim brdrMain As Border = New Border()

            brdrMain.BorderThickness = New Thickness(1)
            brdrMain.BorderBrush = New SolidColorBrush(Colors.Gray)

            Dim ImageStackPanel As StackPanel = New StackPanel()

            Dim brdr As Border = New Border()

            brdr.BorderThickness = New Thickness(1)
            brdr.Background = New SolidColorBrush(Colors.LightGray)

            Dim k As TextBlock = New TextBlock()

            k.Text = ThumbnailText.Text
            k.Foreground = New SolidColorBrush(Colors.Black)
            k.TextAlignment = TextAlignment.Center
            brdr.Child = k

            Dim mainImage As Image = New Image()

            mainImage.Source = ThumbImage.Source
            mainImage.Width = 200
            ImageStackPanel.Children.Add(mainImage)
            ImageStackPanel.Children.Add(brdr)

            brdrMain.Child = ImageStackPanel
            tt.Content = brdrMain
            tt.Cursor = Cursors.Hand
            ToolTipService.SetToolTip(ThumbImage, tt)
            tt.IsOpen = True
            ToolTipFlag = True
        End If
    End Sub '   ImageSetToolTip

    Private Sub ThumbImage_MouseLeave(sender As Object, e As Mouseeventargs)

        ToolTipService.SetToolTip(ThumbImage, Nothing)
        ToolTipFlag = False
    End Sub '   ThumbImage_MouseLeave

    Private Sub ThumbImage_ImageFailed(sender As Object, e As Exceptionroutedeventargs)

        Me.DataContext = New ThumbnailData With {
                        .ImageUri = "files/silverlight.jpg",
                        .Title = "Image Error"}
    End Sub '   ThumbImage_ImageFailed

    Public Property ThumbWidth() As Double
        Get
            Return ThumbImage.Width
        End Get
        Set(value As Double)
            ThumbImage.Width = value
            ThumbShadow.Width = value + 2
            ThumbGrid.Width = value
            ThumbBorder.Width = value
        End Set
    End Property

    Public Property ThumbHeight() As Double
        Get
            Return ThumbImage.Height
        End Get
        Set(value As Double)
            ThumbImage.Height = value
            ThumbShadow.Height = value + 22
            ThumbGrid.Height = value + 20
            ThumbBorder.Height = value + 20
        End Set
    End Property
End Class   '   Thumbnail

Public Class ThumbnailData

    Public Property ImageUri As String
    Public Property Title As String
End Class   '   ThumbnailData
' End Namespace
' ..\ExtendControl\E\ExtendControl\ExtendControl\2-ReusableControl\Pages\1-UserControl-Thumbnail\Thumbnail.xaml.cs
