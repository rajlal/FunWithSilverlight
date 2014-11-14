Imports System
Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes
Imports Microsoft.Expression.Encoder.PlugInMssCtrl

' Namespace ExpressionMediaPlayer

    internal Class MediaElementShimThin
        Inherits MediaElementShim
        Private actualMediaElement As MediaElement
        internal MediaElementShimThin(MediaElement actualMediaElement)
        {
            Me.actualMediaElement = actualMediaElement
        }

        '  MediaElement items
        End Property
        End Property
        End Property
        End Property
        End Property
        End Property
        End Property
        End Property
        End Property
        End Property
        End Property
        End Property

        public event RoutedEventHandler CurrentStateChanged { add { Me.actualMediaElement.CurrentStateChanged += value; } remove { Me.actualMediaElement.CurrentStateChanged -= value; } }
        public event RoutedEventHandler DownloadProgressChanged { add { Me.actualMediaElement.DownloadProgressChanged += value; } remove { Me.actualMediaElement.DownloadProgressChanged -= value; } }
        public event TimelineMarkerRoutedEventHandler MarkerReached { add { Me.actualMediaElement.MarkerReached += value; } remove { Me.actualMediaElement.MarkerReached -= value; } }
        public event RoutedEventHandler MediaEnded { add { Me.actualMediaElement.MediaEnded += value; } remove { Me.actualMediaElement.MediaEnded -= value; } }
        public event EventHandler<ExceptionRoutedEventArgs> MediaFailed { add { Me.actualMediaElement.MediaFailed += value; } remove { Me.actualMediaElement.MediaFailed -= value; } }
        public event RoutedEventHandler MediaOpened { add { Me.actualMediaElement.MediaOpened += value; } remove { Me.actualMediaElement.MediaOpened -= value; } }

        public Sub Pause()

            Me.actualMediaElement.Pause()
        End Sub '   Pause

        public Sub Play()

            Me.actualMediaElement.Play()
        End Sub '   Play


        public Sub SetSource(oMediaStreamSource As MediaStreamSource)

            Me.actualMediaElement.SetSource(oMediaStreamSource)
        End Sub '   SetSource

        public Sub SetSource(oStream As Stream)

            Me.actualMediaElement.SetSource(oStream)
        End Sub '   SetSource


        public Sub Stop()

            Me.actualMediaElement.Stop()
        End Sub '   Stop
        '  FrameworkElement stuff
        End Property
        End Property
        End Property


        '  UIElement stuff
        End Property

        public event MouseButtonEventHandler MouseLeftButtonDown { add { Me.actualMediaElement.MouseLeftButtonDown += value; } remove { Me.actualMediaElement.MouseLeftButtonDown -= value; } }
        End Property
        ''' <summary>
        ''' IPlugInMssOfflineSupport interface
        ''' </summary>
        '  Offline support }
        End Property


        '  ISMT / DFXP Support
        End Property

        public Sub ActivateTextStreamForLanguage(isoThreeLetterLanguageCode As String)

            throw New NotImplementedException()
        End Sub '   ActivateTextStreamForLanguage
    End Class   '   MediaElementShimThin
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\ThinMediaElementShim.cs
