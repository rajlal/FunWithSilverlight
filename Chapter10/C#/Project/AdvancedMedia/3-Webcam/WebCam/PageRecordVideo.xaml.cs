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

using FluxJpeg.Core;
using FluxJpeg.Core.Encoder;
using System.Windows.Media.Imaging;

namespace WebCam
{
    public partial class PageRecordVideo : UserControl
    {
        private CaptureSource videoCaptureSource;
      
       
        private int timerCount;
        private int timerPhotoboothCount;
        public VideoBrush videoBrush;
        public ImageBrush videoCapturedImage0;
        public ImageBrush videoCapturedImage1;
        public ImageBrush videoCapturedImage2;
        public ImageBrush videoCapturedImage3;
        private int photoboothCurrentCounter = 0;

        private SaveFileDialog videoSaveFileDialog = new SaveFileDialog() {DefaultExt = ".jpg", Filter = "JPEG Images (*jpeg *.jpg)|*.jpeg;*.jpg",};

        DispatcherTimer timer = new DispatcherTimer();
        DispatcherTimer timerPhotobooth = new DispatcherTimer();
        public PageRecordVideo()
        {
            InitializeComponent();
            videoCaptureSource = new CaptureSource() {};
            videoCaptureSource.AudioCaptureDevice = CaptureDeviceConfiguration.GetDefaultAudioCaptureDevice();
            videoCaptureSource.VideoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();

            // async capture failed event handler
            videoCaptureSource.CaptureFailed +=
                new EventHandler<ExceptionRoutedEventArgs>(videoCaptureSource_CaptureFailed);

            // async capture completed event handler
            videoCaptureSource.CaptureImageCompleted +=
                new EventHandler<CaptureImageCompletedEventArgs>(videoCaptureSource_CaptureImageCompleted);

            // set the source on the VideoBrush used to display the video
            videoBrush = new VideoBrush();
            videoBrush.SetSource(videoCaptureSource);

            // set the Fill property of the Rectangle (defined in XAML) to the VideoBrush
            webcamVideoDisplay.Fill = videoBrush;
           

            // the brush used to fill the display rectangle
            videoCapturedImage0 = new ImageBrush();
            videoCapturedImage1 = new ImageBrush();
            videoCapturedImage2 = new ImageBrush();
            videoCapturedImage3 = new ImageBrush();

            // set the Fill property of the Rectangle (defined in XAML) to the ImageBrush
            webcamVideoDisplay0.Fill = videoCapturedImage0;
            webcamVideoDisplay1.Fill = videoCapturedImage1;
            webcamVideoDisplay2.Fill = videoCapturedImage2;
            webcamVideoDisplay3.Fill = videoCapturedImage3;

            ImageBrush img = new ImageBrush();
            img.ImageSource = (ImageSource)new ImageSourceConverter().ConvertFromString("Image/bg.png");
            gridPhotoBooth.Background = img;
     }

        private void btnRecord_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RecordVideo();
        }
        private void RecordVideo()
        {
            timerCount=0;
            int TimerCountMinutes=0;
            int TimerCountSeconds = 0;
            
            if (!VideoAccessAvailable())
                return;

            if (videoCaptureSource.VideoCaptureDevice == null)
                return;
            
            if (videoCaptureSource.State != CaptureState.Stopped)
                return;

           
           
            if (timer.IsEnabled) timer.Stop(); else timer.Start();

            timer.Tick +=
                           delegate(object s, EventArgs args)
                           {
                               timerCount++;
                               TimerCountMinutes= timerCount  / 60;
                               TimerCountSeconds = timerCount % 60;
                               StatusPosition.Text = string.Format("{0:D2}", TimerCountMinutes) + ":" + string.Format("{0:D2}", TimerCountSeconds);
                           };
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000); // 1 second
            timer.Start();

