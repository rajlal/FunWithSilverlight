// <copyright file="PlaylistCollection.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>Implements the PlaylistCollection class</summary>
// <author>Microsoft Expression Encoder Team</author>

using Microsoft.Expression.Encoder.PlugInMssCtrl;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;

namespace ExpressionMediaPlayer
{
    /// <summary>
    /// exception for errors when taking the playlist offline
    /// </summary>
    public class PlaylistTakeOfflineException : Exception
    {
        public PlaylistTakeOfflineException()
            : base(Resources.errorInvalidPlaylist)
        {
        }

        public PlaylistTakeOfflineException(string message)
            : base(message)
        {
        }

        public PlaylistTakeOfflineException(string message, Exception exp)
            : base(message, exp)
        {
        }

        public PlaylistTakeOfflineException(string nodeFound, string nodeExpected)
            : base(String.Format(CultureInfo.CurrentUICulture, Resources.errorInvalidPlaylistNode, nodeFound, nodeExpected))
        {
        }
    }

    /// <summary>
    /// event args for downloading playlist events
    /// </summary>
    public class PlaylistDownloadProgressEventArgs : EventArgs
    {
        public PlaylistDownloadProgressEventArgs(double dblProgress)
        {
            Progress = dblProgress;
        }

        public double Progress { get; private set; }
    }

    internal enum QueueItemTask
    {
        downloadFile,
        processManifest,
        queueChunkList
    }

    internal class QueueItem : IDisposable
    {
        internal QueueItemTask task;
        internal IsoUri uriToDownload;
        internal PlaylistItem playlistItem;
        internal ReadOnlyCollection<Uri> chunksToQueue;
        internal int retryCount = 0;
        internal const int MaxRetries = 3;

        internal QueueItem(QueueItemTask task, Uri uri)
        {
            this.task = task;
            this.SetUri(uri);
        }

        internal QueueItem(QueueItemTask task, PlaylistItem playlistItemParameter)
        {
            this.task = task;
            this.playlistItem = playlistItemParameter;
            this.SetUri(playlistItemParameter.MediaSource);
        }

        internal QueueItem(ReadOnlyCollection<Uri> chunksToQueue)
        {
            this.task = QueueItemTask.queueChunkList;
            this.SetUri(new Uri(@"about:blank"));
            this.chunksToQueue = chunksToQueue;
        }

        internal void SetUri(Uri uri)
        {
            Uri absoluteUri = uri;
            if (!absoluteUri.IsAbsoluteUri && HtmlPage.Document.DocumentUri!=null )
            {
                absoluteUri = new Uri(HtmlPage.Document.DocumentUri, uri);
            }
            this.uriToDownload = new IsoUri(absoluteUri);
        }

        public void Dispose()
        {
            this.uriToDownload.Dispose();
            GC.SuppressFinalize(this);
        }

        internal int RetryCount { get { return this.retryCount; } }

        internal int BumpRetryCount()
        {
            this.retryCount++;
#if DEBUG
            MessageTracer.TraceMessage("Retry:" + this.retryCount.ToString() + " task: " + this.task.ToString() + " url=" + this.uriToDownload.ToString());
#endif
            return this.retryCount;
        }

        internal bool DoTask(Playlist playlist)
        {
#if DEBUG
            MessageTracer.TraceMessage("DoTask: retry=" + this.retryCount.ToString() + " now=" + DateTime.Now.ToString() + " task: " + this.task.ToString() + " url=" + this.uriToDownload.ToString());
#endif
            switch (this.task)
            {
                case QueueItemTask.downloadFile:
                    this.uriToDownload.UriDownloadProgressChanged += new EventHandler<UriDownloadProgressRoutedEventArgs>(playlist.PlaylistCollection_UriDownloadProgressChanged);
                    this.uriToDownload.Download();
                    return this.uriToDownload.DownloadSucceeded;
                case QueueItemTask.processManifest:
                    return playlist.ProcessDownloadedOfflineManifest(this.playlistItem, this.uriToDownload);
                case QueueItemTask.queueChunkList:
                    return playlist.EnqueueChunkList(this.chunksToQueue);
            }
            throw new PlaylistTakeOfflineException("Develoer Error!: new task member added to QueueItemTask enum but not added the DoTask() Switch statement!");
        }
    }

#if DEBUG
    static class MessageTracer
    {
        static private MediaPlayer m_player;

