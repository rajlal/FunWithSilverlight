Imports System
Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media

<assembly: CLSCompliant(true)>
' Namespace Microsoft.Expression.Encoder.PlugInMssCtrl

''' <summary>
''' Control interface for the Smooth Streaming Media Stream Source
''' </summary>
Public Interface IPlugInMssCore
    ''' <summary>
    ''' The MediaElement used to render the content
    ''' </summary>
    Property MediaElement As MediaElement
    ''' <summary>
    ''' The internet address of the content manifest
    ''' </summary>
    Property ManifestUrl As Uri
    ''' <summary>
    ''' Start Smooth Streaming playerback
    ''' </summary>
    Sub StartPlayback()
    ''' <summary>
    ''' Stop Smooth Streaming playback
    ''' </summary>
    Sub StopPlayback()
End Interface
''' <summary>
''' Interface for supporting offline playback of Smooth Streaming content.
''' </summary>
Public Interface IPlugInMssOfflineSupport
    ''' <summary>
    ''' Parse a Smooth Streaming Manifest from a stream containing XML
    ''' </summary>
    ''' <param name="manifestStream">stream containing the XML</param>
    Sub ParseManifestFromStream(manifestStream As Stream, manifestUrl As Uri)
    ''' <summary>
    ''' Get the offline bitrate reccommened by the heuristics based on the stream type and the  size of the player window
    ''' </summary>
    ''' <param name="streamType">what type of stream video or audio</param>
    ''' <param name="playerSize">The dimensions of the playback window -- the heuristics will not reccommend a bitrate whose dimensions are larger.</param>
    ''' <returns></returns>
    Function RecommendBitrateInKbps(streamType As MediaStreamType, playerSize As Size) As Long
    ''' <summary>
    ''' Return a collection of URLS for all the chunks at the specified bitrate
    ''' </summary>
    ''' <param name="streamType">The desired stream type</param>
    ''' <param name="bitrateInKbps">The desired bitrate -- note this must match one of the bitrates returned by GetBitratesInKbps</param>
    ''' <returns></returns>
    Function GetChunkUris(streamType As MediaStreamType, bitrateInKbps As Long) As ReadOnlyCollection(Of Uri)
    ''' <summary>
    ''' Set the bitrate that is always selected by the MSS when playing in offline mode
    ''' </summary>
    ''' <param name="streamType">The target stream type video or audio</param>
    ''' <param name="offlineBitrateInKbps">The bitrate to play in offline mode</param>
    Sub SetOfflinePlaybackBitrateInKbps(streamType As MediaStreamType, offlineBitrateInKbps As Long)
End Interface
''' <summary>
''' Interface for providing a statistics graph
''' </summary>
Public Interface IPlugInMssStatisticsGraph
    ''' <summary>
    ''' UIElement to be inserted into the visual tree to display a graph of the playback statistics.
    ''' </summary>
    Property StatisticsGraph As UIElement                    ' ReadOnly
End Interface
' End Namespace   '   Microsoft.Expression.Encoder.PlugInMssCtrl
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\PlugInMSSCtrl\PlugInMSSCtrl.cs
