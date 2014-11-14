'  <copyright file="IsoUri.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Isolated storage aware Uri</summary>
'  <author>Microsoft Expression Encoder Team</author>
Imports System
Imports System.Net
Imports System.Windows
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Diagnostics
Imports System.Threading
Imports System.Globalization
Imports System.Text
Imports System.Security.Cryptography
Imports System.Windows.Browser

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' event for download progress of this item.
    ''' </summary>
    Public Class UriDownloadProgressRoutedEventArgs
        Inherits RoutedEventArgs

        Public Sub New(progress As Double)
            Progress = progress
        End Sub '   New

        Public Property Progress As Double
    End Class   '   UriDownloadProgressRoutedEventArgs
    ''' <summary>
    ''' extended Uri to support isolated offline storage.
    ''' Gives the ability to refer to IsoUri for online and offline cases.
    ''' </summary>
    Public Class IsoUri
        Inherits Uri
        Implements IDisposable
        ''' <summary>
        ''' mutex used when downloading file
        ''' </summary>
        Private _mutexDownloadCompleted As AutoResetEvent = New AutoResetEvent(false)
        ''' <summary>
        ''' mutex used when determining file size
        ''' </summary>
        Private _mutexFileSizeCalcCompleted As AutoResetEvent = New AutoResetEvent(false)
        ''' <summary>
        ''' file size of item pointed to by this URI
        ''' </summary>
        private _fileSizeFromDownload as long? = Nothing

        ''' <summary>
        ''' reverse a string
        ''' </summary>
        ''' <param name="strIn"></param>
        ''' <returns></returns>
        Private ReadOnly Property String() As

        ''' <summary>
        ''' keep updated. the web download accounts for 1/2 the progress, the local copy to iso store for the other 1/2.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        private Sub webClient_DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs)


            If (UriDownloadProgressChanged  IsNot Nothing) Then
                UriDownloadProgressChanged(this, New UriDownloadProgressRoutedEventArgs(e.ProgressPercentage / 200.0))
            End If
        End Sub '   webClient_DownloadProgressChanged

        '  256KB buffer size
        private const int maxDownloadBufferSize = 1 << 18

        ''' <summary>
        ''' callback when remote file opened for read. will copy file to isolated storage
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="args"></param>
        private Sub webClient_OpenReadCompleted(sender As Object, args As OpenReadCompletedEventArgs)

#if DEBUG
            Debug.WriteLine("IsoUri.webClient_OpenReadCompleted:" + Me.ToString())
#endif
            '  Check for error conditions

            If (args.Cancelled  OrElse  args.Error IsNot Nothing  ) Then
                DownloadErrorMessage = args.Error.Message
                DownloadSucceeded = false
                _mutexDownloadCompleted.Set()
                return
            End If


            Dim isoFile As IsolatedStorageFile  = IsolatedStorageFile.GetUserStoreForApplication()
            Dim _outputStream As IsolatedStorageFileStream  = Nothing
            Dim inputStream As Stream  = args.Result

            Try
                Me._fileSizeFromDownload = inputStream.Length
                Debug.WriteLine("isoUri webClient_OpenReadCompleted: length=" + Me._fileSizeFromDownload.ToString())

                If (Me._fileSizeFromDownload < 1) Then
                    '  Download actually failed -- even though WebClient doesn't report this.
                    DownloadErrorMessage = "0000:" + Me.OriginalString
                    DownloadSucceeded = false
                else
                    _outputStream = isoFile.CreateFile(StreamName)
                    Debug.WriteLine("ISO: file adding " + StreamName + " - " + this)
                    long bufferSize = Math.Min(maxDownloadBufferSize, inputStream.Length)
                    byte() buf = New byte(bufferSize)
                    long length = inputStream.Length
                    0, totalread = 0
                    Dim read As Integer  = 0

                    While ((read = inputStream.Read(buf, 0, buf.Length)) > 0)

                        _outputStream.Write(buf, 0, read)
                        totalread += read

                        If (UriDownloadProgressChanged  IsNot Nothing) Then
                            UriDownloadProgressChanged(this, New UriDownloadProgressRoutedEventArgs(0.5 + 0.5 * (CType(totalread, Double) / (double)length)))
                        End If
                    End While   '
                    DownloadErrorMessage = string.Empty
                    DownloadSucceeded = true
                End If
            Catch ex As IsolatedStorageException

                Debug.WriteLine("ISO: failed to copy file successfully into storage " + StreamName + " - " + this + ": " + ex.Message)
                DownloadErrorMessage = ex.Message
                DownloadSucceeded = false
            Catch e As Exception
                Debug.WriteLine("ISOURI: Exception during download:" + StreamName + " - " + this + ": " + e.Message)
                DownloadErrorMessage = e.Message
                DownloadSucceeded = false
            finally
                If (_outputStream  IsNot Nothing) Then
                    _outputStream.Close()
                End If

                If (inputStream  IsNot Nothing) Then
                    inputStream.Close()
                End If

                _mutexDownloadCompleted.Set()
            End Try
        End Sub '   webClient_OpenReadCompleted

        ''' <summary>
        ''' create empty IsoUri
        ''' </summary>
        Public Sub New()

        End Sub '   New


        ''' <summary>
        ''' create IsoUri form string
        ''' </summary>
        ''' <param name="address">location (relative or absolute)</param>
        Public Sub New(address As String)

        End Sub '   New


        ''' <summary>
        ''' create IsoUri given a seed URI
        ''' </summary>
        ''' <param name="oUri"></param>
        Public Sub New(oUri As Uri)

        End Sub '   New


        ''' <summary>
        ''' progress of download
        ''' </summary>
        public event EventHandler<UriDownloadProgressRoutedEventArgs> UriDownloadProgressChanged

        ''' <summary>
        ''' cleanup
        ''' </summary>
        public Sub Dispose()

            Dispose(true)
            GC.SuppressFinalize(this)
        End Sub '   Dispose
        ''' <summary>
        ''' clean up
        ''' </summary>
        protected virtual Sub Dispose(disposing As Boolean)

            If (disposing) Then
                _mutexDownloadCompleted.Close()
                _mutexFileSizeCalcCompleted.Close()
            End If
        End Sub '   Dispose
        ''' <summary>
        ''' return the Iso stream name for this Uri
        ''' </summary>
        Public ReadOnly Property StreamName() As String
            Get