        static internal void SetPlayer(MediaPlayer player)
        {
            m_player = player;
        }

        static internal void TraceMessage(string message)
        {
            Debug.WriteLine(message);
            if (m_player != null)
            {
                m_player.SetNextTickErrorMessage(message);
            }
        }
    }
#endif
    
    public partial class Playlist : DependencyObject
    {
        private OfflineDownloadProgressDialog m_progressDialog;
        private MediaPlayer m_player;
        private Size playerSize;
        private Uri m_baseUriForTakingOffline;


        /// <summary>
        /// event for download progress of playlist items.
        /// </summary>
        public event EventHandler<PlaylistDownloadProgressEventArgs> PlaylistDownloadProgressChanged;

        /// <summary>
        /// is playlist currently downloading?
        /// </summary>
        internal bool IsDownloading
        {
            get;
            private set;
        }

        /// <summary>
        /// items to download if taking this playlist offline
        /// </summary>
        private Queue<QueueItem> m_downloadItemsQueue = null;

        /// <summary>
        /// Add a work item to the Queue
        /// </summary>
        /// <param name="strItem"></param>
        private bool EnqueueItemAndCheckForCancel(QueueItem item)
        {
            bool cancelWasClicked = false;
            lock (m_downloadItemsQueue)
            {
                if (!m_downloadItemsQueue.Contains(item))
                {
#if DEBUG                     
                    MessageTracer.TraceMessage("QueueItem: task=" + item.task.ToString() + " uri=" + item.uriToDownload.ToString() + " isoUri=" + item.uriToDownload.StreamName);
#endif
                    m_downloadItemsQueue.Enqueue(item);
                    if (this.m_progressDialog != null)
                    {
                        this.m_progressDialog.IncrementTotalItemCount();
                        cancelWasClicked = this.m_progressDialog.CancelWasClicked;
                    }
                }
#if DEBUG
                else
                {
                    MessageTracer.TraceMessage("QueueItem Item already in queue:task=" + item.task.ToString() + " uri=" + item.uriToDownload.ToString());
                }
#endif
            }
            return cancelWasClicked;
        }

        /// <summary>
        /// add a mediaitem (image or video/audio) to list of items to download
        /// </summary>
        /// <param name="strItem"></param>
        private bool QueueFileItemForDownloadAndCheckForCancel(Uri uri)
        {
            if (uri!=null && !String.IsNullOrEmpty(uri.ToString())) // Some items may be null -- thumbsource for example
            {
                QueueItem item = new QueueItem(QueueItemTask.downloadFile, uri);
                return this.EnqueueItemAndCheckForCancel(item);
            }
            return false;
        }

