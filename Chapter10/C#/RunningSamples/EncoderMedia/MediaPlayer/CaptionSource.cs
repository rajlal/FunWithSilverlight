// <copyright file="CaptionSource.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>Implements the CaptionSource class</summary>
// <author>Microsoft Expression Encoder Team</author>
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Browser;
using System.Xml;

namespace ExpressionMediaPlayer
{
    /// <summary>
    /// This class describes a Caption source file
    /// </summary>
    [ScriptableType]
    public class CaptionSource : INotifyPropertyChanged
    {
        /// <summary>
        /// top level XML node for this class
        /// </summary>
        internal const string xmlNode = "CaptionSource";
        /// <summary>
        /// The source of the thumbnail for this chapter item.
        /// </summary>
        private Uri m_captionFileSource;
        /// <summary>
        /// The friendly language name of this caption file
        /// </summary>
        private string m_language;
        /// <summary>
        /// The language Id of this caption file
        /// </summary>
        private string m_languageId;
        /// <summary>
        /// type of caption
        /// </summary>
        private string m_type;
        /// <summary>
        /// Event which fires whenever a property changes on this chapter item.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// initialize chapter item
        /// </summary>
        private void Init()
        {                        
        }
        /// <summary>
        /// Initializes a new instance of the ChapterItem class.
        /// </summary>
        public CaptionSource()
        {
            Init();
        }       
        /// <summary>
        /// Gets or sets the source of the caption file
        /// </summary>
        [Description("path to caption file")]
        public Uri CaptionFileSource
        {
            get
            {
                return m_captionFileSource;
            }

            set
            {
                m_captionFileSource = value;
                OnPropertyChanged("CaptionFileSource");
            }
        }
        /// <summary>
        /// Gets or sets the language of this caption file
        /// </summary>
        [Description("language of this caption file")]
        public string Language
        {
            get
            {
                return m_language;
            }

            set
            {
                m_language = value;
                OnPropertyChanged("Language");
            }
        }
        /// <summary>
        /// Gets or sets the language of this caption file
        /// </summary>
        [Description("language id of this caption file")]
        public string LanguageId
        {
            get
            {
                return m_languageId;
            }

            set
            {
                m_languageId = value.Trim();
                if (m_languageId.Length == 3)
                {
                    this.ISOTwoLetterLanguageName = LanguageAlias.IsoThreeLetterToIsoTwoLetter(m_languageId);
                }
                else if (m_languageId.Length == 2)
                {
                    this.ISOTwoLetterLanguageName = m_languageId;
                }
                else
                {
                    Debug.Assert(false, "funky language name encountered" + m_languageId);
                    this.ISOTwoLetterLanguageName = string.Empty;
                }
                OnPropertyChanged("LanguageId");
            }
        }

        public string ISOTwoLetterLanguageName { get; private set; }

        /// <summary>
        /// Gets or sets the language of this caption file
        /// </summary>
        [Description("type of this caption file")]
        public string Type
        {
            get
            {
                return m_type;
            }

            set
            {
                m_type = value;
                OnPropertyChanged("Type");
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
        /// <summary>
        /// deserialise this object
        /// </summary>
        /// <param name="reader">XmlReader to deserialize from</param>
        /// <returns>this</returns>
        internal CaptionSource Deserialize(XmlReader reader)
        {
            if (!reader.IsStartElement(CaptionSource.xmlNode))
                throw new InvalidPlaylistException();
            this.CaptionFileSource = new Uri(reader.GetAttribute("Location"), UriKind.RelativeOrAbsolute);
            this.Language = reader.GetAttribute("Language");
            this.LanguageId = reader.GetAttribute("LanguageId");
            this.Type = reader.GetAttribute("Type");

            reader.Read();

            return this;
        }
        /// <summary>
        /// serialize this object
        /// </summary>
        /// <param name="writer">XmlWriter to serialze to</param>
        internal void Serialize(XmlWriter writer)
        {
            writer.WriteStartElement(CaptionSource.xmlNode);
            writer.WriteAttributeString("Location", this.CaptionFileSource.ToString());
            writer.WriteAttributeString("Language", this.Language);
            writer.WriteAttributeString("LanguageId", this.LanguageId);
            writer.WriteAttributeString("Type", this.Type);
            writer.WriteEndElement();
        }
    }
}
