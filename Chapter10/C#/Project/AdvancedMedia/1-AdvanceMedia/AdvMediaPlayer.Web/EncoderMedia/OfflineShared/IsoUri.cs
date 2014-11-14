// <copyright file="IsoUri.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>Isolated storage aware Uri</summary>
// <author>Microsoft Expression Encoder Team</author>
using System;
using System.Net;
using System.Windows;
using System.IO;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Browser;

namespace ExpressionMediaPlayer
{
    /// <summary>
    /// event for download progress of this item.
    /// </summary>
    public class UriDownloadProgressRoutedEventArgs : RoutedEventArgs
    {
        public UriDownloadProgressRoutedEventArgs(double progress)
        {
            Progress = progress;
        }
        public double Progress { get; set; }
    }
    /// <summary>
    /// extended Uri to support isolated offline storage. 
    /// Gives the ability to refer to IsoUri for online and offline cases.
    /// </summary>
    public class IsoUri : Uri, IDisposable
    {
        /// <summary>
        /// mutex used when downloading file
        /// </summary>
        private AutoResetEvent _mutexDownloadCompleted = new AutoResetEvent(false);
        /// <summary>
        /// mutex used when determining file size
        /// </summary>
        private AutoResetEvent _mutexFileSizeCalcCompleted = new AutoResetEvent(false);
        /// <summary>
        /// file size of item pointed to by this URI
        /// </summary>
        private long? _fileSizeFromDownload = null;

        /// <summary>
        /// reverse a string
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        private static String Reverse(String strIn)
        {
            char[] rgchStrInReversed = strIn.ToCharArray();
            Array.Reverse(rgchStrInReversed);
            return (new string(rgchStrInReversed));
        }

        /// <summary>
        /// keep updated. the web download accounts for 1/2 the progress, the local copy to iso store for the other 1/2. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (UriDownloadProgressChanged != null)
            {
                UriDownloadProgressChanged(this, new UriDownloadProgressRoutedEventArgs(e.ProgressPercentage / 200.0));
            }
        }

        private const int maxDownloadBufferSize = 1 << 18;  // 256KB buffer size

        /// <summary>
        /// callback when remote file opened for read. will copy file to isolated storage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs args)
        {
#if DEBUG
            Debug.WriteLine("IsoUri.webClient_OpenReadCompleted:" + this.ToString());
#endif
            // Check for error conditions
            if (args.Cancelled || null != args.Error)
            {
                DownloadErrorMessage = args.Error.Message;
                DownloadSucceeded = false;
                _mutexDownloadCompleted.Set();
                return;
            }

            IsolatedStorageFile isoFile = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream _outputStream = null;
            Stream inputStream = args.Result;

            try
            {
                this._fileSizeFromDownload = inputStream.Length;
                Debug.WriteLine("isoUri webClient_OpenReadCompleted: length=" + this._fileSizeFromDownload.ToString());
                if (this._fileSizeFromDownload < 1)
                {
                    // Download actually failed -- even though WebClient doesn't report this.
                    DownloadErrorMessage = "0000:" + this.OriginalString;
                    DownloadSucceeded = false;
                }
                else
                {
                    _outputStream = isoFile.CreateFile(StreamName);
                    Debug.WriteLine("ISO: file adding " + StreamName + " - " + this);
                    long bufferSize = Math.Min(maxDownloadBufferSize, inputStream.Length);
                    byte[] buf = new byte[bufferSize];
                    long length = inputStream.Length;
                    int read = 0, totalread = 0;
                    while ((read = inputStream.Read(buf, 0, buf.Length)) > 0)
                    {
                        _outputStream.Write(buf, 0, read);
                        totalread += read;

                        if (UriDownloadProgressChanged != null)
                        {
                            UriDownloadProgressChanged(this, new UriDownloadProgressRoutedEventArgs(0.5 + 0.5 * ((double)totalread / (double)length)));
                        }
                    }
                    DownloadErrorMessage = string.Empty;
                    DownloadSucceeded = true;
                }
            }
            catch (IsolatedStorageException ex)
            {
                Debug.WriteLine("ISO: failed to copy file successfully into storage " + StreamName + " - " + this + ": " + ex.Message);
                DownloadErrorMessage = ex.Message;
                DownloadSucceeded = false;
            }
            catch (Exception e)
            {
                Debug.WriteLine("ISOURI: Exception during download:" + StreamName + " - " + this + ": " + e.Message);
                DownloadErrorMessage = e.Message;
                DownloadSucceeded = false;
            }
            finally
            {
                if (_outputStream != null)
                {
                    _outputStream.Close();
                }
                if (inputStream != null)
                {
                    inputStream.Close();
                }
                _mutexDownloadCompleted.Set();
            }
        }

        /// <summary>
        /// create empty IsoUri
        /// </summary>        
        public IsoUri()
            : base("")
        {
        }

