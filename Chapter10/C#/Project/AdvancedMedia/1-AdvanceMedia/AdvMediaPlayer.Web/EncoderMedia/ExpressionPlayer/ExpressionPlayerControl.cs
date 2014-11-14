// <copyright file="ExpressionPlayerControl.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <author>Microsoft Expression Encoder Team</author>
using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;

namespace ExpressionMediaPlayer
{
    /// <summary>
    /// Expression Media Player that supports adpative streaming
    /// </summary>
    public class ExpressionPlayer : MediaPlayer
    {
        /// <summary>
        /// template is showing preview content in preview of Encoder.
        /// </summary>
        private bool m_InPreviewMode;
        /// <summary>
        /// Initializes a new instance of the ExpressionPlayer class.
        /// </summary>
        public ExpressionPlayer() : base()
        {
        }
        /// <summary>
        /// Performs additional initialization during the start up event.
        /// </summary>
        /// <param name="sender">The object triggering the startup event</param>
        /// <param name="e">The event arguments</param>
        public override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);

			if (Playlist.Items.Count == 0 && Application.Current.InstallState == InstallState.NotInstalled && HtmlPage.IsEnabled)
            {
                string strFakeOutput;
                Uri uriStockContent = HtmlPage.Document.DocumentUri;
                HtmlPage.Document.QueryString.TryGetValue("fakeoutput", out strFakeOutput);

                if (strFakeOutput != null)
                {
                    try
                    {
                        UriBuilder urib = new UriBuilder(Uri.UnescapeDataString(strFakeOutput));
                        uriStockContent = urib.Uri;
                    }
                    catch (UriFormatException)
                    {
                    }

                    Playlist.StretchNonSquarePixels = StretchNonSquarePixels.StretchToFill;
                    
                    Playlist.Items.Clear();

                    for (int items = 0; items < 7; items++)
                    {
                        PlaylistItem playlistItem = new PlaylistItem(Playlist.Items);
                        playlistItem.MediaSource = new Uri(uriStockContent, "sl.wmv");
                        playlistItem.IsAdaptiveStreaming = false;
                        playlistItem.ThumbSource = new Uri(uriStockContent, "sl1.jpg");
                        playlistItem.Title = "Lorum Ipsum Dolar Sit";
                        playlistItem.Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque auctor libero nec justo tempor bibendum.";
                        playlistItem.VideoHeight = 512;
                        playlistItem.VideoHeight = 288;
                        playlistItem.AspectRatioWidth = 16;
                        playlistItem.AspectRatioHeight = 9;
                        Playlist.Items.Add(playlistItem);
                        for (int i = 1; i < 4; i++)
                        {
                            ChapterItem chapter = new ChapterItem();
                            chapter.Position = (double)i;
                            chapter.ThumbSource = new Uri(uriStockContent, "sl" + i + ".jpg");
                            chapter.Title = string.Empty;
                            playlistItem.Chapters.Add(chapter);
                        }
                    }

                    m_InPreviewMode = true;                    
                }                    
            }
        }
        /// <summary>
        /// overrides GetTemplateChildren to disable some functionality when in preview mode
        /// </summary>
        protected override void GetTemplateChildren()
        {
            base.GetTemplateChildren();

            if (m_InPreviewMode)
            {
                var offlineButton = GetTemplateChild(MediaPlayer.OfflineButton) as Button;
                if (offlineButton != null)
                {
                    offlineButton.IsEnabled = false;
                }

                var popoutButton = GetTemplateChild(MediaPlayer.PopOutButton) as Button;
                if (popoutButton != null)
                {
                    popoutButton.IsEnabled = false;
                }
            }
        }
    }
}
