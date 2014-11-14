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
using System.Windows.Media.Imaging;
using FluxJpeg.Core;
using FluxJpeg.Core.Encoder;

namespace WebCam
{
    public partial class PageScreenshotVideo : UserControl
    {
       
        private int timerCount;
        DispatcherTimer timer = new DispatcherTimer();
        private SaveFileDialog screenshotSaveFileDialog = new SaveFileDialog() { DefaultExt = ".jpg", Filter = "JPEG Images (*jpeg *.jpg)|*.jpeg;*.jpg", };



        public PageScreenshotVideo()
        {
            InitializeComponent();
       
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

            myMediaElement.Visibility = Visibility.Visible;
            myMediaElement.Play();
           
            txtStatus.Text= "Playing...";
        }
       

        private void btnStop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
                txtStatus.Text = "Paused.";
                myMediaElement.Pause();
                //myMediaElement.Visibility = Visibility.Collapsed;
                timer.Stop();
           
        }

        private void btnSave_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            myMediaElement.Pause();
            if (screenshotSaveFileDialog.ShowDialog() == false) return;

            txtStatus.Text = "Saving...";
            using (Stream dstStream = screenshotSaveFileDialog.OpenFile())
            {
                SaveSnapshot(dstStream);
            }

            txtStatus.Text = "Saved, Open in Image Viewer";
            myMediaElement.Play();

        }
        private void SaveSnapshot(Stream dstStream)
        {
            try
            {
                 WriteableBitmap screenShot = new WriteableBitmap(myMediaElement, null);
                imageScreenshot.Source = screenShot;
                WriteableBitmap bmp = new WriteableBitmap(imageScreenshot, null);
                EncodeJpeg(bmp, dstStream);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error saving snapshot", MessageBoxButton.OK);
            }
        }
        public static void EncodeJpeg(WriteableBitmap bmp, Stream dstStream)
        {
            // Init buffer in FluxJpeg format
            int w = bmp.PixelWidth;
            int h = bmp.PixelHeight;
            int[] p = bmp.Pixels;
            byte[][,] pixelsForJpeg = new byte[3][,]; // RGB colors
            pixelsForJpeg[0] = new byte[w, h];
            pixelsForJpeg[1] = new byte[w, h];
            pixelsForJpeg[2] = new byte[w, h];

            // Copy WriteableBitmap data into buffer for FluxJpeg
            int i = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int color = p[i++];
                    pixelsForJpeg[0][x, y] = (byte)(color >> 16); // R
                    pixelsForJpeg[1][x, y] = (byte)(color >> 8);  // G
                    pixelsForJpeg[2][x, y] = (byte)(color);       // B
                }
            }

            //Encode Image as JPEG
            var jpegImage = new FluxJpeg.Core.Image(new ColorModel { colorspace = ColorSpace.RGB }, pixelsForJpeg);
            var encoder = new JpegEncoder(jpegImage, 95, dstStream);
            encoder.Encode();
        }

        public static WriteableBitmap DecodeJpeg(Stream srcStream)
        {
            // Decode JPEG
            var decoder = new FluxJpeg.Core.Decoder.JpegDecoder(srcStream);
            var jpegDecoded = decoder.Decode();
            var img = jpegDecoded.Image;
            img.ChangeColorSpace(ColorSpace.RGB);

            // Init Buffer
            int w = img.Width;
            int h = img.Height;
            var result = new WriteableBitmap(w, h);
            int[] p = result.Pixels;
            byte[][,] pixelsFromJpeg = img.Raster;

            // Copy FluxJpeg buffer into WriteableBitmap
            int i = 0;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    p[i++] = (0xFF << 24) // A
                                | (pixelsFromJpeg[0][x, y] << 16) // R
                                | (pixelsFromJpeg[1][x, y] << 8)  // G
                                | pixelsFromJpeg[2][x, y];       // B
                }
            }

            return result;
        }
    }
}
