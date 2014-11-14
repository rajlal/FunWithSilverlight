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
Imports Microsoft.VisualBasic

' Namespace WorkwithXML

Partial Public Class XWriter
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        CreateNode()
    End Sub '   LayoutRoot_Loaded

    Private Sub CreateSelect(sender As Object, e As MouseButtonEventArgs)

        Dim t As TextBlock = CType(sender, TextBlock)
        Dim FileSelectType As String = t.Text

        StatusBar.Text = "Selected: " + ToolTipService.GetToolTip(t).ToString()

        If (FileSelectType = "XML Element") Then
            CreateNode()
        ElseIf (FileSelectType = "Create XML") Then
            CreateXML()

        ElseIf (FileSelectType = "XML File") Then
            LoadRemoteFile("http://silverlightfun.com/Samples/Chapter-05/WebSilverlightNews.xml")
        End If
    End Sub '   CreateSelect

    Private Sub CreateNode()

        Dim output As StringBuilder = New StringBuilder()

        Dim settings As XmlWriterSettings = New XmlWriterSettings()
        settings.Indent = True
        settings.IndentChars = (ControlChars.Tab)
        settings.OmitXmlDeclaration = True
        settings.NewLineOnAttributes = True

        Using writer As XmlWriter = XmlWriter.Create(output, settings)
            writer.WriteStartElement("items")
            writer.WriteStartElement("item")
            writer.WriteElementString("title", "Element - New Silverlight Toolkit Video")
            writer.WriteElementString("link", "http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls")
            writer.WriteElementString("pubDate", "Thu, 26 Mar 2009 17:13:00 GMT")
            writer.WriteElementString("description", "Element - Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.")
            writer.WriteEndElement()
            writer.WriteEndElement()
            writer.Flush()
        End Using

        txtXML.Text = output.ToString()
    End Sub '   CreateNode

    Private Sub CreateXML()

        Dim output As StringBuilder = New StringBuilder()

        Dim xmlString As XElement = <items>
                                        <item>
                                            <title>"XML New Silverlight Toolkit Video"</title>
                                            <link>"http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"</link>
                                            <pubDate>"Thu, 26 Mar 2009 17:13:00 GMT"</pubDate>
                                            <description>"XML Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page."</description>
                                        </item>
                                    </items>

        Using xReader As XmlReader = XmlReader.Create(New StringReader(xmlString.tostring()))
            Dim settings As XmlWriterSettings = New XmlWriterSettings()
            settings.Indent = False
            settings.OmitXmlDeclaration = True

            Using writer As XmlWriter = XmlWriter.Create(output)
                While (xReader.Read())
                    If (xReader.NodeType = XmlNodeType.Element) Then
                        writer.WriteRaw("\n")
                        writer.WriteStartElement(xReader.Name)
                        writer.WriteAttributes(xReader, False)
                        If (xReader.IsEmptyElement) Then
                            writer.WriteEndElement()
                        ElseIf (xReader.NodeType = XmlNodeType.Text) Then
                            writer.WriteString(xReader.Value)
                        End If

                    ElseIf (xReader.NodeType = XmlNodeType.EndElement) Then

                        If (xReader.Name = "item") Then
                            writer.WriteRaw("\n")
                        End If

                        writer.WriteEndElement()
                    End If
                End While   '
            End Using
        End Using

        txtXML.Text = output.ToString()
    End Sub '   CreateXML

    Private Sub LoadRemoteFile(remoteXmlfile As String)

        Dim c As WebClient = New WebClient()
        AddHandler c.OpenReadCompleted, AddressOf RemoteFileReadandWrite ' OpenReadCompletedEventHandler()
        c.OpenReadAsync(New Uri(remoteXmlfile))
    End Sub '   LoadRemoteFile

    Private Sub RemoteFileReadandWrite(sender As Object, e As OpenReadCompletedEventArgs)

        Dim output As StringBuilder = New StringBuilder()

        If (e.Error Is Nothing) Then

            Dim xReader As XmlReader = XmlReader.Create(e.Result)

            Using writer As XmlWriter = XmlWriter.Create(output)
                writer.WriteRaw("\n")
                writer.WriteStartElement("items")
                xReader.ReadToFollowing("item")

                While (Not xReader.EOF)
                    writer.WriteNode(xReader, False)
                    xReader.ReadToFollowing("item")
                End While   '
            End Using
        End If

        txtXML.Text = output.ToString()
    End Sub '   RemoteFileReadandWrite
End Class   '   XWriter
' End Namespace   '   WorkwithXML
' ..\Project_06\DataWeb\2-WorkwithXML\Pages\2-XWriter.xaml.cs
