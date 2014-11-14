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
    public class MemoryStreamAudioSink : AudioSink
    {
        protected override void OnCaptureStarted()
        {
            stream = new MemoryStream();
        }
        protected override void OnCaptureStopped()
        {
        }
        public AudioFormat AudioFormat
        {
            get
            {
                return (audioFormat);
            }
        }
        public MemoryStream AudioData
        {
            get
            {
                return (stream);
            }
        }
        protected override void OnFormatChange(AudioFormat audioFormat)
        {
            if (this.audioFormat == null)
            {
                this.audioFormat = audioFormat;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        protected override void OnSamples(long sampleTime, long sampleDuration, byte[] sampleData)
        {
            stream.Write(sampleData, 0, sampleData.Length);
        }
        MemoryStream stream;
        AudioFormat audioFormat;
    } 
}
