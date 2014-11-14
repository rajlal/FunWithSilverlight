'  <copyright file="ExpressionPlayerControl.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <author>Microsoft Expression Encoder Team</author>
Imports System
Imports System.Windows
Imports System.Windows.Browser
Imports System.Windows.Controls

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' Expression Media Player that supports adpative streaming
    ''' </summary>
    Public Class ExpressionPlayer
        Inherits MediaPlayer
        ''' <summary>
        ''' template is showing preview content in preview of Encoder.
        ''' </summary>
        Private m_InPreviewMode As Boolean
        ''' <summary>
        ''' Initializes a new instance of the ExpressionPlayer class.
        ''' </summary>
        Public Sub New()
            MyBase.New()
        End Sub '   New
        ''' <summary>
        ''' Performs additional initialization during the start up event.
        ''' </summary>
        ''' <param name="sender">The object triggering the startup event</param>
        ''' <param name="e">The event arguments</param>
        public override Sub OnStartup(sender As Object, e As StartupEventArgs)

            MyBase.OnStartup(sender, e)

              0  AndAlso  Application.Current.InstallState = InstallState.NotInstalled  AndAlso  HtmlPage.IsEnabled) Then
              If (Playlist.Items.Count = InstallState.NotInstalled  AndAlso  HtmlPage.IsEnabled) Then

                Dim strFakeOutput As String
                Dim uriStockContent As Uri  = HtmlPage.Document.DocumentUri

                HtmlPage.Document.QueryString.TryGetValue("fakeoutput", out strFakeOutput)


                If (strFakeOutput  IsNot Nothing) Then
                    Try

                        Dim urib As UriBuilder  = New UriBuilder(Uri.UnescapeDataString(strFakeOutput))

                        uriStockContent = urib.Uri

                    Catch ex As UriFormatException
                    End Try


                    Playlist.StretchNonSquarePixels = StretchNonSquarePixels.StretchToFill

                    Playlist.Items.Clear()


                    For items As Integer = 0 To  6

                        PlaylistItem playlistItem = New PlaylistItem(Playlist.Items)
                        playlistItem.MediaSource = New Uri(uriStockContent, "sl.wmv")
                        playlistItem.IsAdaptiveStreaming = false
                        playlistItem.ThumbSource = New Uri(uriStockContent, "sl1.jpg")
                        playlistItem.Title = "Lorum Ipsum Dolar Sit"
                        playlistItem.Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque auctor libero nec justo tempor bibendum."
                        playlistItem.VideoHeight = 512
                        playlistItem.VideoHeight = 288
                        playlistItem.AspectRatioWidth = 16
                        playlistItem.AspectRatioHeight = 9
                        Playlist.Items.Add(playlistItem)

                        For i As Integer = 1 To  3

                            ChapterItem chapter = New ChapterItem()
                            chapter.Position = CType(i, Double)
                            chapter.ThumbSource = New Uri(uriStockContent, "sl" + i + ".jpg")
                            chapter.Title = string.Empty
                            playlistItem.Chapters.Add(chapter)
                        Next    '   i
                    Next    '   items

                    m_InPreviewMode = true
                End If
            End If
        End Sub '   OnStartup
        ''' <summary>
        ''' overrides GetTemplateChildren to disable some functionality when in preview mode
        ''' </summary>
        protected override Sub GetTemplateChildren()

            MyBase.GetTemplateChildren()


            If (m_InPreviewMode) Then
                var offlineButton = GetTemplateChild(MediaPlayer.OfflineButton) as Button

                If (offlineButton  IsNot Nothing) Then
                    offlineButton.IsEnabled = false
                End If


                var popoutButton = GetTemplateChild(MediaPlayer.PopOutButton) as Button

                If (popoutButton  IsNot Nothing) Then
                    popoutButton.IsEnabled = false
                End If
            End If
        End Sub '   GetTemplateChildren
    End Class   '   ExpressionPlayer
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\ExpressionPlayer\ExpressionPlayerControl.cs
