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

' Namespace SlideShow

Partial Public Class App
    Inherits Application

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub Application_Startup(sender As Object, e As StartupEventArgs) Handles Me.Startup

        Dim myWidth As Double = 500
        Dim myHeight As Double = 400

        Dim sAStringArray As String()
        ReDim sAStringArray(0 To e.InitParams.Keys.Count - 1)
        Dim j As Integer = 0

        For Each key As String In e.InitParams.Keys
            If (key = "SlideWidth") Then
                myWidth = Double.Parse(e.InitParams(key).ToString())
            ElseIf (key = "SlideHeight") Then
                myHeight = Double.Parse(e.InitParams(key).ToString())
            Else
                sAStringArray(j) = e.InitParams(key).ToString()
                j += 1
            End If
        Next    '   key

        sAStringArray(j) = "Images/_Default.jpg"
        ReDim Preserve sAStringArray(0 To j)
        Me.RootVisual = New PageSlide(sAStringArray, myWidth, myHeight)
    End Sub '   Application_Startup

    Private Sub Application_Exit(sender As Object, e As EventArgs) Handles Me.Exit

    End Sub '   Application_Exit

    Private Sub Application_UnhandledException(sender As Object, e As ApplicationUnhandledExceptionEventArgs) Handles Me.UnhandledException

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
    Private Sub ReportErrorToDOM(e As ApplicationUnhandledExceptionEventArgs)

        Try

            Dim errorMsg As String = e.ExceptionObject.Message + e.ExceptionObject.StackTrace

            errorMsg = errorMsg.Replace("""", "'").Replace(ChrW(13) & ChrW(10), "\n")

            System.Windows.Browser.HtmlPage.Window.Eval("throw New Error(""Unhandled Error in Silverlight 2 Application " + errorMsg + """);")
        Catch ex As Exception
        End Try
    End Sub '   ReportErrorToDOM
End Class   '   App
' End Namespace   '   SlideShow
' ..\Project_05\ExtendBrowser\2b-ExampleSlideShow\App.xaml.cs
