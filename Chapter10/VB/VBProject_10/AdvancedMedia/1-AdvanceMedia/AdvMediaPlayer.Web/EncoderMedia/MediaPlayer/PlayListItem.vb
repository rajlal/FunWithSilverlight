'  <copyright file="PlaylistItem.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the PlaylistItem class</summary>
'  <author>Microsoft Expression Encoder Team</author>
Imports System
Imports System.ComponentModel
Imports System.Windows.Browser
Imports System.Xml
Imports System.Globalization

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' This class represents a media item in a playlist.
    ''' </summary>
    <ScriptableType>
    Public Class PlaylistItem
        Inherits INotifyPropertyChanged
        Implements IAccessible
        ''' <summary>
        ''' The parent collection of this item.
        ''' </summary>
        Private m_collectionParent As PlaylistCollection
        ''' <summary>
        ''' The title of this item.
        ''' </summary>
        Private m_title As String
        ''' <summary>
        ''' The description of this item.
        ''' </summary>
        Private m_description As String
        ''' <summary>
        ''' The thumbnail source for this item.
        ''' </summary>
        Private m_thumbSource As Uri
        ''' <summary>
        ''' The width of the encoded video for this item.
        ''' </summary>
        private Double m_width
        ''' <summary>
        ''' The height of the encoded video for this item.
        ''' </summary>
        private Double m_height
        ''' <summary>
        ''' The original aspect ratio width for this item.
        ''' </summary>
        private Double m_aspectRatioWidth
        ''' <summary>
        ''' The original aspect ratio height for this item.
        ''' </summary>
        private Double m_aspectRatioHeight
        ''' <summary>
        ''' total filesize of this item
        ''' </summary>
        private long m_fileSize
        ''' <summary>
        ''' The Url for the media of this item.
        ''' </summary>
        Private m_mediaUri As Uri
        ''' <summary>
        ''' collection of closed caption sources
        ''' </summary>
        private ScriptableObservableCollection<CaptionSource> m_captionSources = New ScriptableObservableCollection<CaptionSource>()
        ''' <summary>
        ''' A value indicating whether this item is adaptive streaming or not.
        ''' </summary>
        Private m_isAdaptiveStreaming As Boolean
        ''' <summary>
        ''' The video bitrate of the adaptive stream that was downloaded and stored in isolated storage for offline playback.
        ''' </summary>
        private long m_offlineVideoBitrateInKbps
        ''' <summary>
        ''' The frame rate of this item.
        ''' </summary>
        private SmpteFrameRate m_frameRate = SmpteFrameRate.Unknown
        ''' <summary>
        ''' frame rate in FPS as persisted.
        ''' </summary>
        Private m_frameRateFPS As Double
        ''' <summary>
        ''' The chapters in this item.
        ''' </summary>
        private ScriptableObservableCollection<ChapterItem> m_chapters = New ScriptableObservableCollection<ChapterItem>()
        ''' <summary>
        ''' Property changed event.
        ''' </summary>
        public event PropertyChangedEventHandler PropertyChanged
        ''' <summary>
        ''' init structure
        ''' </summary>
        private Sub Init()

            m_title = string.Empty
            m_description = string.Empty
            m_thumbSource = Nothing
            m_fileSize = 0
            m_frameRate = SmpteFrameRate.Smpte30
            m_width = 640
            m_height = 480
            m_aspectRatioWidth = 4
            m_aspectRatioHeight = 3
            m_offlineVideoBitrateInKbps = 0
        End Sub '   Init
        ''' <summary>
        ''' parameterless constructor required for Edit in Blend.
        ''' </summary>
        Public Sub New()
            m_chapters = New ScriptableObservableCollection<ChapterItem>()
            AddHandler m_chapters.CollectionChanged, AddressOf System.Collections.Specialized.NotifyCollectionChangedEventHandler(Chapters_CollectionChanged)
            Init()
        End Sub '   New
        ''' <summary>
        ''' Initializes a new instance of the PlaylistItem class.
        ''' </summary>
        Public Sub New(collectionParent As PlaylistCollection)
            Init()
            m_collectionParent = collectionParent
        End Sub '   New
        ''' <summary>
        ''' Sets the owner collection. Required to enable declaritve collections
        ''' where playlistitems are instantiated in XAML with default constructor.
        ''' </summary>
        internal PlaylistCollection OwnerCollection
        {
            set
            {
                m_collectionParent = value
            }
        }
        ''' <summary>
        ''' constructor provided for scripting.
        ''' </summary>
        ''' <returns>new chapter</returns>
        public Function CreateNewChapterItem()

            return New ChapterItem()
        End Function  '   CreateNewChapterItem
        ''' <summary>
        ''' Gets the index of this item in the collection.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        Public ReadOnly Property PlaylistIndex() As Integer
            Get


                If (m_collectionParent  IsNot Nothing) Then
                    return m_collectionParent.IndexOf(this) + 1
                else
                    return -1
                End If
            End Get
        End Property
        ''' <summary>
        ''' Gets or sets the description of this playlist item.
        ''' </summary>
        <Description("Description of media item")>
        Public Property Description() As String
            Get

                return m_description
            End Get

            Set

                m_description = value
                OnPropertyChanged("Description")
            End Set
        End Property
        ''' <summary>
        ''' Gets the total file size of the encoded video for this item.
        ''' </summary>
        <TypeConverter(typeofCType()>, LongConverter)
        <Description("file size of item in bytes"), DefaultValue(0)>
        public long FileSize
        {
            get
            {
                return m_fileSize
            }
            set
            {
                m_fileSize = value
            }
        }
        ''' <summary>
        ''' frame rate in FPS as persisted.
        ''' </summary>
        <Description("frame rate in frames per second"), DefaultValue(30)>
        Public Property FrameRate() As Double
            Get

                return m_frameRateFPS
            End Get

            Set

                m_frameRateFPS = value
                SmpteFrameRate = TimeCode.ParseFrameRate(value)
            End Set
        End Property
        ''' <summary>
        ''' Gets the width of the encoded video for this item.
        ''' </summary>
        <Description("height in pixels"), DefaultValue(480)>
        public Double VideoHeight
        {
            get
            {
                return m_height
            }
            set
            {
                m_height = value
            }
        }
        ''' <summary>
        ''' Gets the width of the encoded video for this item.
        ''' </summary>
        <Description("width in pixels"), DefaultValue(640)>
        public Double VideoWidth
        {
            get
            {
                return m_width
            }
            set
            {
                m_width = value
            }
        }

        ''' <summary>
        ''' Gets the aspect ratio height of the encoded video for this item.
        ''' </summary>
        <Description("aspect ratio height"), DefaultValue(3)>
        public Double AspectRatioHeight
        {
            get
            {
                return m_aspectRatioHeight
            }
            set
            {
                m_aspectRatioHeight = value
            }
        }
        ''' <summary>
        ''' Whether the item is DRM protected or not.
        ''' </summary>
        <Description("Whether item is DRM protected or not flag"), DefaultValue(false)>
        Public Property DRM As Boolean

        ''' <summary>
        ''' Gets the aspect ratio width of the encoded video for this item.
        ''' </summary>
        <Description("aspect ratio width"), DefaultValue(4)>
        public Double AspectRatioWidth
        {
            get
            {
                return m_aspectRatioWidth
            }
            set
            {
                m_aspectRatioWidth = value
            }
        }
        ''' <summary>
        ''' Gets or sets a value indicating whether this item uses adaptive streaming.
        ''' </summary>
        <Description("is adaptive streaming item")>
        Public Property IsAdaptiveStreaming() As Boolean
            Get

                return m_isAdaptiveStreaming
            End Get

            Set

                m_isAdaptiveStreaming = value
                OnPropertyChanged("IsAdaptiveStreaming")
            End Set
        End Property
		' / <summary>
        ''' Gets the offline video bitrate used for adaptive streaming.
        ''' Note this item is not scriptable
        ''' </summary>
        internal long OfflineVideoBitrateInKbps
        {
            get
            {
                return m_offlineVideoBitrateInKbps
            }
            set
            {
                m_offlineVideoBitrateInKbps = value
            }
        }

        ''' <summary>
        ''' Replaces back slashes in a Uri with more net friendly forward slashes
        ''' </summary>
        ''' <param name="uriToFix"></param>
        ''' <returns></returns>
        Public ReadOnly Property Uri() As

        ''' <summary>
        ''' Gets or sets the Url of the media item.
        ''' </summary>
        <Description("uri of item")>
        Public Property MediaSource() As Uri
            Get

                return m_mediaUri
            End Get

            Set

                m_mediaUri = FixupBackSlashesInAbsoluteUri(value)
                OnPropertyChanged("MediaSource")
            End Set
        End Property
        ''' <summary>
        ''' Gets or sets the list of closed caption sources
        ''' </summary>
        <Description("list of closed caption source files")>
        public ScriptableObservableCollection<CaptionSource> CaptionSources
        {
            get
            {
                return Me.m_captionSources
            }

            set
            {
                Me.m_captionSources = value
                OnPropertyChanged("CaptionSources")
            }
        }
        ''' <summary>
        ''' Gets or sets the source of the thumbnail for this item.
        ''' Uses a string instead of a URI to facilitate binding and handling cases where
        ''' the thumbnail file is missing without generating a page error.
        ''' </summary>
        <Description("optional thumbnail for gallery and poster frame")>
        Public Property ThumbSource() As Uri
            Get

                return m_thumbSource
            End Get

            Set

                m_thumbSource = value
                OnPropertyChanged("ThumbSource")
            End Set
        End Property
        ''' <summary>
        ''' Gets or sets the title of the playlist item.
        ''' </summary>
        <Description("title of item")>
        Public Property Title() As String
            Get

                return m_title
            End Get

            Set

                m_title = value
                OnPropertyChanged("Title")
            End Set
        End Property
        ''' <summary>
        ''' Gets the chapters in this item.
        ''' </summary>
        <Description("list of chapters")>
        public ScriptableObservableCollection<ChapterItem> Chapters
        {
            get { return m_chapters; }
            set { m_chapters = value; }
        }
        ''' <summary>
        ''' Gets or sets the frame rate of this item.
        ''' </summary>
        <EditorBrowsable(EditorBrowsableState.Never)>
        public SmpteFrameRate SmpteFrameRate
        {
            get
            {
                return m_frameRate
            }
            set
            {
                m_frameRate = value
                m_frameRateFPS = 1 / TimeCode.FromFrames(1, value).TotalSeconds
                OnPropertyChanged("FrameRate")
            }
        }
        ''' <summary>
        ''' chapters collection changed. means this playlist item changed
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        private Sub Chapters_CollectionChanged(sender As Object, e As System.Collections.Specialized.NotifyCollectionChangedEventArgs)

            OnPropertyChanged("Chapters")
        End Sub '   Chapters_CollectionChanged
        ''' <summary>
        ''' accessibility text, will be read out by screen reader if your listbox item has a control
        ''' AutomationProperties.Name bound to this
        ''' </summary>
        Dim IAccessible.AccessibilityText As String

        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, ExpressionMediaPlayer.Resources.readerPlaylistItem, Title, Description)
            }
        }
        #region INotifyPropertyChanged Members
        ''' <summary>
        ''' Implements INotifyPropertyChanged.OnPropertyChanged().
        ''' </summary>
        ''' <param name="memberName">The name of the property that changed.</param>
        protected Sub OnPropertyChanged(memberName As String)


            If (PropertyChanged  IsNot Nothing) Then
                PropertyChanged(this, New PropertyChangedEventArgs(memberName))
            End If
        End Sub '   OnPropertyChanged
        #End Region
        #region Serialization
        ''' <summary>
        ''' top level XML node for this class & chapters
        ''' </summary>
        internal const string xmlNode = "PlaylistItem"
        internal const string xmlChaptersNode = "Chapters"
        internal const string xmlCaptionSourcesNode = "CaptionSources"
        ''' <summary>
        ''' deserialise chapters
        ''' </summary>
        ''' <param name="reader">XmlReader to deserialize from</param>
        ''' <returns>this</returns>
        private Sub DeserializeChapters(reader As XmlReader)


            If ( Not reader.IsStartElement(PlaylistItem.xmlChaptersNode)) Then
                throw New InvalidPlaylistException()
            End If

            reader.Read()
              PlaylistItem.xmlChaptersNode  AndAlso  reader.NodeType = XmlNodeType.EndElement))
              While ( Not (reader.Name = XmlNodeType.EndElement))


                If (reader.IsStartElement("ChapterItem")) Then
                    Me.Chapters.Add(new ChapterItem().Deserialize(reader))
                else if (reader.IsStartElement())
                    throw New InvalidPlaylistException(reader.Name, PlaylistItem.xmlChaptersNode)
                End If
                else if ( Not reader.Read())
            End While   '
        End Sub '   DeserializeChapters
        ''' <summary>
        ''' deserialise caption Sources
        ''' </summary>
        ''' <param name="reader">XmlReader to deserialize from</param>
        ''' <returns>this</returns>
        private Sub DeserializeCaptionSources(reader As XmlReader)


            If ( Not reader.IsStartElement(PlaylistItem.xmlCaptionSourcesNode)) Then
                throw New InvalidPlaylistException()
            End If

            reader.Read()
              PlaylistItem.xmlCaptionSourcesNode  AndAlso  reader.NodeType = XmlNodeType.EndElement))
              While ( Not (reader.Name = XmlNodeType.EndElement))


                If (reader.IsStartElement(CaptionSource.xmlNode)) Then
                    Me.CaptionSources.Add(new CaptionSource().Deserialize(reader))
                else if (reader.IsStartElement())
                    throw New InvalidPlaylistException(reader.Name, PlaylistItem.xmlCaptionSourcesNode)
                End If
                else if ( Not reader.Read())
            End While   '
        End Sub '   DeserializeCaptionSources
        ''' <summary>
        ''' deserialise this object
        ''' </summary>
        ''' <param name="reader">XmlReader to deserialize from</param>
        ''' <returns>this</returns>
        internal PlaylistItem Deserialize(XmlReader reader)
        {

            If ( Not reader.IsStartElement(PlaylistItem.xmlNode)) Then
                throw New InvalidPlaylistException()
            End If

            Init()
            reader.Read()
              PlaylistItem.xmlNode  AndAlso  reader.NodeType = XmlNodeType.EndElement))
              While ( Not (reader.Name = XmlNodeType.EndElement))


                If (reader.IsStartElement("Description")) Then
                    Me.Description = reader.ReadElementContentAsString()
                else if (reader.IsStartElement("FileSize"))
                    Me.FileSize = reader.ReadElementContentAsLong()
                End If
                else if (reader.IsStartElement("FrameRate"))
                    Me.FrameRate = reader.ReadElementContentAsDouble()
                else if (reader.IsStartElement("Height"))
                    Me.VideoHeight = reader.ReadElementContentAsDouble()
                else if (reader.IsStartElement("IsAdaptiveStreaming"))
                    Me.IsAdaptiveStreaming = reader.ReadElementContentAsBoolean()
				else if (reader.IsStartElement("OfflineVideoBitrateInKbps"))
                    Me.OfflineVideoBitrateInKbps = reader.ReadElementContentAsLong()
                else if (reader.IsStartElement("MediaSource"))
                {

                    Dim rawMediaSourceUrl As String  = reader.ReadElementContentAsString()

                    Me.MediaSource = New Uri(rawMediaSourceUrl, UriKind.RelativeOrAbsolute)
                }
                else if (reader.IsStartElement("ThumbSource"))
                {

                    Dim rawThumbSourceUrl As String  = reader.ReadElementContentAsString()

                    Me.ThumbSource = New Uri(rawThumbSourceUrl, UriKind.RelativeOrAbsolute)
                }
                else if (reader.IsStartElement("Title"))
                    Me.Title = reader.ReadElementContentAsString()
                else if (reader.IsStartElement("Width"))
                    Me.VideoWidth = reader.ReadElementContentAsDouble()
                else if (reader.IsStartElement(PlaylistItem.xmlChaptersNode))
                    DeserializeChapters(reader)
                else if (reader.IsStartElement(PlaylistItem.xmlCaptionSourcesNode))
                    DeserializeCaptionSources(reader)
                else if (reader.IsStartElement("AudioCodec"))
                    '  ignored
                    reader.ReadElementContentAsObject()
                else if (reader.IsStartElement("VideoCodec"))
                    '  ignored
                    reader.ReadElementContentAsObject()
                else if (reader.IsStartElement("AspectRatioWidth"))
                    Me.AspectRatioWidth = reader.ReadElementContentAsDouble()
                else if (reader.IsStartElement("AspectRatioHeight"))
                    Me.AspectRatioHeight = reader.ReadElementContentAsDouble()
                else if (reader.IsStartElement("DRM"))
                    Me.DRM = reader.ReadElementContentAsBoolean()
                else if (reader.IsStartElement())
                    throw New InvalidPlaylistException(reader.Name, PlaylistItem.xmlNode)
                else if ( Not reader.Read())
            End While   '

            return this
        }
        ''' <summary>
        ''' serialize this object
        ''' </summary>
        ''' <param name="writer">XmlWriter to serialze to</param>
        internal Sub Serialize(writer As XmlWriter)

            writer.WriteStartElement(PlaylistItem.xmlNode)
            writer.WriteElementString("Description", Me.Description)
            writer.WriteElementString("FileSize", Me.FileSize.ToString(CultureInfo.InvariantCulture))
            writer.WriteElementString("FrameRate", Me.FrameRate.ToString(CultureInfo.InvariantCulture))
            writer.WriteElementString("Height", Me.VideoHeight.ToString(CultureInfo.InvariantCulture))
            writer.WriteElementString("IsAdaptiveStreaming", Me.IsAdaptiveStreaming.ToString().ToLower(CultureInfo.InvariantCulture))
			writer.WriteElementString("OfflineVideoBitrateInKbps", Me.OfflineVideoBitrateInKbps.ToString(CultureInfo.InvariantCulture))
            writer.WriteElementString("MediaSource", Me.MediaSource.ToString())

            If (Me.CaptionSources  IsNot Nothing) Then

                If (CaptionSources.Count > 0) Then
                    writer.WriteStartElement(PlaylistItem.xmlCaptionSourcesNode)

                    For Each var item in CaptionSources

                        item.Serialize(writer)
                    Next    '   var

                    writer.WriteEndElement()
                End If
            End If

            If (Me.ThumbSource  IsNot Nothing) Then
                writer.WriteElementString("ThumbSource", Me.ThumbSource.ToString())
            End If


            writer.WriteElementString("Title", Me.Title)
            writer.WriteElementString("Width", Me.VideoWidth.ToString(CultureInfo.InvariantCulture))
            writer.WriteElementString("AspectRatioWidth", Me.AspectRatioWidth.ToString(CultureInfo.InvariantCulture))
            writer.WriteElementString("AspectRatioHeight", Me.AspectRatioHeight.ToString(CultureInfo.InvariantCulture))
            writer.WriteElementString("DRM", Me.DRM.ToString().ToLower(CultureInfo.InvariantCulture))


            If ((Me.Chapters  IsNot Nothing)  AndAlso  (Me.Chapters.Count > 0)) Then
                writer.WriteStartElement(PlaylistItem.xmlChaptersNode)

                For Each var item in Chapters

                    item.Serialize(writer)
                Next    '   var

                writer.WriteEndElement()
            End If


            writer.WriteEndElement()
        End Sub '   Serialize

        #End Region
    End Class   '   PlaylistItem
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\PlayListItem.cs
