'  <copyright file="SizeConstrainer.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the SizeConstrainer class</summary>
'  <author>Microsoft Expression Encoder Team</author>using System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Markup

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' Wrapper Control that prevents the content it contains from expanding when
    ''' offered infinity eg when inside a table cell with * height.
    ''' </summary>
    <ContentProperty("Child")>
    Public Class SizeConstrainer
        Inherits Control
        ''' <summary>
        ''' A child dependency property.
        ''' </summary>
        Public Property readonly() As
            Get
                return GetValue(ChildProperty) as UIElement
            End Get

            Set
                SetValue(ChildProperty, value)
            End Set
        End Property
        ''' <summary>
        ''' Event handler for the child dependency property.
        ''' </summary>
        ''' <param name="dobj">Dependency object that is changing.</param>
        ''' <param name="args">Event args.</param>
        Private ReadOnly Property Sub() As
        ''' <summary>
        ''' Overrides ArrangeOverride().
        ''' </summary>
        ''' <param name="finalSize">The final size.</param>
        ''' <returns>The arranged size.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            return MyBase.ArrangeOverride(finalSize)
        }
        ''' <summary>
        ''' Overrides OnApplyTemplate().
        ''' </summary>
        public override Sub OnApplyTemplate()

            MyBase.OnApplyTemplate()
            m_presenter = GetTemplateChild("ChildPresenter") as ContentPresenter
            OnChildChanged()
        End Sub '   OnApplyTemplate
    End Class   '   SizeConstrainer
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\SizeConstrainer.cs
