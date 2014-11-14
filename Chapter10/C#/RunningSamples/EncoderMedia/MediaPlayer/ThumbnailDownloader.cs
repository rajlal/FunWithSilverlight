// <copyright file="ThumbnailDownloader.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>Implements the ThumbnailDownloader class</summary>
// <author>Microsoft Expression Encoder Team</author>
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace ExpressionMediaPlayer
{
    public sealed class ThumbnailDownloader
    {
        /// <summary>
        /// Dispatch timer for downloading thumbnails.
        /// </summary>
        private static DispatcherTimer sm_timerThumbnails;
        /// <summary>
        /// queue of thumbnails to download
        /// </summary>
        private static Queue<ThumbnailImage> sm_downloadQueue = new Queue<ThumbnailImage>();
        /// <summary>
        /// create ThumbnailDownloader
        /// </summary>
        private ThumbnailDownloader()
        {
        }
        /// <summary>
        /// Event handler for a Timer tick.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Event args.</param>
        private static void OnTimerTickThumbnailDownload(object sender, EventArgs e)
        {
            lock (sm_downloadQueue)
            {
                if (sm_downloadQueue.Count > 0)
                {
                    ThumbnailImage thumbnail = sm_downloadQueue.Dequeue();
                    thumbnail.LoadImage();
                }
            }
        }        
        /// <summary>
        /// Creates the timer for downloading thumbnails.
        /// </summary>
        public static void EnableThumbnailDownload()
        {
            if (sm_timerThumbnails == null)
            {
                sm_timerThumbnails = new DispatcherTimer();
                sm_timerThumbnails.Interval = new TimeSpan(0/*days*/, 0/*hours*/, 0/*minutes*/, 0/*seconds*/, 60/*milliseconds*/);
                sm_timerThumbnails.Tick += new EventHandler(OnTimerTickThumbnailDownload);
            }

            sm_timerThumbnails.Start();
        }
        /// <summary>
        /// Turns off the timer for downloading thumbnails.
        /// </summary>
        public static void DisableThumbnailDownload()
        {
            if (sm_timerThumbnails != null)
            {
                sm_timerThumbnails.Stop();
            }
        }
        /// <summary>
        /// Place a thumbnail download request in the queue.
        /// </summary>
        public static void Enqueue(ThumbnailImage thumbnail)
        {
            lock (sm_downloadQueue)
            {
                sm_downloadQueue.Enqueue(thumbnail);
            }
        }
    }
}
