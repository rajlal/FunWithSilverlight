'  <copyright file="AccessibleListBox.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <author>Microsoft Expression Encoder Team</author>
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Automation
Imports System
Imports System.Windows.Media

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' allows screen reader to read items defined in a listbox which are not listbox items.
    ''' </summary>
    Public Class AccessibleListBox
        Inherits ListBox
        Private ReadOnly Property DependencyObject() As

        protected override Sub PrepareContainerForItemOverride(element As DependencyObject, item As Object)

            MyBase.PrepareContainerForItemOverride(element, item)
            IAccessible accessible = item as IAccessible

            If (accessible <> Nothing) Then
                Button btnFake = New Button()
                btnFake.IsEnabled = false
                btnFake.Visibility = Visibility.Collapsed
                btnFake.SetValue(AutomationProperties.NameProperty, accessible.AccessibilityText)
                element.SetValue(AutomationProperties.LabeledByProperty, CType(btnFake, UIElement))
            End If


            Dim lbi As ListBoxItem  = CType(GetVisualParentByType(element, ListBoxItem), typeof(ListBoxItem))

        End Sub '   PrepareContainerForItemOverride
    End Class   '   AccessibleListBox
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\AccessibleListBox.cs
