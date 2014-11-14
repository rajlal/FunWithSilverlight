Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives

' Namespace CommonControls

Partial Public Class PageRepeatButton
    Inherits UserControl

    Dim Clicks As Integer = 0
    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub RepeatButton_Click(sender As Object, e As Routedeventargs)

        Clicks += 1
        textValue.Text = "Value: " + Clicks.ToString()
    End Sub '   RepeatButton_Click

    Private Sub RepeatButton_Click_Increase(sender As Object, e As Routedeventargs)

        Clicks += 1
        textValue.Text = "Value: " + Clicks.ToString()
    End Sub '   RepeatButton_Click_Increase

    Private Sub RepeatButton_Click_Decrease(sender As Object, e As Routedeventargs)

        Clicks -= 1
        textValue.Text = "Value: " + Clicks.ToString()
    End Sub '   RepeatButton_Click_Decrease
End Class   '   PageRepeatButton
' End Namespace 
' ..\RichInternetApplication\1-CommonControls\Pages\5-RepeatButton.xaml.cs
