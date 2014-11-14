'  <copyright file="CaptionsManager.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the CaptionsManager class</summary>
'  <author>Microsoft Expression Encoder Team</author>

Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.Net
Imports System.Threading
Imports System.Windows
Imports System.Windows.Browser
Imports System.Windows.Controls
Imports System.Windows.Media
Imports System.Windows.Data
Imports TimedTextInterface
Imports Microsoft.Expression.Encoder.PlugInMssCtrl

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' represent a caption option, ie. "spanish subtitles"
    ''' </summary>
    Public Class CaptionOption
        Public Property Title As String
        Public Property LanguageIdTwoLetterIso As String
        Public Property oType As String
    End Class   '   CaptionOption

    internal Class CaptionsManager
        Inherits DFXPDataReceiver
        ''' <summary>
        ''' player that owns this caption manager
        ''' </summary>
        private m_mediaPlayer As ExpressionMediaPlayer.MediaPlayer 
        ''' <summary>
        ''' The area to display the closed captions in.
        ''' </summary>
        private m_captionsArea As Panel 
        ''' <summary>
        ''' where to contain captions
        ''' </summary>
        private m_canvasCaptions As Canvas 
        /// <summary>
        /// cache of captions
        /// </summary>
        private m_timedText As ITimedTextModel  = null
        /// <summary>
        /// available caption options (type/language)
        /// </summary>
        private captionOptionsByPlaylistItem As Dictionary(Of Integer, Dictionary(Of string, CaptionOption)) = new Dictionary(Of Integer, Dictionary(Of string, CaptionOption))()


        internal Property CurrentISOTwoLetterLanguageName As string 
        internal Property CurrentPlaylistItemIndex As Integer


#if DEBUG_EXTRA_CAPTIONS_PERF
        End Property 