#if DEBUG_EXTRA_SPECIAL

                Dim orig As String  = Me.OriginalString
                Dim name As String  = Path.GetFileName(orig)

                return name
#else
                Dim strUrl As String  = Me.ToString()
                Dim hash As SHA1Managed  = New SHA1Managed()

                Dim rgb as byte()  = hash.ComputeHash(Encoding.Unicode.GetBytes(strUrl))

                Dim sbEncode As StringBuilder  = New StringBuilder()

                sbEncode.AppendFormat(CultureInfo.InvariantCulture, "{0:X8}{1:X8}-", strUrl.GetHashCode(), Reverse(strUrl).GetHashCode())

                For Each by As Byte in rgb
                    sbEncode.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", by)
                Next    '   by

                return sbEncode.ToString()
#endif
            End Get
        End Property
        ''' <summary>
        ''' return an Iso stream from this Uri
        ''' </summary>
        Public ReadOnly Property Stream() As Stream
            Get

                Dim iso As IsolatedStorageFile  = IsolatedStorageFile.GetUserStoreForApplication()

                return iso.OpenFile(StreamName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            End Get
        End Property
        ''' <summary>
        ''' local copy of file exists in isolated storage for this Uri
        ''' </summary>
        Public ReadOnly Property IsoFileExists() As Boolean
            Get
                Try

                    Dim iso As IsolatedStorageFile  = IsolatedStorageFile.GetUserStoreForApplication()

                    return iso.FileExists(StreamName)
                Catch ise As IsolatedStorageException
                    Debug.WriteLine("IsolatedStorageException testing for file exists Not  " + ise.ToString())
                    return false
                End Try
            End Get
        End Property

        Public Property DownloadErrorMessage As String
        Public Property DownloadSucceeded As Boolean
        ''' <summary>
        ''' download the file referred to by this Uri from the cloud to Iso storage
        ''' note: call blocks until download completed
        ''' </summary>
        public Sub Download()

#if DEBUG
            Debug.WriteLine("IsoUri.Download:" + Me.ToString())
#endif
            DownloadErrorMessage = Nothing
            DownloadSucceeded = false

            If (UriDownloadProgressChanged  IsNot Nothing) Then
                UriDownloadProgressChanged(this, New UriDownloadProgressRoutedEventArgs(0.0))
            End If

            _mutexDownloadCompleted.Reset()

            Dim oWebClient As WebClient  = New WebClient()

            AddHandler oWebClient.DownloadProgressChanged, AddressOf DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged)
            AddHandler oWebClient.OpenReadCompleted, AddressOf OpenReadCompletedEventHandler(webClient_OpenReadCompleted)
            oWebClient.OpenReadAsync(this)
            _mutexDownloadCompleted.WaitOne()

            If ( DownloadSucceeded ) Then
                Debug.Assert(IsoFileExists)
            End If
        End Sub '   Download

#if _NO_LONGER_USED
        ''' <summary>
        ''' determine the file size of the file pointed to by this URI, unfortunately we need
        ''' to fetch it. Note: will block, needs to be called off-UI-thread
        ''' </summary>
        internal ReadOnly property FileSizeOnceDownloaded as long? 
            get
                return _fileSizeFromDownload
            end get
        end property

        ''' <summary>
        ''' remove this file from Iso storage
        ''' </summary>
        internal Sub DeleteFromIsoStorage()

            Dim iso As IsolatedStorageFile  = IsolatedStorageFile.GetUserStoreForApplication()

            iso.DeleteFile(StreamName)
        End Sub '   DeleteFromIsoStorage
#endif

        ''' <summary>
        ''' clear all isostorage files associated with this application
        ''' </summary>
        Public ReadOnly Property Sub() As
            Get
                Try
                    If ( Not Me.IsAbsoluteUri  AndAlso  HtmlPage.IsEnabled  AndAlso  HtmlPage.Document  IsNot Nothing  AndAlso  HtmlPage.Document.DocumentUri <> Nothing  AndAlso   Not String.IsNullOrEmpty(Me.ToString())) Then

                        Dim oUri As Uri  = HtmlPage.Document.DocumentUri
                        Dim urib As UriBuilder  = New UriBuilder(oUri)

                        urib.Path = System.Uri.UnescapeDataString(oUri.AbsolutePath.Substring(0, oUri.AbsolutePath.LastIndexOf("/", StringComparison.Ordinal) + 1) + Me.OriginalString)
                        return urib.Uri
                    End If
                Catch ex As InvalidOperationException
                    '  can occur offline. no HTML bridge
                End Try

                return this
            End Get
        End Property
    End Class   '   IsoUri
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\OfflineShared\IsoUri.cs
