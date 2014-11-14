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

' Namespace DeepZoomProject

'  Courtesy of Pete Blois
Public Class MouseWheelEventArgs
    Inherits EventArgs

    Private dDelta As Double
    Private bHandled As Boolean = False

    Public Sub New(delta As Double)

        Me.dDelta = delta
    End Sub '   New

    Public ReadOnly Property Delta() As Double
        Get
            Return Me.dDelta
        End Get
    End Property

    '  Use handled to prevent the default browser behavior Not
    Public Property Handled() As Boolean
        Get
            Return Me.bHandled
        End Get
        Set(value As Boolean)
            Me.bHandled = value
        End Set
    End Property
End Class   '   MouseWheelEventArgs

Public Class MouseWheelHelper

    Public Event Moved As EventHandler(Of MouseWheelEventArgs)
    Private Shared oWorker As Worker
    Private isMouseOver As Boolean = False

    Public Sub New(element As Frameworkelement)


        If (MouseWheelHelper.oWorker Is Nothing) Then
            MouseWheelHelper.oWorker = New Worker()
        End If

        AddHandler MouseWheelHelper.oWorker.Moved, AddressOf HandleMouseWheel

        AddHandler element.MouseEnter, AddressOf HandleMouseEnter
        AddHandler element.MouseLeave, AddressOf HandleMouseLeave
        AddHandler element.MouseMove, AddressOf HandleMouseMove
    End Sub '   New

    Private Sub HandleMouseWheel(sender As Object, args As Mousewheeleventargs)

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

    Private Class Worker

        Public Event Moved As EventHandler(Of MouseWheelEventArgs)

        Public Sub New()

            If (HtmlPage.IsEnabled) Then
                HtmlPage.Window.AttachEvent("DOMMouseScroll", AddressOf HandleMouseWheel)
                HtmlPage.Window.AttachEvent("onmousewheel", AddressOf HandleMouseWheel)
                HtmlPage.Document.AttachEvent("onmousewheel", AddressOf HandleMouseWheel)
            End If
        End Sub '   New

        Private Sub HandleMouseWheel(sender As Object, args As Htmleventargs)

            Dim dDelta As Double = 0.0

            Dim eventObj As ScriptObject = args.EventObject


            If (eventObj.GetProperty("wheelDelta") IsNot Nothing) Then
                dDelta = CType(eventObj.GetProperty("wheelDelta"), Double) / 120

                If (HtmlPage.Window.GetProperty("opera") IsNot Nothing) Then
                    dDelta = -dDelta
                ElseIf (eventObj.GetProperty("detail") IsNot Nothing) Then
                    dDelta = -(CType(eventObj.GetProperty("detail"), Double) / 3)


                    If (HtmlPage.BrowserInformation.UserAgent.IndexOf("Macintosh") <> -1) Then
                        dDelta = dDelta * 3
                    End If
                End If


                If (dDelta <> 0) Then
                    Dim wheelArgs As MouseWheelEventArgs = New MouseWheelEventArgs(dDelta)
                    RaiseEvent Moved(Me, wheelArgs)

                    If (wheelArgs.Handled) Then
                        args.PreventDefault()
                    End If
                End If
            End If
        End Sub '   HandleMouseWheel
    End Class   '   Worker
End Class   '   MouseWheelHelper
' End Namespace
' ..\ExtendControl\E\ExtendControl\ExtendControl\4-DeepZoom\Wonders\Exported Data\wonders\DeepZoomProject\MouseWheelHelper.cs