        /// <summary>
        /// adds all the adaptive chunk items to the queue
        /// </summary>
        /// <param name="playlistItem"></param>
        internal bool ProcessDownloadedOfflineManifest(PlaylistItem playlistItem, IsoUri manifest)
        {  
#if DEBUG
            MessageTracer.TraceMessage("ProcessDownloadedOfflineManifest");
#endif
            if (manifest.IsoFileExists)
            {
                IPlugInMssOfflineSupport offlineSupport = this.m_player.SmoothStreamingOfflineSupport;
                Debug.Assert(offlineSupport != null, "ProcessDownloadedOfflineManifest offlineSupport object not ready!");
                if (offlineSupport != null)
                {
                    Stream manifestStream = manifest.Stream;
                    Uri manifestUri = manifest.FullyQualifiedUri; // Provide host URL so chunk URLs refer to the host

                    Debug.WriteLine("manifest XML is: " + manifestStream.Length.ToString() + " bytes long");
                    Debug.WriteLine("manifest URL is: " + manifestUri.OriginalString);

                    offlineSupport.ParseManifestFromStream(manifestStream, manifestUri);  // But parse the local copy of manifest all ready on hand

                    long recommendedBitrateInKbpsAudio = offlineSupport.RecommendBitrateInKbps(MediaStreamType.Audio, this.playerSize);

                    long recommendedBitrateInKbpsVideo;
                    if (playlistItem.OfflineVideoBitrateInKbps > 0)
                    {
                        recommendedBitrateInKbpsVideo = playlistItem.OfflineVideoBitrateInKbps;
                    }
                    else
                    {
                        recommendedBitrateInKbpsVideo = offlineSupport.RecommendBitrateInKbps(MediaStreamType.Video, this.playerSize);
                        if (recommendedBitrateInKbpsVideo > 0)
                        {
                            playlistItem.OfflineVideoBitrateInKbps = recommendedBitrateInKbpsVideo;
#if DEBUG
                            string source = playlistItem.MediaSource.OriginalString;
                            source = Path.GetDirectoryName(source);
                            source = Path.GetFileNameWithoutExtension(source);
                            this.m_progressDialog.InfoMessage(source + " " + recommendedBitrateInKbpsVideo.ToString());
#endif
                        }
                    }

                    bool result = false;
                    using (ManualResetEvent doneWithListOfChunks = new ManualResetEvent(false))
                    {
                        DispatcherOperation o = this.m_progressDialog.Dispatcher.BeginInvoke(() => GetListOfChunks(doneWithListOfChunks, offlineSupport, recommendedBitrateInKbpsAudio, recommendedBitrateInKbpsVideo));
#if DEBUG
                        const int timeoutForGetListOfChunks = 600000; // Wait up to 10 minutes for GetListOfChunks to complete in debug while dev is stepping through the code
#else
                        const int timeoutForGetListOfChunks = 6000;  // Wait up to 6 seconds for GetListOfChunks to complete in release
#endif
                        result = doneWithListOfChunks.WaitOne(timeoutForGetListOfChunks);
                    }
                    return result;
                }
            }
            return false;
        }

        private void GetListOfChunks(ManualResetEvent doneWithListOfChunks, IPlugInMssOfflineSupport offlineSupport, long recommendedBitrateInKbpsAudio, long recommendedBitrateInKbpsVideo)
        {
            try
            {
                ReadOnlyCollection<Uri> chunkUrisScript = offlineSupport.GetChunkUris(MediaStreamType.Script, 0);
                if (chunkUrisScript != null)
                {
                    if (chunkUrisScript.Count > 0)
                    {
                        Debug.WriteLine("ScriptChunks=" + chunkUrisScript.Count.ToString());
                        QueueItem item = new QueueItem(chunkUrisScript);
                        if (this.EnqueueItemAndCheckForCancel(item)) return;
                    }
                }

                if (recommendedBitrateInKbpsVideo > 0) // Could be audio only
                {
                    ReadOnlyCollection<Uri> chunkUrisVideo = offlineSupport.GetChunkUris(MediaStreamType.Video, recommendedBitrateInKbpsVideo);
                    Debug.Assert(chunkUrisVideo != null);
                    if (chunkUrisVideo != null)
                    {
                        if (chunkUrisVideo.Count > 0)
                        {
                            Debug.WriteLine("VideoChunks=" + chunkUrisVideo.Count.ToString());
                            QueueItem item = new QueueItem(chunkUrisVideo);
                            if (this.EnqueueItemAndCheckForCancel(item)) return;
                        }
                    }
                }

                if (recommendedBitrateInKbpsAudio > 0) // could be video only
                {
                    ReadOnlyCollection<Uri> chunkUrisAudio = offlineSupport.GetChunkUris(MediaStreamType.Audio, recommendedBitrateInKbpsAudio);
                    Debug.Assert(chunkUrisAudio != null);
                    if (chunkUrisAudio != null)
                    {
                        if (chunkUrisAudio.Count > 0)
                        {
                            Debug.WriteLine("AudioChunks=" + chunkUrisAudio.Count.ToString());
                            QueueItem item = new QueueItem(chunkUrisAudio);
                            if (this.EnqueueItemAndCheckForCancel(item)) return;
                        }
                    }
                }
            }
            finally
            {
                doneWithListOfChunks.Set();
            }
        }

