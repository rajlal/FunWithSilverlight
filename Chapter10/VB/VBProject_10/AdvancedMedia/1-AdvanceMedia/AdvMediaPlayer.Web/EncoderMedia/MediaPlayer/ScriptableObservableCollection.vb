'  <copyright file="ScriptableObservableCollection.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements a Scriptable version of the ObservableCollection class</summary>
'  <author>Microsoft Expression Encoder Team</author>
Imports System.Collections.ObjectModel
Imports System.Windows.Browser

' Namespace ExpressionMediaPlayer

    Public Class ScriptableObservableCollection(Of T)
        Inherits ObservableCollection(Of T)
        ''' <summary>
        ''' observable collection callable through scripting
        ''' </summary>
        public ScriptableObservableCollection()
        {}
        ''' <summary>
        ''' add item
        ''' </summary>
        ''' <param name="item">itemt o add</param>
        <ScriptableMember>
        public New Sub Add(item As T)

            MyBase.Add(item)
        End Sub '   Add
        ''' <summary>
        ''' index item
        ''' </summary>
        ''' <param name="index">index</param>
        ''' <returns>item</returns>
        <ScriptableMember>
        public New T this(int index)
        {
            get { return base(index); }
            set { base(index) = value; }
        }
        ''' <summary>
        ''' number of items in collection
        ''' </summary>
        <ScriptableMember>
        public New int Count
        {
            get { return MyBase.Count; }
        }
        ''' <summary>
        ''' clear collection
        ''' </summary>
        <ScriptableMember>
        public New Sub Clear()

            MyBase.Clear()
        End Sub '   Clear
        ''' <summary>
        ''' does list contain this item
        ''' </summary>
        ''' <param name="item">item to find</param>
        ''' <returns>item found</returns>
        <ScriptableMember>
        public New bool Contains(T item)
        {
            return MyBase.Contains(item)
        }
        ''' <summary>
        ''' return index of this item
        ''' </summary>
        ''' <param name="item">item to find</param>
        ''' <returns>index of this item</returns>
        <ScriptableMember>
        public New int IndexOf(T item)
        {
            return MyBase.IndexOf(item)
        }
        ''' <summary>
        ''' insert item into collection
        ''' </summary>
        ''' <param name="index">index to insert at</param>
        ''' <param name="item">item to insert</param>
        <ScriptableMember>
        public New Sub Insert(index As Integer, item As T)

            MyBase.Insert(index, item)
        End Sub '   Insert
        ''' <summary>
        ''' remove item at this index
        ''' </summary>
        ''' <param name="index">index to remove at</param>
        <ScriptableMember>
        public New Sub RemoveAt(index As Integer)

            MyBase.RemoveAt(index)
        End Sub '   RemoveAt
        ''' <summary>
        ''' remove item from collection
        ''' </summary>
        ''' <param name="item">item to remove</param>
        ''' <returns>true if found and removed</returns>
        <ScriptableMember>
        public New bool Remove(T item)
        {
            return MyBase.Remove(item)
        }
    End Class   '   ScriptableObservableCollection<T>
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\ScriptableObservableCollection.cs
