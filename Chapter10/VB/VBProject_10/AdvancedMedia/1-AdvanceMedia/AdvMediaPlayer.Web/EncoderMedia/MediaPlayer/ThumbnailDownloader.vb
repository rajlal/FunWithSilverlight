'  <copyright file="ThumbnailDownloader.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the ThumbnailDownloader class</summary>
'  <author>Microsoft Expression Encoder Team</author>
Imports System
Imports System.Collections.Generic
Imports System.Windows.Threading

' Namespace ExpressionMediaPlayer

    Public sealed Class ThumbnailDownloader
        ''' <summary>
        ''' Dispatch timer for downloading thumbnails.
        ''' </summary>
        Private ReadOnly Property DispatcherTimer() As
        ''' <summary>
        ''' Event handler for a Timer tick.
        ''' </summary>
        ''' <param name="sender">Source of the event.</param>
        ''' <param name="e">Event args.</param>
        End Property


            lock (sm_downloadQueue)
            {

                If (sm_downloadQueue.Count > 0) Then
                    ThumbnailImage thumbnail = sm_downloadQueue.Dequeue()
                    thumbnail.LoadImage()
                End If

            }
        End Sub '   OnTimerTickThumbnailDownload
        ''' <summary>
        ''' Creates the timer for downloading thumbnails.
        ''' </summary>
        End Property



            If (sm_timerThumbnails Is Nothing) Then
                sm_timerThumbnails = New DispatcherTimer()
                sm_timerThumbnails.Interval = New TimeSpan(0/*days*/, 0/*hours*/, 0/*minutes*/, 0/*seconds*/, 60/*milliseconds*/)
                AddHandler sm_timerThumbnails.Tick, AddressOf EventHandler(OnTimerTickThumbnailDownload)
            End If


            sm_timerThumbnails.Start()
        End Sub '   EnableThumbnailDownload
        ''' <summary>
        ''' Turns off the timer for downloading thumbnails.
        ''' </summary>
        End Property



            If (sm_timerThumbnails  IsNot Nothing) Then
                sm_timerThumbnails.Stop()
            End If
        End Sub '   DisableThumbnailDownload
        ''' <summary>
        ''' Place a thumbnail download request in the queue.
        ''' </summary>
        End Property


            lock (sm_downloadQueue)
            {
                sm_downloadQueue.Enqueue(thumbnail)
            }
        End Sub '   Enqueue
    End Class   '   ThumbnailDownloader
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\ThumbnailDownloader.cs