        /// <summary>
        /// Background run able task to place all the chunk items in the download queue
        /// </summary>
        /// <param name="chunkUris"></param>
        internal bool EnqueueChunkList(ReadOnlyCollection<Uri> chunkUris)
        {
            if (chunkUris != null)
            {
                foreach (var chunkUri in chunkUris)
                {
#if DEBUG
                    MessageTracer.TraceMessage("Queueing ChunkUrl: " + chunkUri.ToString());
#endif
                    QueueItem item = new QueueItem(QueueItemTask.downloadFile, chunkUri);
                    if (this.EnqueueItemAndCheckForCancel(item)) return true; // on cancel don't retry the same item
                }
                return true;
            }
            return false;
        }


        /// <summary>
        /// seperate thread to download all the queued media items.
        /// </summary>
        /// <param name="arg"></param>
        private void DownloadThreadProc()
        {
            int countLastLoop = 0;

            while (true)
            {
                QueueItem item = null;

                // Get next item from queue in thread safe manner
                lock (this.m_downloadItemsQueue)
                {
                    // Add the size of the new items to total size count
                    int countThisLoop = this.m_downloadItemsQueue.Count;
                    if (countThisLoop >= countLastLoop)
                    {
#if DEBUG
                        MessageTracer.TraceMessage("Recalculating size of downloaded items");
#endif
                    }
                    else if (countThisLoop < 1)
                    {
                        break;
                    }
                    countLastLoop = countThisLoop;

                    // Pull next item from the queue
                    item = this.m_downloadItemsQueue.Dequeue();
                }

                this.m_progressDialog.CurrentItem++;
               
                // Do the task
                if (!item.DoTask(this))
                {
                    if (item.RetryCount < QueueItem.MaxRetries)
                    {
                        item.BumpRetryCount();
                        Debug.WriteLine("Retrying Item: " + item.ToString());
                        if (this.EnqueueItemAndCheckForCancel(item))
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (this.m_progressDialog != null)
                        {
                            this.m_progressDialog.ReportError(item.uriToDownload.DownloadErrorMessage);                            
                        }
                        break;
                    }
                }

                if (this.m_progressDialog.CancelWasClicked)
                {
                    break;
                }
            }

            this.PlaylistDownloadProgressChanged -= new EventHandler<PlaylistDownloadProgressEventArgs>(this.m_progressDialog.DownloadProgressChanged);
            
            this.IsDownloading = false;

            if (this.m_progressDialog.CancelWasClicked)
            {
                this.m_player.Dispatcher.BeginInvoke
                    (delegate
                    {
                        this.m_progressDialog.Close();
                    });
              
            }
            else if (!this.m_progressDialog.ErrorOccured)
            {
                try
                {
                    ///
                    /// copy playlist XML to isolated storage...
                    ///
                    WriteToIsoStorage();
                    ///
                    /// write marker file (also contains baseUri) to isolated storage...
                    ///
                    WriteDownloadCompleteFile();

                    if (this.m_player != null)
                    {
                        this.m_player.Dispatcher.BeginInvoke
                            (delegate
                            {
                                this.m_progressDialog.DownloadCompletedSuccessfully();
                            });
                    }
                }
                catch (IsolatedStorageException iso)
                {
                    if (this.m_progressDialog != null)
                    {
                        this.m_progressDialog.ReportError(iso.Message);
                    }
                }
            }
        }

