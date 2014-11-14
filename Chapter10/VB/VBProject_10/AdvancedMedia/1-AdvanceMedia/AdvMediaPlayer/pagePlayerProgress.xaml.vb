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

'Namespace AdvMediaPlayer

Partial Public Class pagePlayerProgress
    Inherits UserControl

    Dim timer As DispatcherTimer = New DispatcherTimer()

    Public Sub New()

        InitializeComponent()
    End Sub

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
                           Sub(s As Object, arg As EventArgs)
                               '  StatusPosition.Text = Media.Position.Hours.ToString() + ":" + Media.Position.Minutes.ToString() + ":" + Media.Position.Seconds.ToString() + " / " + Media.NaturalDuration.TimeSpan.Hours.ToString() + ":" + Media.NaturalDuration.TimeSpan.Minutes.ToString() + ":" + Media.NaturalDuration.TimeSpan.Seconds.ToString()

                               StatusPosition.Text = String.Format("{0:D2}", Media.Position.Minutes) + ":" +
                                                        String.Format("{0:D2}", Media.Position.Seconds) + " / " +
                                                        String.Format("{0:D2}", Media.NaturalDuration.TimeSpan.Minutes) + ":" +
                                                        String.Format("{0:D2}", Media.NaturalDuration.TimeSpan.Seconds)

                               '  update progress bar
                               Dim current As Double = Media.Position.TotalMilliseconds
                               Dim total As Double = Media.NaturalDuration.TimeSpan.TotalMilliseconds

                               ProgressBar.Value = current / total * 240

                               Canvas.SetLeft(scrubBar, ProgressBar.Value)
                           End Sub

        '  ten millisecond
        timer.Interval = New TimeSpan(0, 0, 0, 0, 10)
        timer.Start()

        Media.Play()
    End Sub '   PlayMedia

    Private Sub UpdateMediaState()

        btnPlaybig.Visibility = Visibility.Collapsed
        Media.Visibility = Visibility.Visible

        timer.Stop()
    End Sub '   UpdateMediaState

    Private Sub btnPause_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        UpdateMediaState()
        Media.Pause()
    End Sub '   btnPause_MouseLeftButtonUp

    Private Sub btnStop_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        UpdateMediaState()
        Media.Stop()
    End Sub '   btnStop_MouseLeftButtonUp

    Private Sub Media_CurrentStateChanged(sender As Object, e As RoutedEventArgs)

        StatusPosition.Text = String.Format("{0:D2}", Media.Position.Minutes) + ":" +
                                                   String.Format("{0:D2}", Media.Position.Seconds) + " / " +
                                                   String.Format("{0:D2}", Media.NaturalDuration.TimeSpan.Minutes) + ":" +
                                                   String.Format("{0:D2}", Media.NaturalDuration.TimeSpan.Seconds)
    End Sub '   Media_CurrentStateChanged

    Private Sub Media_BufferingProgressChanged(sender As Object, e As RoutedEventArgs)

    End Sub '   Media_BufferingProgressChanged


    Private Sub Image_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        PlayMedia()
    End Sub '   Image_MouseLeftButtonUp

    Private Sub ProgressBar_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        Dim offset As Double = e.GetPosition(ProgressBar).X
        Dim barWidth As Double = ProgressBar.ActualWidth
        Dim ratio As Double = offset / barWidth

        Dim totalTime As TimeSpan = Media.NaturalDuration.TimeSpan
        Dim target As TimeSpan = New TimeSpan(0, 0, CType(totalTime.TotalSeconds * ratio, Integer))

        Media.Position = target
    End Sub '   ProgressBar_MouseLeftButtonUp
End Class   '   pagePlayerProgress:
'End Namespace   '   AdvMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer\pagePlayerProgress.xaml.cs
