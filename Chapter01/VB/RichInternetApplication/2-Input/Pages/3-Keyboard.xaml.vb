Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media

' Namespace InputEvents

Partial Public Class PageKeyboard
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub WaterMarkTitle_GotFocus(sender As Object, e As Routedeventargs)

        WaterMarkTitle.Text = ""
        WaterMarkTitle.Foreground = New SolidColorBrush(Colors.Black)
        UpdateLogTable("GotFocus: WaterMarkTitle")
    End Sub '   WaterMarkTitle_GotFocus

    Private Sub NumberTextBox_GotFocus(sender As Object, e As Routedeventargs)

        UpdateLogTable("GotFocus: NumberTextBox")
    End Sub '   NumberTextBox_GotFocus

    Private Sub Button_GotFocus(sender As Object, e As Routedeventargs)

        UpdateLogTable("GotFocus: Button")
    End Sub '   Button_GotFocus

    Private Sub WaterMarkTitle_LostFocus(sender As Object, e As Routedeventargs)

        If (WaterMarkTitle.Text = "") Then

            WaterMarkTitle.Text = "Enter Title..."
            WaterMarkTitle.Foreground = New SolidColorBrush(Colors.Gray)
        End If

        UpdateLogTable("LostFocus: WaterMarkTitle")
    End Sub '   WaterMarkTitle_LostFocus

    Private Sub NumberTextBox_LostFocus(sender As Object, e As Routedeventargs)

        UpdateLogTable("LostFocus: NumberTextBox")
    End Sub '   NumberTextBox_LostFocus

    Private Sub Button_LostFocus(sender As Object, e As Routedeventargs)

        UpdateLogTable("LostFocus: Button")
    End Sub '   Button_LostFocus

    Private Sub NumberTextBox_KeyDown(sender As Object, e As Keyeventargs)

        If ((e.Key.ToString() = "D0") OrElse (e.Key.ToString() = "D1") OrElse (e.Key.ToString() = "D2") OrElse (e.Key.ToString() = "D3") OrElse (e.Key.ToString() = "D4") OrElse (e.Key.ToString() = "D5") OrElse (e.Key.ToString() = "D6") OrElse (e.Key.ToString() = "D7") OrElse (e.Key.ToString() = "D8") OrElse (e.Key.ToString() = "D9")) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub '   NumberTextBox_KeyDown

    Private Sub KeyBox_KeyUp(sender As Object, e As Keyeventargs)

        UpdateLogTable("Keyup: " + e.Key.ToString())

        If (Keyboard.Modifiers.ToString() <> "none") Then
            UpdateLogTable("Keyboard: " + Keyboard.Modifiers.ToString())
        End If
        LogTable.Text = LogTable.Text + "\n----------------------------"
    End Sub '   KeyBox_KeyUp

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
    ' 
    '  added
    ' 
    Private Sub UserControl_Loaded(sender As Object, e As Routedeventargs)

        'MessageBox.Show("UserControl_Loaded " + e.OriginalSource.ToString())
    End Sub '   UserControl_Loaded

    Private Sub InputMethod_TextChanged(sender As Object, e As Routedeventargs)

        'MessageBox.Show("InputMethod_TextChanged " + e.OriginalSource.ToString())
    End Sub '   InputMethod_TextChanged
End Class   '   PageKeyboard
' End Namespace 
' ..\RichInternetApplication\2-Input\Pages\3-Keyboard.xaml.cs
