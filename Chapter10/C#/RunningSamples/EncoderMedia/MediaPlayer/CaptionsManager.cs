// <copyright file="CaptionsManager.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>Implements the CaptionsManager class</summary>
// <author>Microsoft Expression Encoder Team</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using TimedTextInterface;
using Microsoft.Expression.Encoder.PlugInMssCtrl;

namespace ExpressionMediaPlayer
{
    /// <summary>
    /// represent a caption option, ie. "spanish subtitles"
    /// </summary>
    public class CaptionOption
    {
        public string Title { get; set; }
        public string LanguageIdTwoLetterIso { get; set; }
        public string Type { get; set; }
    };

    internal class CaptionsManager : DFXPDataReceiver 
    {
        /// <summary>
        /// player that owns this caption manager
        /// </summary>
        private ExpressionMediaPlayer.MediaPlayer m_mediaPlayer;
        /// <summary>
        /// The area to display the closed captions in.
        /// </summary>
        private Panel m_captionsArea;
        /// <summary>
        /// where to contain captions
        /// </summary>
        private Canvas m_canvasCaptions;
        /// <summary>
        /// cache of captions
        /// </summary>
        private ITimedTextModel m_timedText = null;
        /// <summary>
        /// available caption options (type/language)
        /// </summary>
        private Dictionary<int, Dictionary<string,CaptionOption> > captionOptionsByPlaylistItem = new Dictionary<int, Dictionary<string,CaptionOption> >();

        internal string CurrentISOTwoLetterLanguageName { get; set; }
        internal int CurrentPlaylistItemIndex { get; set; }

        /// <summary>
        /// caption option currently selected
        /// </summary>
        internal CaptionOption CaptionOptionForPlaylistItemAndLanguage(int playListItemIndex, string isoTwoLetterLanguageName)
        {
            if (this.captionOptionsByPlaylistItem.ContainsKey(playListItemIndex))
            {
                var captionOptions = this.captionOptionsByPlaylistItem[playListItemIndex];
                if ( captionOptions.ContainsKey(isoTwoLetterLanguageName))
                {
                    var captionOption = captionOptions[isoTwoLetterLanguageName];
                    return captionOption;
                }
            }
            return null;
        }


        /// <summary>
        /// Creates an instance of the CaptionsManager class
        /// </summary>
        /// <param name="player">player that owns these captions</param>
        /// <param name="closedCaptionCanvasIn">where to write captions</param>
        internal CaptionsManager(MediaPlayer player, Panel captionsArea)
        {
            this.m_mediaPlayer = player;
            this.m_captionsArea = captionsArea;
            this.CurrentISOTwoLetterLanguageName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            this.CurrentPlaylistItemIndex = -1;
            this.BuildCaptionsListForPlaylist(player.Playlist);
        }
        /// <summary>
        /// update available caption options from this playlist
        /// </summary>
        /// <param name="playlist"></param>
        internal void BuildCaptionsListForPlaylist(Playlist playlist)
        {
            int captionCount = 0;
            this.captionOptionsByPlaylistItem.Clear();
            for (int iItem = 0; iItem < this.m_mediaPlayer.Playlist.Items.Count; iItem++)
            {
                PlaylistItem playlistItem = this.m_mediaPlayer.Playlist.Items[iItem];
                var captionOptionsForThisPlayListItem = BuildCaptionOptionsForPlaylistItem(playlistItem);
                if (captionOptionsForThisPlayListItem != null)
                {
                    captionCount++;
                    captionOptionsByPlaylistItem.Add(iItem, captionOptionsForThisPlayListItem);
                }
            }

            if (captionCount > 0)
            {
                // DFXP captions are used -- load support for them
                LoadTimedTextSupport(TimeSpan.FromMilliseconds(0), null);
            }
        }

