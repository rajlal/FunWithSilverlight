// <copyright file="PlaylistCollection.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>Implements the PlaylistCollection class</summary>
// <author>Microsoft Expression Encoder Team</author>
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Media;
using System.Threading;
using System.Xml;
using System.IO.IsolatedStorage;
using System.IO;
using System.Globalization;
using System.Text;

namespace ExpressionMediaPlayer
{
    /// <summary>
    /// exception for invalid playlist
    /// </summary>
    public class InvalidPlaylistException : Exception
    {
        public InvalidPlaylistException() 
            : base(Resources.errorInvalidPlaylist)
        {
        }
        public InvalidPlaylistException(string message)
            : base(message)
        {
        }
        public InvalidPlaylistException(string nodeFound, string nodeExpected)
            : base(String.Format(CultureInfo.CurrentUICulture, Resources.errorInvalidPlaylistNode, nodeFound, nodeExpected))
        {
        }
    }

    // Summary:
    //     Describes how content is resized to fill its allocated space -- taking into account non-square pixels
    public enum StretchNonSquarePixels
    {
        // Summary:
        //     The content preserves its original size and frame aspect ratio
        NoStretch = 0,
        //
        // Summary:
        //     The content is resized to fill the destination dimensions. The frame aspect ratio is preserved.
        StretchToFill = 1,
        //
        // Summary:
        //     The content is resized to fill the destination dimensions. Original size and frame aspect ratio are ignore.
        StretchDistorted = 2,
    }

    [ScriptableType]
    public partial class Playlist : DependencyObject
    {
        #region PlaylistCore
        /// <summary>
        /// playlist options
        /// </summary>
        private bool m_autoLoad;
        private bool m_autoPlay;
        private bool m_autoRepeat;
        private bool m_displayTimeCode;
        private bool m_enableCachedComposition;
        private bool m_enableCaptions;
        private bool m_enableOffline;
        private bool m_enablePopOut;
        private bool m_startMuted;
        private bool m_startWithPlaylistShowing;
        private StretchNonSquarePixels m_stretchMode;
        private Color m_background;

        /// <summary>
        /// The number of items in the playlist that are DRM protected
        /// </summary>
        private int m_countOfDRMItems;
        /// <summary>
        /// The number of items in the playlist that are Live Items;
        /// </summary>
        private int m_countOfLiveItems;
        /// <summary>
        /// The number of items in the playlist that are adaptive Items;
        /// </summary>
        private int m_countOfAdaptiveItems;

