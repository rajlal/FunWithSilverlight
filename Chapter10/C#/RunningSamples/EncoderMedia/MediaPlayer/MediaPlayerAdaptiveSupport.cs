// <copyright file="MediaPlayerAdaptiveSupport.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>Implements the MediaPlayer class</summary>
// <author>Microsoft Expression Encoder Team</author>
namespace ExpressionMediaPlayer
{
    using Microsoft.Expression.Encoder.PlugInMssCtrl;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Windows;
    using System.Windows.Browser;
    using System.Windows.Controls;
    using System.Windows.Media;

    public partial class MediaPlayer : Control
    {
        /// <summary>
        /// The plugin in loader helper object that loads the Smoooth streamign support module and processes the assembly information
        /// </summary>
        private PlugInLoader m_smoothStreamingSourcePlugIn;
        /// <summary>
        /// The URI of the manifest for the currently playing item 
        /// </summary>
        private Uri m_smoothStreamingItemManifestUri;
        /// <summary>
        /// The offline bitrate in kbps of the currently playing item 
        /// </summary>
        private long m_smoothStreamingItemOfflineVideoBitrateInKbps;

        /// <summary>
        /// Filename of the smooth streaming support module
        /// </summary>
        internal static string SmoothStreamingXAPName
        {
            get
            {
                return "SmoothStreaming.xap";
            }
        }

        /// <summary>
        /// Assembly name of the smooth streaming support module
        /// </summary>
        internal static string SmoothStreamingAssemblyName
        {
            get
            {
                return "SmoothStreaming.dll";
            }
        }

        /// <summary>
        /// class name of the smooth streaming support object for online playback
        /// </summary>
        internal static string SmoothStreamingOnlineObjectName
        {
            get
            {
                return "SmoothStreaming.SmoothStreamingMediaElementShim";
            }
        }

        /// <summary>
        /// class name of the smooth streaming support object for online playback
        /// </summary>
        internal static string SmoothStreamingOfflineObjectName
        {
            get
            {
                return "SmoothStreaming.SmoothStreamingMediaElementShimOffline";
            }
        }

        /// <summary>
        /// Initialize smooth streaming for taking content offline
        /// </summary>
        private void LoadSmoothStreamingModule()
        {

        }

        /// <summary>
        /// Initialize smooth streaming for taking content offline
        /// </summary>
        internal void InitSmoothStreamingToGoOffline()
        {

            this.LoadSmoothStreamingModuleOnlineToGoOffline();
        }


        /// <summary>
        /// Initialize smooth streaming playback for a manifest item
        /// </summary>
        internal void InitSmoothStreaming(PlaylistItem playlistItem)
        {
            this.m_smoothStreamingItemManifestUri = playlistItem.MediaSource;
            this.m_smoothStreamingItemOfflineVideoBitrateInKbps = playlistItem.OfflineVideoBitrateInKbps;

            if (this.m_mediaElementForSmoothStreamingContent == null)
            {
                this.LoadSmoothStreamingModuleForPlayback();
            }
            if (m_smoothStreamingSourcePlugIn != null)
            {
                CreateSmoothStreamingObject();
                StartSmoothStreamingPlayback();
            }
        }

        /// <summary>
        /// Loads the SmoothStreaming support module
        /// </summary>
        private void LoadSmoothStreamingModuleOnlineCore(EventHandler<XAPReadCompletedEventArgs> onPlugInLoadCompleted)
        {
            Debug.WriteLine("InitSmoothStreamingCore!");
            this.m_smoothStreamingSourcePlugIn = new PlugInLoader();
            this.m_smoothStreamingSourcePlugIn.PlugInLoadCompleted += new EventHandler<XAPReadCompletedEventArgs>(onPlugInLoadCompleted);
            Uri xap = new Uri(SmoothStreamingXAPName, UriKind.Relative);
            this.m_smoothStreamingSourcePlugIn.Load(xap, SmoothStreamingAssemblyName);
        }