        /// <summary>
        /// create IsoUri form string
        /// </summary>
        /// <param name="address">location (relative or absolute)</param>
        public IsoUri(string address)
            : base(address, UriKind.RelativeOrAbsolute)
        {
        }

        /// <summary>
        /// create IsoUri given a seed URI
        /// </summary>
        /// <param name="uri"></param>
        public IsoUri(Uri uri)
            : base(uri.ToString(), UriKind.RelativeOrAbsolute)
        {
        }

        /// <summary>
        /// progress of download
        /// </summary>
        public event EventHandler<UriDownloadProgressRoutedEventArgs> UriDownloadProgressChanged;

        /// <summary>
        /// cleanup
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// clean up
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _mutexDownloadCompleted.Close();
                _mutexFileSizeCalcCompleted.Close();
            }
        }

        /// <summary>
        /// return the Iso stream name for this Uri
        /// </summary>              
        public string StreamName
        {
            get
            {
#if DEBUG_EXTRA_SPECIAL
                string orig = this.OriginalString;
                string name = Path.GetFileName(orig);
                return name;
#else
                string strUrl = this.ToString();
                SHA1Managed hash = new SHA1Managed();
                byte[] rgb = hash.ComputeHash(Encoding.Unicode.GetBytes(strUrl));
                StringBuilder sbEncode = new StringBuilder();
                sbEncode.AppendFormat(CultureInfo.InvariantCulture, "{0:X8}{1:X8}-", strUrl.GetHashCode(), Reverse(strUrl).GetHashCode());
                foreach (byte by in rgb)
                {
                    sbEncode.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", by);
                }
                return sbEncode.ToString();
#endif
            }
        }
        /// <summary>
        /// return an Iso stream from this Uri
        /// </summary>
        public Stream Stream
        {
            get
            {
                IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();
                return iso.OpenFile(StreamName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
        }
        /// <summary>
        /// local copy of file exists in isolated storage for this Uri
        /// </summary>
        public bool IsoFileExists
        {
            get
            {
                try
                {
                    IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();
                    return iso.FileExists(StreamName);
                }
                catch (IsolatedStorageException ise)
                {
                    Debug.WriteLine("IsolatedStorageException testing for file exists! " + ise.ToString());
                    return false;
                }
            }
        }

        public string DownloadErrorMessage { get; set; }
        public bool DownloadSucceeded { get; set; }
        /// <summary>
        /// download the file referred to by this Uri from the cloud to Iso storage
        /// note: call blocks until download completed
        /// </summary>        
        public void Download()
        {
#if DEBUG
            Debug.WriteLine("IsoUri.Download:" + this.ToString());
#endif
            DownloadErrorMessage = null;
            DownloadSucceeded = false;
            if (UriDownloadProgressChanged != null)
            {
                UriDownloadProgressChanged(this, new UriDownloadProgressRoutedEventArgs(0.0));
            }
            _mutexDownloadCompleted.Reset();
            WebClient webClient = new WebClient();
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
            webClient.OpenReadAsync(this);
            _mutexDownloadCompleted.WaitOne();
            if ( DownloadSucceeded )
            {
                Debug.Assert(IsoFileExists);
            }
        }

#if _NO_LONGER_USED
        /// <summary>
        /// determine the file size of the file pointed to by this URI, unfortunately we need
        /// to fetch it. Note: will block, needs to be called off-UI-thread
        /// </summary>
        internal long? FileSizeOnceDownloaded
        {
            get
            {
                return _fileSizeFromDownload;
            }
        }
 

        /// <summary>
        /// remove this file from Iso storage
        /// </summary>
        internal void DeleteFromIsoStorage()
        {
            IsolatedStorageFile iso = IsolatedStorageFile.GetUserStoreForApplication();
            iso.DeleteFile(StreamName);
        }
#endif

        /// <summary>
        /// clear all isostorage files associated with this application
        /// </summary>
        public static void ClearIsoStorage()
        {
            IsolatedStorageFile.GetUserStoreForApplication().Remove();
        }
        /// <summary>
        /// gets a fully qualified version of this URI if possible
        /// </summary>
        public Uri FullyQualifiedUri
        {
            get
            {
                try
                {
                    if (!this.IsAbsoluteUri && HtmlPage.IsEnabled && HtmlPage.Document != null && HtmlPage.Document.DocumentUri!=null && !String.IsNullOrEmpty(this.ToString()))
                    {
                        Uri uri = HtmlPage.Document.DocumentUri;
                        UriBuilder urib = new UriBuilder(uri);
                        urib.Path = System.Uri.UnescapeDataString(uri.AbsolutePath.Substring(0, uri.AbsolutePath.LastIndexOf("/", StringComparison.Ordinal) + 1) + this.OriginalString);
                        return urib.Uri;
                    }
                }
                catch (InvalidOperationException)
                {
                    // can occur offline. no HTML bridge
                }

                return this;
            }
        }
    }
}