        /// <summary>
        /// list of playlist items
        /// </summary>        
        private PlaylistCollection _Items = new PlaylistCollection();
        /// <summary>
        /// playlist changed event
        /// </summary>
        [ScriptableMember]
        public event RoutedEventHandler PlaylistChanged;
        /// <summary>
        /// should playlist cue up 
        /// </summary>
        [Description("Automatically cue video when page is loaded"), DefaultValue(true)]
        public bool AutoLoad 
        { 
            get
            {
                return m_autoLoad;
            }
            set
            {
                m_autoLoad = value;
                if (PlaylistChanged != null)
                {
                    PlaylistChanged(this, null);
                }
            }
        }
        /// <summary>
        /// should playlist auto start?
        /// </summary>
        [Description("Automatically start video when cued"), DefaultValue(true)]
        public bool AutoPlay
        {
            get
            {
                return m_autoPlay;
            }
            set
            {
                m_autoPlay = value;
                if (PlaylistChanged != null)
                {
                    PlaylistChanged(this, null);
                }
            }
        }
        /// <summary>
        /// should playlist auto repeat?
        /// </summary>
        [Description("Automatically restart video when at end"), DefaultValue(false)]
        public bool AutoRepeat
        {
            get
            {
                return m_autoRepeat;
            }
            set
            {
                m_autoRepeat = value;
                if (PlaylistChanged != null)
                {
                    PlaylistChanged(this, null);
                }
            }
        }
        /// <summary>
        /// display timecodes
        /// </summary>
        [Description("Display Timecode")]
        public bool DisplayTimeCode
        {
            get
            {
                return m_displayTimeCode;
            }
            set
            {
                m_displayTimeCode = value;
                if (PlaylistChanged != null)
                {
                    PlaylistChanged(this, null);
                }
            }
        }
        /// <summary>
        /// cached composition enabled
        /// </summary>
        [Description("Enable Cached Composition"), DefaultValue(true)]
        public bool EnableCachedComposition
        {
            get
            {
                return m_enableCachedComposition;
            }
            set
            {
                m_enableCachedComposition = value;
                if (PlaylistChanged != null)
                {
                    PlaylistChanged(this, null);
                }
            }
        }
        /// <summary>
        /// captions enabled
        /// </summary>
        [Description("Allow closed captions to show"), DefaultValue(true)]
        public bool EnableCaptions
        {
            get
            {
                return m_enableCaptions;
            }
            set
            {
                m_enableCaptions = value;
                if (PlaylistChanged != null)
                {
                    PlaylistChanged(this, null);
                }
            }
        }
        /// <summary>
        /// offlining enabled
        /// </summary>
        [Description("Enable Player to be run offline"), DefaultValue(true)]
        public bool EnableOffline
        {
            get
            {
                return m_enableOffline && (m_countOfDRMItems < 1) && (m_countOfLiveItems < 1);
            }
            set
            {
                m_enableOffline = value;
                if (PlaylistChanged != null)
                {
                    PlaylistChanged(this, null);
                }
            }
        }
        /// <summary>
        /// popout enabled
        /// </summary>
        [Description("Enable Player popout"), DefaultValue(true)]
        public bool EnablePopOut
        {
            get
            {
                return m_enablePopOut;
            }
            set
            {
                m_enablePopOut = value;
                if (PlaylistChanged != null)
                {
                    PlaylistChanged(this, null);
                }
            }
        }

        /// <summary>
        /// HasAdaptiveItems
        /// </summary>
        [Description("Indicated whether there are any Smooth Streaming items in the playlist")]
        public bool HasAdaptiveItems
        {
            get
            {
                return m_countOfAdaptiveItems > 0;
            }
        }


        /// <summary>
        /// start in muted state
        /// </summary>
        [Description("Mute player on start")]
        public bool StartMuted
        {
            get
            {
                return m_startMuted;
            }
            set
            {
                m_startMuted = value;
                if (PlaylistChanged != null)
                {
                    PlaylistChanged(this, null);
                }
            }
        }

