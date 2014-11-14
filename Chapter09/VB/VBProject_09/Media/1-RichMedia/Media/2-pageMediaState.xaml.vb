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
Imports System.Windows.Threading

' Namespace Media

Partial Public Class pageMediaState
    Inherits UserControl

    Dim timer As DispatcherTimer = New DispatcherTimer()

    Private MediaRewindState As Boolean = False
    Private MediaFastForwardState As Boolean = False
    Private myStoryboard As Storyboard = New Storyboard()

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub ShowMedia(sender As Object, e As MouseButtonEventArgs)

        UpdateMediaState()
        Media.Stop()
        btnPlaybig.Visibility = Visibility.Visible
        Media.Visibility = Visibility.Collapsed
    End Sub '   ShowMedia

    Private Sub btnPlay_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        PlayMedia()
    End Sub '   btnPlay_MouseLeftButtonUp

    Private Sub PlayMedia()

        UpdateMediaState()

        If (timer.IsEnabled) Then
            timer.Stop()
        Else
            timer.Start()
        End If

        AddHandler timer.Tick,
                       Sub(s As Object, args As EventArgs)
                           StatusPosition.Text = Media.Position.Hours.ToString() + ":" +
                                               Media.Position.Minutes.ToString() + ":" +
                                               Media.Position.Seconds.ToString() + " / " +
                                               Media.NaturalDuration.TimeSpan.Hours.ToString() + ":" +
                                               Media.NaturalDuration.TimeSpan.Minutes.ToString() + ":" +
                                               Media.NaturalDuration.TimeSpan.Seconds.ToString()
                       End Sub
        '  one second
        timer.Interval = New TimeSpan(0, 0, 0, 1)
        timer.Start()

        Media.Play()
    End Sub '   PlayMedia

    Private Sub UpdateMediaState()

        btnPlaybig.Visibility = Visibility.Collapsed
        Media.Visibility = Visibility.Visible

        timer.Stop()

        If (MediaRewindState OrElse MediaFastForwardState) Then
            MediaRewindState = False
            MediaFastForwardState = False
            Media.Volume = 0.5
            txtVolume.Text = String.Format("{0:0.0}", Media.Volume)
        End If
    End Sub '   UpdateMediaState
    Private Sub btnPause_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        UpdateMediaState()
        Media.Pause()
    End Sub '   btnPause_MouseLeftButtonUp

    Private Sub btnStop_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        UpdateMediaState()
        Media.Stop()
    End Sub '   btnStop_MouseLeftButtonUp

    Private Sub btnRewind_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        If (timer.IsEnabled) Then
            timer.Stop()
        Else
            timer.Start()
        End If

        Media.Volume = 0
        txtVolume.Text = String.Format("{0:0.0}", Media.Volume)
        MediaRewindState = True
        AddHandler timer.Tick,
                       Sub(s As Object, args As EventArgs)

                           If (Media.CanSeek) Then
                               Media.Position = Media.Position.Subtract(New TimeSpan(0, 0, 0, 1, 0))
                           End If

                           StatusPosition.Text = Media.Position.Hours.ToString() + ":" +
                                                Media.Position.Minutes.ToString() + ":" +
                                                Media.Position.Seconds.ToString() + " / " +
                                                Media.NaturalDuration.TimeSpan.Hours.ToString() + ":" +
                                                Media.NaturalDuration.TimeSpan.Minutes.ToString() + ":" +
                                                Media.NaturalDuration.TimeSpan.Seconds.ToString()
                       End Sub

        '  one second
        timer.Interval = New TimeSpan(0, 0, 0, 0, 200)
        timer.Start()
    End Sub '   btnRewind_MouseLeftButtonUp

    Private Sub btnFastForward_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        If (timer.IsEnabled) Then
            timer.Stop()
        Else
            timer.Start()
        End If

        Media.Volume = 0
        txtVolume.Text = String.Format("{0:0.0}", Media.Volume)
        MediaRewindState = True
        AddHandler timer.Tick,
                       Sub(s As Object, args As EventArgs)

                           If (Media.CanSeek) Then
                               Media.Position = Media.Position.Add(New TimeSpan(0, 0, 0, 1, 0))
                           End If

                           StatusPosition.Text = Media.Position.Hours.ToString() + ":" +
                                                Media.Position.Minutes.ToString() + ":" +
                                                Media.Position.Seconds.ToString() + " / " +
                                                Media.NaturalDuration.TimeSpan.Hours.ToString() + ":" +
                                                Media.NaturalDuration.TimeSpan.Minutes.ToString() + ":" +
                                                Media.NaturalDuration.TimeSpan.Seconds.ToString()
                       End Sub

        '  one second
        timer.Interval = New TimeSpan(0, 0, 0, 0, 200)
        timer.Start()
    End Sub '   btnFastForward_MouseLeftButtonUp

    Private Sub btnFullscreen_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        MakeFullScreen()
    End Sub '   btnFullscreen_MouseLeftButtonUp

    Private Sub btnVolumeDown_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        Media.Volume = Media.Volume - 0.1
        txtVolume.Text = String.Format("{0:0.0}", Media.Volume)
    End Sub '   btnVolumeDown_MouseLeftButtonUp

    Private Sub btnVolumeup_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        Media.Volume = Media.Volume + 0.1
        txtVolume.Text = String.Format("{0:0.0}", Media.Volume)
    End Sub '   btnVolumeup_MouseLeftButtonUp

    Private Sub Media_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        MakeFullScreen()
    End Sub '   Media_MouseLeftButtonUp

    Private Sub MakeFullScreen()

        Dim host As System.Windows.Interop.SilverlightHost = Application.Current.Host
        AddHandler host.Content.FullScreenChanged, AddressOf Content_FullScreenChanged 'EventHandler()
        host.Content.IsFullScreen = True
    End Sub '   MakeFullScreen

    Sub Content_FullScreenChanged(sender As Object, e As EventArgs)

        Dim host As System.Windows.Interop.SilverlightHost = Application.Current.Host

        If (host.Content.IsFullScreen) Then
            '  because canvasMedia holding the media has canvas.left property=40 so equal gap on both sides=80
            Media.Width = Application.Current.Host.Content.ActualWidth - 80
            '  canvasMedia has the canvas.top property=30 so equal space in top and bottom
            Media.Height = Application.Current.Host.Content.ActualHeight - 60
            borderMediastates.Visibility = Visibility.Collapsed
            borderStatus.Visibility = Visibility.Collapsed
        Else
            Media.Width = 320
            Media.Height = 200
            borderMediastates.Visibility = Visibility.Visible
            borderStatus.Visibility = Visibility.Visible
        End If
    End Sub '   Content_FullScreenChanged

    Private Sub Media_CurrentStateChanged(sender As Object, e As RoutedEventArgs)

        StatusBar.Text = Media.CurrentState.ToString()
        StatusPosition.Text = Media.Position.Hours.ToString() + ":" + Media.Position.Minutes.ToString() + ":" + Media.Position.Seconds.ToString() + " / " + Media.NaturalDuration.TimeSpan.Hours.ToString() + ":" + Media.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + Media.NaturalDuration.TimeSpan.Seconds.ToString()
    End Sub '   Media_CurrentStateChanged


    Private Sub Media_BufferingProgressChanged(sender As Object, e As RoutedEventArgs)

        StatusBar.Text = Media.CurrentState.ToString()
        StatusPosition.Text = Media.Position.Hours.ToString() + ":" + Media.Position.Minutes.ToString() + ":" + Media.Position.Seconds.ToString() + " / " + Media.NaturalDuration.TimeSpan.Hours.ToString() + ":" + Media.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + Media.NaturalDuration.TimeSpan.Seconds.ToString()
    End Sub '   Media_BufferingProgressChanged


    Private Sub Image_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        PlayMedia()
    End Sub '   Image_MouseLeftButtonUp
End Class   '   pageMediaState
' End Namespace   '   Media
' ..\Project_09\Media\1-RichMedia\Media\2-pageMediaState.xaml.cs
