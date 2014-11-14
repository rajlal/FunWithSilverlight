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

' Namespace WorkwithXML

Partial Public Class XReader
    Inherits UserControl

    Dim MyNewsList As NewsList = New NewsList()
    Dim FileSelectType As String = "Embedded XML"
    WithEvents CurrentSelectedNewsItem As SelectedNewsItem = New SelectedNewsItem()

    Public Sub New()

        InitializeComponent()

        CurrentSelectedNewsItem.Desc = "No Items Selected"
        CurrentSelectedNewsItem.sLink = "http://silverlightfun.com"
        stackNewsDetails.DataContext = CurrentSelectedNewsItem
    End Sub '   New

    Private Sub UpdateDisplay()

        myDisplayList.Items.Clear()

        For Each s As NewsItem In MyNewsList
            Dim lbi As ListBoxItem = New ListBoxItem()

            Dim sp As StackPanel = New StackPanel()
            Dim si As TextBlock = New TextBlock()

            si.Text = " " + s.Title.Substring(0, 17) + ".."
            si.FontSize = 11
            si.FontFamily = New FontFamily("Verdana")

            Dim tt As ToolTip = New ToolTip()

            tt.Content = s.Title
            ToolTipService.SetToolTip(si, tt)

            Dim tb As TextBlock = New TextBlock()

            tb.Text = " " + s.sDate.Substring(0, 16)
            tb.FontSize = 8
            tb.Foreground = New SolidColorBrush(Colors.Gray)
            sp.Orientation = Orientation.Vertical
            sp.Children.Add(si)
            sp.Children.Add(tb)
            lbi.Content = sp
            AddHandler lbi.MouseLeftButtonUp, AddressOf SetItem '(MouseButtonEventHandler(Of )
            myDisplayList.Items.Add(lbi)
        Next

        myDisplayList.SelectedIndex = 0
        CurrentSelectedNewsItem.Desc = MyNewsList(0).Desc
        CurrentSelectedNewsItem.sLink = MyNewsList(0).sLink
    End Sub '   UpdateDisplay

    Private Sub SetItem(sender As Object, e As System.Windows.Input.MouseButtonEventArgs)

        Dim lbi As ListBoxItem = CType(sender, ListBoxItem)
        Dim sp As StackPanel = CType(lbi.Content, StackPanel)
        Dim sText As TextBlock = CType(sp.Children(0), TextBlock)
        Dim tt As ToolTip = CType(ToolTipService.GetToolTip(sText), ToolTip)
        Dim titleText As String = tt.Content.ToString()

        For Each n As NewsItem In MyNewsList
            If (n.Title.ToLower() = titleText.ToLower()) Then
                CurrentSelectedNewsItem.Desc = n.Desc
                CurrentSelectedNewsItem.sLink = n.sLink
            End If
        Next    '   n
    End Sub '   SetItem

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        LoadXAPFile()
    End Sub '   LayoutRoot_Loaded

    Private Sub FileSelect(sender As Object, e As MouseButtonEventArgs)

        Dim t As TextBlock = CType(sender, TextBlock)

        FileSelectType = t.Text
        StatusBar.Text = "Selected: " + ToolTipService.GetToolTip(t).ToString()

        If (FileSelectType = "Embedded XML") Then
            LoadXAPFile()
            StatusInfo.Text = "SilverlightNews.xml"
        ElseIf (FileSelectType = "Local XML") Then
            LoadLocalFile("Pages/Files/LocalSilverlightNews.xml")
            StatusInfo.Text = "Pages/Files/LocalSilverlightNews.xml"
        ElseIf (FileSelectType = "XHTML") Then
            LoadXHTMLFile("Pages/Files/SilverlightNews.htm")
            StatusInfo.Text = "Pages/Files/SilverlightNews.htm"
        ElseIf (FileSelectType = "Remote XML") Then
            LoadRemoteFile("http://silverlightfun.com/Samples/Chapter-05/WebSilverlightNews.xml")
            StatusInfo.Text = "News.xml @ silverlightfun.com"
        End If
    End Sub '   FileSelect

    Private Sub LoadXAPFile()

        MyNewsList.Clear()

        Dim u As Uri = New Uri("Pages/Files/SilverlightNews.xml", UriKind.Relative)

        Dim xReader As XmlReader = XmlReader.Create("Pages/Files/SilverlightNews.xml")

        xReader.ReadToFollowing("item")

        While (Not xReader.EOF)

            Dim n As NewsItem = New NewsItem()

            xReader.ReadToFollowing("title")
            n.Title = xReader.ReadElementContentAsString()
            xReader.ReadToFollowing("link")
            n.sLink = xReader.ReadElementContentAsString()
            xReader.ReadToFollowing("pubDate")
            n.sDate = xReader.ReadElementContentAsString()
            xReader.ReadToFollowing("description")
            n.Desc = xReader.ReadElementContentAsString()

            MyNewsList.Add(n)
            '  Moves the reader back to the element node.
            xReader.ReadToFollowing("item")
        End While   '

        UpdateDisplay()
    End Sub '   LoadXAPFile

    Private Sub LoadLocalFile(localxmlfile As String)

        Dim xmlClient As WebClient = New WebClient()

        AddHandler xmlClient.DownloadStringCompleted, AddressOf LocalFileLoaded ' ()DownloadStringCompletedEventHandler
        xmlClient.DownloadStringAsync(New Uri(localxmlfile, UriKind.RelativeOrAbsolute))
    End Sub '   LoadLocalFile

    Private Sub LocalFileLoaded(sender As Object, e As DownloadStringCompletedEventArgs)

        If (e.Error Is Nothing) Then

            Dim xmlData As String = e.Result
            Dim xReader As XmlReader = XmlReader.Create(New StringReader(xmlData))

            MyNewsList.Clear()

            xReader.ReadToFollowing("item")

            While (Not xReader.EOF)

                Dim n As NewsItem = New NewsItem()

                xReader.ReadToFollowing("title")
                n.Title = xReader.ReadElementContentAsString()
                xReader.ReadToFollowing("link")
                n.sLink = xReader.ReadElementContentAsString()
                xReader.ReadToFollowing("pubDate")
                n.sDate = xReader.ReadElementContentAsString()
                xReader.ReadToFollowing("description")
                n.Desc = xReader.ReadElementContentAsString()
                MyNewsList.Add(n)
                '  Moves the reader back to the element node.
                xReader.ReadToFollowing("item")
            End While   '

            UpdateDisplay()
        End If
    End Sub '   LocalFileLoaded

    Private Sub LoadXHTMLFile(localXhtmlFile As String)

        MyNewsList.Clear()

        Dim settings As XmlReaderSettings = New XmlReaderSettings()
        settings.DtdProcessing = DtdProcessing.Parse
        settings.XmlResolver = New XmlPreloadedResolver(New XmlXapResolver(), XmlKnownDtds.Xhtml10)

        Dim xReader As XmlReader = XmlReader.Create(localXhtmlFile, settings)

        xReader.ReadToFollowing("item")

        While (Not xReader.EOF)

            Dim n As NewsItem = New NewsItem()

            xReader.ReadToFollowing("title")
            n.Title = xReader.ReadElementContentAsString()
            xReader.ReadToFollowing("link")
            n.sLink = xReader.ReadElementContentAsString()
            xReader.ReadToFollowing("pubDate")
            n.sDate = xReader.ReadElementContentAsString()
            xReader.ReadToFollowing("description")
            n.Desc = xReader.ReadElementContentAsString()

            MyNewsList.Add(n)
            '  Moves the reader back to the element node.
            xReader.ReadToFollowing("item")
        End While   '

        UpdateDisplay()
    End Sub '   LoadXHTMLFile

    Private Sub LoadRemoteFile(remoteXmlfile As String)

        Dim c As WebClient = New WebClient()
        AddHandler c.OpenReadCompleted, AddressOf RemoteFileLoaded 'OpenReadCompletedEventHandler()
        c.OpenReadAsync(New Uri(remoteXmlfile))
    End Sub '   LoadRemoteFile

    Private Sub RemoteFileLoaded(sender As Object, e As OpenReadCompletedEventArgs)

        If (e.Error Is Nothing) Then

            Dim xReader As XmlReader = XmlReader.Create(e.Result)

            MyNewsList.Clear()
            xReader.ReadToFollowing("item")

            While (Not xReader.EOF)

                Dim n As NewsItem = New NewsItem()

                xReader.ReadToFollowing("title")
                n.Title = xReader.ReadElementContentAsString()
                xReader.ReadToFollowing("link")
                n.sLink = xReader.ReadElementContentAsString()
                xReader.ReadToFollowing("pubDate")
                n.sDate = xReader.ReadElementContentAsString()
                xReader.ReadToFollowing("description")
                n.Desc = xReader.ReadElementContentAsString()
                MyNewsList.Add(n)
                '  Moves the reader back to the element node.
                xReader.ReadToFollowing("item")
            End While   '

            UpdateDisplay()
        End If
    End Sub '   RemoteFileLoaded