        private static Dictionary<string,CaptionOption> BuildCaptionOptionsForPlaylistItem(PlaylistItem playlistItem)
        {
            Dictionary<string,CaptionOption> captionOptions = null;
            foreach (CaptionSource captionSource in playlistItem.CaptionSources)
            {
                string strFormat = ExpressionMediaPlayer.Resources.captionsCaptions;
                switch (captionSource.Type.ToUpperInvariant())
                {
                    case "SUBTITLES":
                        strFormat = ExpressionMediaPlayer.Resources.captionsSubtitles;
                        break;
                    case "DESCRIPTIVE":
                        strFormat = ExpressionMediaPlayer.Resources.captionsDescriptive;
                        break;
                }

                CaptionOption option = new CaptionOption();
                option.LanguageIdTwoLetterIso = captionSource.ISOTwoLetterLanguageName;
                Debug.WriteLine("captionSource.Language=" + captionSource.Language);
                option.Title = String.Format(CultureInfo.CurrentUICulture, strFormat, captionSource.Language);
                option.Type = captionSource.Type;
                if (captionOptions == null)
                {
                    captionOptions = new Dictionary<string,CaptionOption>();
                }
                if (captionOptions.ContainsKey(option.LanguageIdTwoLetterIso))
                {
                    Debug.WriteLine("Duplicated language captions! " + option.LanguageIdTwoLetterIso);
                }
                else
                {
                    captionOptions.Add(option.LanguageIdTwoLetterIso, option);
                }
            }
            return captionOptions;
        }
        /// <summary>
        /// make sure the caption overlay panel is present
        /// </summary>
        internal void EnsureCaptionPanel()
        {
            if ((this.m_mediaPlayer != null) && (this.m_mediaPlayer.CurrentMediaElement != null))
            {
                if ((this.m_captionsArea != null) && (m_canvasCaptions == null))
                {
                    // the closed caption canvas sits on top of and mirrors the media element
                    m_canvasCaptions = new Canvas();
                    m_canvasCaptions.IsHitTestVisible = false;    
#if DEBUG_EXTRA_CAPTIONS_VISIBLE
                    m_canvasCaptions.Background = new SolidColorBrush(Color.FromArgb(85,85,0,0));
                    m_canvasCaptions.Opacity = 1.0;
#endif
                    Binding captionsVisibilityBinding = new Binding("CaptionsVisibility");
                    captionsVisibilityBinding.Source = this.m_mediaPlayer;
                    m_canvasCaptions.SetBinding(Canvas.VisibilityProperty, captionsVisibilityBinding);

                    Binding mediaElementHeightBinding = new Binding("Height");
                    mediaElementHeightBinding.Source = this.m_captionsArea;
                    m_canvasCaptions.SetBinding(Canvas.HeightProperty, mediaElementHeightBinding);

                    Binding mediaElementWidthBinding = new Binding("Width");
                    mediaElementWidthBinding.Source = this.m_captionsArea;
                    m_canvasCaptions.SetBinding(Canvas.WidthProperty, mediaElementWidthBinding);

                    this.m_captionsArea.Children.Add(m_canvasCaptions);

               }

                if (this.m_timedText != null)
                {
                    if (this.m_timedText.MediaElement != this.m_mediaPlayer.CurrentMediaElement)
                    {
                        this.m_timedText.MediaElement = this.m_mediaPlayer.CurrentMediaElement;
                    }
                    if (this.m_timedText.Destination != m_canvasCaptions)
                    {
                        this.m_timedText.Destination = m_canvasCaptions;
                    }
                }
            }
        }

        internal void RemoveCaptionPanel()
        {
            if (this.m_mediaPlayer.CurrentMediaElement != null)
            {
                if ((this.m_captionsArea != null) && (this.m_canvasCaptions != null))
                {
                    this.m_captionsArea.Children.Remove(this.m_canvasCaptions);
                    this.m_canvasCaptions = null;
                    this.m_timedText.Destination = null;
                }
            }
        }

        /// <summary>
        /// Available caption options (type/language) as displayed to the user
        /// </summary>
        internal Dictionary<string, CaptionOption> CaptionOptionsForPlayListItem(int playlistIndex)
        {
            if (this.captionOptionsByPlaylistItem.ContainsKey(playlistIndex))
            {
                return this.captionOptionsByPlaylistItem[playlistIndex];
            }
            return null;
        }

#if DEBUG_EXTRA_CAPTIONS_PERF
        private DateTime dbg_downloadStart;
#endif

