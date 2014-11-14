// <copyright file="PlaylistItem.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>Implements the PlaylistItem class</summary>
// <author>Microsoft Expression Encoder Team</author>
using System;
using System.ComponentModel;
using System.Windows.Browser;
using System.Xml;
using System.Globalization;

namespace ExpressionMediaPlayer
{
    /// <summary>
    /// This class represents a media item in a playlist.
    /// </summary>
    [ScriptableType]
    public class PlaylistItem : INotifyPropertyChanged, IAccessible
    {
        /// <summary>
        /// The parent collection of this item.
        /// </summary>
        private PlaylistCollection m_collectionParent;
        /// <summary>
        /// The title of this item.
        /// </summary>
        private String m_title;
        /// <summary>
        /// The description of this item.
        /// </summary>
        private String m_description;
        /// <summary>
        /// The thumbnail source for this item.
        /// </summary>
        private Uri m_thumbSource;
        /// <summary>
        /// The width of the encoded video for this item.
        /// </summary>
        private Double m_width;
        /// <summary>
        /// The height of the encoded video for this item.
        /// </summary>
        private Double m_height;
        /// <summary>
        /// The original aspect ratio width for this item.
        /// </summary>
        private Double m_aspectRatioWidth;
        /// <summary>
        /// The original aspect ratio height for this item.
        /// </summary>
        private Double m_aspectRatioHeight;
        /// <summary>
        /// total filesize of this item
        /// </summary>
        private long m_fileSize;
        /// <summary>
        /// The Url for the media of this item.
        /// </summary>
        private Uri m_mediaUri;
        /// <summary>
        /// collection of closed caption sources
        /// </summary>
        private ScriptableObservableCollection<CaptionSource> m_captionSources = new ScriptableObservableCollection<CaptionSource>();
        /// <summary>
        /// A value indicating whether this item is adaptive streaming or not.
        /// </summary>
        private bool m_isAdaptiveStreaming;
        /// <summary>
        /// The video bitrate of the adaptive stream that was downloaded and stored in isolated storage for offline playback. 
        /// </summary>
        private long m_offlineVideoBitrateInKbps;
        /// <summary>
        /// The frame rate of this item.
        /// </summary>
        private SmpteFrameRate m_frameRate = SmpteFrameRate.Unknown;
        /// <summary>
        /// frame rate in FPS as persisted.
        /// </summary>
        private double m_frameRateFPS;        
        /// <summary>
        /// The chapters in this item.
        /// </summary>
        private ScriptableObservableCollection<ChapterItem> m_chapters = new ScriptableObservableCollection<ChapterItem>();
        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// init structure
        /// </summary>
        private void Init()
        {
            m_title = string.Empty;
            m_description = string.Empty;
            m_thumbSource = null;
            m_fileSize = 0;
            m_frameRate = SmpteFrameRate.Smpte30;
            m_width = 640;
            m_height = 480;
            m_aspectRatioWidth = 4;
            m_aspectRatioHeight = 3;
            m_offlineVideoBitrateInKbps = 0;
        }
        /// <summary>
        /// parameterless constructor required for Edit in Blend.
        /// </summary>
        public PlaylistItem()
        {
            m_chapters = new ScriptableObservableCollection<ChapterItem>();
            m_chapters.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Chapters_CollectionChanged);            
            Init();            
        }
        /// <summary>
        /// Initializes a new instance of the PlaylistItem class.
        /// </summary>
        public PlaylistItem(PlaylistCollection collectionParent)
        {
            Init();
            m_collectionParent = collectionParent;
        }
        /// <summary>
        /// Sets the owner collection. Required to enable declaritve collections 
        /// where playlistitems are instantiated in XAML with default constructor.
        /// </summary>
        internal PlaylistCollection OwnerCollection
        {
            set
            {
                m_collectionParent = value;
            }
        }
        /// <summary>
        /// constructor provided for scripting.
        /// </summary>
        /// <returns>new chapter</returns>
        public ChapterItem CreateNewChapterItem()
        {
            return new ChapterItem();
        }
        /// <summary>
        /// Gets the index of this item in the collection.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)]
        public int PlaylistIndex
        {
            get
            {
                if (m_collectionParent != null)
                {
                    return m_collectionParent.IndexOf(this) + 1;
                }
                else
                {
                    return -1;
                }
            }
        }
        /// <summary>
        /// Gets or sets the description of this playlist item.
        /// </summary>
        [Description("Description of media item")]       
        public String Description
        {
            get
            {
                return m_description;
            }

            set
            {
                m_description = value;
                OnPropertyChanged("Description");
            }
        }
        /// <summary>
        /// Gets the total file size of the encoded video for this item.
        /// </summary>
        [TypeConverter(typeof(LongConverter))]
        [Description("file size of item in bytes"), DefaultValue(0)]
        public long FileSize
        {
            get
            {
                return m_fileSize;
            }
            set
            {
                m_fileSize = value;
            }
        }
        /// <summary>
        /// frame rate in FPS as persisted.
        /// </summary>
        [Description("frame rate in frames per second"), DefaultValue(30)]
        public double FrameRate
        {
            get
            {
                return m_frameRateFPS;
            }
            set
            {
                m_frameRateFPS = value;
                SmpteFrameRate = TimeCode.ParseFrameRate(value);
            }
        }
        /// <summary>
        /// Gets the width of the encoded video for this item.
        /// </summary>
        [Description("height in pixels"), DefaultValue(480)]
        public Double VideoHeight
        {
            get
            {
                return m_height;
            }
            set
            {
                m_height = value;
            }
        }
        /// <summary>
        /// Gets the width of the encoded video for this item.
        /// </summary>
        [Description("width in pixels"), DefaultValue(640)]
        public Double VideoWidth
        {
            get
            {
                return m_width;
            }
            set
            {
                m_width = value;
            }
        }

