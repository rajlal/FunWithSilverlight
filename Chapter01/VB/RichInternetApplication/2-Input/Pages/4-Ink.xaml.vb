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
Imports System.Windows.Ink

' Namespace InputEvents

Partial Public Class PageInk
    Inherits UserControl

    Dim InkStroke As Stroke
    Dim InkColor As Color
    Dim InkWidth As Double
    Dim InkHeight As Double
    Public Sub New()

        InitializeComponent()
        SetDefaults()
    End Sub '   New

    Private Sub InkPresenterControl_MouseLeftButtonDown(sender As Object, e As Mousebuttoneventargs)

        InkPresenterControl.CaptureMouse()
        Dim MyStylusPointCollection As StylusPointCollection = New StylusPointCollection()
        MyStylusPointCollection.Add(e.StylusDevice.GetStylusPoints(InkPresenterControl))
        InkStroke = New Stroke(MyStylusPointCollection)
        InkPresenterControl.Strokes.Add(InkStroke)
    End Sub '   InkPresenterControl_MouseLeftButtonDown

    Private Sub InkPresenterControl_LostMouseCapture(sender As Object, e As Mouseeventargs)

        InkStroke = Nothing
    End Sub '   InkPresenterControl_LostMouseCapture

    Private Sub InkPresenterControl_MouseMove(sender As Object, e As Mouseeventargs)

        If (InkStroke IsNot Nothing) Then
            InkStroke.DrawingAttributes.Color = InkColor
            InkStroke.DrawingAttributes.Width = InkWidth
            InkStroke.DrawingAttributes.Height = InkHeight
            InkStroke.StylusPoints.Add(e.StylusDevice.GetStylusPoints(InkPresenterControl))
        End If
    End Sub '   InkPresenterControl_MouseMove

    ' Set the Clip property of the inkpresenter so that the strokes
    ' are contained within the boundary of the inkpresenter
    Private Sub SetDefaults()

        Dim MyRectangleGeometry As RectangleGeometry = New RectangleGeometry()
        MyRectangleGeometry.Rect = New Rect(0, 0, InkPresenterControl.ActualWidth, InkPresenterControl.ActualHeight)
        InkPresenterControl.Clip = MyRectangleGeometry
        InkColor = Colors.Black
        UpdateStatus("Color: Black")
        InkWidth = 2.0
        InkHeight = 2.0
        StatusThickness.Text = "Stroke thickness: " + CStr(InkWidth)
    End Sub '   SetDefaults

    Private Sub RedBox_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        InkColor = Colors.Red
        UpdateStatus("Color: Red")
    End Sub '   RedBox_MouseLeftButtonUp

    Private Sub BlueBox_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        InkColor = Colors.Blue
        UpdateStatus("Color: Blue")
    End Sub '   BlueBox_MouseLeftButtonUp

    Private Sub GreenBox_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        InkColor = Colors.Green
        UpdateStatus("Color: Green")
    End Sub '   GreenBox_MouseLeftButtonUp

    Private Sub YellowBox_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        InkColor = Colors.Yellow
        UpdateStatus("Color: Yellow")
    End Sub '   YellowBox_MouseLeftButtonUp

    Private Sub WhiteBox_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        InkColor = Colors.White
        UpdateStatus("Color: White/Erase")
    End Sub '   WhiteBox_MouseLeftButtonUp

    Private Sub UpdateStatus(status As String)

        StatusBar.Text = status
    End Sub '   UpdateStatus

    Private Sub BlackBox_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        InkColor = Colors.Black
        UpdateStatus("Color: Black")
    End Sub '   BlackBox_MouseLeftButtonUp

    Private Sub EraseBox_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        InkPresenterControl.Strokes.Clear()
    End Sub '   EraseBox_MouseLeftButtonUp

    Private Sub IncreaseThickness_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        If (InkWidth < 20) Then
            InkHeight = InkWidth + 1.0
            InkWidth = InkWidth + 1.0
        End If

        StatusThickness.Text = "Stroke thickness: " + CStr(InkWidth)
    End Sub '   IncreaseThickness_MouseLeftButtonUp

    Private Sub DecreaseThickness_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        If (InkWidth > 2) Then
            InkHeight = InkWidth - 1.0
            InkWidth = InkWidth - 1.0
        End If

        StatusThickness.Text = "Stroke thickness: " + CStr(InkWidth)
    End Sub '   DecreaseThickness_MouseLeftButtonUp

    Private Sub Mirror_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        Dim InkStrokeCollection As StrokeCollection = New StrokeCollection()

        For Each stroke As Stroke In InkPresenterControl.Strokes

            Dim newcollection As StylusPointCollection = New StylusPointCollection()

            For Each p As StylusPoint In stroke.StylusPoints
                ' Create the mirror image
                Dim newpoint As StylusPoint = New StylusPoint()
                newpoint.X = InkPresenterControl.ActualWidth - p.X
                newpoint.Y = p.Y
                newcollection.Add(newpoint)
            Next    '   p

            ' Add the mirror image to InkStrokeCollection
            Dim newStroke As Stroke = New Stroke(newcollection)
            newStroke.DrawingAttributes = stroke.DrawingAttributes
            InkStrokeCollection.Add(newStroke)
        Next    '   stroke

        For Each s As Stroke In InkStrokeCollection

            InkPresenterControl.Strokes.Add(s)
        Next    '   s
    End Sub '   Mirror_MouseLeftButtonUp
End Class   '   PageInk
' End Namespace 
' ..\RichInternetApplication\2-Input\Pages\4-Ink.xaml.cs
