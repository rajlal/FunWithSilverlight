'  <copyright file="StretchBox.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the StretchBox class</summary>
'  <author>Microsoft Expression Encoder Team</author>
' Namespace ExpressionMediaPlayer

Imports    using System
    using System.Diagnostics
    using System.Globalization
    using System.Windows
    using System.Windows.Controls
    using System.Windows.Markup
    using System.Windows.Media

    ''' <summary>
    ''' Stretch box control.
    ''' </summary>
    <ContentProperty("Child")>
    <TemplatePart(Name := StretchBox.ChildBorder, Type := GetType(Border))>
    Public Class StretchBox
        Inherits Control
        ''' <summary>
        ''' ChildBorder property string.
        ''' </summary>
        private const string ChildBorder = "ChildBorder"
        ''' <summary>
        ''' Child border.
        ''' </summary>
        Private childBorder As Border
        ''' <summary>
        ''' Original size.
        ''' </summary>
        private Size sizeOriginal
        ''' <summary>
        ''' Flag indicating whether the controls original size has been measured.
        ''' </summary>
        Private sizeOriginalHasBeenMeasured As Boolean
        ''' <summary>
        ''' Scaling factor.
        ''' </summary>
        Private scalingFactor As Double = 1.0
        ''' <summary>
        ''' The size we actually need.
        ''' </summary>
        private Size sizeActuallyNeeded
        ''' <summary>
        ''' Child dependency property.
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
        ''' Overrides OnApplyTemplate().
        ''' </summary>
        public override Sub OnApplyTemplate()

            MyBase.OnApplyTemplate()
            childBorder = GetTemplateChild(ChildBorder) as Border
            sizeOriginal = New Size(0, 0)
            sizeOriginalHasBeenMeasured = false
            OnChildChanged()
        End Sub '   OnApplyTemplate
        ''' <summary>
        ''' Dependency property handler for the child property.
        ''' </summary>
        ''' <param name="dobj">Source dependency object.</param>
        ''' <param name="args">Event args.</param>
        Private ReadOnly Property Sub() As
        ''' <summary>
        ''' Overrides ArrangeOverride().
        ''' </summary>
        ''' <param name="finalSize">The final size.</param>
        ''' <returns>The returned final size.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {

            If (childBorder  IsNot Nothing) Then

                Dim st As ScaleTransform  = New ScaleTransform()

                st.ScaleX = scalingFactor
                st.ScaleY = scalingFactor
                st.CenterX = 0
                st.CenterY = 0

                childBorder.RenderTransform = st

                childBorder.Arrange(new Rect(0, 0, sizeActuallyNeeded.Width, sizeActuallyNeeded.Height))
            End If


            return finalSize
        }
    End Class   '   StretchBox
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\StretchBox.cs