        /// <summary>
        /// Gets the aspect ratio height of the encoded video for this item.
        /// </summary>
        [Description("aspect ratio height"), DefaultValue(3)]
        public Double AspectRatioHeight
        {
            get
            {
                return m_aspectRatioHeight;
            }
            set
            {
                m_aspectRatioHeight = value;
            }
        }
        /// <summary>
        /// Whether the item is DRM protected or not.
        /// </summary>
        [Description("Whether item is DRM protected or not flag"), DefaultValue(false)]
        public bool DRM { get; set; }

        /// <summary>
        /// Gets the aspect ratio width of the encoded video for this item.
        /// </summary>
        [Description("aspect ratio width"), DefaultValue(4)]
        public Double AspectRatioWidth
        {
            get
            {
                return m_aspectRatioWidth;
            }
            set
            {
                m_aspectRatioWidth = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this item uses adaptive streaming.
        /// </summary>
        [Description("is adaptive streaming item")]
        public bool IsAdaptiveStreaming
        {
            get
            {
                return m_isAdaptiveStreaming;
            }

            set
            {
                m_isAdaptiveStreaming = value;
                OnPropertyChanged("IsAdaptiveStreaming");
            }
        }
		/// <summary>
        /// Gets the offline video bitrate used for adaptive streaming.
        /// Note this item is not scriptable
        /// </summary>
        internal long OfflineVideoBitrateInKbps
        {
            get
            {
                return m_offlineVideoBitrateInKbps;
            }
            set
            {
                m_offlineVideoBitrateInKbps = value;
            }
        }

        /// <summary>
        /// Replaces back slashes in a Uri with more net friendly forward slashes
        /// </summary>
        /// <param name="uriToFix"></param>
        /// <returns></returns>
        public static Uri FixupBackSlashesInAbsoluteUri(Uri uriToFix)
        {
            // Only needed for absolute Uri's 
            if (uriToFix.IsAbsoluteUri)
            {
                string fixedString = uriToFix.OriginalString.Replace("%5C", "/");
                fixedString = fixedString.Replace("%5c", "/");
                fixedString = fixedString.Replace("\\", "/");
                return new Uri(fixedString, UriKind.Absolute);
            }
            return uriToFix;
        }

        /// <summary>
        /// Gets or sets the Url of the media item.
        /// </summary>
        [Description("uri of item")]
        public Uri MediaSource
        {
            get
            {
                return m_mediaUri;
            }

            set
            {
                m_mediaUri = FixupBackSlashesInAbsoluteUri(value);
                OnPropertyChanged("MediaSource");
            }
        }
        /// <summary>
        /// Gets or sets the list of closed caption sources
        /// </summary>
        [Description("list of closed caption source files")]
        public ScriptableObservableCollection<CaptionSource> CaptionSources
        {
            get
            {
                return this.m_captionSources;
            }

            set
            {
                this.m_captionSources = value;
                OnPropertyChanged("CaptionSources");
            }
        }
        /// <summary>
        /// Gets or sets the source of the thumbnail for this item. 
        /// Uses a string instead of a URI to facilitate binding and handling cases where 
        /// the thumbnail file is missing without generating a page error.
        /// </summary>
        [Description("optional thumbnail for gallery and poster frame")]
        public Uri ThumbSource
        {
            get
            {
                return m_thumbSource;
            }

            set
            {
                m_thumbSource = value;
                OnPropertyChanged("ThumbSource");
            }
        }
        /// <summary>
        /// Gets or sets the title of the playlist item.
        /// </summary>
        [Description("title of item")]
        public String Title
        {
            get
            {
                return m_title;
            }

            set
            {
                m_title = value;
                OnPropertyChanged("Title");                
            }
        }
        /// <summary>
        /// Gets the chapters in this item.
        /// </summary>
        [Description("list of chapters")]
        public ScriptableObservableCollection<ChapterItem> Chapters
        {
            get { return m_chapters; }
            set { m_chapters = value; }
        }
        /// <summary>
        /// Gets or sets the frame rate of this item.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SmpteFrameRate SmpteFrameRate
        {
            get
            {
                return m_frameRate;
            }
            set
            {
                m_frameRate = value;
                m_frameRateFPS = 1 / TimeCode.FromFrames(1, value).TotalSeconds;
                OnPropertyChanged("FrameRate");
            }
        }
        /// <summary>
        /// chapters collection changed. means this playlist item changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chapters_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Chapters");
        }
        /// <summary>
        /// accessibility text, will be read out by screen reader if your listbox item has a control 
        /// AutomationProperties.Name bound to this
        /// </summary>
        string IAccessible.AccessibilityText
        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, ExpressionMediaPlayer.Resources.readerPlaylistItem, Title, Description);
            }
        }
        #region INotifyPropertyChanged Members
        /// <summary>
        /// Implements INotifyPropertyChanged.OnPropertyChanged().
        /// </summary>
        /// <param name="memberName">The name of the property that changed.</param>
        protected void OnPropertyChanged(string memberName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(memberName));
            }
        }
        #endregion
        #region Serialization
        /// <summary>
        /// top level XML node for this class & chapters
        /// </summary>
        internal const string xmlNode = "PlaylistItem";
        internal const string xmlChaptersNode = "Chapters";
        internal const string xmlCaptionSourcesNode = "CaptionSources";
        /// <summary>
        /// deserialise chapters
        /// </summary>
        /// <param name="reader">XmlReader to deserialize from</param>
        /// <returns>this</returns>
        private void DeserializeChapters(XmlReader reader)
        {
            if (!reader.IsStartElement(PlaylistItem.xmlChaptersNode))
                throw new InvalidPlaylistException();

            reader.Read();
            while (!(reader.Name == PlaylistItem.xmlChaptersNode && reader.NodeType == XmlNodeType.EndElement))
            {
                if (reader.IsStartElement("ChapterItem"))
                    this.Chapters.Add(new ChapterItem().Deserialize(reader));
                else if (reader.IsStartElement())
                    throw new InvalidPlaylistException(reader.Name, PlaylistItem.xmlChaptersNode);
                else if (!reader.Read())
                    break;
            }            
        }
        /// <summary>
        /// deserialise caption Sources
        /// </summary>
        /// <param name="reader">XmlReader to deserialize from</param>
        /// <returns>this</returns>
        private void DeserializeCaptionSources(XmlReader reader)
        {
            if (!reader.IsStartElement(PlaylistItem.xmlCaptionSourcesNode))
                throw new InvalidPlaylistException();

            reader.Read();
            while (!(reader.Name == PlaylistItem.xmlCaptionSourcesNode && reader.NodeType == XmlNodeType.EndElement))
            {
                if (reader.IsStartElement(CaptionSource.xmlNode))
                    this.CaptionSources.Add(new CaptionSource().Deserialize(reader));
                else if (reader.IsStartElement())
                    throw new InvalidPlaylistException(reader.Name, PlaylistItem.xmlCaptionSourcesNode);
                else if (!reader.Read())
                    break;
            }
        }
        /// <summary>
        /// deserialise this object
        /// </summary>
        /// <param name="reader">XmlReader to deserialize from</param>
        /// <returns>this</returns>
        internal PlaylistItem Deserialize(XmlReader reader)
        {
            if (!reader.IsStartElement(PlaylistItem.xmlNode))
                throw new InvalidPlaylistException();

            Init();
            reader.Read();
            while (!(reader.Name == PlaylistItem.xmlNode && reader.NodeType == XmlNodeType.EndElement))
            {
                if (reader.IsStartElement("Description"))
                    this.Description = reader.ReadElementContentAsString();
                else if (reader.IsStartElement("FileSize"))
                    this.FileSize = reader.ReadElementContentAsLong();
                else if (reader.IsStartElement("FrameRate"))
                    this.FrameRate = reader.ReadElementContentAsDouble();
                else if (reader.IsStartElement("Height"))
                    this.VideoHeight = reader.ReadElementContentAsDouble();
                else if (reader.IsStartElement("IsAdaptiveStreaming"))
                    this.IsAdaptiveStreaming = reader.ReadElementContentAsBoolean();
				else if (reader.IsStartElement("OfflineVideoBitrateInKbps"))
                    this.OfflineVideoBitrateInKbps = reader.ReadElementContentAsLong();
                else if (reader.IsStartElement("MediaSource"))
                {
                    string rawMediaSourceUrl = reader.ReadElementContentAsString();
                    this.MediaSource = new Uri(rawMediaSourceUrl, UriKind.RelativeOrAbsolute);
                }
                else if (reader.IsStartElement("ThumbSource"))
                {
                    string rawThumbSourceUrl = reader.ReadElementContentAsString();
                    this.ThumbSource = new Uri(rawThumbSourceUrl, UriKind.RelativeOrAbsolute);
                }
                else if (reader.IsStartElement("Title"))
                    this.Title = reader.ReadElementContentAsString();
                else if (reader.IsStartElement("Width"))
                    this.VideoWidth = reader.ReadElementContentAsDouble();
                else if (reader.IsStartElement(PlaylistItem.xmlChaptersNode))
                    DeserializeChapters(reader);
                else if (reader.IsStartElement(PlaylistItem.xmlCaptionSourcesNode))
                    DeserializeCaptionSources(reader);
                else if (reader.IsStartElement("AudioCodec"))
                    reader.ReadElementContentAsObject(); // ignored
                else if (reader.IsStartElement("VideoCodec"))
                    reader.ReadElementContentAsObject(); // ignored
                else if (reader.IsStartElement("AspectRatioWidth"))
                    this.AspectRatioWidth = reader.ReadElementContentAsDouble();
                else if (reader.IsStartElement("AspectRatioHeight"))
                    this.AspectRatioHeight = reader.ReadElementContentAsDouble();
                else if (reader.IsStartElement("DRM"))
                    this.DRM = reader.ReadElementContentAsBoolean();
                else if (reader.IsStartElement())
                    throw new InvalidPlaylistException(reader.Name, PlaylistItem.xmlNode);                        
                else if (!reader.Read())
                    break;
            }
            return this;
        }
        /// <summary>
        /// serialize this object
        /// </summary>
        /// <param name="writer">XmlWriter to serialze to</param>
        internal void Serialize(XmlWriter writer)
        {
            writer.WriteStartElement(PlaylistItem.xmlNode);
            writer.WriteElementString("Description", this.Description);
            writer.WriteElementString("FileSize", this.FileSize.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("FrameRate", this.FrameRate.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("Height", this.VideoHeight.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("IsAdaptiveStreaming", this.IsAdaptiveStreaming.ToString().ToLower(CultureInfo.InvariantCulture));
			writer.WriteElementString("OfflineVideoBitrateInKbps", this.OfflineVideoBitrateInKbps.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("MediaSource", this.MediaSource.ToString());
            if (this.CaptionSources != null)
            {
                if (CaptionSources.Count > 0)
                {
                    writer.WriteStartElement(PlaylistItem.xmlCaptionSourcesNode);
                    foreach (var item in CaptionSources)
                    {
                        item.Serialize(writer);
                    }
                    writer.WriteEndElement();
                }
            }
            if (this.ThumbSource != null)
            {
                writer.WriteElementString("ThumbSource", this.ThumbSource.ToString());
            }

            writer.WriteElementString("Title", this.Title);
            writer.WriteElementString("Width", this.VideoWidth.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("AspectRatioWidth", this.AspectRatioWidth.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("AspectRatioHeight", this.AspectRatioHeight.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("DRM", this.DRM.ToString().ToLower(CultureInfo.InvariantCulture));

            if ((this.Chapters != null) && (this.Chapters.Count > 0))
            {
                writer.WriteStartElement(PlaylistItem.xmlChaptersNode);
                foreach (var item in Chapters)
                {
                    item.Serialize(writer);
                }
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
        #endregion
    }
}
