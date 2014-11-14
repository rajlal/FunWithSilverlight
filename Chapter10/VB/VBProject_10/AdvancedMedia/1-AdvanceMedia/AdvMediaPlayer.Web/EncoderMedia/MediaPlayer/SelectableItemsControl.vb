'  <copyright file="SelectableItemsControl.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media

' Namespace MediaPlayerExtensions

    Public Class SelectableItemsControl
        Inherits ItemsControl
        ''' <summary>
        ''' currently selected item
        ''' </summary>
        Private m_SelectedItem As Object
        ''' <summary>
        ''' item hovering over
        ''' </summary>
        Private m_HoverItem As Object
        ''' <summary>
        ''' index of item over
        ''' </summary>
        Private m_HoverIndex As Integer
        ''' <summary>
        ''' dependancy property for currently selected item index
        ''' </summary>
        Public Property readonly() As
            Get
                return CType(GetValue(SelectedIndexProperty, Integer))
            End Get

            Set
                SetValue(SelectedIndexProperty, value)
            End Set
        End Property
        ''' <summary>
        ''' SelectedIndex dependancy property changed
        ''' </summary>
        End Property


            Dim list As SelectableItemsControl  = CType(obj,  SelectableItemsControl)


            If (list  IsNot Nothing) Then
                list.UpdateSelectedIndex()
            End If
        End Sub '   OnSelectedIndexChanged
        ''' <summary>
        ''' property wrapper for HoverIndex dependancy property
        ''' </summary>
        Public Property HoverIndex() As Integer
            Get
                return CType(GetValue(HoverIndexProperty, Integer))
            End Get

            Set
                SetValue(HoverIndexProperty, value)
            End Set
        End Property
        ''' <summary>
        ''' HoverIndex dependancy property changed
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <param name="args"></param>
        End Property


            Dim list As SelectableItemsControl  = CType(obj,  SelectableItemsControl)


            If (list  IsNot Nothing) Then
                list.UpdateHoverIndex()
            End If
        End Sub '   OnHoverIndexChanged
        ''' <summary>
        ''' sync the HoverItem from the current hovered over index
        ''' </summary>
        private Sub UpdateHoverIndex()

            Dim hoverIndex As Integer  = Math.Max(-1, Math.Min(HoverIndex, Items.Count - 1))


            If (hoverIndex = m_HoverIndex) Then
                return
            End If

            m_HoverIndex = hoverIndex

            If (hoverIndex = -1) Then
                HoverItem = Nothing
            else
                HoverItem = Items(m_HoverIndex)
            End If
        End Sub '   UpdateHoverIndex
        ''' <summary>
        ''' update SelectedItem from the currently selected item index
        ''' </summary>
        private Sub UpdateSelectedIndex()

            Dim selectedIndex As Integer  = Math.Max(-1, Math.Min(SelectedIndex, Items.Count - 1))


            If (selectedIndex = -1) Then
                SelectedItem = Nothing
            else
                SelectedItem = Items(SelectedIndex)
            End If
        End Sub '   UpdateSelectedIndex
        ''' <summary>
        ''' currently selected item
        ''' </summary>
        Public Property SelectedItem() As Object
            Get

                return m_SelectedItem
            End Get

            Set

                m_SelectedItem = value

                If (SelectionChanged  IsNot Nothing) Then
                    SelectionChanged(this, Nothing)
                End If
            End Set
        End Property
        ''' <summary>
        ''' current item hovered over
        ''' </summary>
        Public Property HoverItem() As Object
            Get

                return m_HoverItem
            End Get

            Set


                If (value = m_HoverItem) Then
                    return
                End If

                m_HoverItem = value

                If (HoverChanged  IsNot Nothing) Then
                    HoverChanged(this, Nothing)
                End If
            End Set
        End Property
    End Class   '   SelectableItemsControl
' End Namespace   '   MediaPlayerExtensions
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\SelectableItemsControl.cs
