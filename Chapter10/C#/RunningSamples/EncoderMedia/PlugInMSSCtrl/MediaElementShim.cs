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

namespace Microsoft.Expression.Encoder.PlugInMssCtrl
{
    public interface DFXPDataReceiver 
    {
        void AddDFXPData(TimeSpan timeStamp, Stream dfxpData);
    }

    public interface MediaElementShim
    {
        // MediaElement items
        bool AutoPlay { get; set; }
        double BufferingProgress { get; }
        bool CanSeek { get; }
        MediaElementState CurrentState { get; }
        double DownloadProgress { get; }
        double DownloadProgressOffset { get; }
        TimelineMarkerCollection Markers { get; }
        Duration NaturalDuration { get; }
        TimeSpan Position { get; set; }
        Uri Source { get; set; }
        Stretch Stretch { get; set; }
        double Volume { get; set; }

        event RoutedEventHandler CurrentStateChanged;
        event RoutedEventHandler DownloadProgressChanged;
        event TimelineMarkerRoutedEventHandler MarkerReached;
        event RoutedEventHandler MediaEnded;
        event EventHandler<ExceptionRoutedEventArgs> MediaFailed;
        event RoutedEventHandler MediaOpened;

        void Pause();
        void Play();

        void SetSource(MediaStreamSource mediaStreamSource);
        void SetSource(Stream stream);

        void Stop();

        // FrameworkElement stuff
        double Height { get; set; }
        double Width { get; set; }
        Visibility Visibility { get; set; }

        // UIElement stuff
        CacheMode CacheMode { get; set; }
        event MouseButtonEventHandler MouseLeftButtonDown;

        UIElement UIElement { get; }

        // Offline support
        IPlugInMssOfflineSupport OfflineSupport { get; }

        // ISMT / DFXP Support
        DFXPDataReceiver DFXPDataReceiver { get; set; }
        void ActivateTextStreamForLanguage(string isoThreeLetterLanguageCode);
    }
}
