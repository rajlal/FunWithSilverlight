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
Imports System.Windows.Media.Imaging
Imports System.ComponentModel
Imports System.Windows.Data
Imports System.Xml
Imports System.Windows.Browser
Imports System.Text
Imports System.IO
Imports System.Json
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json

' Namespace UseJSON

Partial Public Class JLoadSave
    Inherits UserControl
    Public Sub New()
        InitializeComponent()
    End Sub '   New

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        ResetView()
        stackLoadText.Visibility = Visibility.Visible
        LoadTextReader()
    End Sub '   LayoutRoot_Loaded

    Private Sub SelectAction(sender As Object, e As MouseButtonEventArgs)

        ResetView()

        Dim t As TextBlock = CType(sender, TextBlock)
        Dim SelectType As String = t.Text

        StatusBar.Text = ToolTipService.GetToolTip(t).ToString()

        If (SelectType = "Load TextReader") Then
            stackLoadText.Visibility = Visibility.Visible
            LoadTextReader()
        ElseIf (SelectType = "Load Stream") Then
            stackLoadStream.Visibility = Visibility.Visible
            LoadStream()
        ElseIf (SelectType = "Parse text") Then
            stackParseText.Visibility = Visibility.Visible
            ParseJSONText()
        ElseIf (SelectType = "Save") Then
            stackSave.Visibility = Visibility.Visible
            SaveJSON()
        End If
    End Sub '   SelectAction

    Private Sub SaveJSON()

        Dim myJson As JsonObject = New JsonObject()

        myJson.Add("title", "Seven New Community Gallery Entries")
        myJson.Add("link", "http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2")
        myJson.Add("pubDate", "Wed, 25 Mar 2009 23:43:00 GMT")
        myJson.Add("description", "Created JSON, Play the Silverlight Gobang Game, practice Creating Particle Effects in Silverlight, dance with a 3D Silver Mouse, and more Not  Find inspiration and upload your Silverlight projects to share with the community in the Gallery. ")

        Dim mStream As MemoryStream = New MemoryStream()

        myJson.Save(mStream)

        Dim cAJson As Byte() = mStream.ToArray()

        mStream.Close()
        txtSave.Text = Encoding.UTF8.GetString(cAJson, 0, cAJson.Length)
        StatusInfo.Text = "JsonObject.Save"
    End Sub '   SaveJSON

    Private Sub ParseJSONText()

        Dim stringJson As String = "{'title':'Seven New Community Gallery Entries','link':'http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2','pubDate':'Wed, 25 Mar 2009 23:43:00 GMT','description':'Parsed JSON, Play the Silverlight Gobang Game, practice Creating Particle Effects in Silverlight, dance with a 3D Silver Mouse, and more Not Find inspiration and upload your Silverlight projects to share with the community in the Gallery. '}"

        Dim blogitem As JsonObject = CType(JsonObject.Parse(stringJson), JsonObject)

        itemTitleP.Text = blogitem("title")
        itemLinkP.NavigateUri = New Uri(blogitem("link"))

        Dim tt As ToolTip = New ToolTip()

        tt.Content = blogitem("link")
        ToolTipService.SetToolTip(itemLink, tt)
        itempubDateP.Text = blogitem("pubDate")
        itemDescP.Text = blogitem("description")

        StatusInfo.Text = "JSON Parsed from Text"
    End Sub '   ParseJSONText

    Private Sub LoadStream()

        LoadRemoteFile("http://silverlightfun.com/Samples/Chapter-05/WebSilverlightNewsItem.Json")
        StatusInfo.Text = "Remote JSON file"
    End Sub '   LoadStream

    Private Sub LoadRemoteFile(remoteJsonfile As String)

        Dim c As WebClient = New WebClient()

        AddHandler c.OpenReadCompleted, AddressOf RemoteFileLoaded ' OpenReadCompletedEventHandler()
        c.OpenReadAsync(New Uri(remoteJsonfile))
    End Sub '   LoadRemoteFile

    Private Sub RemoteFileLoaded(sender As Object, e As OpenReadCompletedEventArgs)

        If (e.Error Is Nothing) Then

            Dim responseStream As Stream = e.Result
            Dim blogitem As JsonObject = CType(JsonObject.Load(responseStream), JsonObject)

            itemTitle.Text = blogitem("title")
            itemLink.NavigateUri = New Uri(blogitem("link"))

            Dim tt As ToolTip = New ToolTip()

            tt.Content = blogitem("link")
            ToolTipService.SetToolTip(itemLink, tt)
            itempubDate.Text = blogitem("pubDate")
            itemDesc.Text = blogitem("description")
        End If
    End Sub '   RemoteFileLoaded

    Private Sub LoadTextReader()

        Dim stringJson As String = "{'title':'Seven New Community Gallery Entries','link':'http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2','pubDate':'Wed, 25 Mar 2009 23:43:00 GMT','description':'TextReader Play the Silverlight Gobang Game, practice Creating Particle Effects in Silverlight, dance with a 3D Silver Mouse, and more Not Find inspiration and upload your Silverlight projects to share with the community in the Gallery. '}"
        Dim blogitem As JsonObject = CType(JsonObject.Load(New StringReader(stringJson)), JsonObject)

        itemTitleS.Text = blogitem("title")
        itemLinkS.NavigateUri = New Uri(blogitem("link"))

        Dim tt As ToolTip = New ToolTip()

        tt.Content = blogitem("link")
        ToolTipService.SetToolTip(itemLink, tt)
        itempubDateS.Text = blogitem("pubDate")
        itemDescS.Text = blogitem("description")

        StatusInfo.Text = "JSON from TextReader "
    End Sub '   LoadTextReader

    Private Sub ResetView()

        stackLoadStream.Visibility = Visibility.Collapsed
        stackLoadText.Visibility = Visibility.Collapsed
        stackParseText.Visibility = Visibility.Collapsed
        stackSave.Visibility = Visibility.Collapsed
    End Sub '   ResetView
End Class   '   JLoadSave
' End Namespace   '   UseJSON
' ..\Project_06\DataWeb\3-UseJSON\JLoadSave.xaml.cs
