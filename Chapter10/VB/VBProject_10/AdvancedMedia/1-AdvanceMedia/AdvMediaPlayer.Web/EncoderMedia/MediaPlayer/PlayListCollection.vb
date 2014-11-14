'  <copyright file="PlaylistCollection.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the PlaylistCollection class</summary>
'  <author>Microsoft Expression Encoder Team</author>
Imports System.Windows.Browser
Imports System.Xml
Imports System.Windows
Imports System.Collections.Specialized

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' This class represents a collection of media items to play.
    ''' </summary>
    <ScriptableType>
    Public Class PlaylistCollection
        Inherits ScriptableObservableCollection(Of PlaylistItem)
        ''' <summary>
        ''' Inserts an item into the collection.
        ''' </summary>
        ''' <param name="index">Index to insert the item at.</param>
        ''' <param name="item">Item to insert.</param>
        protected override Sub InsertItem(index As Integer, item As PlaylistItem)


            If (item  IsNot Nothing) Then
                item.OwnerCollection = this
                AddHandler item.PropertyChanged, AddressOf System.ComponentModel.PropertyChangedEventHandler(item_PropertyChanged)
                MyBase.InsertItem(index, item)
            End If
        End Sub '   InsertItem
        ''' <summary>
        ''' playlist item changed
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Sub item_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs)

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))
        End Sub '   item_PropertyChanged
        ''' <summary>
        ''' Removes an item at the given index.
        ''' </summary>
        ''' <param name="index">Index of the item to remove.</param>
        protected override Sub RemoveItem(index As Integer)

            Items(index).OwnerCollection = Nothing
            MyBase.RemoveItem(index)
        End Sub '   RemoveItem

        #region Serialization
        ''' <summary>
        ''' top level XML node for this class
        ''' </summary>
        internal const string xmlNode = "Items"
        ''' <summary>
        ''' deserialise this object
        ''' </summary>
        ''' <param name="reader">XmlReader to deserialize from</param>
        ''' <returns>this</returns>
        internal PlaylistCollection Deserialize(XmlReader reader)
        {

            If ( Not reader.IsStartElement(PlaylistCollection.xmlNode)) Then
                throw New InvalidPlaylistException()
            End If

            reader.Read()
              PlaylistCollection.xmlNode  AndAlso  reader.NodeType = XmlNodeType.EndElement))
              While ( Not (reader.Name = XmlNodeType.EndElement))


                If (reader.IsStartElement("PlaylistItem")) Then
                    Me.Add(new PlaylistItem().Deserialize(reader))
                else if (reader.IsStartElement())
                    throw New InvalidPlaylistException(reader.Name, PlaylistCollection.xmlNode)
                End If
                else if ( Not reader.Read())
            End While   '

            return this
        }
        ''' <summary>
        ''' serialize this object
        ''' </summary>
        ''' <param name="writer">XmlWriter to serialze to</param>
        internal Sub Serialize(writer As XmlWriter)

            writer.WriteStartElement(PlaylistCollection.xmlNode)

            For Each var item in this

                item.Serialize(writer)
            Next    '   var

            writer.WriteEndElement()
        End Sub '   Serialize

        #End Region
    End Class   '   PlaylistCollection
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\PlayListCollection.cs
