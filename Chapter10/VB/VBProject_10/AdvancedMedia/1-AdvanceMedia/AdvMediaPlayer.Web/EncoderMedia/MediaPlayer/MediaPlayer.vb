'  <copyright file="MediaPlayer.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the MediaPlayer class</summary>
'  <author>Microsoft Expression Encoder Team</author>
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Globalization
Imports System.Windows
Imports System.Windows.Browser
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Threading
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Net.NetworkInformation
Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider
Imports Microsoft.Expression.Encoder.PlugInMssCtrl
Imports System.Collections.Generic
Imports Microsoft.VisualBasic

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' This class represents the base class for a MediaPlayer control.
    ''' </summary>
    <TemplatePart(Name:=MediaPlayer.StretchBox, Type:=typeofCType()>, FrameworkElement)
    <TemplatePart(Name:=MediaPlayer.VideoWindow, Type:=typeof(Panel))>
    <TemplatePart(Name:=MediaPlayer.MediaElement, Type := typeofCType()>, MediaElement)
    <TemplatePart(Name:=MediaPlayer.MediaElementGrid, Type := typeofCType()>, Grid)
    <TemplatePart(Name:=MediaPlayer.StartButton, Type := typeof(ButtonBase))>
    <TemplatePart(Name:=MediaPlayer.PlayPauseButton, Type := typeof(ToggleButton))>
    <TemplatePart(Name:=MediaPlayer.PreviousButton, Type := typeof(ButtonBase))>
    <TemplatePart(Name:=MediaPlayer.NextButton, Type := typeof(ButtonBase))>
    <TemplatePart(Name:=MediaPlayer.StopButton, Type := typeof(ButtonBase))>
    <TemplatePart(Name:=MediaPlayer.FullScreenButton, Type := typeof(ButtonBase))>
    <TemplatePart(Name:=MediaPlayer.OfflineButton, Type := typeof(ButtonBase))>
    <TemplatePart(Name:=MediaPlayer.PopOutButton, Type := typeof(ButtonBase))>
    <TemplatePart(Name:=MediaPlayer.PlugInButton, Type := typeof(ToggleButton))>
    <TemplatePart(Name:=MediaPlayer.MuteButton, Type := typeof(ToggleButton))>
    <TemplatePart(Name:=MediaPlayer.ClosedCaptionButton, Type := typeof(ToggleButton))>
    <TemplatePart(Name:=MediaPlayer.ClosedCaptionsOptionsMenu, Type := typeofCType()>, Popup)
    <TemplatePart(Name:=MediaPlayer.ClosedCaptionsOptionsList, Type := typeofCType()>, ListBox)
    <TemplatePart(Name:=MediaPlayer.VolumeDownButton, Type := typeof(ButtonBase))>
    <TemplatePart(Name:=MediaPlayer.VolumeUpButton, Type := typeof(ButtonBase))>
    <TemplatePart(Name:=MediaPlayer.PlaylistButton, Type := typeof(ToggleButton))>
    <TemplatePart(Name:=MediaPlayer.ChapterButton, Type := typeof(ToggleButton))>
    <TemplatePart(Name:=MediaPlayer.PositionSlider, Type := typeof(SensitiveSlider))>
    <TemplatePart(Name:=MediaPlayer.VolumeSlider, Type := typeof(SensitiveSlider))>
    <TemplatePart(Name:=MediaPlayer.PlaylistSelector, Type := typeof(Selector))>
    <TemplatePart(Name:=MediaPlayer.ChaptersSelector, Type := typeof(Selector))>
    <TemplatePart(Name:=MediaPlayer.ErrorMessageElement, Type := typeofCType()>, FrameworkElement)
    <TemplatePart(Name:=MediaPlayer.ClosedCaptionArea, Type := typeofCType()>, Grid)
    <TemplatePart(Name:=MediaPlayer.GridPlugIn, Type := typeofCType()>, Grid)
    Public Partial Class MediaPlayer
        Inherits Control
        #region DP definitions
        ''' <summary>
        ''' Using a DependencyProperty as the backing store for Playlist.  This enables animation, styling, binding, etc...
        ''' </summary>
        Public Property readonly() As
            Get
                return (Playlist)GetValue(PlaylistProperty)
            End Get

            Set

                SetValue(PlaylistProperty, value)

                If (Me.CaptionsManager  IsNot Nothing) Then
                    Me.CaptionsManager.BuildCaptionsListForPlaylist(Me.Playlist)
                End If
            End Set
        End Property


        ''' <summary>
        ''' Gets the index of the item currently selected.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        <ScriptableMember>
        Public ReadOnly Property CurrentPlaylistIndex() As Integer
            Get
                return m_currentPlaylistIndex
            End Get
        End Property

        ''' <summary>
        ''' Gets the current PlaylistItem selected.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        <ScriptableMember>
        public PlaylistItem CurrentPlaylistItem
        {
            get
            {

                If (m_currentPlaylistIndex >= 0  AndAlso  m_currentPlaylistIndex < Playlist.Items.Count) Then
                    return Playlist.Items(m_currentPlaylistIndex)
                End If


                return Nothing
            }
        }

        ''' <summary>
        ''' Gets or sets the current playback position.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        <ScriptableMember>
        public Double PlaybackPosition
        {
            get
            {
                return (Double)GetValue(PlaybackPositionProperty)
            }
        }

        ''' <summary>
        ''' Gets the text of the playback position.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property PlaybackPositionText() As String
            Get
                return CType(GetValue(PlaybackPositionTextProperty, String))
            End Get
        End Property

        ''' <summary>
        ''' Gets the text of the Cpu utilization.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property CpuText() As String
            Get
                return CType(GetValue(CpuTextProperty, String))
            End Get
        End Property

        ''' <summary>
        ''' Gets the duration of the current playlist item.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        <ScriptableMember>
        public Double PlaybackDuration
        {
            get
            {
                return (Double)GetValue(PlaybackDurationProperty)
            }
        }

        ''' <summary>
        ''' Gets the current duration as a string.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property PlaybackDurationText() As String
            Get
                return CType(GetValue(PlaybackDurationTextProperty, String))
            End Get
        End Property

        ''' <summary>
        ''' Gets the current buffering percent.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        <ScriptableMember>
        public Double BufferingPercent
        {
            get { return (Double)GetValue(BufferingPercentProperty); }
        }

        ''' <summary>
        ''' Gets the source of the current poster image.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property PosterImageSource() As Uri
            Get
                return CType(GetValue(PosterImageSourceProperty, Uri))
            End Get
        End Property

        ''' <summary>
        ''' Gets the max width of the current poster image.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        public Double PosterImageMaxWidth
        {
            get { return (Double)GetValue(PosterImageMaxWidthProperty); }
        }

        ''' <summary>
        ''' Gets the max height of the current poster image.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        public Double PosterImageMaxHeight
        {
            get { return (Double)GetValue(PosterImageMaxHeightProperty); }
        }

        ''' <summary>
        ''' Gets the visibility for the buffering control.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property BufferingControlVisibility() As Visibility
            Get
                return CType(GetValue(BufferingControlVisibilityProperty, Visibility))
            End Get
        End Property

        ''' <summary>
        ''' Gets the visibility for the Offline download control.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property OfflineDownloadProgressVisibility() As Visibility
            Get
                return CType(GetValue(OfflineDownloadProgressVisibilityProperty, Visibility))
            End Get
        End Property

        ''' <summary>
        ''' Gets the dowloading offset percent.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        <ScriptableMember>
        public Double DownloadOffsetPercent
        {
            get { return (Double)GetValue(DownloadOffsetPercentProperty); }
        }

        ''' <summary>
        ''' Gets the downloaded percent.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        <ScriptableMember>
        public Double DownloadPercent
        {
            get { return (Double)GetValue(DownloadPercentProperty); }
        }

        ''' <summary>
        ''' Gets the visibility of the captions button.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property CaptionsButtonVisibility() As Visibility
            Get

                return CType(GetValue(CaptionsButtonVisibilityProperty, Visibility))
            End Get
        End Property

        ''' <summary>
        ''' Gets the visibility of the offline button.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property OfflineButtonVisibility() As Visibility
            Get

                return CType(GetValue(OfflineButtonVisibilityProperty, Visibility))
            End Get
        End Property
        ''' <summary>
        ''' Gets whether the offline button is enabled
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        public Boolean OfflineButtonEnabled
        {
            get
            {
                return (Boolean) GetValue(OfflineButtonEnabledProperty)
            }
        }

        ''' <summary>
        ''' Gets the visibility of the popout button.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property PopOutButtonVisibility() As Visibility
            Get

                return CType(GetValue(PopOutButtonVisibilityProperty, Visibility))
            End Get
        End Property

        ''' <summary>
        ''' Gets the visibility of the PlugIn button.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property PlugInButtonVisibility() As Visibility
            Get

                return CType(GetValue(PlugInButtonVisibilityProperty, Visibility))
            End Get
        End Property

        ''' <summary>
        ''' Gets the visibility of the chapters button.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property ChaptersButtonVisibility() As Visibility
            Get

                return CType(GetValue(ChaptersButtonVisibilityProperty, Visibility))
            End Get
        End Property

        ''' <summary>
        ''' Gets the visibility of the Playlist button.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property PlaylistButtonVisibility() As Visibility
            Get

                return CType(GetValue(PlaylistButtonVisibilityProperty, Visibility))
            End Get
        End Property

        ''' <summary>
        ''' Gets the visibility of the Cpu textblock.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property CpuVisibility() As Visibility
            Get

                return CType(GetValue(CpuVisibilityProperty, Visibility))
            End Get
        End Property

        ''' <summary>
        ''' Gets the caption text.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property CaptionText() As String
            Get

                return CType(GetValue(CaptionTextProperty, String))
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the visibility of captions.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public Property CaptionsVisibility() As Visibility
            Get
                return CType(GetValue(CaptionsVisibilityProperty, Visibility))
            End Get

            Set
                SetValue(CaptionsVisibilityProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the CaptionOptions
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public Property CaptionOptions() As Dictionary(Of String, CaptionOption)
            Get
                return ((Dictionary<string, CaptionOption>) GetValue(CaptionOptionsProperty))
            End Get

            Set
                SetValue(CaptionOptionsProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the user background color.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        public Brush UserBackgroundColor
        {
            get { return (Brush)GetValue(UserBackgroundColorProperty); }
            set { SetValue(UserBackgroundColorProperty, value); }
        }

        ''' <summary>
        ''' Gets the current media element state.
        ''' </summary>
        <ScriptableMember>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property CurrentState() As MediaElementState
            Get


                If (m_mediaElement  IsNot Nothing) Then
                    return m_mediaElement.CurrentState
                End If


                return MediaElementState.Stopped
            End Get
        End Property

        ''' <summary>
        ''' Gets a value indicating whether the media element can step.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property FrameStepVisibility() As Visibility
            Get
                return CType(GetValue(FrameStepVisibilityProperty, Visibility))
            End Get
        End Property

        ''' <summary>
        ''' Gets a value indicating whether the error message is visible.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property ErrorMessageVisibility() As Visibility
            Get
                return CType(GetValue(ErrorMessageVisibilityProperty, Visibility))
            End Get
        End Property

        ''' <summary>
        ''' Gets the source of the current poster image.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property ErrorMessage() As String
            Get
                return CType(GetValue(ErrorMessageProperty, String))
            End Get
        End Property

        ''' <summary>
        ''' Gets a value indicating whether the PreviousButton control is enabled.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        <ScriptableMember>
        Public ReadOnly Property ButtonPreviousIsEnabled() As Boolean
            Get
                return CType(GetValue(ButtonPreviousIsEnabledProperty, Boolean))
            End Get
        End Property

        ''' <summary>
        ''' Gets a value indicating whether the NextButton control is enabled.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        <ScriptableMember>
        Public ReadOnly Property ButtonNextIsEnabled() As Boolean
            Get
                return CType(GetValue(ButtonNextIsEnabledProperty, Boolean))
            End Get
        End Property

        ''' <summary>
        ''' Gets the current media element we are using. -- If the SSME is in use this should return null
        ''' </summary>
        public MediaElementShim CurrentMediaElement
        {
            get
            {
                return m_mediaElement
            }
        }

        Public ReadOnly Property ConventionalMediaElement() As MediaElement
            Get

                '  if in conventional mode -- return actual media element -- if in smooth streaming mode -- return null

                If (Me.m_mediaElement = Me.m_mediaElementForConventionalContent) Then
                    return Me.m_actualMediaElement
                End If

                return Nothing
            End Get
        End Property

        public MediaElementShim SmoothStreamingMediaElementShim
        {
            get
            {
                return m_mediaElementForSmoothStreamingContent
            }
        }

        Public ReadOnly Property SmoothStreamingOfflineSupport() As IPlugInMssOfflineSupport
            Get


                If (Me.m_smoothStreamingOfflineSupport Is Nothing  AndAlso  Me.m_mediaElementForSmoothStreamingContent  IsNot Nothing) Then
                    Me.m_smoothStreamingOfflineSupport = Me.m_mediaElementForSmoothStreamingContent.OfflineSupport
                End If

                return m_smoothStreamingOfflineSupport
            End Get
        End Property

        ''' <summary>
        ''' Sets the playback position of the position slider.
        ''' </summary>
        ''' <param name="value">The new playback position.</param>
        protected virtual Sub SetPlaybackPosition(value As Double)


            If (PlaybackPosition = value) Then
                return
            End If


            SetValue(PlaybackPositionProperty, value)
            '  don't update slider position while the user is dragging
            '  don't update the slider position while there is a pending seek -- as this can generate an additional seek request

            If (((m_sliderPosition  IsNot Nothing)  AndAlso  ( Not m_sliderPosition.IsDragging))  AndAlso  ( Not m_seekOnNextTick) ) Then
                '  update slider position

                If (m_sliderPosition.Value  <>  value) Then
                    '  Don't generate a seek
                    m_sliderPosition.ValueChanged -= OnSliderPositionChanged
                    m_sliderPosition.Value = value
                    m_sliderPosition.ValueChanged += OnSliderPositionChanged
                End If
            End If

            '  update position text as well
            UpdatePositionDisplay()
        End Sub '   SetPlaybackPosition


        ''' <summary>
        ''' Sets the playback duration of the position slider.
        ''' </summary>
        ''' <param name="value">The new playback duration.</param>
        protected virtual Sub SetPlaybackDuration(value As Double)

            SetValue(PlaybackDurationProperty, value)

            If (m_sliderPosition  IsNot Nothing) Then
                '  Don't generate a seek
                m_sliderPosition.ValueChanged -= OnSliderPositionChanged
                m_sliderPosition.Value = 0.0
                m_sliderPosition.Maximum = PlaybackDuration
                m_sliderPosition.ValueChanged += OnSliderPositionChanged
            End If
        End Sub '   SetPlaybackDuration

        ''' <summary>
        ''' Sets the playback position text.
        ''' </summary>
        ''' <param name="value">New text for the playback position.</param>
        protected virtual Sub SetPlaybackPositionText(value As String)

            SetValue(PlaybackPositionTextProperty, value)
        End Sub '   SetPlaybackPositionText


        ''' <summary>
        ''' Sets the Cpu utilization text.
        ''' </summary>
        ''' <param name="value">New text for the Cpu Utilization</param>
        protected virtual Sub SetCpuText(value As String)

            SetValue(CpuTextProperty, value)
        End Sub '   SetCpuText


        ''' <summary>
        ''' Sets the playback duration text.
        ''' </summary>
        ''' <param name="value">The new duration text.</param>
        protected virtual Sub SetPlaybackDurationText(value As String)

            SetValue(PlaybackDurationTextProperty, value)
        End Sub '   SetPlaybackDurationText


        ''' <summary>
        ''' Sets the buffering percentage.
        ''' </summary>
        ''' <param name="value">New value for the buffering percent.</param>
        protected Sub SetBufferingPercent(value As Double)

            SetValue(BufferingPercentProperty, value)
        End Sub '   SetBufferingPercent


        ''' <summary>
        ''' Sets the poster image.
        ''' </summary>
        ''' <param name="value">New poster image.</param>
        protected virtual Sub SetPosterImageSource(value As String)

            SetValue(PosterImageSourceProperty, value)
        End Sub '   SetPosterImageSource


        ''' <summary>
        ''' Sets the poster image width.
        ''' </summary>
        ''' <param name="value">New poster image width.</param>
        protected virtual Sub SetPosterImageMaxWidth(value As Double)

            SetValue(PosterImageMaxWidthProperty, value)
        End Sub '   SetPosterImageMaxWidth


        ''' <summary>
        ''' Sets the poster image height.
        ''' </summary>
        ''' <param name="value">New poster image height.</param>
        protected virtual Sub SetPosterImageMaxHeight(value As Double)

            SetValue(PosterImageMaxHeightProperty, value)
        End Sub '   SetPosterImageMaxHeight


        ''' <summary>
        ''' Sets the visibility of the buffering control.
        ''' </summary>
        ''' <param name="value">New visibility for the buffering control.</param>
        protected virtual Sub SetBufferingControlVisibility(value As Visibility)

            SetValue(BufferingControlVisibilityProperty, value)
        End Sub '   SetBufferingControlVisibility


        ''' <summary>
        ''' Sets the visibility of the Offline download progress bar.
        ''' </summary>
        ''' <param name="value">New visibility for the Offline download progress bar.</param>
        internal virtual Sub SetOfflineDownloadProgressVisibility(value As Visibility)

            SetValue(OfflineDownloadProgressVisibilityProperty, value)
        End Sub '   SetOfflineDownloadProgressVisibility


        ''' <summary>
        ''' Sets the download offset percent.
        ''' </summary>
        ''' <param name="value">New value for the download offset.</param>
        protected Sub SetDownloadOffsetPercent(value As Double)

            SetValue(DownloadOffsetPercentProperty, value)
        End Sub '   SetDownloadOffsetPercent


        ''' <summary>
        ''' Sets the download percent.
        ''' </summary>
        ''' <param name="value">New value for the download percent.</param>
        protected Sub SetDownloadPercent(value As Double)

            SetValue(DownloadPercentProperty, value)
        End Sub '   SetDownloadPercent


        ''' <summary>
        ''' Sets the caption text.
        ''' </summary>
        ''' <param name="value">New value for the caption text.</param>
        public virtual Sub SetCaptionText(value As String)

            SetValue(CaptionTextProperty, value)
        End Sub '   SetCaptionText


        ''' <summary>
        ''' Sets the visibility of the captions button.
        ''' </summary>
        ''' <param name="value">New visibility of the captions button.</param>
        protected virtual Sub SetCaptionsButtonVisibility(value As Visibility)

            SetValue(CaptionsButtonVisibilityProperty, value)
        End Sub '   SetCaptionsButtonVisibility


        ''' <summary>
        ''' Sets the visibility of the offline button.
        ''' </summary>
        ''' <param name="value">New visibility of the offline button.</param>
        internal virtual Sub SetOfflineButtonVisibility(value As Visibility)

            SetValue(OfflineButtonVisibilityProperty, value)
        End Sub '   SetOfflineButtonVisibility


        ''' <summary>
        ''' Sets the visibility of the offline button.
        ''' </summary>
        ''' <param name="value">New visibility of the offline button.</param>
        internal virtual Sub SetOfflineButtonEnabled(value As Boolean)

            SetValue(OfflineButtonEnabledProperty, value)
        End Sub '   SetOfflineButtonEnabled


        ''' <summary>
        ''' Sets the visibility of the popout button.
        ''' </summary>
        ''' <param name="value">New visibility of the popout button.</param>
        protected virtual Sub SetPopOutButtonVisibility(value As Visibility)

            SetValue(PopOutButtonVisibilityProperty, value)
        End Sub '   SetPopOutButtonVisibility


        ''' <summary>
        ''' Sets the visibility of the PlugIn button.
        ''' </summary>
        ''' <param name="value">New visibility of the PlugIn button.</param>
        protected virtual Sub SetPlugInButtonVisibility(value As Visibility)


            If (value = Visibility.Collapsed) Then
                ShowPlugIn(false)
            else

                If (Me.m_buttonPlugIn  IsNot Nothing) Then
                    ShowPlugIn(Me.m_buttonPlugIn.IsChecked = true)
                End If
            End If
            SetValue(PlugInButtonVisibilityProperty, value)
        End Sub '   SetPlugInButtonVisibility


        ''' <summary>
        ''' Sets the visibility of the chapters button.
        ''' </summary>
        ''' <param name="value">New visibility of the chapters button.</param>
        protected virtual Sub SetChaptersButtonVisibility(value As Visibility)

            SetValue(ChaptersButtonVisibilityProperty, value)

            If (value = Visibility.Collapsed) Then
                ShowChapters(false)
            End If
        End Sub '   SetChaptersButtonVisibility


        ''' <summary>
        ''' Sets the visibility of the playlist button.
        ''' </summary>
        ''' <param name="value">New visibility of the playlist button.</param>
        protected virtual Sub SetPlaylistButtonVisibility(value As Visibility)

            SetValue(PlaylistButtonVisibilityProperty, value)

            If (value = Visibility.Collapsed) Then
                ShowPlaylist(false)
            End If
        End Sub '   SetPlaylistButtonVisibility


        ''' <summary>
        ''' Sets the visibility of the Cpu stats.
        ''' </summary>
        ''' <param name="value">New visibility of the Cpu stats.</param>
        protected virtual Sub SetCpuVisibility(value As Visibility)

            SetValue(CpuVisibilityProperty, value)
        End Sub '   SetCpuVisibility


        #End Region

        #region Attached Properties
        ''' <summary>
        ''' Gets the current value of the hide on click property.
        ''' </summary>
        ''' <param name="obj">Dependency property object.</param>
        ''' <returns>Flag indicating the status of the hide on click properry.</returns>
        Public ReadOnly Property bool() As

        ''' <summary>
        ''' Sets the value of the hide on click property.
        ''' </summary>
        ''' <param name="obj">Dependency property object.</param>
        ''' <param name="value">The new value of the hide on click property.</param>
        Public ReadOnly Property Sub() As

        ''' <summary>
        ''' Unescapes an escaped string.
        ''' </summary>
        ''' <param name="escaped">The escaped string.</param>
        ''' <returns>The new unescaped string.</returns>
        Public ReadOnly Property String() As

        #End Region

        #region public methods

        ''' <summary>
        ''' Plays the current item in the playlist.
        ''' </summary>
        <ScriptableMember>
        public virtual Sub Play()


            If ( Not m_inPlayState) Then
                TogglePlayPause()
            End If
        End Sub '   Play

        ''' <summary>
        ''' Pauses the current playlist item.
        ''' </summary>
        <ScriptableMember>
        public virtual Sub Pause()


            If (m_inPlayState) Then
                TogglePlayPause()
            End If
        End Sub '   Pause

        ''' <summary>
        ''' Stops the current playlist item.
        ''' </summary>
        <ScriptableMember>
        public virtual Sub Stop()

            ButtonClickStopLogic()
        End Sub '   Stop


        ''' <summary>
        ''' Current media position
        ''' </summary>
        <ScriptableMember>
        public virtual double Position
        {
            get
            {
                return (m_mediaElement.Position.TotalSeconds)
            }
            set
            {
                m_mediaElement.Position = New TimeSpan((long)(value * 10000000))
                SeekToTime(value)
            }
        }

        ''' <summary>
        ''' Goes to the next playlist item at the next ui update interval
        ''' </summary>
        public Sub GoToPlaylistItemOnNextTick(playlistItemIndex As Integer)

            m_playWhenMediaElementReady = true
            '  don't set it if already set

            If ( Not m_goToItemOnNextTick) Then
                m_goToItemOnNextTick = true
                m_goToItemOnNextTickIndex = playlistItemIndex
            End If
        End Sub '   GoToPlaylistItemOnNextTick

        ''' <summary>
        ''' Goes to the next playlist item.
        ''' </summary>
        <ScriptableMember>
        public Sub GoToNextPlaylistItem()


            If (Me.Playlist  IsNot Nothing  AndAlso  ((Me.m_currentPlaylistIndex+1) < Playlist.Items.Count)) Then
                GoToPlaylistItemOnNextTick(m_currentPlaylistIndex + 1)
            End If
        End Sub '   GoToNextPlaylistItem

        ''' <summary>
        ''' Goes to the next playlist item.
        ''' </summary>
        <ScriptableMember>
        public Sub GoToPreviousPlaylistItem()

            GoToPlaylistItemOnNextTick(m_currentPlaylistIndex - 1)
        End Sub '   GoToPreviousPlaylistItem


        public Sub GoToPlaylistItem1(playlistItemIndex As Integer)

            GoToPlaylistItemOnNextTick(playlistItemIndex)
        End Sub '   GoToPlaylistItem1


        ''' <summary>
        ''' Goes to a playlist item.
        ''' </summary>
        ''' <param name="playlistItemIndex">The index of the playlist item to go to.</param>
        <ScriptableMember>
        private Sub GoToPlaylistItemInternal(playlistItemIndex As Integer)


            If (IsDesignTime) Then
                return
            End If

#if DEBUG
            Debug.WriteLine("GoToPlaylistItem item=" + playlistItemIndex.ToString() +  DateTime.Now.ToString())
#endif

            If (playlistItemIndex >= Playlist.Items.Count) Then

                If (Playlist.AutoRepeat) Then
                    playlistItemIndex = 0
                End If
            End If


            If (playlistItemIndex >= 0  AndAlso  playlistItemIndex < Playlist.Items.Count) Then
                m_currentChapterIndex = 0
                ClearCaptionText()

                (m_currentPlaylistIndex = playlistItemIndex) AndAlso (Me.m_mediaElement IsNot Nothing)
                Dim canSkipReset As Boolean  = playlistItemIndex) AndAlso (Me.m_mediaElement IsNot Nothing)
                if (canSkipReset)
                {

                    Select Case (Me.m_mediaElement.CurrentState)

                        case MediaElementState.Closed:
                        case MediaElementState.Stopped:
                            canSkipReset = false

                        default:
                    End Select '    Me.m_mediaElement.CurrentState

                }

                if ( Not canSkipReset)
                {
#if DEBUG
                    Debug.WriteLine("GoToPlaylistItem not skipping")
#endif
                    m_currentPlaylistIndex = playlistItemIndex
                    PlaylistItem playlistItem = Playlist.Items(m_currentPlaylistIndex)

                    if (m_selectorChapters  IsNot Nothing)
                    {
                        m_selectorChapters.ItemsSource = playlistItem.Chapters
                        SetChaptersButtonVisibility(playlistItem.Chapters.Count>0?Visibility.Visible:Visibility.Collapsed)
                    }

                    '  Update window title with playlist item title (or filename)
                    if ( Not Application.Current.IsRunningOutOfBrowser  AndAlso  tmlPage.IsEnabled  AndAlso  HtmlPage.Document  IsNot Nothing)
                    {

                        Dim newTitle As String  = string.Empty

                        if ( Not string.IsNullOrEmpty(playlistItem.Title))
                        {
                            newTitle = playlistItem.Title
                        }
                        else
                        {
                            if (playlistItem.MediaSource  IsNot Nothing)
                            {
                                if (playlistItem.IsAdaptiveStreaming)
                                {
                                    newTitle = System.IO.Path.GetDirectoryName(playlistItem.MediaSource.ToString())
                                    newTitle = System.IO.Path.GetFileName(newTitle)
                                }
                                else
                                {
                                    newTitle = System.IO.Path.GetFileName(playlistItem.MediaSource.ToString())
                                }
                            }
                        }

                        HtmlPage.Document.SetProperty("title", newTitle)
                    }

                    m_mediaFailureRetryCount = 0

                    DoOpenPlaylistItem(playlistItem)

                    '  Update and show the poster frame for the current item -- if not actively playing
                    if ( Not m_inPlayState  AndAlso   Not Playlist.AutoPlay)
                    {
                        DisplayPoster(m_currentPlaylistIndex)
                    }

                    UpdateCaptionOptionsMenuForCurrentPlayListItem()
                }
            ElseIf (playlistItemIndex >= Playlist.Items.Count) Then
                '  Reached end -- flag that playback is paused.
                m_inPlayState = false
            End If


            OnItemStarted()
        End Sub '   GoToPlaylistItemInternal


        private Sub DoOpenPlaylistItem(playlistItem As PlaylistItem)


            If (Me.m_elementVideoWindow  IsNot Nothing) Then
                '  Audio only type templates

                If (Me.m_elementVideoWindow.Visibility = Visibility.Collapsed) Then
                    '  SmoothStreaming not supported in audio only case

                    If (playlistItem.IsAdaptiveStreaming) Then

                        Dim strErrorMessage As String  = String.Format(CultureInfo.CurrentUICulture, ExpressionMediaPlayer.Resources.errorInvalidMedia, playlistItem.MediaSource.OriginalString)

                        ShowErrorMessage(strErrorMessage)
                        return
                    End If
                End If
            End If


            '  Attach media source to the MediaElement
            m_currentItemIsAdaptive = playlistItem.IsAdaptiveStreaming
            SetPlugInButtonVisibility(m_currentItemIsAdaptive ? Visibility.Visible : Visibility.Collapsed)

            Try

                If (m_currentItemIsAdaptive) Then
                    Me.InitSmoothStreaming(playlistItem)
                else
                    Me.InitConventionalStreaming(playlistItem)
                End If


            Catch iso As IsolatedStorageException

                ShowErrorMessage(iso.Message)
                Stop()
            End Try
        End Sub '   DoOpenPlaylistItem
        #End Region

        #region TemplateHandlers

        ''' <summary>
        ''' Overrides base.OnApplyTemplate().
        ''' </summary>
        public override Sub OnApplyTemplate()

            MyBase.OnApplyTemplate()

            GetTemplateChildren()

            ConfigureBinding()
            ApplyProperties()

            HookHandlers()
        End Sub '   OnApplyTemplate


        ''' <summary>
        ''' Gets the child elements of the template.
        ''' </summary>
        protected virtual Sub GetTemplateChildren()

            m_elementStretchBox = GetTemplateChild(StretchBox) as FrameworkElement
            m_elementVideoWindow = GetTemplateChild(VideoWindow) as Panel
            m_buttonStart = GetTemplateChild(StartButton) as ButtonBase
            m_buttonPlayPause = GetTemplateChild(PlayPauseButton) as ToggleButton
            m_buttonPrevious = GetTemplateChild(PreviousButton) as ButtonBase
            m_buttonNext = GetTemplateChild(NextButton) as ButtonBase
            m_buttonStop = GetTemplateChild(StopButton) as ButtonBase
            m_buttonFullScreen = GetTemplateChild(FullScreenButton) as ButtonBase
            m_buttonOffline = GetTemplateChild(OfflineButton) as ButtonBase
            m_buttonPopOut = GetTemplateChild(PopOutButton) as ButtonBase
            m_buttonPlugIn = GetTemplateChild(PlugInButton) as ToggleButton
            m_sliderPosition = GetTemplateChild(PositionSlider) as SensitiveSlider
            m_buttonMute = GetTemplateChild(MuteButton) as ToggleButton
            m_buttonVolumeDown = GetTemplateChild(VolumeDownButton) as ButtonBase
            m_buttonVolumeUp = GetTemplateChild(VolumeUpButton) as ButtonBase
            m_sliderVolume = GetTemplateChild(VolumeSlider) as SensitiveSlider
            m_buttonPlaylist = GetTemplateChild(PlaylistButton) as ToggleButton
            m_buttonChapter = GetTemplateChild(ChapterButton) as ToggleButton
            m_selectorPlaylist = GetTemplateChild(PlaylistSelector) as Selector
            m_selectorChapters = GetTemplateChild(ChaptersSelector) as Selector
            m_elementErrorMessage = GetTemplateChild(ErrorMessageElement) as FrameworkElement
            m_buttonStepForwards = GetTemplateChild(ButtonStepForwards) as ButtonBase
            m_buttonStepBackwards = GetTemplateChild(ButtonStepBackwards) as ButtonBase
            m_gridPlugIn = GetTemplateChild(GridPlugIn) as Grid

            m_buttonClosedCaptions = GetTemplateChild(ClosedCaptionButton) as ToggleButton
            m_gridCaptionArea = GetTemplateChild(ClosedCaptionArea) as Grid
            m_popupClosedCaptionsOptionsMenu = GetTemplateChild(ClosedCaptionsOptionsMenu) as Popup
            m_listboxCaptionOptions = GetTemplateChild(ClosedCaptionsOptionsList) as ListBox

            m_mediaElementGrid = GetTemplateChild(MediaElementGrid) as Grid
            Debug.Assert(m_mediaElementGrid  IsNot Nothing, "essential markup item missing: " + MediaElementGrid)


            If (m_listboxCaptionOptions  IsNot Nothing) Then
                m_listboxCaptionOptions.DataContext = this
            End If


            m_actualMediaElement = GetTemplateChild(oMediaElement) as MediaElement
            Debug.Assert(m_actualMediaElement  IsNot Nothing, "essential markup item missing: " + oMediaElement)
        End Sub '   GetTemplateChildren


        #End Region

        #region EventHandlers

        ''' <summary>
        ''' Parse init params. It should be just a playlist so parse that.
        ''' </summary>
        ''' <param name="e">Event args.</param>
        protected virtual Sub ParseStartupParameters(e As StartupEventArgs)


            If (Nothing = e) Then
                Debug.WriteLine("ParseStartupParameters passed bogus data")
                return
            End If


            Dim strInitValue As String  = string.Empty

            Try

                Dim strSimpleMediaSource As String  = string.Empty
                Dim strSimpleThumb As String  = string.Empty
                Dim strSimpleTitle As String  = string.Empty
                Dim strSimpleAspect As String  = string.Empty
                Dim strIsAdaptive As String  = string.Empty
                Dim strPlaylist As String  = Nothing


                If (e.InitParams.TryGetValue("playerSettings", out strInitValue)) Then
                    strPlaylist = string.Empty
                    string() separators = New string() { ChrW(13) & ChrW(10), "\t" }
                    string() initPieces = strInitValue.Split(separators, StringSplitOptions.RemoveEmptyEntries)

                    For Each initPiece As String in initPieces

                        Try
                            strPlaylist += System.Uri.UnescapeDataString(initPiece)

                        Catch ex As IndexOutOfRangeException

                            '  just add the escaped string
                            strPlaylist += initPiece
                        End Try
                    Next    '   initPiece

                else

                    If (e.InitParams.TryGetValue("MediaSource", out strInitValue)) Then
                        strSimpleMediaSource = strInitValue
                    End If


                    If (e.InitParams.TryGetValue("ThumbSource", out strInitValue)) Then
                        strSimpleThumb = strInitValue
                    End If


                    If (e.InitParams.TryGetValue("Title", out strInitValue)) Then
                        strSimpleTitle = strInitValue
                    End If


                    If (e.InitParams.TryGetValue("Aspect", out strInitValue)) Then
                        strSimpleAspect = strInitValue
                    End If


                    If (e.InitParams.TryGetValue("IsAdaptiveStreaming", out strInitValue)) Then
                        strIsAdaptive = strInitValue
                    End If
                End If

                '  Cpu Utilization reporting parameters
                {

                    Dim monitorCpu As Boolean  = false
                    Dim strCpuValue As String  = string.Empty


                    If (e.InitParams.TryGetValue("ShowCpuStats", out strCpuValue)) Then

                        Dim showCpuStats As Boolean  = false


                        If (bool.TryParse(strCpuValue, out showCpuStats)) Then

                            If (showCpuStats) Then
                                Me.SetCpuVisibility(Visibility.Visible)
                                monitorCpu = true
                            End If
                        End If
                    End If


                    If (e.InitParams.TryGetValue("CpuStatsReport", out strCpuValue)) Then
                        Me.m_cpuStatsReport = strCpuValue
                        monitorCpu = true
                    End If



                    If (monitorCpu) Then
                        Try
                            m_analyticsOptional = New Analytics()

                        Catch ex As System.Exception

                            '  wierd case Analytics.ctor can throw when performance registry data is messed up
                            m_analyticsOptional = Nothing
                        End Try
                    End If
                }

                '  Check for simplfied init case

                If (strPlaylist Is Nothing) Then
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
                        + "<MediaSource>" + strSimpleMediaSource + "</MediaSource>"

                    If ( Not string.IsNullOrEmpty(strSimpleThumb)) Then
                        strPlaylist += "<ThumbSource>" + strSimpleThumb + "</ThumbSource>"
                    End If


                    If ( Not string.IsNullOrEmpty(strSimpleTitle)) Then
                        strPlaylist += "<Title>" + strSimpleTitle + "</Title>"
                    End If


                    If ( Not string.IsNullOrEmpty(strSimpleAspect)) Then
                        strSimpleAspect = strSimpleAspect.ToUpper(CultureInfo.InvariantCulture)

                        If (strSimpleAspect = "WIDE") Then
                            strPlaylist += "<AspectRatioWidth>16</AspectRatioWidth><AspectRatioHeight>9</AspectRatioHeight>"
                        ElseIf (strSimpleAspect = "NARROW") Then
                            strPlaylist += "<AspectRatioWidth>4</AspectRatioWidth><AspectRatioHeight>3</AspectRatioHeight>"
                        End If
                    End If
                    ' {
                        Dim isSet As Boolean  = false
                        Dim isAdaptive As Boolean  = false


                        If ( Not string.IsNullOrEmpty(strIsAdaptive)) Then
                            isSet = bool.TryParse(strIsAdaptive, out isAdaptive)
                        End If



                        If ( Not isSet) Then

                            Dim tmp As String  = strSimpleMediaSource.ToUpper(CultureInfo.CurrentCulture)

                            isAdaptive = tmp.Contains(".ISM")  OrElse  tmp.Contains(".ISML")
                        End If


                        Dim param As String  = "<IsAdaptiveStreaming>" + isAdaptive.ToString(CultureInfo.InvariantCulture).ToLower() + "</IsAdaptiveStreaming>"

                        strPlaylist += param
                    ' }
                    strPlaylist += "</PlaylistItem>"
                        + "</Items>"
                        + "</Playlist>"
                End If


                If (strPlaylist  IsNot Nothing) Then
                    SetValue(PlaylistProperty, Playlist.Create(strPlaylist))
                    ApplyProperties()
                End If


            Catch xe As System.Xml.XmlException

                '  special case. Encoder precompiled template? fail silently...

                If (strInitValue.IndexOf("<$=", StringComparison.OrdinalIgnoreCase) > 0  AndAlso  strInitValue.IndexOf("$>", StringComparison.OrdinalIgnoreCase) > 0) Then
                    SetValue(PlaylistProperty, New Playlist())
                    return
                End If

                ShowErrorMessage(xe.ToString())

            Catch xe As NullReferenceException

                ShowErrorMessage(xe.ToString())

            Catch xe As InvalidCastException

                ShowErrorMessage(xe.ToString())

            Catch xe As InvalidPlaylistException

                ShowErrorMessage(xe.ToString())

            Catch xe As IndexOutOfRangeException

                ShowErrorMessage(xe.ToString())
            End Try
        End Sub '   ParseStartupParameters

        ''' <summary>
        ''' Write the Cpu Utilization statitics data to isolated storage
        ''' </summary>
        ''' <param name="reportName">The HTML specified report filename</param>
        ''' <param name="videoSource">The video file name that was just rendered</param>
        ''' <param name="CpuSampleSum">The sum of processor load samples</param>
        ''' <param name="CpuSampleCount">The number of processor load samples taken</param>
        Private ReadOnly Property Sub() As
            Get

                return Application.Current.GetType() = typeof(Application)
            End Get
        End Property

        ''' <summary>
        ''' Event handler for the playlist clicked event.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonClickPlaylist(sender As Object, e As RoutedEventArgs)

            ToggleButton btn = sender as ToggleButton
            ShowPlaylist(btn.IsChecked = true)
        End Sub '   OnButtonClickPlaylist


        ''' <summary>
        ''' show/hide playlist
        ''' </summary>
        ''' <param name="show">true for show, false for hide</param>
        <ScriptableMember>
        public virtual Sub ShowPlaylist(show As Boolean)


            If (show) Then
                VisualStateManager.GoToState(this, "showPlaylist", true)
                if (m_selectorPlaylist  IsNot Nothing  AndAlso  m_selectorPlaylist.SelectedItem  IsNot Nothing)
                {
                    if (m_selectorPlaylist.SelectedItem is ListBoxItem)
                    {
                        (CType(m_selectorPlaylist.SelectedItem, ListBoxItem)).Focus()
                    }
                }
            ElseIf ( Not show) Then
                VisualStateManager.GoToState(this, "hidePlaylist", true)
            End If
        End Sub '   ShowPlaylist

        ''' <summary>
        ''' Event handler for the chapter button event.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonClickChapter(sender As Object, e As RoutedEventArgs)

            ToggleButton btn = sender as ToggleButton
            ShowChapters(btn.IsChecked = true)
        End Sub '   OnButtonClickChapter


        ''' <summary>
        ''' show/hide chapters control
        ''' </summary>
        ''' <param name="show">true for show, false for hide</param>
        <ScriptableMember>
        public virtual Sub ShowChapters(show As Boolean)

            VisualStateManager.GoToState(this, show ? "showChapters" : "hideChapters", true)

            If (show  AndAlso Then
                m_selectorChapters  IsNot Nothing  AndAlso
                m_selectorChapters.SelectedItem <> Nothing  AndAlso
                m_selectorChapters.SelectedItem is ListBoxItem)
            {
                (CType(m_selectorChapters.SelectedItem, ListBoxItem)).Focus()
            End If
            }
        End Sub '   ShowChapters


        ''' <summary>
        ''' get/set if chapters showing
        ''' </summary>
        <ScriptableMember>
        public virtual bool ChaptersVisible
        {
            get
            {
                return m_buttonChapter.IsChecked = true
            }
            set
            {
                m_buttonChapter.IsChecked = value
                ShowChapters(value)
            }
        }

        ''' <summary>
        ''' set/get if playlist showing
        ''' </summary>
        <ScriptableMember>
        public virtual bool PlaylistVisible
        {
            get
            {
                return m_buttonPlaylist.IsChecked = true
            }
            set
            {
                m_buttonPlaylist.IsChecked = value
                ShowPlaylist(value)
            }
        }

        ''' <summary>
        ''' Update the state of the buffering progress indicator.
        ''' </summary>
        private Sub UpdateBufferingControls()

            '  control the visiblity of the buffering control

            If ((m_currentPlaylistIndex >= 0)  AndAlso  (m_currentPlaylistIndex < Playlist.Items.Count)  AndAlso  (MediaElementState.Buffering = m_mediaElement.CurrentState)) Then
                if (BufferingControlVisibility  <>  Visibility.Visible)
                {
                    SetBufferingControlVisibility(Visibility.Visible)
                    VisualStateManager.GoToState(this, "showBuffering", true)
                }

                SetBufferingPercent(m_mediaElement.BufferingProgress * ProgressConst.Progress2Percent)
            else

                If (BufferingControlVisibility  <>  Visibility.Collapsed) Then
                    SetBufferingControlVisibility(Visibility.Collapsed)
                    VisualStateManager.GoToState(this, "hideBuffering", true)
                End If
            End If
        End Sub '   UpdateBufferingControls


        ''' <summary>
        ''' Update the playback position slider and playback position text display with current media position
        ''' </summary>
        private Sub UpdatePlaybackPosition()

            '  Don't update position based on media element while the user is dragging the slider

            If (m_sliderPosition Is Nothing  OrElse   Not m_sliderPosition.IsDragging) Then
                '  while playing or paused -- get position from the media element
                Double position = 0.0

                Select Case (m_mediaElement.CurrentState)

                    case MediaElementState.Playing:
                    case MediaElementState.Paused:
                    case MediaElementState.Buffering:
                        position = m_mediaElement.Position.TotalSeconds

                    default:
                End Select '    m_mediaElement.CurrentState


                SetPlaybackPosition(position)
            End If
        End Sub '   UpdatePlaybackPosition

        ''' <summary>
        ''' Update Cpu utilization data
        ''' </summary>
        private Sub UpdateCpuStats()


            If (Me.m_analyticsOptional  IsNot Nothing) Then

                If (MediaElementState.Playing = m_mediaElement.CurrentState) Then
                    Me.m_cpuLastSample = m_analyticsOptional.AverageProcessLoad
                    Me.m_cpuSampleSum += m_cpuLastSample
                    Me.m_cpuSampleCount += 1
                    Me.m_cpuPeakSample = Math.Max(Me.m_cpuPeakSample, Me.m_cpuLastSample)


                    If (Me.CpuVisibility = Visibility.Visible) Then

                        Dim cpuText As String  = (CType(Me.m_cpuLastSample, Integer)).ToString(CultureInfo.CurrentCulture)


                        If (Me.m_cpuSampleCount > 0) Then
                            cpuText += " " + (CType((Me.m_cpuSampleSum, Integer) / Me.m_cpuSampleCount)).ToString(CultureInfo.CurrentCulture)
                        End If

                        SetCpuText(cpuText)
                    End If
                End If
            End If
        End Sub '   UpdateCpuStats

        ''' <summary>
        ''' If chapter list is visible -- Update selected chapter
        ''' Set Selection onto matching chapterlist item
        ''' </summary>
        private Sub UpdateChapterListbox()


            If ((m_selectorChapters  IsNot Nothing)  AndAlso  (m_selectorChapters.Visibility = Visibility.Visible)  AndAlso  (m_selectorChapters.SelectedIndex  <>  m_currentChapterIndex)  AndAlso  (m_currentChapterIndex >= 0)  AndAlso  (m_currentChapterIndex < m_selectorChapters.Items.Count)) Then
                '  avoid clicked-on selection change logic
                m_selectorChapters.SelectionChanged -= OnListBoxSelectionChangedChapters
                m_selectorChapters.SelectedIndex = m_currentChapterIndex
                m_selectorChapters.SelectionChanged += OnListBoxSelectionChangedChapters

                '  Scroll current chapter into view
                Dim objCurrentChapterItem As Object  = m_selectorChapters.Items(m_currentChapterIndex)


                If (objCurrentChapterItem  IsNot Nothing) Then
                    Try

                        If (m_selectorChapters is ListBox) Then
                            (CType(m_selectorChapters, ListBox)).ScrollIntoView(objCurrentChapterItem)
                        End If


                    Catch nre As NullReferenceException

                        Debug.WriteLine(nre.ToString())
                        Debug.WriteLine(nre.StackTrace.ToString())
                    End Try
                End If
            End If
        End Sub '   UpdateChapterListbox

        ''' <summary>
        ''' If the Playlist is visible -- Update selected item
        ''' Set Selection onto matching Playlist item
        ''' </summary>
        private Sub UpdatePlaylistListbox()


            If ((m_selectorPlaylist  IsNot Nothing)  AndAlso  (m_selectorPlaylist.Visibility = Visibility.Visible)  AndAlso  (m_selectorPlaylist.SelectedIndex  <>  m_currentPlaylistIndex)  AndAlso  (m_currentPlaylistIndex >= 0)  AndAlso  (m_currentPlaylistIndex < Playlist.Items.Count)  AndAlso  (m_currentPlaylistIndex < m_selectorPlaylist.Items.Count)) Then
                '  avoid clicked-on selection change logic
                m_selectorPlaylist.SelectionChanged -= OnListBoxSelectionChangedPlaylist
                m_selectorPlaylist.SelectedIndex = m_currentPlaylistIndex
                m_selectorPlaylist.SelectionChanged += OnListBoxSelectionChangedPlaylist

                '  Scroll current playlist item into view
                Dim objCurrentPlaylistItem As Object  = m_selectorPlaylist.Items(m_currentPlaylistIndex)


                If (objCurrentPlaylistItem  IsNot Nothing) Then
                    Try

                        If (m_selectorChapters is ListBox) Then
                            (CType(m_selectorChapters, ListBox)).ScrollIntoView(objCurrentPlaylistItem)
                        End If


                    Catch nre As NullReferenceException

                        Debug.WriteLine(nre.ToString())
                        Debug.WriteLine(nre.StackTrace.ToString())
                    End Try
                End If
            End If
        End Sub '   UpdatePlaylistListbox

        ''' <summary>
        ''' Update the download progress bar as needed.
        ''' </summary>
        private Sub UpdateDownloadProgress()


            If (m_downloadProgressNeedsUpdating) Then
                m_downloadProgressNeedsUpdating = false
                Double downloadProgress = m_mediaElement.DownloadProgress
                SetDownloadPercent(downloadProgress * ProgressConst.Progress2Percent)
                SetDownloadOffsetPercent(m_mediaElement.DownloadProgressOffset * ProgressConst.Progress2Percent)
            End If
        End Sub '   UpdateDownloadProgress

        ''' <summary>
        ''' Restore play state after dragging slider position.
        ''' </summary>
        private Sub RestorePlayStateAfterSliderDrag()


            If (m_sliderPosition  IsNot Nothing) Then

                If ( Not m_sliderPosition.IsDragging  AndAlso  m_inPlayStateBeforeSliderPositionDrag) Then
                    m_inPlayStateBeforeSliderPositionDrag = false
                    InternalPlayAfterDrag()
                End If
            End If
        End Sub '   RestorePlayStateAfterSliderDrag


        internal Sub EnableClosedCaptionButton(enableButton As Boolean)


            If (Me.m_buttonClosedCaptions  IsNot Nothing) Then
                if(Me.m_buttonClosedCaptions.Dispatcher.CheckAccess())
                {
                    Me.m_buttonClosedCaptions.IsEnabled = enableButton
                }
                else
                {
                    Me.m_buttonClosedCaptions.Dispatcher.BeginInvoke(() => Me.EnableClosedCaptionButton(enableButton))
                }
            End If
        End Sub '   EnableClosedCaptionButton

        ''' <summary>
        ''' Event handler for a Timer tick.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnTimerTick(sender As Object, e As EventArgs)


            If (m_goToItemOnNextTick) Then
                m_goToItemOnNextTick = false
                GoToPlaylistItemInternal(m_goToItemOnNextTickIndex)
                return
            End If



            If (m_mediaElement Is Nothing) Then
                return
            End If



            If (m_seekOnNextTick) Then
                DoActualSeek()
            End If


            InternalDoPlayWhenReady()


            If (Me.CaptionsManager  IsNot Nothing) Then

                If (Me.m_refreshCaptionsOnNextTick) Then
                    Me.m_refreshCaptionsOnNextTick = false
                    Me.CaptionsManager.RefreshCaptions()
                End If
            End If

            UpdatePlaybackPosition()
            UpdateChapterListbox()
            UpdatePlaylistListbox()
            UpdatePrevNext()
            UpdateDownloadProgress()
            UpdateBufferingControls()
            RestorePlayStateAfterSliderDrag()


            If (Me.nextTickErrorMessage  IsNot Nothing) Then
                Me.ShowErrorMessage(Me.nextTickErrorMessage)
                Me.nextTickErrorMessage = Nothing
            End If


            UpdateCpuStats()

            RepositionCaptionPopup()
        End Sub '   OnTimerTick


        ''' <summary>
        ''' Event handler for the control fade out event.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnTimerControlFadeOutTick(sender As Object, e As EventArgs)

            if(currentControlState  <>  desiredControlState)
            {
                GoToControlState(desiredControlState)
            }

            SetDesiredControlState()
        End Sub '   OnTimerControlFadeOutTick


        ''' <summary>
        ''' Event handler for the slider position changed event.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnSliderPositionChanged(sender As Object, e As RoutedEventArgs)

            SeekToTime(m_sliderPosition.Value)
        End Sub '   OnSliderPositionChanged


        ''' <summary>
        ''' Handle DragStart event.
        ''' </summary>
        ''' <param name="sender">Source object, Thumb.</param>
        ''' <param name="e">Drag start args.</param>
        private Sub OnSliderPositionDragStarted(sender As Object, e As DragStartedEventArgs)


            If (m_inPlayState) Then
                m_inPlayStateBeforeSliderPositionDrag = true
                InternalPause()
            End If
        End Sub '   OnSliderPositionDragStarted

        ''' <summary>
        ''' overridable, item has started
        ''' </summary>
        public virtual Sub OnItemStarted()
        End Sub '   OnItemStarted


        ''' <summary>
        ''' Handle DragCompleted event.
        ''' </summary>.
        ''' <param name="sender">Source object, Thumb.</param>
        ''' <param name="e">Drag completed args.</param>
        private Sub OnSliderPositionDragCompleted(sender As Object, e As DragCompletedEventArgs)

            SeekToTime(m_sliderPosition.Value)
            DoActualSeek()
        End Sub '   OnSliderPositionDragCompleted


        ''' <summary>
        ''' Event handler for the media element state changed event.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnMediaElementCurrentStateChanged(sender As Object, e As RoutedEventArgs)

            MediaElementState currentState = m_mediaElement.CurrentState
#if DEBUG
            Debug.WriteLine("OnMediaElementCurrentStateChanged New state=" + currentState.ToString())
#endif

            '  Update the state of the play/pause button

            Select Case (currentState)

                case MediaElementState.Playing:
                case MediaElementState.Opening:
                case MediaElementState.Buffering:
                case MediaElementState.AcquiringLicense:
                    {

                        If (m_buttonPlayPause  IsNot Nothing) Then
                            m_buttonPlayPause.IsChecked = true
                        End If


                    }

                case MediaElementState.Paused:
                case MediaElementState.Stopped:
                case MediaElementState.Closed:
                    {

                        If (m_buttonPlayPause  IsNot Nothing) Then
                            m_buttonPlayPause.IsChecked = false
                        End If


                    }

                default:
            End Select '    currentState


            '  update the state of the thumbnail downloader to avoid taxing network bandwidth when the ME needs bandwidth

            Select Case (currentState)

                case MediaElementState.Opening:
                case MediaElementState.Buffering:
                case MediaElementState.AcquiringLicense:
                default:
                    ThumbnailDownloader.DisableThumbnailDownload()

                case MediaElementState.Playing:
                case MediaElementState.Paused:
                case MediaElementState.Stopped:
                case MediaElementState.Closed:

                    If ( Not IsDesignTime) Then
                        ThumbnailDownloader.EnableThumbnailDownload()
                    End If
            End Select '    currentState

            UpdateBufferingControls()


            If (StateChanged  IsNot Nothing) Then
                StateChanged(sender, e)
            End If
        End Sub '   OnMediaElementCurrentStateChanged

        ''' <summary>
        ''' Event handler for the media opened event from the media element.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnMediaElementMediaOpened(sender As Object, e As RoutedEventArgs)

            ShowErrorMessage(Nothing)
#if DEBUG
            Debug.WriteLine("OnMediaElementMediaOpened: " + DateTime.Now.ToString())
#endif
            '  hide the "start" button if present and visible

            If (m_buttonStart  IsNot Nothing  AndAlso  m_buttonStart.Visibility  <>  Visibility.Collapsed) Then
                m_buttonStart.Visibility = Visibility.Collapsed
            End If


            DisplayPoster(-1)
            EnableCaptionsArea()
            UpdateCaptionData()
            PerformResize()

            SetPlaybackDuration(m_mediaElement.NaturalDuration.TimeSpan.TotalSeconds)
            UpdateDurationDisplay()
            UpdateCanStep()

            '  Start playing or Pausing the item depending on user settings and current play state.

            If (Playlist.AutoPlay) Then
                InternalPlay()
            ElseIf (Playlist.AutoLoad) Then
                InternalPause()
            End If
        End Sub '   OnMediaElementMediaOpened

        ''' <summary>
        ''' Event handler for the media ended event from the media element.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnMediaElementMediaEnded(sender As Object, e As RoutedEventArgs)

            Debug.WriteLine("OnMediaElementMediaEnded")


            If ((Me.m_analyticsOptional  IsNot Nothing)  AndAlso  ( Not string.IsNullOrEmpty(m_cpuStatsReport))) Then

                Dim currentVideoSource As String  = string.Empty


                If (Me.m_currentPlaylistIndex >= 0  AndAlso  Me.m_currentPlaylistIndex < Playlist.Items.Count) Then
                    PlaylistItem playlistItem = Playlist.Items(Me.m_currentPlaylistIndex)
                    currentVideoSource = playlistItem.MediaSource.ToString()
                End If

                WriteCpuStatData(Me.m_cpuStatsReport, currentVideoSource, Me.m_cpuSampleSum, Me.m_cpuSampleCount, Me.m_cpuPeakSample)
                Me.m_cpuSampleSum = 0
                Me.m_cpuSampleCount = 1
                Me.m_cpuPeakSample = 0
            End If

            GoToNextPlaylistItem()
        End Sub '   OnMediaElementMediaEnded


        ''' <summary>
        ''' Event handler for the media failed event from the media element.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnMediaElementMediaFailed(sender As Object, e As ExceptionRoutedEventArgs)

            Debug.WriteLine("OnMediaElementMediaFailed")

            '  Work-around the fact that silverlight doesn't do any http retries when 404 is returned on 1st attempt -- but a number of CDNs do exactly that on the 1st attempt
             m_mediaFailureRetryCount += 1


            If ((m_mediaFailureRetryCount < maxMediaFailureRetries) Then
            '  With offline playback -- either the isolated storage file is there or it isn't -- no point in retrying
             AndAlso  ( Not MediaPlayer.IsOffline)
             AndAlso  (Playlist  IsNot Nothing)
             AndAlso  (m_currentPlaylistIndex >= 0)
             AndAlso  (Playlist.Items.Count > m_currentPlaylistIndex))
            {
                PlaylistItem playlistItem = Playlist.Items(m_currentPlaylistIndex)
            End If
                '  SmoothStreaming has it's own retry logic -- don't be redundant

                If ( Not playlistItem.IsAdaptiveStreaming ) Then
                    ShowErrorMessage(m_mediaFailureRetryCount.ToString(CultureInfo.CurrentCulture))
                    DoOpenPlaylistItem(playlistItem)
                    return
                End If

            }

            Dim sourceUri As Uri  = Nothing


            If (m_mediaElement.Source  IsNot Nothing) Then
                sourceUri = m_mediaElement.Source
            ElseIf (m_currentPlaylistIndex >= 0  AndAlso  m_currentPlaylistIndex < Playlist.Items.Count) Then
                sourceUri = Playlist.Items(m_currentPlaylistIndex).MediaSource
            End If


            Dim strErrorMessage As String  = string.Empty


            If (sourceUri  IsNot Nothing) Then

                Dim strUrl As String  = string.Empty


                If ( sourceUri.IsAbsoluteUri ) Then
                    strUrl = sourceUri.AbsolutePath
                else
                    strUrl = sourceUri.OriginalString
                End If


                strErrorMessage = String.Format(CultureInfo.CurrentUICulture, ExpressionMediaPlayer.Resources.errorInvalidMedia, strUrl)
            End If



            If (m_currentPlaylistIndex >= 0  AndAlso  m_currentPlaylistIndex < Playlist.Items.Count  AndAlso  Playlist.Items(m_currentPlaylistIndex).IsAdaptiveStreaming) Then
                strErrorMessage = String.Format(CultureInfo.CurrentUICulture, ExpressionMediaPlayer.Resources.errorNonSmoothStreamingServer, strErrorMessage)
            End If


            strErrorMessage += ChrW(13) & ChrW(10) + e.ErrorException.Message.ToString()

            Debug.WriteLine("Failure message=" + strErrorMessage)

            ShowErrorMessage(strErrorMessage)
        End Sub '   OnMediaElementMediaFailed


        ''' <summary>
        ''' Event handler for the mouse down event from the media element.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnMediaElementMouseDown(sender As Object, e As RoutedEventArgs)


            If ((DateTime.Now - m_lastMediaElementClick).TotalMilliseconds < 300) Then
                TogglePlayPause()
                ToggleFullScreen()
            else
                TogglePlayPause()
            End If


            m_lastMediaElementClick = DateTime.Now
        End Sub '   OnMediaElementMouseDown


        ''' <summary>
        ''' Event handler for the mouse moved event from the stretch box.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnStretchBoxMouseMove(sender As Object, e As MouseEventArgs)


            If (Application.Current.Host.Content.IsFullScreen) Then
                GoToControlState(ExitFullScreen)
            End If
        End Sub '   OnStretchBoxMouseMove

        ''' <summary>
        ''' Event handler for the download progress event from the media element.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnMediaElementDownloadProgressChanged(sender As Object, e As RoutedEventArgs)

#if DEBUG_EXTRA
            Debug.WriteLine("OnMediaElementDownloadProgressChanged:" + e.ToString())
#endif
            m_downloadProgressNeedsUpdating = true
        End Sub '   OnMediaElementDownloadProgressChanged


        ''' <summary>
        ''' Click handler for the start button.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonClickStart(sender As Object, e As RoutedEventArgs)

            GoToPlaylistItemOnNextTick(0)
        End Sub '   OnButtonClickStart


        ''' <summary>
        ''' Click handler for the offline button.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonClickOffline(sender As Object, e As RoutedEventArgs)


            If (Application.Current.InstallState = InstallState.NotInstalled) Then

                If (Playlist.ContentNeedsDownloading()) Then

                    Dim enoughSpace As Boolean  = Playlist.EnsureStorageSpace()


                    If (enoughSpace) Then
                        EnqueueTakeContentOffline()
                    End If
                End If
            End If
        End Sub '   OnButtonClickOffline

        ''' <summary>
        ''' Click handler for the popout button.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonClickPopOut(sender As Object, e As RoutedEventArgs)

            PopOutWindow()
        End Sub '   OnButtonClickPopOut


        ''' <summary>
        ''' Click handler for the PlugIn button.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonClickPlugIn(sender As Object, e As RoutedEventArgs)

            ToggleButton toggleButton = (ToggleButton)sender
            ShowPlugIn(toggleButton.IsChecked = true)
        End Sub '   OnButtonClickPlugIn


        ''' <summary>
        ''' show/hide plugin control
        ''' </summary>
        ''' <param name="show">true for show, false for hide</param>
        <ScriptableMember>
        public virtual Sub ShowPlugIn(show As Boolean)

            VisualStateManager.GoToState(this, ((show) ? "showPlugIn" : "hidePlugIn"), true)
        End Sub '   ShowPlugIn


        ''' <summary>
        ''' Click handler for the play and pause button.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonClickPlayPause(sender As Object, e As RoutedEventArgs)

            '  If the big "play" button is shown -- restart playback from the 1st item in the playlist

            If (m_buttonStart  IsNot Nothing  AndAlso  m_buttonStart.Visibility = Visibility.Visible) Then
                OnButtonClickStart(sender, e)
                return
            End If


            TogglePlayPause()
        End Sub '   OnButtonClickPlayPause


        ''' <summary>
        ''' Click handler for the stop button.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonClickStop(sender As Object, e As RoutedEventArgs)

            ButtonClickStopLogic()
        End Sub '   OnButtonClickStop


        private Sub ButtonClickStopLogic()

            m_inPlayState = false
            m_currentPlaylistIndex = 0
            m_currentChapterIndex = 0
            DisplayPoster(m_currentPlaylistIndex)

            If (m_mediaElement  IsNot Nothing) Then
                m_mediaElement.Stop()
                m_mediaElement.AutoPlay = false
                m_mediaElement.Source = Nothing
            End If


            If (m_buttonStart  IsNot Nothing) Then
                m_buttonStart.Visibility = Visibility.Visible
            End If


            If (m_buttonPlayPause  IsNot Nothing) Then
                m_buttonPlayPause.IsChecked = false
            End If
        End Sub '   ButtonClickStopLogic

        ''' <summary>
        ''' Click handler for the previous button.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonClickPrevious(sender As Object, e As RoutedEventArgs)

            SeekToPreviousItem()
        End Sub '   OnButtonClickPrevious


        ''' <summary>
        ''' Click handler for the next button.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonClickNext(sender As Object, e As RoutedEventArgs)

            SeekToNextItem()
        End Sub '   OnButtonClickNext


        ''' <summary>
        ''' seek player to start of current playlistitem
        ''' </summary>
        <ScriptableMember>
        public virtual Sub SeekToStart()


            If ((m_currentPlaylistIndex >= 0) Then
             AndAlso  (m_currentPlaylistIndex < Playlist.Items.Count)
             AndAlso  (Playlist.Items(m_currentPlaylistIndex)  IsNot Nothing))
            {
                '  Pause playback -- frame step mode isn't very useful otherwise.
                if (m_inPlayState)
                {
                    InternalPause()
            End If
                }

                SeekToTime(0)
            }
        End Sub '   SeekToStart


        ''' <summary>
        ''' seek player to end of current playlist item
        ''' </summary>
        <ScriptableMember>
        public virtual Sub SeekToEnd()


            If ((m_currentPlaylistIndex >= 0) Then
             AndAlso  (m_currentPlaylistIndex < Playlist.Items.Count)
             AndAlso  (Playlist.Items(m_currentPlaylistIndex)  IsNot Nothing)
             AndAlso  (Playlist.Items(m_currentPlaylistIndex).SmpteFrameRate  <>  SmpteFrameRate.Unknown))
            {
                '  Pause playback -- frame step mode isn't very useful otherwise.
                if (m_inPlayState)
                {
                    InternalPause()
            End If
                }
                SeekToTime(m_mediaElement.NaturalDuration.TimeSpan.TotalSeconds)
            }
        End Sub '   SeekToEnd


        ''' <summary>
        ''' scriptable access to volume
        ''' </summary>
        <ScriptableMember>
        <Category("Media"), Description("Set player volume 0.0(off) to 1.0(full)")>
        public virtual double Volume
        {
            get
            {
                return m_mediaElement.Volume
            }
            set
            {
                UnMuteAt(value)
            }
        }

        ''' <summary>
        ''' Scriptable access to Mute
        ''' </summary>
        <ScriptableMember>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        public virtual bool Mute
        {
            get
            {
                return m_mediaElement.Volume = 0.0f
            }
            set
            {

                If (value = true) Then
                    CacheVolumeLevel()
                else
                    UnMute()
                End If
            }
        }

        ''' <summary>
        ''' Increments the volume by the given amount.
        ''' </summary>
        ''' <param name="dblVolumeIncrement">Amount to increment the volume.</param>
        private Sub VolumeIncrement(dblVolumeIncrement As Double)

            Dim dblVolume As Double  = m_mediaElement.Volume

            dblVolume = Math.Min(1.0, Math.Max(0.0, dblVolume + dblVolumeIncrement))
            UnMuteAt(dblVolume)
        End Sub '   VolumeIncrement


        ''' <summary>
        ''' Click handler for the volume down button.
        ''' </summary>
        ''' <param name="sender">Source of this event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonClickVolumeDown(sender As Object, e As RoutedEventArgs)


            If (m_sliderVolume  IsNot Nothing) Then
                VolumeIncrement(-m_sliderVolume.SmallChange)
            else
                VolumeIncrement(-0.1)
            End If
        End Sub '   OnButtonClickVolumeDown

        ''' <summary>
        ''' Click handler for the volume up button.
        ''' </summary>
        ''' <param name="sender">Source of this event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonClickVolumeUp(sender As Object, e As RoutedEventArgs)


            If (m_sliderVolume  IsNot Nothing) Then
                VolumeIncrement(m_sliderVolume.SmallChange)
            else
                VolumeIncrement(0.1)
            End If
        End Sub '   OnButtonClickVolumeUp

        ''' <summary>
        ''' Event handler for the volume changed event.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnSliderVolumeChanged(sender As Object, e As RoutedEventArgs)

#if DEBUG
            Debug.WriteLine("OnSliderVolumeChanged current value:" + m_sliderVolume.Value.ToString())
#endif

            If (m_volumeCacheSuppressLevel < 1) Then
                UnMuteAt(m_sliderVolume.Value)
            End If
        End Sub '   OnSliderVolumeChanged

        ''' <summary>
        ''' Unmute the volume.
        ''' </summary>
        ''' <returns>Returns true.</returns>
        private Sub UnMuteAt(unmutedVolume As Double)


            If ( (m_buttonMute  IsNot Nothing)  AndAlso  ((m_buttonMute.IsChecked Is Nothing)  OrElse  (true = m_buttonMute.IsChecked))) Then
                m_buttonMute.IsChecked = false
            End If


            UnCacheVolumeLevelAt(unmutedVolume)
        End Sub '   UnMuteAt


        ''' <summary>
        ''' Unmute the volume.
        ''' </summary>
        private Sub UnMute()

            UnMuteAt(Double.NaN)
        End Sub '   UnMute


        ''' <summary>
        ''' Caches the current volume level.
        ''' </summary>
        private Sub CacheVolumeLevel()

            m_mutedCache = true


            If (m_mediaElement  IsNot Nothing) Then
                m_dblUnMutedVolume = m_mediaElement.Volume
                m_mediaElement.Volume = 0.0
#if DEBUG
                Debug.WriteLine("CacheVolumeLevel cached value:" + m_dblUnMutedVolume.ToString())
#endif
            End If


            If (m_sliderVolume  IsNot Nothing) Then
                m_volumeCacheSuppressLevel += 1
                m_sliderVolume.Value = 0.0
                m_volumeCacheSuppressLevel -= 1
            End If



            If ((m_buttonMute  IsNot Nothing)  AndAlso  ((m_buttonMute.IsChecked Is Nothing)  OrElse  (false = m_buttonMute.IsChecked))) Then
                m_buttonMute.IsChecked = true
            End If
        End Sub '   CacheVolumeLevel

        ''' <summary>
        ''' Uncaches the current volume level with a supplied volume level
        ''' </summary>
        private Sub UnCacheVolumeLevelAt(unmutedVolume As Double)

            m_mutedCache = false


            If (m_dblUnMutedVolume < VolumeMuteThreshold) Then
                m_dblUnMutedVolume = VolumeDefault
            End If



            If ( Not Double.IsNaN(unmutedVolume)) Then
                m_dblUnMutedVolume = unmutedVolume
            End If



            If (m_mediaElement  IsNot Nothing) Then
                m_mediaElement.Volume = m_dblUnMutedVolume
            End If


            If (m_sliderVolume  IsNot Nothing) Then
                m_volumeCacheSuppressLevel += 1
                m_sliderVolume.Value = m_dblUnMutedVolume
                m_volumeCacheSuppressLevel -= 1
#if DEBUG
                Debug.WriteLine("UnCacheVolumeLevelAt value now:" + m_sliderVolume.Value.ToString())
#endif
            End If
        End Sub '   UnCacheVolumeLevelAt

        ''' <summary>
        ''' Uncaches the current volume level.
        ''' </summary>
        private Sub UnCacheVolumeLevel()

            UnCacheVolumeLevelAt(Double.NaN)
        End Sub '   UnCacheVolumeLevel


        ''' <summary>
        ''' Click handler for the Mute button.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonClickMute(sender As Object, e As RoutedEventArgs)


            If (m_buttonMute.IsChecked = false  AndAlso  (m_mutedCache  OrElse  m_mediaElement.Volume < VolumeMuteThreshold)) Then
                UnCacheVolumeLevel()
            else
                CacheVolumeLevel()
            End If
        End Sub '   OnButtonClickMute

        ''' <summary>
        ''' step one frame forwards
        ''' </summary>
        private Sub StepForwards()


            If ((m_currentPlaylistIndex >= 0) Then
                 AndAlso  (m_currentPlaylistIndex < Playlist.Items.Count)
                 AndAlso  (Playlist.Items(m_currentPlaylistIndex)  IsNot Nothing)
                 AndAlso  (Playlist.Items(m_currentPlaylistIndex).SmpteFrameRate  <>  SmpteFrameRate.Unknown)
                 AndAlso  (m_mediaElement.Position.TotalSeconds < m_mediaElement.NaturalDuration.TimeSpan.TotalSeconds))
            {
                '  Pause playback -- frame step mode isn't very useful otherwise.
                if (m_inPlayState)
                {
                    InternalPause()
            End If
                }

                TimeCode oneFrame = New TimeCode("00:00:00:01", Playlist.Items(m_currentPlaylistIndex).SmpteFrameRate)
                TimeCode current = TimeCode.FromAbsoluteTime((Double)m_mediaElement.Position.TotalSeconds, Playlist.Items(m_currentPlaylistIndex).SmpteFrameRate)
                TimeCode newPosition = current.Add(oneFrame)
                SeekToTime(newPosition.TotalSeconds)
            }
        End Sub '   StepForwards


        ''' <summary>
        ''' step one frame backwards
        ''' </summary>
        private Sub StepBackwards()


            If ((m_currentPlaylistIndex >= 0)  AndAlso  (m_currentPlaylistIndex < Playlist.Items.Count)  AndAlso  (Playlist.Items(m_currentPlaylistIndex)  IsNot Nothing)  AndAlso  (Playlist.Items(m_currentPlaylistIndex).SmpteFrameRate  <>  SmpteFrameRate.Unknown)  AndAlso  (m_mediaElement.Position.TotalSeconds > 0)) Then
                '  Pause playback -- frame step mode isn't very useful otherwise.

                If (m_inPlayState) Then
                    InternalPause()
                End If


                TimeCode oneFrame = New TimeCode("00:00:00:01", Playlist.Items(m_currentPlaylistIndex).SmpteFrameRate)

                TimeCode current = TimeCode.FromAbsoluteTime((Double)m_mediaElement.Position.TotalSeconds, Playlist.Items(m_currentPlaylistIndex).SmpteFrameRate)

                TimeCode newPosition
                Try
                    newPosition = current.Subtract(oneFrame)

                Catch ex As OverflowException

                    newPosition = New TimeCode("00:00:00:00", Playlist.Items(m_currentPlaylistIndex).SmpteFrameRate);
                End Try


                SeekToTime(newPosition.TotalSeconds)
            End If
        End Sub '   StepBackwards

        ''' <summary>
        ''' Event handler for the step forwards event.
        ''' </summary>
        ''' <param name="sender">Source of this event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonStepForwards(sender As Object, e As RoutedEventArgs)

            StepForwards()
        End Sub '   OnButtonStepForwards


        ''' <summary>
        ''' Event handler for the step backwards button.
        ''' </summary>
        ''' <param name="sender">Source of this event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonStepBackwards(sender As Object, e As RoutedEventArgs)

            StepBackwards()
        End Sub '   OnButtonStepBackwards


        ''' <summary>
        ''' fixup for frame accuracy. Always step forward one frame.
        ''' </summary>
        private Sub FixupFrameStep()


            If (Playlist.DisplayTimeCodeµ) AndAlso  (CType(GetValue(FrameStepVisibilityProperty, Visibility)) = Visibility.Visible)) Then
                StepForwards()
            End If
        End Sub '   FixupFrameStep

        ''' <summary>
        ''' Event handler for the resized event.
        ''' </summary>
        ''' <param name="sender">Source of this event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnResized(sender As Object, e As EventArgs)

            PerformResize()
            Me.repositionCaptionPopup = true
        End Sub '   OnResized


        ''' <summary>
        ''' Event handler for the zoom event.
        ''' </summary>
        ''' <param name="sender">Source of this event.</param>
        ''' <param name="e">Event args.</param>
        Sub OnZoomed(sender As Object, e As EventArgs)

            PerformResize()
            Me.repositionCaptionPopup = true
        End Sub '   OnZoomed


        ''' <summary>
        ''' Event handler for Application Exit event.
        ''' </summary>
        ''' <param name="sender">Source of this event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnAppExit(sender As Object, e As EventArgs)

            Stop()
        End Sub '   OnAppExit


        ''' <summary>
        ''' Event handler for the full screen changed event.
        ''' </summary>
        ''' <param name="sender">Source of this event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnFullScreenChanged(sender As Object, e As EventArgs)

            PerformResize()
            Me.repositionCaptionPopup = true
        End Sub '   OnFullScreenChanged


        ''' <summary>
        ''' Click handler for the full screen button.
        ''' </summary>
        ''' <param name="sender">Source of this event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonClickFullScreen(sender As Object, e As RoutedEventArgs)

            ToggleFullScreen()
        End Sub '   OnButtonClickFullScreen


        ''' <summary>
        ''' Event handler for changing the playlist item.
        ''' </summary>
        ''' <param name="sender">Source of this event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnListBoxSelectionChangedPlaylist(sender As Object, e As RoutedEventArgs)


            If (m_selectorPlaylist  IsNot Nothing  AndAlso  GetHideOnClick(m_selectorPlaylist)) Then

                If (m_buttonPlaylist  IsNot Nothing) Then
                    m_buttonPlaylist.IsChecked = false
                End If

                VisualStateManager.GoToState(this, "hidePlaylist", true)
            End If

            GoToPlaylistItemOnNextTick(m_selectorPlaylist.SelectedIndex)
        End Sub '   OnListBoxSelectionChangedPlaylist


        ''' <summary>
        ''' Event handler for the chapters item changed event.
        ''' </summary>
        ''' <param name="sender">Source of this event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnListBoxSelectionChangedChapters(sender As Object, e As RoutedEventArgs)


            If (m_selectorChapters  IsNot Nothing  AndAlso  GetHideOnClick(m_selectorChapters)) Then

                If (m_buttonChapter  IsNot Nothing) Then
                    m_buttonChapter.IsChecked = false
                End If


                VisualStateManager.GoToState(this, "hideChapters", true)
            End If


            m_currentChapterIndex = m_selectorChapters.SelectedIndex

            If ((m_currentPlaylistIndex >= 0)  AndAlso  (m_currentPlaylistIndex < Playlist.Items.Count)  AndAlso  (m_currentChapterIndex >= 0)  AndAlso  (m_currentChapterIndex < Playlist.Items(m_currentPlaylistIndex).Chapters.Count)) Then
                SeekToTime(Playlist.Items(m_currentPlaylistIndex).Chapters(m_currentChapterIndex).Position)
            End If
        End Sub '   OnListBoxSelectionChangedChapters

        ''' <summary>
        ''' Event handler for the marker reached event from the media element.
        ''' </summary>
        ''' <param name="sender">Source of this event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnMediaElementMarkerReached(sender As Object, e As TimelineMarkerRoutedEventArgs)

#if DEBUG
            Debug.WriteLine("OnMediaElementMarkerReached:" + e.ToString())
#endif
            ' Test if this a "Marker" vs a "Caption"

            If ((m_selectorChapters  IsNot Nothing)  AndAlso  e.Marker.Type.Equals(MarkerType)) Then
                '  compute current chapter index from playback position
                m_currentChapterIndex = ChapterIndexFromPosition(e.Marker.Time)
            End If


            '  Display marker or caption text in the caption area
            Dim oType As String  = e.Marker.Type.ToUpper(CultureInfo.InvariantCulture)


            If (oType.Equals(CaptionType)) Then
                SetCaptionText(e.Marker.Text)
            End If



            If (MarkerReached  IsNot Nothing) Then
                ScriptableTimelineMarkerRoutedEventArgs timelineMarkerEventArgs = New ScriptableTimelineMarkerRoutedEventArgs( New ScriptableTimelineMarker(e.Marker))
                MarkerReached(sender, timelineMarkerEventArgs)
            End If
        End Sub '   OnMediaElementMarkerReached

        ''' <summary>
        ''' Helper routine for playing the current item.
        ''' </summary>
        private Sub InternalPlay()

            InternalPlayBase(false)
        End Sub '   InternalPlay


        private Sub InternalPlayAfterDrag()

            InternalPlayBase(true)
        End Sub '   InternalPlayAfterDrag


        private Sub InternalPlayBase(restoreAfterDrag As Boolean)


            If (m_buttonStart  IsNot Nothing) Then
                m_buttonStart.Visibility = Visibility.Collapsed
            End If



            If (m_mediaElement  IsNot Nothing) Then

                If (restoreAfterDrag) Then
                    m_playWhenMediaElementReady = true
                ElseIf ( Not RewindLogic()) Then
                    m_playWhenMediaElementReady = true
                End If
            End If
        End Sub '   InternalPlayBase


        private Function RewindLogic()

            Me.CurrentState = MediaElementState.Stopped
            Dim rewind As Boolean  = MediaElementState.Stopped

            If (  Not rewind ) Then
                '  Within 1 second of end of play of last item in list? do a big rewind / restart

                If ((PlaybackDuration > 0)  AndAlso  (CurrentPlaylistIndex = (Playlist.Items.Count - 1))) Then
                    var diff = PlaybackDuration - Position
                    Debug.WriteLine("RewindLogic diff=" + diff.ToString())

                    If (diff < 1.0) Then
                        rewind = true
                    End If
                End If
            End If


            If (rewind) Then
                GoToPlaylistItemOnNextTick(0)
                SeekToTime(0)
            End If

            return rewind
        End Function  '   RewindLogic


        ''' <summary>
        ''' This bit of defered logic was added after current builds of the SSME started throwing errors when a Play commaned is issued against the SSME prior to the SSME entering an "open" state
        ''' This work-around defers the call to play until the ME / SSME has opened and is ready to play.
        ''' </summary>
        private Sub InternalDoPlayWhenReady()


            If (m_playWhenMediaElementReady) Then

                If (m_mediaElement  IsNot Nothing) Then
                    var currentState = m_mediaElement.CurrentState
                    Debug.WriteLine("InternalDoPlayWhenReady: currentState=" + currentState.ToString())

                    Select Case (currentState)

                        '  Already playing -- mark flags
                        case MediaElementState.Playing:
                            m_playWhenMediaElementReady = false
                            m_inPlayState = true
                            return
                        case MediaElementState.Paused:
                        case MediaElementState.Stopped:
                            m_mediaElement.Play()
                            m_playWhenMediaElementReady = false
                            m_inPlayState = true
                            return
                        case MediaElementState.Closed:
                        case MediaElementState.Opening:
                        case MediaElementState.AcquiringLicense:
                        case MediaElementState.Buffering:
                        case MediaElementState.Individualizing:
                            Debug.WriteLine("Waiting for MediaElement to be ready to play")

                        default:
                            Debug.Assert(false, "Need to add code to handle New MediaElementState")
                            return
                    End Select '    currentState
                End If
            End If
        End Sub '   InternalDoPlayWhenReady

        ''' <summary>
        ''' Helper routine for pausing the current item.
        ''' </summary>
        private Sub InternalPause()


            If (m_buttonStart  IsNot Nothing) Then
                m_buttonStart.Visibility = Visibility.Collapsed
            End If



            If (m_mediaElement  IsNot Nothing) Then
                m_mediaElement.Pause()
            End If


            m_inPlayState = false
        End Sub '   InternalPause


        #End Region

        #region ProtectedUtilities

        ''' <summary>
        ''' Updates the position display.
        ''' </summary>
        protected virtual Sub UpdatePositionDisplay()

            SmpteFrameRate frameRate = SmpteFrameRate.Unknown

            If (Playlist.DisplayTimeCode) Then

                If ((m_currentPlaylistIndex >= 0)  AndAlso  (m_currentPlaylistIndex < Playlist.Items.Count)) Then
                    frameRate = Playlist.Items(m_currentPlaylistIndex).SmpteFrameRate
                End If
            End If

            SetPlaybackPositionText(TimeCode.ConvertToString(PlaybackPosition, frameRate))
        End Sub '   UpdatePositionDisplay


        ''' <summary>
        ''' Updates the duration display.
        ''' </summary>
        protected virtual Sub UpdateDurationDisplay()

            SmpteFrameRate frameRate = SmpteFrameRate.Unknown

            If (Playlist.DisplayTimeCode) Then

                If ((m_currentPlaylistIndex >= 0)  AndAlso  (m_currentPlaylistIndex < Playlist.Items.Count)) Then
                    frameRate = Playlist.Items(m_currentPlaylistIndex).SmpteFrameRate
                End If
            End If

            SetPlaybackDurationText(TimeCode.ConvertToString(PlaybackDuration, frameRate))
        End Sub '   UpdateDurationDisplay


        Private nextTickErrorMessage As String = Nothing
        internal Sub SetNextTickErrorMessage(message As String)


            If ( Not string.IsNullOrEmpty(Me.nextTickErrorMessage)) Then
                nextTickErrorMessage += ChrW(13) & ChrW(10)

                nextTickErrorMessage += message

            else
                nextTickErrorMessage = message
            End If
        End Sub '   SetNextTickErrorMessage

        internal static MediaPlayer sm_mediaPlayer = Nothing
        internal static Sub StaticShowErrorMessage(message As String)


            If (sm_mediaPlayer  IsNot Nothing) Then
                sm_mediaPlayer.ShowErrorMessage(message)
            End If
        End Sub '   StaticShowErrorMessage

        ''' <summary>
        ''' Shows an error message.
        ''' </summary>
        ''' <param name="message">The error message to display.</param>
        internal virtual Sub ShowErrorMessage(message As String)


            If (String.IsNullOrEmpty(message)) Then
                SetValue(ErrorMessageVisibilityProperty, Visibility.Collapsed)
                SetValue(ErrorMessageProperty, String.Empty)
                return
            End If


            SetValue(ErrorMessageProperty, message)
            SetValue(ErrorMessageVisibilityProperty, Visibility.Visible)

            '  if we cant find element for error message use popup...

            If (m_elementErrorMessage Is Nothing) Then

                If (Application.Current.InstallState = InstallState.NotInstalled  AndAlso  HtmlPage.IsEnabled) Then
                    HtmlPage.Window.Alert(message)
                else
                    MessageBox.Show(message)
                End If
            End If
        End Sub '   ShowErrorMessage

        ''' <summary>
        ''' Creates a new position timer.
        ''' </summary>
        ''' <param name="interval">Interval of the timer.</param>
        protected Sub CreatePositionTimer()


            If (m_timer Is Nothing) Then
                m_timer = New DispatcherTimer()
                '  approximately 6 NTSC frames
                m_timer.Interval = New TimeSpan(0/*days*/, 0/*hours*/, 0/*minutes*/, 0/*seconds*/,180/*milliseconds*/)
                AddHandler m_timer.Tick, AddressOf EventHandler(OnTimerTick)
            End If


            m_timer.Start()
        End Sub '   CreatePositionTimer


        ''' <summary>
        ''' UnCreates the position timer.
        ''' </summary>
        protected Sub UnCreatePositionTimer()


            If (m_timer  IsNot Nothing) Then
                AddHandler m_timer.Tick, AddressOf EventHandler(OnTimerTick)
                m_timer.Stop()
                m_timer = Nothing
            End If
        End Sub '   UnCreatePositionTimer

        ''' <summary>
        ''' Creates the timer for fading out the controls.
        ''' </summary>
        protected Sub CreateFadeoutTimer()


            If (m_timerControlFadeOut Is Nothing) Then
                m_timerControlFadeOut = New DispatcherTimer()
                '  2 seconds
                m_timerControlFadeOut.Interval = New TimeSpan(0, 0, 0, 2, 0)
                AddHandler m_timerControlFadeOut.Tick, AddressOf EventHandler(OnTimerControlFadeOutTick)
            End If


            m_timerControlFadeOut.Start()
        End Sub '   CreateFadeoutTimer


        ''' <summary>
        ''' UnCreates the timer for fading out the controls.
        ''' </summary>
        protected Sub UnCreateFadeoutTimer()


            If (m_timerControlFadeOut  IsNot Nothing) Then
                RemoveHandler m_timerControlFadeOut.Tick, AddressOf
                m_timerControlFadeOut.Stop()
                m_timerControlFadeOut = Nothing
            End If
        End Sub '   UnCreateFadeoutTimer

        ''' <summary>
        ''' Hooks our event handlers.
        ''' </summary>
        protected virtual Sub HookHandlers()

            Debug.WriteLine("UnhookHandlers:")


            If ( Not IsDesignTime) Then
                CreatePositionTimer()
                CreateFadeoutTimer()
            End If


            AddHandler Me.KeyDown, AddressOf New KeyEventHandler(MediaPlayerKeyDown)


            If (Application.Current  IsNot Nothing) Then
                Application.Current.Host.Content.FullScreenChanged += OnFullScreenChanged
                Application.Current.Host.Content.Resized += OnResized
                Application.Current.Host.Content.Zoomed += OnZoomed
                Application.Current.Exit += OnAppExit
            End If



            If (m_elementStretchBox  IsNot Nothing) Then
                m_elementStretchBox.MouseMove += OnStretchBoxMouseMove
            End If



            If (m_buttonStart  IsNot Nothing) Then
                m_buttonStart.Click += OnButtonClickStart
            End If



            If (m_buttonOffline  IsNot Nothing) Then
                m_buttonOffline.Click += OnButtonClickOffline
            End If



            If (m_buttonPopOut  IsNot Nothing) Then
                m_buttonPopOut.Click += OnButtonClickPopOut
            End If



            If (m_buttonPlugIn  IsNot Nothing) Then
                m_buttonPlugIn.Click += OnButtonClickPlugIn
            End If



            If (m_buttonPlayPause  IsNot Nothing) Then
                m_buttonPlayPause.Click += OnButtonClickPlayPause
            End If



            If (m_buttonStop  IsNot Nothing) Then
                m_buttonStop.Click += OnButtonClickStop
            End If



            If (m_buttonPrevious  IsNot Nothing) Then
                m_buttonPrevious.Click += OnButtonClickPrevious
            End If



            If (m_buttonNext  IsNot Nothing) Then
                m_buttonNext.Click += OnButtonClickNext
            End If



            If (m_buttonMute  IsNot Nothing) Then
                m_buttonMute.Click += OnButtonClickMute
            End If



            If (m_buttonStepBackwards  IsNot Nothing) Then
                m_buttonStepBackwards.Click += OnButtonStepBackwards
            End If



            If (m_buttonStepForwards  IsNot Nothing) Then
                m_buttonStepForwards.Click += OnButtonStepForwards
            End If


	        if (m_buttonClosedCaptions  IsNot Nothing)
            {
                m_buttonClosedCaptions.Click += OnButtonClickClosedCaptions
            }


            If (m_listboxCaptionOptions IsNot Nothing) Then
                m_listboxCaptionOptions.SelectionChanged += OnCaptionOptionsSelectedIndexChanged
            End If



            If ( m_popupClosedCaptionsOptionsMenu  IsNot Nothing ) Then
                m_popupClosedCaptionsOptionsMenu.Closed += OnClosedCaptionsOptionsMenu_Closed
            End If



            If (m_buttonVolumeDown  IsNot Nothing) Then
                m_buttonVolumeDown.Click += OnButtonClickVolumeDown
            End If



            If (m_buttonVolumeUp  IsNot Nothing) Then
                m_buttonVolumeUp.Click += OnButtonClickVolumeUp
            End If



            If (m_buttonFullScreen  IsNot Nothing) Then
                m_buttonFullScreen.Click += OnButtonClickFullScreen
            End If



            If (m_buttonPlaylist  IsNot Nothing) Then
                m_buttonPlaylist.Click += OnButtonClickPlaylist
            End If



            If (m_selectorPlaylist  IsNot Nothing) Then
                m_selectorPlaylist.SelectionChanged += OnListBoxSelectionChangedPlaylist
            End If



            If (m_selectorChapters  IsNot Nothing) Then
                m_selectorChapters.SelectionChanged += OnListBoxSelectionChangedChapters
            End If



            If (m_buttonChapter  IsNot Nothing) Then
                m_buttonChapter.Click += OnButtonClickChapter
            End If



            If (m_sliderPosition  IsNot Nothing) Then
                m_sliderPosition.ValueChanged += OnSliderPositionChanged
                m_sliderPosition.DragStarted += OnSliderPositionDragStarted
                m_sliderPosition.DragCompleted += OnSliderPositionDragCompleted
            End If



            If (m_sliderVolume  IsNot Nothing) Then
                m_sliderVolume.ValueChanged += OnSliderVolumeChanged
            End If



            If ( Not IsDesignTime) Then
                ThumbnailDownloader.EnableThumbnailDownload()
            End If
        End Sub '   HookHandlers

        private Sub UnhookMediaElementEvents()

            Debug.WriteLine("UnhookMediaElementEvents:")

            If (m_mediaElement  IsNot Nothing) Then
                m_mediaElementForConventionalContent).ToString() + " adpt=" + (m_mediaElement = m_mediaElementForSmoothStreamingContent).ToString())
                Debug.WriteLine("UnhookMediaElementEvents: conv=" + (m_mediaElement = m_mediaElementForSmoothStreamingContent).ToString())
                m_mediaElement.MediaFailed -= OnMediaElementMediaFailed
                m_mediaElement.MediaOpened -= OnMediaElementMediaOpened
                m_mediaElement.MediaEnded -= OnMediaElementMediaEnded
                m_mediaElement.CurrentStateChanged -= OnMediaElementCurrentStateChanged
                m_mediaElement.MarkerReached -= OnMediaElementMarkerReached
                m_mediaElement.DownloadProgressChanged -= OnMediaElementDownloadProgressChanged
                m_mediaElement.MouseLeftButtonDown -= OnMediaElementMouseDown
                return
            End If

            Debug.Assert(false, "Shouldn't ever be here Not ")
        End Sub '   UnhookMediaElementEvents


        private Sub HookMediaElementEvents()

            Debug.WriteLine("HookMediaElementEvents:")

            If (m_mediaElement  IsNot Nothing) Then
                m_mediaElementForConventionalContent).ToString() + " adpt=" + (m_mediaElement = m_mediaElementForSmoothStreamingContent).ToString())
                Debug.WriteLine("HookMediaElementEvents: conv=" + (m_mediaElement = m_mediaElementForSmoothStreamingContent).ToString())
                m_mediaElement.MediaFailed += OnMediaElementMediaFailed
                m_mediaElement.MediaOpened += OnMediaElementMediaOpened
                m_mediaElement.MediaEnded += OnMediaElementMediaEnded
                m_mediaElement.CurrentStateChanged += OnMediaElementCurrentStateChanged
                m_mediaElement.MarkerReached += OnMediaElementMarkerReached
                m_mediaElement.DownloadProgressChanged += OnMediaElementDownloadProgressChanged
                m_mediaElement.MouseLeftButtonDown += OnMediaElementMouseDown
                return
            End If

            Debug.Assert(false, "Shouldn't ever be here Not ")
        End Sub '   HookMediaElementEvents


        ''' <summary>
        ''' clicks a button (or toggle button) programmatically if it is showing.
        ''' </summary>
        ''' <param name="buttonBase"></param>
        Private ReadOnly Property Sub() As

        ''' <summary>
        ''' Applies our cached properties.
        ''' </summary>
        protected virtual Sub ApplyProperties()

            Stop()

            SetPopOutButtonVisibility(Bool2Visibility(Me.IsPopOutAllowed))

            Dim notInstalled As Boolean  = Application.Current.InstallState=InstallState.NotInstalled
            Dim allowOffline As Boolean  = Playlist.EnableOffline AndAlso notInstalled

            SetOfflineButtonVisibility(Bool2Visibility(allowOffline))
            SetChaptersButtonVisibility(Visibility.Collapsed)
            SetPlugInButtonVisibility(Visibility.Collapsed)

            If (Playlist  IsNot Nothing) Then
                Mute = Playlist.StartMuted


                If (Playlist.Items  IsNot Nothing) Then

                    Dim moreThanOne As Boolean  = Playlist.Items.Count > 1

                    SetPlaylistButtonVisibility(moreThanOne ? Visibility.Visible : Visibility.Collapsed)

                    If ( Not moreThanOne) Then
                        ShowPlaylist(false)
                    ElseIf (Playlist.StartWithPlaylistShowing) Then
                        ShowPlaylist(true)
                    End If



                    If (Playlist.Items.Count > 0) Then

                        If (Playlist.AutoLoad  OrElse  Playlist.AutoPlay) Then
                            GoToPlaylistItemOnNextTick(0)
                        else
                            DisplayPoster(0)
                        End If
                    End If
                End If
            End If


            If (m_buttonStart  IsNot Nothing) Then
                m_buttonStart.Visibility = Playlist.AutoPlay ? Visibility.Collapsed : Visibility.Visible
            End If



            If (m_sliderVolume  IsNot Nothing) Then
                m_sliderVolume.Minimum = 0
                m_sliderVolume.Maximum = 1
                m_sliderVolume.SmallChange = 0.1
                m_sliderVolume.LargeChange = 0.2

                m_volumeCacheSuppressLevel += 1

                If (m_mutedCache) Then
                    m_sliderVolume.Value = 0.0
                else
                    m_sliderVolume.Value = m_dblUnMutedVolume
                End If

                m_volumeCacheSuppressLevel -= 1
            End If



            If (m_buttonMute  IsNot Nothing) Then
                m_buttonMute.IsChecked = m_mutedCache
            End If


            SetClosedCaptionButton(Playlist.EnableCaptions)


            If (m_mediaElement  IsNot Nothing) Then
                ApplyPropertiesToMediaElement()
            End If

            PerformResize()
        End Sub '   ApplyProperties


        private Sub ApplyPropertiesToMediaElement()

            Debug.Assert(m_mediaElement  IsNot Nothing)

            If (m_mediaElement  IsNot Nothing) Then
                m_mediaElement.CacheMode = (Playlist.EnableCachedComposition  AndAlso  (m_mediaElement.CacheMode Is Nothing)) ? New BitmapCache() : Nothing


                If (Playlist  IsNot Nothing) Then
                    m_mediaElement.AutoPlay = Playlist.AutoPlay
                End If



                If ( m_sliderVolume  IsNot Nothing ) Then
                    m_mediaElement.Volume = m_sliderVolume.Value
                End If
            End If
        End Sub '   ApplyPropertiesToMediaElement


        ''' <summary>
        ''' Adjust the size of the poster image to match the displayed video .
        ''' </summary>
        protected virtual Sub AdjustPosterSize(playlistItemIndex As Integer)

            Debug.WriteLine("AdjustPosterSize: " + playlistItemIndex.ToString())

            If ((Playlist.StretchNonSquarePixels = StretchNonSquarePixels.NoStretch) Then
                 AndAlso  ( Not Application.Current.Host.Content.IsFullScreen)
                 AndAlso  (playlistItemIndex >= 0)
                 AndAlso  (playlistItemIndex < Playlist.Items.Count)
                 AndAlso  (Playlist.Items(playlistItemIndex).VideoWidth > 0)
                 AndAlso  (Playlist.Items(playlistItemIndex).VideoHeight > 0))
            {
                var mw = Playlist.Items(playlistItemIndex).VideoWidth
            End If
                var mh = Playlist.Items(playlistItemIndex).VideoHeight
                Debug.WriteLine("AdjustPosterSize: mw=" + mw.ToString() + " mh=" + mh.ToString())
                SetPosterImageMaxWidth(mw)
                SetPosterImageMaxHeight(mh)
            }
            else
            {
                Debug.WriteLine("AdjustPosterSize: Infinite Not ")
                SetPosterImageMaxWidth(Double.PositiveInfinity)
                SetPosterImageMaxHeight(Double.PositiveInfinity)
            }
        End Sub '   AdjustPosterSize


        ''' <summary>
        ''' Displays a poster image for a playlist item.
        ''' </summary>
        ''' <param name="playlistItemIndex">Index of the item to display a poster image for.</param>
        protected virtual Sub DisplayPoster(playlistItemIndex As Integer)

            Debug.WriteLine("DisplayPoster: " + playlistItemIndex.ToString())

            If (playlistItemIndex >= 0  AndAlso  playlistItemIndex < Playlist.Items.Count) Then

                If (Playlist.Items(playlistItemIndex).ThumbSource  IsNot Nothing) Then
                    SetPosterImageSource(Playlist.Items(playlistItemIndex).ThumbSource.ToString())
                else
                    SetPosterImageSource(string.Empty)
                End If

                AdjustPosterSize(playlistItemIndex)
                Debug.WriteLine("DisplayPoster: showPosterFrame")
                VisualStateManager.GoToState(this, "showPosterFrame", true)
                return
            End If

            Debug.WriteLine("DisplayPoster: hidePosterFrame")
            VisualStateManager.GoToState(this, "hidePosterFrame", true)
        End Sub '   DisplayPoster


        ''' <summary>
        ''' Seeks to the given time.
        ''' </summary>
        ''' <param name="seconds">Time to seek to.</param>
        <ScriptableMember>
        public virtual Sub SeekToTime(seconds As Double)

            '  collapse / defer seeks
            m_seekOnNextTick = true
            m_seekOnNextTickPosition = seconds
        End Sub '   SeekToTime


        ''' <summary>
        ''' Performs the actual seek.
        ''' </summary>
        protected virtual Sub DoActualSeek()


            If ( Not m_mediaElement.CanSeek  OrElse   Not m_seekOnNextTick) Then
                m_seekOnNextTick = false
                return
            End If


            '  Don't attempt to seek unless the element is actually playing or paused

            Select Case (m_mediaElement.CurrentState)

                case MediaElementState.Playing:
                case MediaElementState.Paused:

                case MediaElementState.Opening:
                case MediaElementState.Buffering:
                case MediaElementState.AcquiringLicense:
                case MediaElementState.Stopped:
                case MediaElementState.Closed:
                default:
                    '  Defering while media isn't ready...
                    return
            End Select '    m_mediaElement.CurrentState



            If (m_sliderPosition  IsNot Nothing) Then

                If (m_sliderPosition.IsDragging  AndAlso  m_currentItemIsAdaptive) Then
                    '  Defering Seek during Drag
                    return
                End If
            End If

            '  Finaly go ahead and seek!
            ClearCaptionText()
            m_seekOnNextTick = false

            Dim seconds As Double  = Math.Min(PlaybackDuration, Math.Max(0.0, m_seekOnNextTickPosition))

            Dim newPosition As TimeSpan  = TimeSpan.FromSeconds(seconds)

            m_mediaElement.Position = newPosition

            '  update chapter index (listbox selection will be updated in the TimerTick code)
            m_currentChapterIndex = ChapterIndexFromPosition(newPosition)
        End Sub '   DoActualSeek


        ''' <summary>
        ''' Skips forwards or backwards.
        ''' </summary>
        ''' <param name="direction">Direction to skip.</param>
        protected virtual Sub SkipTime(direction As Integer)

            Dim delta As Double  = Math.Max(MinDelta, PlaybackDuration / SkipSteps)
            Dim skipbuffer As Double  = (delta - SkipBuffer)
            Dim newposition As Double  = PlaybackPosition + (delta * direction)


            If (newposition < -(skipbuffer)  AndAlso  m_currentPlaylistIndex > 0) Then
                GoToPreviousPlaylistItem()
            ElseIf ((newposition > (PlaybackDuration + skipbuffer))  AndAlso  (m_currentPlaylistIndex < (Playlist.Items.Count-1))) Then
                GoToNextPlaylistItem()
            End If

            else
            {
                newposition = Math.Max(0.0, newposition)
                newposition = Math.Min(PlaybackDuration,  newposition)
                SeekToTime(newposition)
            }
        End Sub '   SkipTime


        ''' <summary>
        ''' Seeks to the next playlist item.
        ''' </summary>
        <ScriptableMember>
        public virtual Sub SeekToNextItem()


            If ( Not SeekToChapterPoint(m_currentChapterIndex + 1)) Then
                SkipTime(1)
            End If
        End Sub '   SeekToNextItem

        ''' <summary>
        ''' Seeks to the previous playlist item.
        ''' </summary>
        <ScriptableMember>
        public virtual Sub SeekToPreviousItem()


            If ( Not SeekToChapterPoint(m_currentChapterIndex - 1)) Then
                SkipTime(-1)
            End If
        End Sub '   SeekToPreviousItem

        ''' <summary>
        ''' Finds a chapter index from a position.
        ''' </summary>
        ''' <param name="position">The position we are looking for.</param>
        ''' <returns>The index of the chapter item for this position.</returns>
        protected Function ChapterIndexFromPosition(position As TimeSpan) As Integer

            Dim seconds As Double  = position.TotalSeconds

            Dim indexChapter As Integer  = 0


            If ((m_currentPlaylistIndex >= 0)  AndAlso  (m_currentPlaylistIndex < Playlist.Items.Count)) Then

                While (indexChapter < Playlist.Items(m_currentPlaylistIndex).Chapters.Count  AndAlso  Playlist.Items(m_currentPlaylistIndex).Chapters(indexChapter).Position < seconds)

                    indexChapter += 1
                End While   '
            End If

            return indexChapter
        End Function  '   ChapterIndexFromPosition


        ''' <summary>
        ''' Seeks to a chapter point.
        ''' </summary>
        ''' <param name="chapterIndex">The index of the chapter point to seek to.</param>
        ''' <returns>true if we found the index, false otherwise.</returns>
        <ScriptableMember>
        public virtual bool SeekToChapterPoint(int chapterIndex)
        {

            If ((m_currentPlaylistIndex >= 0)  AndAlso  (m_currentPlaylistIndex < Playlist.Items.Count)) Then

                If ((chapterIndex >= 0)  AndAlso  (chapterIndex < Playlist.Items(m_currentPlaylistIndex).Chapters.Count)) Then
                    m_currentChapterIndex = chapterIndex
                    m_selectorChapters.SelectedIndex = m_currentChapterIndex
                    return true
                End If
            End If

            return false
        }

        ''' <summary>
        ''' Toggles our play/pause state.
        ''' </summary>
        protected virtual Sub TogglePlayPause()


            If (m_mediaElement Is Nothing) Then
                '  Special case with some audio-only templates that lack the big start button -- m_mediaElement will be null until after the first item has been played
                GoToPlaylistItemOnNextTick(0)
                return
            End If


            '  Change current play state depending on current state.


            Select Case (m_mediaElement.CurrentState)

                case MediaElementState.AcquiringLicense:
                case MediaElementState.Buffering:
                case MediaElementState.Individualizing:
                case MediaElementState.Playing:
                case MediaElementState.Opening:
                    InternalPause()
                    FixupFrameStep()

                case MediaElementState.Paused:
                case MediaElementState.Closed:
                case MediaElementState.Stopped:
                    m_inPlayState = true
                    InternalPlay()
            End Select '    m_mediaElement.CurrentState
        End Sub '   TogglePlayPause

        '''
        ''' CAPTIONING SUPPORT
        '''

        ''' <summary>
        ''' Click handler for the closed captions button.
        ''' </summary>
        ''' <param name="sender">Source of this event.</param>
        ''' <param name="e">Event args.</param>
        private Sub OnButtonClickClosedCaptions(sender As Object, e As RoutedEventArgs)


            If (m_popupClosedCaptionsOptionsMenu  IsNot Nothing) Then

                If (m_popupClosedCaptionsOptionsMenu.IsOpen) Then
                    '  Popup open -- close it
                    m_popupClosedCaptionsOptionsMenu.IsOpen = false
                End If
            End If


            If (EnableCaptionsArea()) Then

                If (Me.CaptionsManager  IsNot Nothing) Then
                    '  Get the set of language choices for the current playlist item.
                    var captionOptions = Me.CaptionOptions

                    If (captionOptions  IsNot Nothing) Then
                        '  More than one language choice -- display language selection popup menu

                        If (captionOptions.Count > 1) Then
                            if (m_listboxCaptionOptions  IsNot Nothing)
                            {
                                '  clear any prior selection -- since it is the selection change event that closed the popup menu.
                                m_listboxCaptionOptions.SelectedItem = Nothing
                            }
                            if ((m_popupClosedCaptionsOptionsMenu  IsNot Nothing)  AndAlso  (m_buttonClosedCaptions  IsNot Nothing))
                            {
                                '  Show the popup menu -- but make it transparent -- the popup opened handler will reposition the menu and make it visuble
                                Me.repositionCaptionPopup = true
                                Me.m_popupClosedCaptionsOptionsMenu.HorizontalOffset = -1000
                                Me.m_popupClosedCaptionsOptionsMenu.VerticalOffset = -1000
                                Me.m_popupClosedCaptionsOptionsMenu.Opacity = 0.0
                                Me.m_popupClosedCaptionsOptionsMenu.IsOpen = true
                                return
                            }
                        else
                            '  there is only one language choice -- choose it.

                            For Each var captionOption in captionOptions

                                SetCaptionOptionData(captionOption.Value.LanguageIdTwoLetterIso, captionOption.Value)
                                return
                            Next    '   var
                        End If
                    End If
                End If
            End If
            '  If we got here -- the current playlit item has no caption data or something wierd is going on -- clear out any lurking caption data
            SetCaptionOptionData(string.Empty, Nothing)
        End Sub '   OnButtonClickClosedCaptions


        ''' <summary>
        ''' Download the supplied language version of the captions
        ''' </summary>
        ''' <param name="language">language to download</param>
        private Sub SetCaptionOptionData(languageIdTwoLetterIso As String, captionOption As CaptionOption)


            If (Me.CurrentMediaElement  IsNot Nothing  AndAlso  Playlist.Items.Count > 0  AndAlso  m_currentPlaylistIndex >= 0  AndAlso  m_currentPlaylistIndex < Playlist.Items.Count  AndAlso  Playlist.Items(m_currentPlaylistIndex)  IsNot Nothing  AndAlso  Playlist.Items(m_currentPlaylistIndex).CaptionSources  IsNot Nothing  AndAlso  Me.CaptionsManager  IsNot Nothing) Then
                Me.CaptionsManager.ClearDFXPEvents()


                If (languageIdTwoLetterIso = string.Empty  OrElse  captionOption Is Nothing) Then
                    Me.CaptionsManager.RemoveCaptionPanel()
                else
                    Me.CaptionsManager.EnsureCaptionPanel()

                    Dim captionSourceChosen As CaptionSource  = Nothing


                    For Each captionSource As CaptionSource in Playlist.Items(m_currentPlaylistIndex).CaptionSources


                        If ((String.CompareOrdinal(captionSource.ISOTwoLetterLanguageName, captionOption.LanguageIdTwoLetterIso) = 0)  AndAlso Then
                            (captionSource.Type = captionOption.Type))
                        {
                            captionSourceChosen = captionSource
                        End If

                        }
                    Next    '   captionSource



                    If (captionSourceChosen  IsNot Nothing) Then
                        Me.CaptionsManager.DownloadCaptions(Playlist.Items(m_currentPlaylistIndex).IsAdaptiveStreaming, captionSourceChosen)
                    End If
                End If
            End If
        End Sub '   SetCaptionOptionData

        ''' <summary>
        ''' Checks / Unchecks the ClosedCaptions button
        ''' </summary>
        ''' <param name="enableCaptions"></param>
        private Sub SetClosedCaptionButton(enableCaptions As Boolean)


            If (m_buttonClosedCaptions  IsNot Nothing) Then
                m_buttonClosedCaptions.IsChecked =  Not enableCaptions
            End If
        End Sub '   SetClosedCaptionButton

        ''' <summary>
        ''' initialize DXFP captions
        ''' </summary>
        private Function EnableCaptionsArea()

            Dim enableCaptionsArea As Boolean  = false


            If (Me.m_buttonClosedCaptions  IsNot Nothing) Then
                (Me.m_buttonClosedCaptions.IsChecked = false)
                enableCaptionsArea = false)
            End If

            Me.CaptionsVisibility = Bool2Visibility(enableCaptionsArea)
            return enableCaptionsArea
        End Function  '   EnableCaptionsArea


        ''' <summary>
        ''' Handler called when the langauge menu selection is changed -- in this case it simply causes the popup to close
        ''' The actual handling of the language change is dealt with in the popup closed handler -- since there are other ways to close the popup
        ''' and language needs to be set in those cases as well.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Sub OnCaptionOptionsSelectedIndexChanged(sender As Object, e As EventArgs)

            '  "Auto" close the popup after they make a selection
            m_popupClosedCaptionsOptionsMenu.IsOpen = false
        End Sub '   OnCaptionOptionsSelectedIndexChanged


        Private repositionCaptionPopup As Boolean = false
        private Sub RepositionCaptionPopup()


            If (m_popupClosedCaptionsOptionsMenu Is Nothing) Then
                return
            End If


            If ( Not m_popupClosedCaptionsOptionsMenu.IsOpen) Then
                m_popupClosedCaptionsOptionsMenu.Opacity = 0.0
                return
            End If



            If ( Not Me.repositionCaptionPopup) Then
                return
            End If


            Debug.WriteLine("RepositionCaptionPopup IsOpen=" + m_popupClosedCaptionsOptionsMenu.IsOpen.ToString() + " Opacity=" + m_popupClosedCaptionsOptionsMenu.Opacity.ToString() + " HOffset=" + m_popupClosedCaptionsOptionsMenu.HorizontalOffset.ToString() + " VOffest=" + m_popupClosedCaptionsOptionsMenu.VerticalOffset.ToString())

            Dim parentWindow As FrameworkElement  = m_elementVideoWindow


            If (Nothing = parentWindow) Then

                Dim parent As FrameworkElement  = CType(m_popupClosedCaptionsOptionsMenu.Parent,  FrameworkElement)


                While (parent  IsNot Nothing)

                    parentWindow = parent
                    parent = parent.Parent as FrameworkElement


                    If (parentWindow.GetType() = StretchBox.GetType()) Then
                    End If
                End While   '
            End If

            Debug.Assert(parentWindow  IsNot Nothing, "XAML wierdness in this template")

            If (parentWindow  IsNot Nothing) Then
                Me.repositionCaptionPopup = false
                '  reposition the popup near the closed captions button
                var transform = m_buttonClosedCaptions.TransformToVisual(parentWindow)
                var point = transform.Transform(new Point(0, 0))

                Dim menuWidth As Double  = m_listboxCaptionOptions.ActualWidth
                Dim menuHeight As Double  = m_listboxCaptionOptions.ActualHeight

                point.X = Math.Max(0, point.X)
                point.X = Math.Min((parentWindow.ActualWidth - menuWidth), point.X)
                m_popupClosedCaptionsOptionsMenu.HorizontalOffset = point.X

                point.Y = Math.Max(0, point.Y)
                point.Y = Math.Min((parentWindow.ActualHeight - menuHeight), point.Y)
                m_popupClosedCaptionsOptionsMenu.VerticalOffset = point.Y

                m_popupClosedCaptionsOptionsMenu.Opacity = 1.0

                Debug.WriteLine("RepositionCaptionPopup completed IsOpen=" + m_popupClosedCaptionsOptionsMenu.IsOpen.ToString() + " Opacity=" + m_popupClosedCaptionsOptionsMenu.Opacity.ToString() + " HOffset=" + m_popupClosedCaptionsOptionsMenu.HorizontalOffset.ToString() + " VOffest=" + m_popupClosedCaptionsOptionsMenu.VerticalOffset.ToString())
            End If
        End Sub '   RepositionCaptionPopup

        ''' <summary>
        ''' Handler for when the language selection popup is closed -- updates the captions to the selected language
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Sub OnClosedCaptionsOptionsMenu_Closed(sender As Object, e As EventArgs)

            EnableCaptionsArea()
            UpdateCaptionOptions(Me.m_listboxCaptionOptions)
        End Sub '   OnClosedCaptionsOptionsMenu_Closed


        ''' <summary>
        ''' helper function that actually effects the downloading and parsing of a captions file matching the current language settings for the current playlist item.
        ''' </summary>
        private Sub UpdateCaptionData()


            If ((Me.CaptionsManager  IsNot Nothing)  AndAlso  (Me.CaptionsVisibility = System.Windows.Visibility.Visible)) Then
                '  If there is a currently selected caption option -- load it's data
                SetCaptionOptionData(Me.CaptionsManager.CurrentISOTwoLetterLanguageName, Me.CaptionsManager.CaptionOptionForPlaylistItemAndLanguage(Me.m_currentPlaylistIndex, Me.CaptionsManager.CurrentISOTwoLetterLanguageName))
            else
                '  Clears out captions -- removes events from media element
                SetCaptionOptionData(string.Empty, Nothing)
            End If
        End Sub '   UpdateCaptionData

        ''' <summary>
        ''' Helper method for updating the selected language settting
        ''' </summary>
        ''' <param name="oListbox"></param>
        private Sub UpdateCaptionOptions(oListbox As ListBox)


            If ((oListbox  IsNot Nothing)  AndAlso  (Me.CaptionsManager  IsNot Nothing)) Then

                If (Me.CaptionsVisibility = System.Windows.Visibility.Visible) Then
                    KeyValuePair<string, CaptionOption>? selectedItem = oListbox.SelectedItem as KeyValuePair<string, CaptionOption>?

                    If (selectedItem  IsNot Nothing) Then

                        Dim captionOption As CaptionOption  = selectedItem.Value.Value


                        If (captionOption  IsNot Nothing) Then
                            Me.CaptionsManager.CurrentISOTwoLetterLanguageName = captionOption.LanguageIdTwoLetterIso
                            UpdateCaptionData()
                        End If
                    End If
                End If
            End If
        End Sub '   UpdateCaptionOptions


        ''' <summary>
        ''' scripting utility function
        ''' </summary>
        <ScriptableMember>
        public Function CreateUri(sUnri As String) As Uri

            return New Uri(sUnri, UriKind.RelativeOrAbsolute)
        End Function  '   CreateUri


        #End Region

        #region PrivateUtilityMethods

        ''' <summary>
        ''' Goes to our control state.
        ''' </summary>
        ''' <param name="controlState">Control state to go to.</param>
        private Sub GoToControlState(controlState As String)

            m_timerControlFadeOut.Stop()
            VisualStateManager.GoToState(this, controlState, true)
            currentControlState = controlState

            Dim element As FrameworkElement  = CType(Application.Current.RootVisual,  FrameworkElement)


            If (element  IsNot Nothing) Then

                If (EnterFullScreen = currentControlState) Then
                    element.Cursor = Cursors.None
                ElseIf (ExitFullScreen = currentControlState) Then
                    element.Cursor = Cursors.Arrow
                End If
            End If

            m_timerControlFadeOut.Start()
        End Sub '   GoToControlState


        ''' <summary>
        ''' Sets the desired control state.
        ''' </summary>
        private Sub SetDesiredControlState()


            If ((Application.Current  IsNot Nothing)) Then

                If (Application.Current.Host.Content.IsFullScreen) Then
                    desiredControlState = EnterFullScreen
                else
                    desiredControlState = ExitFullScreen
                End If
            End If
        End Sub '   SetDesiredControlState


        ''' <summary>
        ''' Performs a resize.
        ''' </summary>
        private Sub PerformResize()


            If ((Application.Current  IsNot Nothing)) Then

                If (Application.Current.Host.Content.IsFullScreen) Then
                    Me.HorizontalAlignment = HorizontalAlignment.Stretch
                    Me.VerticalAlignment = VerticalAlignment.Stretch
                End If
            End If

            SetDesiredControlState()

            '  special case for adjusting poster size between normal and full screen mode prior to playing the 1st item.
            AdjustPosterSize(((m_currentPlaylistIndex >= 0) ? (m_currentPlaylistIndex) : (0)))

            ApplySizeAndStretch()

            Me.m_refreshCaptionsOnNextTick = true
        End Sub '   PerformResize


        ''' <summary>
        ''' Apply size and stretch mode to the MediaElement
        ''' </summary>
        private Sub ApplySizeAndStretch()


            If (Playlist Is Nothing  OrElse  m_mediaElement Is Nothing) Then
                '  Skip when called during boot up
                return
            End If


            StretchNonSquarePixels currentMode = Me.Playlist.StretchNonSquarePixels
#if DEBUG_SIZE_AND_STRETCH
            Debug.WriteLine("ApplySizeAndStretch currentMode=" + currentMode.ToString())
#endif


            If (Application.Current.Host.Content.IsFullScreen) Then
                '  override stretchMode for FullScreen if they have none.

                If (currentMode = StretchNonSquarePixels.NoStretch) Then
                    currentMode = StretchNonSquarePixels.StretchToFill

#if DEBUG_SIZE_AND_STRETCH
                    Debug.WriteLine("IsFullScreen currentMode=" + currentMode.ToString())
#endif
                End If
            End If

            '  For "stretchy modes" -- just let mediaElement do it's thing

            If (StretchNonSquarePixels.StretchDistorted = currentMode) Then
                m_mediaElement.Width = double.NaN
                m_mediaElement.Height = double.NaN
                m_mediaElement.Stretch = Stretch.Fill

#if DEBUG_SIZE_AND_STRETCH
                Debug.WriteLine("ApplySizeAndStretch  -= 1 StretchDistorted shortcut Not ")
#endif
                return
            End If


            '  Auto sizing
            Dim widthToSet As Double  = double.NaN
            Dim heightToSet As Double  = double.NaN
            Dim mediaElementStretchMode As Stretch  = Stretch.Fill

            Dim squarePixelVideoWidth As Double  = 320
            Dim squarePixelVideoHeight As Double  = 240


            If ((m_currentPlaylistIndex >= 0)  AndAlso  (m_currentPlaylistIndex < Playlist.Items.Count)) Then
                var currentItem = Playlist.Items(m_currentPlaylistIndex)


                If ((currentItem.VideoWidth > 0)  AndAlso  (currentItem.VideoHeight > 0)  AndAlso  (currentItem.VideoHeight < short.MaxValue)  AndAlso  (currentItem.VideoHeight < short.MaxValue)) Then

                    Dim pixelWidth As Double  = currentItem.VideoWidth
                    Dim pixelHeight As Double  = currentItem.VideoHeight

#if DEBUG_SIZE_AND_STRETCH
                    Debug.WriteLine("pixelWidth=" + pixelWidth.ToString())
                    Debug.WriteLine("pixelHeight=" + pixelHeight.ToString())
#endif

                    Dim aspectRatioWidth As Double  = currentItem.AspectRatioWidth
                    Dim aspectRatioHeight As Double  = currentItem.AspectRatioHeight


                    If (double.IsNaN(aspectRatioWidth)  OrElse  double.IsNaN(aspectRatioHeight)) Then

                        Dim pixelRatio As Double  = 4/3


                        If ( pixelHeight > 0.0 ) Then
                            pixelRatio = pixelWidth / pixelHeight
                        End If


                        If (pixelRatio > 720 / 480) Then
                            aspectRatioWidth = 16
                            aspectRatioHeight = 9
                        else
                            aspectRatioWidth = 4
                            aspectRatioHeight = 3
                        End If
                    End If

#if DEBUG_SIZE_AND_STRETCH
                    Debug.WriteLine("aspectRatioWidth=" + aspectRatioWidth.ToString())
                    Debug.WriteLine("aspectRatioHeight=" + aspectRatioHeight.ToString())
#endif


                    If (aspectRatioWidth > aspectRatioHeight) Then

                        Dim squarePixelWidth As Double  = (pixelHeight * aspectRatioWidth) / aspectRatioHeight

                        squarePixelVideoWidth = squarePixelWidth
                        squarePixelVideoHeight = pixelHeight
                    else

                        Dim squarePixelHeight As Double  = (pixelWidth * aspectRatioHeight) / aspectRatioWidth

                        squarePixelVideoWidth = pixelWidth
                        squarePixelVideoHeight = squarePixelHeight
                    End If
                End If
            End If


#if DEBUG_SIZE_AND_STRETCH
            Debug.WriteLine("squarePixelVideoWidth=" + squarePixelVideoWidth.ToString())
            Debug.WriteLine("squarePixelVideoHeight=" + squarePixelVideoHeight.ToString())
#endif

            Dim scalingFactor As Double  = 1.0


            If (m_elementVideoWindow  IsNot Nothing) Then

                Dim availibleWidth As Double  = m_elementVideoWindow.ActualWidth
                Dim availibleHeight As Double  = m_elementVideoWindow.ActualHeight

                if (Application.Current.Host.Content.IsFullScreen)
                {
                    availibleWidth = Application.Current.Host.Content.ActualWidth
                    availibleHeight = Application.Current.Host.Content.ActualHeight
                }

#if DEBUG_SIZE_AND_STRETCH
                Debug.WriteLine("availibleWidth=" + availibleWidth.ToString())
                Debug.WriteLine("availibleHeight=" + availibleHeight.ToString())
#endif

                Double scalingFactorX = availibleWidth / squarePixelVideoWidth
                Double scalingFactorY = availibleHeight / squarePixelVideoHeight
                scalingFactor = Math.Min(scalingFactorX, scalingFactorY)
            else
#if DEBUG_SIZE_AND_STRETCH
                Debug.WriteLine("elementVideoWindow is missing from the template XAML Not  Not  Not ")
#endif
            End If



            If (scalingFactor < 1.0) Then
                widthToSet = squarePixelVideoWidth * scalingFactor
                heightToSet = squarePixelVideoHeight * scalingFactor
            ElseIf (StretchNonSquarePixels.StretchToFill = currentMode) Then

                widthToSet = squarePixelVideoWidth * scalingFactor
                heightToSet = squarePixelVideoHeight * scalingFactor
            End If

            else if (StretchNonSquarePixels.NoStretch = currentMode)
            {
                widthToSet = squarePixelVideoWidth
                heightToSet = squarePixelVideoHeight
            }

#if DEBUG_SIZE_AND_STRETCH
            Debug.WriteLine("ApplySizeAndStretch widthToSet=" + widthToSet.ToString() + " heightToSet=" + heightToSet.ToString() + " Stretch=" + mediaElementStretchMode.ToString())
#endif

            m_mediaElement.Width = widthToSet
            m_mediaElement.Height = heightToSet
            m_mediaElement.Stretch = mediaElementStretchMode
        End Sub '   ApplySizeAndStretch


        ''' <summary>
        ''' Clears our caption text.
        ''' </summary>
        private Sub ClearCaptionText()

            SetCaptionText(string.Empty)

            If (Me.CaptionsManager  IsNot Nothing) Then
                Me.CaptionsManager.ClearCaptions()
            End If
        End Sub '   ClearCaptionText

        ''' <summary>
        ''' Updates the frame step property.
        ''' </summary>
        private Sub UpdateCanStep()

            ' stepping isn't compatible with AdaptiveStreaming MSS currently

            If (IsDesignTime  OrElse  ((m_mediaElement  IsNot Nothing)  AndAlso  (m_mediaElement.CanSeek)  AndAlso  (m_currentPlaylistIndex >= 0)  AndAlso  (m_currentPlaylistIndex < Playlist.Items.Count)  AndAlso  (Playlist.Items(m_currentPlaylistIndex).SmpteFrameRate  <>  SmpteFrameRate.Unknown)  AndAlso  ( Not Playlist.Items(m_currentPlaylistIndex).IsAdaptiveStreaming))) Then
                SetValue(FrameStepVisibilityProperty, Visibility.Visible)
                return
            End If


            SetValue(FrameStepVisibilityProperty, Visibility.Collapsed)
        End Sub '   UpdateCanStep


        ''' <summary>
        ''' Updates the ButtonPreviousIsEnabled and ButtonNextIsEnabled properties.
        ''' </summary>
        private Sub UpdatePrevNext()


            If (m_currentPlaylistIndex >= 0  AndAlso  m_currentPlaylistIndex < Playlist.Items.Count) Then

                Dim chapterCount As Integer  = Playlist.Items(m_currentPlaylistIndex).Chapters.Count

                if (chapterCount > 0)
                {
                    SetValue(ButtonPreviousIsEnabledProperty, m_currentChapterIndex > 0)
                    SetValue(ButtonNextIsEnabledProperty, m_currentChapterIndex < (chapterCount - 1))
                }
                else
                {
                    SetValue(ButtonPreviousIsEnabledProperty, PlaybackPosition > 0.0)
                    SetValue(ButtonNextIsEnabledProperty, PlaybackPosition < PlaybackDuration)
                }
            else
                SetValue(ButtonPreviousIsEnabledProperty, false)
                SetValue(ButtonNextIsEnabledProperty, false)
            End If
        End Sub '   UpdatePrevNext

        #End Region

        #region Offline Support
        ''' <summary>
        ''' offline/online state change
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        private Sub PlayerInstallStateChanged(sender As Object, e As EventArgs)

            Debug.WriteLine("PlayerInstallStateChanged InstallState=" + Application.Current.InstallState.ToString())

            If ( Not IsDesignTime) Then
                Stop()

                Select Case (Application.Current.InstallState)

                    case InstallState.Installed:
                        Me.SetOfflineButtonVisibility(Visibility.Collapsed)
                        Me.SetOfflineButtonEnabled(false)

                    case InstallState.Installing:
                        Me.SetOfflineButtonEnabled(false)

                    case InstallState.InstallFailed:
                    case InstallState.NotInstalled:
                        IsoUri.ClearIsoStorage()
                        Me.SetOfflineButtonVisibility(Visibility.Visible)
                        Me.SetOfflineButtonEnabled(Playlist.EnableOffline)
                End Select '    Application.Current.InstallState
            End If
        End Sub '   PlayerInstallStateChanged


        private Sub EnqueueTakeContentOffline()

            Stop()
            SetOfflineDownloadProgressVisibility(Visibility.Visible)
            SetOfflineButtonEnabled(false)
            Playlist.TakeOffline(this)
        End Sub '   EnqueueTakeContentOffline


        ''' <summary>
        ''' network went on/off line
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        private Sub PlayerNetworkAddressChanged(sender As Object, e As EventArgs)


            If ( Not NetworkInterface.GetIsNetworkAvailable()  AndAlso  Playlist.IsDownloading) Then
                IsoUri.ClearIsoStorage()
                ShowErrorMessage(ExpressionMediaPlayer.Resources.errorOfflineInterrupted)
            End If
        End Sub '   PlayerNetworkAddressChanged
        #End Region

        ''' <summary>
        ''' This class contains constants for progress reporting.
        ''' </summary>
        Private sealed Class ProgressConst
        {
            ''' <summary>
            ''' Maximum progress in the control.
            ''' </summary>
            public const double MaxProgress = 1.0

            ''' <summary>
            ''' Maximum progress percent.
            ''' </summary>
            public const double MaxPercent = 100.0

            ''' <summary>
            ''' Converts a progress to a percent.
            ''' </summary>
            public const double Progress2Percent = 100.0

            ''' <summary>
            ''' Prevents a default instance of the ProgressConst class from being created.
            ''' </summary>
            private ProgressConst()
            {
            }
        End Class   '   ProgressConst
    End Class   '   MediaPlayer
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\MediaPlayer.cs