        /// <summary>
        /// start with playlist visible
        /// </summary>
        [Description("Show playlist on start")]
        public bool StartWithPlaylistShowing
        {
            get
            {
                return m_startWithPlaylistShowing;
            }
            set
            {
                m_startWithPlaylistShowing = value;
                if (PlaylistChanged != null)
                {
                    PlaylistChanged(this, null);
                }
            }
        }
        /// <summary>
        /// type of video stretch
        /// </summary>
        [Description("Stretch Mode")]
        public StretchNonSquarePixels StretchNonSquarePixels
        {
            get
            {
                return m_stretchMode;
            }
            set
            {
                m_stretchMode = value;
                if (PlaylistChanged != null)
                {
                    PlaylistChanged(this, null);
                }
            }
        }
        /// <summary>
        /// color of background
        /// </summary>
        public Color Background
        {
            get
            {
                return m_background;
            }
            set
            {
                m_background = value;
                if (PlaylistChanged != null)
                {
                    PlaylistChanged(this, null);
                }
            }
        }
        /// <summary>
        /// list of playlist items.
        /// </summary>
        [Description("Playlist")]
        public PlaylistCollection Items
        {
            get
            {
                return _Items;
            }
            set
            {
                _Items = value;
            }
        }
        /// <summary>
        /// init structure
        /// </summary>
        private void Init()
        {
            AutoLoad = true;
            AutoPlay = true;
            AutoRepeat = false;
            EnableCachedComposition = true;
            EnableCaptions = true;
            EnablePopOut = true;
            EnableOffline = true;
            m_countOfDRMItems = 1;
            m_countOfLiveItems = 1;
            m_countOfAdaptiveItems = 0;
        }
        /// <summary>
        /// construct a playlistitem, provided for scripting.
        /// </summary>
        /// <returns>new playlist item</returns> 
        public PlaylistItem CreateNewPlaylistItem()
        {
            return new PlaylistItem(Items);
        }
        /// <summary>
        /// playlist constructor
        /// </summary>
        public Playlist()
        {
            this.StretchNonSquarePixels = StretchNonSquarePixels.NoStretch;
            _Items = new PlaylistCollection();
            _Items.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Items_CollectionChanged);
            Init();         
        }
        /// <summary>
        /// inform playlist changed if the items collection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (PlaylistChanged!=null)
            {
                PlaylistChanged(this, null);
            }
            m_countOfDRMItems = 0;
            m_countOfLiveItems = 0;
            m_countOfAdaptiveItems = 0;
            foreach (var item in this.Items)
            {
                if (item.DRM)
                {
                    m_countOfDRMItems++;
                }
                if (item.IsAdaptiveStreaming)
                {
                    m_countOfAdaptiveItems++;
                    string source = item.MediaSource.OriginalString.ToLower();
                    if (source.Contains(@".isml/manifest"))
                    {
                        m_countOfLiveItems++;
                    }
                }
                else
                {
                    if (item.MediaSource.IsAbsoluteUri)
                    {
                        string scheme = item.MediaSource.Scheme.ToLower();
                        if (scheme == ("mms:"))
                        {
                            m_countOfLiveItems++;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// create playlist from XML string
        /// </summary>
        /// <param name="playlistXml">XML string</param>
        /// <returns>playlist</returns>
        public static Playlist Create(string playlistXml)
        {
            Playlist playlist = new Playlist();
            playlist.ParseXml(playlistXml);
            return playlist;
        }
        /// <summary>
        /// parse playlist from XML
        /// </summary>
        /// <param name="playlistXml"></param>
        public void ParseXml(string playlistXml)
        {
            UTF8Encoding enc = new UTF8Encoding();
            using (MemoryStream ms = new MemoryStream(enc.GetBytes(playlistXml)))
            {
                XmlReaderSettings xmlrs = new XmlReaderSettings();
                xmlrs.IgnoreComments = true;
                xmlrs.IgnoreWhitespace = true;
                XmlReader reader = XmlReader.Create(ms, xmlrs);
                Deserialize(reader);
            }
            if (PlaylistChanged != null)
            {
                PlaylistChanged(this, null);
            }
        }
        #endregion

        #region Serialization
        /// <summary>
        /// top level XML node for this class
        /// </summary>
        internal const string xmlNode = "Playlist";
        /// <summary>
        /// deserialise this object
        /// </summary>
        /// <param name="reader">XmlReader to deserialize from</param>
        /// <returns>this</returns>
        internal void Deserialize(XmlReader reader)
        {
            if (!reader.IsStartElement(Playlist.xmlNode))
                throw new InvalidPlaylistException();

            Init();
            reader.Read();
            while (!(reader.Name == Playlist.xmlNode && reader.NodeType == XmlNodeType.EndElement))
            {
                if (reader.IsStartElement("AutoLoad"))
                    this.AutoLoad = reader.ReadElementContentAsBoolean();
                else if (reader.IsStartElement("AutoPlay"))
                    this.AutoPlay = reader.ReadElementContentAsBoolean();
                else if (reader.IsStartElement("AutoRepeat"))
                    this.AutoRepeat = reader.ReadElementContentAsBoolean();
                else if (reader.IsStartElement("DisplayTimeCode"))
                    this.DisplayTimeCode = reader.ReadElementContentAsBoolean();
                else if (reader.IsStartElement("EnableCachedComposition"))
                    this.EnableCachedComposition = reader.ReadElementContentAsBoolean();
                else if (reader.IsStartElement("EnableCaptions"))
                    this.EnableCaptions = reader.ReadElementContentAsBoolean();
                else if (reader.IsStartElement("EnablePopOut"))
                    this.EnablePopOut = reader.ReadElementContentAsBoolean();
                else if (reader.IsStartElement("EnableOffline"))
                    this.EnableOffline = reader.ReadElementContentAsBoolean();
                else if (reader.IsStartElement("StartMuted"))
                    this.StartMuted = reader.ReadElementContentAsBoolean();
                else if (reader.IsStartElement("StartWithPlaylistShowing"))
                {
                    this.StartWithPlaylistShowing = reader.ReadElementContentAsBoolean();
                }
                else if (reader.IsStartElement("StretchMode"))
                {
                    Stretch tmp = (Stretch)Enum.Parse(typeof(Stretch), reader.ReadElementContentAsString(), false);
                    switch(tmp)
                    {
                        case Stretch.None:
                            this.StretchNonSquarePixels = StretchNonSquarePixels.NoStretch;
                            break;
                        case Stretch.Uniform:
                        case Stretch.UniformToFill:
                            this.StretchNonSquarePixels = StretchNonSquarePixels.StretchToFill;
                            break;
                        case Stretch.Fill:
                            this.StretchNonSquarePixels = StretchNonSquarePixels.StretchDistorted;
                            break;
                    }
                }
                else if (reader.IsStartElement("StretchNonSquarePixels"))
                {
                    StretchNonSquarePixels tmp = (StretchNonSquarePixels)Enum.Parse(typeof(StretchNonSquarePixels), reader.ReadElementContentAsString(), false);
                    this.StretchNonSquarePixels = tmp;
                }
                else if (reader.IsStartElement(PlaylistCollection.xmlNode))
                {
                    this.Items.Clear();
                    this.Items.Deserialize(reader);
                }
                else if (reader.IsStartElement())
                    throw new InvalidPlaylistException(reader.Name, Playlist.xmlNode);
                else if (!reader.Read())
                    break;
            }
        }
        /// <summary>
        /// serialize this object
        /// </summary>
        /// <param name="writer">XmlWriter</param>
        internal void Serialize(XmlWriter writer)
        {
            writer.WriteStartElement(Playlist.xmlNode);
            writer.WriteElementString("AutoLoad", this.AutoLoad.ToString().ToLower(CultureInfo.InvariantCulture));
            writer.WriteElementString("AutoPlay", this.AutoPlay.ToString().ToLower(CultureInfo.InvariantCulture));
            writer.WriteElementString("AutoRepeat", this.AutoRepeat.ToString().ToLower(CultureInfo.InvariantCulture));
            writer.WriteElementString("DisplayTimeCode", this.DisplayTimeCode.ToString().ToLower(CultureInfo.InvariantCulture));
            writer.WriteElementString("EnableCachedComposition", this.EnableCachedComposition.ToString().ToLower(CultureInfo.InvariantCulture));
            writer.WriteElementString("EnableCaptions", this.EnableCaptions.ToString().ToLower(CultureInfo.InvariantCulture));
            writer.WriteElementString("EnablePopOut", this.EnablePopOut.ToString().ToLower(CultureInfo.InvariantCulture));
            writer.WriteElementString("EnableOffline", this.EnableOffline.ToString().ToLower(CultureInfo.InvariantCulture));
            writer.WriteElementString("StartMuted", this.StartMuted.ToString().ToLower(CultureInfo.InvariantCulture));
            writer.WriteElementString("StretchNonSquarePixels", this.StretchNonSquarePixels.ToString());
            _Items.Serialize(writer);
            writer.WriteEndElement();
        }
        #endregion

    }
}

