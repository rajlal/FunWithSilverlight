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

    Public Partial class PageDoubleAnimation
        Inherits UserControl

    Private flagPaused As Boolean
    Private Selected As String = "Truck"
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


        Else
            myStoryboard.Pause()
            flagPaused = True
        End If

        End Sub '   btnPause_MouseLeftButtonUp

        private Sub btnStop_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

            GetStoryboard()
            myStoryboard.Stop()
        End Sub '   btnStop_MouseLeftButtonUp


        private Sub ShowBasic(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            canvasBasic.Visibility = Visibility.Visible
            SelectedAnim = "Basic"
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   ShowBasic


        private Sub ShowSpeed(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            canvasSpeed.Visibility = Visibility.Visible
            SelectedAnim = "Speed"
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()

        End Sub '   ShowSpeed

    Private Sub sliderFastDuration_ValueChanged(sender As Object, e As Routedpropertychangedeventargs(Of Double))

        Try

            myStoryboard.Stop()

            Dim animDuration As TimeSpan = New TimeSpan(0, 0, 0, 0, CType(sliderFastDuration.Value, Integer))
            animFastCar.Duration = animDuration
            txtFastCarDuration.Text = "." + CStr(CType(sliderFastDuration.Value, Integer))


        Catch ex As Exception

        End Try

    End Sub '   sliderFastDuration_ValueChanged

        private Sub ShowRotation(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            canvasRotation.Visibility = Visibility.Visible
            SelectedAnim = "Rotation"
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   ShowRotation


        private Sub ShowRace(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            canvasRace.Visibility = Visibility.Visible
            SelectedAnim = "Race"
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()

        End Sub '   ShowRace

    Private Sub sliderDuration_ValueChanged(sender As Object, e As Routedpropertychangedeventargs(Of Double))


        Try

            myStoryboard.Stop()
            Dim animHours As Double = sliderDuration.Value / 3600.0
            Dim minutesLeft As Double = sliderDuration.Value Mod 3600.0
            Dim animMinutes As Double = minutesLeft / 60.0
            Dim animSeconds As Double = minutesLeft Mod 60.0

            Dim animDuration As TimeSpan = New TimeSpan(CType(animHours, Integer), CType(animMinutes, Integer), CType(animSeconds, Integer))

            If (Selected = "Truck") Then
                animDoubleTruck.Duration = animDuration
            Else
                animDoubleCar.Duration = animDuration
            End If

            txtDuration.Text = "Truck: " + CStr(animDoubleTruck.Duration.TimeSpan.Hours) + ":" + CStr(animDoubleTruck.Duration.TimeSpan.Minutes) + ":" + CStr(animDoubleTruck.Duration.TimeSpan.Seconds)
            txtDuration.Text += " / Car:" + CStr(animDoubleCar.Duration.TimeSpan.Hours) + ":" + CStr(animDoubleCar.Duration.TimeSpan.Minutes) + ":" + CStr(animDoubleCar.Duration.TimeSpan.Seconds)
        Catch zx As Exception

        End Try

    End Sub '   sliderDuration_ValueChanged

        private Sub ImgTruck_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

            Selected = "Truck"
        txtDuration.Text = "Truck: " + CStr(animDoubleTruck.Duration.TimeSpan.Hours) + ":" + CStr(animDoubleTruck.Duration.TimeSpan.Minutes) + ":" + CStr(animDoubleTruck.Duration.TimeSpan.Seconds)
        txtDuration.Text += " / Car:" + CStr(animDoubleCar.Duration.TimeSpan.Hours) + ":" + CStr(animDoubleCar.Duration.TimeSpan.Minutes) + ":" + CStr(animDoubleCar.Duration.TimeSpan.Seconds)
            StatusBar.Text = "Selected: Truck"


        End Sub '   ImgTruck_MouseLeftButtonUp

        private Sub ImgCar_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

            Selected = "Car"
        txtDuration.Text = "Truck: " + CStr(animDoubleTruck.Duration.TimeSpan.Hours) + ":" + CStr(animDoubleTruck.Duration.TimeSpan.Minutes) + ":" + CStr(animDoubleTruck.Duration.TimeSpan.Seconds)
        txtDuration.Text += " / Car:" + CStr(animDoubleCar.Duration.TimeSpan.Hours) + ":" + CStr(animDoubleCar.Duration.TimeSpan.Minutes) + ":" + CStr(animDoubleCar.Duration.TimeSpan.Seconds)
            StatusBar.Text = "Selected: Car"

        End Sub '   ImgCar_MouseLeftButtonUp

        private Sub CollapseAll()

            myStoryboard.Stop()
            canvasBasic.Visibility = Visibility.Collapsed
            canvasRotation.Visibility = Visibility.Collapsed
            canvasRace.Visibility = Visibility.Collapsed
            canvasSpeed.Visibility = Visibility.Collapsed
        End Sub '   CollapseAll

    Private Function GetStoryboard() As Storyboard

        If (SelectedAnim = "Basic") Then

            myStoryboard = myBasicStoryboard


        ElseIf (SelectedAnim = "Speed") Then
            myStoryboard = mySpeedStoryboard

        ElseIf (SelectedAnim = "Rotation") Then
            myStoryboard = myRotationStoryboard
        Else
            myStoryboard = myRaceStoryboard
        End If
        Return myStoryboard
    End Function




End Class   '   PageDoubleAnimation

' End Namespace 
' ..\Animation\Animation\Pages\1-PageDoubleAnimation.xaml.cs
