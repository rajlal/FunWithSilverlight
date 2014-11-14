// <copyright file="MediaPlayer.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>Implements the MediaPlayer class</summary>
// <author>Microsoft Expression Encoder Team</author>
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net.NetworkInformation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using Microsoft.Expression.Encoder.PlugInMssCtrl;
using System.Collections.Generic;

namespace ExpressionMediaPlayer
{
    /// <summary>
    /// This class represents the base class for a MediaPlayer control.
    /// </summary>
    [TemplatePart(Name=MediaPlayer.StretchBox, Type=typeof(FrameworkElement))]
    [TemplatePart(Name=MediaPlayer.VideoWindow, Type=typeof(Panel))]
    [TemplatePart(Name=MediaPlayer.MediaElement, Type = typeof(MediaElement))]
    [TemplatePart(Name=MediaPlayer.MediaElementGrid, Type = typeof(Grid))]
    [TemplatePart(Name=MediaPlayer.StartButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name=MediaPlayer.PlayPauseButton, Type = typeof(ToggleButton))]
    [TemplatePart(Name=MediaPlayer.PreviousButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name=MediaPlayer.NextButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name=MediaPlayer.StopButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name=MediaPlayer.FullScreenButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name=MediaPlayer.OfflineButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name=MediaPlayer.PopOutButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name=MediaPlayer.PlugInButton, Type = typeof(ToggleButton))]
    [TemplatePart(Name=MediaPlayer.MuteButton, Type = typeof(ToggleButton))]
    [TemplatePart(Name=MediaPlayer.ClosedCaptionButton, Type = typeof(ToggleButton))]
    [TemplatePart(Name=MediaPlayer.ClosedCaptionsOptionsMenu, Type = typeof(Popup))]
    [TemplatePart(Name=MediaPlayer.ClosedCaptionsOptionsList, Type = typeof(ListBox))]            
    [TemplatePart(Name=MediaPlayer.VolumeDownButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name=MediaPlayer.VolumeUpButton, Type = typeof(ButtonBase))]
    [TemplatePart(Name=MediaPlayer.PlaylistButton, Type = typeof(ToggleButton))]
    [TemplatePart(Name=MediaPlayer.ChapterButton, Type = typeof(ToggleButton))]
    [TemplatePart(Name=MediaPlayer.PositionSlider, Type = typeof(SensitiveSlider))]
    [TemplatePart(Name=MediaPlayer.VolumeSlider, Type = typeof(SensitiveSlider))]    
    [TemplatePart(Name=MediaPlayer.PlaylistSelector, Type = typeof(Selector))]
    [TemplatePart(Name=MediaPlayer.ChaptersSelector, Type = typeof(Selector))]
    [TemplatePart(Name=MediaPlayer.ErrorMessageElement, Type = typeof(FrameworkElement))]
    [TemplatePart(Name=MediaPlayer.ClosedCaptionArea, Type = typeof(Grid))]    
    [TemplatePart(Name=MediaPlayer.GridPlugIn, Type = typeof(Grid))]
    public partial class MediaPlayer : Control
    {
        #region DP definitions
        /// <summary>
        /// Using a DependencyProperty as the backing store for Playlist.  This enables animation, styling, binding, etc...        
        /// </summary>
        public static readonly DependencyProperty PlaylistProperty =
            DependencyProperty.Register("Playlist", typeof(Playlist), typeof(MediaPlayer), new PropertyMetadata(PlaylistChanged));

        /// <summary>
        /// Using a DependencyProperty as the backing store for PlaybackPosition.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PlaybackPositionProperty =
            DependencyProperty.Register("PlaybackPosition", typeof(Double), typeof(MediaPlayer), new PropertyMetadata(null));

        /// <summary>
        /// Using a DependencyProperty as the backing store for PlaybackPositionText.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PlaybackPositionTextProperty =
            DependencyProperty.Register("PlaybackPositionText", typeof(String), typeof(MediaPlayer), new PropertyMetadata(null));

        /// <summary>
        /// Using a DependencyProperty as the backing store for CpuText.
        /// </summary>
        public static readonly DependencyProperty CpuTextProperty =
            DependencyProperty.Register("CpuText", typeof(String), typeof(MediaPlayer), new PropertyMetadata(null));

        /// <summary>
        /// Using a DependencyProperty as the backing store for PlaybackDuration.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PlaybackDurationProperty =
            DependencyProperty.Register("PlaybackDuration", typeof(Double), typeof(MediaPlayer), new PropertyMetadata(null));

