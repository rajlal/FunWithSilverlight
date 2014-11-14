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

' Namespace ImageManipulation

Partial Public Class App
    Inherits Application

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub Application_Startup(sender As Object, e As Startupeventargs) Handles Me.Startup

        'Me.RootVisual = New Page()
        Me.RootVisual = New PageImage()
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
' ..\Graphics\G\Graphics\Graphics\5-ImageManipulation\App.xaml.cs