            videoCaptureSource.Start();

          
            txtStatus.Text= "Capture Started.";
        }
        private bool VideoAccessAvailable()
        {
            return CaptureDeviceConfiguration.AllowedDeviceAccess || CaptureDeviceConfiguration.RequestDeviceAccess();
        }
        private void btnStop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (videoCaptureSource.State == CaptureState.Started)
            {
                videoCaptureSource.Stop();
               
                timer.Stop();
                txtStatus.Text = "Capture Stopped.";
            }
        }

        private void btnSave_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (videoSaveFileDialog.ShowDialog() == false) return;

            txtStatus.Text = "Saving...";
            using (Stream dstStream = videoSaveFileDialog.OpenFile())
            {
                SaveSnapshot(dstStream);
            }

            //MessageBox.Show("Your record is saved.");
            txtStatus.Text = "Saved, Open in Image Viewer";
        }
        void videoCaptureSource_CaptureImageCompleted(object sender, CaptureImageCompletedEventArgs e)
        {
            if (photoboothCurrentCounter == 0)
            {
                videoCapturedImage0.ImageSource = e.Result;
                photoboothCurrentCounter++;
            }
            else if (photoboothCurrentCounter == 1)
            {
                videoCapturedImage1.ImageSource = e.Result;
                photoboothCurrentCounter++;
            }
            else if (photoboothCurrentCounter == 2)
            {
                videoCapturedImage2.ImageSource = e.Result;
                photoboothCurrentCounter++;
            }
            else if (photoboothCurrentCounter == 3)
            {
                videoCapturedImage3.ImageSource = e.Result;
                photoboothCurrentCounter  = 0;
            }
        }

        void videoCaptureSource_CaptureFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show(string.Format("Failed to capture image: {0}", e.ErrorException.Message));
        }
        private void SaveSnapshot(Stream dstStream)
        {
            try
            {
                // Render Rectangle manually into WriteableBitmap and don't use CaptureSource.AsyncCaptureImage:
                // 1.: It doesn't work in all cases.
                // 2.: It only works if captureSource was started.
                // 3.: It only captures the raw camera stream and ignores the applied effect (shader).
                WriteableBitmap bmp = new WriteableBitmap(webcamVideoDisplay , null);
                EncodeJpeg(bmp, dstStream);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error saving snapshot", MessageBoxButton.OK);
            }
        }
        private void SaveSnapshotPhotoBooth(Stream dstStream)
        {
            try
            {
                // Render Rectangle manually into WriteableBitmap and don't use CaptureSource.AsyncCaptureImage:
                // 1.: It doesn't work in all cases.
                // 2.: It only works if captureSource was started.
                // 3.: It only captures the raw camera stream and ignores the applied effect (shader).
                WriteableBitmap bmp = new WriteableBitmap(gridPhotoBooth, null);
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

        private void btnPhotoBooth_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (videoCaptureSource.State != CaptureState.Started)
                RecordVideo();

            txtStatus.Text = "Saving..."; 
            if (videoSaveFileDialog.ShowDialog() == false) return;
            timerPhotoboothCount = 0;
            if (timerPhotobooth.IsEnabled) timerPhotobooth.Stop(); else timerPhotobooth.Start();

            timerPhotobooth.Tick +=
                           delegate(object s, EventArgs args)
                           {
                              videoCaptureSource.CaptureImageAsync();
                               int i = timerPhotoboothCount +1;
                              txtStatus.Text = "Saving photo..." +i ;
                                   
                               if (timerPhotoboothCount == 4)
                               {
                                   timerPhotobooth.Stop();
                                   txtStatus.Text = "Saving...";
                                    using (Stream dstStream = videoSaveFileDialog.OpenFile())
                                    {
                                        SaveSnapshotPhotoBooth(dstStream);
                                    }
                                     txtStatus.Text = "Saved, Open in Image Viewer";
                               }
                               timerPhotoboothCount++;
                              
                           };
            timerPhotobooth.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timerPhotobooth.Start();

            
           
        }

    }
}
