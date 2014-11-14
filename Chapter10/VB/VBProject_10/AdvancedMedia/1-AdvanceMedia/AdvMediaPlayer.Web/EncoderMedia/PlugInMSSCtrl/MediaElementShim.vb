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

' Namespace Microsoft.Expression.Encoder.PlugInMssCtrl

Public Interface DFXPDataReceiver

    Sub AddDFXPData(timeStamp As TimeSpan, dfxpData As Stream)
End Interface

Public Interface MediaElementShim
    '
    '  MediaElement items
    '
    Property AutoPlay As Boolean
    Property BufferingProgress As Double
    Property CanSeek As Boolean
    Property CurrentState As MediaElementState              ' ReadOnly
    Property DownloadProgressOffset As Double               ' ReadOnly
    Property Markers As TimelineMarkerCollection            ' ReadOnly
    Property NaturalDuration As Duration                    ' ReadOnly
    Property Position As TimeSpan
    Property Source As Uri
    Property Stretch As Stretch
    Property Volume As Double

    Event CurrentStateChanged As RoutedEventHandler
    Event DownloadProgressChanged As RoutedEventHandler
    Event MarkerReached As TimelineMarkerRoutedEventHandler
    Event MediaEnded As RoutedEventHandler
    Property DownloadProgress As Double                     ' ReadOnly
    Event MediaFailed As EventHandler(Of ExceptionRoutedEventArgs)
    Event MediaOpened As RoutedEventHandler

    Sub Pause()
    Sub Play()
    Sub SetSource(mediaStreamSource As MediaStreamSource)
    Sub SetSource(stream As Stream)
    Sub [Stop]()
    '
    '  FrameworkElement stuff
    '
    Property Height As Double
    Property Width As Double
    Property Visibility As Visibility
    '
    '  UIElement stuff
    '
    Property CacheMode As CacheMode
    Property UIElement As UIElement                         ' ReadOnly
    Event MouseLeftButtonDown As MouseButtonEventHandler
    '
    '  Offline support
    '
    Property OfflineSupport As IPlugInMssOfflineSupport  ' ReadOnly
    '
    '  ISMT / DFXP Support
    '
    Property DFXPDataReceiver As DFXPDataReceiver ' ReadOnly

    Sub ActivateTextStreamForLanguage(isoThreeLetterLanguageCode As String)
End Interface
' End Namespace   '   Microsoft.Expression.Encoder.PlugInMssCtrl
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\PlugInMSSCtrl\MediaElementShim.cs
