Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Windows.Controls
Imports Microsoft.Expression.Encoder.PlugInMssCtrl

' Namespace TimedTextInterface

Public Interface ITimedTextEvents

    Property Count As Integer                               'ReadOnly
    Property TimeSpan As TimeSpan                           'ReadOnly
    Property ErrorInfo As String                            'ReadOnly
End Interface

Public Interface ITimedTextModel
    ''' <summary>
    ''' The media element object that for triggering events
    ''' </summary>
    Property MediaElement As MediaElementShim
    ''' <summary>
    ''' The Panel on which to display the DFXP captions
    ''' </summary>
    Property Destination As Panel
    ''' <summary>
    ''' Parse the supplied DFXP data and add event from it between the specified time ranges into the set of events
    ''' </summary>
    ''' <param name="dfxpData"></param>
    ''' <param name="offset"></param>
    ''' <param name="start"></param>
    ''' <param name="end"></param>
    ''' <returns>string.Empty on success, some sort of error message otherwise</returns>
    Function ParseData(timeStamp As TimeSpan, dfxpData As Stream) As ITimedTextEvents
    ''' <summary>
    ''' Remove all event data
    ''' </summary>
    Sub ClearEventData()
    ''' <summary>
    ''' Remove all DFXP Markers
    ''' </summary>
    Sub ClearMarkers()
    ''' <summary>
    ''' Attach markers for current event data -- not this method must be called on the UI thread
    ''' </summary>
    Sub AttachEvents(events As ITimedTextEvents)
    ''' <summary>
    ''' Clear the DFXP caption area (call this after a seek)
    ''' </summary>
    Sub ClearCaptionArea()
    ''' <summary>
    ''' Redraw the DFXP caption area (call this after a window resize event)
    ''' </summary>
    Sub RefreshCaptionArea()
End Interface
' End Namespace   '   TimedTextInterface
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\PlugInMSSCtrl\TimedTextInterface.cs
