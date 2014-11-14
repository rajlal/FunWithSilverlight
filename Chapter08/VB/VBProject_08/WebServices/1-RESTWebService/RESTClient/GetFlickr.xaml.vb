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
Imports System.Threading
Imports System.Security
Imports System.IO
Imports System.Windows.Media.Imaging
Imports System.Xml

' Namespace RESTClient

Partial Public Class GetFlickr
    Inherits UserControl

    Dim syncContext As SynchronizationContext
    Dim statusString As String
    Dim MyPhotoList As PhotoList = New PhotoList()
    Dim CurrentPhotoId As String = ""
    Dim CurrentIndex As Integer = 0

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub ButtonGet_Click(sender As Object, e As RoutedEventArgs)

        '  Instantiate the data service context and clear any existing bindings.
        ButtonGet.Content = "Loading..."
        ButtonGet.IsEnabled = False
        MyPhotoList.Clear()

        '  Grab SynchronizationContext while on UI Thread
        syncContext = SynchronizationContext.Current
        '  Create request
        Dim searchPhotos As String = txtMessage.Text
        Dim request As HttpWebRequest =
                    CType(WebRequest.Create(New Uri("http://api.flickr.com/services/rest/?method=flickr.photos.search&api_key=eaa91b4b29442d95cb58e286fd43e106&tags=" + searchPhotos,
                                                    UriKind.Absolute)), 
                        HttpWebRequest)

        '  Make async call for request stream.  Callback will be called on a background thread.
        '  we could also use POST if we needed
        request.Method = "GET"
        '  we can also set custom headers on our request as well
        '  req.Headers.Headers.Add("x-made-by-silverlight")
        request.BeginGetResponse(New AsyncCallback(AddressOf ResponseCallback), request)
    End Sub '   ButtonGet_Click

    Private Sub GetPhoto(id As String)

        CurrentPhotoId = id
        '  Instantiate the data service context and clear any existing bindings.
        ButtonGet.Content = "Image..."
        ButtonGet.IsEnabled = False

        '  Grab SynchronizationContext while on UI Thread
        syncContext = SynchronizationContext.Current
        '  Create request
        Dim idPhoto As String = id
        Dim request As HttpWebRequest =
                    CType(WebRequest.Create(New Uri("http://api.flickr.com/services/rest/?method=flickr.photos.getSizes&api_key=eaa91b4b29442d95cb58e286fd43e106&photo_id=" + idPhoto,
                                                    UriKind.Absolute)), 
                        HttpWebRequest)
        '  Make async call for request stream.  Callback will be called on a background thread.
        '  we could also use POST if we needed
        request.Method = "GET"
        '  we can also set custom headers on our request as well
        '  req.Headers.Headers.Add("x-made-by-silverlight")
        request.BeginGetResponse(New AsyncCallback(AddressOf ResponseCallback), request)
    End Sub '   GetPhoto

    Private Sub ResponseCallback(ar As IAsyncResult)

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
        syncContext.Post(AddressOf ExtractResponse, response)
    End Sub '   ResponseCallback

    Private Sub ExtractResponse(state As Object)

        Dim response As HttpWebResponse = CType(state, HttpWebResponse)


        If ((response IsNot Nothing) AndAlso (response.StatusCode = HttpStatusCode.OK)) Then

            Dim responseReader As StreamReader = New StreamReader(response.GetResponseStream())
            Dim txtMessageText As String = responseReader.ReadToEnd()
            Dim flickrResult As XmlReader = XmlReader.Create(New StringReader(txtMessageText))

            flickrResult.ReadToFollowing("photo")

            If (flickrResult.EOF) Then
                flickrResult = XmlReader.Create(New StringReader(txtMessageText))
                flickrResult.ReadToFollowing("size")

                While (Not flickrResult.EOF)
                    If (flickrResult.GetAttribute("label") = "Thumbnail") Then
                        For Each p As PhotoItem In MyPhotoList
                            If (flickrResult.GetAttribute("source").Contains(p.Id)) Then
                                p.ImageUrl = flickrResult.GetAttribute("source")
                                p.Url = flickrResult.GetAttribute("url")
                            End If

                            CurrentIndex += 1
                        Next    '   p
                    End If

                    flickrResult.ReadToFollowing("size")
                End While   '

                UpdateDisplay()
            Else
                While (Not flickrResult.EOF)

                    Dim p As PhotoItem = New PhotoItem()

                    p.Id = flickrResult.GetAttribute("id")
                    p.Title = flickrResult.GetAttribute("title")
                    MyPhotoList.Add(p)
                    '  Moves the reader back to the element node.
                    flickrResult.ReadToFollowing("photo")
                End While   '

                For Each p As PhotoItem In MyPhotoList

                    GetPhoto(p.Id)
                Next    '   p

                ButtonGet.IsEnabled = True
                ButtonGet.Content = "GET"
                UpdateDisplay()
            End If
        Else
            txtMessage.Text = "Get failed: " + statusString
            ButtonGet.IsEnabled = True
            ButtonGet.Content = "GET"
        End If
    End Sub '   ExtractResponse

    Private Sub UpdateDisplay()

        Me.PhotoGrid.Visibility = Visibility.Collapsed
        Me.PhotoGrid.DataContext = MyPhotoList
        Me.PhotoGrid.UpdateLayout()
        Me.PhotoGrid.Visibility = Visibility.Visible
    End Sub '   UpdateDisplay

    Private Sub ResourceImageFailed(sender As Object, e As ExceptionRoutedEventArgs)

        Dim i As Image = CType(sender, Image)

        i.Source = New BitmapImage(New Uri("images/error.png", UriKind.Relative))

        Dim t As ToolTip = New ToolTip()

        t.Content = New TextBlock() With
                    {
                        .FontFamily = New FontFamily("Arial"),
                        .FontSize = 12,
                        .Text = "Error retrieving Image",
                        .TextWrapping = TextWrapping.Wrap
                    }
        ToolTipService.SetToolTip(i, t)
    End Sub '   ResourceImageFailed
End Class   '   GetFlickr

Public Class PhotoList
    Inherits List(Of PhotoItem)

    Dim si As PhotoItem

    Public Property val() As PhotoItem
        Get
            Return si
        End Get
        Set(value As PhotoItem)
            si = value
        End Set
    End Property

    Public Sub New()

    End Sub '   New
End Class   '   PhotoList

Public Class PhotoItem

    Private _title As String
    Private _id As String
    Private _imageurl As String
    Private _url As String

    Public Property Title() As String
        Get
            Return _title
        End Get

        Set(value As String)
            _title = value
        End Set
    End Property

    Public Property Id() As String
        Get
            Return _id
        End Get

        Set(value As String)
            _id = value
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


    Public Sub New(title As String, id As String, imageurl As String, url As String)

        _Title = title
        _Id = id
        _ImageUrl = imageurl
        _Url = url
    End Sub '   New
End Class   '   PhotoItem
' End Namespace
' ..\Project_08\WebServices\1-RESTWebService\RESTClient\GetFlickr.xaml.cs
