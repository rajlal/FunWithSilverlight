Imports System
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes
Imports System.IO

' Namespace WebCam

Public Class MemoryStreamAudioSink
    Inherits AudioSink

    Protected Overrides Sub OnCaptureStarted()

        _stream = New MemoryStream()
    End Sub '   OnCaptureStarted

    Protected Overrides Sub OnCaptureStopped()

    End Sub '   OnCaptureStopped

    Public ReadOnly Property oAudioFormat() As AudioFormat
        Get
            Return (_audioFormat)
        End Get
    End Property

    Public ReadOnly Property AudioData() As MemoryStream
        Get

            Return (_stream)
        End Get
    End Property

    Protected Overrides Sub OnFormatChange(oAudioFormat As AudioFormat)

        If (Me._audioFormat Is Nothing) Then
            Me._audioFormat = oaudioFormat
        Else
            Throw New InvalidOperationException()
        End If
    End Sub '   OnFormatChange

    Protected Overrides Sub OnSamples(sampleTime As Long, sampleDuration As Long, sampleData As Byte())

        _stream.Write(sampleData, 0, sampleData.Length)
    End Sub '   OnSamples

    Dim _stream As MemoryStream
    Dim _audioFormat As AudioFormat
End Class   '   MemoryStreamAudioSink
' End Namespace   '   WebCam
' ..\Project_10\AdvancedMedia\3-Webcam\WebCam\MemoryStreamAudioSink.cs
