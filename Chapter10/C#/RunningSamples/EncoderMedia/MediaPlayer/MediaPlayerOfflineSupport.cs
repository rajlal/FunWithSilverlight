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
        /// The base URI of the online player -- which is needed so the offline player construct the correct IsoUri StreamNames for loading the chunks 
        /// </summary>
        private static Uri sm_offlineBaseUri;

        /// <summary>
        /// is the player in an offline state?
        /// </summary>
        /// 
        internal static bool IsOffline
        {
            get
            {
                if (Application.Current.InstallState == InstallState.Installed)
                {
                    if (OfflineBaseUri != null)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// Obtains the base URI of the online player -- stored in the special file in isloated storage 
        /// </summary>
        public static Uri OfflineBaseUri
        {
            get
            {
                if (sm_offlineBaseUri == null)
                {
                    try
                    {
                        var isoStore = IsolatedStorageFile.GetUserStoreForApplication();
#if DEBUG
                        string[] dirNames = isoStore.GetDirectoryNames();
                        foreach (string dirName in dirNames)
                        {
                            Debug.WriteLine("IsolatedStorage dirName=" + dirName);
                        }
                        string[] fileNames = isoStore.GetFileNames();
                        foreach (string fileName in fileNames)
                        {
                            Debug.WriteLine("IsolatedStorage fileName=" + fileName);
                        }
#endif
                        var streamReader = new StreamReader(isoStore.OpenFile(Playlist.downloadCompletedFilename, System.IO.FileMode.Open, System.IO.FileAccess.Read), Encoding.Unicode);
                        string offlineBaseUrl = streamReader.ReadLine();
                        string videoBitrateTakenOfflineText = streamReader.ReadLine();
                        streamReader.Close();
                        sm_offlineBaseUri = new Uri(offlineBaseUrl, UriKind.Absolute);
#if DEBUG
                        Debug.WriteLine("OfflineBaseUri=" + offlineBaseUrl);
#endif
                    }
                    catch (IsolatedStorageException iso)
                    {
                        Debug.WriteLine("IsolatedStorageException attempting read the download complete file!:" + iso.Message);
                        StaticShowErrorMessage(iso.Message);
                    }
                }
                return sm_offlineBaseUri;
            }
        }
        /// <summary>
        /// For offline playback form a useful IsoUri
        /// </summary>
        internal static IsoUri MakeOfflineIsoUri(Uri mediaUri)
        {
            if (mediaUri != null)
            {
                IsoUri isoUri;
                if (mediaUri.IsAbsoluteUri)
                {
                    isoUri = new IsoUri(mediaUri);
                }
                else
                {
                    isoUri = new IsoUri(new Uri(MediaPlayer.OfflineBaseUri, mediaUri));
                }
                return isoUri;
            }
            return null;
        }
    }
}