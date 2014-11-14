// <copyright file="PlaylistCollection.cs" company="Microsoft">
//     Copyright © Microsoft Corporation. All rights reserved.
// </copyright>
// <summary>Implements the PlaylistCollection class</summary>
// <author>Microsoft Expression Encoder Team</author>
using System.Windows.Browser;
using System.Xml;
using System.Windows;
using System.Collections.Specialized;

namespace ExpressionMediaPlayer
{
    /// <summary>
    /// This class represents a collection of media items to play.
    /// </summary>    
    [ScriptableType]    
    public class PlaylistCollection : ScriptableObservableCollection<PlaylistItem>
    {
        /// <summary>
        /// Inserts an item into the collection.
        /// </summary>
        /// <param name="index">Index to insert the item at.</param>
        /// <param name="item">Item to insert.</param>
        protected override void InsertItem(int index, PlaylistItem item)
        {
            if (item != null)
            {
                item.OwnerCollection = this;
                item.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(item_PropertyChanged);
                base.InsertItem(index, item);
            }
        }
        /// <summary>
        /// playlist item changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {            
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        /// <summary>
        /// Removes an item at the given index.
        /// </summary>
        /// <param name="index">Index of the item to remove.</param>
        protected override void RemoveItem(int index)
        {
            Items[index].OwnerCollection = null;
            base.RemoveItem(index);
        }
        #region Serialization
        /// <summary>
        /// top level XML node for this class
        /// </summary>
        internal const string xmlNode = "Items";        
        /// <summary>
        /// deserialise this object
        /// </summary>
        /// <param name="reader">XmlReader to deserialize from</param>
        /// <returns>this</returns>
        internal PlaylistCollection Deserialize(XmlReader reader)
        {
            if (!reader.IsStartElement(PlaylistCollection.xmlNode))
                throw new InvalidPlaylistException();

            reader.Read();
            while (!(reader.Name == PlaylistCollection.xmlNode && reader.NodeType == XmlNodeType.EndElement))
            {
                if (reader.IsStartElement("PlaylistItem"))
                    this.Add(new PlaylistItem().Deserialize(reader));
                else if (reader.IsStartElement())
                    throw new InvalidPlaylistException(reader.Name, PlaylistCollection.xmlNode);
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
            writer.WriteStartElement(PlaylistCollection.xmlNode);
            foreach (var item in this)
            {
                item.Serialize(writer);
            }
            writer.WriteEndElement();
        }
        #endregion
    }
}
