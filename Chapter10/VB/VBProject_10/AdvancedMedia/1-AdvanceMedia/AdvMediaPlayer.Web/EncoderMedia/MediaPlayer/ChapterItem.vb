'  <copyright file="ChapterItem.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the ChapterItem class</summary>
'  <author>Microsoft Expression Encoder Team</author>
Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Windows.Browser
Imports System.Xml

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' This class describes a Chapter point in the media stream. Chapter points can
    ''' contain a title and a thumbnail among other things.
    ''' </summary>
    <ScriptableType>
    Public Class ChapterItem
        Inherits INotifyPropertyChanged
        Implements IAccessible
        ''' <summary>
        ''' top level XML node for this class
        ''' </summary>
        internal const string xmlNode = "ChapterItem"
        ''' <summary>
        ''' The position of the chapter item.
        ''' </summary>
        Private m_position As Double
        ''' <summary>
        ''' The source of the thumbnail for this chapter item.
        ''' </summary>
        Private m_thumbSource As Uri
        ''' <summary>
        ''' The title of this chapter item.
        ''' </summary>
        Private m_title As String
        ''' <summary>
        ''' Event which fires whenever a property changes on this chapter item.
        ''' </summary>
        public event PropertyChangedEventHandler PropertyChanged
        ''' <summary>
        ''' initialize chapter item
        ''' </summary>
        private Sub Init()

            m_title = string.Empty
            m_thumbSource = Nothing
        End Sub '   Init
        ''' <summary>
        ''' Initializes a new instance of the ChapterItem class.
        ''' </summary>
        Public Sub New()
            Init()
        End Sub '   New
        ''' <summary>
        ''' Gets or sets the position in the media stream of this chapter item.
        ''' </summary>
        <Description("chapter position in seconds")>
        Public Property Position() As Double
            Get

                return m_position
            End Get

            Set

                m_position = value
                OnPropertyChanged("Position")
                OnPropertyChanged("PositionText")
            End Set
        End Property
        ''' <summary>
        ''' Gets or sets the source of the thumbnail for this chapter item.
        ''' </summary>
        <Description("path to chapter thumbnail if required")>
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
        ''' Gets or sets the title of this chapter item.
        ''' </summary>
        <Description("title text of chapter")>
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
        ''' Gets the position of this chapter item as a string.
        ''' </summary>
        public virtual string PositionText
        {
            get
            {
                return TimeCode.ConvertToString(m_position, SmpteFrameRate.Unknown)
            }
        }
        ''' <summary>
        ''' accessibility text, will be read out by screen reader if your listbox item has a control
        ''' AutomationProperties.Name bound to this
        ''' </summary>
        Dim IAccessible.AccessibilityText As String

        {
            get
            {
                return String.Format(CultureInfo.InvariantCulture, ExpressionMediaPlayer.Resources.readerChapterItem, New TimeSpan((long)Position * 10000000).ReadableTime(), Title)
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
        ''' <summary>
        ''' deserialise this object
        ''' </summary>
        ''' <param name="reader">XmlReader to deserialize from</param>
        ''' <returns>this</returns>
        internal ChapterItem Deserialize(XmlReader reader)
        {

            If ( Not reader.IsStartElement(ChapterItem.xmlNode)) Then
                throw New InvalidPlaylistException()
            End If

            reader.Read()
              ChapterItem.xmlNode  AndAlso  reader.NodeType = XmlNodeType.EndElement))
              While ( Not (reader.Name = XmlNodeType.EndElement))


                If (reader.IsStartElement("Position")) Then
                    Me.Position = reader.ReadElementContentAsDouble()
                else if (reader.IsStartElement("ThumbSource"))
                    Me.ThumbSource = New Uri(reader.ReadElementContentAsString(), UriKind.RelativeOrAbsolute)
                End If
                else if (reader.IsStartElement("Title"))
                    Me.Title = reader.ReadElementContentAsString()
                else if (reader.IsStartElement())
                    throw New InvalidPlaylistException(reader.Name, ChapterItem.xmlNode)
                else if ( Not reader.Read())
            End While   '

            return this
        }
        ''' <summary>
        ''' serialize this object
        ''' </summary>
        ''' <param name="writer">XmlWriter to serialze to</param>
        internal Sub Serialize(writer As XmlWriter)

            writer.WriteStartElement(ChapterItem.xmlNode)
            writer.WriteElementString("Position", Me.Position.ToString(CultureInfo.InvariantCulture))

            If (Me.ThumbSource  IsNot Nothing) Then
                writer.WriteElementString("ThumbSource", Me.ThumbSource.ToString())
            End If

            writer.WriteElementString("Title", Me.Title)
            writer.WriteEndElement()
        End Sub '   Serialize
    End Class   '   ChapterItem
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\ChapterItem.cs
