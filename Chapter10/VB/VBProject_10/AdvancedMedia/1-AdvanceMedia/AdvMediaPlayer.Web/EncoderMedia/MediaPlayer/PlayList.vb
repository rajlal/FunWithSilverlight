'  <copyright file="PlaylistCollection.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the PlaylistCollection class</summary>
'  <author>Microsoft Expression Encoder Team</author>
Imports System
Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Browser
Imports System.Windows.Media
Imports System.Threading
Imports System.Xml
Imports System.IO.IsolatedStorage
Imports System.IO
Imports System.Globalization
Imports System.Text

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' exception for invalid playlist
    ''' </summary>
    Public Class InvalidPlaylistException
        Inherits Exception
        Public Sub New()
        End Sub '   New

        Public Sub New(message As String)

        {
        End Sub '   New

        Public Sub New(nodeFound As String, nodeExpected As String)

        {
        End Sub '   New
    End Class   '   InvalidPlaylistException

    '  Summary:
    '      Describes how content is resized to fill its allocated space -- taking into account non-square pixels
    public enum StretchNonSquarePixels
    {
        '  Summary:
        '      The content preserves its original size and frame aspect ratio
        NoStretch = 0,
        '
        '  Summary:
        '      The content is resized to fill the destination dimensions. The frame aspect ratio is preserved.
        StretchToFill = 1,
        '
        '  Summary:
        '      The content is resized to fill the destination dimensions. Original size and frame aspect ratio are ignore.
        StretchDistorted = 2,
    }

    <ScriptableType>
    Public Partial Class Playlist
        Inherits DependencyObject
    {
        #region PlaylistCore
        ''' <summary>
        ''' playlist options
        ''' </summary>
        Private m_autoLoad As Boolean
        Private m_autoPlay As Boolean
        Private m_autoRepeat As Boolean
        Private m_displayTimeCode As Boolean
        Private m_enableCachedComposition As Boolean
        Private m_enableCaptions As Boolean
        Private m_enableOffline As Boolean
        Private m_enablePopOut As Boolean
        Private m_startMuted As Boolean
        Private m_startWithPlaylistShowing As Boolean
        private StretchNonSquarePixels m_stretchMode
        Private m_background As Color

        ''' <summary>
        ''' The number of items in the playlist that are DRM protected
        ''' </summary>
        Private m_countOfDRMItems As Integer
        ''' <summary>
        ''' The number of items in the playlist that are Live Items
        ''' </summary>
        Private m_countOfLiveItems As Integer
        ''' <summary>
        ''' The number of items in the playlist that are adaptive Items
        ''' </summary>
        Private m_countOfAdaptiveItems As Integer

        ''' <summary>
        ''' list of playlist items
        ''' </summary>
        private PlaylistCollection _Items = New PlaylistCollection()
        ''' <summary>
        ''' playlist changed event
        ''' </summary>
        <ScriptableMember>
        public event RoutedEventHandler PlaylistChanged
        ''' <summary>
        ''' should playlist cue up
        ''' </summary>
        <Description("Automatically cue video when page is loaded"), DefaultValue(true)>
        Public Property AutoLoad() As Boolean
            Get

                return m_autoLoad
            End Get

            Set

                m_autoLoad = value

                If (PlaylistChanged  IsNot Nothing) Then
                    PlaylistChanged(this, Nothing)
                End If
            End Set
        End Property
        ''' <summary>
        ''' should playlist auto start?
        ''' </summary>
        <Description("Automatically start video when cued"), DefaultValue(true)>
        Public Property AutoPlay() As Boolean
            Get

                return m_autoPlay
            End Get

            Set

                m_autoPlay = value

                If (PlaylistChanged  IsNot Nothing) Then
                    PlaylistChanged(this, Nothing)
                End If
            End Set
        End Property
        ''' <summary>
        ''' should playlist auto repeat?
        ''' </summary>
        <Description("Automatically restart video when at end"), DefaultValue(false)>
        Public Property AutoRepeat() As Boolean
            Get

                return m_autoRepeat
            End Get

            Set

                m_autoRepeat = value

                If (PlaylistChanged  IsNot Nothing) Then
                    PlaylistChanged(this, Nothing)
                End If
            End Set
        End Property
        ''' <summary>
        ''' display timecodes
        ''' </summary>
        <Description("Display Timecode")>
        Public Property DisplayTimeCode() As Boolean
            Get

                return m_displayTimeCode
            End Get

            Set

                m_displayTimeCode = value

                If (PlaylistChanged  IsNot Nothing) Then
                    PlaylistChanged(this, Nothing)
                End If
            End Set
        End Property
        ''' <summary>
        ''' cached composition enabled
        ''' </summary>
        <Description("Enable Cached Composition"), DefaultValue(true)>
        Public Property EnableCachedComposition() As Boolean
            Get

                return m_enableCachedComposition
            End Get

            Set

                m_enableCachedComposition = value

                If (PlaylistChanged  IsNot Nothing) Then
                    PlaylistChanged(this, Nothing)
                End If
            End Set
        End Property
        ''' <summary>
        ''' captions enabled
        ''' </summary>
        <Description("Allow closed captions to show"), DefaultValue(true)>
        Public Property EnableCaptions() As Boolean
            Get

                return m_enableCaptions
            End Get

            Set

                m_enableCaptions = value

                If (PlaylistChanged  IsNot Nothing) Then
                    PlaylistChanged(this, Nothing)
                End If
            End Set
        End Property
        ''' <summary>
        ''' offlining enabled
        ''' </summary>
        <Description("Enable Player to be run offline"), DefaultValue(true)>
        Public Property EnableOffline() As Boolean
            Get

                return m_enableOffline  AndAlso  (m_countOfDRMItems < 1)  AndAlso  (m_countOfLiveItems < 1)
            End Get

            Set

                m_enableOffline = value

                If (PlaylistChanged  IsNot Nothing) Then
                    PlaylistChanged(this, Nothing)
                End If
            End Set
        End Property
        ''' <summary>
        ''' popout enabled
        ''' </summary>
        <Description("Enable Player popout"), DefaultValue(true)>
        Public Property EnablePopOut() As Boolean
            Get

                return m_enablePopOut
            End Get

            Set

                m_enablePopOut = value

                If (PlaylistChanged  IsNot Nothing) Then
                    PlaylistChanged(this, Nothing)
                End If
            End Set
        End Property


        ''' <summary>
        ''' HasAdaptiveItems
        ''' </summary>
        <Description("Indicated whether there are any Smooth Streaming items in the playlist")>
        Public ReadOnly Property HasAdaptiveItems() As Boolean
            Get

                return m_countOfAdaptiveItems > 0
            End Get
        End Property

        ''' <summary>
        ''' start in muted state
        ''' </summary>
        <Description("Mute player on start")>
        Public Property StartMuted() As Boolean
            Get

                return m_startMuted
            End Get

            Set

                m_startMuted = value

                If (PlaylistChanged  IsNot Nothing) Then
                    PlaylistChanged(this, Nothing)
                End If
            End Set
        End Property


        ''' <summary>
        ''' start with playlist visible
        ''' </summary>
        <Description("Show playlist on start")>
        Public Property StartWithPlaylistShowing() As Boolean
            Get

                return m_startWithPlaylistShowing
            End Get

            Set

                m_startWithPlaylistShowing = value

                If (PlaylistChanged  IsNot Nothing) Then
                    PlaylistChanged(this, Nothing)
                End If
            End Set
        End Property
        ''' <summary>
        ''' type of video stretch
        ''' </summary>
        <Description("Stretch Mode")>
        public StretchNonSquarePixels StretchNonSquarePixels
        {
            get
            {
                return m_stretchMode
            }
            set
            {
                m_stretchMode = value

                If (PlaylistChanged  IsNot Nothing) Then
                    PlaylistChanged(this, Nothing)
                End If

            }
        }
        ''' <summary>
        ''' color of background
        ''' </summary>
        Public Property Background() As Color
            Get

                return m_background
            End Get

            Set

                m_background = value

                If (PlaylistChanged  IsNot Nothing) Then
                    PlaylistChanged(this, Nothing)
                End If
            End Set
        End Property
        ''' <summary>
        ''' list of playlist items.
        ''' </summary>
        <Description("Playlist")>
        public PlaylistCollection Items
        {
            get
            {
                return _Items
            }
            set
            {
                _Items = value
            }
        }
        ''' <summary>
        ''' init structure
        ''' </summary>
        private Sub Init()

            AutoLoad = true
            AutoPlay = true
            AutoRepeat = false
            EnableCachedComposition = true
            EnableCaptions = true
            EnablePopOut = true
            EnableOffline = true
            m_countOfDRMItems = 1
            m_countOfLiveItems = 1
            m_countOfAdaptiveItems = 0
        End Sub '   Init
        ''' <summary>
        ''' construct a playlistitem, provided for scripting.
        ''' </summary>
        ''' <returns>new playlist item</returns>
        public PlaylistItem CreateNewPlaylistItem()
        {
            return New PlaylistItem(Items)
        }
        ''' <summary>
        ''' playlist constructor
        ''' </summary>
        Public Sub New()
            Me.StretchNonSquarePixels = StretchNonSquarePixels.NoStretch
            _Items = New PlaylistCollection()
            AddHandler _Items.CollectionChanged, AddressOf System.Collections.Specialized.NotifyCollectionChangedEventHandler(Items_CollectionChanged)
            Init()
        End Sub '   New
        ''' <summary>
        ''' inform playlist changed if the items collection changed
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        private Sub Items_CollectionChanged(sender As Object, e As System.Collections.Specialized.NotifyCollectionChangedEventArgs)


            If (PlaylistChanged <> Nothing) Then
                PlaylistChanged(this, Nothing)
            End If

            m_countOfDRMItems = 0
            m_countOfLiveItems = 0
            m_countOfAdaptiveItems = 0

            For Each var item in Me.Items


                If (item.DRM) Then
                    m_countOfDRMItems += 1
                End If


                If (item.IsAdaptiveStreaming) Then
                    m_countOfAdaptiveItems += 1

                    Dim source As String  = item.MediaSource.OriginalString.ToLower()

                    if (source.Contains(@".isml/manifest"))
                    {
                        m_countOfLiveItems += 1
                    }
                else

                    If (item.MediaSource.IsAbsoluteUri) Then

                        Dim scheme As String  = item.MediaSource.Scheme.ToLower()


                        If (scheme = ("mms:")) Then
                            m_countOfLiveItems += 1
                        End If
                    End If
                End If
            Next    '   var
        End Sub '   Items_CollectionChanged
        ''' <summary>
        ''' create playlist from XML string
        ''' </summary>
        ''' <param name="playlistXml">XML string</param>
        ''' <returns>playlist</returns>
        Public ReadOnly Property Playlist() As
        ''' <summary>
        ''' parse playlist from XML
        ''' </summary>
        ''' <param name="playlistXml"></param>
        public Sub ParseXml(playlistXml As String)

            UTF8Encoding enc = New UTF8Encoding()
            using (MemoryStream ms = New MemoryStream(enc.GetBytes(playlistXml)))
            {
                XmlReaderSettings xmlrs = New XmlReaderSettings()
                xmlrs.IgnoreComments = true
                xmlrs.IgnoreWhitespace = true

                Dim reader As XmlReader  = XmlReader.Create(ms, xmlrs)

                Deserialize(reader)
            }

            If (PlaylistChanged  IsNot Nothing) Then
                PlaylistChanged(this, Nothing)
            End If
        End Sub '   ParseXml
        #End Region

        #region Serialization
        ''' <summary>
        ''' top level XML node for this class
        ''' </summary>
        internal const string xmlNode = "Playlist"
        ''' <summary>
        ''' deserialise this object
        ''' </summary>
        ''' <param name="reader">XmlReader to deserialize from</param>
        ''' <returns>this</returns>
        internal Sub Deserialize(reader As XmlReader)


            If ( Not reader.IsStartElement(Playlist.xmlNode)) Then
                throw New InvalidPlaylistException()
            End If

            Init()
            reader.Read()
              Playlist.xmlNode  AndAlso  reader.NodeType = XmlNodeType.EndElement))
              While ( Not (reader.Name = XmlNodeType.EndElement))


                If (reader.IsStartElement("AutoLoad")) Then
                    Me.AutoLoad = reader.ReadElementContentAsBoolean()
                else if (reader.IsStartElement("AutoPlay"))
                    Me.AutoPlay = reader.ReadElementContentAsBoolean()
                End If
                else if (reader.IsStartElement("AutoRepeat"))
                    Me.AutoRepeat = reader.ReadElementContentAsBoolean()
                else if (reader.IsStartElement("DisplayTimeCode"))
                    Me.DisplayTimeCode = reader.ReadElementContentAsBoolean()
                else if (reader.IsStartElement("EnableCachedComposition"))
                    Me.EnableCachedComposition = reader.ReadElementContentAsBoolean()
                else if (reader.IsStartElement("EnableCaptions"))
                    Me.EnableCaptions = reader.ReadElementContentAsBoolean()
                else if (reader.IsStartElement("EnablePopOut"))
                    Me.EnablePopOut = reader.ReadElementContentAsBoolean()
                else if (reader.IsStartElement("EnableOffline"))
                    Me.EnableOffline = reader.ReadElementContentAsBoolean()
                else if (reader.IsStartElement("StartMuted"))
                    Me.StartMuted = reader.ReadElementContentAsBoolean()
                else if (reader.IsStartElement("StartWithPlaylistShowing"))
                {
                    Me.StartWithPlaylistShowing = reader.ReadElementContentAsBoolean()
                }
                else if (reader.IsStartElement("StretchMode"))
                {

                    Dim tmp As Stretch  = (Stretch)Enum.Parse(GetType(Stretch), reader.ReadElementContentAsString(), false)

                    switch(tmp)
                    {
                        case Stretch.None:
                            Me.StretchNonSquarePixels = StretchNonSquarePixels.NoStretch

                        case Stretch.Uniform:
                        case Stretch.UniformToFill:
                            Me.StretchNonSquarePixels = StretchNonSquarePixels.StretchToFill

                        case Stretch.Fill:
                            Me.StretchNonSquarePixels = StretchNonSquarePixels.StretchDistorted

                    }
                }
                else if (reader.IsStartElement("StretchNonSquarePixels"))
                {
                    StretchNonSquarePixels tmp = (StretchNonSquarePixels)Enum.Parse(GetType(StretchNonSquarePixels), reader.ReadElementContentAsString(), false)
                    Me.StretchNonSquarePixels = tmp
                }
                else if (reader.IsStartElement(PlaylistCollection.xmlNode))
                {
                    Me.Items.Clear()
                    Me.Items.Deserialize(reader)
                }
                else if (reader.IsStartElement())
                    throw New InvalidPlaylistException(reader.Name, Playlist.xmlNode)
                else if ( Not reader.Read())
            End While   '
        End Sub '   Deserialize
        ''' <summary>
        ''' serialize this object
        ''' </summary>
        ''' <param name="writer">XmlWriter</param>
        internal Sub Serialize(writer As XmlWriter)

            writer.WriteStartElement(Playlist.xmlNode)
            writer.WriteElementString("AutoLoad", Me.AutoLoad.ToString().ToLower(CultureInfo.InvariantCulture))
            writer.WriteElementString("AutoPlay", Me.AutoPlay.ToString().ToLower(CultureInfo.InvariantCulture))
            writer.WriteElementString("AutoRepeat", Me.AutoRepeat.ToString().ToLower(CultureInfo.InvariantCulture))
            writer.WriteElementString("DisplayTimeCode", Me.DisplayTimeCode.ToString().ToLower(CultureInfo.InvariantCulture))
            writer.WriteElementString("EnableCachedComposition", Me.EnableCachedComposition.ToString().ToLower(CultureInfo.InvariantCulture))
            writer.WriteElementString("EnableCaptions", Me.EnableCaptions.ToString().ToLower(CultureInfo.InvariantCulture))
            writer.WriteElementString("EnablePopOut", Me.EnablePopOut.ToString().ToLower(CultureInfo.InvariantCulture))
            writer.WriteElementString("EnableOffline", Me.EnableOffline.ToString().ToLower(CultureInfo.InvariantCulture))
            writer.WriteElementString("StartMuted", Me.StartMuted.ToString().ToLower(CultureInfo.InvariantCulture))
            writer.WriteElementString("StretchNonSquarePixels", Me.StretchNonSquarePixels.ToString())
            _Items.Serialize(writer)
            writer.WriteEndElement()
        End Sub '   Serialize

        #End Region
    End Class   '   Playlist
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\PlayList.cs
