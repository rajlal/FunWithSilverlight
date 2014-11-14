''' <copyright file="HotspotControl.xaml.cs" company="Microsoft">
'''     Copyright © Microsoft Corporation. All rights reserved.
''' </copyright>
''' <summary>Implements the Hotspot class which listens to various mouse
''' events and fires associated animations</summary>
''' <author>Microsoft Expression Encoder Team</author>
Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media.Animation
Imports System.Windows.Threading
Imports System.Diagnostics
Imports System.Windows.Media

' Namespace ExpressionMediaPlayer

    Public Partial Class HotspotControl
        Inherits Grid
        ''' <summary>
        ''' hotspot has focus
        ''' </summary>
        Private m_hotspotHasFocus As Boolean
        ''' <summary>
        ''' timer for delay for exit animation
        ''' </summary>
        End Property
        ''' <summary>
        ''' Hotspot control, fires named animations on various mouse events
        ''' </summary>
        Public Sub New()
            AddHandler Me.MouseEnter, AddressOf New MouseEventHandler(OnMouseEnter)
            AddHandler Me.MouseLeave, AddressOf New MouseEventHandler(OnMouseLeave)

            SetEvents(this)

            m_timerExitAnim = New DispatcherTimer()
            AddHandler m_timerExitAnim.Tick, AddressOf EventHandler(ExitAnimationTimer)
            Delay = 1.0
        End Sub '   New
        ''' <summary>
        ''' when exit animation timer times out
        ''' </summary>
        private Sub ExitAnimationTimer(sender As Object, e As EventArgs)

            m_timerExitAnim.Stop()

            If (Me.MouseLeaveAnimation  IsNot Nothing) Then
                Me.MouseLeaveAnimation.Begin()
            End If
        End Sub '   ExitAnimationTimer
        ''' <summary>
        ''' timed delay before showing exit animation
        ''' </summary>
        Public Property Delay As Double
        ''' <summary>
        ''' Animation to fire when mouse enters hotspot
        ''' </summary>
        End Property
        ''' <summary>
        ''' Animation to fire when mouse leaves hotspot
        ''' </summary>
        End Property
        ''' <summary>
        ''' handle mouse entering hotspot
        ''' </summary>
        protected Sub OnMouseEnter(sender,MouseEventArgs As Object e)

            FireEnterAnimation()
        End Sub '   OnMouseEnter
        ''' <summary>
        ''' handle mouse leaving hotspot
        ''' </summary>
        protected Sub OnMouseLeave(sender As Object, e As MouseEventArgs)

            FireLeaveAnimation()
        End Sub '   OnMouseLeave
        ''' <summary>
        ''' is this control a child of the hotspot area
        ''' </summary>
        ''' <param name="dependancyObject"></param>
        ''' <returns></returns>
        private Function IsHotSpotOrChild(dependancyObjectTest As Object) As Boolean
   
            Dim bReturn  As Boolean = False

            If (dependancyObjectTest <> Nothing  AndAlso  dependancyObjectTest is DependencyObject) Then
                DependencyObject dependancyObject = dependancyObjectTest as DependencyObject
                do
                    If (dependancyObject = CType(Me, DependencyObject) Then
                        bReturn = True
                        Exit Do
                    End If

                    dependancyObject = VisualTreeHelper.GetParent(dependancyObject)
                while (dependancyObject IsNot Nothing)
            End If

            Return bReturn
        End Function  '   IsHotSpotOrChild
        ''' <summary>
        ''' handle focus entering hotspot control (or any child controls)
        ''' </summary>
        Sub HotspotControl_GotFocus(sender As Object, e As RoutedEventArgs)

            If (IsHotSpotOrChild(e.OriginalSource)) Then
                If (Not m_hotspotHasFocus) Then
                    FireEnterAnimation()
                    m_hotspotHasFocus = true
                End If
            End If
        End Sub '   HotspotControl_GotFocus
        ''' <summary>
        ''' handle focus leaving hotspot control (or any child controls)
        ''' </summary>
        Sub HotspotControl_LostFocus(sender As Object, e As RoutedEventArgs)

            Dim objectNewFocus As Object  = FocusManager.GetFocusedElement()

            If (Not IsHotSpotOrChild(objectNewFocus)) Then
                FireLeaveAnimation()
                m_hotspotHasFocus = false
            End If
        End Sub '   HotspotControl_LostFocus
        ''' <summary>
        ''' set animation events for this element and all child controls
        ''' </summary>
        ''' <param name="elem"></param>
        private Sub SetEvents(elem As UIElement)

            AddHandler elem.GotFocus, AddressOf RoutedEventHandler(HotspotControl_GotFocus)
            AddHandler elem.LostFocus, AddressOf RoutedEventHandler(HotspotControl_LostFocus)

            If (elem is Panel) Then
                For Each elemChild As UIElement in ((Panel)elem).Children
                    SetEvents(elemChild)
                Next    '   elemChild
            End If
        End Sub '   SetEvents
        ''' <summary>
        ''' shows the hotspot enter animation if defined
        ''' </summary>
        protected Sub FireEnterAnimation()

            If (Me.MouseEnterAnimation  IsNot Nothing) Then
                Me.m_timerExitAnim.Stop()
                Me.MouseEnterAnimation.Begin()
            End If
        End Sub '   FireEnterAnimation
        ''' <summary>
        ''' shows the hostspot leave animation if defined
        ''' </summary>
        protected Sub FireLeaveAnimation()

            If (Me.MouseLeaveAnimation  IsNot Nothing) Then
                m_timerExitAnim.Interval = New TimeSpan((long)(Delay * 10000000))
                m_timerExitAnim.Start()
            End If
        End Sub '   FireLeaveAnimation
    End Class   '   HotspotControl
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\ExpressionPlayer\HotspotControl.cs
