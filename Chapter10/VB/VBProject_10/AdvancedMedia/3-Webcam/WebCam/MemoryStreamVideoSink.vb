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

Public Class MemoryStreamVideoSink
    Inherits VideoSink

    Private _stream As Stream

    Private _Video As Video

    Public Sub New()

    End Sub

    Public Property CurrentVideo() As Video
        Get

            Return _Video
        End Get
        Private Set(value As Video)

            _Video = value
        End Set
    End Property

    Protected Overrides Sub OnCaptureStarted()

        CurrentVideo = New Video()
        _stream = New MemoryStream()
    End Sub '   OnCaptureStarted

    Protected Overrides Sub OnCaptureStopped()

        Dim cABuffer As Byte()

        ReDim cabuffer(CType(_stream.Length, Integer))

        _stream.Position = 0
        _stream.Read(cABuffer, 0, cABuffer.Length)
        _stream.Close()
        CurrentVideo.Data = cABuffer
    End Sub '   OnCaptureStopped

    Protected Overrides Sub OnFormatChange(ovideoFormat As VideoFormat)

        CurrentVideo.Width = ovideoFormat.PixelWidth
        CurrentVideo.Height = ovideoFormat.PixelHeight
        CurrentVideo.FramesPerSecond = ovideoFormat.FramesPerSecond
    End Sub '   OnFormatChange

    Protected Overrides Sub OnSample(sampleTime As Long, frameDuration As Long, sampleData As Byte())

        CurrentVideo.SampleSize = sampleData.Length
        _stream.Write(sampleData, 0, sampleData.Length)
    End Sub '   OnSample
End Class   '   MemoryStreamVideoSink:
' End Namespace   '   WebCam
' ..\Project_10\AdvancedMedia\3-Webcam\WebCam\MemoryStreamVideoSink.cs
