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

' Namespace FunBadge

Partial Public Class MainPage
    Inherits UserControl
    Public Sub New()
        InitializeComponent()
    End Sub '   New

    Public Sub New(startValue As Integer, backColor As String)
        InitializeComponent()
        Face.Source = New BitmapImage(New Uri("images/smiley/" + startValue.ToString() + ".png", UriKind.Relative))

        '  to get the color
        Dim xamlString As String = "<Canvas xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" Background=""" + backColor + """/>"
        Dim c As Canvas = CType(System.Windows.Markup.XamlReader.Load(xamlString), Canvas)
        Dim myParameterBrush As SolidColorBrush = CType(c.Background, SolidColorBrush)

        LayoutRoot.Background = myParameterBrush
    End Sub '   New

    Private Sub Image_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        Dim facestring As String = (CType(CType(sender, Image).Source, BitmapImage)).UriSource.OriginalString

        facestring = facestring.ToLower().Replace(".png", "")
        facestring = facestring.Substring(14)

        Dim count As Integer = Convert.ToInt32(facestring)

        count += 1
        If (count = 11) Then
            count = 0
        End If


        Dim i As Image = New Image()

        Face.Source = New BitmapImage(New Uri("images/smiley/" + count.ToString() + ".png", UriKind.Relative))
    End Sub '   Image_MouseLeftButtonUp
End Class   '   MainPage
' End Namespace   '   FunBadge
' ..\Project_07\ASPAJAX\2-ASPUserControl\FunBadge\MainPage.xaml.cs