        internal const string downloadCompletedFilename = "DownloadCompleted.txt";

        internal static bool ContentNeedsDownloading()
        {
            IsolatedStorageFile isoStore;
            try
            {
                isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                if (isoStore.FileExists(downloadCompletedFilename))
                {
                    return false;
                }
            }
            catch (IsolatedStorageException ise)
            {
                Debug.WriteLine("IsolatedStorageException checking for download completed file" + ise.ToString());
            }
            return true;
        }

        void WriteDownloadCompleteFile()
        {
            if (this.m_baseUriForTakingOffline.IsAbsoluteUri)
            {
                try
                {
                    var isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                    var streamWriter = new StreamWriter(isoStore.CreateFile(downloadCompletedFilename), Encoding.Unicode);
                    streamWriter.WriteLine(this.m_baseUriForTakingOffline.AbsoluteUri);
                    streamWriter.Close();
#if DEBUG
                    string msg = "Files in isolatedstorage: " + isoStore.GetFileNames().Length.ToString() + "\r\n"
                    + "Space Left:" + isoStore.AvailableFreeSpace.ToString() + " of " + isoStore.Quota.ToString() + "\r\n"
                    + downloadCompletedFilename;
                    MessageTracer.TraceMessage(msg);
#endif
                }
                catch (IsolatedStorageException isoException)
                {
                    if (this.m_progressDialog != null)
                    {
                        this.m_progressDialog.ReportError(isoException.Message);
                    }
                }
            }
            else
            {
                throw new ArgumentException("must have an absolute Uri to tak offline");
            }
        }

        /// <summary>
        /// update download progress
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PlaylistCollection_UriDownloadProgressChanged(object sender, UriDownloadProgressRoutedEventArgs e)
        {
            if (PlaylistDownloadProgressChanged != null)
            {
                PlaylistDownloadProgressChanged(this, new PlaylistDownloadProgressEventArgs(e.Progress));
            }
        }

        internal static void ClearPlaylistItemsFromIsolatedStorage()
        {
            IsolatedStorageFile.GetUserStoreForApplication().Remove();
        }

        internal long spaceNeededForOfflinePlaylistCache = -1;

        internal long ComputeSpaceNeededForOfflinePlaylist()
        {
            if (this.spaceNeededForOfflinePlaylistCache > 0)
            {
                return this.spaceNeededForOfflinePlaylistCache;
            }

            //
            // calculate space
            //
            long totalSpaceRequired = 1 << 20; // 1Mb for playlist etc.
            bool anyUnknown = false;
            foreach (PlaylistItem playlistItem in Items)
            {
                if (playlistItem.FileSize <= 0)
                {
                    anyUnknown = true;
#if DEBUG
                    MessageTracer.TraceMessage("Figuring item Size unknown size:" + playlistItem.MediaSource.ToString());
#endif
                }
                else
                {
                    var itemSize = playlistItem.FileSize;
#if DEBUG
                    MessageTracer.TraceMessage("Figuring item Size:" + playlistItem.MediaSource.ToString() + " Size:" + itemSize.ToString());
#endif
                    totalSpaceRequired += itemSize;
                }
            }

            //
            // if we cant determine it we try for at least 50Mb...
            //
            if (anyUnknown)
            {
#if DEBUG
                MessageTracer.TraceMessage("Adding 50MB to buffer for size of unknown items");
#endif
                totalSpaceRequired = Math.Max(totalSpaceRequired, 50 * 1024 * 1024);
            }

#if DEBUG
            MessageTracer.TraceMessage("Estimated Storage size:" + totalSpaceRequired.ToString());
#endif
            this.spaceNeededForOfflinePlaylistCache = totalSpaceRequired;
            return totalSpaceRequired;
        }

