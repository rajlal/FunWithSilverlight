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
        Public class MouseWheelEventArgs
        Inherits EventArgs

            Private delta As Double
            Private handled As Boolean = false

            Public Sub New(delta As Double)

                Me.delta = delta
            End Sub '   New

            Public ReadOnly Property Delta() As Double
                Get
                    return Me.delta
                End Get
            End Property

            '  Use handled to prevent the default browser behavior Not
            Public Property Handled() As Boolean
                Get
                    return Me.handled
                End Get
                Set
                    Me.handled = value
                End Set
            End Property
        End Class   '   MouseWheelEventArgs

        Public class MouseWheelHelper

            public event EventHandler<MouseWheelEventArgs> Moved
            End Property

            Public Sub New(element As Frameworkelement)


                If (MouseWheelHelper.worker = Nothing) Then
                    MouseWheelHelper.worker = New Worker()
                End If

                MouseWheelHelper.worker.Moved += Me.HandleMouseWheel

                element.MouseEnter += Me.HandleMouseEnter
                element.MouseLeave += Me.HandleMouseLeave
                element.MouseMove += Me.HandleMouseMove
            End Sub '   New

            private Sub HandleMouseWheel(sender As Object, args As Mousewheeleventargs)


                If (Me.isMouseOver) Then
                    Me.Moved(this, args)
                End If
            End Sub '   HandleMouseWheel

            private Sub HandleMouseEnter(sender As Object, e As Eventargs)

                Me.isMouseOver = true
            End Sub '   HandleMouseEnter

            private Sub HandleMouseLeave(sender As Object, e As Eventargs)

                Me.isMouseOver = false
            End Sub '   HandleMouseLeave

            private Sub HandleMouseMove(sender As Object, e As Eventargs)

                Me.isMouseOver = true
            End Sub '   HandleMouseMove

            Private class Worker

                public event EventHandler<MouseWheelEventArgs> Moved

                Public Sub New()


                    If (HtmlPage.IsEnabled) Then
                        HtmlPage.Window.AttachEvent("DOMMouseScroll", Me.HandleMouseWheel)
                        HtmlPage.Window.AttachEvent("onmousewheel", Me.HandleMouseWheel)
                        HtmlPage.Document.AttachEvent("onmousewheel", Me.HandleMouseWheel)
                    End If
                End Sub '   New

                private Sub HandleMouseWheel(sender As Object, args As Htmleventargs)

                    Dim delta As Double  = 0

                    Dim eventObj As ScriptObject  = args.EventObject


                    If (eventObj.GetProperty("wheelDelta")  IsNot Nothing) Then
                        delta = (CType(eventObj.GetProperty("wheelDelta", Double))) / 120

                        if (HtmlPage.Window.GetProperty("opera")  IsNot Nothing)
                            delta = -delta
                    ElseIf (eventObj.GetProperty("detail")  IsNot Nothing) Then
                        delta = -(CType(eventObj.GetProperty("detail", Double))) / 3


                        If (HtmlPage.BrowserInformation.UserAgent.IndexOf("Macintosh")  <>  -1) Then
                            delta = delta * 3
                        End If
                    End If


                    If (delta  <>  0  AndAlso  Me.Moved  IsNot Nothing) Then
                        MouseWheelEventArgs wheelArgs = New MouseWheelEventArgs(delta)
                        Me.Moved(this, wheelArgs)


                        If (wheelArgs.Handled) Then
                            args.PreventDefault()
                        End If
                    End If
                End Sub '   HandleMouseWheel
            End Class   '   Worker
        End Class   '   MouseWheelHelper
' End Namespace
' ..\ExtendControl\E\ExtendControl\ExtendControl\4-DeepZoom\WorldMap\Exported Data\worldmap\DeepZoomProject\MouseWheelHelper.cs