        /// <summary>
        /// Loads the SmoothStreaming support module from the host when running online for the purpose of taking content offline
        /// </summary>
        internal void LoadSmoothStreamingModuleOnlineToGoOffline()
        {
            Debug.WriteLine("LoadSmoothStreamingModuleOnlineToGoOffline!");
            LoadSmoothStreamingModuleOnlineCore(new EventHandler<XAPReadCompletedEventArgs>(OnSmoothStreamingLoadedToGoOffine));
        }

        /// <summary>
        /// Callback that is called once the SmoothStreaming support module is downloaded form the host when running online
        /// </summary>
        private void OnSmoothStreamingLoadedToGoOffine(object sender, XAPReadCompletedEventArgs e)
        {
            if (OnSmoothStreamingLoadedErrorCheck(e))
            {
                CreateSmoothStreamingObject();
                if (this.m_mediaElementForSmoothStreamingContent != null)
                {
                    IPlugInMssOfflineSupport offlineSupport = this.SmoothStreamingOfflineSupport;
                    Debug.Assert(offlineSupport != null, "OnSmoothStreamingLoadedToGoOffine object not ready!");
                }
                ShowSmoothStreamingMediaElement();
            }
        }

        /// <summary>
        /// Loads the SmoothStreaming support module for playback
        /// </summary>
        private void LoadSmoothStreamingModuleForPlayback()
        {
            Debug.WriteLine("start LoadSmoothStreamingModuleForPlayback");
            LoadSmoothStreamingModuleOnlineCore(new EventHandler<XAPReadCompletedEventArgs>(OnSmoothStreamingLoadedForPlayback));
        }

        private static bool OnSmoothStreamingLoadedErrorCheck(XAPReadCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Debug.WriteLine("SmoothStreamingSupportLoadCompleted -- Cancelled");
                return false;
            }
            else if (e.Error != null)
            {
                string errorMessage = e.Error.Message;

                if (e.Error.InnerException != null)
                {
                    if (0 != errorMessage.CompareTo(e.Error.InnerException.Message))
                    {
                        errorMessage += (" -> " + e.Error.InnerException.Message);
                    }
                }
                Debug.WriteLine("SmoothStreamingSupportLoadCompleted -- Failed:" + errorMessage);
                StaticShowErrorMessage(errorMessage);
                return false;
            }
            return true;
        }


        /// <summary>
        /// Callback that is called once the SmoothStreaming support module is downloaded form the host when running online
        /// </summary>
        private void OnSmoothStreamingLoadedForPlayback(object sender, XAPReadCompletedEventArgs e)
        {
            if (OnSmoothStreamingLoadedErrorCheck(e))
            {
                bool startedOK = false;
                try
                {
                    CreateSmoothStreamingObject();
                    StartSmoothStreamingPlayback();
                    startedOK = true;
                }
                catch (IsolatedStorageException ise)
                {
                    this.ShowErrorMessage(ise.ToString());
                }
                finally
                {
                    if (!startedOK)
                    {
                        this.ButtonClickStopLogic();
                    }
                }
            }
        }

        /// <summary>
        /// Create a smooth streaming object (either online or offline)
        /// </summary>
        private void CreateSmoothStreamingObject()
        {
            try
            {
                if (this.m_mediaElementForSmoothStreamingContent == null)
                {
                    Object smoothStreamingObject = null;
                    if (MediaPlayer.IsOffline)
                    {
                        smoothStreamingObject = this.m_smoothStreamingSourcePlugIn.CreateObject(MediaPlayer.SmoothStreamingOfflineObjectName);
                        Debug.Assert(smoothStreamingObject != null, "failed to create: " + MediaPlayer.SmoothStreamingOfflineObjectName);
                    }
                    else
                    {
                        smoothStreamingObject = this.m_smoothStreamingSourcePlugIn.CreateObject(MediaPlayer.SmoothStreamingOnlineObjectName);
                        Debug.Assert(smoothStreamingObject != null, "failed to create: " + MediaPlayer.SmoothStreamingOnlineObjectName);
                    }

                    this.m_mediaElementForSmoothStreamingContent = smoothStreamingObject as MediaElementShim;
                }
            }
            catch (PlugInLoaderFailedException fe)
            {
                Debug.WriteLine("SmoothStreamingSupportLoadCompleted: Create Failed:" + fe.Message);
            }
        }