#endif

        ''' <summary>
        ''' download the captions data
        ''' </summary>
        ''' <param name="isAdaptive">Whether the content is smooth streaming or conventional</param>
        ''' <param name="captionSource">this captions source data</param>
        internal Sub DownloadCaptions(isAdaptive As Boolean, captionSource As CaptionSource)

            '  Remove old caption data if present

            If (Me.m_timedText IsNot Nothing) Then
                Me.m_timedText.ClearCaptionArea()
                Me.m_timedText.ClearEventData()
            End If


            '  If caption data passed in request it

            If (captionSource  IsNot Nothing) Then

                If (captionSource.CaptionFileSource  IsNot Nothing) Then
#if DEBUG_EXTRA_CAPTIONS_PERF
                    Debug.WriteLine("Downloading captions data file URL:" + captionSource.CaptionFileSource.ToString())
                    dbg_downloadStart = DateTime.Now
#endif
                    Debug.WriteLine("DownloadCaptions: isAdaptive=" + isAdaptive.ToString() + " lang=" + captionSource.ISOTwoLetterLanguageName + " file=" + captionSource.CaptionFileSource.ToString())
                    if (isAdaptive)
                    {
                        if (Me.m_mediaPlayer.CurrentMediaElement.DFXPDataReceiver  <>  this )
                        {
                            Me.m_mediaPlayer.CurrentMediaElement.DFXPDataReceiver = this
                        }

                        Dim isoThree As String  = LanguageAlias.IsoTwoLetterToIsoThreeLetter(captionSource.ISOTwoLetterLanguageName)

                        Me.m_mediaPlayer.CurrentMediaElement.ActivateTextStreamForLanguage(isoThree)
                    }
                    else
                    {
                        if (MediaPlayer.IsOffline)
                        {
                            Debug.WriteLine("DownloadCaptions:  -= 1 Snarfing from IsolatedStorage")
                            IsoUri offlineIsoUri = MediaPlayer.MakeOfflineIsoUri(captionSource.CaptionFileSource)
                            if (offlineIsoUri.IsoFileExists)
                            {
                                Me.m_mediaPlayer.EnableClosedCaptionButton(false)
                                Me.AddDFXPDataFromUIThread(TimeSpan.FromMilliseconds(0), offlineIsoUri.Stream)
                            }
                        }
                        else
                        {
                            Debug.WriteLine("DownloadCaptions:  -= 1 downloading via WebClient")
                            '  While processing a new set of captions -- disable the CC button
                            Me.m_mediaPlayer.EnableClosedCaptionButton(false)

                            Dim webClient As WebClient  = New WebClient()

                            AddHandler webClient.OpenReadCompleted, AddressOf OpenReadCompletedEventHandler(DownloadClosedCaptionsCompleted)
                            AddHandler webClient.DownloadProgressChanged, AddressOf DownloadProgressChangedEventHandler(DownloadProgressChanged)
                            webClient.OpenReadAsync(captionSource.CaptionFileSource)
                        }
                    }
                End If
            End If
        End Sub '   DownloadCaptions
        ''' <summary>
        ''' show downloading text for captions
        ''' </summary>
        Sub DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs)

            m_mediaPlayer.SetCaptionText(String.Format(ExpressionMediaPlayer.Resources.captionsDownloading,e.ProgressPercentage))
        End Sub '   DownloadProgressChanged


        ''' <summary>
        ''' captions have completed download. remember captions text
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="eventArgs"></param>
        private Sub DownloadClosedCaptionsCompleted(sender As Object, eventArgs As OpenReadCompletedEventArgs)

            m_mediaPlayer.SetCaptionText(string.Empty)
            if (eventArgs.Cancelled)
            {
                Debug.WriteLine("canceled download of captions data Not ")
            }
            else if (eventArgs.Error  IsNot Nothing)
            {
                Debug.WriteLine("Error occured downloading captions data error=" + eventArgs.Error.Message)
                m_mediaPlayer.ShowErrorMessage(eventArgs.Error.Message)
                m_mediaPlayer.EnableClosedCaptionButton(true)
            }
            else
            {
                if (eventArgs.Result Is Nothing)
                {
                    Debug.WriteLine("Nothing captions file result Not ")
                    m_mediaPlayer.EnableClosedCaptionButton(true)
                ElseIf (eventArgs.Result.Length < 1) Then
                    Debug.WriteLine("Empty captions file result Not ")
                    m_mediaPlayer.EnableClosedCaptionButton(true)
                End If

                else
                {
#if DEBUG_EXTRA_CAPTIONS_PERF

                    Dim diff As TimeSpan  = DateTime.Now - dbg_downloadStart

                    Debug.WriteLine("Downloading captions data file complete: diff(MS)=" + diff.TotalMilliseconds.ToString())
#endif
                    Me.AddDFXPDataFromUIThread(TimeSpan.FromMilliseconds(0), eventArgs.Result)
                }
            }
        End Sub '   DownloadClosedCaptionsCompleted


        ''' <summary>
        ''' Thread for parsing DFXP data
        ''' </summary>
        Dim threadParseTimedText As Thread  = Nothing

        ''' <summary>
        ''' Worker thread method for creating the marker and ad data from the manifest
        ''' </summary>
        private Sub ParseTimedTextWorkerThread(data As Object)


            If (Me.m_timedText  IsNot Nothing) Then
#if DEBUG_EXTRA_CAPTIONS_PERF

            Dim startParse As DateTime  = DateTime.Now

#endif
                Dim dfxpData As Stream  = CType(data, Stream)

                AddDFXPData(TimeSpan.FromMilliseconds(0), dfxpData)
                Me.m_mediaPlayer.EnableClosedCaptionButton(true)
                return
            End If

            Debug.Assert(false, "ParseTimedTextWorkerThread called with unexpected data")
        End Sub '   ParseTimedTextWorkerThread
        ''' <summary>
        ''' Process DFXP Data while running on a background thread
        ''' </summary>
        ''' <param name="dfxpData"></param>
        public Sub AddDFXPData(timeStamp As TimeSpan, dfxpData As Stream)

            Debug.Assert(dfxpData  IsNot Nothing)

            If (Me.m_timedText Is Nothing) Then
                LoadTimedTextSupport(timeStamp, dfxpData)
            else
                var eventData = Me.m_timedText.ParseData(timeStamp, dfxpData)
#if DEBUG_EXTRA_CAPTIONS_PERF

            Dim parseDiff As TimeSpan  = DateTime.Now - startParse

            Debug.WriteLine("ParseTime: diff(MS)=" + parseDiff.TotalMilliseconds.ToString())
#endif

                If ( eventData  IsNot Nothing ) Then
                    If ( Not string.IsNullOrEmpty(eventData.ErrorInfo)) Then
                        Me.m_mediaPlayer.SetNextTickErrorMessage(eventData.ErrorInfo)
                    else
                        Me.m_mediaPlayer.Dispatcher.BeginInvoke(() => Me.m_timedText.AttachEvents(eventData))
                    End If
                End If

                Debug.WriteLine("ParseTimedTextWorkerThread() completed Not ")
            End If
        End Sub '   AddDFXPData

        ''' <summary>
        ''' DFXP source file (as text string) for captions.
        ''' </summary>
        private Sub AddDFXPDataFromUIThread(timeStamp As TimeSpan, dfxpData As Stream)


            If (Me.m_timedText  IsNot Nothing) Then
                if (Me.threadParseTimedText  IsNot Nothing  AndAlso  Me.threadParseTimedText.IsAlive)
                {
                    Debug.Assert(false, "Attempting to start New parse thread while prior one is still running")
                    return
                }

                Thread worker = New Thread(ParseTimedTextWorkerThread)
                worker.Start(dfxpData)
                Me.threadParseTimedText = worker
            else
                LoadTimedTextSupport(timeStamp, dfxpData)
            End If
        End Sub '   AddDFXPDataFromUIThread

        ''' <summary>
        ''' Clear out cache of DFXP captions -- needed when switching playlist items or switching languages
        ''' </summary>
        internal Sub ClearDFXPEvents()


            If (Me.m_timedText  IsNot Nothing) Then
                Me.m_timedText.ClearMarkers()
            End If
        End Sub '   ClearDFXPEvents

        ''' <summary>
        ''' Helper routine for the media player to call after a window resize to recreate the captions using the current window size.
        ''' </summary>
        internal Sub RefreshCaptions()


            If (Me.m_timedText <> Nothing) Then
                Me.m_timedText.RefreshCaptionArea()
            End If
        End Sub '   RefreshCaptions

        ''' <summary>
        ''' Helper routine to clear the DFXP caption area
        ''' </summary>
        internal Sub ClearCaptions()


            If ((Me.m_timedText Is Nothing)  OrElse  (Me.m_canvasCaptions Is Nothing)) Then
                return
            End If

            Me.m_timedText.ClearCaptionArea()
        End Sub '   ClearCaptions


        internal const string TimedTextXAPName = "TimedTextLibrary.XAP"
        const string TimedTextAssemblyName = "TimedTextLibrary.DLL"
        const string TimedTextObjectName = "TimedTextLibrary.TimedTextModel"
        End Property
        End Property
        PlugInLoader m_timedTextPlugin
        ''' <summary>
        ''' Loads the SmoothStreaming support module
        ''' </summary>
        private Sub LoadTimedTextSupport(timeStamp As TimeSpan, streamToSetAfterLoad As Stream)

            Debug.WriteLine("LoadTimedTextSupport Not ")

            If ( Me.m_timedTextPlugin Is Nothing ) Then
                Debug.Assert((Nothing = Me.m_streamToSetAfterLoad), "Expected defered stream to be Nothing Not ")
                Me.m_timeStampToSetAfterLoad = timeStamp
                Me.m_streamToSetAfterLoad = streamToSetAfterLoad
                Me.m_timedTextPlugin = New PlugInLoader()
                AddHandler Me.m_timedTextPlugin.PlugInLoadCompleted, AddressOf New EventHandler<XAPReadCompletedEventArgs>(OnTimedTextLoaded)

                Dim xap As Uri  = New Uri(TimedTextXAPName, UriKind.Relative)

                Me.m_timedTextPlugin.Load(xap, TimedTextAssemblyName)
            End If
        End Sub '   LoadTimedTextSupport

        ''' <summary>
        ''' Callback that is called once the TimedText support module is downloaded form the host
        ''' </summary>
        private Sub OnTimedTextLoaded(sender As Object, e As XAPReadCompletedEventArgs)


            If (e.Cancelled) Then
                Debug.WriteLine("OnTimedTextLoaded  -= 1 Cancelled")
                return
            ElseIf (e.Error  IsNot Nothing) Then
                Debug.WriteLine("OnTimedTextLoaded  -= 1 Failed:" + e.Error.Message)

                If (e.Error.InnerException  IsNot Nothing) Then
                    Debug.WriteLine("OnTimedTextLoaded -> " + e.Error.InnerException.Message)
                End If

                return
            End If

            Me.m_timedText = (Me.m_timedTextPlugin.CreateObject(TimedTextObjectName)) as ITimedTextModel
            Debug.Assert(Me.m_timedText  IsNot Nothing,"Failed to created TimedText support")

            If (Nothing  <>  Me.m_streamToSetAfterLoad) Then
                Me.AddDFXPDataFromUIThread(Me.m_timeStampToSetAfterLoad, Me.m_streamToSetAfterLoad)
                Me.m_streamToSetAfterLoad = Nothing
            End If
        End Sub '   OnTimedTextLoaded
    End Class   '   CaptionsManager
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\CaptionsManager.cs
