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
Imports System.ServiceModel.Syndication

' Namespace RSSandAtom

Partial Public Class Syndication
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        GetFeed("http://silverlight.net/blogs/news/rss.aspx")
    End Sub '   LayoutRoot_Loaded

    Private Sub SelectAction(sender As Object, e As MouseButtonEventArgs)

        Dim t As TextBlock = CType(sender, TextBlock)
        Dim SelectType As String = t.Text

        StatusBar.Text = ToolTipService.GetToolTip(t).ToString()

        FeedList.ItemsSource = Nothing

        If (SelectType = "Load RSS") Then
            GetFeed("http://silverlight.net/blogs/news/rss.aspx")
        ElseIf (SelectType = "Load Atom") Then
            GetFeed("http://silverlight.net/blogs/microsoft/atom.aspx")
        ElseIf (SelectType = "XML to RSS") Then
            Dim s As SyndicationFeedFormatter = GetBlog("rss")
            FeedList.ItemsSource = s.Feed.Items
            StatusInfo.Text = "Rss20FeedFormatter(feed)"
        ElseIf (SelectType = "XML to Atom") Then
            Dim s As SyndicationFeedFormatter = GetBlog("atom")
            FeedList.ItemsSource = s.Feed.Items
            StatusInfo.Text = "Atom10FeedFormatter(feed)"
        End If
    End Sub '   SelectAction

    Sub GetFeed(SyndicationFeed As String)

        Dim c As WebClient = New WebClient()
        AddHandler c.OpenReadCompleted, AddressOf ProcessFeed ' OpenReadCompletedEventHandler()
        c.OpenReadAsync(New Uri(SyndicationFeed))
    End Sub '   GetFeed

    Sub ProcessFeed(sender As Object, e As OpenReadCompletedEventArgs)

        Try
            FeedList.ItemsSource = Nothing

            Dim rdr As XmlReader = XmlReader.Create(e.Result)
            Dim feed As SyndicationFeed = SyndicationFeed.Load(rdr)

            FeedList.ItemsSource = feed.Items
        Catch ex As Exception
            StatusInfo.Text = "Security error !"
        End Try
    End Sub '   ProcessFeed

    Public Function GetBlog(format As String) As SyndicationFeedFormatter

        Dim feed As SyndicationFeed = New SyndicationFeed("Archived News", "Silverlight Blogs and News", New Uri("http://silverlight.net/blogs/news/default.aspx"))

        feed.Description = New TextSyndicationContent("Silverlight News Blog")

        Dim item1 As SyndicationItem = New SyndicationItem(
            "New Silverlight Toolkit Video",
            "Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.",
            New Uri("http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"),
            "ItemOneID",
            DateTime.Now)

        Dim item2 As SyndicationItem = New SyndicationItem(
            "Seven New Community Gallery Entries",
            "Play the Silverlight Gobang Game, practice Creating Particle Effects in Silverlight, dance with a 3D Silver Mouse, and more Not  Find inspiration and upload your Silverlight projects to share with the community in the Gallery.",
            New Uri("http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2"),
            "ItemTwoID",
             DateTime.Now)

        Dim item3 As SyndicationItem = New SyndicationItem(
            "16 New Silverlight Showcases",
            "Improve your tonal memory with TwinNotes, create your own bracelets using Brighton’s Interactive Charm Builder, browse the selection of movies at Ramp DVD Store, and more in the Silverlight Showcase.",
            New Uri("http://silverlight.net/Showcase"),
            "ItemThreeID",
             DateTime.Now)

        Dim items As List(Of SyndicationItem) = New List(Of SyndicationItem)()

        items.Add(item1)
        items.Add(item2)
        items.Add(item3)

        feed.Items = items

        If (format = "rss") Then
            Return New Rss20FeedFormatter(feed)
        ElseIf (format = "atom") Then
            Return New Atom10FeedFormatter(feed)
        Else
            Return Nothing
        End If
    End Function
End Class   '   Syndication
' End Namespace   '   RSSandAtom
' ..\Project_06\DataWeb\4-RSSandAtom\Syndication.xaml.cs
