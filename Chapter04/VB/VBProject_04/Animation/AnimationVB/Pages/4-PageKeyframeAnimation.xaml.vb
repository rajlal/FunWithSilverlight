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

Partial Public Class PageKeyframeAnimation
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

    Private Sub showDiscrete(sender As Object, e As Mousebuttoneventargs)

        CollapseAll()
        CanvasDiscrete.Visibility = Visibility.Visible
        SelectedAnim = "Discrete"
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   showDiscrete

    Private Sub showLinear(sender As Object, e As Mousebuttoneventargs)

        CollapseAll()
        CanvasLinear.Visibility = Visibility.Visible
        SelectedAnim = "Linear"
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   showLinear

    Private Sub showSplined(sender As Object, e As Mousebuttoneventargs)

        CollapseAll()
        CanvasSplined.Visibility = Visibility.Visible
        SelectedAnim = "Splined"
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   showSplined

    Private Sub CollapseAll()

        myStoryboard.Stop()
        CanvasDiscrete.Visibility = Visibility.Collapsed
        CanvasLinear.Visibility = Visibility.Collapsed
        CanvasSplined.Visibility = Visibility.Collapsed
    End Sub '   CollapseAll

    Private Function GetStoryboard() As Storyboard

        If (SelectedAnim = "Linear") Then
            myStoryboard = myLinearStoryboard
        ElseIf (SelectedAnim = "Discrete") Then
            myStoryboard = myDiscreteStoryboard
        Else
            myStoryboard = mySplinedStoryboard
        End If
        Return myStoryboard
    End Function
End Class   '   PageKeyframeAnimation
' End Namespace 
' ..\Animation\Animation\Pages\4-PageKeyframeAnimation.xaml.cs
