'  <copyright file="CaptionSource.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the CaptionSource class</summary>
'  <author>Microsoft Expression Encoder Team</author>
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Globalization
Imports System.Windows.Browser
Imports System.Xml

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' This class describes a Caption source file
    ''' </summary>
    <ScriptableType>
    Public Class CaptionSource
        Inherits INotifyPropertyChanged
        ''' <summary>
        ''' top level XML node for this class
        ''' </summary>
        internal const string xmlNode = "CaptionSource"
        ''' <summary>
        ''' The source of the thumbnail for this chapter item.
        ''' </summary>
        Private m_captionFileSource As Uri
        ''' <summary>
        ''' The friendly language name of this caption file
        ''' </summary>
        Private m_language As String
        ''' <summary>
        ''' The language Id of this caption file
        ''' </summary>
        Private m_languageId As String
        ''' <summary>
        ''' type of caption
        ''' </summary>
        Private m_type As String
        ''' <summary>
        ''' Event which fires whenever a property changes on this chapter item.
        ''' </summary>
        public event PropertyChangedEventHandler PropertyChanged
        ''' <summary>
        ''' initialize chapter item
        ''' </summary>
        private Sub Init()
        End Sub '   Init
        ''' <summary>
        ''' Initializes a new instance of the ChapterItem class.
        ''' </summary>
        Public Sub New()
            Init()
        End Sub '   New
        ''' <summary>
        ''' Gets or sets the source of the caption file
        ''' </summary>
        <Description("path to caption file")>
        Public Property CaptionFileSource() As Uri
            Get

                return m_captionFileSource
            End Get

            Set

                m_captionFileSource = value
                OnPropertyChanged("CaptionFileSource")
            End Set
        End Property
        ''' <summary>
        ''' Gets or sets the language of this caption file
        ''' </summary>
        <Description("language of this caption file")>
        Public Property Language() As String
            Get

                return m_language
            End Get

            Set

                m_language = value
                OnPropertyChanged("Language")
            End Set
        End Property
        ''' <summary>
        ''' Gets or sets the language of this caption file
        ''' </summary>
        <Description("language id of this caption file")>
        Public Property LanguageId() As String
            Get

                return m_languageId
            End Get

            Set

                m_languageId = value.Trim()

                If (m_languageId.Length = 3) Then
                    Me.ISOTwoLetterLanguageName = LanguageAlias.IsoThreeLetterToIsoTwoLetter(m_languageId)
                ElseIf (m_languageId.Length = 2) Then
                    Me.ISOTwoLetterLanguageName = m_languageId
                End If

                else
                {
                    Debug.Assert(false, "funky language name encountered" + m_languageId)
                    Me.ISOTwoLetterLanguageName = string.Empty
                }
                OnPropertyChanged("LanguageId")
            End Set
        End Property

        Public Property ISOTwoLetterLanguageName() As String
            Get

                return m_type
            End Get

            Set

                m_type = value
                OnPropertyChanged("Type")
            End Set
        End Property
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
        ''' <summary>
        ''' deserialise this object
        ''' </summary>
        ''' <param name="reader">XmlReader to deserialize from</param>
        ''' <returns>this</returns>
        internal CaptionSource Deserialize(XmlReader reader)
        {

            If ( Not reader.IsStartElement(CaptionSource.xmlNode)) Then
                throw New InvalidPlaylistException()
            End If
            Me.CaptionFileSource = New Uri(reader.GetAttribute("Location"), UriKind.RelativeOrAbsolute)
            Me.Language = reader.GetAttribute("Language")
            Me.LanguageId = reader.GetAttribute("LanguageId")
            Me.Type = reader.GetAttribute("Type")

            reader.Read()

            return this
        }
        ''' <summary>
        ''' serialize this object
        ''' </summary>
        ''' <param name="writer">XmlWriter to serialze to</param>
        internal Sub Serialize(writer As XmlWriter)

            writer.WriteStartElement(CaptionSource.xmlNode)
            writer.WriteAttributeString("Location", Me.CaptionFileSource.ToString())
            writer.WriteAttributeString("Language", Me.Language)
            writer.WriteAttributeString("LanguageId", Me.LanguageId)
            writer.WriteAttributeString("Type", Me.Type)
            writer.WriteEndElement()
        End Sub '   Serialize
    End Class   '   CaptionSource
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\CaptionSource.cs
