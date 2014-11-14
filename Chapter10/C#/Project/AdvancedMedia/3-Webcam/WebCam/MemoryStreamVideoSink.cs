using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;

namespace WebCam
{
    public class MemoryStreamVideoSink: VideoSink
    {
        private Stream _stream;

        public MemoryStreamVideoSink()
        {
        }

        public Video CurrentVideo { get; private set; }       

        protected override void OnCaptureStarted()
        {
            CurrentVideo = new Video();
            _stream = new MemoryStream();
        }

        protected override void OnCaptureStopped()
        {
            byte[] buffer = new byte[_stream.Length];
            _stream.Position = 0;
            _stream.Read(buffer, 0, buffer.Length);
            _stream.Close();
            CurrentVideo.Data = buffer;
        }

        protected override void OnFormatChange(VideoFormat videoFormat)
        {
            CurrentVideo.Width = videoFormat.PixelWidth;
            CurrentVideo.Height = videoFormat.PixelHeight;
            CurrentVideo.FramesPerSecond = videoFormat.FramesPerSecond;
        }

        protected override void OnSample(long sampleTime, long frameDuration, byte[] sampleData)
        {
            CurrentVideo.SampleSize = sampleData.Length;
            _stream.Write(sampleData, 0, sampleData.Length);
        }
    }
  
}
