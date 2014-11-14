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

' Namespace Animation

Partial Public Class PagePointAnimation
    Inherits UserControl

    Private flagPaused As Boolean
    Private SelectedAnim As String = "Basic"
    Private myStoryboard As Storyboard = New Storyboard()

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub btnPlay_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        GetStoryboard()
        myStoryboard.Stop()
        myStoryboard.Begin()
    End Sub '   btnPlay_MouseLeftButtonUp

    Private Sub btnPause_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        GetStoryboard()

        If (flagPaused) Then
            myStoryboard.Resume()
            flagPaused = False
        Else
            myStoryboard.Pause()
            flagPaused = True
        End If
    End Sub '   btnPause_MouseLeftButtonUp

    Private Sub btnStop_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        GetStoryboard()
        myStoryboard.Stop()
    End Sub '   btnStop_MouseLeftButtonUp

    Private Sub ShowBasic(sender As Object, e As Mousebuttoneventargs)

        CollapseAll()
        CanvasBasic.Visibility = Visibility.Visible
        SelectedAnim = "Basic"
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   ShowBasic

    Private Sub ShowEclipse(sender As Object, e As Mousebuttoneventargs)

        CollapseAll()
        CanvasEclipse.Visibility = Visibility.Visible
        SelectedAnim = "Eclipse"
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   ShowEclipse

    Private Sub ShowSunrise(sender As Object, e As Mousebuttoneventargs)

        CollapseAll()
        CanvasSunrise.Visibility = Visibility.Visible
        SelectedAnim = "Sunrise"
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   ShowSunrise

    Private Sub CollapseAll()

        myStoryboard.Stop()
        CanvasBasic.Visibility = Visibility.Collapsed
        CanvasEclipse.Visibility = Visibility.Collapsed
        CanvasSunrise.Visibility = Visibility.Collapsed
    End Sub '   CollapseAll

    Private Function GetStoryboard() As Storyboard

        If (SelectedAnim = "Basic") Then
            myStoryboard = myBasicStoryboard
        ElseIf (SelectedAnim = "Eclipse") Then
            myStoryboard = myEclipseStoryboard
        Else
            myStoryboard = mySunriseStoryboard
        End If

        Return myStoryboard
    End Function
End Class   '   PagePointAnimation

' End Namespace 
' ..\Animation\Animation\Pages\2-PagePointAnimation.xaml.cs
