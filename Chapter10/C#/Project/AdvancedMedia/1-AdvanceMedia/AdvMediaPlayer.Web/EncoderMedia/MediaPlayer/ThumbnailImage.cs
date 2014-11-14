// <copyright file="ThumbnailImage.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>Implements the ThumbnailImage class</summary>
// <author>Microsoft Expression Encoder Team</author>
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ExpressionMediaPlayer
{
    /// <summary>
    ///  Similar function as the Image control -- but handles UriFormatException cases by leaving image blank instead of thowing the exception higher.
    /// </summary>
    public class ThumbnailImage : ContentControl
    {
        /// <summary>
        /// Using a DependencyProperty as the backing store for SourceUrl.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty SourceProperty = 
            DependencyProperty.Register("Source", typeof(string), typeof(ThumbnailImage), new PropertyMetadata(new PropertyChangedCallback(SourcePropChanged)));
        /// <summary>
        /// Initializes a new instance of the ThumbnailImage class.
        /// </summary>
        public ThumbnailImage()
        {
        }
        /// <summary>
        /// Gets or sets the source of this thumbnail.
        /// </summary>
        public String Source
        {
            get { return (String)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        /// <summary>
        /// Load image
        /// </summary>
        public void LoadImage()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.Source))
                {
                    Image image = new Image();
                    string sourceEscaped = Uri.EscapeUriString(this.Source);
                    Uri thumbnailUri = new Uri(sourceEscaped, UriKind.RelativeOrAbsolute);

                    Debug.WriteLine("ThumbnailImage:LoadImage:" + thumbnailUri.ToString());

                    if (MediaPlayer.IsOffline)
                    {
                        IsoUri isoThumbnailUri = MediaPlayer.MakeOfflineIsoUri(thumbnailUri);
                        if (isoThumbnailUri.IsoFileExists)
                        {
                            BitmapImage bi = new BitmapImage();
                            bi.SetSource(isoThumbnailUri.Stream);
                            image.Source = bi;
                        }
                    }
                    else
                    {
                        BitmapImage bi = new BitmapImage(thumbnailUri);
                        image.Source = bi;
                    }
                    image.ImageFailed += this.OnImageFailed;
                    this.Content = image;
                }
                else
                {
                    Rectangle emptyRect = new Rectangle();
                    emptyRect.Width = 32;
                    emptyRect.Height = 24;
                    emptyRect.Fill = new SolidColorBrush(Color.FromArgb(0x00, 0x00, 0x00, 0x00));
                    this.Content = emptyRect;
                }
            }
            catch(UriFormatException)
            {
                // leave thumbnail blank if image can't be loaded
                Rectangle emptyRect = new Rectangle();
                this.Content = emptyRect;                
            }
        }
        /// <summary>
        /// Property handler for the source dependency property.
        /// </summary>
        /// <param name="imageObject">Source dependency object.</param>
        /// <param name="eventArgs">Event args.</param>
        protected static void SourcePropChanged(DependencyObject imageObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            ThumbnailImage thumbnailImage = imageObject as ThumbnailImage;
            if (thumbnailImage != null && eventArgs.NewValue != null && eventArgs.NewValue is string)
            {
                ThumbnailDownloader.Enqueue(thumbnailImage);
            }
        }
        /// <summary>
        /// Event handler for the Image failed event from the image.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            string strErrorMessage = string.Empty;
            if (this.Source != null)
            {
                strErrorMessage += this.Source.ToString() + "\r\n";
            }
            strErrorMessage += e.ErrorException.ToString() + "\r\n";
            Debug.WriteLine("ThumbnailImage:OnImageFailed:" + strErrorMessage);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = strErrorMessage;
            textBlock.TextWrapping = TextWrapping.Wrap;

            Content = textBlock;

        }
    }
}
