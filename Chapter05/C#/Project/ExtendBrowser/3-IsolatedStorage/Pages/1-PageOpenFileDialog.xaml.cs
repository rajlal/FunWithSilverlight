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
using System.Windows.Browser;
using System.Windows.Media.Imaging;

namespace LocalStorage
{
    public partial class PageOpenFileDialog : UserControl
    {
        public PageOpenFileDialog()
        {
            InitializeComponent();
      
        }

        private void OpenFileDialog()
        {
            lblFilename.Text = "";
            videoContainer.Stop();
            videoContainer.Visibility = Visibility.Collapsed;
            textContainer.Visibility = Visibility.Collapsed;
            imageContainer.Visibility = Visibility.Collapsed;
            OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Filter = "All files (*.*)|*.*|Text files (txt)|*.txt|XAML files (xaml)|*.xaml|Image files (jpg, png)|*.jpg;*.png|Windows Media Video (wmv)|*.wmv";
                dialog.FilterIndex = 1;
                if (dialog.ShowDialog().Value)
                {
                    FileInfo info = dialog.File;
                    this.lblFilename.Text = "Filename: " + info.Name.ToString();
                    try
                    {
                        switch (dialog.File.Extension.ToLower() )
                        {
                            case ".txt":
                                SetText(info.OpenText());
                                return;
                            case ".xaml":
                                SetText(info.OpenText());
                                return;
                            case ".jpg":
                                SetImage(info.OpenRead());
                                return;
                            case ".jpeg":
                                SetImage(info.OpenRead());
                                return;
                            case ".png":
                                SetImage(info.OpenRead());
                                return;
                            case ".wmv":
                                SetVideo(info.OpenRead());
                                return;
                            case ".wav":
                                SetVideo(info.OpenRead());
                                return;
                            default:
                                SetText(info.OpenText());
                                return;
                        }
                    }
                    catch
                    {
                        HtmlPage.Window.Alert("Cancelled");
                    }
                }
            
        }
        private void SetText(StreamReader txtS)
        {
            this.textContainer.Text = txtS.ReadToEnd();
            txtS.Close();
            this.textContainer.Visibility = Visibility.Visible;

        }
        private void SetImage(Stream imgS)
        {
            BitmapImage image = new BitmapImage();
            image.SetSource(imgS);
            this.imageContainer.Source = image;
            imgS.Close();
            this.imageContainer.Visibility = Visibility.Visible;

        }
        private void SetVideo(Stream vidS)
        {
            this.videoContainer.SetSource(vidS);
            this.videoContainer.Play();
            this.videoContainer.Visibility = Visibility.Visible;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog();
        }
    }
}
