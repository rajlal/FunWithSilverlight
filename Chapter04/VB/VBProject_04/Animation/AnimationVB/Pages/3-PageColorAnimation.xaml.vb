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

    Public Partial class PageColorAnimation
        Inherits UserControl

    Private flagPaused As Boolean
    Private SelectedAnim As String = "Basic"
    Private myStoryboard As Storyboard = New Storyboard()

        Public Sub New()

            InitializeComponent()
        End Sub '   New


        private Sub btnPlay_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

            GetStoryboard()
            myStoryboard.Stop()
            myStoryboard.Begin()

        End Sub '   btnPlay_MouseLeftButtonUp

        private Sub btnPause_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

            GetStoryboard()

            If (flagPaused) Then

                myStoryboard.Resume()
                flagPaused = false


            else
            myStoryboard.Pause()
                flagPaused = true
        End If

        End Sub '   btnPause_MouseLeftButtonUp

        private Sub btnStop_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

            GetStoryboard()
            myStoryboard.Stop()
        End Sub '   btnStop_MouseLeftButtonUp

        private Sub ShowBasic(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            CanvasBasic.Visibility = Visibility.Visible
            SelectedAnim = "Basic"
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
            StatusInfo.Text = "Animation TargetProperty: Color"
        End Sub '   ShowBasic

        private Sub ShowGradient(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            CanvasGradient.Visibility = Visibility.Visible
            SelectedAnim = "Gradient"
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
            StatusInfo.Text = "Animation Target: GradientStop"
        End Sub '   ShowGradient

        private Sub ShowRainbow(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            CanvasRainbow.Visibility = Visibility.Visible
            SelectedAnim = "Rainbow"
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
            StatusInfo.Text = "Animation TargetProperty: Ellipse.Fill"
        End Sub '   ShowRainbow



        private Sub CollapseAll()

            myStoryboard.Stop()
            CanvasBasic.Visibility = Visibility.Collapsed
            CanvasGradient.Visibility = Visibility.Collapsed
            CanvasRainbow.Visibility = Visibility.Collapsed
        End Sub '   CollapseAll

    Private Function GetStoryboard() As Storyboard

        If (SelectedAnim = "Basic") Then

            myStoryboard = myBasicStoryboard


        ElseIf (SelectedAnim = "Gradient") Then
            myStoryboard = myGradientStoryboard
        Else
            myStoryboard = myRainbowStoryboard
        End If

        Return myStoryboard
    End Function '   Ellipse_MouseLeftButtonUp

    Private Sub Ellipse_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, Ellipse)).ToString()
    End Sub


End Class   '   PageColorAnimation

' End Namespace 
' ..\Animation\Animation\Pages\3-PageColorAnimation.xaml.cs
