Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes
Imports Microsoft.VisualBasic

' Namespace CommonControls

Partial Public Class App
    Inherits Application

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub Application_Startup(sender As Object, e As Startupeventargs) Handles Me.Startup

        Dim sChoice As String = "0"

        Select Case (schoice)
            Case "0"            '  To use example change the value of sChoice
                Me.RootVisual = New Page()
            Case "1"            '  Example 1 Grid
                Me.RootVisual = New PageGrid()
            Case "2"            '  Example 2 Grid Splitter
                Me.RootVisual = New PageGridSplitter()
            Case "3"            '  Example 3 Popup which uses TestPopup xaml
                Me.RootVisual = New PagePopup()
            Case "4"            '  Example 4 StackPanel
                Me.RootVisual = New PageStackPanel()
            Case "5"            '  Example 5 RepeatButton
                Me.RootVisual = New PageRepeatButton()
            Case "6"            '  Example 6 ToolTip 6 variations
                Me.RootVisual = New PageToolTip()
            Case "7"            '  Example 7 Canvas and Shapes
                Me.RootVisual = New PageCanvasShapes()
        End Select
    End Sub '   Application_Startup

    Private Sub Application_Exit(sender As Object, e As Eventargs) Handles Me.Exit

    End Sub '   Application_Exit

    Private Sub Application_UnhandledException(sender As Object, e As Applicationunhandledexceptioneventargs) Handles Me.UnhandledException

        '  If the app is running outside of the debugger then report the exception using
        '  the browser's exception mechanism. On IE this will display it a yellow alert 
        '  icon in the status bar and Firefox will display a script error.

        If (Not System.Diagnostics.Debugger.IsAttached) Then

            '  NOTE: This will allow the application to continue running after an exception has been thrown
            '  but not handled. 
            '  For production applications this error handling should be replaced with something that will 
            '  report the error to the website and stop the application.
            e.Handled = True
            Deployment.Current.Dispatcher.BeginInvoke(Sub() ReportErrorToDOM(e))
        End If
    End Sub '   Application_UnhandledException

    Private Sub ReportErrorToDOM(e As Applicationunhandledexceptioneventargs)

        Try

            Dim errorMsg As String = e.ExceptionObject.Message + e.ExceptionObject.StackTrace
            errorMsg = errorMsg.Replace("""", "'").Replace(ChrW(13) & ChrW(10), "\n")

            System.Windows.Browser.HtmlPage.Window.Eval("throw New Error(""Unhandled Error in Silverlight 2 Application " + errorMsg + """);")

        Catch ex As Exception

        End Try

    End Sub '   ReportErrorToDOM
End Class   '   App
' End Namespace 
' ..\RichInternetApplication\1-CommonControls\App.xaml.cs
