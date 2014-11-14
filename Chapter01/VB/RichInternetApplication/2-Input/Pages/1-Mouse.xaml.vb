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

Partial Public Class PageMouse
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
        '            AddHandler MouseButtonEventHandler(DynamicYellowBox_MouseLeftButtonDown), New YellowBox.MouseLeftButtonDown
        '            AddHandler MouseButtonEventHandler(DynamicYellowBox_MouseLeftButtonUp), New YellowBox.MouseLeftButtonUp
    End Sub '   New

    Private Sub WhiteCanvas_MouseEnter(sender As Object, e As Mouseeventargs)

        UpdateStatus("MouseEnter: WhiteBlock")
    End Sub '   WhiteCanvas_MouseEnter

    Private Sub WhiteCanvas_MouseLeave(sender As Object, e As Mouseeventargs)

        UpdateStatus("MouseLeave: WhiteBlock")
    End Sub '   WhiteCanvas_MouseLeave

    Private Sub WhiteCanvas_MouseMove(sender As Object, e As Mouseeventargs)

        '  MouseButtonEventArgs position relative to the Item
        StatusXPosition.Text = e.GetPosition(CType(sender, UIElement)).X.ToString()
        StatusYPosition.Text = e.GetPosition(CType(sender, UIElement)).Y.ToString()
    End Sub '   WhiteCanvas_MouseMove

    Private Sub WhiteCanvas_MouseLeftButtonDown(sender As Object, e As Mousebuttoneventargs)

        UpdateLogTable("LeftButtonDown: WhiteBlock")
    End Sub '   WhiteCanvas_MouseLeftButtonDown

    Private Sub WhiteCanvas_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        UpdateLogTable("LeftButtonUp: WhiteBlock")
        LogTable.Text = LogTable.Text + "\n----------------------------"
    End Sub '   WhiteCanvas_MouseLeftButtonUp

    Private Sub RedBox_MouseEnter(sender As Object, e As Mouseeventargs)

        UpdateStatus("MouseEnter: RedBox")
    End Sub '   RedBox_MouseEnter

    Private Sub RedBox_MouseLeave(sender As Object, e As Mouseeventargs)

        UpdateStatus("MouseLeave: RedBox")
    End Sub '   RedBox_MouseLeave

    Private Sub RedBox_MouseMove(sender As Object, e As Mouseeventargs)

        '  MouseButtonEventArgs position relative to the Silverlight Plug-in
        StatusXPosition.Text = e.GetPosition(Nothing).X.ToString()
        StatusYPosition.Text = e.GetPosition(Nothing).Y.ToString()
    End Sub '   RedBox_MouseMove

    Private Sub RedBox_MouseLeftButtonDown(sender As Object, e As Mousebuttoneventargs)

        UpdateLogTable("LeftButtonDown: RedBox")
    End Sub '   RedBox_MouseLeftButtonDown

    Private Sub RedBox_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        UpdateLogTable("LeftButtonUp: RedBox")
    End Sub '   RedBox_MouseLeftButtonUp


    Private Sub GreenBox_MouseEnter(sender As Object, e As Mouseeventargs)

        UpdateStatus("MouseEnter: GreenBox")
    End Sub '   GreenBox_MouseEnter

    Private Sub GreenBox_MouseLeave(sender As Object, e As Mouseeventargs)

        UpdateStatus("MouseLeave: GreenBox")
    End Sub '   GreenBox_MouseLeave

    Private Sub GreenBox_MouseMove(sender As Object, e As Mouseeventargs)

        '  MouseButtonEventArgs position relative to the Silverlight Plug-in
        StatusXPosition.Text = e.GetPosition(Nothing).X.ToString()
        StatusYPosition.Text = e.GetPosition(Nothing).Y.ToString()
    End Sub '   GreenBox_MouseMove

    Private Sub GreenBox_MouseLeftButtonDown(sender As Object, e As Mousebuttoneventargs)

        UpdateLogTable("LeftButtonDown: GreenBox")
        e.Handled = True
    End Sub '   GreenBox_MouseLeftButtonDown

    Private Sub GreenBox_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        UpdateLogTable("LeftButtonUp: GreenBox")
        e.Handled = True
        LogTable.Text = LogTable.Text + "\n----------------------------"
    End Sub '   GreenBox_MouseLeftButtonUp

    Private Sub BlueBox_MouseEnter(sender As Object, e As Mouseeventargs)

        UpdateStatus("MouseEnter: BlueBox")
    End Sub '   BlueBox_MouseEnter

    Private Sub BlueBox_MouseLeave(sender As Object, e As Mouseeventargs)

        UpdateStatus("MouseLeave: BlueBox")
    End Sub '   BlueBox_MouseLeave

    Private Sub YellowBox_MouseEnter(sender As Object, e As Mouseeventargs)

        UpdateStatus("MouseEnter: YellowBox")
    End Sub '   YellowBox_MouseEnter

    Private Sub YellowBox_MouseLeave(sender As Object, e As Mouseeventargs)

        UpdateStatus("MouseLeave: YellowBox")

    End Sub '   YellowBox_MouseLeave

    Private Sub DynamicYellowBox_MouseLeftButtonDown(sender As Object, e As Mousebuttoneventargs) Handles YellowBox.MouseLeftButtonDown

        UpdateLogTable("LeftButtonDown: YellowBox")
    End Sub '   DynamicYellowBox_MouseLeftButtonDown

    Private Sub DynamicYellowBox_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs) Handles YellowBox.MouseLeftButtonUp

        UpdateLogTable("LeftButtonUp: YellowBox")
    End Sub '   DynamicYellowBox_MouseLeftButtonUp

    Private Sub ResetLogTable()

        LogTable.Text = ""
    End Sub '   ResetLogTable

    Private Sub UpdateStatus(EventName As String)

        StatusBar.Text = EventName
    End Sub '   UpdateStatus

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
End Class   '   PageMouse
' End Namespace 
' ..\RichInternetApplication\2-Input\Pages\1-Mouse.xaml.cs