        /// <summary>
        /// download the captions data
        /// </summary>
        /// <param name="isAdaptive">Whether the content is smooth streaming or conventional</param>
        /// <param name="captionSource">this captions source data</param>
        internal void DownloadCaptions(bool isAdaptive, CaptionSource captionSource)
        {
            // Remove old caption data if present
            if (this.m_timedText != null)
            {
                this.m_timedText.ClearCaptionArea();
                this.m_timedText.ClearEventData();
            }

            // If caption data passed in request it
            if (captionSource != null)
            {                
                if (captionSource.CaptionFileSource != null)
                {
#if DEBUG_EXTRA_CAPTIONS_PERF
                    Debug.WriteLine("Downloading captions data file URL:" + captionSource.CaptionFileSource.ToString());
                    dbg_downloadStart = DateTime.Now;
#endif
                    Debug.WriteLine("DownloadCaptions: isAdaptive=" + isAdaptive.ToString() + " lang=" + captionSource.ISOTwoLetterLanguageName + " file=" + captionSource.CaptionFileSource.ToString());
                    if (isAdaptive)
                    {
                        if (this.m_mediaPlayer.CurrentMediaElement.DFXPDataReceiver != this )
                        {
                            this.m_mediaPlayer.CurrentMediaElement.DFXPDataReceiver = this;
                        }
                        string isoThree = LanguageAlias.IsoTwoLetterToIsoThreeLetter(captionSource.ISOTwoLetterLanguageName);
                        this.m_mediaPlayer.CurrentMediaElement.ActivateTextStreamForLanguage(isoThree);
                    }
                    else
                    {
                        if (MediaPlayer.IsOffline)
                        {
                            Debug.WriteLine("DownloadCaptions: -- Snarfing from IsolatedStorage");
                            IsoUri offlineIsoUri = MediaPlayer.MakeOfflineIsoUri(captionSource.CaptionFileSource);
                            if (offlineIsoUri.IsoFileExists)
                            {
                                this.m_mediaPlayer.EnableClosedCaptionButton(false);
                                this.AddDFXPDataFromUIThread(TimeSpan.FromMilliseconds(0), offlineIsoUri.Stream);
                            }
                        }
                        else
                        {
                            Debug.WriteLine("DownloadCaptions: -- downloading via WebClient");
                            // While processing a new set of captions -- disable the CC button
                            this.m_mediaPlayer.EnableClosedCaptionButton(false);
                            WebClient webClient = new WebClient();
                            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(DownloadClosedCaptionsCompleted);
                            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
                            webClient.OpenReadAsync(captionSource.CaptionFileSource);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// show downloading text for captions
        /// </summary>
        void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            m_mediaPlayer.SetCaptionText(String.Format(ExpressionMediaPlayer.Resources.captionsDownloading,e.ProgressPercentage));
        }

        /// <summary>
        /// captions have completed download. remember captions text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void DownloadClosedCaptionsCompleted(object sender, OpenReadCompletedEventArgs eventArgs) 
        {
            m_mediaPlayer.SetCaptionText(string.Empty);
            if (eventArgs.Cancelled)
            {
                Debug.WriteLine("canceled download of captions data!");
            }
            else if (eventArgs.Error != null)
            {
                Debug.WriteLine("Error occured downloading captions data error=" + eventArgs.Error.Message);
                m_mediaPlayer.ShowErrorMessage(eventArgs.Error.Message);
                m_mediaPlayer.EnableClosedCaptionButton(true);
            }
            else
            {
                if (eventArgs.Result == null)
                {
                    Debug.WriteLine("null captions file result!");
                    m_mediaPlayer.EnableClosedCaptionButton(true);
                }
                else if (eventArgs.Result.Length < 1)
                {
                    Debug.WriteLine("Empty captions file result!");
                    m_mediaPlayer.EnableClosedCaptionButton(true);
                }
                else
                {
#if DEBUG_EXTRA_CAPTIONS_PERF
                    TimeSpan diff = DateTime.Now - dbg_downloadStart;
                    Debug.WriteLine("Downloading captions data file complete: diff(MS)=" + diff.TotalMilliseconds.ToString());
#endif
                    this.AddDFXPDataFromUIThread(TimeSpan.FromMilliseconds(0), eventArgs.Result);
                }
            }
        }

        /// <summary>
        /// Thread for parsing DFXP data
        /// </summary>
        Thread threadParseTimedText = null;

        /// <summary>
        /// Worker thread method for creating the marker and ad data from the manifest
        /// </summary>
        private void ParseTimedTextWorkerThread(Object data)
        {
            if (this.m_timedText != null)
            {
#if DEBUG_EXTRA_CAPTIONS_PERF
            DateTime startParse = DateTime.Now;
#endif
                Stream dfxpData = data as Stream;
                AddDFXPData(TimeSpan.FromMilliseconds(0), dfxpData);
                this.m_mediaPlayer.EnableClosedCaptionButton(true);
                return;
            }
            Debug.Assert(false, "ParseTimedTextWorkerThread called with unexpected data");
        }

        /// <summary>
        /// Process DFXP Data while running on a background thread
        /// </summary>
        /// <param name="dfxpData"></param>
        public void AddDFXPData(TimeSpan timeStamp, Stream dfxpData)
        {
            Debug.Assert(dfxpData != null);
            if (this.m_timedText == null)
            {
                LoadTimedTextSupport(timeStamp, dfxpData);
            }
            else
            {
                var eventData = this.m_timedText.ParseData(timeStamp, dfxpData);                
#if DEBUG_EXTRA_CAPTIONS_PERF
            TimeSpan parseDiff = DateTime.Now - startParse;
            Debug.WriteLine("ParseTime: diff(MS)=" + parseDiff.TotalMilliseconds.ToString());
#endif
                if ( eventData != null )
                {
                    if (!string.IsNullOrEmpty(eventData.ErrorInfo))
                    {
                        this.m_mediaPlayer.SetNextTickErrorMessage(eventData.ErrorInfo);
                    }
                    else
                    {
                        this.m_mediaPlayer.Dispatcher.BeginInvoke(() => this.m_timedText.AttachEvents(eventData));
                    }
                }
                Debug.WriteLine("ParseTimedTextWorkerThread() completed!");
            }
        }

        /// <summary>
        /// DFXP source file (as text string) for captions. 
        /// </summary>
        private void AddDFXPDataFromUIThread(TimeSpan timeStamp, Stream dfxpData)
        {
            if (this.m_timedText != null)
            {
                if (this.threadParseTimedText != null && this.threadParseTimedText.IsAlive)
                {
                    Debug.Assert(false, "Attempting to start new parse thread while prior one is still running");
                    return;
                }

                Thread worker = new Thread(ParseTimedTextWorkerThread);
                worker.Start(dfxpData);
                this.threadParseTimedText = worker;
            }
            else
            {
                LoadTimedTextSupport(timeStamp, dfxpData);
            }
        }

        /// <summary>
        /// Clear out cache of DFXP captions -- needed when switching playlist items or switching languages
        /// </summary>
        internal void ClearDFXPEvents()
        {
            if (this.m_timedText != null)
            {
                this.m_timedText.ClearMarkers();
            }
        }

        /// <summary>
        /// Helper routine for the media player to call after a window resize to recreate the captions using the current window size.
        /// </summary>
        internal void RefreshCaptions()
        {
            if (this.m_timedText!=null)
            {
                this.m_timedText.RefreshCaptionArea();
            }
        }

        /// <summary>
        /// Helper routine to clear the DFXP caption area
        /// </summary>
        internal void ClearCaptions()
        {
            if ((this.m_timedText == null) || (this.m_canvasCaptions == null))
            {
                return;
            }
            this.m_timedText.ClearCaptionArea();
        }

        internal const string TimedTextXAPName = "TimedTextLibrary.XAP";
        const string TimedTextAssemblyName = "TimedTextLibrary.DLL";
        const string TimedTextObjectName = "TimedTextLibrary.TimedTextModel";
        private TimeSpan m_timeStampToSetAfterLoad;
        private Stream m_streamToSetAfterLoad;
        PlugInLoader m_timedTextPlugin;
        /// <summary>
        /// Loads the SmoothStreaming support module
        /// </summary>
        private void LoadTimedTextSupport(TimeSpan timeStamp, Stream streamToSetAfterLoad)
        {
            Debug.WriteLine("LoadTimedTextSupport!");
            if ( this.m_timedTextPlugin == null )
            {
                Debug.Assert((null == this.m_streamToSetAfterLoad), "Expected defered stream to be null!");
                this.m_timeStampToSetAfterLoad = timeStamp;
                this.m_streamToSetAfterLoad = streamToSetAfterLoad;
                this.m_timedTextPlugin = new PlugInLoader();
                this.m_timedTextPlugin.PlugInLoadCompleted += new EventHandler<XAPReadCompletedEventArgs>(OnTimedTextLoaded);
                Uri xap = new Uri(TimedTextXAPName, UriKind.Relative);
                this.m_timedTextPlugin.Load(xap, TimedTextAssemblyName);
            }
        }

        /// <summary>
        /// Callback that is called once the TimedText support module is downloaded form the host
        /// </summary>
        private void OnTimedTextLoaded(object sender, XAPReadCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Debug.WriteLine("OnTimedTextLoaded -- Cancelled");
                return;
            }
            else if (e.Error != null)
            {
                Debug.WriteLine("OnTimedTextLoaded -- Failed:" + e.Error.Message);
                if (e.Error.InnerException != null)
                {
                    Debug.WriteLine("OnTimedTextLoaded -> " + e.Error.InnerException.Message);
                }
                return;
            }
            this.m_timedText = (this.m_timedTextPlugin.CreateObject(TimedTextObjectName)) as ITimedTextModel;
            Debug.Assert(this.m_timedText != null,"Failed to created TimedText support");
            if (null != this.m_streamToSetAfterLoad)
            {
                this.AddDFXPDataFromUIThread(this.m_timeStampToSetAfterLoad, this.m_streamToSetAfterLoad);
                this.m_streamToSetAfterLoad = null;
            }
        }        
    }
}

