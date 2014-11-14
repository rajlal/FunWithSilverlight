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
Imports System.IO
Imports System.Threading
Imports System.Security
Imports System.Windows.Media.Imaging
Imports System.Xml

' Namespace RESTClient

Partial Public Class PostTwitter
    Inherits UserControl

    Dim syncContext As SynchronizationContext
    Dim statusString As String
    Dim searchString As String
    Dim MyTwitterList As TwitterList = New TwitterList()

    Public Sub New()

        InitializeComponent()
    End Sub '   New


    Private Sub ButtonPost_Click(sender As Object, e As RoutedEventArgs)

        ButtonPost.Content = "Loading..."
        ButtonPost.IsEnabled = False
        searchString = txtMessage.Text

        '  Grab SynchronizationContext while on UI Thread
        syncContext = SynchronizationContext.Current

        '  Create request
        Dim request As HttpWebRequest =
                 CType(WebRequest.Create(New Uri("http://search.twitter.com/search.rss",
                                                 UriKind.Absolute)), 
                     HttpWebRequest)
        request.Method = "POST"
        request.ContentType = "application/x-www-form-urlencoded"
        '  Make async call for request stream.  Callback will be called on a background thread.
        Dim asyncResult As IAsyncResult =
                request.BeginGetRequestStream(New AsyncCallback(AddressOf RequestStreamCallbackPost), request)
    End Sub '   ButtonPost_Click

    Private Sub RequestStreamCallbackPost(ar As IAsyncResult)

        Dim request As HttpWebRequest = CType(ar.AsyncState, HttpWebRequest)
        ' request.ContentType = "application/atom+xml"
        Dim requestStream As Stream = request.EndGetRequestStream(ar)
        Dim streamWriter As StreamWriter = New StreamWriter(requestStream)

        streamWriter.Write("q=" + searchString)
        streamWriter.Write("&lang=en")
        streamWriter.Write("&rpp=12")

        streamWriter.Flush()
        '  Close the stream.
        streamWriter.Close()
        '  Make async call for response.  Callback will be called on a background thread.
        request.BeginGetResponse(New AsyncCallback(AddressOf ResponseCallbackPost), request)
    End Sub '   RequestStreamCallbackPost

    Private Sub ResponseCallbackPost(ar As IAsyncResult)

        Dim request As HttpWebRequest = CType(ar.AsyncState, HttpWebRequest)
        Dim response As WebResponse = Nothing

        Try
            response = request.EndGetResponse(ar)
        Catch we As WebException
            statusString = we.Status.ToString()
        Catch se As SecurityException
            statusString = se.Message

            If (statusString = "") Then
                statusString = se.InnerException.Message
            End If
        End Try

        '  Invoke onto UI thread
        syncContext.Post(AddressOf ExtractResponsePost, response)
    End Sub '   ResponseCallbackPost

    Private Sub ExtractResponsePost(state As Object)

        Dim response As HttpWebResponse = CType(state, HttpWebResponse)

        If (response IsNot Nothing AndAlso response.StatusCode = HttpStatusCode.OK) Then

            Dim responseReader As StreamReader = New StreamReader(response.GetResponseStream())
            Dim txtMessageText As String = responseReader.ReadToEnd()
            Dim twitterResult As XmlReader = XmlReader.Create(New StringReader(txtMessageText))

            twitterResult.ReadToFollowing("item")

            While (Not twitterResult.EOF)

                Dim t As TwitterItem = New TwitterItem()

                twitterResult.ReadToFollowing("title")
                t.Title = twitterResult.ReadElementContentAsString()
                twitterResult.ReadToFollowing("link")
                t.Url = twitterResult.ReadElementContentAsString()
                twitterResult.ReadToFollowing("author")
                t.Author = twitterResult.ReadElementContentAsString()
                twitterResult.ReadToFollowing("google:image_link")
                t.ImageUrl = twitterResult.ReadElementContentAsString()

                MyTwitterList.Add(t)
                '  Moves the reader back to the element node.
                twitterResult.ReadToFollowing("item")
            End While   '

        Else
            txtMessage.Text = "Post failed: " + statusString
        End If

        ButtonPost.IsEnabled = True
        ButtonPost.Content = "POST"
        UpdateDisplay()
    End Sub '   ExtractResponsePost

    Private Sub UpdateDisplay()

        Me.TwitterGrid.DataContext = MyTwitterList

        Try
            Me.TwitterGrid.UpdateLayout()
        Catch ex As Exception
            statusString = ex.Message
        End Try
    End Sub '   UpdateDisplay

    Private Sub ResourceImageFailed(sender As Object, e As ExceptionRoutedEventArgs)

        Dim i As Image = CType(sender, Image)
        Dim t As ToolTip = New ToolTip()

        i.Source = New BitmapImage(New Uri("images/error.jpg", UriKind.Relative))


        t.Content = New TextBlock() With
                {
                    .FontFamily = New FontFamily("Arial"),
                    .FontSize = 12,
                    .Text = "Error retrieving Image" +
                            Environment.NewLine +
                            e.ErrorException.Message +
                            Environment.NewLine + e.ErrorException.StackTrace,
                    .TextWrapping = TextWrapping.Wrap
                }
        ToolTipService.SetToolTip(i, t)
    End Sub '   ResourceImageFailed
End Class   '   PostTwitter

Public Class TwitterList
    Inherits List(Of TwitterItem)

    Dim si As TwitterItem

    Public Property val As TwitterItem
        Get
            Return si
        End Get
        Set(value As TwitterItem)
            si = value
        End Set
    End Property

    Sub New()

    End Sub '   New
End Class   '   TwitterList

Public Class TwitterItem

    Private _title As String
    Private _author As String
    Private _imageurl As String = "images/loading.png"
    Private _url As String = "http://silverlightfun.com"

    Public Property Title() As String
        Get
            Return _title
        End Get

        Set(value As String)
            _title = value
        End Set
    End Property

    Public Property Author() As String
        Get
            Return _author
        End Get

        Set(value As String)
            _author = value
        End Set
    End Property

    Public Property ImageUrl() As String
        Get
            Return _imageurl
        End Get

        Set(value As String)
            _imageurl = value
        End Set
    End Property

    Public Property Url() As String
        Get
            Return _url
        End Get

        Set(value As String)
            _url = value
        End Set
    End Property

    Public Sub New()

    End Sub '   New

    Public Sub New(title As String, author As String, imageurl As String, url As String)

        _title = title
        _author = author
        _imageurl = imageurl
        _url = url
    End Sub '   New
End Class   '   TwitterItem
' End Namespace
' ..\Project_08\WebServices\1-RESTWebService\RESTClient\PostTwitter.xaml.cs