        internal long SpaceQuotaRequiredIfMoreIsNeeded()
        {
            long totalSpaceRequired = ComputeSpaceNeededForOfflinePlaylist();
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
            long spaceQuota = isoStore.Quota;
#if DEBUG
            MessageTracer.TraceMessage("Quota size:" + spaceQuota.ToString());
#endif
            long usedSize = isoStore.UsedSize;
#if DEBUG
            MessageTracer.TraceMessage("Used size:" + usedSize.ToString());
#endif
            long spaceAvailible = (spaceQuota - usedSize);
#if DEBUG
            MessageTracer.TraceMessage("spaceAvailible size:" + spaceAvailible.ToString());
#endif
            long additionalQuotaRequired = (totalSpaceRequired - spaceAvailible);
#if DEBUG
            MessageTracer.TraceMessage("aditionalQuotaRequired:" + additionalQuotaRequired.ToString());
#endif
            if (additionalQuotaRequired > 0)
            {
                return spaceQuota + additionalQuotaRequired;
            }
            return 0;
        }

        /// <summary>
        /// ensure enough iso storage space for files we wish to copy
        /// </summary>
        /// <returns></returns>
        internal bool EnsureStorageSpace()
        {
            try
            {
                ClearPlaylistItemsFromIsolatedStorage();

                long moreSpaceNeeded = SpaceQuotaRequiredIfMoreIsNeeded();
                if (moreSpaceNeeded > 0)
                {
                    IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                    if (!isoStore.IncreaseQuotaTo(moreSpaceNeeded))
                    {
#if DEBUG
                        MessageTracer.TraceMessage("Failed to increase quota!");
#endif
                        return false;
                    }
                }
            }
        
            catch (IsolatedStorageException iso)
            {
#if DEBUG
                MessageTracer.TraceMessage("IsolatedStorageException attempting to increase quota!" + iso.Message);
#endif
                if (this.m_player != null)
                {
                    this.m_player.ShowErrorMessage(iso.Message);
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// make a local isolated storage cache of this playlist
        /// </summary>
        public void TakeOffline(MediaPlayer player)
        {
            if (this.IsDownloading)
            {
                throw new PlaylistTakeOfflineException("TakeOffline called while already downloading");
            }

            if (!HtmlPage.IsEnabled || HtmlPage.Document == null || HtmlPage.Document.DocumentUri==null)
            {
                throw new PlaylistTakeOfflineException("TakeOffline called with HtmlPage in invalid state");
            }

            this.m_player = player;
            this.playerSize = new Size(this.m_player.ActualWidth, this.m_player.ActualHeight);
            this.m_progressDialog = new OfflineDownloadProgressDialog(this.m_player);
            this.m_progressDialog.Show();
            this.PlaylistDownloadProgressChanged += new EventHandler<PlaylistDownloadProgressEventArgs>(this.m_progressDialog.DownloadProgressChanged);

            Uri xapUriRelative = new Uri(MediaPlayer.SmoothStreamingXAPName, UriKind.Relative);
            Uri xapUriFull = new Uri(HtmlPage.Document.DocumentUri, xapUriRelative);
            string baseUrl = xapUriFull.AbsoluteUri.Replace(MediaPlayer.SmoothStreamingXAPName, string.Empty);
            this.m_baseUriForTakingOffline = new Uri(baseUrl, UriKind.Absolute);

#if DEBUG
            MessageTracer.SetPlayer(player);
#endif
            this.IsDownloading = true;

            bool needSmoothStreamingXAP = false;
            bool needTimedTextLibraryXAP = false;

            // if any items are smooth streaming items -- init support for that
            foreach (PlaylistItem playlistItem in Items)
            {
                if (!needSmoothStreamingXAP)
                {
                    if (playlistItem.IsAdaptiveStreaming)
                    {
                        needSmoothStreamingXAP = true;
                        player.LoadSmoothStreamingModuleOnlineToGoOffline();
                    }
                }
                if (!needTimedTextLibraryXAP)
                {
                    if (playlistItem.CaptionSources != null)
                    {
                        if (playlistItem.CaptionSources.Count > 0)
                        {
                            needTimedTextLibraryXAP = true;
                        }
                    }
                }
                if (needSmoothStreamingXAP && needTimedTextLibraryXAP)
                {
                    break;
                }
            }

            this.m_downloadItemsQueue = new Queue<QueueItem>();

            if (needSmoothStreamingXAP)
            {
                Uri xapSmoothStreaming = new Uri(MediaPlayer.SmoothStreamingXAPName, UriKind.Relative);
                if (this.QueueFileItemForDownloadAndCheckForCancel(xapSmoothStreaming)) return;
            }

            if (needTimedTextLibraryXAP)
            {
                Uri xapTimedTextLibrary = new Uri(CaptionsManager.TimedTextXAPName, UriKind.Relative);
                if (this.QueueFileItemForDownloadAndCheckForCancel(xapTimedTextLibrary)) return;
            }

            ///
            /// queue media items for download to isolated storage
            ///
            foreach (PlaylistItem playlistItem in Items)
            {
                if (this.QueueFileItemForDownloadAndCheckForCancel(playlistItem.MediaSource)) return;
                if (this.QueueFileItemForDownloadAndCheckForCancel(playlistItem.ThumbSource)) return;

                foreach (ChapterItem chapterItem in playlistItem.Chapters)
                {
                    if (this.QueueFileItemForDownloadAndCheckForCancel(chapterItem.ThumbSource)) return;
                }
                if (playlistItem.IsAdaptiveStreaming)
                {
                    QueueItem processManifestItem = new QueueItem(QueueItemTask.processManifest, playlistItem);
                    if (this.EnqueueItemAndCheckForCancel(processManifestItem)) return;

                }
                if (playlistItem.CaptionSources != null && !playlistItem.IsAdaptiveStreaming)  // DFXP data for Smooth Streaming is loaded from chunks / ISMT
                {
                    foreach (var captionSource in playlistItem.CaptionSources)
                    {
                        if (this.QueueFileItemForDownloadAndCheckForCancel(captionSource.CaptionFileSource)) return;
                    }
                }
            }

            ///
            /// copy media items to isolated storage
            ///
            Thread thread = new Thread(DownloadThreadProc);
            thread.Start();
        }

        #region OfflineSerializationSupport
        /// <summary>
        /// top level XML node
        /// </summary>
        private const string s_PlaylistIsoName = "Playlist.xml";
        /// <summary>
        /// persist playlist in offline storage
        /// </summary>
        public void WriteToIsoStorage()
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                XmlWriterSettings xmlws = new XmlWriterSettings();
                xmlws.Indent = true;
                using (IsolatedStorageFileStream isoStream = isoStore.CreateFile(s_PlaylistIsoName))
                using (XmlWriter writer = XmlWriter.Create(isoStream, xmlws))
                {
                    this.Serialize(writer);
                }
            }
        }
        /// <summary>
        /// load this playlist from the offline storage
        /// </summary>
        /// <returns></returns>
        public static Playlist ReadFromIsoStorage()
        {
           XmlReaderSettings xmlrs = new XmlReaderSettings();
           xmlrs.IgnoreComments = true;
           xmlrs.IgnoreWhitespace = true;
           using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
           {
               using (IsolatedStorageFileStream isoStream = isoStore.OpenFile(s_PlaylistIsoName, FileMode.Open))
               using (XmlReader reader = XmlReader.Create(isoStream, xmlrs))
               {
                   Playlist playlist = new Playlist();
                   playlist.Deserialize(reader);
                   return playlist;
               }
           }
        }
        #endregion
    }
}
