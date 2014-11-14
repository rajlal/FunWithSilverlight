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
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json

' Namespace UseJSON

Partial Public Class JSerializer
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        stackSerialize.Visibility = Visibility.Collapsed
        DeSerializeJSON()
    End Sub '   LayoutRoot_Loaded

    Private Sub SelectAction(sender As Object, e As MouseButtonEventArgs)

        Dim t As TextBlock = CType(sender, TextBlock)
        Dim SelectType As String = t.Text

        StatusBar.Text = ToolTipService.GetToolTip(t).ToString()

        If (SelectType = "Serialize") Then
            stackSerialize.Visibility = Visibility.Visible
            stackDeserialize.Visibility = Visibility.Collapsed
            SerializeJSON()
        ElseIf (SelectType = "De-Serialize") Then
            stackSerialize.Visibility = Visibility.Collapsed
            stackDeserialize.Visibility = Visibility.Visible
            DeSerializeJSON()
        End If
    End Sub '   SelectAction

    Private Sub SerializeJSON()

        Dim myitem As BlogItemJSON = New BlogItemJSON()

        myitem.title = "Seven New Community Gallery Entries"
        myitem.link = "http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2"
        myitem.pubDate = "Wed, 25 Mar 2009 23:43:00 GMT"
        myitem.description = "XAP Play the Silverlight Gobang Game, practice Creating Particle Effects in Silverlight, dance with a 3D Silver Mouse, and more Not  Find inspiration and upload your Silverlight projects to share with the community in the Gallery. "

        ' Create a stream to serialize the object to.
        Dim mStream As MemoryStream = New MemoryStream()

        '  Serializer the BlogItem object to the stream.
        Dim Serializer As DataContractJsonSerializer = New DataContractJsonSerializer(GetType(BlogItemJSON))

        Serializer.WriteObject(mStream, myitem)

        Dim cAJson As Byte() = mStream.ToArray()

        mStream.Close()
        jsonData.Text = Encoding.UTF8.GetString(cAJson, 0, cAJson.Length)
    End Sub '   SerializeJSON

    Private Sub DeSerializeJSON()

        Dim deserializedBlogItem As BlogItemJSON = New BlogItemJSON()
        Dim jsonString As String = "{'title':'Seven New Community Gallery Entries','link':'http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2','pubDate':'Wed, 25 Mar 2009 23:43:00 GMT','description':'XAP Play the Silverlight Gobang Game, practice Creating Particle Effects in Silverlight, dance with a 3D Silver Mouse, and more Not Find inspiration and upload your Silverlight projects to share with the community in the Gallery. '}"

        Dim mStream As MemoryStream = New MemoryStream(Encoding.UTF8.GetBytes(jsonString))
        Dim Serializer As DataContractJsonSerializer = New DataContractJsonSerializer(deserializedBlogItem.GetType())

        deserializedBlogItem = CType(Serializer.ReadObject(mStream), BlogItemJSON)
        mStream.Close()
        itemTitle.Text = deserializedBlogItem.title
        itemLink.NavigateUri = New Uri(deserializedBlogItem.link)

        Dim tt As ToolTip = New ToolTip()

        tt.Content = deserializedBlogItem.link
        ToolTipService.SetToolTip(itemLink, tt)
        itempubDate.Text = deserializedBlogItem.pubDate
        itemDesc.Text = deserializedBlogItem.description
    End Sub '   DeSerializeJSON
End Class   '   JSerializer

<DataContract()>
Public Class BlogItemJSON
    <DataMember()>
    Public Property title As String
    <DataMember()>
    Public Property link As String
    <DataMember()>
    Public Property pubDate As String
    <DataMember()>
    Public Property description As String
End Class   '   BlogItemJSON
' End Namespace   '   UseJSON
' ..\Project_06\DataWeb\3-UseJSON\JSerializer.xaml.cs