        /// <summary>
        /// make the SSME visible and hooked up to events
        /// </summary>
        private void ShowSmoothStreamingMediaElement()
        {
            if (this.m_mediaElementForConventionalContent != null)
            {
                switch (this.m_mediaElementForConventionalContent.CurrentState)
                {
                    case MediaElementState.Stopped:
                    case MediaElementState.Closed:
                        break;
                    default:
                        this.m_mediaElementForConventionalContent.Stop();
                        break;
                }
                this.m_mediaElementForConventionalContent.Source = null;
                this.m_mediaElementForConventionalContent.Visibility = Visibility.Collapsed;
            }
            if (this.m_mediaElementForSmoothStreamingContent != null)
            {
                this.m_mediaElementForSmoothStreamingContent.Visibility = Visibility.Visible;
                if (this.m_mediaElement != m_mediaElementForSmoothStreamingContent)
                {
                    if (this.m_mediaElement != null)
                    {
                        this.UnhookMediaElementEvents();
                    }
                    if (this.m_mediaElementGrid != null)
                    {
                        this.m_mediaElementGrid.Children.Clear();
                        UIElement ue = this.m_mediaElementForSmoothStreamingContent.UIElement;
                        this.m_mediaElementGrid.Children.Add(ue);
                    }
                    this.m_mediaElement = this.m_mediaElementForSmoothStreamingContent;
                    this.HookMediaElementEvents();
                    this.ApplyPropertiesToMediaElement();
                }
            }
        }

        /// <summary>
        /// Actually start playback once the support module is loaded and the smooth streaming object is created.
        /// </summary>
        private void StartSmoothStreamingPlayback()
        {
#if DEBUG
            Debug.WriteLine("StartSmoothStreamingPlayback");
#endif
            if (this.m_mediaElementForSmoothStreamingContent != null)
            {
                if (this.m_smoothStreamingItemManifestUri != null)
                {
                    this.ShowSmoothStreamingMediaElement();

                    if (MediaPlayer.IsOffline)
                    {
                        IsoUri offlineIsoUri = MediaPlayer.MakeOfflineIsoUri(this.m_smoothStreamingItemManifestUri);
                        if (offlineIsoUri.IsoFileExists)
                        {
                            // Force heuristics to play the bitrate actually present in offline storage
                            if ((this.m_smoothStreamingItemOfflineVideoBitrateInKbps > 0) && (this.SmoothStreamingOfflineSupport != null))
                            {
                                // Force heuristics to play the bitrate actually present in offline storage
                                this.SmoothStreamingOfflineSupport.SetOfflinePlaybackBitrateInKbps(MediaStreamType.Video, this.m_smoothStreamingItemOfflineVideoBitrateInKbps);
                            }
                            this.SmoothStreamingMediaElementShim.Source = offlineIsoUri;
                        }
                        else
                        {
                            string err = "Manifest missing from isolated storage!" + this.m_smoothStreamingItemManifestUri.OriginalString;
                            this.ShowErrorMessage(err);
                            throw new IsolatedStorageException(err);
                        }
                    }
                    else
                    {
                        // Construct the absolute uri
                        Uri absoluteUri;
                        if (this.m_smoothStreamingItemManifestUri.IsAbsoluteUri || HtmlPage.Document.DocumentUri == null)
                        {
                            absoluteUri = this.m_smoothStreamingItemManifestUri;
                        }
                        else
                        {   
                            absoluteUri = new Uri(HtmlPage.Document.DocumentUri, this.m_smoothStreamingItemManifestUri);
                        }
                        this.m_mediaElementForSmoothStreamingContent.Source = absoluteUri;
                    }

                    // Display stats graph if this template has a slot for it.
                    if (this.m_gridPlugIn != null)
                    {
                        IPlugInMssStatisticsGraph plugInGraph = this.m_mediaElementForSmoothStreamingContent as IPlugInMssStatisticsGraph;
                        if (plugInGraph != null)
                        {
                            this.m_gridPlugIn.Children.Clear(); // Remove all prior plug ins
                            this.m_gridPlugIn.Children.Add(plugInGraph.StatisticsGraph);
                        }
                    }
                }
            }
        }
    }
}

