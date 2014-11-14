'  <copyright file="PlaylistCollection.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the PlaylistCollection class</summary>
'  <author>Microsoft Expression Encoder Team</author>

Imports Microsoft.Expression.Encoder.PlugInMssCtrl
Imports System
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Text
Imports System.Threading
Imports System.Windows
Imports System.Windows.Browser
Imports System.Windows.Controls
Imports System.Windows.Media
Imports System.Windows.Threading
Imports System.Xml
Imports Microsoft.VisualBasic

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' exception for errors when taking the playlist offline
    ''' </summary>
    Public Class PlaylistTakeOfflineException
        Inherits Exception
        Public Sub New()
        End Sub '   New


        Public Sub New(message As String)
        End Sub '   New


        Public Sub New(message As String, exp As Exception)
        End Sub '   New


        Public Sub New(nodeFound As String, nodeExpected As String)
        End Sub '   New
    End Class   '   PlaylistTakeOfflineException

    ''' <summary>
    ''' event args for downloading playlist events
    ''' </summary>
    Public Class PlaylistDownloadProgressEventArgs
        Inherits EventArgs
    {
        Public Sub New(dblProgress As Double)
            Progress = dblProgress
        End Sub '   New


        Public ReadOnly Property Progress() As Double

        internal QueueItem(QueueItemTask oTask, PlaylistItem playlistItemParameter)
        {
            Me.oTask = oTask
            Me.playlistItem = playlistItemParameter
            Me.SetUri(playlistItemParameter.MediaSource)
        }

        internal QueueItem(ReadOnlyCollection<Uri> chunksToQueue)
        {
            Me.oTask = QueueItemTask.queueChunkList
            Me.SetUri(new Uri(@"about:blank"))
            Me.chunksToQueue = chunksToQueue
        }

        internal Sub SetUri(oUri As Uri)

            Dim absoluteUri As Uri  = oUri


            If ( Not absoluteUri.IsAbsoluteUri  AndAlso  HtmlPage.Document.DocumentUri <> Nothing ) Then
                absoluteUri = New Uri(HtmlPage.Document.DocumentUri, oUri)
            End If

            Me.uriToDownload = New IsoUri(absoluteUri)
        End Sub '   SetUri


        public Sub Dispose()

            Me.uriToDownload.Dispose()
            GC.SuppressFinalize(this)
        End Sub '   Dispose


        internal int RetryCount { get { return Me.retryCount; } }

        internal int BumpRetryCount()
        {
            Me.retryCount += 1
#if DEBUG
            MessageTracer.TraceMessage("Retry:" + Me.retryCount.ToString() + " task: " + Me.oTask.ToString() + " url=" + Me.uriToDownload.ToString())
#endif
            return Me.retryCount
        }

        internal bool DoTask(Playlist playlist)
        {
#if DEBUG
            MessageTracer.TraceMessage("DoTask: retry=" + Me.retryCount.ToString() + " now=" + DateTime.Now.ToString() + " task: " + Me.oTask.ToString() + " url=" + Me.uriToDownload.ToString())
#endif

            Select Case (Me.oTask)

                case QueueItemTask.downloadFile:
                    AddHandler Me.uriToDownload.UriDownloadProgressChanged, AddressOf New EventHandler<UriDownloadProgressRoutedEventArgs>(playlist.PlaylistCollection_UriDownloadProgressChanged)
                    Me.uriToDownload.Download()
                    return Me.uriToDownload.DownloadSucceeded
                case QueueItemTask.processManifest:
                    return playlist.ProcessDownloadedOfflineManifest(Me.playlistItem, Me.uriToDownload)
                case QueueItemTask.queueChunkList:
                    return playlist.EnqueueChunkList(Me.chunksToQueue)
            End Select '    Me.oTask

            throw New PlaylistTakeOfflineException("Develoer Error Not : New task member added to QueueItemTask enum but not added the DoTask() Switch statement Not ")
        }
    End Class   '   QueueItem


#if DEBUG

    Dim Class MessageTracer

    {
        private MediaPlayer m_player

        internal Sub SetPlayer(player As MediaPlayer)

            m_player = player
        End Sub '   SetPlayer


        internal Sub TraceMessage(message As String)

            Debug.WriteLine(message)

            If (m_player  IsNot Nothing) Then
                m_player.SetNextTickErrorMessage(message)
            End If
        End Sub '   TraceMessage
    End Class   '   MessageTracer

#endif

    Public Partial Class Playlist
        Inherits DependencyObject
    {
        Private m_progressDialog As OfflineDownloadProgressDialog
        Private m_player As MediaPlayer
        private Size playerSize
        Private m_baseUriForTakingOffline As Uri

        ''' <summary>
        ''' event for download progress of playlist items.
        ''' </summary>
        public event EventHandler<PlaylistDownloadProgressEventArgs> PlaylistDownloadProgressChanged

        ''' <summary>
        ''' is playlist currently downloading?
        ''' </summary>
        internal bool IsDownloading
        {
            get
            private set
        }

        ''' <summary>
        ''' items to download if taking this playlist offline
        ''' </summary>
        private Queue<QueueItem> m_downloadItemsQueue = Nothing

        ''' <summary>
        ''' Add a work item to the Queue
        ''' </summary>
        ''' <param name="strItem"></param>
        private Function EnqueueItemAndCheckForCancel(item As QueueItem) As Boolean

            Dim cancelWasClicked As Boolean  = false

            lock (m_downloadItemsQueue)
            {

                If ( Not m_downloadItemsQueue.Contains(item)) Then
#if DEBUG
                    MessageTracer.TraceMessage("QueueItem: task=" + item.oTask.ToString() + " uri=" + item.uriToDownload.ToString() + " isoUri=" + item.uriToDownload.StreamName)
#endif
                    m_downloadItemsQueue.Enqueue(item)

                    If (Me.m_progressDialog  IsNot Nothing) Then
                        Me.m_progressDialog.IncrementTotalItemCount()
                        cancelWasClicked = Me.m_progressDialog.CancelWasClicked
                    End If
                End If
#if DEBUG
                else
                {
                    MessageTracer.TraceMessage("QueueItem Item already in queue:task=" + item.oTask.ToString() + " uri=" + item.uriToDownload.ToString())
                }
#endif
            }
            return cancelWasClicked
        End Function  '   EnqueueItemAndCheckForCancel


        ''' <summary>
        ''' add a mediaitem (image or video/audio) to list of items to download
        ''' </summary>
        ''' <param name="strItem"></param>
        private Function QueueFileItemForDownloadAndCheckForCancel(oUri As Uri) As Boolean

            '  Some items may be null -- thumbsource for example

            If (oUri <> Nothing  AndAlso   Not String.IsNullOrEmpty(oUri.ToString())) Then

                Dim item As QueueItem  = New QueueItem(QueueItemTask.downloadFile, oUri)

                return Me.EnqueueItemAndCheckForCancel(item)
            End If

            return false
        End Function  '   QueueFileItemForDownloadAndCheckForCancel


        ''' <summary>
        ''' adds all the adaptive chunk items to the queue
        ''' </summary>
        ''' <param name="playlistItem"></param>
        internal bool ProcessDownloadedOfflineManifest(PlaylistItem playlistItem, IsoUri manifest)
        {
#if DEBUG
            MessageTracer.TraceMessage("ProcessDownloadedOfflineManifest")
#endif

            If (manifest.IsoFileExists) Then

                Dim offlineSupport As IPlugInMssOfflineSupport  = Me.m_player.SmoothStreamingOfflineSupport

                Debug.Assert(offlineSupport  IsNot Nothing, "ProcessDownloadedOfflineManifest offlineSupport object not ready Not ")

                If (offlineSupport  IsNot Nothing) Then

                    Dim manifestStream As Stream  = manifest.Stream
                    '  Provide host URL so chunk URLs refer to the host
                    Dim manifestUri As Uri  = manifest.FullyQualifiedUri

                    Debug.WriteLine("manifest XML is: " + manifestStream.Length.ToString() + " bytes long")
                    Debug.WriteLine("manifest URL is: " + manifestUri.OriginalString)

                    '  But parse the local copy of manifest all ready on hand
                    offlineSupport.ParseManifestFromStream(manifestStream, manifestUri)

                    long recommendedBitrateInKbpsAudio = offlineSupport.RecommendBitrateInKbps(MediaStreamType.Audio, Me.playerSize)

                    long recommendedBitrateInKbpsVideo

                    If (playlistItem.OfflineVideoBitrateInKbps > 0) Then
                        recommendedBitrateInKbpsVideo = playlistItem.OfflineVideoBitrateInKbps
                    else
                        recommendedBitrateInKbpsVideo = offlineSupport.RecommendBitrateInKbps(MediaStreamType.Video, Me.playerSize)

                        If (recommendedBitrateInKbpsVideo > 0) Then
                            playlistItem.OfflineVideoBitrateInKbps = recommendedBitrateInKbpsVideo
#if DEBUG

                            Dim source As String  = playlistItem.MediaSource.OriginalString

                            source = Path.GetDirectoryName(source)
                            source = Path.GetFileNameWithoutExtension(source)
                            Me.m_progressDialog.InfoMessage(source + " " + recommendedBitrateInKbpsVideo.ToString())
#endif
                        End If
                    End If

                    Dim result As Boolean  = false

                    using (ManualResetEvent doneWithListOfChunks = New ManualResetEvent(false))
                    {
                        DispatcherOperation o = Me.m_progressDialog.Dispatcher.BeginInvoke(() => GetListOfChunks(doneWithListOfChunks, offlineSupport, recommendedBitrateInKbpsAudio, recommendedBitrateInKbpsVideo))
#if DEBUG
                        '  Wait up to 10 minutes for GetListOfChunks to complete in debug while dev is stepping through the code
                        const int timeoutForGetListOfChunks = 600000
#else
                        '  Wait up to 6 seconds for GetListOfChunks to complete in release
                        const int timeoutForGetListOfChunks = 6000
#endif
                        result = doneWithListOfChunks.WaitOne(timeoutForGetListOfChunks)
                    }
                    return result
                End If
            End If
            return false
        }

        private Sub GetListOfChunks(doneWithListOfChunks As ManualResetEvent, offlineSupport As IPlugInMssOfflineSupport, recommendedBitrateInKbpsAudio As long, recommendedBitrateInKbpsVideo As long)

            Try
                ReadOnlyCollection<Uri> chunkUrisScript = offlineSupport.GetChunkUris(MediaStreamType.Script, 0)

                If (chunkUrisScript  IsNot Nothing) Then

                    If (chunkUrisScript.Count > 0) Then
                        Debug.WriteLine("ScriptChunks=" + chunkUrisScript.Count.ToString())

                        Dim item As QueueItem  = New QueueItem(chunkUrisScript)

                        If (Me.EnqueueItemAndCheckForCancel(item) Then
                            return
                        End If
                    End If
                End If


                '  Could be audio only

                If (recommendedBitrateInKbpsVideo > 0) Then
                    ReadOnlyCollection<Uri> chunkUrisVideo = offlineSupport.GetChunkUris(MediaStreamType.Video, recommendedBitrateInKbpsVideo)
                    Debug.Assert(chunkUrisVideo  IsNot Nothing)

                    If (chunkUrisVideo  IsNot Nothing) Then

                        If (chunkUrisVideo.Count > 0) Then
                            Debug.WriteLine("VideoChunks=" + chunkUrisVideo.Count.ToString())

                            Dim item As QueueItem  = New QueueItem(chunkUrisVideo)

                            If (Me.EnqueueItemAndCheckForCancel(item) Then
                                return
                            End If
                        End If
                    End If
                End If

                '  could be video only

                If (recommendedBitrateInKbpsAudio > 0) Then
                    ReadOnlyCollection<Uri> chunkUrisAudio = offlineSupport.GetChunkUris(MediaStreamType.Audio, recommendedBitrateInKbpsAudio)
                    Debug.Assert(chunkUrisAudio  IsNot Nothing)

                    If (chunkUrisAudio  IsNot Nothing) Then

                        If (chunkUrisAudio.Count > 0) Then
                            Debug.WriteLine("AudioChunks=" + chunkUrisAudio.Count.ToString())

                            Dim item As QueueItem  = New QueueItem(chunkUrisAudio)

                            If (Me.EnqueueItemAndCheckForCancel(item) Then
                                return
                            End If
                        End If
                    End If
                End If

            finally

                doneWithListOfChunks.Set()
            End Try
        End Sub '   GetListOfChunks

        ''' <summary>
        ''' Background run able task to place all the chunk items in the download queue
        ''' </summary>
        ''' <param name="chunkUris"></param>
        internal bool EnqueueChunkList(ReadOnlyCollection<Uri> chunkUris)
        {

            If (chunkUris  IsNot Nothing) Then

                For Each var chunkUri in chunkUris

#if DEBUG
                    MessageTracer.TraceMessage("Queueing ChunkUrl: " + chunkUri.ToString())
#endif

                    Dim item As QueueItem  = New QueueItem(QueueItemTask.downloadFile, chunkUri)
                    '  on cancel don't retry the same item

                    If (Me.EnqueueItemAndCheckForCancel(item)) return true Then
                Next    '   var

                return true
                    End If
            End If

            return false
        }

        ''' <summary>
        ''' seperate thread to download all the queued media items.
        ''' </summary>
        ''' <param name="arg"></param>
        private Sub DownloadThreadProc()

            Dim countLastLoop As Integer  = 0


            While (true)

                Dim item As QueueItem  = Nothing

                '  Get next item from queue in thread safe manner
                lock (Me.m_downloadItemsQueue)
                {
                    '  Add the size of the new items to total size count
                    Dim countThisLoop As Integer  = Me.m_downloadItemsQueue.Count


                    If (countThisLoop >= countLastLoop) Then
#if DEBUG
                        MessageTracer.TraceMessage("Recalculating size of downloaded items")
#endif
                    ElseIf (countThisLoop < 1) Then
                    End If

                    countLastLoop = countThisLoop

                    '  Pull next item from the queue
                    item = Me.m_downloadItemsQueue.Dequeue()
                }

                Me.m_progressDialog.CurrentItem += 1

                '  Do the task

                If ( Not item.DoTask(this)) Then

                    If (item.RetryCount < QueueItem.MaxRetries) Then
                        item.BumpRetryCount()
                        Debug.WriteLine("Retrying Item: " + item.ToString())
                        if (Me.EnqueueItemAndCheckForCancel(item))
                        {

                        }
                    else

                        If (Me.m_progressDialog  IsNot Nothing) Then
                            Me.m_progressDialog.ReportError(item.uriToDownload.DownloadErrorMessage)
                        End If
                    End If
                End If



                If (Me.m_progressDialog.CancelWasClicked) Then
                End If
            End While   '

            RemoveHandler Me.PlaylistDownloadProgressChanged, AddressOf New

            Me.IsDownloading = false


            If (Me.m_progressDialog.CancelWasClicked) Then
                Me.m_player.Dispatcher.BeginInvoke
                    (delegate
                    {
                        Me.m_progressDialog.Close()
                    })

            ElseIf ( Not Me.m_progressDialog.ErrorOccured) Then
                Try
                    '''
                    ''' copy playlist XML to isolated storage...
                    '''
                    WriteToIsoStorage()
                    '''
                    ''' write marker file (also contains baseUri) to isolated storage...
                    '''
                    WriteDownloadCompleteFile()


                    If (Me.m_player  IsNot Nothing) Then
                        Me.m_player.Dispatcher.BeginInvoke
                            (delegate
                            {
                                Me.m_progressDialog.DownloadCompletedSuccessfully()
                            })
                    End If


                Catch iso As IsolatedStorageException


                    If (Me.m_progressDialog  IsNot Nothing) Then
                        Me.m_progressDialog.ReportError(iso.Message)
                    End If
                End Try
            End If
        End Sub '   DownloadThreadProc

        internal const string downloadCompletedFilename = "DownloadCompleted.txt"

        internal static bool ContentNeedsDownloading()
        {

            Dim isoStore As IsolatedStorageFile

            Try
                isoStore = IsolatedStorageFile.GetUserStoreForApplication()

                If (isoStore.FileExists(downloadCompletedFilename)) Then
                    return false
                End If


            Catch ise As IsolatedStorageException

                Debug.WriteLine("IsolatedStorageException checking for download completed file" + ise.ToString())
            End Try

            return true
        }

        Sub WriteDownloadCompleteFile()


            If (Me.m_baseUriForTakingOffline.IsAbsoluteUri) Then
                Try
                    var isoStore = IsolatedStorageFile.GetUserStoreForApplication()
                    var streamWriter = New StreamWriter(isoStore.CreateFile(downloadCompletedFilename), Encoding.Unicode)
                    streamWriter.WriteLine(Me.m_baseUriForTakingOffline.AbsoluteUri)
                    streamWriter.Close()
#if DEBUG

                    Dim msg As String  = "Files in isolatedstorage: " + isoStore.GetFileNames().Length.ToString() + ChrW(13) & ChrW(10)

                    + "Space Left:" + isoStore.AvailableFreeSpace.ToString() + " of " + isoStore.Quota.ToString() + ChrW(13) & ChrW(10)
                    + downloadCompletedFilename
                    MessageTracer.TraceMessage(msg)
#endif

                Catch isoException As IsolatedStorageException

                    if (Me.m_progressDialog  IsNot Nothing)
                    {
                        Me.m_progressDialog.ReportError(isoException.Message)
                    }
                End Try

            else
                throw New ArgumentException("must have an absolute Uri to tak offline")
            End If
        End Sub '   WriteDownloadCompleteFile

        ''' <summary>
        ''' update download progress
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        public Sub PlaylistCollection_UriDownloadProgressChanged(sender As Object, e As UriDownloadProgressRoutedEventArgs)


            If (PlaylistDownloadProgressChanged  IsNot Nothing) Then
                PlaylistDownloadProgressChanged(this, New PlaylistDownloadProgressEventArgs(e.Progress))
            End If
        End Sub '   PlaylistCollection_UriDownloadProgressChanged

        internal static Sub ClearPlaylistItemsFromIsolatedStorage()

            IsolatedStorageFile.GetUserStoreForApplication().Remove()
        End Sub '   ClearPlaylistItemsFromIsolatedStorage


        internal long spaceNeededForOfflinePlaylistCache = -1

        internal long ComputeSpaceNeededForOfflinePlaylist()
        {

            If (Me.spaceNeededForOfflinePlaylistCache > 0) Then
                return Me.spaceNeededForOfflinePlaylistCache
            End If


            '
            '  calculate space
            '
            '  1Mb for playlist etc.
            long totalSpaceRequired = 1 << 20

            Dim anyUnknown As Boolean  = false


            For Each playlistItem As PlaylistItem in Items


                If (playlistItem.FileSize <= 0) Then
                    anyUnknown = true
#if DEBUG
                    MessageTracer.TraceMessage("Figuring item Size unknown size:" + playlistItem.MediaSource.ToString())
#endif
                else
                    var itemSize = playlistItem.FileSize
#if DEBUG
                    MessageTracer.TraceMessage("Figuring item Size:" + playlistItem.MediaSource.ToString() + " Size:" + itemSize.ToString())
#endif
                    totalSpaceRequired += itemSize
                End If
            Next    '   playlistItem


            '
            '  if we cant determine it we try for at least 50Mb...
            '

            If (anyUnknown) Then
#if DEBUG
                MessageTracer.TraceMessage("Adding 50MB to buffer for size of unknown items")
#endif
                totalSpaceRequired = Math.Max(totalSpaceRequired, 50 * 1024 * 1024)
            End If


#if DEBUG
            MessageTracer.TraceMessage("Estimated Storage size:" + totalSpaceRequired.ToString())
#endif
            Me.spaceNeededForOfflinePlaylistCache = totalSpaceRequired
            return totalSpaceRequired
        }

        internal long SpaceQuotaRequiredIfMoreIsNeeded()
        {
            long totalSpaceRequired = ComputeSpaceNeededForOfflinePlaylist()

            Dim isoStore As IsolatedStorageFile  = IsolatedStorageFile.GetUserStoreForApplication()

            long spaceQuota = isoStore.Quota
#if DEBUG
            MessageTracer.TraceMessage("Quota size:" + spaceQuota.ToString())
#endif
            long usedSize = isoStore.UsedSize
#if DEBUG
            MessageTracer.TraceMessage("Used size:" + usedSize.ToString())
#endif
            long spaceAvailible = (spaceQuota - usedSize)
#if DEBUG
            MessageTracer.TraceMessage("spaceAvailible size:" + spaceAvailible.ToString())
#endif
            long additionalQuotaRequired = (totalSpaceRequired - spaceAvailible)
#if DEBUG
            MessageTracer.TraceMessage("aditionalQuotaRequired:" + additionalQuotaRequired.ToString())
#endif

            If (additionalQuotaRequired > 0) Then
                return spaceQuota + additionalQuotaRequired
            End If

            return 0
        }

        ''' <summary>
        ''' ensure enough iso storage space for files we wish to copy
        ''' </summary>
        ''' <returns></returns>
        internal bool EnsureStorageSpace()
        {
            Try
                ClearPlaylistItemsFromIsolatedStorage()

                long moreSpaceNeeded = SpaceQuotaRequiredIfMoreIsNeeded()

                If (moreSpaceNeeded > 0) Then

                    Dim isoStore As IsolatedStorageFile  = IsolatedStorageFile.GetUserStoreForApplication()


                    If ( Not isoStore.IncreaseQuotaTo(moreSpaceNeeded)) Then
#if DEBUG
                        MessageTracer.TraceMessage("Failed to increase quota Not ")
#endif
                        return false
                    End If
                End If
            End Try


            catch (IsolatedStorageException iso)
            {
#if DEBUG
                MessageTracer.TraceMessage("IsolatedStorageException attempting to increase quota Not " + iso.Message)
#endif

                If (Me.m_player  IsNot Nothing) Then
                    Me.m_player.ShowErrorMessage(iso.Message)
                End If

                return false
            }
            return true
        }

        ''' <summary>
        ''' make a local isolated storage cache of this playlist
        ''' </summary>
        public Sub TakeOffline(player As MediaPlayer)


            If (Me.IsDownloading) Then
                throw New PlaylistTakeOfflineException("TakeOffline called while already downloading")
            End If



            If ( Not HtmlPage.IsEnabled  OrElse  HtmlPage.Document Is Nothing  OrElse  HtmlPage.Document.DocumentUri=Nothing) Then
                throw New PlaylistTakeOfflineException("TakeOffline called with HtmlPage in invalid state")
            End If


            Me.m_player = player
            Me.playerSize = New Size(Me.m_player.ActualWidth, Me.m_player.ActualHeight)
            Me.m_progressDialog = New OfflineDownloadProgressDialog(Me.m_player)
            Me.m_progressDialog.Show()
            AddHandler Me.PlaylistDownloadProgressChanged, AddressOf New EventHandler<PlaylistDownloadProgressEventArgs>(Me.m_progressDialog.DownloadProgressChanged)

            Dim xapUriRelative As Uri  = New Uri(MediaPlayer.SmoothStreamingXAPName, UriKind.Relative)
            Dim xapUriFull As Uri  = New Uri(HtmlPage.Document.DocumentUri, xapUriRelative)
            Dim baseUrl As String  = xapUriFull.AbsoluteUri.Replace(MediaPlayer.SmoothStreamingXAPName, string.Empty)

            Me.m_baseUriForTakingOffline = New Uri(baseUrl, UriKind.Absolute)

#if DEBUG
            MessageTracer.SetPlayer(player)
#endif
            Me.IsDownloading = true

            Dim needSmoothStreamingXAP As Boolean  = false
            Dim needTimedTextLibraryXAP As Boolean  = false

            '  if any items are smooth streaming items -- init support for that

            For Each playlistItem As PlaylistItem in Items


                If ( Not needSmoothStreamingXAP) Then

                    If (playlistItem.IsAdaptiveStreaming) Then
                        needSmoothStreamingXAP = true
                        player.LoadSmoothStreamingModuleOnlineToGoOffline()
                    End If
                End If

                If ( Not needTimedTextLibraryXAP) Then

                    If (playlistItem.CaptionSources  IsNot Nothing) Then

                        If (playlistItem.CaptionSources.Count > 0) Then
                            needTimedTextLibraryXAP = true
                        End If
                    End If
                End If


                If (needSmoothStreamingXAP  AndAlso  needTimedTextLibraryXAP) Then
                End If
            Next    '   playlistItem


            Me.m_downloadItemsQueue = New Queue<QueueItem>()


            If (needSmoothStreamingXAP) Then

                Dim xapSmoothStreaming As Uri  = New Uri(MediaPlayer.SmoothStreamingXAPName, UriKind.Relative)

                If (Me.QueueFileItemForDownloadAndCheckForCancel(xapSmoothStreaming) Then
                    return
                End If
            End If


            If (needTimedTextLibraryXAP) Then

                Dim xapTimedTextLibrary As Uri  = New Uri(CaptionsManager.TimedTextXAPName, UriKind.Relative)

                If (Me.QueueFileItemForDownloadAndCheckForCancel(xapTimedTextLibrary) Then
                    return
                End If
            End If

            '''
            ''' queue media items for download to isolated storage
            '''

            For Each playlistItem As PlaylistItem in Items

                If (Me.QueueFileItemForDownloadAndCheckForCancel(playlistItem.MediaSource) Then
                    return
                End If

                If (Me.QueueFileItemForDownloadAndCheckForCancel(playlistItem.ThumbSource) Then
                    return
                End If



                For Each chapterItem As ChapterItem in playlistItem.Chapters

                    If (Me.QueueFileItemForDownloadAndCheckForCancel(chapterItem.ThumbSource) Then
                        return
                    End If
                Next    '   chapterItem


                If (playlistItem.IsAdaptiveStreaming) Then

                    Dim processManifestItem As QueueItem  = New QueueItem(QueueItemTask.processManifest, playlistItem)

                    If (Me.EnqueueItemAndCheckForCancel(processManifestItem) Then
                        return
                    End If
                End If
                '  DFXP data for Smooth Streaming is loaded from chunks / ISMT

                If (playlistItem.CaptionSources  IsNot Nothing  AndAlso   Not playlistItem.IsAdaptiveStreaming) Then

                    For Each var captionSource in playlistItem.CaptionSources

                        If (Me.QueueFileItemForDownloadAndCheckForCancel(captionSource.CaptionFileSource) Then
                            return
                        End If
                    Next    '   var
                End If
            Next    '   playlistItem


            '''
            ''' copy media items to isolated storage
            '''
            Thread thread = New Thread(DownloadThreadProc)
            thread.Start()
        End Sub '   TakeOffline


        #region OfflineSerializationSupport
        ''' <summary>
        ''' top level XML node
        ''' </summary>
        private const string s_PlaylistIsoName = "Playlist.xml"
        ''' <summary>
        ''' persist playlist in offline storage
        ''' </summary>
        public Sub WriteToIsoStorage()

            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                XmlWriterSettings xmlws = New XmlWriterSettings()
                xmlws.Indent = true
                using (IsolatedStorageFileStream isoStream = isoStore.CreateFile(s_PlaylistIsoName))
                using (XmlWriter writer = XmlWriter.Create(isoStream, xmlws))
                {
                    Me.Serialize(writer)
                }
            }
        End Sub '   WriteToIsoStorage
        ''' <summary>
        ''' load this playlist from the offline storage
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Playlist() As
        #End Region
    End Class   '   Playlist
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\PlayListOffline.cs