        /// <summary>
        /// Using a DependencyProperty as the backing store for PlaybackDurationText.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PlaybackDurationTextProperty =
            DependencyProperty.Register("PlaybackDurationText", typeof(String), typeof(MediaPlayer), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Using a DependencyProperty as the backing store for BufferingPercent.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty BufferingPercentProperty =
            DependencyProperty.Register("BufferingPercent", typeof(Double), typeof(MediaPlayer), new PropertyMetadata(0.0));

        /// <summary>
        /// Using a DependencyProperty as the backing store for PosterImageSource.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PosterImageSourceProperty =
            DependencyProperty.Register("PosterImageSource", typeof(string), typeof(MediaPlayer), new PropertyMetadata(null));

        /// <summary>
        /// Using a DependencyProperty as the backing store for PosterImageMaxWidth.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PosterImageMaxWidthProperty =
            DependencyProperty.Register("PosterImageMaxWidth", typeof(Double), typeof(MediaPlayer), new PropertyMetadata(Double.PositiveInfinity));

        /// <summary>
        /// Using a DependencyProperty as the backing store for PosterImageMaxHeight.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PosterImageMaxHeightProperty =
            DependencyProperty.Register("PosterImageMaxHeight", typeof(Double), typeof(MediaPlayer), new PropertyMetadata(Double.PositiveInfinity));

        /// <summary>
        /// Using a DependencyProperty as the backing store for BufferingControlVisibility  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty BufferingControlVisibilityProperty =
            DependencyProperty.Register("BufferingControlVisibility", typeof(Visibility), typeof(MediaPlayer), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Using a DependencyProperty as the backing store for OfflineDownloadProgressVisibility  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty OfflineDownloadProgressVisibilityProperty =
            DependencyProperty.Register("OfflineDownloadProgressVisibility", typeof(Visibility), typeof(MediaPlayer), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Using a DependencyProperty as the backing store for DownloadOffsetPercent.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty DownloadOffsetPercentProperty =
            DependencyProperty.Register("DownloadOffsetPercent", typeof(Double), typeof(MediaPlayer), new PropertyMetadata(0.0));

        /// <summary>
        /// Using a DependencyProperty as the backing store for DownloadPercent.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty DownloadPercentProperty =
            DependencyProperty.Register("DownloadPercent", typeof(Double), typeof(MediaPlayer), new PropertyMetadata(0.0));

        /// <summary>
        /// Using a DependencyProperty as the backing store for PlaybackPositionText.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty CaptionTextProperty =
            DependencyProperty.Register("CaptionText", typeof(String), typeof(MediaPlayer), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Using a DependencyProperty as the backing store for CaptionsEnabled.
        /// </summary>
        public static readonly DependencyProperty CaptionsVisibilityProperty =
            DependencyProperty.Register("CaptionsVisibility", typeof(Visibility), typeof(MediaPlayer), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Using a DependencyProperty as the backing store for CaptionOptions menu.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty CaptionOptionsProperty =
            DependencyProperty.Register("CaptionOptions", typeof(Dictionary<string, CaptionOption>), typeof(MediaPlayer), new PropertyMetadata(null));

        /// <summary>
        /// Using a DependencyProperty as the backing store for CaptionsButtonVisibilityProperty.  This may show/hide the closed caption button.
        /// </summary>
        public static readonly DependencyProperty CaptionsButtonVisibilityProperty =
            DependencyProperty.Register("CaptionsButtonVisibility", typeof(Visibility), typeof(MediaPlayer), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Using a DependencyProperty as the backing store for OfflineButtonVisibilityProperty.  This may show/hide the offline button.
        /// </summary>
        public static readonly DependencyProperty OfflineButtonVisibilityProperty =
            DependencyProperty.Register("OfflineButtonVisibility", typeof(Visibility), typeof(MediaPlayer), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Using a DependencyProperty as the backing store for OfflineButtonVisibilityProperty.  This will enable/disable the offline button.
        /// </summary>
        public static readonly DependencyProperty OfflineButtonEnabledProperty =
            DependencyProperty.Register("OfflineButtonEnabled", typeof(Boolean), typeof(MediaPlayer), new PropertyMetadata(true));

        /// <summary>
        /// Using a DependencyProperty as the backing store for PopOutButtonVisibilityProperty.  This may show/hide the popout button.
        /// </summary>
        public static readonly DependencyProperty PopOutButtonVisibilityProperty =
            DependencyProperty.Register("PopOutButtonVisibility", typeof(Visibility), typeof(MediaPlayer), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Using a DependencyProperty as the backing store for PlugInButtonVisibilityProperty.  This may show/hide the PlugIn button.
        /// </summary>
        public static readonly DependencyProperty PlugInButtonVisibilityProperty =
            DependencyProperty.Register("PlugInButtonVisibility", typeof(Visibility), typeof(MediaPlayer), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Using a DependencyProperty as the backing store for ChaptersButtonVisibilityProperty.  This may show/hide the popout button.
        /// </summary>
        public static readonly DependencyProperty ChaptersButtonVisibilityProperty =
            DependencyProperty.Register("ChaptersButtonVisibility", typeof(Visibility), typeof(MediaPlayer), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Using a DependencyProperty as the backing store for PlaylistButtonVisibilityProperty.  This may show/hide the popout button.
        /// </summary>
        public static readonly DependencyProperty PlaylistButtonVisibilityProperty =
            DependencyProperty.Register("PlaylistButtonVisibility", typeof(Visibility), typeof(MediaPlayer), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Using a DependencyProperty as the backing store for CpuVisibilityProperty.  
        /// </summary>
        public static readonly DependencyProperty CpuVisibilityProperty =
            DependencyProperty.Register("CpuVisibility", typeof(Visibility), typeof(MediaPlayer), new PropertyMetadata(Visibility.Collapsed));
        
        /// <summary>
        /// Using a DependencyProperty as the backing store for UserBackgroundColor.
        /// </summary>
        public static readonly DependencyProperty UserBackgroundColorProperty =
            DependencyProperty.Register("UserBackgroundColor", typeof(Brush), typeof(MediaPlayer), new PropertyMetadata(null));

        /// <summary>
        /// Using a DependencyProperty as the backing store for HideOnClick.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty HideOnClickProperty =
            DependencyProperty.RegisterAttached("HideOnClick", typeof(Boolean), typeof(MediaPlayer), new PropertyMetadata(false));

        /// <summary>
        /// Using a DependencyProperty as the backing store for FrameStepVisibility.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty FrameStepVisibilityProperty =
            DependencyProperty.Register("FrameStepVisibility", typeof(Visibility), typeof(MediaPlayer), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Using a DependencyProperty as the backing store for ErrorMessageVisibilityProperty.  This may show/hide the error message
        /// </summary>
        public static readonly DependencyProperty ErrorMessageVisibilityProperty =
            DependencyProperty.Register("ErrorMessageVisibility", typeof(Visibility), typeof(MediaPlayer), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Using a DependencyProperty as the backing store for ErrorMessage.  
        /// </summary>
        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register("ErrorMessage", typeof(String), typeof(MediaPlayer), new PropertyMetadata(String.Empty));

        /// <summary>
        /// Using a DependencyProperty as the backing store for ButtonPreviousIsEnabled. 
        /// </summary>
        public static readonly DependencyProperty ButtonPreviousIsEnabledProperty =
            DependencyProperty.Register("ButtonPreviousIsEnabled", typeof(Boolean), typeof(MediaPlayer), new PropertyMetadata(false));

        /// <summary>
        /// Using a DependencyProperty as the backing store for ButtonNextIsEnabled. 
        /// </summary>
        public static readonly DependencyProperty ButtonNextIsEnabledProperty =
            DependencyProperty.Register("ButtonNextIsEnabled", typeof(Boolean), typeof(MediaPlayer), new PropertyMetadata(false));

        #endregion

        /// <summary>
        /// String for the stretch box template element.
        /// </summary>
        protected const string StretchBox = "stretchBox";

        /// <summary>
        /// String for the video window template element.
        /// </summary>
        protected const string VideoWindow = "videoWindow";

        /// <summary>
        /// String for the media element template item.
        /// </summary>
        protected const string MediaElement = "mediaElement";

        /// <summary>
        /// String for the media element grid template item.
        /// </summary>
        protected const string MediaElementGrid = "mediaElementGrid";

        /// <summary>
        /// String for the start button template element.
        /// </summary>
        protected const string StartButton = "startButton";

        /// <summary>
        /// String for the play pause button template element.
        /// </summary>
        protected const string PlayPauseButton = "playPauseButton";
        
        /// <summary>
        /// String for the previous button template element.
        /// </summary>
        protected const string PreviousButton = "previousButton";

        /// <summary>
        /// String for the next button template element.
        /// </summary>
        protected const string NextButton = "nextButton";

        /// <summary>
        /// String for the stop button template element.
        /// </summary>
        protected const string StopButton = "stopButton";

        /// <summary>
        /// String for the full screen template element.
        /// </summary>
        protected const string FullScreenButton = "fullScreenButton";

        /// <summary>
        /// String for the offline button element.
        /// </summary>
        protected const string OfflineButton = "offlineButton";

        /// <summary>
        /// String for the popout button element.
        /// </summary>
        protected const string PopOutButton = "popOutButton";

        /// <summary>
        /// String for the PlugIn button element.
        /// </summary>
        protected const string PlugInButton = "plugInButton";

        /// <summary>
        /// String for the mute button template element.
        /// </summary>
        protected const string MuteButton = "muteButton";

        /// <summary>
        /// String for the closed captions button template element.
        /// </summary>
        protected const string ClosedCaptionButton = "closedCaptionsButton";

        /// <summary>
        /// String for the closed captions options menu
        /// </summary>
        protected const string ClosedCaptionsOptionsMenu = "closedCaptionsOptionsMenu";

        /// <summary>
        /// list of the closed caption options
        /// </summary>
        protected const string ClosedCaptionsOptionsList = "closedCaptionsOptionsList";

        /// <summary>
        /// String for the volume down button template element.
        /// </summary>
        protected const string VolumeDownButton = "volumeDownButton";

        /// <summary>
        /// String for the volume up button template element.
        /// </summary>
        protected const string VolumeUpButton = "volumeUpButton";

        /// <summary>
        /// String for the playlist button template element.
        /// </summary>
        protected const string PlaylistButton = "playlistButton";

        /// <summary>
        /// String for the chapter button template element.
        /// </summary>
        protected const string ChapterButton = "chapterButton";

        /// <summary>
        /// String for the volume slider template element.
        /// </summary>
        protected const string VolumeSlider = "volumeSlider";

        /// <summary>
        /// String for the step forwards button template element.
        /// </summary>
        protected const string ButtonStepForwards = "stepForwardsButton";

        /// <summary>
        /// String for the step backwards template element.
        /// </summary>
        protected const string ButtonStepBackwards = "stepBackwardsButton";

        /// <summary>
        /// String for the position slider template element.
        /// </summary>
        protected const string PositionSlider = "positionSlider";

        /// <summary>
        /// String for the playlist selector template element.
        /// </summary>
        protected const string PlaylistSelector = "playlistSelector";

        /// <summary>
        /// String for the chapters selector template element.
        /// </summary>
        protected const string ChaptersSelector = "chaptersSelector";

        /// <summary>
        /// String for the error messages template element.
        /// </summary>
        protected const string ErrorMessageElement = "errorMessage";

        /// <summary>
        /// String for the closed captions background template element.
        /// </summary>
        protected const string ClosedCaptionArea = "closedCaptionArea";

        /// <summary>
        /// String for the plugin UI template element.
        /// </summary>
        protected const string GridPlugIn = "gridPlugIn";

        /// <summary>
        /// String for the enter FullScreen animation.
        /// </summary>
        protected const string EnterFullScreen = "enterFullScreen";

        /// <summary>
        /// String for the exit FullScreen animation.
        /// </summary>
        protected const string ExitFullScreen = "exitFullScreen";

        /// <summary>
        /// Minimum time to skip when seeking.
        /// </summary>
        private const double MinDelta = 5.0;

        /// <summary>
        /// Buffer for skip padding.
        /// </summary>
        private const double SkipBuffer = 1.0;

        /// <summary>
        /// The increment for skipping steps.
        /// </summary>
        private const double SkipSteps = 10.0;

        /// <summary>
        /// Default volume.
        /// </summary>
        private const Double VolumeDefault = 1.0;

        /// <summary>
        /// Threshold for volume muting.
        /// </summary>
        private const Double VolumeMuteThreshold = 0.01;

        /// <summary>
        /// Marker type element name.
        /// </summary>
        protected const String MarkerType = "NAME";

        /// <summary>
        /// Caption type element name.
        /// </summary>
        protected const String CaptionType = "CAPTION";

        /// <summary>
        /// Stretch box framework element.
        /// </summary>
        private FrameworkElement m_elementStretchBox;

        /// <summary>
        /// VideoWindow framework element -- used for internal sizing calculations
        /// </summary>
        private Panel m_elementVideoWindow;
        
        /// <summary>
        /// The main media element for playing audio and video.
        /// </summary>
        private Grid m_mediaElementGrid;
        private MediaElement m_actualMediaElement;
        private MediaElementShim m_mediaElementForConventionalContent;
        private MediaElementShim m_mediaElementForSmoothStreamingContent;
        private MediaElementShim m_mediaElement;
        private IPlugInMssOfflineSupport m_smoothStreamingOfflineSupport;

        /// <summary>
        /// The start button.
        /// </summary>
        private ButtonBase m_buttonStart;

        /// <summary>
        /// Pause and play button.
        /// </summary>
        private ToggleButton m_buttonPlayPause;

        /// <summary>
        /// The previous button.
        /// </summary>
        private ButtonBase m_buttonPrevious;

        /// <summary>
        /// The next button.
        /// </summary>
        private ButtonBase m_buttonNext;

        /// <summary>
        /// The stop button.
        /// </summary>
        private ButtonBase m_buttonStop;

        /// <summary>
        /// Step forwards button.
        /// </summary>
        private ButtonBase m_buttonStepForwards;

        /// <summary>
        /// Step backwards button.
        /// </summary>
        private ButtonBase m_buttonStepBackwards;

        /// <summary>
        /// Button which toggles the playlist control.
        /// </summary>
        private ToggleButton m_buttonPlaylist;

        /// <summary>
        /// Button which toggles the chapter control.
        /// </summary>
        private ToggleButton m_buttonChapter;

        /// <summary>
        /// Full screen button.
        /// </summary>
        private ButtonBase m_buttonFullScreen;

        /// <summary>
        /// Offline button.
        /// </summary>
        private ButtonBase m_buttonOffline;

        /// <summary>
        /// PopOut button.
        /// </summary>
        private ButtonBase m_buttonPopOut;

        /// <summary>
        /// PlugIn button.
        /// </summary>
        private ToggleButton m_buttonPlugIn;
        
        /// <summary>
        /// Button which toggles muting the audio stream.
        /// </summary>
        private ToggleButton m_buttonMute;

        /// <summary>
        /// Button which toggles closed captions.
        /// </summary>
        private ToggleButton m_buttonClosedCaptions;

        /// <summary>
        /// menu of closed caption options
        /// </summary>
        private Popup m_popupClosedCaptionsOptionsMenu;

        /// <summary>
        /// listbox containing closed caption options
        /// </summary>
        private ListBox m_listboxCaptionOptions;

        /// <summary>
        /// Button which turns the volume down.
        /// </summary>
        private ButtonBase m_buttonVolumeDown;

        /// <summary>
        /// Button which turns the volume up.
        /// </summary>
        private ButtonBase m_buttonVolumeUp;

        /// <summary>
        /// Volume slider.
        /// </summary>
        private SensitiveSlider m_sliderVolume;

        /// <summary>
        /// Position slider.
        /// </summary>
        private SensitiveSlider m_sliderPosition;

        /// <summary>
        /// ListBox for the playlist control.
        /// </summary>
        private Selector m_selectorPlaylist;

        /// <summary>
        /// ListBox for the chapters control.
        /// </summary>
        private Selector m_selectorChapters;

        /// <summary>
        /// Dispatch timer for posting UI messages.
        /// </summary>
        private DispatcherTimer m_timer;

        /// <summary>
        /// Background color area for closed captions.
        /// </summary>
        //private FrameworkElement m_elementCaptionBackground;

        /// <summary>
        /// Grid for displaying closed captions
        /// </summary>
        private Grid m_gridCaptionArea;

        /// <summary>
        /// element for closed captions background.
        /// </summary>
        private FrameworkElement m_elementErrorMessage;

        /// <summary>
        /// grid for PlugIn 
        /// </summary>
        private Grid m_gridPlugIn;
     
        /// <summary>
        /// The index of the current playlist item.
        /// </summary>
        private int m_currentPlaylistIndex;

        /// <summary>
        /// Retry count on media element failures.
        /// </summary>
        private int m_mediaFailureRetryCount;

        /// <summary>
        /// The maximum number of Retries to attempt.
        /// </summary>
        private const int maxMediaFailureRetries = 5;

        /// <summary>
        /// Set when  the current playlist item uses adpative streaming.
        /// </summary>
        private bool m_currentItemIsAdaptive;

        /// <summary>
        /// The index of the current chapter item.
        /// </summary>
        private int m_currentChapterIndex;

        /// <summary>
        /// Current play state.
        /// </summary>
        private bool m_inPlayState;

        /// <summary>
        /// Flag to issue play request when media element is ready.
        /// </summary>
        private bool m_playWhenMediaElementReady;

        /// <summary>
        /// Play state when user started dragging the PositionSlider thumb.
        /// </summary>
        private bool m_inPlayStateBeforeSliderPositionDrag;

        /// <summary>
        /// Flag for updating the download progress control.
        /// </summary>
        private bool m_downloadProgressNeedsUpdating;

        /// <summary>
        /// The last time the media element was clicked.
        /// </summary>
        private DateTime m_lastMediaElementClick = new DateTime(0);

        /// <summary>
        /// Dispatch timer for fading out controls.
        /// </summary>
        private DispatcherTimer m_timerControlFadeOut;
        
        /// <summary>
        /// Current control state.
        /// </summary>
        private String currentControlState;

        /// <summary>
        /// Desired control state.
        /// </summary>
        private String desiredControlState;

        /// <summary>
        /// Flag which tracks whether we should perform a seek on the next tick.
        /// </summary>
        private bool m_seekOnNextTick;

        /// <summary>
        /// The position to seek to when m_seekOnNextTick is set.
        /// </summary>
        private double m_seekOnNextTickPosition;

        /// <summary>
        /// Flag which tracks whether we should goto a different next playlist item on the next tick.
        /// </summary>
        private bool m_goToItemOnNextTick;

        /// <summary>
        /// The item to goto when m_goToItemOnNextTick is set.
        /// </summary>
        private int m_goToItemOnNextTickIndex;

        /// <summary>
        /// Flag which tracks whether we should refresh the captions on the next tick.
        /// </summary>
        private bool m_refreshCaptionsOnNextTick;

        /// <summary>
        /// Flag for tracking mute status in volume slider control.
        /// </summary>
        private bool m_mutedCache;

        /// <summary>
        /// Current unmuted volume.
        /// </summary>
        private Double m_dblUnMutedVolume = VolumeDefault;

        /// <summary>
        /// Volume slider change suppression flag.
        /// </summary>
        private int m_volumeCacheSuppressLevel = 0;

        private CaptionsManager m_captionsManagerObject;
        private CaptionsManager CaptionsManager
        {
            get            
            {
                if ((this.m_captionsManagerObject == null) && (this.m_elementVideoWindow != null))
                {
                    this.m_captionsManagerObject = new CaptionsManager(this, this.m_gridCaptionArea);
                }
                return this.m_captionsManagerObject;
            }
        }

        private Analytics m_analyticsOptional = null;
        private double m_cpuLastSample = 0.0;
        private double m_cpuPeakSample = 0.0; 
        private double m_cpuSampleSum = 0.0;
        private int m_cpuSampleCount = 1;
        private string m_cpuStatsReport = string.Empty;

        /// <summary>
        /// Initializes a new instance of the MediaPlayer class.
        /// </summary>
        public MediaPlayer()
        {
            DefaultStyleKey = typeof(MediaPlayer);

            DataContext = new ExpressionMediaPlayer.Resources();

            SetValue(PlaylistProperty, new Playlist());        
            SetPlaybackPosition(0.0);
            SetPlaybackDuration(0.0);
            TimeCode timeCodeZero = new TimeCode(0.0, SmpteFrameRate.Unknown);
            String stringZero = timeCodeZero.ToString();
            SetPlaybackPositionText(stringZero);
            SetPlaybackDurationText(stringZero);

            SetBufferingPercent(0);
            SetDownloadOffsetPercent(0);
            SetDownloadPercent(0);
            UpdateCanStep();
            
            if (!IsDesignTime)
                HtmlPage.RegisterScriptableObject("Player", this);

            Application.Current.InstallStateChanged += new EventHandler(PlayerInstallStateChanged);
            NetworkChange.NetworkAddressChanged += new NetworkAddressChangedEventHandler(PlayerNetworkAddressChanged);

            if (sm_mediaPlayer == null)
            {
                sm_mediaPlayer = this;
            }
        }

        private void UpdateCaptionOptionsMenuForCurrentPlayListItem()
        {
            if (this.CaptionsManager != null)
            {
                this.CaptionOptions = this.CaptionsManager.CaptionOptionsForPlayListItem(this.CurrentPlaylistIndex);
            }
        }

        #region events
        /// <summary>
        /// Event which fires when the state of this MediaPlayer changes.
        /// </summary>
        [ScriptableMember]
        public event RoutedEventHandler StateChanged;

        /// <summary>
        /// Event which fires when a marker is reached.
        /// </summary>
        [ScriptableMember]
        public event ScriptableTimelineMarkerRoutedEventHandler MarkerReached; 
        #endregion

        #region properties
        
        /// <summary>
        /// Gets the list of items to play.
        /// </summary>
        [ScriptableMember]
        [Category("Media"), Description("Playlist settings")]
        public Playlist Playlist
        {
            get { return (Playlist)GetValue(PlaylistProperty); }
            set 
            { 
                SetValue(PlaylistProperty, value);
                if (this.CaptionsManager != null)
                {
                    this.CaptionsManager.BuildCaptionsListForPlaylist(this.Playlist);
                }
            }
        }

        /// <summary>
        /// Gets the index of the item currently selected.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptableMember]
        public int CurrentPlaylistIndex
        {
            get { return m_currentPlaylistIndex; }
        }

        /// <summary>
        /// Gets the current PlaylistItem selected.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptableMember]
        public PlaylistItem CurrentPlaylistItem
        {
            get
            {
                if (m_currentPlaylistIndex >= 0 && m_currentPlaylistIndex < Playlist.Items.Count)
                {
                    return Playlist.Items[m_currentPlaylistIndex];
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the current playback position.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptableMember]
        public Double PlaybackPosition
        {
            get
            {
                return (Double)GetValue(PlaybackPositionProperty);
            }
        }

        /// <summary>
        /// Gets the text of the playback position.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public String PlaybackPositionText
        {
            get { return (String)GetValue(PlaybackPositionTextProperty); }
        }

        /// <summary>
        /// Gets the text of the Cpu utilization.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public String CpuText
        {
            get { return (String)GetValue(CpuTextProperty); }
        }

        /// <summary>
        /// Gets the duration of the current playlist item.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptableMember]
        public Double PlaybackDuration
        {
            get
            {
                return (Double)GetValue(PlaybackDurationProperty);
            }
        }

        /// <summary>
        /// Gets the current duration as a string.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public String PlaybackDurationText
        {
            get { return (String)GetValue(PlaybackDurationTextProperty); }
        }

        /// <summary>
        /// Gets the current buffering percent.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptableMember]
        public Double BufferingPercent
        {
            get { return (Double)GetValue(BufferingPercentProperty); }
        }

        /// <summary>
        /// Gets the source of the current poster image.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Uri PosterImageSource
        {
            get { return (Uri)GetValue(PosterImageSourceProperty); }
        }

        /// <summary>
        /// Gets the max width of the current poster image.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Double PosterImageMaxWidth
        {
            get { return (Double)GetValue(PosterImageMaxWidthProperty); }
        }

        /// <summary>
        /// Gets the max height of the current poster image.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Double PosterImageMaxHeight
        {
            get { return (Double)GetValue(PosterImageMaxHeightProperty); }
        }

        /// <summary>
        /// Gets the visibility for the buffering control.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Visibility BufferingControlVisibility
        {
            get { return (Visibility)GetValue(BufferingControlVisibilityProperty); }
        }
        
        /// <summary>
        /// Gets the visibility for the Offline download control.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Visibility OfflineDownloadProgressVisibility
        {
            get { return (Visibility)GetValue(OfflineDownloadProgressVisibilityProperty); }
        }

        /// <summary>
        /// Gets the dowloading offset percent.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptableMember]
        public Double DownloadOffsetPercent
        {
            get { return (Double)GetValue(DownloadOffsetPercentProperty); }
        }

        /// <summary>
        /// Gets the downloaded percent.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptableMember]
        public Double DownloadPercent
        {
            get { return (Double)GetValue(DownloadPercentProperty); }
        }

        /// <summary>
        /// Gets the visibility of the captions button.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Visibility CaptionsButtonVisibility
        {
            get
            {
                return (Visibility)GetValue(CaptionsButtonVisibilityProperty);
            }
        }

        /// <summary>
        /// Gets the visibility of the offline button.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Visibility OfflineButtonVisibility
        {
            get
            {
                return (Visibility)GetValue(OfflineButtonVisibilityProperty);
            }
        }
        /// <summary>
        /// Gets whether the offline button is enabled
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Boolean OfflineButtonEnabled
        {
            get
            {
                return (Boolean) GetValue(OfflineButtonEnabledProperty);
            }
        }

        /// <summary>
        /// Gets the visibility of the popout button.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Visibility PopOutButtonVisibility
        {
            get
            {
                return (Visibility)GetValue(PopOutButtonVisibilityProperty);
            }
        }

        /// <summary>
        /// Gets the visibility of the PlugIn button.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Visibility PlugInButtonVisibility
        {
            get
            {
                return (Visibility)GetValue(PlugInButtonVisibilityProperty);
            }
        }

        /// <summary>
        /// Gets the visibility of the chapters button.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Visibility ChaptersButtonVisibility
        {
            get
            {
                return (Visibility)GetValue(ChaptersButtonVisibilityProperty);
            }
        }

        /// <summary>
        /// Gets the visibility of the Playlist button.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Visibility PlaylistButtonVisibility
        {
            get
            {
                return (Visibility)GetValue(PlaylistButtonVisibilityProperty);
            }
        }

        /// <summary>
        /// Gets the visibility of the Cpu textblock.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Visibility CpuVisibility
        {
            get
            {
                return (Visibility)GetValue(CpuVisibilityProperty);
            }
        }
        
        /// <summary>
        /// Gets the caption text.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public String CaptionText
        {
            get
            {
                return (String)GetValue(CaptionTextProperty);
            }
        }

        /// <summary>
        /// Gets or sets the visibility of captions.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Visibility CaptionsVisibility
        {
            get { return (Visibility)GetValue(CaptionsVisibilityProperty); }
            set { SetValue(CaptionsVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the CaptionOptions
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<string, CaptionOption> CaptionOptions
        {
            get { return ((Dictionary<string, CaptionOption>) GetValue(CaptionOptionsProperty)); }
            set { SetValue(CaptionOptionsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the user background color.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Brush UserBackgroundColor
        {
            get { return (Brush)GetValue(UserBackgroundColorProperty); }
            set { SetValue(UserBackgroundColorProperty, value); }
        }

        /// <summary>
        /// Gets the current media element state.
        /// </summary>
        [ScriptableMember]
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public MediaElementState CurrentState
        {
            get
            {
                if (m_mediaElement != null)
                {
                    return m_mediaElement.CurrentState;
                }

                return MediaElementState.Stopped;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the media element can step.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Visibility FrameStepVisibility
        {
            get { return (Visibility)GetValue(FrameStepVisibilityProperty); }
        }

        /// <summary>
        /// Gets a value indicating whether the error message is visible.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public Visibility ErrorMessageVisibility
        {
            get { return (Visibility)GetValue(ErrorMessageVisibilityProperty); }
        }

        /// <summary>
        /// Gets the source of the current poster image.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public String ErrorMessage
        {
            get { return (String)GetValue(ErrorMessageProperty); }
        }

        /// <summary>
        /// Gets a value indicating whether the PreviousButton control is enabled.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptableMember]
        public bool ButtonPreviousIsEnabled
        {
            get { return (bool)GetValue(ButtonPreviousIsEnabledProperty); }
        }

        /// <summary>
        /// Gets a value indicating whether the NextButton control is enabled.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        [ScriptableMember]
        public bool ButtonNextIsEnabled
        {
            get { return (bool)GetValue(ButtonNextIsEnabledProperty); }
        }
        
        /// <summary>
        /// Gets the current media element we are using. -- If the SSME is in use this should return null
        /// </summary>
        public MediaElementShim CurrentMediaElement
        {
            get
            {
                return m_mediaElement;
            }
        }

        public MediaElement ConventionalMediaElement
        {
            get
            {
                // if in conventional mode -- return actual media element -- if in smooth streaming mode -- return null
                if (this.m_mediaElement == this.m_mediaElementForConventionalContent)
                {
                    return this.m_actualMediaElement;
                }
                return null;
            }
        }

        public MediaElementShim SmoothStreamingMediaElementShim
        {
            get
            {
                return m_mediaElementForSmoothStreamingContent;
            }
        }

        public IPlugInMssOfflineSupport SmoothStreamingOfflineSupport
        {
            get
            {
                if (this.m_smoothStreamingOfflineSupport == null && this.m_mediaElementForSmoothStreamingContent != null)
                {
                    this.m_smoothStreamingOfflineSupport = this.m_mediaElementForSmoothStreamingContent.OfflineSupport;
                }
                return m_smoothStreamingOfflineSupport;
            }
        }

        /// <summary>
        /// Sets the playback position of the position slider.
        /// </summary>
        /// <param name="value">The new playback position.</param>        
        protected virtual void SetPlaybackPosition(double value)
        {
            if (PlaybackPosition == value)
            {
                return;
            }

            SetValue(PlaybackPositionProperty, value);
            if (((m_sliderPosition != null) && (!m_sliderPosition.IsDragging))  // don't update slider position while the user is dragging
            && (!m_seekOnNextTick) )                                            // don't update the slider position while there is a pending seek -- as this can generate an additional seek request
            {
                // update slider position
                if (m_sliderPosition.Value != value)
                {
                    m_sliderPosition.ValueChanged -= OnSliderPositionChanged; // Don't generate a seek
                    m_sliderPosition.Value = value;
                    m_sliderPosition.ValueChanged += OnSliderPositionChanged;
                }
            }
               
            // update position text as well
            UpdatePositionDisplay();
        }

        /// <summary>
        /// Sets the playback duration of the position slider.
        /// </summary>
        /// <param name="value">The new playback duration.</param>
        protected virtual void SetPlaybackDuration(double value)
        {
            SetValue(PlaybackDurationProperty, value);
            if (m_sliderPosition != null)
            {
                m_sliderPosition.ValueChanged -= OnSliderPositionChanged; // Don't generate a seek
                m_sliderPosition.Value = 0.0;
                m_sliderPosition.Maximum = PlaybackDuration;
                m_sliderPosition.ValueChanged += OnSliderPositionChanged;
            }
        }

        /// <summary>
        /// Sets the playback position text.
        /// </summary>
        /// <param name="value">New text for the playback position.</param>
        protected virtual void SetPlaybackPositionText(string value)
        {
            SetValue(PlaybackPositionTextProperty, value);
        }

        /// <summary>
        /// Sets the Cpu utilization text.
        /// </summary>
        /// <param name="value">New text for the Cpu Utilization</param>
        protected virtual void SetCpuText(string value)
        {
            SetValue(CpuTextProperty, value);
        }

        /// <summary>
        /// Sets the playback duration text.
        /// </summary>
        /// <param name="value">The new duration text.</param>
        protected virtual void SetPlaybackDurationText(string value)
        {
            SetValue(PlaybackDurationTextProperty, value);
        }

        /// <summary>
        /// Sets the buffering percentage.
        /// </summary>
        /// <param name="value">New value for the buffering percent.</param>
        protected void SetBufferingPercent(double value)
        {
            SetValue(BufferingPercentProperty, value);
        }

        /// <summary>
        /// Sets the poster image.
        /// </summary>
        /// <param name="value">New poster image.</param>
        protected virtual void SetPosterImageSource(string value)
        {
            SetValue(PosterImageSourceProperty, value);
        }

        /// <summary>
        /// Sets the poster image width.
        /// </summary>
        /// <param name="value">New poster image width.</param>
        protected virtual void SetPosterImageMaxWidth(Double value)
        {
            SetValue(PosterImageMaxWidthProperty, value);
        }

        /// <summary>
        /// Sets the poster image height.
        /// </summary>
        /// <param name="value">New poster image height.</param>
        protected virtual void SetPosterImageMaxHeight(Double value)
        {
            SetValue(PosterImageMaxHeightProperty, value);
        }

        /// <summary>
        /// Sets the visibility of the buffering control.
        /// </summary>
        /// <param name="value">New visibility for the buffering control.</param>
        protected virtual void SetBufferingControlVisibility(Visibility value)
        {
            SetValue(BufferingControlVisibilityProperty, value);
        }

        /// <summary>
        /// Sets the visibility of the Offline download progress bar.
        /// </summary>
        /// <param name="value">New visibility for the Offline download progress bar.</param>
        internal virtual void SetOfflineDownloadProgressVisibility(Visibility value)
        {
            SetValue(OfflineDownloadProgressVisibilityProperty, value);
        }

        /// <summary>
        /// Sets the download offset percent.
        /// </summary>
        /// <param name="value">New value for the download offset.</param>
        protected void SetDownloadOffsetPercent(double value)
        {
            SetValue(DownloadOffsetPercentProperty, value);
        }

        /// <summary>
        /// Sets the download percent.
        /// </summary>
        /// <param name="value">New value for the download percent.</param>
        protected void SetDownloadPercent(double value)
        {
            SetValue(DownloadPercentProperty, value);
        }

        /// <summary>
        /// Sets the caption text.
        /// </summary>
        /// <param name="value">New value for the caption text.</param>
        public virtual void SetCaptionText(string value)
        {
            SetValue(CaptionTextProperty, value);            
        }

        /// <summary>
        /// Sets the visibility of the captions button.
        /// </summary>
        /// <param name="value">New visibility of the captions button.</param>
        protected virtual void SetCaptionsButtonVisibility(Visibility value)
        {
            SetValue(CaptionsButtonVisibilityProperty, value);
        }

        /// <summary>
        /// Sets the visibility of the offline button.
        /// </summary>
        /// <param name="value">New visibility of the offline button.</param>
        internal virtual void SetOfflineButtonVisibility(Visibility value)
        {
            SetValue(OfflineButtonVisibilityProperty, value);
        }

        /// <summary>
        /// Sets the visibility of the offline button.
        /// </summary>
        /// <param name="value">New visibility of the offline button.</param>
        internal virtual void SetOfflineButtonEnabled(Boolean value)
        {
            SetValue(OfflineButtonEnabledProperty, value);
        }

        /// <summary>
        /// Sets the visibility of the popout button.
        /// </summary>
        /// <param name="value">New visibility of the popout button.</param>
        protected virtual void SetPopOutButtonVisibility(Visibility value)
        {
            SetValue(PopOutButtonVisibilityProperty, value);
        }

        /// <summary>
        /// Sets the visibility of the PlugIn button.
        /// </summary>
        /// <param name="value">New visibility of the PlugIn button.</param>
        protected virtual void SetPlugInButtonVisibility(Visibility value)
        {
            if (value == Visibility.Collapsed)
            {
                ShowPlugIn(false);
            }
            else
            {
                if (this.m_buttonPlugIn != null)
                {
                    ShowPlugIn(this.m_buttonPlugIn.IsChecked == true);
                }
            }
            SetValue(PlugInButtonVisibilityProperty, value);
        }

        /// <summary>
        /// Sets the visibility of the chapters button.
        /// </summary>
        /// <param name="value">New visibility of the chapters button.</param>
        protected virtual void SetChaptersButtonVisibility(Visibility value)
        {
            SetValue(ChaptersButtonVisibilityProperty, value);
            if (value == Visibility.Collapsed)
                ShowChapters(false);
        }

        /// <summary>
        /// Sets the visibility of the playlist button.
        /// </summary>
        /// <param name="value">New visibility of the playlist button.</param>
        protected virtual void SetPlaylistButtonVisibility(Visibility value)
        {
            SetValue(PlaylistButtonVisibilityProperty, value);
            if (value == Visibility.Collapsed)
                ShowPlaylist(false);
        }

        /// <summary>
        /// Sets the visibility of the Cpu stats.
        /// </summary>
        /// <param name="value">New visibility of the Cpu stats.</param>
        protected virtual void SetCpuVisibility(Visibility value)
        {
            SetValue(CpuVisibilityProperty, value);
        }

        #endregion

        #region Attached Properties
        /// <summary>
        /// Gets the current value of the hide on click property.
        /// </summary>
        /// <param name="obj">Dependency property object.</param>
        /// <returns>Flag indicating the status of the hide on click properry.</returns>
        public static bool GetHideOnClick(DependencyObject obj)
        {
            return (bool)obj.GetValue(HideOnClickProperty);
        }

        /// <summary>
        /// Sets the value of the hide on click property.
        /// </summary>
        /// <param name="obj">Dependency property object.</param>
        /// <param name="value">The new value of the hide on click property.</param>
        public static void SetHideOnClick(DependencyObject obj, bool value)
        {
            obj.SetValue(HideOnClickProperty, value);
        }
        #endregion

        #region Utilties

        /// <summary>
        /// Toggles full screen mode.
        /// </summary>
        [ScriptableMember]
        public virtual void ToggleFullScreen()
        {
            Application.Current.Host.Content.IsFullScreen = !(Application.Current.Host.Content.IsFullScreen);
        }

        private bool IsPopOutAllowed
        {
            get
            {
                return (Application.Current.InstallState == InstallState.NotInstalled)
                    && HtmlPage.IsEnabled
                    && (HtmlPage.Document != null)
                    && (HtmlPage.Document.DocumentUri != null)
                    && Playlist.EnablePopOut
                    /* && HtmlPage.IsPopupWindowAllowed */
                    ;
            }
        }

        /// <summary>
        /// causes window to popout
        /// </summary>
        [ScriptableMember]
        public virtual void PopOutWindow()
        {
            try
            {
                if (this.IsPopOutAllowed)
                {
                    Pause();
                    if (m_buttonPopOut != null)
                    {
                        m_buttonPopOut.IsEnabled = false;
                    }

                    HtmlPopupWindowOptions popupOptions = new HtmlPopupWindowOptions();
                    popupOptions.Directories = false;
                    popupOptions.Height = 480;
                    popupOptions.Width = 640;
                    if (Playlist != null && Playlist.Items.Count > 0 && !Double.IsNaN(Playlist.Items[0].VideoWidth) && !Double.IsNaN(Playlist.Items[0].VideoHeight))
                    {
                        popupOptions.Width = (int)(Playlist.Items[0].VideoWidth + 0.5);
                        popupOptions.Height = (int)(Playlist.Items[0].VideoHeight + 0.5);
                    }
                    popupOptions.Scrollbars = false;
                    popupOptions.Status = false;
                    popupOptions.Toolbar = false;
                    popupOptions.Resizeable = true;
                    popupOptions.Menubar = false;
                    popupOptions.Location = false;
                    HtmlPage.PopupWindow(HtmlPage.Document.DocumentUri, null, popupOptions);
                }
            }
            catch (ArgumentException)
            {
                // popup fails for local paths/UNC paths, in this case resume video playback
                Play(); 
            }
        }

        /// <summary>
        /// Returns a System.Windows.Media.Color from a color format string.
        /// </summary>
        /// <param name="color">String describing the color.</param>
        /// <returns>The new System.Windows.Media.Color.</returns>
        public static Color ColorFromString(String color)
        {
            UInt32 uiValue = UInt32.Parse(color.Substring(1), System.Globalization.NumberStyles.AllowHexSpecifier | System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            byte a = (byte)((uiValue >> 0x18) & 0xFF);
            byte r = (byte)((uiValue >> 0x10) & 0xFF);
            byte g = (byte)((uiValue >> 0x08) & 0xFF);
            byte b = (byte)((uiValue) & 0xFF);
            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Unescapes an escaped string.
        /// </summary>
        /// <param name="escaped">The escaped string.</param>
        /// <returns>The new unescaped string.</returns>
        public static String UnEscape(String escaped)
        {
            String tmp = System.Uri.UnescapeDataString(escaped);
            tmp = tmp.Replace("%21", "!");
            tmp = tmp.Replace("%26", "&");
            tmp = tmp.Replace("%27", "\'");
            tmp = tmp.Replace("%2C", ",");
            tmp = tmp.Replace("\\\"", "\"");
            tmp = tmp.Replace("\\\\", "\\");
            return tmp;
        }

        #endregion

        #region public methods

        /// <summary>
        /// Plays the current item in the playlist.
        /// </summary>
        [ScriptableMember]
        public virtual void Play()
        {
            if (!m_inPlayState)
            {
                TogglePlayPause();
            }
        }

        /// <summary>
        /// Pauses the current playlist item.
        /// </summary>
        [ScriptableMember]
        public virtual void Pause()
        {
            if (m_inPlayState)
            {
                TogglePlayPause();
            }
        }

        /// <summary>
        /// Stops the current playlist item.
        /// </summary>
        [ScriptableMember]
        public virtual void Stop()
        {
            ButtonClickStopLogic();
        }

        /// <summary>
        /// Current media position
        /// </summary>
        [ScriptableMember]
        public virtual double Position
        {
            get
            {
                return (m_mediaElement.Position.TotalSeconds);
            }
            set
            {
                m_mediaElement.Position = new TimeSpan((long)(value * 10000000));
                SeekToTime(value);                
            }
        }

        /// <summary>
        /// Goes to the next playlist item at the next ui update interval
        /// </summary>        
        public void GoToPlaylistItemOnNextTick(int playlistItemIndex)
        {
            m_playWhenMediaElementReady = true;
            if (!m_goToItemOnNextTick) // don't set it if already set
            {
                m_goToItemOnNextTick = true;
                m_goToItemOnNextTickIndex = playlistItemIndex;
            }
        }

        /// <summary>
        /// Goes to the next playlist item.
        /// </summary>
        [ScriptableMember]
        public void GoToNextPlaylistItem()
        {
            if (this.Playlist != null && ((this.m_currentPlaylistIndex+1) < Playlist.Items.Count))
            {
                GoToPlaylistItemOnNextTick(m_currentPlaylistIndex + 1);
            }
        }

        /// <summary>
        /// Goes to the next playlist item.
        /// </summary>
        [ScriptableMember]
        public void GoToPreviousPlaylistItem()
        {
            GoToPlaylistItemOnNextTick(m_currentPlaylistIndex - 1);
        }

        public void GoToPlaylistItem1(int playlistItemIndex)
        {
            GoToPlaylistItemOnNextTick(playlistItemIndex);
        }

        /// <summary>
        /// Goes to a playlist item.
        /// </summary>
        /// <param name="playlistItemIndex">The index of the playlist item to go to.</param>
        [ScriptableMember]
        private void GoToPlaylistItemInternal(int playlistItemIndex)
        {
            if (IsDesignTime)
            {
                return;
            }
#if DEBUG
            Debug.WriteLine("GoToPlaylistItem item=" + playlistItemIndex.ToString() +  DateTime.Now.ToString());
#endif
            if (playlistItemIndex >= Playlist.Items.Count)
            {
                if (Playlist.AutoRepeat)
                {
                    playlistItemIndex = 0;
                }
            }

            if (playlistItemIndex >= 0 && playlistItemIndex < Playlist.Items.Count)
            {
                m_currentChapterIndex = 0;
                ClearCaptionText();

                bool canSkipReset = (m_currentPlaylistIndex == playlistItemIndex) && (this.m_mediaElement != null);
                if (canSkipReset)
                {
                    switch (this.m_mediaElement.CurrentState)
                    {
                        case MediaElementState.Closed:
                        case MediaElementState.Stopped:
                            canSkipReset = false;
                            break;
                        default:
                            break;
                    }
                }

                if (!canSkipReset)
                {
#if DEBUG
                    Debug.WriteLine("GoToPlaylistItem not skipping");
#endif
                    m_currentPlaylistIndex = playlistItemIndex;
                    PlaylistItem playlistItem = Playlist.Items[m_currentPlaylistIndex];

                    if (m_selectorChapters != null)
                    {
                        m_selectorChapters.ItemsSource = playlistItem.Chapters;
                        SetChaptersButtonVisibility(playlistItem.Chapters.Count>0?Visibility.Visible:Visibility.Collapsed);
                    }

                    // Update window title with playlist item title (or filename)
                    if (!Application.Current.IsRunningOutOfBrowser && 
                        HtmlPage.IsEnabled && 
                        HtmlPage.Document != null)
                    {
                        string newTitle = string.Empty;
                        if (!string.IsNullOrEmpty(playlistItem.Title))
                        {
                            newTitle = playlistItem.Title;
                        }
                        else
                        {
                            if (playlistItem.MediaSource != null)
                            {
                                if (playlistItem.IsAdaptiveStreaming)
                                {
                                    newTitle = System.IO.Path.GetDirectoryName(playlistItem.MediaSource.ToString());
                                    newTitle = System.IO.Path.GetFileName(newTitle);
                                }
                                else
                                {
                                    newTitle = System.IO.Path.GetFileName(playlistItem.MediaSource.ToString());
                                }
                            }
                        }

                        HtmlPage.Document.SetProperty("title", newTitle);
                    }

                    m_mediaFailureRetryCount = 0;

                    DoOpenPlaylistItem(playlistItem);

                    // Update and show the poster frame for the current item -- if not actively playing
                    if (!m_inPlayState && !Playlist.AutoPlay)
                    {
                        DisplayPoster(m_currentPlaylistIndex);
                    }

                    UpdateCaptionOptionsMenuForCurrentPlayListItem();
                }
            }
            else if (playlistItemIndex >= Playlist.Items.Count)
            {
                // Reached end -- flag that playback is paused.
                m_inPlayState = false;
            }

            OnItemStarted();
        }

        private void DoOpenPlaylistItem(PlaylistItem playlistItem)
        {
            if (this.m_elementVideoWindow != null)
            {
                if (this.m_elementVideoWindow.Visibility == Visibility.Collapsed) // Audio only type templates
                {
                    if (playlistItem.IsAdaptiveStreaming) // SmoothStreaming not supported in audio only case
                    {
                        string strErrorMessage = String.Format(CultureInfo.CurrentUICulture, ExpressionMediaPlayer.Resources.errorInvalidMedia, playlistItem.MediaSource.OriginalString);
                        ShowErrorMessage(strErrorMessage);
                        return;
                    }
                }
            }


            // Attach media source to the MediaElement
            m_currentItemIsAdaptive = playlistItem.IsAdaptiveStreaming;
            SetPlugInButtonVisibility(m_currentItemIsAdaptive ? Visibility.Visible : Visibility.Collapsed);

            try
            {
                if (m_currentItemIsAdaptive)
                {
                    this.InitSmoothStreaming(playlistItem);
                }
                else
                {
                    this.InitConventionalStreaming(playlistItem);
                }

            }
            catch (IsolatedStorageException iso)
            {
                ShowErrorMessage(iso.Message);
                Stop();
            }
        }
        #endregion

        #region TemplateHandlers

        /// <summary>
        /// Overrides base.OnApplyTemplate().
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            GetTemplateChildren();

            ConfigureBinding();
            ApplyProperties();

            HookHandlers();
        }

        /// <summary>
        /// Gets the child elements of the template.
        /// </summary>
        protected virtual void GetTemplateChildren()
        {
            m_elementStretchBox = GetTemplateChild(StretchBox) as FrameworkElement;
            m_elementVideoWindow = GetTemplateChild(VideoWindow) as Panel;
            m_buttonStart = GetTemplateChild(StartButton) as ButtonBase;
            m_buttonPlayPause = GetTemplateChild(PlayPauseButton) as ToggleButton;
            m_buttonPrevious = GetTemplateChild(PreviousButton) as ButtonBase;
            m_buttonNext = GetTemplateChild(NextButton) as ButtonBase;
            m_buttonStop = GetTemplateChild(StopButton) as ButtonBase;
            m_buttonFullScreen = GetTemplateChild(FullScreenButton) as ButtonBase;
            m_buttonOffline = GetTemplateChild(OfflineButton) as ButtonBase;
            m_buttonPopOut = GetTemplateChild(PopOutButton) as ButtonBase;
            m_buttonPlugIn = GetTemplateChild(PlugInButton) as ToggleButton;
            m_sliderPosition = GetTemplateChild(PositionSlider) as SensitiveSlider;
            m_buttonMute = GetTemplateChild(MuteButton) as ToggleButton;
            m_buttonVolumeDown = GetTemplateChild(VolumeDownButton) as ButtonBase;
            m_buttonVolumeUp = GetTemplateChild(VolumeUpButton) as ButtonBase;
            m_sliderVolume = GetTemplateChild(VolumeSlider) as SensitiveSlider;
            m_buttonPlaylist = GetTemplateChild(PlaylistButton) as ToggleButton;
            m_buttonChapter = GetTemplateChild(ChapterButton) as ToggleButton;
            m_selectorPlaylist = GetTemplateChild(PlaylistSelector) as Selector;
            m_selectorChapters = GetTemplateChild(ChaptersSelector) as Selector;
            m_elementErrorMessage = GetTemplateChild(ErrorMessageElement) as FrameworkElement;
            m_buttonStepForwards = GetTemplateChild(ButtonStepForwards) as ButtonBase;
            m_buttonStepBackwards = GetTemplateChild(ButtonStepBackwards) as ButtonBase;
            m_gridPlugIn = GetTemplateChild(GridPlugIn) as Grid;

            m_buttonClosedCaptions = GetTemplateChild(ClosedCaptionButton) as ToggleButton;
            m_gridCaptionArea = GetTemplateChild(ClosedCaptionArea) as Grid;
            m_popupClosedCaptionsOptionsMenu = GetTemplateChild(ClosedCaptionsOptionsMenu) as Popup;
            m_listboxCaptionOptions = GetTemplateChild(ClosedCaptionsOptionsList) as ListBox;

            m_mediaElementGrid = GetTemplateChild(MediaElementGrid) as Grid;
            Debug.Assert(m_mediaElementGrid != null, "essential markup item missing: " + MediaElementGrid);

            if (m_listboxCaptionOptions != null)
            {
                m_listboxCaptionOptions.DataContext = this;
            }

            m_actualMediaElement = GetTemplateChild(MediaElement) as MediaElement;
            Debug.Assert(m_actualMediaElement != null, "essential markup item missing: " + MediaElement);
        }

        #endregion

        #region EventHandlers

        /// <summary>
        /// Parse init params. It should be just a playlist so parse that.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected virtual void ParseStartupParameters(StartupEventArgs e)
        {
            if (null == e)
            {
                Debug.WriteLine("ParseStartupParameters passed bogus data");
                return;
            }

            String strInitValue = string.Empty;

            try
            {
                string strSimpleMediaSource = string.Empty;
                string strSimpleThumb = string.Empty;
                string strSimpleTitle = string.Empty;
                string strSimpleAspect = string.Empty;
                string strIsAdaptive = string.Empty;
                string strPlaylist = null;

                if (e.InitParams.TryGetValue("playerSettings", out strInitValue))
                {
                    strPlaylist = string.Empty;
                    string[] separators = new string[] { "\r\n", "\t" };
                    string[] initPieces = strInitValue.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string initPiece in initPieces)
                    {
                        try
                        {
                            strPlaylist += System.Uri.UnescapeDataString(initPiece);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            // just add the escaped string
                            strPlaylist += initPiece;
                        }
                    }
                }
                else
                {
                    if (e.InitParams.TryGetValue("MediaSource", out strInitValue))
                    {
                        strSimpleMediaSource = strInitValue;
                    }
                    if (e.InitParams.TryGetValue("ThumbSource", out strInitValue))
                    {
                        strSimpleThumb = strInitValue;
                    }
                    if (e.InitParams.TryGetValue("Title", out strInitValue))
                    {
                        strSimpleTitle = strInitValue;
                    }
                    if (e.InitParams.TryGetValue("Aspect", out strInitValue))
                    {
                        strSimpleAspect = strInitValue;
                    }
                    if (e.InitParams.TryGetValue("IsAdaptiveStreaming", out strInitValue))
                    {
                        strIsAdaptive = strInitValue;
                    }
                }

                // Cpu Utilization reporting parameters
                {
                    bool monitorCpu = false;
                    string strCpuValue = string.Empty;
                    if (e.InitParams.TryGetValue("ShowCpuStats", out strCpuValue))
                    {
                        bool showCpuStats = false;
                        if (bool.TryParse(strCpuValue, out showCpuStats))
                        {
                            if (showCpuStats)
                            {
                                this.SetCpuVisibility(Visibility.Visible);
                                monitorCpu = true;
                            }
                        }
                    }
                    if (e.InitParams.TryGetValue("CpuStatsReport", out strCpuValue))
                    {
                        this.m_cpuStatsReport = strCpuValue;
                        monitorCpu = true;
                    }

                    if (monitorCpu)
                    {
                        try
                        {
                            m_analyticsOptional = new Analytics();
                        }
                        catch (System.Exception)
                        {
                            // wierd case Analytics.ctor can throw when performance registry data is messed up
                            m_analyticsOptional = null;
                        }
                    }
                }

                // Check for simplfied init case
                if (strPlaylist == null)
                {
                    strPlaylist = "<Playlist>"
                        + "<AutoLoad>true</AutoLoad>"
                        + "<AutoPlay>true</AutoPlay>"
                        + "<DisplayTimeCode>false</DisplayTimeCode>"
                        + "<EnableCachedComposition>true</EnableCachedComposition>"
                        + "<EnableCaptions>true</EnableCaptions>"
                        + "<EnableOffline>false</EnableOffline>"
                        + "<EnablePopOut>false</EnablePopOut>"
                        + "<StartMuted>false</StartMuted>"
                        + "<StretchNonSquarePixels>StretchToFill</StretchNonSquarePixels>"
                        + "<Items>"
                        + "<PlaylistItem>"
                        + "<MediaSource>" + strSimpleMediaSource + "</MediaSource>";
                    if (!string.IsNullOrEmpty(strSimpleThumb))
                    {
                        strPlaylist += "<ThumbSource>" + strSimpleThumb + "</ThumbSource>";
                    }
                    if (!string.IsNullOrEmpty(strSimpleTitle))
                    {
                        strPlaylist += "<Title>" + strSimpleTitle + "</Title>";
                    }
                    if (!string.IsNullOrEmpty(strSimpleAspect))
                    {
                        strSimpleAspect = strSimpleAspect.ToUpper(CultureInfo.InvariantCulture);
                        if (strSimpleAspect == "WIDE")
                        {
                            strPlaylist += "<AspectRatioWidth>16</AspectRatioWidth><AspectRatioHeight>9</AspectRatioHeight>";
                        }
                        else if (strSimpleAspect == "NARROW")
                        {
                            strPlaylist += "<AspectRatioWidth>4</AspectRatioWidth><AspectRatioHeight>3</AspectRatioHeight>";
                        }
                    }
                    {
                        bool isSet = false;
                        bool isAdaptive = false;
                        if (!string.IsNullOrEmpty(strIsAdaptive))
                        {
                            isSet = bool.TryParse(strIsAdaptive, out isAdaptive);
                        }

                        if (!isSet)
                        {
                            string tmp = strSimpleMediaSource.ToUpper(CultureInfo.CurrentCulture);
                            isAdaptive = tmp.Contains(".ISM") || tmp.Contains(".ISML");
                        }

                        string param = "<IsAdaptiveStreaming>" + isAdaptive.ToString(CultureInfo.InvariantCulture).ToLower() + "</IsAdaptiveStreaming>";

                        strPlaylist += param;
                    }
                    strPlaylist += "</PlaylistItem>"
                        + "</Items>"
                        + "</Playlist>";
                }
                if (strPlaylist != null)
                {
                    SetValue(PlaylistProperty, Playlist.Create(strPlaylist));
                    ApplyProperties();
                }
            }
            catch (System.Xml.XmlException xe)
            {
                // special case. Encoder precompiled template? fail silently...
                if (strInitValue.IndexOf("<$=", StringComparison.OrdinalIgnoreCase) > 0 && strInitValue.IndexOf("$>", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    SetValue(PlaylistProperty, new Playlist());
                    return;
                }
                ShowErrorMessage(xe.ToString());
            }
            catch (NullReferenceException xe)
            {
                ShowErrorMessage(xe.ToString());
            }
            catch (InvalidCastException xe)
            {
                ShowErrorMessage(xe.ToString());
            }
            catch (InvalidPlaylistException xe)
            {
                ShowErrorMessage(xe.ToString());
            }
            catch (IndexOutOfRangeException xe)
            {
                ShowErrorMessage(xe.ToString());
            }        
        }

        /// <summary>
        /// Write the Cpu Utilization statitics data to isolated storage
        /// </summary>
        /// <param name="reportName">The HTML specified report filename</param>
        /// <param name="videoSource">The video file name that was just rendered</param>
        /// <param name="CpuSampleSum">The sum of processor load samples</param>
        /// <param name="CpuSampleCount">The number of processor load samples taken</param>
        private static void WriteCpuStatData(string reportName, string videoSource, double cpuSampleSum, int cpuSampleCount, double cpuPeakSample)
        {

            DateTime now = DateTime.Now;
            Double averageCpu = 0.0;
            if (cpuSampleCount > 0)
            {
                averageCpu = cpuSampleSum / cpuSampleCount;
            }

            string reportData = "\"" + reportName + "\", "
                + now.ToString(CultureInfo.InvariantCulture) + ", \"" 
                + videoSource + "\", " 
                + cpuSampleSum.ToString(CultureInfo.InvariantCulture) + ", " 
                + cpuSampleCount.ToString(CultureInfo.InvariantCulture) + ", "
                + averageCpu.ToString(CultureInfo.InvariantCulture) + ", "
                + cpuPeakSample.ToString(CultureInfo.InvariantCulture);

            Debug.WriteLine("WriteCpuStatData(" + reportData + ")");

            try
            {
                Stream statStream = null;
                bool writeHeader = false;

                IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();
                if (iso.FileExists(reportName))
                {
                    statStream = iso.OpenFile(reportName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    statStream.Position = statStream.Length;
                }
                else
                {
                    writeHeader = true;
                    statStream = iso.CreateFile(reportName);
                }

                StreamWriter writer = new StreamWriter(statStream, System.Text.Encoding.UTF8);

                if (writeHeader)
                {
                    writer.WriteLine("Name, DateTime, Source, Sum, Count, Average, Peak");
                }

                writer.WriteLine(reportData);

                writer.Flush();
                writer.Close();
            }
            catch (IsolatedStorageException ex)
            {
                Debug.WriteLine("ISO: failed to writr WriteCpuStatData data to isolated storage " + reportName + ": " + ex.Message);
            }
        }

        /// <summary>
        /// playlist has changed, update visibility of buttons/lit etc.
        /// </summary>
        void Playlist_PlaylistChanged(object sender, RoutedEventArgs e)
        {
            ApplyProperties();
        }
        /// <summary>
        /// playlist has changed, hook the playlist changed event
        /// </summary>
        private void PlaylistChanged()
        {
            Playlist.PlaylistChanged += new RoutedEventHandler(Playlist_PlaylistChanged);           
        }
        /// <summary>
        /// Dependancy Property "Playlist" has changed. Need to hook the playlist changed
        /// </summary>
        private static void PlaylistChanged(DependencyObject dependancyProperty, DependencyPropertyChangedEventArgs e)
        {
            MediaPlayer player = dependancyProperty as MediaPlayer;
            if (player != null)
            {
                player.PlaylistChanged();
            }
        }
        /// <summary>
        /// Handler for the OnStartup event.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        public virtual void OnStartup(object sender, StartupEventArgs e)
        {
            if (Application.Current.InstallState == InstallState.NotInstalled)
            {
                ParseStartupParameters(e);
            }
            else             
            {       
                try
                {
                    SetValue(PlaylistProperty, Playlist.ReadFromIsoStorage());
                    ApplyProperties();                    
                }
                catch (IsolatedStorageException xe)
                {
                    IsoUri.ClearIsoStorage();
                    ShowErrorMessage(xe.ToString());
                }
                catch (NullReferenceException xe)
                {
                    ShowErrorMessage(xe.ToString());
                }
                catch (InvalidCastException xe)
                {
                    ShowErrorMessage(xe.ToString());
                }
                catch (InvalidPlaylistException xe)
                {
                    ShowErrorMessage(xe.ToString());
                }   
            }
        }

        /// <summary>
        /// Detect if we are running in Blend
        /// </summary>
        /// <returns>are we running in Blend?</returns>
        static protected bool IsDesignTime
        {
            get
            {
                return Application.Current.GetType() == typeof(Application);
            }
        }

        /// <summary>
        /// Event handler for the playlist clicked event.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonClickPlaylist(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = sender as ToggleButton;
            ShowPlaylist(btn.IsChecked == true);            
        }

        /// <summary>
        /// show/hide playlist
        /// </summary>
        /// <param name="show">true for show, false for hide</param>
        [ScriptableMember]
        public virtual void ShowPlaylist(bool show)
        {
            if (show)
            {
                VisualStateManager.GoToState(this, "showPlaylist", true);
                if (m_selectorPlaylist != null && m_selectorPlaylist.SelectedItem != null)
                {
                    if (m_selectorPlaylist.SelectedItem is ListBoxItem)
                    {
                        ((ListBoxItem)m_selectorPlaylist.SelectedItem).Focus();
                    }
                }
            }
            else if (!show)
            {
                VisualStateManager.GoToState(this, "hidePlaylist", true);
            }
        }

        /// <summary>
        /// Event handler for the chapter button event.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonClickChapter(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = sender as ToggleButton;
            ShowChapters(btn.IsChecked == true);            
        }
        
        /// <summary>
        /// show/hide chapters control
        /// </summary>
        /// <param name="show">true for show, false for hide</param>
        [ScriptableMember]
        public virtual void ShowChapters(bool show)
        {
            VisualStateManager.GoToState(this, show ? "showChapters" : "hideChapters", true);
            if (show && 
                m_selectorChapters != null && 
                m_selectorChapters.SelectedItem!=null &&
                m_selectorChapters.SelectedItem is ListBoxItem)
            {
                ((ListBoxItem)m_selectorChapters.SelectedItem).Focus();
            }
        }

        /// <summary>
        /// get/set if chapters showing
        /// </summary>
        [ScriptableMember]
        public virtual bool ChaptersVisible
        {
            get
            {
                return m_buttonChapter.IsChecked == true;
            }
            set
            {
                m_buttonChapter.IsChecked = value;
                ShowChapters(value);                
            }
        }

        /// <summary>
        /// set/get if playlist showing
        /// </summary>
        [ScriptableMember]
        public virtual bool PlaylistVisible
        {
            get
            {
                return m_buttonPlaylist.IsChecked == true;
            }
            set
            {
                m_buttonPlaylist.IsChecked = value;
                ShowPlaylist(value);
            }
        }

        /// <summary>
        /// Update the state of the buffering progress indicator.
        /// </summary>
        private void UpdateBufferingControls()
        {
            // control the visiblity of the buffering control
            if ((m_currentPlaylistIndex >= 0)
            && (m_currentPlaylistIndex < Playlist.Items.Count)
            && (MediaElementState.Buffering == m_mediaElement.CurrentState))
            {
                if (BufferingControlVisibility != Visibility.Visible)
                {
                    SetBufferingControlVisibility(Visibility.Visible);
                    VisualStateManager.GoToState(this, "showBuffering", true);
                }

                SetBufferingPercent(m_mediaElement.BufferingProgress * ProgressConst.Progress2Percent);
            }
            else
            {
                if (BufferingControlVisibility != Visibility.Collapsed)
                {
                    SetBufferingControlVisibility(Visibility.Collapsed);
                    VisualStateManager.GoToState(this, "hideBuffering", true);
                }
            }
        }

        /// <summary>
        /// Update the playback position slider and playback position text display with current media position
        /// </summary>
        private void UpdatePlaybackPosition()
        {
            // Don't update position based on media element while the user is dragging the slider
            if (m_sliderPosition == null || !m_sliderPosition.IsDragging)
            {
                // while playing or paused -- get position from the media element
                Double position = 0.0;
                switch (m_mediaElement.CurrentState)
                {
                    case MediaElementState.Playing:
                    case MediaElementState.Paused:
                    case MediaElementState.Buffering:
                        position = m_mediaElement.Position.TotalSeconds;
                        break;
                    default:
                        break;
                }

                SetPlaybackPosition(position);
            }
        }

        /// <summary>
        /// Update Cpu utilization data
        /// </summary>
        private void UpdateCpuStats()
        {
            if (this.m_analyticsOptional != null)
            {
                if (MediaElementState.Playing == m_mediaElement.CurrentState)
                {
                    this.m_cpuLastSample = m_analyticsOptional.AverageProcessLoad;
                    this.m_cpuSampleSum += m_cpuLastSample;
                    this.m_cpuSampleCount++;
                    this.m_cpuPeakSample = Math.Max(this.m_cpuPeakSample, this.m_cpuLastSample);

                    if (this.CpuVisibility == Visibility.Visible)
                    {
                        string cpuText = ((int)this.m_cpuLastSample).ToString(CultureInfo.CurrentCulture);
                        if (this.m_cpuSampleCount > 0)
                        {
                            cpuText += " " + ((int)(this.m_cpuSampleSum / this.m_cpuSampleCount)).ToString(CultureInfo.CurrentCulture);
                        }
                        SetCpuText(cpuText);
                    }
                }
            }
        }

        /// <summary>
        /// If chapter list is visible -- Update selected chapter
        /// Set Selection onto matching chapterlist item
        /// </summary>
        private void UpdateChapterListbox()
        {
            if ((m_selectorChapters != null)
            && (m_selectorChapters.Visibility == Visibility.Visible)
            && (m_selectorChapters.SelectedIndex != m_currentChapterIndex)
            && (m_currentChapterIndex >= 0)
            && (m_currentChapterIndex < m_selectorChapters.Items.Count))
            {
                m_selectorChapters.SelectionChanged -= OnListBoxSelectionChangedChapters; // avoid clicked-on selection change logic
                m_selectorChapters.SelectedIndex = m_currentChapterIndex;
                m_selectorChapters.SelectionChanged += OnListBoxSelectionChangedChapters;

                // Scroll current chapter into view
                Object objCurrentChapterItem = m_selectorChapters.Items[m_currentChapterIndex];
                if (objCurrentChapterItem != null)
                {
                    try
                    {
                        if (m_selectorChapters is ListBox)
                        {
                            ((ListBox)m_selectorChapters).ScrollIntoView(objCurrentChapterItem);
                        }
                    }
                    catch (NullReferenceException nre)
                    {
                        Debug.WriteLine(nre.ToString());
                        Debug.WriteLine(nre.StackTrace.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// If the Playlist is visible -- Update selected item
        /// Set Selection onto matching Playlist item
        /// </summary>
        private void UpdatePlaylistListbox()
        {
            if ((m_selectorPlaylist != null)
            && (m_selectorPlaylist.Visibility == Visibility.Visible)
            && (m_selectorPlaylist.SelectedIndex != m_currentPlaylistIndex)
            && (m_currentPlaylistIndex >= 0)
            && (m_currentPlaylistIndex < Playlist.Items.Count)
            && (m_currentPlaylistIndex < m_selectorPlaylist.Items.Count))
            {
                m_selectorPlaylist.SelectionChanged -= OnListBoxSelectionChangedPlaylist; // avoid clicked-on selection change logic
                m_selectorPlaylist.SelectedIndex = m_currentPlaylistIndex;
                m_selectorPlaylist.SelectionChanged += OnListBoxSelectionChangedPlaylist;

                // Scroll current playlist item into view
                Object objCurrentPlaylistItem = m_selectorPlaylist.Items[m_currentPlaylistIndex];
                if (objCurrentPlaylistItem != null)
                {
                    try
                    {
                        if (m_selectorChapters is ListBox)
                        {
                            ((ListBox)m_selectorChapters).ScrollIntoView(objCurrentPlaylistItem);
                        }
                    }
                    catch (NullReferenceException nre)
                    {
                        Debug.WriteLine(nre.ToString());
                        Debug.WriteLine(nre.StackTrace.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Update the download progress bar as needed.
        /// </summary>
        private void UpdateDownloadProgress()
        {
            if (m_downloadProgressNeedsUpdating)
            {
                m_downloadProgressNeedsUpdating = false;
                Double downloadProgress = m_mediaElement.DownloadProgress;
                SetDownloadPercent(downloadProgress * ProgressConst.Progress2Percent);
                SetDownloadOffsetPercent(m_mediaElement.DownloadProgressOffset * ProgressConst.Progress2Percent);
            }
        }

        /// <summary>
        /// Restore play state after dragging slider position.
        /// </summary>
        private void RestorePlayStateAfterSliderDrag()
        {
            if (m_sliderPosition != null)
            {
                if (!m_sliderPosition.IsDragging && m_inPlayStateBeforeSliderPositionDrag)
                {
                    m_inPlayStateBeforeSliderPositionDrag = false;
                    InternalPlayAfterDrag();
                }
            }
        }

        internal void EnableClosedCaptionButton(bool enableButton)
        {
            if (this.m_buttonClosedCaptions != null)
            {
                if(this.m_buttonClosedCaptions.Dispatcher.CheckAccess())
                {
                    this.m_buttonClosedCaptions.IsEnabled = enableButton;
                }
                else
                {
                    this.m_buttonClosedCaptions.Dispatcher.BeginInvoke(() => this.EnableClosedCaptionButton(enableButton));
                }
            }
        }

        /// <summary>
        /// Event handler for a Timer tick.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnTimerTick(object sender, EventArgs e)
        {
            if (m_goToItemOnNextTick)
            {
                m_goToItemOnNextTick = false;
                GoToPlaylistItemInternal(m_goToItemOnNextTickIndex);
                return;
            }

            if (m_mediaElement == null)
            {
                return;
            }

            if (m_seekOnNextTick)
            {
                DoActualSeek();
            }

            InternalDoPlayWhenReady();
            
            if (this.CaptionsManager != null)
            {
                if (this.m_refreshCaptionsOnNextTick)
                {
                    this.m_refreshCaptionsOnNextTick = false;
                    this.CaptionsManager.RefreshCaptions();
                }
            }

            UpdatePlaybackPosition();
            UpdateChapterListbox();
            UpdatePlaylistListbox();
            UpdatePrevNext();
            UpdateDownloadProgress();
            UpdateBufferingControls();
            RestorePlayStateAfterSliderDrag();

            if (this.nextTickErrorMessage != null)
            {
                this.ShowErrorMessage(this.nextTickErrorMessage);
                this.nextTickErrorMessage = null;
            }

            UpdateCpuStats();

            RepositionCaptionPopup();
        }

        /// <summary>
        /// Event handler for the control fade out event.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnTimerControlFadeOutTick(object sender, EventArgs e)
        {
            if(currentControlState != desiredControlState)
            {
                GoToControlState(desiredControlState);
            }

            SetDesiredControlState();
        }

        /// <summary>
        /// Event handler for the slider position changed event.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnSliderPositionChanged(object sender, RoutedEventArgs e)
        {
            SeekToTime(m_sliderPosition.Value);
        }

        /// <summary>
        /// Handle DragStart event.
        /// </summary>
        /// <param name="sender">Source object, Thumb.</param>
        /// <param name="e">Drag start args.</param>
        private void OnSliderPositionDragStarted(object sender, DragStartedEventArgs e)
        {
            if (m_inPlayState)
            {
                m_inPlayStateBeforeSliderPositionDrag = true;
                InternalPause();
            }
        }

        /// <summary>
        /// overridable, item has started
        /// </summary>
        public virtual void OnItemStarted()
        {
        }

        /// <summary>
        /// Handle DragCompleted event.
        /// </summary>.
        /// <param name="sender">Source object, Thumb.</param>
        /// <param name="e">Drag completed args.</param>
        private void OnSliderPositionDragCompleted(object sender, DragCompletedEventArgs e)
        {
            SeekToTime(m_sliderPosition.Value);
            DoActualSeek();
        }

        /// <summary>
        /// Event handler for the media element state changed event.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnMediaElementCurrentStateChanged(object sender, RoutedEventArgs e)
        {
            MediaElementState currentState = m_mediaElement.CurrentState;
#if DEBUG
            Debug.WriteLine("OnMediaElementCurrentStateChanged new state=" + currentState.ToString());
#endif

            // Update the state of the play/pause button
            switch (currentState)
            {
                case MediaElementState.Playing:
                case MediaElementState.Opening:
                case MediaElementState.Buffering:
                case MediaElementState.AcquiringLicense:
                    {
                        if (m_buttonPlayPause != null)
                        {
                            m_buttonPlayPause.IsChecked = true;
                        }
                        break;
                    }

                case MediaElementState.Paused:
                case MediaElementState.Stopped:
                case MediaElementState.Closed:
                    {
                        if (m_buttonPlayPause != null)
                        {
                            m_buttonPlayPause.IsChecked = false;
                        }
                        break;
                    }

                default:
                    break;
            }

            // update the state of the thumbnail downloader to avoid taxing network bandwidth when the ME needs bandwidth
            switch (currentState)
            {
                case MediaElementState.Opening:
                case MediaElementState.Buffering:
                case MediaElementState.AcquiringLicense:
                default:
                    ThumbnailDownloader.DisableThumbnailDownload();
                    break;
                case MediaElementState.Playing:
                case MediaElementState.Paused:
                case MediaElementState.Stopped:
                case MediaElementState.Closed:
                    if (!IsDesignTime)
                    {
                        ThumbnailDownloader.EnableThumbnailDownload();
                    }
                    break;
            }

            UpdateBufferingControls();

            if (StateChanged != null)
            {
                StateChanged(sender, e);
            }
        }

        /// <summary>
        /// Event handler for the media opened event from the media element.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnMediaElementMediaOpened(object sender, RoutedEventArgs e)
        {
            ShowErrorMessage(null);
#if DEBUG
            Debug.WriteLine("OnMediaElementMediaOpened: " + DateTime.Now.ToString());
#endif
            // hide the "start" button if present and visible
            if (m_buttonStart != null && m_buttonStart.Visibility != Visibility.Collapsed)
            {
                m_buttonStart.Visibility = Visibility.Collapsed;
            }

            DisplayPoster(-1);
            EnableCaptionsArea();
            UpdateCaptionData();
            PerformResize();

            SetPlaybackDuration(m_mediaElement.NaturalDuration.TimeSpan.TotalSeconds);
            UpdateDurationDisplay();
            UpdateCanStep();

            // Start playing or Pausing the item depending on user settings and current play state.
            if (Playlist.AutoPlay)
            {
                InternalPlay();
            }
            else if (Playlist.AutoLoad)
            {
                InternalPause();
            }
         }

        /// <summary>
        /// Event handler for the media ended event from the media element.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnMediaElementMediaEnded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("OnMediaElementMediaEnded");

            if ((this.m_analyticsOptional != null) && (!string.IsNullOrEmpty(m_cpuStatsReport)))
            {
                string currentVideoSource = string.Empty;
                if (this.m_currentPlaylistIndex >= 0 && this.m_currentPlaylistIndex < Playlist.Items.Count)
                {
                    PlaylistItem playlistItem = Playlist.Items[this.m_currentPlaylistIndex];
                    currentVideoSource = playlistItem.MediaSource.ToString();
                }
                WriteCpuStatData(this.m_cpuStatsReport, currentVideoSource, this.m_cpuSampleSum, this.m_cpuSampleCount, this.m_cpuPeakSample);
                this.m_cpuSampleSum = 0;
                this.m_cpuSampleCount = 1;
                this.m_cpuPeakSample = 0;
            }
            GoToNextPlaylistItem();
        }

        /// <summary>
        /// Event handler for the media failed event from the media element.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnMediaElementMediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Debug.WriteLine("OnMediaElementMediaFailed");

            // Work-around the fact that silverlight doesn't do any http retries when 404 is returned on 1st attempt -- but a number of CDNs do exactly that on the 1st attempt
             m_mediaFailureRetryCount++;

            if ((m_mediaFailureRetryCount < maxMediaFailureRetries)
            && (!MediaPlayer.IsOffline) // With offline playback -- either the isolated storage file is there or it isn't -- no point in retrying
            && (Playlist != null) 
            && (m_currentPlaylistIndex >= 0) 
            && (Playlist.Items.Count > m_currentPlaylistIndex))
            {
                PlaylistItem playlistItem = Playlist.Items[m_currentPlaylistIndex];
                if (!playlistItem.IsAdaptiveStreaming ) // SmoothStreaming has it's own retry logic -- don't be redundant
                {
                    ShowErrorMessage(m_mediaFailureRetryCount.ToString(CultureInfo.CurrentCulture));
                    DoOpenPlaylistItem(playlistItem);
                    return;
                }
            }

            Uri sourceUri = null;
            if (m_mediaElement.Source != null)
            {
                sourceUri = m_mediaElement.Source;
            }
            else if (m_currentPlaylistIndex >= 0 && m_currentPlaylistIndex < Playlist.Items.Count)
            {
                sourceUri = Playlist.Items[m_currentPlaylistIndex].MediaSource;
            }

            string strErrorMessage = string.Empty;
            if (sourceUri != null)
            {
                string strUrl = string.Empty;
                if ( sourceUri.IsAbsoluteUri )
                {
                    strUrl = sourceUri.AbsolutePath;
                }
                else
                {
                    strUrl = sourceUri.OriginalString;
                }

                strErrorMessage = String.Format(CultureInfo.CurrentUICulture, ExpressionMediaPlayer.Resources.errorInvalidMedia, strUrl);
            }

            if (m_currentPlaylistIndex >= 0 && m_currentPlaylistIndex < Playlist.Items.Count && Playlist.Items[m_currentPlaylistIndex].IsAdaptiveStreaming)
            {
                strErrorMessage = String.Format(CultureInfo.CurrentUICulture, ExpressionMediaPlayer.Resources.errorNonSmoothStreamingServer, strErrorMessage);
            }

            strErrorMessage += "\r\n" + e.ErrorException.Message.ToString();

            Debug.WriteLine("Failure message=" + strErrorMessage);

            ShowErrorMessage(strErrorMessage);
        }

        /// <summary>
        /// Event handler for the mouse down event from the media element.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
       private void OnMediaElementMouseDown(object sender, RoutedEventArgs e)
        {
            if ((DateTime.Now - m_lastMediaElementClick).TotalMilliseconds < 300)
            {
                TogglePlayPause();
                ToggleFullScreen();
            }
            else
            {
                TogglePlayPause();
            }

            m_lastMediaElementClick = DateTime.Now;
        }

        /// <summary>
        /// Event handler for the mouse moved event from the stretch box.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnStretchBoxMouseMove(object sender, MouseEventArgs e)
        {
            if (Application.Current.Host.Content.IsFullScreen)
            {
                GoToControlState(ExitFullScreen);
            }
        }

        /// <summary>
        /// Event handler for the download progress event from the media element.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnMediaElementDownloadProgressChanged(object sender, RoutedEventArgs e)
        {
#if DEBUG_EXTRA
            Debug.WriteLine("OnMediaElementDownloadProgressChanged:" + e.ToString());
#endif
            m_downloadProgressNeedsUpdating = true;
        }

        /// <summary>
        /// Click handler for the start button.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonClickStart(object sender, RoutedEventArgs e)
        {
            GoToPlaylistItemOnNextTick(0);
        }

        /// <summary>
        /// Click handler for the offline button.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonClickOffline(object sender, RoutedEventArgs e)
        {
            if (Application.Current.InstallState == InstallState.NotInstalled)
            {
                if (Playlist.ContentNeedsDownloading())
                {
                    bool enoughSpace = Playlist.EnsureStorageSpace();
                    if (enoughSpace)
                    {
                        EnqueueTakeContentOffline();
                    }
                }
            }
        }

        /// <summary>
        /// Click handler for the popout button.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonClickPopOut(object sender, RoutedEventArgs e)
        {
            PopOutWindow();
        }
       
        /// <summary>
        /// Click handler for the PlugIn button.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonClickPlugIn(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = (ToggleButton)sender;
            ShowPlugIn(toggleButton.IsChecked == true); 
        }

        /// <summary>
        /// show/hide plugin control
        /// </summary>
        /// <param name="show">true for show, false for hide</param>
        [ScriptableMember]
        public virtual void ShowPlugIn(bool show)
        {
            VisualStateManager.GoToState(this, ((show) ? "showPlugIn" : "hidePlugIn"), true);
        }

        /// <summary>
        /// Click handler for the play and pause button.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonClickPlayPause(object sender, RoutedEventArgs e)
        {
            // If the big "play" button is shown -- restart playback from the 1st item in the playlist 
            if (m_buttonStart != null && m_buttonStart.Visibility == Visibility.Visible)
            {
                OnButtonClickStart(sender, e);
                return;
            }

            TogglePlayPause();
        }

        /// <summary>
        /// Click handler for the stop button.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonClickStop(object sender, RoutedEventArgs e)
        {
            ButtonClickStopLogic();
        }

        private void ButtonClickStopLogic()
        {
            m_inPlayState = false;
            m_currentPlaylistIndex = 0;
            m_currentChapterIndex = 0;
            DisplayPoster(m_currentPlaylistIndex);
            if (m_mediaElement != null)
            {
                m_mediaElement.Stop();
                m_mediaElement.AutoPlay = false;
                m_mediaElement.Source = null;
            }
            if (m_buttonStart != null)
            {
                m_buttonStart.Visibility = Visibility.Visible;
            }
            if (m_buttonPlayPause != null)
            {
                m_buttonPlayPause.IsChecked = false;
            }
        }

        /// <summary>
        /// Click handler for the previous button.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonClickPrevious(object sender, RoutedEventArgs e)
        {
            SeekToPreviousItem();
        }

        /// <summary>
        /// Click handler for the next button.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonClickNext(object sender, RoutedEventArgs e)
        {
            SeekToNextItem();
        }

        /// <summary>
        /// seek player to start of current playlistitem
        /// </summary>
        [ScriptableMember]
        public virtual void SeekToStart()
        {
            if ((m_currentPlaylistIndex >= 0)
            && (m_currentPlaylistIndex < Playlist.Items.Count)
            && (Playlist.Items[m_currentPlaylistIndex] != null))
            {
                // Pause playback -- frame step mode isn't very useful otherwise.
                if (m_inPlayState)
                {
                    InternalPause();
                }

                SeekToTime(0);
            }
        }

        /// <summary>
        /// seek player to end of current playlist item
        /// </summary>
        [ScriptableMember]
        public virtual void SeekToEnd()
        {
            if ((m_currentPlaylistIndex >= 0)
            && (m_currentPlaylistIndex < Playlist.Items.Count)
            && (Playlist.Items[m_currentPlaylistIndex] != null)
            && (Playlist.Items[m_currentPlaylistIndex].SmpteFrameRate != SmpteFrameRate.Unknown))
            {
                // Pause playback -- frame step mode isn't very useful otherwise.
                if (m_inPlayState)
                {
                    InternalPause();
                }
                SeekToTime(m_mediaElement.NaturalDuration.TimeSpan.TotalSeconds);                    
            }
        }

        /// <summary>
        /// scriptable access to volume
        /// </summary>
        [ScriptableMember]
        [Category("Media"), Description("Set player volume 0.0(off) to 1.0(full)")]
        public virtual double Volume
        {
            get
            {
                return m_mediaElement.Volume;
            }
            set
            {
                UnMuteAt(value);
            }
        }

        /// <summary>
        /// Scriptable access to Mute
        /// </summary>
        [ScriptableMember]
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public virtual bool Mute
        {
            get
            {
                return m_mediaElement.Volume == 0.0f;
            }
            set
            {
                if (value == true)
                    CacheVolumeLevel();
                else
                    UnMute();
            }
        }

        /// <summary>
        /// Increments the volume by the given amount.
        /// </summary>
        /// <param name="dblVolumeIncrement">Amount to increment the volume.</param>
        private void VolumeIncrement(double dblVolumeIncrement)
        {
            double dblVolume = m_mediaElement.Volume;
            dblVolume = Math.Min(1.0, Math.Max(0.0, dblVolume + dblVolumeIncrement));
            UnMuteAt(dblVolume);
        }

        /// <summary>
        /// Click handler for the volume down button.
        /// </summary>
        /// <param name="sender">Source of this event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonClickVolumeDown(object sender, RoutedEventArgs e)
        {
            if (m_sliderVolume != null)
            {
                VolumeIncrement(-m_sliderVolume.SmallChange);
            }
            else
            {
                VolumeIncrement(-0.1);
            }
        }

        /// <summary>
        /// Click handler for the volume up button.
        /// </summary>
        /// <param name="sender">Source of this event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonClickVolumeUp(object sender, RoutedEventArgs e)
        {
            if (m_sliderVolume != null)
            {
                VolumeIncrement(m_sliderVolume.SmallChange);
            }
            else
            {
                VolumeIncrement(0.1);
            }
        }

        /// <summary>
        /// Event handler for the volume changed event.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnSliderVolumeChanged(object sender, RoutedEventArgs e)
        {
#if DEBUG
            Debug.WriteLine("OnSliderVolumeChanged current value:" + m_sliderVolume.Value.ToString());
#endif
            if (m_volumeCacheSuppressLevel < 1)
            {
                UnMuteAt(m_sliderVolume.Value);
            }
        }

        /// <summary>
        /// Unmute the volume.
        /// </summary>
        /// <returns>Returns true.</returns>
        private void UnMuteAt(Double unmutedVolume)
        {
            if ( (m_buttonMute != null) && ((m_buttonMute.IsChecked == null) || (true == m_buttonMute.IsChecked)))
            {
                m_buttonMute.IsChecked = false;
            }

            UnCacheVolumeLevelAt(unmutedVolume);
        }

        /// <summary>
        /// Unmute the volume.
        /// </summary>
        private void UnMute()
        {
            UnMuteAt(Double.NaN);
        }

        /// <summary>
        /// Caches the current volume level.
        /// </summary>
        private void CacheVolumeLevel()
        {
            m_mutedCache = true;

            if (m_mediaElement != null)
            {
                m_dblUnMutedVolume = m_mediaElement.Volume;
                m_mediaElement.Volume = 0.0;
#if DEBUG
                Debug.WriteLine("CacheVolumeLevel cached value:" + m_dblUnMutedVolume.ToString());
#endif
            }
            if (m_sliderVolume != null)
            {
                m_volumeCacheSuppressLevel++;
                m_sliderVolume.Value = 0.0;
                m_volumeCacheSuppressLevel--;
            }

            if ((m_buttonMute != null) && ((m_buttonMute.IsChecked == null) || (false == m_buttonMute.IsChecked)))
            {
                m_buttonMute.IsChecked = true;
            }
        }

        /// <summary>
        /// Uncaches the current volume level with a supplied volume level
        /// </summary>
        private void UnCacheVolumeLevelAt(Double unmutedVolume)
        {
            m_mutedCache = false;

            if (m_dblUnMutedVolume < VolumeMuteThreshold)
            {
                m_dblUnMutedVolume = VolumeDefault;
            }

            if (!Double.IsNaN(unmutedVolume))
            {
                m_dblUnMutedVolume = unmutedVolume;
            }

            if (m_mediaElement != null)
            {
                m_mediaElement.Volume = m_dblUnMutedVolume;
            }
            if (m_sliderVolume != null)
            {
                m_volumeCacheSuppressLevel++;
                m_sliderVolume.Value = m_dblUnMutedVolume;
                m_volumeCacheSuppressLevel--;
#if DEBUG
                Debug.WriteLine("UnCacheVolumeLevelAt value now:" + m_sliderVolume.Value.ToString());
#endif
            }
        }

        /// <summary>
        /// Uncaches the current volume level.
        /// </summary>
        private void UnCacheVolumeLevel()
        {
            UnCacheVolumeLevelAt(Double.NaN);
        }

        /// <summary>
        /// Click handler for the Mute button.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonClickMute(object sender, RoutedEventArgs e)
        {
            if (m_buttonMute.IsChecked == false && (m_mutedCache || m_mediaElement.Volume < VolumeMuteThreshold))
            {
                UnCacheVolumeLevel();
            }
            else
            {
                CacheVolumeLevel();
            }
        }

        /// <summary>
        /// step one frame forwards
        /// </summary>
        private void StepForwards()
        {
            if ((m_currentPlaylistIndex >= 0)
                && (m_currentPlaylistIndex < Playlist.Items.Count)
                && (Playlist.Items[m_currentPlaylistIndex] != null)
                && (Playlist.Items[m_currentPlaylistIndex].SmpteFrameRate != SmpteFrameRate.Unknown)
                && (m_mediaElement.Position.TotalSeconds < m_mediaElement.NaturalDuration.TimeSpan.TotalSeconds))
            {
                // Pause playback -- frame step mode isn't very useful otherwise.
                if (m_inPlayState)
                {
                    InternalPause();
                }

                TimeCode oneFrame = new TimeCode("00:00:00:01", Playlist.Items[m_currentPlaylistIndex].SmpteFrameRate);
                TimeCode current = TimeCode.FromAbsoluteTime((Double)m_mediaElement.Position.TotalSeconds, Playlist.Items[m_currentPlaylistIndex].SmpteFrameRate);
                TimeCode newPosition = current.Add(oneFrame);
                SeekToTime(newPosition.TotalSeconds);
            }
        }

        /// <summary>
        /// step one frame backwards
        /// </summary>
        private void StepBackwards()
        {
            if ((m_currentPlaylistIndex >= 0)
            && (m_currentPlaylistIndex < Playlist.Items.Count)
            && (Playlist.Items[m_currentPlaylistIndex] != null)
            && (Playlist.Items[m_currentPlaylistIndex].SmpteFrameRate != SmpteFrameRate.Unknown)
            && (m_mediaElement.Position.TotalSeconds > 0))
            {
                // Pause playback -- frame step mode isn't very useful otherwise.
                if (m_inPlayState)
                {
                    InternalPause();
                }

                TimeCode oneFrame = new TimeCode("00:00:00:01", Playlist.Items[m_currentPlaylistIndex].SmpteFrameRate);

                TimeCode current = TimeCode.FromAbsoluteTime((Double)m_mediaElement.Position.TotalSeconds, Playlist.Items[m_currentPlaylistIndex].SmpteFrameRate);

                TimeCode newPosition;
                try
                {
                    newPosition = current.Subtract(oneFrame);
                }
                catch (OverflowException)
                {
                    newPosition = new TimeCode("00:00:00:00", Playlist.Items[m_currentPlaylistIndex].SmpteFrameRate); ;
                }

                SeekToTime(newPosition.TotalSeconds);
            }
        }

        /// <summary>
        /// Event handler for the step forwards event.
        /// </summary>
        /// <param name="sender">Source of this event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonStepForwards(object sender, RoutedEventArgs e)
        {
            StepForwards();
        }

        /// <summary>
        /// Event handler for the step backwards button.
        /// </summary>
        /// <param name="sender">Source of this event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonStepBackwards(object sender, RoutedEventArgs e)
        {
            StepBackwards();
        }

        /// <summary>
        /// fixup for frame accuracy. Always step forward one frame.
        /// </summary>
        private void FixupFrameStep()
        {
            if (Playlist.DisplayTimeCode && 
                (Visibility) GetValue(FrameStepVisibilityProperty) == Visibility.Visible)
            {
                StepForwards();
            }
        }

        /// <summary>
        /// Event handler for the resized event.
        /// </summary>
        /// <param name="sender">Source of this event.</param>
        /// <param name="e">Event args.</param>
        private void OnResized(object sender, EventArgs e)
        {
            PerformResize();
            this.repositionCaptionPopup = true;
        }

        /// <summary>
        /// Event handler for the zoom event.
        /// </summary>
        /// <param name="sender">Source of this event.</param>
        /// <param name="e">Event args.</param>
        void OnZoomed(object sender, EventArgs e)
        {
            PerformResize();
            this.repositionCaptionPopup = true;
        }

        /// <summary>
        /// Event handler for Application Exit event.
        /// </summary>
        /// <param name="sender">Source of this event.</param>
        /// <param name="e">Event args.</param>
        private void OnAppExit(object sender, EventArgs e)
        {
            Stop();
        }

        /// <summary>
        /// Event handler for the full screen changed event.
        /// </summary>
        /// <param name="sender">Source of this event.</param>
        /// <param name="e">Event args.</param>
        private void OnFullScreenChanged(object sender, EventArgs e)
        {
            PerformResize();
            this.repositionCaptionPopup = true;
        }

        /// <summary>
        /// Click handler for the full screen button.
        /// </summary>
        /// <param name="sender">Source of this event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonClickFullScreen(object sender, RoutedEventArgs e)
        {
            ToggleFullScreen();
        }

        /// <summary>
        /// Event handler for changing the playlist item.
        /// </summary>
        /// <param name="sender">Source of this event.</param>
        /// <param name="e">Event args.</param>
        private void OnListBoxSelectionChangedPlaylist(object sender, RoutedEventArgs e)
        {
            if (m_selectorPlaylist != null && GetHideOnClick(m_selectorPlaylist))
            {
                if (m_buttonPlaylist != null)
                {
                    m_buttonPlaylist.IsChecked = false;
                }
                VisualStateManager.GoToState(this, "hidePlaylist", true);
            }
            GoToPlaylistItemOnNextTick(m_selectorPlaylist.SelectedIndex);
        }

        /// <summary>
        /// Event handler for the chapters item changed event.
        /// </summary>
        /// <param name="sender">Source of this event.</param>
        /// <param name="e">Event args.</param>
        private void OnListBoxSelectionChangedChapters(object sender, RoutedEventArgs e)
        {
            if (m_selectorChapters != null && GetHideOnClick(m_selectorChapters))
            {
                if (m_buttonChapter != null)
                {
                    m_buttonChapter.IsChecked = false;
                }

                VisualStateManager.GoToState(this, "hideChapters", true);
            }

            m_currentChapterIndex = m_selectorChapters.SelectedIndex;
            if ((m_currentPlaylistIndex >= 0)
            && (m_currentPlaylistIndex < Playlist.Items.Count)
            && (m_currentChapterIndex >= 0) 
            && (m_currentChapterIndex < Playlist.Items[m_currentPlaylistIndex].Chapters.Count))
            {
                SeekToTime(Playlist.Items[m_currentPlaylistIndex].Chapters[m_currentChapterIndex].Position);
            }
        }

        /// <summary>
        /// Event handler for the marker reached event from the media element.
        /// </summary>
        /// <param name="sender">Source of this event.</param>
        /// <param name="e">Event args.</param>
        private void OnMediaElementMarkerReached(object sender, TimelineMarkerRoutedEventArgs e)
        {
#if DEBUG
            Debug.WriteLine("OnMediaElementMarkerReached:" + e.ToString());
#endif
            //Test if this a "Marker" vs a "Caption"
            if ((m_selectorChapters != null) && e.Marker.Type.Equals(MarkerType))
            {
                // compute current chapter index from playback position
                m_currentChapterIndex = ChapterIndexFromPosition(e.Marker.Time);
            }

            // Display marker or caption text in the caption area
            String type = e.Marker.Type.ToUpper(CultureInfo.InvariantCulture);
            if (type.Equals(CaptionType))
            {
                SetCaptionText(e.Marker.Text);
            }

            if (MarkerReached != null)
            {
                ScriptableTimelineMarkerRoutedEventArgs timelineMarkerEventArgs = new ScriptableTimelineMarkerRoutedEventArgs( new ScriptableTimelineMarker(e.Marker));
                MarkerReached(sender, timelineMarkerEventArgs);
            }
        }

        /// <summary>
        /// Helper routine for playing the current item.
        /// </summary>
        private void InternalPlay()
        {
            InternalPlayBase(false);
        }

        private void InternalPlayAfterDrag()
        {
            InternalPlayBase(true);
        }

        private void InternalPlayBase(bool restoreAfterDrag)
        {
            if (m_buttonStart != null)
            {
                m_buttonStart.Visibility = Visibility.Collapsed;
            }

            if (m_mediaElement != null)
            {
                if (restoreAfterDrag)
                {
                    m_playWhenMediaElementReady = true;
                }
                else if (!RewindLogic())
                {
                    m_playWhenMediaElementReady = true;
                }
            }
        }

        private bool RewindLogic()
        {
            bool rewind = this.CurrentState == MediaElementState.Stopped;
            if ( !rewind )
            {
                // Within 1 second of end of play of last item in list? do a big rewind / restart
                if ((PlaybackDuration > 0) && (CurrentPlaylistIndex == (Playlist.Items.Count - 1)))
                {
                    var diff = PlaybackDuration - Position;
                    Debug.WriteLine("RewindLogic diff=" + diff.ToString());
                    if (diff < 1.0)
                    {
                        rewind = true;
                    }
                }
            }
            if (rewind)
            {
                GoToPlaylistItemOnNextTick(0);
                SeekToTime(0);
            }
            return rewind;
        }

        /// <summary>
        /// This bit of defered logic was added after current builds of the SSME started throwing errors when a Play commaned is issued against the SSME prior to the SSME entering an "open" state
        /// This work-around defers the call to play until the ME / SSME has opened and is ready to play.
        /// </summary>
        private void InternalDoPlayWhenReady()
        {
            if (m_playWhenMediaElementReady)
            {
                if (m_mediaElement != null)
                {
                    var currentState = m_mediaElement.CurrentState;
                    Debug.WriteLine("InternalDoPlayWhenReady: currentState=" + currentState.ToString());
                    switch (currentState)
                    {
                        case MediaElementState.Playing: // Already playing -- mark flags;
                            m_playWhenMediaElementReady = false;
                            m_inPlayState = true;
                            return;
                        case MediaElementState.Paused:
                        case MediaElementState.Stopped:
                            m_mediaElement.Play();
                            m_playWhenMediaElementReady = false;
                            m_inPlayState = true;
                            return;
                        case MediaElementState.Closed: 
                        case MediaElementState.Opening:
                        case MediaElementState.AcquiringLicense:
                        case MediaElementState.Buffering:
                        case MediaElementState.Individualizing:
                            Debug.WriteLine("Waiting for MediaElement to be ready to play");
                            break;
                        default:
                            Debug.Assert(false, "Need to add code to handle new MediaElementState");
                            return;
                    }
                }
            }
        }

        /// <summary>
        /// Helper routine for pausing the current item.
        /// </summary>
        private void InternalPause()
        {
            if (m_buttonStart != null)
            {
                m_buttonStart.Visibility = Visibility.Collapsed;
            }

            if (m_mediaElement != null)
            {
                m_mediaElement.Pause();
            }

            m_inPlayState = false;
        }

        #endregion


        #region ProtectedUtilities

        /// <summary>
        /// Updates the position display.
        /// </summary>
        protected virtual void UpdatePositionDisplay()
        {
            SmpteFrameRate frameRate = SmpteFrameRate.Unknown;
            if (Playlist.DisplayTimeCode)
            {
                if ((m_currentPlaylistIndex >= 0) && (m_currentPlaylistIndex < Playlist.Items.Count))
                {
                    frameRate = Playlist.Items[m_currentPlaylistIndex].SmpteFrameRate;
                }
            }

            SetPlaybackPositionText(TimeCode.ConvertToString(PlaybackPosition, frameRate));
        }

        /// <summary>
        /// Updates the duration display.
        /// </summary>
        protected virtual void UpdateDurationDisplay()
        {
            SmpteFrameRate frameRate = SmpteFrameRate.Unknown;
            if (Playlist.DisplayTimeCode)
            {
                if ((m_currentPlaylistIndex >= 0) && (m_currentPlaylistIndex < Playlist.Items.Count))
                {
                    frameRate = Playlist.Items[m_currentPlaylistIndex].SmpteFrameRate;
                }
            }

            SetPlaybackDurationText(TimeCode.ConvertToString(PlaybackDuration, frameRate));
        }

        private string nextTickErrorMessage = null;
        internal void SetNextTickErrorMessage(string message)
        {
            if (!string.IsNullOrEmpty(this.nextTickErrorMessage))
            {
                nextTickErrorMessage += "\r\n";
                nextTickErrorMessage += message;
            }
            else
            {
                nextTickErrorMessage = message;
            }
        }

        internal static MediaPlayer sm_mediaPlayer = null;
        internal static void StaticShowErrorMessage(string message)
        {
            if (sm_mediaPlayer != null)
            {
                sm_mediaPlayer.ShowErrorMessage(message);
            }
        }

        /// <summary>
        /// Shows an error message.
        /// </summary>
        /// <param name="message">The error message to display.</param>
        internal virtual void ShowErrorMessage(string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                SetValue(ErrorMessageVisibilityProperty, Visibility.Collapsed);
                SetValue(ErrorMessageProperty, String.Empty);
                return;
            }

            SetValue(ErrorMessageProperty, message);
            SetValue(ErrorMessageVisibilityProperty, Visibility.Visible);

            // if we cant find element for error message use popup...
            if (m_elementErrorMessage == null)
            {
                if (Application.Current.InstallState == InstallState.NotInstalled && HtmlPage.IsEnabled)
                    HtmlPage.Window.Alert(message);
                else
                    MessageBox.Show(message);
            }
        }

        /// <summary>
        /// Creates a new position timer.
        /// </summary>
        /// <param name="interval">Interval of the timer.</param>
        protected void CreatePositionTimer()
        {
            if (m_timer == null)
            {
                m_timer = new DispatcherTimer();
                m_timer.Interval = new TimeSpan(0/*days*/, 0/*hours*/, 0/*minutes*/, 0/*seconds*/,180/*milliseconds*/); // approximately 6 NTSC frames
                m_timer.Tick += new EventHandler(OnTimerTick);
            }

            m_timer.Start();
        }

        /// <summary>
        /// UnCreates the position timer.
        /// </summary>
        protected void UnCreatePositionTimer()
        {
            if (m_timer != null)
            {
                m_timer.Tick += new EventHandler(OnTimerTick);
                m_timer.Stop();
                m_timer = null;
            }
        }

        /// <summary>
        /// Creates the timer for fading out the controls.
        /// </summary>
        protected void CreateFadeoutTimer()
        {
            if (m_timerControlFadeOut == null)
            {
                m_timerControlFadeOut = new DispatcherTimer();
                m_timerControlFadeOut.Interval = new TimeSpan(0, 0, 0, 2, 0); // 2 seconds
                m_timerControlFadeOut.Tick += new EventHandler(OnTimerControlFadeOutTick);
            }

            m_timerControlFadeOut.Start();
        }

        /// <summary>
        /// UnCreates the timer for fading out the controls.
        /// </summary>
        protected void UnCreateFadeoutTimer()
        {
            if (m_timerControlFadeOut != null)
            {
                m_timerControlFadeOut.Tick -= new EventHandler(OnTimerControlFadeOutTick);
                m_timerControlFadeOut.Stop();
                m_timerControlFadeOut = null;
            }
        }

        /// <summary>
        /// Hooks our event handlers.
        /// </summary>
        protected virtual void HookHandlers()
        {
            Debug.WriteLine("UnhookHandlers:");

            if (!IsDesignTime)
            {
                CreatePositionTimer();
                CreateFadeoutTimer();
            }

            this.KeyDown += new KeyEventHandler(MediaPlayerKeyDown);

            if (Application.Current != null)
            {
                Application.Current.Host.Content.FullScreenChanged += OnFullScreenChanged;
                Application.Current.Host.Content.Resized += OnResized;
                Application.Current.Host.Content.Zoomed += OnZoomed;
                Application.Current.Exit += OnAppExit;
            }

            if (m_elementStretchBox != null)
            {
                m_elementStretchBox.MouseMove += OnStretchBoxMouseMove;
            }

            if (m_buttonStart != null)
            {
                m_buttonStart.Click += OnButtonClickStart;
            }

            if (m_buttonOffline != null)
            {
                m_buttonOffline.Click += OnButtonClickOffline;
            }

            if (m_buttonPopOut != null)
            {
                m_buttonPopOut.Click += OnButtonClickPopOut;
            }

            if (m_buttonPlugIn != null)
            {
                m_buttonPlugIn.Click += OnButtonClickPlugIn;
            } 
            
            if (m_buttonPlayPause != null)
            {
                m_buttonPlayPause.Click += OnButtonClickPlayPause;
            }

            if (m_buttonStop != null)
            {
                m_buttonStop.Click += OnButtonClickStop;
            }

            if (m_buttonPrevious != null)
            {
                m_buttonPrevious.Click += OnButtonClickPrevious;
            }

            if (m_buttonNext != null)
            {
                m_buttonNext.Click += OnButtonClickNext;
            }

            if (m_buttonMute != null)
            {
                m_buttonMute.Click += OnButtonClickMute;
            }

            if (m_buttonStepBackwards != null)
            {
                m_buttonStepBackwards.Click += OnButtonStepBackwards;
            }

            if (m_buttonStepForwards != null)
            {
                m_buttonStepForwards.Click += OnButtonStepForwards;
            }

	        if (m_buttonClosedCaptions != null)
            {
                m_buttonClosedCaptions.Click += OnButtonClickClosedCaptions;
            }

            if (m_listboxCaptionOptions!= null)
            {
                m_listboxCaptionOptions.SelectionChanged += OnCaptionOptionsSelectedIndexChanged;
            }

            if ( m_popupClosedCaptionsOptionsMenu != null )
            {
                m_popupClosedCaptionsOptionsMenu.Closed += OnClosedCaptionsOptionsMenu_Closed;
            }

            if (m_buttonVolumeDown != null)
            {
                m_buttonVolumeDown.Click += OnButtonClickVolumeDown;
            }

            if (m_buttonVolumeUp != null)
            {
                m_buttonVolumeUp.Click += OnButtonClickVolumeUp;
            }

            if (m_buttonFullScreen != null)
            {
                m_buttonFullScreen.Click += OnButtonClickFullScreen;
            }

            if (m_buttonPlaylist != null)
            {
                m_buttonPlaylist.Click += OnButtonClickPlaylist;
            }

            if (m_selectorPlaylist != null)
            {
                m_selectorPlaylist.SelectionChanged += OnListBoxSelectionChangedPlaylist;
            }

            if (m_selectorChapters != null)
            {
                m_selectorChapters.SelectionChanged += OnListBoxSelectionChangedChapters;
            }

            if (m_buttonChapter != null)
            {
                m_buttonChapter.Click += OnButtonClickChapter;
            }

            if (m_sliderPosition != null)
            {
                m_sliderPosition.ValueChanged += OnSliderPositionChanged;
                m_sliderPosition.DragStarted += OnSliderPositionDragStarted;
                m_sliderPosition.DragCompleted += OnSliderPositionDragCompleted;
            }

            if (m_sliderVolume != null)
            {
                m_sliderVolume.ValueChanged += OnSliderVolumeChanged;
            }

            if (!IsDesignTime)
            {
                ThumbnailDownloader.EnableThumbnailDownload();
            }
        }

        private void UnhookMediaElementEvents()
        {
            Debug.WriteLine("UnhookMediaElementEvents:");
            if (m_mediaElement != null)
            {
                Debug.WriteLine("UnhookMediaElementEvents: conv=" + (m_mediaElement == m_mediaElementForConventionalContent).ToString() + " adpt=" + (m_mediaElement == m_mediaElementForSmoothStreamingContent).ToString());
                m_mediaElement.MediaFailed -= OnMediaElementMediaFailed;
                m_mediaElement.MediaOpened -= OnMediaElementMediaOpened;
                m_mediaElement.MediaEnded -= OnMediaElementMediaEnded;
                m_mediaElement.CurrentStateChanged -= OnMediaElementCurrentStateChanged;
                m_mediaElement.MarkerReached -= OnMediaElementMarkerReached;
                m_mediaElement.DownloadProgressChanged -= OnMediaElementDownloadProgressChanged;
                m_mediaElement.MouseLeftButtonDown -= OnMediaElementMouseDown;
                return;
            }
            Debug.Assert(false, "Shouldn't ever be here!");
        }

        private void HookMediaElementEvents()
        {
            Debug.WriteLine("HookMediaElementEvents:");
            if (m_mediaElement != null)
            {
                Debug.WriteLine("HookMediaElementEvents: conv=" + (m_mediaElement == m_mediaElementForConventionalContent).ToString() + " adpt=" + (m_mediaElement == m_mediaElementForSmoothStreamingContent).ToString());
                m_mediaElement.MediaFailed += OnMediaElementMediaFailed;
                m_mediaElement.MediaOpened += OnMediaElementMediaOpened;
                m_mediaElement.MediaEnded += OnMediaElementMediaEnded;
                m_mediaElement.CurrentStateChanged += OnMediaElementCurrentStateChanged;
                m_mediaElement.MarkerReached += OnMediaElementMarkerReached;
                m_mediaElement.DownloadProgressChanged += OnMediaElementDownloadProgressChanged;
                m_mediaElement.MouseLeftButtonDown += OnMediaElementMouseDown;
                return;
            }
            Debug.Assert(false, "Shouldn't ever be here!");
        }

        /// <summary>
        /// clicks a button (or toggle button) programmatically if it is showing.
        /// </summary>
        /// <param name="buttonBase"></param>
        private static void ClickButton(ButtonBase buttonBase, RoutedEventHandler handler)
        {
            if (buttonBase != null && buttonBase.Visibility == Visibility.Visible)
            {
                if (buttonBase is Button)
                {
                    ButtonAutomationPeer buttonAutoPeer = new ButtonAutomationPeer(buttonBase as Button);
                    if (buttonAutoPeer.IsEnabled())
                    {
                        IInvokeProvider invokeProvider = (IInvokeProvider)buttonAutoPeer.GetPattern(PatternInterface.Invoke);
                        invokeProvider.Invoke();
                    }
                }
                else if (buttonBase is ToggleButton)
                {
                    ToggleButtonAutomationPeer buttonAutoPeer = new ToggleButtonAutomationPeer(buttonBase as ToggleButton);
                    if (buttonAutoPeer.IsEnabled())
                    {
                        IToggleProvider toggleProvider = (IToggleProvider)buttonAutoPeer.GetPattern(PatternInterface.Toggle);                    
                        toggleProvider.Toggle();
                        Debug.Assert(handler != null);
                        handler((ToggleButton)buttonBase, null);                        
                    }
                }
                else if (buttonBase is RepeatButton)
                {
                    RepeatButtonAutomationPeer buttonAutoPeer = new RepeatButtonAutomationPeer(buttonBase as RepeatButton);
                    if (buttonAutoPeer.IsEnabled())
                    {
                        IInvokeProvider invokeProvider = (IInvokeProvider)buttonAutoPeer.GetPattern(PatternInterface.Invoke);
                        invokeProvider.Invoke();
                    }
                }
            }
        }

        /// <summary>
        /// handle multimedia keyboard controls
        /// </summary>
        protected virtual void MediaPlayerKeyDown(object sender, KeyEventArgs e)
        {
            // do not intercept key strokes if in an input control
            object focusObject = FocusManager.GetFocusedElement();
            if (focusObject != null)
            {
                if (focusObject.GetType() == typeof(TextBox) ||                    
                    focusObject.GetType() == typeof(ComboBox) ||
                    focusObject.GetType() == typeof(PasswordBox))
                {
                    return;
                }
            }

            switch (e.PlatformKeyCode)
            {
                case 0xB0: //VK_MEDIA_NEXT_TRACK 
                    GoToNextPlaylistItem();
                    break;
                case 0xB1: //VK_MEDIA_PREV_TRACK 
                    GoToPreviousPlaylistItem();
                    break;
                case 0xB2: //VK_MEDIA_STOP
                    OnButtonClickStop(null,null);
                    break;                
                case 0xB3: //VK_MEDIA_PLAY_PAUSE
                    TogglePlayPause();
                    break;                
            }

            switch (e.Key)
            {
                case Key.B:
                    ClickButton(m_buttonPrevious, OnButtonClickPrevious);
                    break;
                case Key.C:
                    ClickButton(m_buttonChapter, OnButtonClickChapter);
                    break;
                case Key.D:
                    ClickButton(m_buttonOffline, OnButtonClickOffline);
                    break;
                case Key.F:
                    ToggleFullScreen();
                    break;
                case Key.G:
                    ClickButton(m_buttonPlugIn, OnButtonClickPlugIn);
                    break;
                case Key.I:
                    ClickButton(m_buttonStepBackwards, OnButtonStepBackwards);
                    break;
                case Key.J:
                    ClickButton(m_buttonVolumeDown, OnButtonClickVolumeDown);
                    break;
                case Key.K:
                    ClickButton(m_buttonVolumeUp, OnButtonClickVolumeUp);
                    break;
                case Key.L:
                    ClickButton(m_buttonClosedCaptions, OnButtonClickClosedCaptions);
                    break;
                case Key.M:
                    ClickButton(m_buttonMute, OnButtonClickMute);
                    break;
                case Key.N:
                    ClickButton(m_buttonNext, OnButtonClickNext);
                    break;
                case Key.O:
                    ClickButton(m_buttonStepForwards, OnButtonStepForwards);
                    break;
                case Key.P:
                    TogglePlayPause();
                    break;
                case Key.S:
                    ClickButton(m_buttonStop, OnButtonClickStop);
                    break;
                case Key.V:
                    ClickButton(m_buttonPlaylist, OnButtonClickPlaylist);
                    break;
                case Key.X:
                    ClickButton(m_buttonPopOut, OnButtonClickPopOut);
                    break;            
                case Key.Home:
                    SeekToStart();
                    break;
                case Key.End:
                    SeekToEnd();
                    break;
            }
        }

        /// <summary>
        /// Unhooks our event handlers.
        /// </summary>
        protected virtual void UnhookHandlers()
        {
            Debug.WriteLine("UnhookHandlers:");

            if (!IsDesignTime)
            {
                UnCreatePositionTimer();
                UnCreateFadeoutTimer();
            }

            this.KeyDown -= new KeyEventHandler(MediaPlayerKeyDown);

            if (m_elementStretchBox != null)
            {
                m_elementStretchBox.MouseMove -= OnStretchBoxMouseMove;
            }

            if (m_buttonStart != null)
            {
                m_buttonStart.Click -= OnButtonClickStart;
            }

            if (m_buttonOffline != null)
            {
                m_buttonOffline.Click -= OnButtonClickOffline;
            }

            if (m_buttonPopOut!= null)
            {
                m_buttonPopOut.Click -= OnButtonClickPopOut;
            }

            if (m_buttonPlugIn != null)
            {
                m_buttonPlugIn.Click -= OnButtonClickPlugIn;
            } 

            if (m_buttonPlayPause != null)
            {
                m_buttonPlayPause.Click -= OnButtonClickPlayPause;
            }

            if (m_buttonStop != null)
            {
                m_buttonStop.Click -= OnButtonClickStop;
            }

            if (m_buttonPrevious != null)
            {
                m_buttonPrevious.Click -= OnButtonClickPrevious;
            }

            if (m_buttonNext != null)
            {
                m_buttonNext.Click -= OnButtonClickNext;
            }

            if (m_buttonMute != null)
            {
                m_buttonMute.Click -= OnButtonClickMute;
            }

            if (m_buttonClosedCaptions != null)
            {
                m_buttonClosedCaptions.Click -= OnButtonClickClosedCaptions;
            }

            if (m_listboxCaptionOptions != null)
            {
                m_listboxCaptionOptions.SelectionChanged -= OnCaptionOptionsSelectedIndexChanged;
            }

            if (m_popupClosedCaptionsOptionsMenu != null)
            {
                m_popupClosedCaptionsOptionsMenu.Closed -= OnClosedCaptionsOptionsMenu_Closed;
            }

            if (m_buttonVolumeDown != null)
            {
                m_buttonVolumeDown.Click -= OnButtonClickVolumeDown;
            }

            if (m_buttonVolumeUp != null)
            {
                m_buttonVolumeUp.Click -= OnButtonClickVolumeUp;
            }

            if (m_buttonFullScreen != null)
            {
                m_buttonFullScreen.Click -= OnButtonClickFullScreen;
            }

            if (m_buttonStepBackwards != null)
            {
                m_buttonStepBackwards.Click -= OnButtonStepBackwards;
            }

            if (m_buttonStepForwards != null)
            {
                m_buttonStepForwards.Click -= OnButtonStepForwards;
            }

            if (m_buttonPlaylist != null)
            {
                m_buttonPlaylist.Click -= OnButtonClickPlaylist;
            }

            if (m_selectorPlaylist != null)
            {
                m_selectorPlaylist.SelectionChanged -= OnListBoxSelectionChangedPlaylist;
            }

            if (m_selectorChapters != null)
            {
                m_selectorChapters.SelectionChanged -= OnListBoxSelectionChangedChapters;
            }

            if (m_buttonChapter != null)
            {
                m_buttonChapter.Click -= OnButtonClickChapter;
            }

            if (m_sliderPosition != null)
            {
                m_sliderPosition.ValueChanged -= OnSliderPositionChanged;
                m_sliderPosition.DragStarted -= OnSliderPositionDragStarted;
                m_sliderPosition.DragCompleted -= OnSliderPositionDragCompleted;
            }

            if (m_sliderVolume != null)
            {
                m_sliderVolume.ValueChanged -= OnSliderVolumeChanged;
            }

            if (m_timer != null)
            {
                m_timer.Stop();
            }

            if (Application.Current != null)
            {
                Application.Current.Host.Content.FullScreenChanged -= OnFullScreenChanged;
                Application.Current.Host.Content.Resized -= OnResized;
                Application.Current.Host.Content.Zoomed -= OnZoomed;
                Application.Current.Exit -= OnAppExit;
            }

            ThumbnailDownloader.DisableThumbnailDownload();
        }

        /// <summary>
        /// Make the conventional media element visible and hooked up to events.
        /// </summary>
        private void ShowConventionalMediaElement()
        {
            if (this.m_mediaElementForSmoothStreamingContent != null)
            {
                switch (this.m_mediaElementForSmoothStreamingContent.CurrentState)
                {
                    case MediaElementState.Stopped:
                    case MediaElementState.Closed:
                        break;
                    default:
                        this.m_mediaElementForSmoothStreamingContent.Stop();
                        break;
                }
                this.m_mediaElementForSmoothStreamingContent.Source = null;
                this.m_mediaElementForSmoothStreamingContent.Visibility = Visibility.Collapsed;
            }
            
            if (this.m_mediaElementForConventionalContent == null)
            {
                this.m_mediaElementForConventionalContent = new MediaElementShimThin(this.m_actualMediaElement);
            }
            if (this.m_mediaElementForConventionalContent != null)
            {
                this.m_mediaElementForConventionalContent.Visibility = Visibility.Visible;
                if (this.m_mediaElement != m_mediaElementForConventionalContent)
                {
                    if (this.m_mediaElement != null)
                    {
                        this.UnhookMediaElementEvents();
                    }
                    if (this.m_mediaElementGrid != null)
                    {
                        this.m_mediaElementGrid.Children.Clear();
                        this.m_mediaElementGrid.Children.Add(this.m_actualMediaElement);
                    }
                    this.m_mediaElement = this.m_mediaElementForConventionalContent;
                    this.HookMediaElementEvents();
                    this.ApplyPropertiesToMediaElement();
                }
            }
        }

        /// <summary>
        /// Initialize convention content playback
        /// </summary>
        /// <param name="playlistItem"></param>
        internal void InitConventionalStreaming(PlaylistItem playlistItem)
        {
            this.ShowConventionalMediaElement();

            if (playlistItem.MediaSource != null)
            {
                if (MediaPlayer.IsOffline)
                {
                    IsoUri offlineIsoUri = MediaPlayer.MakeOfflineIsoUri(playlistItem.MediaSource);
                    if (offlineIsoUri.IsoFileExists)
                    {
                        m_mediaElement.SetSource(offlineIsoUri.Stream);
                    }
                    else
                    {
                        string strErrorMessage = String.Format(CultureInfo.CurrentUICulture, ExpressionMediaPlayer.Resources.errorInvalidMedia, playlistItem.MediaSource.ToString());
                        ShowErrorMessage(strErrorMessage);
                    }
                }
                else
                {
                    IsoUri isoUri = new IsoUri(playlistItem.MediaSource);
                    m_mediaElement.Source = isoUri.FullyQualifiedUri;
                }
            }
        }

        /// <summary>
        /// Configures the binding for the playlist control.
        /// </summary>
        protected void ConfigureBinding()
        {
            if (m_selectorPlaylist != null)
            {
                m_selectorPlaylist.ItemsSource = this.Playlist.Items;
            }
        }

        /// <summary>
        /// Do this once instead of many times
        /// </summary>
        /// <param name="isVisible"></param>
        /// <returns></returns>
        static Visibility Bool2Visibility(bool isVisible)
        {
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Applies our cached properties.
        /// </summary>
        protected virtual void ApplyProperties()
        {
            Stop();

            SetPopOutButtonVisibility(Bool2Visibility(this.IsPopOutAllowed));

            bool notInstalled = Application.Current.InstallState==InstallState.NotInstalled;
            bool allowOffline = Playlist.EnableOffline && notInstalled;
            SetOfflineButtonVisibility(Bool2Visibility(allowOffline));            
            SetChaptersButtonVisibility(Visibility.Collapsed);
            SetPlugInButtonVisibility(Visibility.Collapsed);
            if (Playlist != null)
            {
                Mute = Playlist.StartMuted;

                if (Playlist.Items != null)
                {
                    bool moreThanOne = Playlist.Items.Count > 1;

                    SetPlaylistButtonVisibility(moreThanOne ? Visibility.Visible : Visibility.Collapsed);
                    if (!moreThanOne)
                    {
                        ShowPlaylist(false);
                    }
                    else if (Playlist.StartWithPlaylistShowing)
                    {
                        ShowPlaylist(true);
                    }

                    if (Playlist.Items.Count > 0)
                    {
                        if (Playlist.AutoLoad || Playlist.AutoPlay)
                        {
                            GoToPlaylistItemOnNextTick(0);
                        }
                        else
                        {
                            DisplayPoster(0);
                        }
                    }
                }                   
            }

            if (m_buttonStart != null)
            {
                m_buttonStart.Visibility = Playlist.AutoPlay ? Visibility.Collapsed : Visibility.Visible;
            }

            if (m_sliderVolume != null)
            {
                m_sliderVolume.Minimum = 0;
                m_sliderVolume.Maximum = 1;
                m_sliderVolume.SmallChange = 0.1;
                m_sliderVolume.LargeChange = 0.2;

                m_volumeCacheSuppressLevel++;
                if (m_mutedCache)
                {
                    m_sliderVolume.Value = 0.0;
                }
                else
                {
                    m_sliderVolume.Value = m_dblUnMutedVolume;
                }
                m_volumeCacheSuppressLevel--;
            }

            if (m_buttonMute != null)
            {
                m_buttonMute.IsChecked = m_mutedCache;
            }

            SetClosedCaptionButton(Playlist.EnableCaptions);

            if (m_mediaElement != null)
            {
                ApplyPropertiesToMediaElement();
            }
            PerformResize();
        }

        private void ApplyPropertiesToMediaElement()
        {
            Debug.Assert(m_mediaElement != null);
            if (m_mediaElement != null)
            {
                m_mediaElement.CacheMode = (Playlist.EnableCachedComposition && (m_mediaElement.CacheMode == null)) ? new BitmapCache() : null;

                if (Playlist != null)
                {
                    m_mediaElement.AutoPlay = Playlist.AutoPlay;
                }

                if ( m_sliderVolume != null )
                {
                    m_mediaElement.Volume = m_sliderVolume.Value;
                }
            }
        }

        /// <summary>
        /// Adjust the size of the poster image to match the displayed video .
        /// </summary>
        protected virtual void AdjustPosterSize(int playlistItemIndex)
        {
            Debug.WriteLine("AdjustPosterSize: " + playlistItemIndex.ToString());
            if ((Playlist.StretchNonSquarePixels == StretchNonSquarePixels.NoStretch) 
                && (!Application.Current.Host.Content.IsFullScreen)
                && (playlistItemIndex >= 0) 
                && (playlistItemIndex < Playlist.Items.Count)
                && (Playlist.Items[playlistItemIndex].VideoWidth > 0)
                && (Playlist.Items[playlistItemIndex].VideoHeight > 0))
            {
                var mw = Playlist.Items[playlistItemIndex].VideoWidth;
                var mh = Playlist.Items[playlistItemIndex].VideoHeight;
                Debug.WriteLine("AdjustPosterSize: mw=" + mw.ToString() + " mh=" + mh.ToString());
                SetPosterImageMaxWidth(mw);
                SetPosterImageMaxHeight(mh);
            }
            else
            {
                Debug.WriteLine("AdjustPosterSize: Infinite!");
                SetPosterImageMaxWidth(Double.PositiveInfinity);
                SetPosterImageMaxHeight(Double.PositiveInfinity);
            }            
        }

        /// <summary>
        /// Displays a poster image for a playlist item.
        /// </summary>
        /// <param name="playlistItemIndex">Index of the item to display a poster image for.</param>
        protected virtual void DisplayPoster(int playlistItemIndex)
        {
            Debug.WriteLine("DisplayPoster: " + playlistItemIndex.ToString());
            if (playlistItemIndex >= 0 && playlistItemIndex < Playlist.Items.Count)
            {
                if (Playlist.Items[playlistItemIndex].ThumbSource != null)
                {
                    SetPosterImageSource(Playlist.Items[playlistItemIndex].ThumbSource.ToString());
                }
                else
                {
                    SetPosterImageSource(string.Empty);
                }
                AdjustPosterSize(playlistItemIndex);
                Debug.WriteLine("DisplayPoster: showPosterFrame");
                VisualStateManager.GoToState(this, "showPosterFrame", true);
                return;
            }
            Debug.WriteLine("DisplayPoster: hidePosterFrame");
            VisualStateManager.GoToState(this, "hidePosterFrame", true);                
        }

        /// <summary>
        /// Seeks to the given time.
        /// </summary>
        /// <param name="seconds">Time to seek to.</param>
        [ScriptableMember]
        public virtual void SeekToTime(double seconds)
        {
            // collapse / defer seeks
            m_seekOnNextTick = true;
            m_seekOnNextTickPosition = seconds;
        }
       
        /// <summary>
        /// Performs the actual seek.
        /// </summary>
        protected virtual void DoActualSeek()
        {            
            if (!m_mediaElement.CanSeek || !m_seekOnNextTick)
            {
                m_seekOnNextTick = false;
                return;
            }

            // Don't attempt to seek unless the element is actually playing or paused
            switch (m_mediaElement.CurrentState)
            {
                case MediaElementState.Playing:
                case MediaElementState.Paused:
                    break;
                case MediaElementState.Opening:
                case MediaElementState.Buffering:
                case MediaElementState.AcquiringLicense:
                case MediaElementState.Stopped:
                case MediaElementState.Closed:
                default:
                    // Defering while media isn't ready...
                    return;
            }

            if (m_sliderPosition != null)
            {
                if (m_sliderPosition.IsDragging && m_currentItemIsAdaptive)
                {
                    // Defering Seek during Drag
                    return;
                }
            }

            // Finaly go ahead and seek!
            ClearCaptionText();
            m_seekOnNextTick = false;
            double seconds = Math.Min(PlaybackDuration, Math.Max(0.0, m_seekOnNextTickPosition));

            TimeSpan newPosition = TimeSpan.FromSeconds(seconds);
            m_mediaElement.Position = newPosition;

            // update chapter index (listbox selection will be updated in the TimerTick code)
            m_currentChapterIndex = ChapterIndexFromPosition(newPosition);
        }

        /// <summary>
        /// Skips forwards or backwards.
        /// </summary>
        /// <param name="direction">Direction to skip.</param>
        protected virtual void SkipTime(int direction)
        {
            double delta = Math.Max(MinDelta, PlaybackDuration / SkipSteps);
            double skipbuffer = (delta - SkipBuffer);
            double newposition = PlaybackPosition + (delta * direction);
            if (newposition < -(skipbuffer) && m_currentPlaylistIndex > 0)
            {
                GoToPreviousPlaylistItem();
            }
            else if ((newposition > (PlaybackDuration + skipbuffer)) && (m_currentPlaylistIndex < (Playlist.Items.Count-1)))
            {
                GoToNextPlaylistItem();
            }
            else
            {
                newposition = Math.Max(0.0, newposition);
                newposition = Math.Min(PlaybackDuration,  newposition);
                SeekToTime(newposition);
            }
        }

        /// <summary>
        /// Seeks to the next playlist item.
        /// </summary>
        [ScriptableMember]
        public virtual void SeekToNextItem()
        {
            if (!SeekToChapterPoint(m_currentChapterIndex + 1))
            {
                SkipTime(1);
            }
        }

        /// <summary>
        /// Seeks to the previous playlist item.
        /// </summary>
        [ScriptableMember]
        public virtual void SeekToPreviousItem()
        {
            if (!SeekToChapterPoint(m_currentChapterIndex - 1))
            {
                SkipTime(-1);
            }
        }

        /// <summary>
        /// Finds a chapter index from a position.
        /// </summary>
        /// <param name="position">The position we are looking for.</param>
        /// <returns>The index of the chapter item for this position.</returns>
        protected int ChapterIndexFromPosition(TimeSpan position)
        {
            double seconds = position.TotalSeconds;

            int indexChapter = 0;
            if ((m_currentPlaylistIndex >= 0) && (m_currentPlaylistIndex < Playlist.Items.Count))
            {
                while (indexChapter < Playlist.Items[m_currentPlaylistIndex].Chapters.Count && Playlist.Items[m_currentPlaylistIndex].Chapters[indexChapter].Position < seconds)
                {
                    indexChapter++;
                }
            }

            return indexChapter;
        }

        /// <summary>
        /// Seeks to a chapter point.
        /// </summary>
        /// <param name="chapterIndex">The index of the chapter point to seek to.</param>
        /// <returns>true if we found the index, false otherwise.</returns>
        [ScriptableMember]
        public virtual bool SeekToChapterPoint(int chapterIndex)
        {
            if ((m_currentPlaylistIndex >= 0) && (m_currentPlaylistIndex < Playlist.Items.Count))
            {
                if ((chapterIndex >= 0) && (chapterIndex < Playlist.Items[m_currentPlaylistIndex].Chapters.Count))
                {
                    m_currentChapterIndex = chapterIndex;
                    m_selectorChapters.SelectedIndex = m_currentChapterIndex;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Toggles our play/pause state.
        /// </summary>
        protected virtual void TogglePlayPause()
        {
            if (m_mediaElement == null)
            {
                // Special case with some audio-only templates that lack the big start button -- m_mediaElement will be null until after the first item has been played
                GoToPlaylistItemOnNextTick(0);
                return;
            }

            // Change current play state depending on current state.

            switch (m_mediaElement.CurrentState)
            {
                case MediaElementState.AcquiringLicense:
                case MediaElementState.Buffering:
                case MediaElementState.Individualizing:
                case MediaElementState.Playing:
                case MediaElementState.Opening:
                    InternalPause();
                    FixupFrameStep();
                    break;
                case MediaElementState.Paused:
                case MediaElementState.Closed:
                case MediaElementState.Stopped:
                    m_inPlayState = true;
                    InternalPlay();
                    break;
            }
        }


        ///
        /// CAPTIONING SUPPORT
        /// 

        /// <summary>
        /// Click handler for the closed captions button.
        /// </summary>
        /// <param name="sender">Source of this event.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonClickClosedCaptions(object sender, RoutedEventArgs e)
        {
            if (m_popupClosedCaptionsOptionsMenu != null)
            {
                if (m_popupClosedCaptionsOptionsMenu.IsOpen)
                {
                    // Popup open -- close it 
                    m_popupClosedCaptionsOptionsMenu.IsOpen = false;
                }
            }

            if (EnableCaptionsArea())
            {
                if (this.CaptionsManager != null)
                {
                    // Get the set of language choices for the current playlist item.
                    var captionOptions = this.CaptionOptions;
                    if (captionOptions != null)
                    {
                        // More than one language choice -- display language selection popup menu
                        if (captionOptions.Count > 1)
                        {
                            if (m_listboxCaptionOptions != null)
                            {
                                // clear any prior selection -- since it is the selection change event that closed the popup menu.
                                m_listboxCaptionOptions.SelectedItem = null;
                            }
                            if ((m_popupClosedCaptionsOptionsMenu != null) && (m_buttonClosedCaptions != null))
                            {
                                // Show the popup menu -- but make it transparent -- the popup opened handler will reposition the menu and make it visuble
                                this.repositionCaptionPopup = true;
                                this.m_popupClosedCaptionsOptionsMenu.HorizontalOffset = -1000;
                                this.m_popupClosedCaptionsOptionsMenu.VerticalOffset = -1000;
                                this.m_popupClosedCaptionsOptionsMenu.Opacity = 0.0;
                                this.m_popupClosedCaptionsOptionsMenu.IsOpen = true;
                                return;
                            }
                        }
                        else
                        {
                            // there is only one language choice -- choose it.
                            foreach (var captionOption in captionOptions)
                            {
                                SetCaptionOptionData(captionOption.Value.LanguageIdTwoLetterIso, captionOption.Value);
                                return;
                            }
                        }
                    }
                }
            }
            // If we got here -- the current playlit item has no caption data or something wierd is going on -- clear out any lurking caption data
            SetCaptionOptionData(string.Empty, null);
        }

        /// <summary>
        /// Download the supplied language version of the captions
        /// </summary>
        /// <param name="language">language to download</param>
        private void SetCaptionOptionData(string languageIdTwoLetterIso, CaptionOption captionOption)
        {
            if (this.CurrentMediaElement != null &&
                Playlist.Items.Count > 0 &&
                m_currentPlaylistIndex >= 0 &&
                m_currentPlaylistIndex < Playlist.Items.Count &&
                Playlist.Items[m_currentPlaylistIndex] != null &&
                Playlist.Items[m_currentPlaylistIndex].CaptionSources != null &&
                this.CaptionsManager != null)
            {


                this.CaptionsManager.ClearDFXPEvents();

                if (languageIdTwoLetterIso == string.Empty || captionOption == null)
                {                    
                    this.CaptionsManager.RemoveCaptionPanel();
                }
                else
                {
                    this.CaptionsManager.EnsureCaptionPanel();
                    CaptionSource captionSourceChosen = null;

                    foreach (CaptionSource captionSource in Playlist.Items[m_currentPlaylistIndex].CaptionSources)
                    {
                        if ((String.CompareOrdinal(captionSource.ISOTwoLetterLanguageName, captionOption.LanguageIdTwoLetterIso) == 0) &&
                            (captionSource.Type == captionOption.Type))
                        {
                            captionSourceChosen = captionSource;
                            break;
                        }
                    }

                    if (captionSourceChosen != null)
                    {
                        this.CaptionsManager.DownloadCaptions(Playlist.Items[m_currentPlaylistIndex].IsAdaptiveStreaming, captionSourceChosen);
                    }
                }
            }
        }

        /// <summary>
        /// Checks / Unchecks the ClosedCaptions button
        /// </summary>
        /// <param name="enableCaptions"></param>
        private void SetClosedCaptionButton(bool enableCaptions)
        {
            if (m_buttonClosedCaptions != null)
            {
                m_buttonClosedCaptions.IsChecked = !enableCaptions;
            }
        }

        /// <summary>
        /// initialize DXFP captions
        /// </summary>
        private bool EnableCaptionsArea()
        {
            bool enableCaptionsArea = false;
            if (this.m_buttonClosedCaptions != null)
            {
                enableCaptionsArea = (this.m_buttonClosedCaptions.IsChecked == false);
            }
            this.CaptionsVisibility = Bool2Visibility(enableCaptionsArea);
            return enableCaptionsArea;
        }

        /// <summary>
        /// Handler called when the langauge menu selection is changed -- in this case it simply causes the popup to close
        /// The actual handling of the language change is dealt with in the popup closed handler -- since there are other ways to close the popup
        /// and language needs to be set in those cases as well.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnCaptionOptionsSelectedIndexChanged(object sender, EventArgs e)
        {
            m_popupClosedCaptionsOptionsMenu.IsOpen = false;  // "Auto" close the popup after they make a selection
        }

        private bool repositionCaptionPopup = false;
        private void RepositionCaptionPopup()
        {
            if (m_popupClosedCaptionsOptionsMenu == null)
            {
                return;
            }
            if (!m_popupClosedCaptionsOptionsMenu.IsOpen)
            {
                m_popupClosedCaptionsOptionsMenu.Opacity = 0.0;
                return;
            }

            if (!this.repositionCaptionPopup)
            {
                return;
            }

            Debug.WriteLine("RepositionCaptionPopup IsOpen=" + m_popupClosedCaptionsOptionsMenu.IsOpen.ToString() + " Opacity=" + m_popupClosedCaptionsOptionsMenu.Opacity.ToString() + " HOffset=" + m_popupClosedCaptionsOptionsMenu.HorizontalOffset.ToString() + " VOffest=" + m_popupClosedCaptionsOptionsMenu.VerticalOffset.ToString());

            FrameworkElement parentWindow = m_elementVideoWindow;
            if (null == parentWindow)
            {
                FrameworkElement parent = m_popupClosedCaptionsOptionsMenu.Parent as FrameworkElement;
                while (parent != null)
                {
                    parentWindow = parent;
                    parent = parent.Parent as FrameworkElement;

                    if (parentWindow.GetType() == StretchBox.GetType())
                    {
                        break;
                    }
                }
            }
            Debug.Assert(parentWindow != null, "XAML wierdness in this template");
            if (parentWindow != null)
            {
                this.repositionCaptionPopup = false;
                // reposition the popup near the closed captions button
                var transform = m_buttonClosedCaptions.TransformToVisual(parentWindow);
                var point = transform.Transform(new Point(0, 0));

                double menuWidth = m_listboxCaptionOptions.ActualWidth;
                double menuHeight = m_listboxCaptionOptions.ActualHeight;

                point.X = Math.Max(0, point.X);
                point.X = Math.Min((parentWindow.ActualWidth - menuWidth), point.X);
                m_popupClosedCaptionsOptionsMenu.HorizontalOffset = point.X;

                point.Y = Math.Max(0, point.Y);
                point.Y = Math.Min((parentWindow.ActualHeight - menuHeight), point.Y);
                m_popupClosedCaptionsOptionsMenu.VerticalOffset = point.Y;

                m_popupClosedCaptionsOptionsMenu.Opacity = 1.0;

                Debug.WriteLine("RepositionCaptionPopup completed IsOpen=" + m_popupClosedCaptionsOptionsMenu.IsOpen.ToString() + " Opacity=" + m_popupClosedCaptionsOptionsMenu.Opacity.ToString() + " HOffset=" + m_popupClosedCaptionsOptionsMenu.HorizontalOffset.ToString() + " VOffest=" + m_popupClosedCaptionsOptionsMenu.VerticalOffset.ToString());

            }
        }

        /// <summary>
        /// Handler for when the language selection popup is closed -- updates the captions to the selected language 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnClosedCaptionsOptionsMenu_Closed(object sender, EventArgs e)
        {
            EnableCaptionsArea();
            UpdateCaptionOptions(this.m_listboxCaptionOptions);
        }

        /// <summary>
        /// helper function that actually effects the downloading and parsing of a captions file matching the current language settings for the current playlist item.
        /// </summary>
        private void UpdateCaptionData()
        {
            if ((this.CaptionsManager != null) && (this.CaptionsVisibility == System.Windows.Visibility.Visible))
            {
                // If there is a currently selected caption option -- load it's data
                SetCaptionOptionData(this.CaptionsManager.CurrentISOTwoLetterLanguageName, this.CaptionsManager.CaptionOptionForPlaylistItemAndLanguage(this.m_currentPlaylistIndex, this.CaptionsManager.CurrentISOTwoLetterLanguageName));
            }
            else
            {
                SetCaptionOptionData(string.Empty, null); // Clears out captions -- removes events from media element
            }
        }

        /// <summary>
        /// Helper method for updating the selected language settting 
        /// </summary>
        /// <param name="listbox"></param>
        private void UpdateCaptionOptions(ListBox listbox)
        {
            if ((listbox != null) && (this.CaptionsManager != null))
            {
                if (this.CaptionsVisibility == System.Windows.Visibility.Visible)
                {
                    KeyValuePair<string, CaptionOption>? selectedItem = listbox.SelectedItem as KeyValuePair<string, CaptionOption>?;
                    if (selectedItem != null)
                    {
                        CaptionOption captionOption = selectedItem.Value.Value;
                        if (captionOption != null)
                        {
                            this.CaptionsManager.CurrentISOTwoLetterLanguageName = captionOption.LanguageIdTwoLetterIso;
                            UpdateCaptionData();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// scripting utility function
        /// </summary>
        [ScriptableMember]
        public Uri CreateUri(string uri)
        {
            return new Uri(uri, UriKind.RelativeOrAbsolute);
        }

        #endregion

        #region PrivateUtilityMethods

        /// <summary>
        /// Goes to our control state.
        /// </summary>
        /// <param name="controlState">Control state to go to.</param>
        private void GoToControlState(String controlState)
        {
            m_timerControlFadeOut.Stop();
            VisualStateManager.GoToState(this, controlState, true);
            currentControlState = controlState;
            FrameworkElement element = Application.Current.RootVisual as FrameworkElement;
            if (element != null)
            {
                if (EnterFullScreen == currentControlState)
                {
                    element.Cursor = Cursors.None;
                }
                else if (ExitFullScreen == currentControlState)
                {
                    element.Cursor = Cursors.Arrow;
                }
            }

            m_timerControlFadeOut.Start();
        }

        /// <summary>
        /// Sets the desired control state.
        /// </summary>
        private void SetDesiredControlState()
        {
            if ((Application.Current != null))
            {
                if (Application.Current.Host.Content.IsFullScreen)
                {
                    desiredControlState = EnterFullScreen;
                }
                else
                {
                    desiredControlState = ExitFullScreen;
                }
            }
        }

        /// <summary>
        /// Performs a resize.
        /// </summary>
        private void PerformResize()
        {
            if ((Application.Current != null))
            {
                if (Application.Current.Host.Content.IsFullScreen)
                {
                    this.HorizontalAlignment = HorizontalAlignment.Stretch;
                    this.VerticalAlignment = VerticalAlignment.Stretch;
                }
            }

            SetDesiredControlState();

            AdjustPosterSize(((m_currentPlaylistIndex >= 0) ? (m_currentPlaylistIndex) : (0)));// special case for adjusting poster size between normal and full screen mode prior to playing the 1st item.

            ApplySizeAndStretch();

            this.m_refreshCaptionsOnNextTick = true;
        }

        /// <summary>
        /// Apply size and stretch mode to the MediaElement
        /// </summary>
        private void ApplySizeAndStretch()
        {
            if (Playlist == null || m_mediaElement == null)
            {
                // Skip when called during boot up
                return;
            }

            StretchNonSquarePixels currentMode = this.Playlist.StretchNonSquarePixels;
#if DEBUG_SIZE_AND_STRETCH
            Debug.WriteLine("ApplySizeAndStretch currentMode=" + currentMode.ToString());
#endif

            if (Application.Current.Host.Content.IsFullScreen)
            {
                if (currentMode == StretchNonSquarePixels.NoStretch) // override stretchMode for FullScreen if they have none.
                {
                    currentMode = StretchNonSquarePixels.StretchToFill;

#if DEBUG_SIZE_AND_STRETCH
                    Debug.WriteLine("IsFullScreen currentMode=" + currentMode.ToString());
#endif
                }
            }

            // For "stretchy modes" -- just let mediaElement do it's thing
            if (StretchNonSquarePixels.StretchDistorted == currentMode)
            {
                m_mediaElement.Width = double.NaN;
                m_mediaElement.Height = double.NaN;
                m_mediaElement.Stretch = Stretch.Fill;

#if DEBUG_SIZE_AND_STRETCH
                Debug.WriteLine("ApplySizeAndStretch -- StretchDistorted shortcut!");
#endif
                return;
            }

            double widthToSet = double.NaN; // Auto sizing
            double heightToSet = double.NaN; 
            Stretch mediaElementStretchMode = Stretch.Fill;

            double squarePixelVideoWidth = 320;
            double squarePixelVideoHeight = 240;

            if ((m_currentPlaylistIndex >= 0) && (m_currentPlaylistIndex < Playlist.Items.Count))
            {
                var currentItem = Playlist.Items[m_currentPlaylistIndex];

                if ((currentItem.VideoWidth > 0)
                && (currentItem.VideoHeight > 0)
                && (currentItem.VideoHeight < short.MaxValue)
                && (currentItem.VideoHeight < short.MaxValue))
                {
                    double pixelWidth = currentItem.VideoWidth;
                    double pixelHeight = currentItem.VideoHeight;

#if DEBUG_SIZE_AND_STRETCH
                    Debug.WriteLine("pixelWidth=" + pixelWidth.ToString());
                    Debug.WriteLine("pixelHeight=" + pixelHeight.ToString());
#endif

                    double aspectRatioWidth = currentItem.AspectRatioWidth;
                    double aspectRatioHeight = currentItem.AspectRatioHeight;
                    if (double.IsNaN(aspectRatioWidth) || double.IsNaN(aspectRatioHeight))
                    {
                        double pixelRatio = 4/3;
                        if ( pixelHeight > 0.0 )
                        {
                            pixelRatio = pixelWidth / pixelHeight;
                        }
                        if (pixelRatio > 720 / 480)
                        {
                            aspectRatioWidth = 16;
                            aspectRatioHeight = 9;
                        }
                        else
                        {
                            aspectRatioWidth = 4;
                            aspectRatioHeight = 3;
                        }
                    }

#if DEBUG_SIZE_AND_STRETCH
                    Debug.WriteLine("aspectRatioWidth=" + aspectRatioWidth.ToString());
                    Debug.WriteLine("aspectRatioHeight=" + aspectRatioHeight.ToString());
#endif

                    if (aspectRatioWidth > aspectRatioHeight)
                    {
                        double squarePixelWidth = (pixelHeight * aspectRatioWidth) / aspectRatioHeight;

                        squarePixelVideoWidth = squarePixelWidth;
                        squarePixelVideoHeight = pixelHeight;
                    }
                    else
                    {
                        double squarePixelHeight = (pixelWidth * aspectRatioHeight) / aspectRatioWidth;

                        squarePixelVideoWidth = pixelWidth;
                        squarePixelVideoHeight = squarePixelHeight;
                    }
                }
            }

#if DEBUG_SIZE_AND_STRETCH
            Debug.WriteLine("squarePixelVideoWidth=" + squarePixelVideoWidth.ToString());
            Debug.WriteLine("squarePixelVideoHeight=" + squarePixelVideoHeight.ToString());
#endif

            double scalingFactor = 1.0;

            if (m_elementVideoWindow != null)
            {
                double availibleWidth = m_elementVideoWindow.ActualWidth;
                double availibleHeight = m_elementVideoWindow.ActualHeight;

                if (Application.Current.Host.Content.IsFullScreen)
                {
                    availibleWidth = Application.Current.Host.Content.ActualWidth;
                    availibleHeight = Application.Current.Host.Content.ActualHeight;
                }

#if DEBUG_SIZE_AND_STRETCH
                Debug.WriteLine("availibleWidth=" + availibleWidth.ToString());
                Debug.WriteLine("availibleHeight=" + availibleHeight.ToString());
#endif

                Double scalingFactorX = availibleWidth / squarePixelVideoWidth;
                Double scalingFactorY = availibleHeight / squarePixelVideoHeight;
                scalingFactor = Math.Min(scalingFactorX, scalingFactorY);
            }
            else
            {
#if DEBUG_SIZE_AND_STRETCH
                Debug.WriteLine("elementVideoWindow is missing from the template XAML!!!");
#endif
            }

            if (scalingFactor < 1.0)
            {
                widthToSet = squarePixelVideoWidth * scalingFactor;
                heightToSet = squarePixelVideoHeight * scalingFactor;
            }
            else if (StretchNonSquarePixels.StretchToFill == currentMode)
            {

                widthToSet = squarePixelVideoWidth * scalingFactor;
                heightToSet = squarePixelVideoHeight * scalingFactor;
            }
            else if (StretchNonSquarePixels.NoStretch == currentMode)
            {
                widthToSet = squarePixelVideoWidth;
                heightToSet = squarePixelVideoHeight;
            }

#if DEBUG_SIZE_AND_STRETCH
            Debug.WriteLine("ApplySizeAndStretch widthToSet=" + widthToSet.ToString() + " heightToSet=" + heightToSet.ToString() + " Stretch=" + mediaElementStretchMode.ToString());
#endif

            m_mediaElement.Width = widthToSet;
            m_mediaElement.Height = heightToSet;
            m_mediaElement.Stretch = mediaElementStretchMode;
        }

        /// <summary>
        /// Clears our caption text.
        /// </summary>
        private void ClearCaptionText()
        {
            SetCaptionText(string.Empty);
            if (this.CaptionsManager != null)
            {
                this.CaptionsManager.ClearCaptions();
            }
        }

        /// <summary>
        /// Updates the frame step property.
        /// </summary>
        private void UpdateCanStep()
        {
            if (IsDesignTime || 
                (  (m_mediaElement != null)
                && (m_mediaElement.CanSeek)
                && (m_currentPlaylistIndex >= 0)
                && (m_currentPlaylistIndex < Playlist.Items.Count)
                && (Playlist.Items[m_currentPlaylistIndex].SmpteFrameRate != SmpteFrameRate.Unknown)
                && (!Playlist.Items[m_currentPlaylistIndex].IsAdaptiveStreaming)) ) //stepping isn't compatible with AdaptiveStreaming MSS currently
            {
                SetValue(FrameStepVisibilityProperty, Visibility.Visible);
                return;
            }

            SetValue(FrameStepVisibilityProperty, Visibility.Collapsed);
        }

        /// <summary>
        /// Updates the ButtonPreviousIsEnabled and ButtonNextIsEnabled properties.
        /// </summary>
        private void UpdatePrevNext()
        {
            if (m_currentPlaylistIndex >= 0 && m_currentPlaylistIndex < Playlist.Items.Count)
            {
                int chapterCount = Playlist.Items[m_currentPlaylistIndex].Chapters.Count;
                if (chapterCount > 0)
                {
                    SetValue(ButtonPreviousIsEnabledProperty, m_currentChapterIndex > 0);
                    SetValue(ButtonNextIsEnabledProperty, m_currentChapterIndex < (chapterCount - 1));
                }
                else
                {
                    SetValue(ButtonPreviousIsEnabledProperty, PlaybackPosition > 0.0);
                    SetValue(ButtonNextIsEnabledProperty, PlaybackPosition < PlaybackDuration);
                }
            }
            else
            {
                SetValue(ButtonPreviousIsEnabledProperty, false);
                SetValue(ButtonNextIsEnabledProperty, false);
            }
        }

        #endregion

        #region Offline Support
        /// <summary>
        /// offline/online state change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerInstallStateChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("PlayerInstallStateChanged InstallState=" + Application.Current.InstallState.ToString());
            if (!IsDesignTime)
            {
                Stop();
                switch (Application.Current.InstallState)
                {
                    case InstallState.Installed:
                        this.SetOfflineButtonVisibility(Visibility.Collapsed);
                        this.SetOfflineButtonEnabled(false);
                        break;
                    case InstallState.Installing:
                        this.SetOfflineButtonEnabled(false);
                        break;
                    case InstallState.InstallFailed:
                    case InstallState.NotInstalled:
                        IsoUri.ClearIsoStorage();
                        this.SetOfflineButtonVisibility(Visibility.Visible);
                        this.SetOfflineButtonEnabled(Playlist.EnableOffline);
                        break;
                }
            }
        }

        private void EnqueueTakeContentOffline()
        {
            Stop();
            SetOfflineDownloadProgressVisibility(Visibility.Visible);
            SetOfflineButtonEnabled(false);
            Playlist.TakeOffline(this);
        }

        /// <summary>
        /// network went on/off line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerNetworkAddressChanged(object sender, EventArgs e)
        {
            if (!NetworkInterface.GetIsNetworkAvailable() && Playlist.IsDownloading)
            {
                IsoUri.ClearIsoStorage();
                ShowErrorMessage(ExpressionMediaPlayer.Resources.errorOfflineInterrupted);
            }
        }
        #endregion

        /// <summary>
        /// This class contains constants for progress reporting.
        /// </summary>
        private sealed class ProgressConst
        {
            /// <summary>
            /// Maximum progress in the control.
            /// </summary>
            public const double MaxProgress = 1.0;

            /// <summary>
            /// Maximum progress percent.
            /// </summary>
            public const double MaxPercent = 100.0;

            /// <summary>
            /// Converts a progress to a percent.
            /// </summary>
            public const double Progress2Percent = 100.0;

            /// <summary>
            /// Prevents a default instance of the ProgressConst class from being created.
            /// </summary>
            private ProgressConst()
            {
            }
        }
    }
}

