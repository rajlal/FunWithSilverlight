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
Imports System.Security
Imports System.IO
Imports System.Threading

' Namespace RESTClient

    Public Partial Class MainPage
        Inherits UserControl

        Public Sub New()

            InitializeComponent()
        End Sub '   New


        Dim syncContext As SynchronizationContext

        private Sub Button_Click(sender As object, e As RoutedEventArgs)
        End Sub '   Button_Click


        Dim statusString As String
        ' private void RequestStreamCallback(IAsyncResult ar)
        ' {
        '     HttpWebRequest request = ar.AsyncState as HttpWebRequest
        '     request.ContentType = "application/atom+xml"
        '     '  Make async call for response.  Callback will be called on a background thread.
        '     request.BeginGetResponse(new AsyncCallback(ResponseCallback), request)

        ' }
    End Class   '   MainPage
' End Namespace
' ..\Project_08\WebServices\1-RESTWebService\RESTClient\MainPage.xaml.cs
