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
' Namespace InputEvents

Partial Public Class PageMouseCapture
    Inherits UserControl

    Dim isMouseCaptured As Boolean
    Dim mouseTop As Double
    Dim mouseLeft As Double
    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub ResetLogTable()

        LogTable.Text = ""
    End Sub '   ResetLogTable

    Private Sub Box_MouseLeftButtonDown(sender As Object, e As Mousebuttoneventargs)

        UpdateLogTable("LeftButtonDown: Box")
        Dim myBox As Rectangle = CType(sender, Rectangle)
        '  Nothing to get the position relative to the Siverlght Plug-in
        mouseTop = e.GetPosition(Nothing).Y
        '  Nothing to get the position relative to the Siverlght Plug-in
        mouseLeft = e.GetPosition(Nothing).X
        isMouseCaptured = True
        UpdateLogTable("MouseCaptured: Box")
        myBox.CaptureMouse()
        UpdateLogTable("isMouseCaptured: True")
        UpdateLogTable("CaptureMouse()")
    End Sub '   Box_MouseLeftButtonDown

    Private Sub Box_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        UpdateLogTable("LeftButtonUp: Box")
        Dim myBox As Rectangle = CType(sender, Rectangle)
        isMouseCaptured = False
        myBox.ReleaseMouseCapture()

        UpdateLogTable("isMouseCaptured: False")
        UpdateLogTable("ReleaseMouseCapture()")
        LogTable.Text = LogTable.Text + "\n----------------------------"
        Dim newTop As Double = e.GetPosition(Nothing).Y
        Dim newLeft As Double = e.GetPosition(Nothing).X

        If ((newTop > 100) AndAlso (newLeft > 100)) Then

            myBox.SetValue(Canvas.LeftProperty, 100.0)
            myBox.SetValue(Canvas.TopProperty, 100.0)


        ElseIf ((newTop > 100) AndAlso (newLeft < 70)) Then
            myBox.SetValue(Canvas.LeftProperty, 20.0)
            myBox.SetValue(Canvas.TopProperty, 100.0)
        ElseIf ((newTop < 70) AndAlso (newLeft > 100)) Then
            myBox.SetValue(Canvas.LeftProperty, 100.0)
            myBox.SetValue(Canvas.TopProperty, 20.0)
        Else
            myBox.SetValue(Canvas.LeftProperty, 20.0)
            myBox.SetValue(Canvas.TopProperty, 20.0)
        End If

        mouseTop = -1
        mouseLeft = -1
    End Sub '   Box_MouseLeftButtonUp

    Private Sub Box_MouseMove(sender As Object, e As Mouseeventargs)

        StatusXPosition.Text = e.GetPosition(Nothing).X.ToString()
        StatusYPosition.Text = e.GetPosition(Nothing).Y.ToString()

        Dim myBox As Rectangle = CType(sender, Rectangle)

        If (isMouseCaptured) Then

            '  Calculate the current position of the object.
            Dim CurrentTop As Double = e.GetPosition(Nothing).Y - mouseTop
            Dim CurrentLeft As Double = e.GetPosition(Nothing).X - mouseLeft
            Dim newTop As Double = CurrentTop + CType(myBox.GetValue(Canvas.TopProperty), Double)
            Dim newLeft As Double = CurrentLeft + CType(myBox.GetValue(Canvas.LeftProperty), Double)
            '  Set New position for the Box .
            myBox.SetValue(Canvas.TopProperty, newTop)
            myBox.SetValue(Canvas.LeftProperty, newLeft)
            '  Update global top/left positions .
            mouseTop = e.GetPosition(Nothing).Y
            mouseLeft = e.GetPosition(Nothing).X
        End If
    End Sub '   Box_MouseMove

    Private Sub UpdateLogTable(EventName As String)

        If (LogTable.Text = "") Then
            LogTable.Text = EventName
        Else
            LogTable.Text = LogTable.Text + "\n" + EventName
        End If
    End Sub '   UpdateLogTable

    Private Sub HyperlinkButton_Click(sender As Object, e As Routedeventargs)

        LogTable.Text = ""
    End Sub '   HyperlinkButton_Click
End Class   '   PageMouseCapture
' End Namespace 
' ..\RichInternetApplication\2-Input\Pages\2-MouseCapture.xaml.cs
