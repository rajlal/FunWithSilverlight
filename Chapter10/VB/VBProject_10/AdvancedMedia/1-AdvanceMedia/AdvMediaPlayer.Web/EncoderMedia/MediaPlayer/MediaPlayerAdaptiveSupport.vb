'  <copyright file="MediaPlayerAdaptiveSupport.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the MediaPlayer class</summary>
'  <author>Microsoft Expression Encoder Team</author>
' Namespace ExpressionMediaPlayer

Imports    using Microsoft.Expression.Encoder.PlugInMssCtrl
    using System
    using System.Collections.Generic
    using System.Diagnostics
    using System.Globalization
    using System.IO
    using System.IO.IsolatedStorage
    using System.Net
    using System.Text
    using System.Threading
    using System.Windows
    using System.Windows.Browser
    using System.Windows.Controls
    using System.Windows.Media

    Public Partial Class MediaPlayer
        Inherits Control
        ''' <summary>
        ''' The plugin in loader helper object that loads the Smoooth streamign support module and processes the assembly information
        ''' </summary>
        private PlugInLoader m_smoothStreamingSourcePlugIn
        ''' <summary>
        ''' The URI of the manifest for the currently playing item
        ''' </summary>
        Private m_smoothStreamingItemManifestUri As Uri
        ''' <summary>
        ''' The offline bitrate in kbps of the currently playing item
        ''' </summary>
        private long m_smoothStreamingItemOfflineVideoBitrateInKbps

        ''' <summary>
        ''' Filename of the smooth streaming support module
        ''' </summary>
        internal static string SmoothStreamingXAPName
        {
            get
            {
                return "SmoothStreaming.xap"
            }
        }

        ''' <summary>
        ''' Assembly name of the smooth streaming support module
        ''' </summary>
        internal static string SmoothStreamingAssemblyName
        {
            get
            {
                return "SmoothStreaming.dll"
            }
        }

        ''' <summary>
        ''' class name of the smooth streaming support object for online playback
        ''' </summary>
        internal static string SmoothStreamingOnlineObjectName
        {
            get
            {
                return "SmoothStreaming.SmoothStreamingMediaElementShim"
            }
        }

        ''' <summary>
        ''' class name of the smooth streaming support object for online playback
        ''' </summary>
        internal static string SmoothStreamingOfflineObjectName
        {
            get
            {
                return "SmoothStreaming.SmoothStreamingMediaElementShimOffline"
            }
        }

        ''' <summary>
        ''' Initialize smooth streaming for taking content offline
        ''' </summary>
        private Sub LoadSmoothStreamingModule()
        End Sub '   LoadSmoothStreamingModule


        ''' <summary>
        ''' Initialize smooth streaming for taking content offline
        ''' </summary>
        internal Sub InitSmoothStreamingToGoOffline()

            Me.LoadSmoothStreamingModuleOnlineToGoOffline()
        End Sub '   InitSmoothStreamingToGoOffline


        ''' <summary>
        ''' Initialize smooth streaming playback for a manifest item
        ''' </summary>
        internal Sub InitSmoothStreaming(playlistItem As PlaylistItem)

            Me.m_smoothStreamingItemManifestUri = playlistItem.MediaSource
            Me.m_smoothStreamingItemOfflineVideoBitrateInKbps = playlistItem.OfflineVideoBitrateInKbps


            If (Me.m_mediaElementForSmoothStreamingContent Is Nothing) Then
                Me.LoadSmoothStreamingModuleForPlayback()
            End If


            If (m_smoothStreamingSourcePlugIn  IsNot Nothing) Then
                CreateSmoothStreamingObject()
                StartSmoothStreamingPlayback()
            End If
        End Sub '   InitSmoothStreaming

        ''' <summary>
        ''' Loads the SmoothStreaming support module
        ''' </summary>
        private Sub LoadSmoothStreamingModuleOnlineCore(onPlugInLoadCompleted As EventHandler(Of XAPReadCompletedEventArgs))

            Debug.WriteLine("InitSmoothStreamingCore Not ")
            Me.m_smoothStreamingSourcePlugIn = New PlugInLoader()
            AddHandler Me.m_smoothStreamingSourcePlugIn.PlugInLoadCompleted, AddressOf New EventHandler<XAPReadCompletedEventArgs>(onPlugInLoadCompleted)

            Dim xap As Uri  = New Uri(SmoothStreamingXAPName, UriKind.Relative)

            Me.m_smoothStreamingSourcePlugIn.Load(xap, SmoothStreamingAssemblyName)
        End Sub '   LoadSmoothStreamingModuleOnlineCore


        ''' <summary>
        ''' Loads the SmoothStreaming support module from the host when running online for the purpose of taking content offline
        ''' </summary>
        internal Sub LoadSmoothStreamingModuleOnlineToGoOffline()

            Debug.WriteLine("LoadSmoothStreamingModuleOnlineToGoOffline Not ")
            LoadSmoothStreamingModuleOnlineCore(new EventHandler<XAPReadCompletedEventArgs>(OnSmoothStreamingLoadedToGoOffine))
        End Sub '   LoadSmoothStreamingModuleOnlineToGoOffline


        ''' <summary>
        ''' Callback that is called once the SmoothStreaming support module is downloaded form the host when running online
        ''' </summary>
        private Sub OnSmoothStreamingLoadedToGoOffine(sender As Object, e As XAPReadCompletedEventArgs)


            If (OnSmoothStreamingLoadedErrorCheck(e)) Then
                CreateSmoothStreamingObject()

                If (Me.m_mediaElementForSmoothStreamingContent  IsNot Nothing) Then

                    Dim offlineSupport As IPlugInMssOfflineSupport  = Me.SmoothStreamingOfflineSupport

                    Debug.Assert(offlineSupport  IsNot Nothing, "OnSmoothStreamingLoadedToGoOffine object not ready Not ")
                End If

                ShowSmoothStreamingMediaElement()
            End If
        End Sub '   OnSmoothStreamingLoadedToGoOffine

        ''' <summary>
        ''' Loads the SmoothStreaming support module for playback
        ''' </summary>
        private Sub LoadSmoothStreamingModuleForPlayback()

            Debug.WriteLine("start LoadSmoothStreamingModuleForPlayback")
            LoadSmoothStreamingModuleOnlineCore(new EventHandler<XAPReadCompletedEventArgs>(OnSmoothStreamingLoadedForPlayback))
        End Sub '   LoadSmoothStreamingModuleForPlayback


        Private ReadOnly Property bool() As

        ''' <summary>
        ''' Callback that is called once the SmoothStreaming support module is downloaded form the host when running online
        ''' </summary>
        private Sub OnSmoothStreamingLoadedForPlayback(sender As Object, e As XAPReadCompletedEventArgs)


            If (OnSmoothStreamingLoadedErrorCheck(e)) Then

                Dim startedOK As Boolean  = false

                Try
                    CreateSmoothStreamingObject()
                    StartSmoothStreamingPlayback()
                    startedOK = true

                Catch ise As IsolatedStorageException

                    Me.ShowErrorMessage(ise.ToString())

                finally


                    If ( Not startedOK) Then
                        Me.ButtonClickStopLogic()
                    End If
                End Try
            End If
        End Sub '   OnSmoothStreamingLoadedForPlayback

        ''' <summary>
        ''' Create a smooth streaming object (either online or offline)
        ''' </summary>
        private Sub CreateSmoothStreamingObject()

            Try

                If (Me.m_mediaElementForSmoothStreamingContent Is Nothing) Then

                    Dim smoothStreamingObject As Object  = Nothing


                    If (MediaPlayer.IsOffline) Then
                        smoothStreamingObject = Me.m_smoothStreamingSourcePlugIn.CreateObject(MediaPlayer.SmoothStreamingOfflineObjectName)
                        Debug.Assert(smoothStreamingObject  IsNot Nothing, "failed to create: " + MediaPlayer.SmoothStreamingOfflineObjectName)
                    else
                        smoothStreamingObject = Me.m_smoothStreamingSourcePlugIn.CreateObject(MediaPlayer.SmoothStreamingOnlineObjectName)
                        Debug.Assert(smoothStreamingObject  IsNot Nothing, "failed to create: " + MediaPlayer.SmoothStreamingOnlineObjectName)
                    End If


                    Me.m_mediaElementForSmoothStreamingContent = smoothStreamingObject as MediaElementShim
                End If


            Catch fe As PlugInLoaderFailedException

                Debug.WriteLine("SmoothStreamingSupportLoadCompleted: Create Failed:" + fe.Message)
            End Try
        End Sub '   CreateSmoothStreamingObject

        ''' <summary>
        ''' make the SSME visible and hooked up to events
        ''' </summary>
        private Sub ShowSmoothStreamingMediaElement()


            If (Me.m_mediaElementForConventionalContent  IsNot Nothing) Then

                Select Case (Me.m_mediaElementForConventionalContent.CurrentState)

                    case MediaElementState.Stopped:
                    case MediaElementState.Closed:

                    default:
                        Me.m_mediaElementForConventionalContent.Stop()
                End Select '    Me.m_mediaElementForConventionalContent.CurrentState

                Me.m_mediaElementForConventionalContent.Source = Nothing
                Me.m_mediaElementForConventionalContent.Visibility = Visibility.Collapsed
            End If


            If (Me.m_mediaElementForSmoothStreamingContent  IsNot Nothing) Then
                Me.m_mediaElementForSmoothStreamingContent.Visibility = Visibility.Visible

                If (Me.m_mediaElement  <>  m_mediaElementForSmoothStreamingContent) Then

                    If (Me.m_mediaElement  IsNot Nothing) Then
                        Me.UnhookMediaElementEvents()
                    End If


                    If (Me.m_mediaElementGrid  IsNot Nothing) Then
                        Me.m_mediaElementGrid.Children.Clear()

                        Dim ue As UIElement  = Me.m_mediaElementForSmoothStreamingContent.UIElement

                        Me.m_mediaElementGrid.Children.Add(ue)
                    End If

                    Me.m_mediaElement = Me.m_mediaElementForSmoothStreamingContent
                    Me.HookMediaElementEvents()
                    Me.ApplyPropertiesToMediaElement()
                End If
            End If
        End Sub '   ShowSmoothStreamingMediaElement


        ''' <summary>
        ''' Actually start playback once the support module is loaded and the smooth streaming object is created.
        ''' </summary>
        private Sub StartSmoothStreamingPlayback()

