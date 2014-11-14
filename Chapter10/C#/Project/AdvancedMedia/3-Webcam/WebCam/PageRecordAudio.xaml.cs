using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;

namespace WebCam
{
    public partial class PageRecordAudio : UserControl
    {
        private CaptureSource audioCaptureSource;
        private MemoryStreamAudioSink audioSink;
       
        private int timerCount;
        private SaveFileDialog audioSaveFileDialog = new SaveFileDialog() { Filter = "Audio files (*.wav)|*.wav" };
        DispatcherTimer timer = new DispatcherTimer();

    

        public PageRecordAudio()
        {
            InitializeComponent();
            audioCaptureSource = new CaptureSource() { VideoCaptureDevice = null };
            audioCaptureSource.AudioCaptureDevice = CaptureDeviceConfiguration.GetDefaultAudioCaptureDevice();

        }

        private void btnRecord_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RecordAudio();
        }
        private void RecordAudio()
        {
            timerCount=0;
            int TimerCountMinutes=0;
            int TimerCountSeconds = 0;
            
            if (!AudioAccessAvailable())
                return;

               if (audioCaptureSource.AudioCaptureDevice == null)
                return;
            
            if (audioCaptureSource.State != CaptureState.Stopped)
                return;

         
           
            audioSink = new MemoryStreamAudioSink();
            audioSink.CaptureSource = audioCaptureSource;

            if (timer.IsEnabled) timer.Stop(); else timer.Start();

            timer.Tick +=
                           delegate(object s, EventArgs args)
                           {
                               timerCount++;
                               TimerCountMinutes= timerCount  / 60;
                               TimerCountSeconds = timerCount % 60;
                               StatusPosition.Text = string.Format("{0:D2}", TimerCountMinutes) + ":" + string.Format("{0:D2}", TimerCountSeconds);
                           };
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000); // ten millisecond
            timer.Start();

            audioCaptureSource.Start();

           
            txtStatus.Text= "Recording...";
        }
        private bool AudioAccessAvailable()
        {
            return CaptureDeviceConfiguration.AllowedDeviceAccess || CaptureDeviceConfiguration.RequestDeviceAccess();
        }
        public static void SavePcmToWav(Stream rawData, Stream output, AudioFormat audioFormat)
        {
            if (audioFormat.WaveFormat != WaveFormatType.Pcm)
                throw new ArgumentException("PCM coding supported!");
         
            BinaryWriter binaryWaveWriter = new BinaryWriter(output);

            binaryWaveWriter.Write("RIFF".ToCharArray());
            binaryWaveWriter.Write((uint)(rawData.Length + 36));
            binaryWaveWriter.Write("WAVE".ToCharArray());
            binaryWaveWriter.Write("fmt ".ToCharArray());
            binaryWaveWriter.Write((uint)0x10);
            binaryWaveWriter.Write((ushort)0x01);
            binaryWaveWriter.Write((ushort)audioFormat.Channels);
            binaryWaveWriter.Write((uint)audioFormat.SamplesPerSecond);
            binaryWaveWriter.Write((uint)(audioFormat.BitsPerSample * audioFormat.SamplesPerSecond * audioFormat.Channels / 8));
            binaryWaveWriter.Write((ushort)(audioFormat.BitsPerSample * audioFormat.Channels / 8));
            binaryWaveWriter.Write((ushort)audioFormat.BitsPerSample);
            binaryWaveWriter.Write("data".ToCharArray());
            binaryWaveWriter.Write((uint)rawData.Length);
            long originalRawDataStreamPosition = rawData.Position;
            rawData.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[4096];
            int data;
            while ((data = rawData.Read(buffer, 0, 4096)) > 0)
            {
                binaryWaveWriter.Write(buffer, 0, data);
            }
            rawData.Seek(originalRawDataStreamPosition, SeekOrigin.Begin);
        }

        private void btnStop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (audioCaptureSource.State == CaptureState.Started)
            {
                audioCaptureSource.Stop();
            
                timer.Stop();
                txtStatus.Text = "Recorded, Save the record.";
            }
        }

        private void btnSave_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (audioSaveFileDialog.ShowDialog() == false) return;
            
            txtStatus.Text = "Saving...";
            Stream audioStream = audioSaveFileDialog.OpenFile();
            SavePcmToWav(audioSink.AudioData, audioStream, audioSink.AudioFormat);
            audioStream.Close();
            MessageBox.Show("Your record is saved.");
            txtStatus.Text = "Saved, Try opening the file in Windows Media Player";
        }
    }
}
