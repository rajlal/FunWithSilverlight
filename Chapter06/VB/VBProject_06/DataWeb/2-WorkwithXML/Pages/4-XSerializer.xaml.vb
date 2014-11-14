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
Imports System.Xml.Linq
Imports System.Xml.Resolvers
Imports System.Xml.Serialization

' Namespace WorkwithXML

Partial Public Class XSerializer
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        stackDeserialize.Visibility = Visibility.Visible
        stackSerialize.Visibility = Visibility.Collapsed
        DeSerializeXML()
    End Sub '   LayoutRoot_Loaded

    Private Sub FileSelect(sender As Object, e As MouseButtonEventArgs)

        Dim t As TextBlock = CType(sender, TextBlock)
        Dim SelectType As String = t.Text

        StatusBar.Text = "Selected: " + ToolTipService.GetToolTip(t).ToString()

        If (SelectType = "Serialize") Then
            stackSerialize.Visibility = Visibility.Visible
            stackDeserialize.Visibility = Visibility.Collapsed
            SerializeXML()
        ElseIf (SelectType = "De-Serialize") Then
            stackSerialize.Visibility = Visibility.Collapsed
            stackDeserialize.Visibility = Visibility.Visible
            DeSerializeXML()
        End If
    End Sub '   FileSelect

    Private Sub SerializeXML()

        Dim myitem As blogitem = New blogitem()

        myitem.title = "Seven New Community Gallery Entries"
        myitem.sLink = "http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2"
        myitem.pubDate = "Wed, 25 Mar 2009 23:43:00 GMT"
        myitem.desc = "XAP Play the Silverlight Gobang Game, practice Creating Particle Effects in Silverlight, dance with a 3D Silver Mouse, and more Not  Find inspiration and upload your Silverlight projects to share with the community in the Gallery. "

        Dim mysilverlightblog As blog = New blog()

        mysilverlightblog.item = myitem

        Dim output As StringBuilder = New StringBuilder()

        Dim doc As XDocument = New XDocument()
        Dim serializer As XmlSerializer = New XmlSerializer(GetType(blog))
        Dim xWriter As XmlWriter = XmlWriter.Create(output)
        serializer.Serialize(xWriter, mysilverlightblog)
        xmlData.Text = output.ToString()
    End Sub '   SerializeXML

    Private Sub DeSerializeXML()

        Dim xmlString As String = "<?xml version='1.0' encoding='utf-8'?> " & Environment.NewLine &
                                    "<items xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' " & Environment.NewLine &
                                    "     xmlns:xsd='http://www.w3.org/2001/XMLSchema'> " & Environment.NewLine &
                                    "   <item>  " & Environment.NewLine &
                                    "      <title>New Silverlight Toolkit Video</title>  " & Environment.NewLine &
                                    "      <link>http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls</link>  " & Environment.NewLine &
                                    "      <pubDate>Thu, 26 Mar 2009 17:13:00 GMT</pubDate> " & Environment.NewLine &
                                    "      <description>XAP Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.</description> " & Environment.NewLine &
                                    "    </item>  " & Environment.NewLine &
                                    "  </items>"

        Dim xReader As XmlReader = XmlReader.Create(New StringReader(xmlString))

        Dim serializer As XmlSerializer = New XmlSerializer(GetType(blog))

        Dim slbitem As blog = CType(serializer.Deserialize(xReader), blog)

        itemTitle.Text = slbitem.item.title
        itemLink.NavigateUri = New Uri(slbitem.item.sLink)

        Dim tt As ToolTip = New ToolTip()

        tt.Content = slbitem.item.sLink
        ToolTipService.SetToolTip(itemLink, tt)
        itempubDate.Text = slbitem.item.pubDate
        itemDesc.Text = slbitem.item.desc
            End Sub '   DeSerializeXML
End Class   '   XSerializer

<XmlRoot("items")>
Public Class blog
    <XmlElement("item")>
    Public Property item As blogitem
End Class   '   blog

Public Class blogitem
    <XmlElement("title")>
    Public Property title As String
    <XmlElement("link")>
    Public Property sLink As String
    <XmlElement("pubDate")>
    Public Property pubDate As String
    <XmlElement("description")>
    Public Property desc As String
End Class   '   blogitem
' End Namespace   '   WorkwithXML
' ..\Project_06\DataWeb\2-WorkwithXML\Pages\4-XSerializer.xaml.cs
