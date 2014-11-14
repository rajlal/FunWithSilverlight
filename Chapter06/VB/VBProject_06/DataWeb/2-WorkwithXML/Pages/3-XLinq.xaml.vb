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
Imports System.Windows.Markup
Imports Microsoft.VisualBasic

' Namespace WorkwithXML

Partial Public Class XLinq
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        stackDocument.Visibility = Visibility.Collapsed
        stackOthers.Visibility = Visibility.Collapsed
        stackElement.Visibility = Visibility.Visible
    End Sub '   LayoutRoot_Loaded

    Private Sub CreateSelect(sender As Object, e As MouseButtonEventArgs)

        Dim t As TextBlock = CType(sender, TextBlock)
        Dim FileSelectType As String = t.Text

        StatusBar.Text = ToolTipService.GetToolTip(t).ToString()

        If (FileSelectType = "XElement") Then
            stackDocument.Visibility = Visibility.Collapsed
            stackOthers.Visibility = Visibility.Collapsed
            stackElement.Visibility = Visibility.Visible
        ElseIf (FileSelectType = "XDocument") Then
            stackElement.Visibility = Visibility.Collapsed
            stackOthers.Visibility = Visibility.Collapsed
            stackDocument.Visibility = Visibility.Visible

        ElseIf (FileSelectType = "XOthers") Then
            stackOthers.Visibility = Visibility.Visible
            stackElement.Visibility = Visibility.Collapsed
            stackDocument.Visibility = Visibility.Collapsed
        End If
    End Sub '   CreateSelect

    Private Sub CreateXElement(sender As Object, e As MouseButtonEventArgs)

        Dim t As TextBlock = CType(sender, TextBlock)
        Dim SelectSubType As String = t.Text

        StatusInfo.Text = ToolTipService.GetToolTip(t).ToString()

        If (SelectSubType = "Create XML") Then
            XElementCreate()
        ElseIf (SelectSubType = "Load XML") Then
            XElementLoadXML()
        ElseIf (SelectSubType = "Create XAML") Then
            XElementLoadXAML()
        ElseIf (SelectSubType = "Load XAML") Then
            XElementLoadXAMLFile()
        End If
    End Sub '   CreateXElement

    Private Sub XElementCreate()

        canvasXElement.Visibility = Visibility.Collapsed
        txtXElement.Visibility = Visibility.Visible

        Dim output As StringBuilder = New StringBuilder()

        Dim xmlTree As XElement = New XElement("item",
                New XElement("title", "Local New Silverlight Toolkit Video"),
                New XElement("link", "http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"),
                New XElement("pubDate", "Thu, 26 Mar 2009 17:13:00 GMT"),
                New XElement("description", "Local Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.")
                )

        output.Append(xmlTree)
        txtXElement.Text = output.ToString()
    End Sub '   XElementCreate

    Private Sub XElementLoadXML()

        canvasXElement.Visibility = Visibility.Collapsed
        txtXElement.Visibility = Visibility.Visible

        Dim output As StringBuilder = New StringBuilder()

        Dim u As Uri = New Uri("Pages/Files/SilverlightNews.xml", UriKind.Relative)

        Dim xReader As XmlReader = XmlReader.Create("Pages/Files/SilverlightNews.xml")

        Dim xmlTree As XElement = XElement.Load(xReader)
        output.Append(xmlTree)
        txtXElement.Text = output.ToString()
    End Sub '   XElementLoadXML

    Private Sub XElementLoadXAML()

        txtXElement.Visibility = Visibility.Collapsed
        canvasXElement.Visibility = Visibility.Visible

        Dim smileyElement As XElement =
                        <Canvas Margin='60,40,40,40' xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' x:Name='Smiley' Width='68.6579' Height='65.359' Clip='F1 M 0,0L 68.6579,0L 68.6579,65.359L 0,65.359L 0,0'>
                            <Canvas Width='68.6579' Height='65.359' Canvas.Left='0' Canvas.Top='0'>
                                <Ellipse Width='68.6579' Height='65.359' Canvas.Left='0' Canvas.Top='0' Stretch='Fill' StrokeLineJoin='Round' Stroke='#FF000000' Fill='#FFF7FF08'/>
                                <Path Width='7.04111' Height='6.55237' Canvas.Left='18.2232' Canvas.Top='21.4838' Stretch='Fill' StrokeLineJoin='Round' Stroke='#FF000000' Fill='#FF060725' Data='F1 M 21.7438,21.9838C 23.412,21.9838 24.7643,23.2267 24.7643,24.76C 24.7643,26.2932 23.412,27.5362 21.7438,27.5362C 20.0756,27.5362 18.7232,26.2932 18.7232,24.76C 18.7232,23.2267 20.0756,21.9838 21.7438,21.9838 Z '/>
                                <Path Width='7.0412' Height='6.55249' Canvas.Left='43.6955' Canvas.Top='21.4838' Stretch='Fill' StrokeLineJoin='Round' Stroke='#FF000000' Fill='#FF07081F' Data='F1 M 47.2161,21.9838C 48.8843,21.9838 50.2367,23.2268 50.2367,24.76C 50.2367,26.2933 48.8843,27.5363 47.2161,27.5363C 45.5479,27.5363 44.1955,26.2933 44.1955,24.76C 44.1955,23.2268 45.5478,21.9838 47.2161,21.9838 Z '/>
                                <Path Width='20.6681' Height='7.00052' Canvas.Left='24.1972' Canvas.Top='41.719' Stretch='Fill' StrokeLineJoin='Round' Stroke='#FF1A1D4B' Data='F1 M 44.3652,42.219C 42.745,45.7453 38.952,48.2195 34.5312,48.2195C 30.1104,48.2195 26.3174,45.7453 24.6972,42.219'/>
                            </Canvas>
                        </Canvas>

        Dim MySmiley As UIElement = CType(XamlReader.Load(smileyElement.ToString()), UIElement)

        canvasXElement.Children.Add(MySmiley)
    End Sub '   XElementLoadXAML

    Private Sub XElementLoadXAMLFile()

        canvasXElement.Visibility = Visibility.Visible
        txtXElement.Visibility = Visibility.Collapsed

        Dim xReader As XmlReader

        xReader = XmlReader.Create("Pages/files/smiley.xaml")
        xReader.MoveToContent()

        Dim MySmiley As UIElement = CType(XamlReader.Load(xReader.ReadOuterXml()), UIElement)

        canvasXElement.Children.Add(MySmiley)
    End Sub '   XElementLoadXAMLFile

    Private Sub CreateXDocument(sender As Object, e As MouseButtonEventArgs)

        Dim t As TextBlock = CType(sender, TextBlock)
        Dim SelectSubType As String = t.Text

        StatusInfo.Text = ToolTipService.GetToolTip(t).ToString()

        If (SelectSubType = "Create XML") Then
            XDocumentCreate()
        ElseIf (SelectSubType = "Load XML") Then
            XDocumentLoadXML()
        ElseIf (SelectSubType = "Remote XML") Then
            XDocumentLoadRemoteXML()
        End If
    End Sub '   CreateXDocument

    Private Sub XDocumentCreate()

        Dim output As StringBuilder = New StringBuilder()

        Dim srcTree As XDocument = New XDocument(
                New XDeclaration("1.0", "utf-8", "yes"),
                New XComment("Sample Silverlight Blog Item"),
                New XElement("items",
                    New XElement("item",
                        New XElement("title", "XAP New Silverlight Toolkit Video"),
                        New XElement("link", "http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"),
                        New XElement("pubDate", "Thu, 26 Mar 2009 17:13:00 GMT"),
                        New XElement("description", "XAP Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.")
                        )
                    )
                )

        output.Append(srcTree.Declaration)
        output.Append(Environment.NewLine)
        output.Append(srcTree)
        txtXDocument.Text = output.ToString()
    End Sub '   XDocumentCreate

    Private Sub XDocumentLoadXML()

        Dim output As StringBuilder = New StringBuilder()

        Dim str As String = _
        "<?xml version='1.0' encoding='utf-8'?> " & vbCrLf &
        "<!--- Sample Silverlight Blog Items  -->" & vbcrlf &
        "<items>" & vbcrlf &
        "    <title>Archived News</title>" & vbcrlf &
        "    <link>http://silverlight.net/blogs/news/default.aspx</link>" & vbcrlf &
        "    <description>Silverlight News Blog</description>" & vbcrlf &
        "    <item>" & vbcrlf &
        "        <title>XAP New Silverlight Toolkit Video</title>" & vbcrlf &
        "        <link>http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls</link>" & vbcrlf &
        "        <pubDate>Thu, 26 Mar 2009 17:13:00 GMT</pubDate>" & vbcrlf &
        "        <description>XAP Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.</description>" & vbcrlf &
        "    </item>" & vbcrlf &
        "</items>"

        Dim doc As XDocument = XDocument.Parse(str)
        output.Append(doc)
        output.Append(Environment.NewLine)
        txtXDocument.Text = output.ToString()
    End Sub '   XDocumentLoadXML

    Private Sub XDocumentLoadRemoteXML()

        Dim c As WebClient = New WebClient()
        AddHandler c.OpenReadCompleted, AddressOf RemoteFileLoaded ' OpenReadCompletedEventHandler()
        c.OpenReadAsync(New Uri("http://silverlightfun.com/Samples/Chapter-05/WebSilverlightNews.xml"))
    End Sub '   XDocumentLoadRemoteXML

    Private Sub RemoteFileLoaded(sender As Object, e As OpenReadCompletedEventArgs)

        Dim output As StringBuilder = New StringBuilder()

        If (e.Error Is Nothing) Then

            Dim xReader As XmlReader = XmlReader.Create(e.Result)

            Dim doc As XDocument = XDocument.Load(xReader)
            output.Append(doc)
            output.Append(Environment.NewLine)
            txtXDocument.Text = output.ToString()
        End If
    End Sub '   RemoteFileLoaded

    Private Sub CreateXOthers(sender As Object, e As MouseButtonEventArgs)

        Dim t As TextBlock = CType(sender, TextBlock)
        Dim SelectSubType As String = t.Text

        StatusInfo.Text = ToolTipService.GetToolTip(t).ToString()

        If (SelectSubType = "XAttribute") Then
            CreateXAttribute()
        ElseIf (SelectSubType = "XComment") Then
            CreateXComment()
        ElseIf (SelectSubType = "XDocumentType") Then
            CreateXDocumentType()
        ElseIf (SelectSubType = "XProcessingInst") Then
            CreateXProcessingInstructions()
        ElseIf (SelectSubType = "XCData") Then
            CreateXCData()
        End If
    End Sub '   CreateXOthers

    Private Sub CreateXAttribute()

        Dim output As StringBuilder = New StringBuilder()

        Dim xmlTree As XElement = New XElement("item",
                   New XAttribute("flagread", "0"),
                   New XElement("title", "Local New Silverlight Toolkit Video"),
                   New XElement("link", "http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"),
                   New XElement("pubDate", "Thu, 26 Mar 2009 17:13:00 GMT"),
                   New XElement("description", "Local Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.")
                   )

        output.Append("Attribute Added: ")
        output.Append(xmlTree.FirstAttribute)
        output.Append(Environment.NewLine)
        output.Append(xmlTree)
        output.Append(Environment.NewLine)

        txtXMLOthers.Text = output.ToString()
    End Sub '   CreateXAttribute

    Private Sub CreateXComment()

        Dim output As StringBuilder = New StringBuilder()

        Dim xmlTree As XElement = New XElement("item",
           New XComment("This is first item"),
           New XElement("title", "Local New Silverlight Toolkit Video"),
           New XElement("link", "http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"),
           New XElement("pubDate", "Thu, 26 Mar 2009 17:13:00 GMT"),
           New XElement("description", "Local Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.")
           )
        output.Append("Comment Added: This is first item")
        output.Append(Environment.NewLine)
        output.Append(xmlTree)
        output.Append(Environment.NewLine)
        txtXMLOthers.Text = output.ToString()
    End Sub '   CreateXComment

    Private Sub CreateXDocumentType()

        Dim internalSubset As String = "<!ELEMENT items (item+)> " & vbcrlf &
                                          "<!ELEMENT item (title, link,pubDate,description)> " & vbcrlf &
                                          "<!ELEMENT title (#PCDATA)> " & vbcrlf &
                                          "<!ELEMENT link (#PCDATA)> " & vbcrlf &
                                          "<!ELEMENT pubDate (#PCDATA)>"

        Dim output As StringBuilder = New StringBuilder()

        Dim doc As XDocument = New XDocument(
                    New XDocumentType("items", Nothing, Nothing, internalSubset),
                    New XElement("items",
                    New XElement("item",
                                   New XElement("title", "Local New Silverlight Toolkit Video"),
                                   New XElement("link", "http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"),
                                   New XElement("pubDate", "Thu, 26 Mar 2009 17:13:00 GMT"),
                                   New XElement("description", "Local Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.")
                                )
                            )
                    )
        doc.Declaration = New XDeclaration("1.0", "utf-8", "true")
        output.Append(doc)
        output.Append(Environment.NewLine)
        txtXMLOthers.Text = output.ToString()
    End Sub '   CreateXDocumentType

    Private Sub CreateXProcessingInstructions()

        Dim output As StringBuilder = New StringBuilder()
        Dim target As String = "xml-stylesheet"
        Dim data As String = "href=""style.css"" title=""Compact"" type=""text/css"""

        Dim doc As XDocument = New XDocument(
        New XProcessingInstruction(target, data),
                New XElement("items",
                    New XElement("item",
                                   New XElement("title", "Local New Silverlight Toolkit Video"),
                                   New XElement("link", "http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"),
                                   New XElement("pubDate", "Thu, 26 Mar 2009 17:13:00 GMT"),
                                   New XElement("description", "Local Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.")
                                )
                            )
                    )
        doc.Declaration = New XDeclaration("1.0", "utf-8", "true")
        output.Append(doc)
        output.Append(Environment.NewLine)
        txtXMLOthers.Text = output.ToString()
    End Sub '   CreateXProcessingInstructions

    Private Sub CreateXCData()

        Dim output As StringBuilder = New StringBuilder()

        Dim xmlTree As XElement = New XElement("item",
                   New XElement("title", "Local New Silverlight Toolkit Video"),
                   New XElement("link", "http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"),
                   New XElement("pubDate", "Thu, 26 Mar 2009 17:13:00 GMT"),
                   New XElement("description", "Local Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page."),
                   New XElement("htmldescription", New XCData("<p>Todd Miranda demonstrates how to use <b>Silverlight Themes</b>. <br>Watch this and other Silverlight videos on the Learn page.</p>"))
                )
        output.Append("Attribute Added: ")
        output.Append(xmlTree.FirstAttribute)
        output.Append(Environment.NewLine)
        output.Append(xmlTree)
        output.Append(Environment.NewLine)

        txtXMLOthers.Text = output.ToString()
    End Sub '   CreateXCData
End Class   '   XLinq
' End Namespace   '   WorkwithXML
' ..\Project_06\DataWeb\2-WorkwithXML\Pages\3-XLinq.xaml.cs
