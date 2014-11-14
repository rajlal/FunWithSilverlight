Imports System
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes
Imports System.Windows.Browser

' Namespace DeepZoom

Public Class MouseWheelEventArgs
    Inherits EventArgs

    Private m_Handled As Boolean = False

    Private m_dDelta As Double

    Public Sub New(dValue As Double)

        Me.m_dDelta = dValue
    End Sub '   New

    Public ReadOnly Property Delta() As Double
        Get
            Return Me.m_dDelta
        End Get
    End Property

    '  Use handled to prevent the default browser behavior Not
    Public Property Handled() As Boolean
        Get
            Return Me.m_Handled
        End Get
        Set(bValue As Boolean)
            Me.m_Handled = bValue
        End Set
    End Property
End Class   '   MouseWheelEventArgs

Public Class MouseWheelHelper

    Private isMouseOver As Boolean

    Public Event Moved As EventHandler(Of MouseWheelEventArgs)

    Public Shared m_oWorker As Worker

    Public Sub New(element As FrameworkElement)

        If (MouseWheelHelper.m_oWorker Is Nothing) Then
            MouseWheelHelper.m_oWorker = New Worker()
        End If

        AddHandler MouseWheelHelper.m_oWorker.Moved, AddressOf Me.HandleMouseWheel

        AddHandler element.MouseEnter, AddressOf Me.HandleMouseEnter
        AddHandler element.MouseLeave, AddressOf Me.HandleMouseLeave
        AddHandler element.MouseMove, AddressOf Me.HandleMouseMove
    End Sub '   New

    Private Sub HandleMouseWheel(sender As Object, args As MouseWheeleventargs)

        If (Me.isMouseOver) Then
            RaiseEvent Moved(Me, args)
        End If
    End Sub '   HandleMouseWheel

    Private Sub HandleMouseEnter(sender As Object, e As Eventargs)

        Me.isMouseOver = True
    End Sub '   HandleMouseEnter

    Private Sub HandleMouseLeave(sender As Object, e As Eventargs)

        Me.isMouseOver = False
    End Sub '   HandleMouseLeave

    Private Sub HandleMouseMove(sender As Object, e As Eventargs)

        Me.isMouseOver = True
    End Sub '   HandleMouseMove

    Public Class Worker

        Public Event Moved As EventHandler(Of MouseWheelEventArgs)

        Public Sub New()

            If (HtmlPage.IsEnabled) Then
                HtmlPage.Window.AttachEvent("DOMMouseScroll", AddressOf Me.HandleMouseWheel)
                HtmlPage.Window.AttachEvent("onmousewheel", AddressOf Me.HandleMouseWheel)
                HtmlPage.Document.AttachEvent("onmousewheel", AddressOf Me.HandleMouseWheel)
            End If
        End Sub '   New

        Private Sub HandleMouseWheel(sender As Object, args As HtmlEventArgs)

            Dim dDelta As Double = 0.0

            Dim eventObj As ScriptObject = args.EventObject

            If (eventObj.GetProperty("wheelDelta") IsNot Nothing) Then
                dDelta = (CType(eventObj.GetProperty("wheelDelta"), Double)) / 120

                If (HtmlPage.Window.GetProperty("opera") IsNot Nothing) Then
                    dDelta = -dDelta
                End If
            ElseIf (eventObj.GetProperty("detail") IsNot Nothing) Then
                dDelta = -(CType(eventObj.GetProperty("detail"), Double)) / 3

                If (HtmlPage.BrowserInformation.UserAgent.IndexOf("Macintosh") <> -1) Then
                    dDelta = dDelta * 3
                End If
            End If

            If ((dDelta <> 0.0) AndAlso (Me.MovedEvent IsNot Nothing)) Then
                Dim wheelArgs As MouseWheelEventArgs = New MouseWheelEventArgs(dDelta)

                RaiseEvent Moved(Me, wheelArgs)

                If (wheelArgs.Handled) Then
                    args.PreventDefault()
                End If
            End If
        End Sub '   HandleMouseWheel
    End Class   '   Worker
End Class   '   MouseWheelHelper
' End Namespace
' ..\ExtendControl\E\ExtendControl\ExtendControl\4-DeepZoom\MouseWheelHelper.cs