#if DEBUG
            Debug.WriteLine("StartSmoothStreamingPlayback")
#endif

            If (Me.m_mediaElementForSmoothStreamingContent  IsNot Nothing) Then

                If (Me.m_smoothStreamingItemManifestUri  IsNot Nothing) Then
                    Me.ShowSmoothStreamingMediaElement()


                    If (MediaPlayer.IsOffline) Then
                        IsoUri offlineIsoUri = MediaPlayer.MakeOfflineIsoUri(Me.m_smoothStreamingItemManifestUri)
                        if (offlineIsoUri.IsoFileExists)
                        {
                            '  Force heuristics to play the bitrate actually present in offline storage
                            if ((Me.m_smoothStreamingItemOfflineVideoBitrateInKbps > 0)  AndAlso  (Me.SmoothStreamingOfflineSupport  IsNot Nothing))
                            {
                                '  Force heuristics to play the bitrate actually present in offline storage
                                Me.SmoothStreamingOfflineSupport.SetOfflinePlaybackBitrateInKbps(MediaStreamType.Video, Me.m_smoothStreamingItemOfflineVideoBitrateInKbps)
                            }
                            Me.SmoothStreamingMediaElementShim.Source = offlineIsoUri
                        }
                        else
                        {

                            Dim err As String  = "Manifest missing from isolated storage Not " + Me.m_smoothStreamingItemManifestUri.OriginalString

                            Me.ShowErrorMessage(err)
                            throw New IsolatedStorageException(err)
                        }
                    else
                        '  Construct the absolute uri
                        Dim absoluteUri As Uri


                        If (Me.m_smoothStreamingItemManifestUri.IsAbsoluteUri  OrElse  HtmlPage.Document.DocumentUri Is Nothing) Then
                            absoluteUri = Me.m_smoothStreamingItemManifestUri
                        else
                            absoluteUri = New Uri(HtmlPage.Document.DocumentUri, Me.m_smoothStreamingItemManifestUri)
                        End If

                        Me.m_mediaElementForSmoothStreamingContent.Source = absoluteUri
                    End If


                    '  Display stats graph if this template has a slot for it.

                    If (Me.m_gridPlugIn  IsNot Nothing) Then
                        IPlugInMssStatisticsGraph plugInGraph = Me.m_mediaElementForSmoothStreamingContent as IPlugInMssStatisticsGraph

                        If (plugInGraph  IsNot Nothing) Then
                            '  Remove all prior plug ins
                            Me.m_gridPlugIn.Children.Clear()
                            Me.m_gridPlugIn.Children.Add(plugInGraph.StatisticsGraph)
                        End If
                    End If
                End If
            End If
        End Sub '   StartSmoothStreamingPlayback
    End Class   '   MediaPlayer
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\MediaPlayerAdaptiveSupport.cs
