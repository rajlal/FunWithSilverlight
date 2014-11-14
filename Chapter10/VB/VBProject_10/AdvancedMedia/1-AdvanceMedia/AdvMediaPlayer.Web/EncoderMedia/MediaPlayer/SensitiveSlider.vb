'  <copyright file="SensitiveSlider.cs" company="Microsoft">
'      Copyright © Microsoft Corporation. All rights reserved.
'  </copyright>
'  <summary>Implements the SensitiveSlider class</summary>
'  <author>Microsoft Expression Encoder Team</author>
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Input

' Namespace ExpressionMediaPlayer

    ''' <summary>
    ''' A timeline slider control class.
    ''' </summary>
    <TemplatePart(Name := SensitiveSlider.HorizontalThumb, Type := GetType(Thumb))>
    <TemplatePart(Name := SensitiveSlider.VerticalThumb, Type := GetType(Thumb))>
    Public Class SensitiveSlider
        Inherits Slider
        ''' <summary>
        ''' part names used for the up/down track parts in slider
        ''' </summary>
        Private readonly As  string() PartNamesArray = New string() {
            '  old part names in Slider control
            "LeftTrack",
            "RightTrack",
            "UpTrack",
            "DownTrack",
            '  new part names in Slider control
            "HorizontalTrackLargeChangeDecreaseRepeatButton",
            "HorizontalTrackLargeChangeIncreaseRepeatButton",
            "VerticalTrackLargeChangeDecreaseRepeatButton",
            "VerticalTrackLargeChangeIncreaseRepeatButton"
        }
        ''' <summary>
        ''' Property string for HorizontalThumb. (old)
        ''' </summary>
        private const string HorizontalThumb = "HorizontalThumb"
        ''' <summary>
        ''' Property string for VerticalThumb.
        ''' </summary>
        private const string VerticalThumb = "VerticalThumb"
        ''' <summary>
        ''' Internal element for Slider horizontal thumb "HorizontalThumb".
        ''' </summary>
        private Thumb m_horizontalThumb
        ''' <summary>
        ''' Internal element for Slider verical thumb "VerticalThumb".
        ''' </summary>
        private Thumb m_verticalThumb
        ''' <summary>
        ''' The slider position at the start of a drag.
        ''' </summary>
        Private m_dragStartValue As Double
        ''' <summary>
        ''' Initializes a new instance of the SensitiveSlider class. You can click anywhere on slider to step to a particular time.
        ''' </summary>
        Public Sub New()
            AddHandler Me.ValueChanged, AddressOf New RoutedPropertyChangedEventHandler<double>(SensitiveSliderValueChanged)
        End Sub '   New
        ''' <summary>
        ''' Value changed event (with optional filtering of SliderDrag value changes.
        ''' </summary>
        public event RoutedPropertyChangedEventHandler<double> FilteredValueChanged
        ''' <summary>
        ''' Thumb drag started event.
        ''' </summary>
        public event DragStartedEventHandler DragStarted
        ''' <summary>
        ''' Thumb drag completed event.
        ''' </summary>
        public event DragCompletedEventHandler DragCompleted
        ''' <summary>
        ''' Gets a value indicating whether the user is currently dragging the thumb.
        ''' </summary>
        <System.ComponentModel.EditorBrowsable(EditorBrowsableState.Never)>
        public virtual bool IsDragging
        {
            get
            {

                If (m_horizontalThumb  IsNot Nothing  AndAlso  m_horizontalThumb.IsDragging) Then
                    Debug.WriteLine("Horizontal drag Not ")
                    return true
                End If



                If (m_verticalThumb  IsNot Nothing  AndAlso  m_verticalThumb.IsDragging) Then
                    Debug.WriteLine("Vertical drag Not ")
                    return true
                End If


                return false
            }
        }
        ''' <summary>
        ''' Overridden OnApplyTemplate, sets internal elements and events.
        ''' </summary>
        public override Sub OnApplyTemplate()

            MyBase.OnApplyTemplate()

            m_horizontalThumb = GetTemplateChild(HorizontalThumb) as Thumb
            m_verticalThumb = GetTemplateChild(VerticalThumb) as Thumb


            If (m_horizontalThumb  IsNot Nothing) Then
                AddHandler m_horizontalThumb.DragStarted, AddressOf DragStartedEventHandler(OnDragStarted)
                AddHandler m_horizontalThumb.DragCompleted, AddressOf DragCompletedEventHandler(OnDragCompleted)
            End If



            If (m_verticalThumb  IsNot Nothing) Then
                AddHandler m_verticalThumb.DragStarted, AddressOf DragStartedEventHandler(OnDragStarted)
                AddHandler m_verticalThumb.DragCompleted, AddressOf DragCompletedEventHandler(OnDragCompleted)
            End If



            For Each partName As String in PartNamesArray

                Dim element As FrameworkElement  = CType(GetTemplateChild(partName),  FrameworkElement)


                If (element  IsNot Nothing) Then
                    AddHandler element.MouseLeftButtonDown, AddressOf MouseButtonEventHandler(OnMouseClick)
                End If
            Next    '   partName
        End Sub '   OnApplyTemplate
        ''' <summary>
        ''' Filtered ValueChanged event handler.
        ''' </summary>
        ''' <param name="sender">Source object, Slider.</param>
        ''' <param name="e">Property changed event args.</param>
        Sub SensitiveSliderValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of double))

            '''/Debug.WriteLine("SensitiveSliderValueChanged -- sending change new value:" + e.NewValue.ToString(CultureInfo.CurrentCulture))
            RoutedPropertyChangedEventHandler<double> handler = FilteredValueChanged

            If (handler  IsNot Nothing  AndAlso   Not IsDragging) Then
                handler(this, New RoutedPropertyChangedEventArgs<double>(e.OldValue, e.NewValue))
            End If
        End Sub '   SensitiveSliderValueChanged
        ''' <summary>
        ''' Computes the new slider value based on mouse position -- takes into account the orientation of the slider.
        ''' </summary>
        ''' <returns>The new slider value.</returns>
        ''' <param name="args">Mouse button args.</param>
        protected Function CalcValue(args As MouseEventArgs) As Double

            Dim pt As Point  = args.GetPosition(this)
            Dim valueNew As Double


            If (Me.Orientation = Orientation.Horizontal) Then
                valueNew =(((pt.X-(m_horizontalThumb.ActualWidth / 2)) /
                            (ActualWidth - m_horizontalThumb.ActualWidth) * (Maximum - Minimum) )
                            + Minimum )
            else
                valueNew =(((ActualHeight - pt.Y)-(m_verticalThumb.ActualHeight / 2)) /
                            (ActualHeight - m_verticalThumb.ActualHeight) * ((Maximum - Minimum) )
                            + Minimum )
            End If

            return valueNew
        End Function  '   CalcValue
        ''' <summary>
        ''' Handle DragStart event.
        ''' </summary>
        ''' <param name="sender">Source object, Thumb.</param>
        ''' <param name="e">Drag start args.</param>
        protected virtual Sub OnDragStarted(sender As Object, e As DragStartedEventArgs)

            m_dragStartValue = Me.Value

            If (DragStarted  IsNot Nothing) Then
                DragStarted(sender, e)
            End If
        End Sub '   OnDragStarted
        ''' <summary>
        ''' Handle DragCompleted event.
        ''' </summary>
        ''' <param name="sender">Source object, Thumb.</param>
        ''' <param name="e">Drag completed args.</param>
        protected virtual Sub OnDragCompleted(sender As Object, e As DragCompletedEventArgs)

            RoutedPropertyChangedEventHandler<double> handler = FilteredValueChanged

            If (handler  IsNot Nothing) Then
                handler(this, New RoutedPropertyChangedEventArgs<double>(m_dragStartValue, Me.Value))
            End If



            If (DragCompleted  IsNot Nothing) Then
                DragCompleted(sender, e)
            End If
        End Sub '   OnDragCompleted
        ''' <summary>
        ''' Send send change event when the mouse clicks down the track bar.
        ''' </summary>
        ''' <param name="sender">Source object, track.</param>
        ''' <param name="args">Mouse button args.</param>
        protected virtual Sub OnMouseClick(sender As Object, args As MouseButtonEventArgs)

            Value = CalcValue(args)
        End Sub '   OnMouseClick
    End Class   '   SensitiveSlider
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\SensitiveSlider.cs
