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

' Namespace PaintBrushes

Partial Public Class PageSolidBrush
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub GetSolidColorBrush()

        ClearAll()
        canvasSolidColor.Visibility = Visibility.Visible
        StatusBar.Text = "SolidColor Brush support 16x16x16x16=65536 Colors"
    End Sub '   GetSolidColorBrush

    Private Sub slider_ValueChanged(sender As Object, e As RoutedPropertyChangedEventargs(Of Double))

        Dim colorValueA As String = String.Format("{0:0#}", sliderA.Value)
        Dim colorValueR As String = String.Format("{0:0#}", sliderR.Value)
        Dim colorValueG As String = String.Format("{0:0#}", sliderG.Value)
        Dim colorValueB As String = String.Format("{0:0#}", sliderB.Value)

        Dim cValueA As String = ConvertHex(colorValueA)
        Dim cValueR As String = ConvertHex(colorValueR)
        Dim cValueG As String = ConvertHex(colorValueG)
        Dim cValueB As String = ConvertHex(colorValueB)
        Dim colorValue As String = cValueA + cValueR + cValueG + cValueB

        txtColorA.Text = cValueA
        txtColorR.Text = cValueR
        txtColorG.Text = cValueG
        txtColorB.Text = cValueB

        txtColor.Text = "#" + colorValue
        rectangleColor.Fill = New SolidColorBrush(getColorFromHex(colorValue))
    End Sub '   slider_ValueChanged

    Private Function ConvertHex(s As String) As String

        Dim rHex As String = s

        Select Case (s)
            Case "10"
                rHex = "AA"
            Case "11"
                rHex = "BB"
            Case "12"
                rHex = "CC"
            Case "13"
                rHex = "DD"
            Case "14"
                rHex = "EE"
            Case "15"
                rHex = "FF"
        End Select    '   s

        Return rHex
    End Function '   CreateWebSafeColors

    Public Function getColorFromHex(s As String) As Color

        Dim a As Byte = System.Convert.ToByte(s.Substring(0, 2), 16)
        Dim r As Byte = System.Convert.ToByte(s.Substring(2, 2), 16)
        Dim g As Byte = System.Convert.ToByte(s.Substring(4, 2), 16)
        Dim b As Byte = System.Convert.ToByte(s.Substring(6, 2), 16)
        Return Color.FromArgb(a, r, g, b)
    End Function '   ShowPreDefined

    Private Sub CreateWebSafeColors(sender As Object, e As MouseButtonEventArgs)

        ClearAll()
        Dim wscolors As String() = New String() {"00", "33", "66", "99", "CC", "FF"}

        Dim topv As Double = 0

        For i As Integer = 0 To 5
            topv += 25
            Dim leftv As Double = 6

            For j As Integer = 0 To 5
                For k As Integer = 0 To 5
                    Dim currentcolor As String = wscolors(i) + wscolors(j) + wscolors(k)
                    Dim rc As Rectangle = New Rectangle()

                    rc.SetValue(Canvas.LeftProperty, leftv)
                    rc.SetValue(Canvas.TopProperty, topv)
                    rc.Fill = New SolidColorBrush(getColorFromHex("FF" + currentcolor))
                    rc.Width = 7
                    rc.Height = 24
                    rc.Cursor = Cursors.Hand
                    Dim t As ToolTip = New ToolTip()
                    t.Content = "FF" + currentcolor
                    ToolTipService.SetToolTip(rc, t)
                    leftv += 8
                    CanvasWS.Children.Add(rc)
                Next    '   k
            Next    '   j
        Next

        CanvasWS.Visibility = Visibility.Visible
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   ClearAll

    Private Sub ShowPreDefined(sender As Object, e As MouseButtonEventArgs)

        ClearAll()
        canvasPreDefColor.Visibility = Visibility.Visible
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   Colors_MouseLeftButtonUp

    Private Sub ClearAll()

        canvasSolidColor.Visibility = Visibility.Collapsed
        canvasPreDefColor.Visibility = Visibility.Collapsed
        CanvasWS.Visibility = Visibility.Collapsed
    End Sub  '   ConvertHex

    Private Sub Colors_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        GetSolidColorBrush()
    End Sub  '   getColorFromHex
End Class   '   PageSolidBrush

' End Namespace 
' ..\Graphics\G\Graphics\Graphics\3-PaintBrush\Pages\1-PageSolidBrush.xaml.cs
