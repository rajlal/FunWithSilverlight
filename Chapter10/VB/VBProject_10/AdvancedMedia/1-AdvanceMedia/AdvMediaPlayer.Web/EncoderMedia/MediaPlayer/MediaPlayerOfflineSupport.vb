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
        ''' The base URI of the online player -- which is needed so the offline player construct the correct IsoUri StreamNames for loading the chunks
        ''' </summary>
        Private ReadOnly Property Uri() As
            Get


                If (Application.Current.InstallState = InstallState.Installed) Then

                    If (OfflineBaseUri  IsNot Nothing) Then
                        return true
                    End If
                End If
                return false
            End Get
        End Property
        ''' <summary>
        ''' Obtains the base URI of the online player -- stored in the special file in isloated storage
        ''' </summary>
        Public ReadOnly Property Uri() As
            Get


                If (sm_offlineBaseUri Is Nothing) Then
                    Try
                        var isoStore = IsolatedStorageFile.GetUserStoreForApplication()
#if DEBUG
                        string() dirNames = isoStore.GetDirectoryNames()

                        For Each dirName As String in dirNames

                            Debug.WriteLine("IsolatedStorage dirName=" + dirName)
                        Next    '   dirName

                        string() fileNames = isoStore.GetFileNames()

                        For Each fileName As String in fileNames

                            Debug.WriteLine("IsolatedStorage fileName=" + fileName)
                        Next    '   fileName

#endif
                        var streamReader = New StreamReader(isoStore.OpenFile(Playlist.downloadCompletedFilename, System.IO.FileMode.Open, System.IO.FileAccess.Read), Encoding.Unicode)

                        Dim offlineBaseUrl As String  = streamReader.ReadLine()
                        Dim videoBitrateTakenOfflineText As String  = streamReader.ReadLine()

                        streamReader.Close()
                        sm_offlineBaseUri = New Uri(offlineBaseUrl, UriKind.Absolute)
#if DEBUG
                        Debug.WriteLine("OfflineBaseUri=" + offlineBaseUrl)
#endif

                    Catch iso As IsolatedStorageException

                        Debug.WriteLine("IsolatedStorageException attempting read the download complete file Not :" + iso.Message)
                        StaticShowErrorMessage(iso.Message)
                    End Try
                End If
                return sm_offlineBaseUri
            End Get
        End Property
        ''' <summary>
        ''' For offline playback form a useful IsoUri
        ''' </summary>
        internal static IsoUri MakeOfflineIsoUri(Uri mediaUri)
        {

            If (mediaUri  IsNot Nothing) Then
                IsoUri isoUri

                If (mediaUri.IsAbsoluteUri) Then
                    isoUri = New IsoUri(mediaUri)
                else
                    isoUri = New IsoUri(new Uri(MediaPlayer.OfflineBaseUri, mediaUri))
                End If

                return isoUri
            End If

            return Nothing
        }
    End Class   '   MediaPlayer
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\MediaPlayerOfflineSupport.cs
