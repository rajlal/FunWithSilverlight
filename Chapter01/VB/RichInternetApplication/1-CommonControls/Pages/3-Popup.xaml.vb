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
Imports System.Windows.Controls.Primitives

' Namespace CommonControls

Partial Public Class PagePopup
    Inherits UserControl

    Dim gridPopup As Popup
    Public Sub New()

        InitializeComponent()

    End Sub '   New

    Private Sub Button_Click(sender As Object, e As Routedeventargs)

    End Sub '   Button_Click

    Private Sub ShowPopup()

        '  Create a popup. 
        gridPopup = New Popup()
        '  Set the Child property of Popup to an instance of MyControl. 
        gridPopup.Child = New TestPopup()
        '  Set where the popup will show up on the screen. 
        gridPopup.VerticalOffset = 100
        gridPopup.HorizontalOffset = 100
        '  Open the popup. 
        gridPopup.IsOpen = True
    End Sub '   ShowPopup

    Private Sub TextBlock_MouseEnter(sender As Object, e As Mouseeventargs)

        ShowPopup()
    End Sub '   TextBlock_MouseEnter

    Private Sub TextBlock_MouseLeave(sender As Object, e As Mouseeventargs)

        gridPopup.IsOpen = False
    End Sub '   TextBlock_MouseLeave
End Class   '   PagePopup
' End Namespace 
' ..\RichInternetApplication\1-CommonControls\Pages\3-Popup.xaml.cs