End Class   '   XReader

Public Class NewsList
    Inherits List(Of NewsItem)

    Dim si As NewsItem

    Public Property Val As NewsItem

    Public Sub New()
    End Sub '   New
End Class   '   NewsList

Public Class NewsItem

    Private _Title As String
    Private _Link As String
    Private _Date As String
    Private _Desc As String

    Public Property Title() As String
        Get

            Return _Title
        End Get
        Set(value As String)

            _Title = value
        End Set
    End Property

    Public Property Desc() As String
        Get

            Return _Desc
        End Get
        Set(value As String)

            _Desc = value
        End Set
    End Property

    Public Property sDate() As String
        Get

            Return _Date
        End Get
        Set(value As String)

            _Date = value
        End Set
    End Property

    Public Property sLink() As String
        Get

            Return _Link
        End Get
        Set(value As String)

            _Link = value
        End Set
    End Property

    Public Sub New()
    End Sub '   New

    Public Sub New(title As String, slink As String, sDate As String, desc As String)
        Me._Title = title
        Me._Link = slink
        Me._Date = sDate
        Me._Desc = desc
    End Sub '   New
End Class   '   NewsItem

Public Class SelectedNewsItem
    Inherits NewsItem
    Implements INotifyPropertyChanged
    'Error	1	Class 'SelectedNewsItem' must implement 'Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs)' for interface 'System.ComponentModel.INotifyPropertyChanged'.

    'Private _Desc As String
    'Private _Link As String
    '  Declare the PropertyChanged event.
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    '  Create the property that will be the source of the binding.

    Public Overloads Property Desc() As String
        Get
            Return MyBase.Desc
        End Get
        Set(value As String)

            MyBase.Desc = value
            NotifyPropertyChanged("Desc")
        End Set
    End Property

    Public Overloads Property sLink() As String
        Get
            Return MyBase.sLink
        End Get
        Set(value As String)

            MyBase.sLink = value
            NotifyPropertyChanged("Link")
        End Set
    End Property

    Public Sub NotifyPropertyChanged(propertyName As String)


        'If (PropertyChanged IsNot Nothing) Then
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        'End If
    End Sub '   NotifyPropertyChanged
End Class   '   SelectedNewsItem
' End Namespace   '   WorkwithXML
' ..\Project_06\DataWeb\2-WorkwithXML\Pages\1-XReader.xaml.cs
