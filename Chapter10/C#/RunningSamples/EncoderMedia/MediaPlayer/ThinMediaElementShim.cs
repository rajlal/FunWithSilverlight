using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Expression.Encoder.PlugInMssCtrl;

namespace ExpressionMediaPlayer
{
    internal class MediaElementShimThin : MediaElementShim
    {
        private MediaElement actualMediaElement;
        internal MediaElementShimThin(MediaElement actualMediaElement)
        {
            this.actualMediaElement = actualMediaElement; 
        }

        // MediaElement items
        public bool AutoPlay { get { return this.actualMediaElement.AutoPlay; } set { this.actualMediaElement.AutoPlay = value; } }
        public double BufferingProgress { get { return this.actualMediaElement.BufferingProgress; } }
        public bool CanSeek { get { return this.actualMediaElement.CanSeek; } }
        public MediaElementState CurrentState { get { return this.actualMediaElement.CurrentState; } }
        public double DownloadProgress { get { return this.actualMediaElement.DownloadProgress; } }
        public double DownloadProgressOffset { get { return this.actualMediaElement.DownloadProgressOffset; } }
        public TimelineMarkerCollection Markers { get { return this.actualMediaElement.Markers; } }
        public Duration NaturalDuration { get { return this.actualMediaElement.NaturalDuration; } }
        public TimeSpan Position { get { return this.actualMediaElement.Position; } set { this.actualMediaElement.Position = value; } }
        public Uri Source { get { return this.actualMediaElement.Source; } set { this.actualMediaElement.Source = value; } }
        public Stretch Stretch { get { return this.actualMediaElement.Stretch; } set { this.actualMediaElement.Stretch = value; } }
        public double Volume { get { return this.actualMediaElement.Volume; } set { this.actualMediaElement.Volume = value; } }

        public event RoutedEventHandler CurrentStateChanged { add { this.actualMediaElement.CurrentStateChanged += value; } remove { this.actualMediaElement.CurrentStateChanged -= value; } }
        public event RoutedEventHandler DownloadProgressChanged { add { this.actualMediaElement.DownloadProgressChanged += value; } remove { this.actualMediaElement.DownloadProgressChanged -= value; } }
        public event TimelineMarkerRoutedEventHandler MarkerReached { add { this.actualMediaElement.MarkerReached += value; } remove { this.actualMediaElement.MarkerReached -= value; } }
        public event RoutedEventHandler MediaEnded { add { this.actualMediaElement.MediaEnded += value; } remove { this.actualMediaElement.MediaEnded -= value; } }
        public event EventHandler<ExceptionRoutedEventArgs> MediaFailed { add { this.actualMediaElement.MediaFailed += value; } remove { this.actualMediaElement.MediaFailed -= value; } }
        public event RoutedEventHandler MediaOpened { add { this.actualMediaElement.MediaOpened += value; } remove { this.actualMediaElement.MediaOpened -= value; } }

        public void Pause() { this.actualMediaElement.Pause(); }
        public void Play() { this.actualMediaElement.Play(); }

        public void SetSource(MediaStreamSource mediaStreamSource) { this.actualMediaElement.SetSource(mediaStreamSource); }
        public void SetSource(Stream stream) { this.actualMediaElement.SetSource(stream); }

        public void Stop() { this.actualMediaElement.Stop(); }

        // FrameworkElement stuff
        public double Height { get { return this.actualMediaElement.Height; } set { this.actualMediaElement.Height = value; } }
        public double Width { get { return this.actualMediaElement.Width; } set { this.actualMediaElement.Width = value; } }
        public Visibility Visibility { get { return this.actualMediaElement.Visibility; }  set { this.actualMediaElement.Visibility = value; } }

        // UIElement stuff
        public CacheMode CacheMode { get { return this.actualMediaElement.CacheMode; } set { this.actualMediaElement.CacheMode = value; } }
        public event MouseButtonEventHandler MouseLeftButtonDown { add { this.actualMediaElement.MouseLeftButtonDown += value; } remove { this.actualMediaElement.MouseLeftButtonDown -= value; } }

        public UIElement UIElement { get { return this.actualMediaElement as UIElement; } }
        /// <summary>
        /// IPlugInMssOfflineSupport interface
        /// </summary>
        // Offline support }
        public IPlugInMssOfflineSupport OfflineSupport { get { throw new PlatformNotSupportedException(); } }

        // ISMT / DFXP Support
        public DFXPDataReceiver DFXPDataReceiver { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        public void ActivateTextStreamForLanguage(string isoThreeLetterLanguageCode) { throw new NotImplementedException(); }
    }
}
