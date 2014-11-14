Imports System
Imports System.Windows
Imports System.IO
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Xml
Imports System.Windows.Markup
Imports System.Windows.Browser
Imports System.Windows.Media.Imaging

' Namespace DynamicResource

Partial Public Class PageDynamic
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub AddSmiley(x As Double, y As Double)

        Dim xmlReader As XmlReader

        If (CType(x, Integer) Mod 2 = 0) Then
            xmlReader = XmlReader.Create("files/smiley.xaml")
        Else
            xmlReader = XmlReader.Create("/DynamicResource;component/files/smileypink.xaml")
        End If

        xmlReader.MoveToContent()
        Dim MySmiley As UIElement = CType(XamlReader.Load(xmlReader.ReadOuterXml()), UIElement)
        MySmiley.SetValue(Canvas.TopProperty, y)
        MySmiley.SetValue(Canvas.LeftProperty, x)

        DynamicItemContainer.Children.Add(MySmiley)
    End Sub '   AddSmiley

    Private Sub RemoveSmiley(sender As Object)

        DynamicItemContainer.Children.Remove(CType(sender, UIElement))
    End Sub '   RemoveSmiley

    Private Sub RemoveAllSmiley()

        If (HtmlPage.Window.Confirm("Delete All Smiley?")) Then

            Dim TotalCount As Integer = DynamicItemContainer.Children.Count

            For i As Integer = TotalCount - 1 To 2 Step -1
                DynamicItemContainer.Children.RemoveAt(i)
            Next    '   i
        End If
    End Sub '   RemoveAllSmiley

    Private Sub ReadXamlFromResource(sender As Object, e As Mousebuttoneventargs)

        Dim xmlReader As XmlReader
        xmlReader = XmlReader.Create("/DynamicResource;component/files/smiley.xaml")
        xmlReader.MoveToContent()
        Dim MySmiley As UIElement = CType(XamlReader.Load(xmlReader.ReadOuterXml()), UIElement)
        MySmiley.SetValue(Canvas.TopProperty, 45.0)
        MySmiley.SetValue(Canvas.LeftProperty, 65.0)

        DynamicItemContainer.Children.Clear()
        DynamicItemContainer.Children.Add(MySmiley)
        StatusBar.Text = ToolTipService.GetToolTip(XAMLResource).ToString()
    End Sub '   ReadXamlFromResource

    Private Sub ReadXamlFromFile(sender As Object, e As Mousebuttoneventargs)

        Dim xmlReader As XmlReader

        xmlReader = XmlReader.Create("files/smileypink.xaml")
        xmlReader.MoveToContent()

        Dim MySmiley As UIElement = CType(XamlReader.Load(xmlReader.ReadOuterXml()), UIElement)
        MySmiley.SetValue(Canvas.TopProperty, 45.0)
        MySmiley.SetValue(Canvas.LeftProperty, 65.0)

        DynamicItemContainer.Children.Clear()
        DynamicItemContainer.Children.Add(MySmiley)
        StatusBar.Text = ToolTipService.GetToolTip(XAMLFile).ToString()
    End Sub '   ReadXamlFromFile

    Private Sub UserControl_Loaded(sender As Object, e As Routedeventargs)

        HtmlPage.Plugin.Focus()
        ListDynamic.Focus()
        ListDynamic.SelectedIndex = 0
    End Sub '   UserControl_Loaded

    Private Sub ReadXAMLFromCode(sender As Object, e As Mousebuttoneventargs)

        Dim XamlFromCode As String = ""
        XamlFromCode = "<Canvas xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' x:Name='Smiley' Width='68.6579' Height='65.359' Clip='F1 M 0,0L 68.6579,0L 68.6579,65.359L 0,65.359L 0,0' Canvas.Left='65' Canvas.Top='45'>"
        XamlFromCode = XamlFromCode + "<Canvas x:Name='Group' Width='68.6579' Height='65.359' >"
        XamlFromCode = XamlFromCode + "<Ellipse x:Name='Ellipse' Width='68.6579' Height='65.359' Canvas.Left='0' Canvas.Top='0' Stretch='Fill' StrokeLineJoin='Round' Stroke='#FF000000' Fill='LightGreen'/>"
        XamlFromCode = XamlFromCode + "<Path x:Name='Path' Width='7.04111' Height='6.55237' Canvas.Left='18.2232' Canvas.Top='21.4838' Stretch='Fill' StrokeLineJoin='Round' Stroke='#FF000000' Fill='#FF060725' Data='F1 M 21.7438,21.9838C 23.412,21.9838 24.7643,23.2267 24.7643,24.76C 24.7643,26.2932 23.412,27.5362 21.7438,27.5362C 20.0756,27.5362 18.7232,26.2932 18.7232,24.76C 18.7232,23.2267 20.0756,21.9838 21.7438,21.9838 Z '/>"
        XamlFromCode = XamlFromCode + "<Path x:Name='Path_0' Width='7.0412' Height='6.55249' Canvas.Left='43.6955' Canvas.Top='21.4838' Stretch='Fill' StrokeLineJoin='Round' Stroke='#FF000000' Fill='#FF07081F' Data='F1 M 47.2161,21.9838C 48.8843,21.9838 50.2367,23.2268 50.2367,24.76C 50.2367,26.2933 48.8843,27.5363 47.2161,27.5363C 45.5479,27.5363 44.1955,26.2933 44.1955,24.76C 44.1955,23.2268 45.5478,21.9838 47.2161,21.9838 Z '/>"
        XamlFromCode = XamlFromCode + "<Path x:Name='Path_1' Width='20.6681' Height='7.00052' Canvas.Left='24.1972' Canvas.Top='41.719' Stretch='Fill' StrokeLineJoin='Round' Stroke='#FF1A1D4B' Data='F1 M 44.3652,42.219C 42.745,45.7453 38.952,48.2195 34.5312,48.2195C 30.1104,48.2195 26.3174,45.7453 24.6972,42.219'/>"
        XamlFromCode = XamlFromCode + "</Canvas>"
        XamlFromCode = XamlFromCode + "</Canvas>"
        Dim MySmiley As UIElement = CType(XamlReader.Load(XamlFromCode), UIElement)
        MySmiley.SetValue(Canvas.TopProperty, 45.0)
        MySmiley.SetValue(Canvas.LeftProperty, 65.0)
        DynamicItemContainer.Children.Clear()
        DynamicItemContainer.Children.Add(MySmiley)
        StatusBar.Text = ToolTipService.GetToolTip(XAMLCode).ToString()
    End Sub '   ReadXAMLFromCode

    Private Sub ReadImageFromAssembly(sender As Object, e As Mousebuttoneventargs)

        Dim Img As Image = New Image()
        Img.Source = New BitmapImage(New Uri("files/silverlight.png", UriKind.Relative))
        Img.SetValue(Canvas.TopProperty, 20.0)
        Img.SetValue(Canvas.LeftProperty, 20.0)
        DynamicItemContainer.Children.Clear()
        DynamicItemContainer.Children.Add(Img)
        StatusBar.Text = ToolTipService.GetToolTip(ImageFile).ToString()
    End Sub '   ReadImageFromAssembly

    Private Sub ReadMediaFromFile(sender As Object, e As Mousebuttoneventargs)

        Dim MyMedia As MediaElement = New MediaElement()
        MyMedia.Width = 120.0
        MyMedia.Height = 120.0
        MyMedia.Source = New Uri("files/SilverlightAnimated.wmv", UriKind.Relative)
        MyMedia.SetValue(Canvas.TopProperty, 40.0)
        MyMedia.SetValue(Canvas.LeftProperty, 40.0)
        MyMedia.AutoPlay = True
        DynamicItemContainer.Children.Clear()
        DynamicItemContainer.Children.Add(MyMedia)
        MyMedia.Play()
        StatusBar.Text = ToolTipService.GetToolTip(MediaFile).ToString()
    End Sub '   ReadMediaFromFile

    Private Sub ReadAudioFromFile(sender As Object, e As Mousebuttoneventargs)

        Dim AudioSample As MediaElement = New MediaElement()
        AudioSample.Source = New Uri("files/Audio.wma", UriKind.Relative)
        AudioSample.AutoPlay = True
        DynamicItemContainer.Children.Clear()
        DynamicItemContainer.Children.Add(AudioSample)
        Dim AudioPlaying As TextBlock = New TextBlock()

        AudioPlaying.SetValue(Canvas.TopProperty, 40.0)
        AudioPlaying.SetValue(Canvas.LeftProperty, 40.0)
        AudioPlaying.Text = "Audio Playing \n(Turn Your Speaker On)"
        DynamicItemContainer.Children.Add(AudioPlaying)
        StatusBar.Text = ToolTipService.GetToolTip(AudioFile).ToString()
    End Sub '   ReadAudioFromFile

    Private Sub ReadEmbeddedFonts(sender As Object, e As Mousebuttoneventargs)

        Dim FontStackPanel As StackPanel = New StackPanel()
        FontStackPanel.Orientation = Orientation.Vertical

        Dim ArialText As TextBlock = New TextBlock()
        ArialText.Text = "Arial"
        ArialText.FontSize = 14.0
        ArialText.Margin = New Thickness(1.0)
        ArialText.FontFamily = New FontFamily("Arial")

        Dim ArialBlackText As TextBlock = New TextBlock()
        ArialBlackText.Text = "Arial Black"
        ArialBlackText.FontSize = 14.0
        ArialBlackText.Margin = New Thickness(1.0)
        ArialBlackText.FontFamily = New FontFamily("Arial Black")

        Dim ComicSansMSText As TextBlock = New TextBlock()
        ComicSansMSText.Text = "Comic Sans MS"
        ComicSansMSText.FontSize = 14.0
        ComicSansMSText.Margin = New Thickness(1.0)
        ComicSansMSText.FontFamily = New FontFamily("Comic Sans MS")

        Dim CourierNewText As TextBlock = New TextBlock()
        CourierNewText.Text = "Courier New"
        CourierNewText.FontSize = 14.0
        CourierNewText.Margin = New Thickness(1.0)
        CourierNewText.FontFamily = New FontFamily("Courier New")

        Dim GeorgiaText As TextBlock = New TextBlock()
        GeorgiaText.Text = "Georgia"
        GeorgiaText.FontSize = 14.0
        GeorgiaText.Margin = New Thickness(1.0)
        GeorgiaText.FontFamily = New FontFamily("Georgia")

        Dim LucidaGrandeText As TextBlock = New TextBlock()
        LucidaGrandeText.Text = "Lucida Grande"
        LucidaGrandeText.FontSize = 14.0
        LucidaGrandeText.Margin = New Thickness(1.0)
        LucidaGrandeText.FontFamily = New FontFamily("Lucida Grande")

        Dim LucidaSansUnicodeText As TextBlock = New TextBlock()
        LucidaSansUnicodeText.Text = "Lucida Sans Unicode"
        LucidaSansUnicodeText.FontSize = 14.0
        LucidaSansUnicodeText.Margin = New Thickness(1.0)
        LucidaSansUnicodeText.FontFamily = New FontFamily("Lucida Sans Unicode")

        Dim TimesNewRomanText As TextBlock = New TextBlock()
        TimesNewRomanText.Text = "Times New Roman"
        TimesNewRomanText.FontSize = 14.0
        TimesNewRomanText.Margin = New Thickness(1.0)
        TimesNewRomanText.FontFamily = New FontFamily("Times New Roman")

        Dim TrebuchetMSText As TextBlock = New TextBlock()
        TrebuchetMSText.Text = "Trebuchet MS"
        TrebuchetMSText.FontSize = 14.0
        TrebuchetMSText.Margin = New Thickness(1.0)
        TrebuchetMSText.FontFamily = New FontFamily("Trebuchet MS")

        Dim VerdanaText As TextBlock = New TextBlock()
        VerdanaText.Text = "Verdana"
        VerdanaText.FontSize = 14.0
        VerdanaText.Margin = New Thickness(1.0)
        VerdanaText.FontFamily = New FontFamily("Verdana")

        FontStackPanel.Children.Add(ArialText)
        FontStackPanel.Children.Add(ArialBlackText)
        FontStackPanel.Children.Add(ComicSansMSText)
        FontStackPanel.Children.Add(CourierNewText)
        FontStackPanel.Children.Add(GeorgiaText)
        FontStackPanel.Children.Add(LucidaGrandeText)
        FontStackPanel.Children.Add(LucidaSansUnicodeText)
        FontStackPanel.Children.Add(TimesNewRomanText)
        FontStackPanel.Children.Add(TrebuchetMSText)
        FontStackPanel.Children.Add(VerdanaText)

        FontStackPanel.SetValue(Canvas.TopProperty, 0.0)
        FontStackPanel.SetValue(Canvas.LeftProperty, 0.0)
        DynamicItemContainer.Children.Clear()
        DynamicItemContainer.Children.Add(FontStackPanel)
        StatusBar.Text = ToolTipService.GetToolTip(EmbeddedFont).ToString()
    End Sub '   ReadEmbeddedFonts

    Private Sub ReadFontsFromFile(sender As Object, e As Mousebuttoneventargs)

        Dim FontStackPanel As StackPanel = New StackPanel()
        FontStackPanel.Orientation = Orientation.Vertical
        Dim AllegroText As TextBlock = New TextBlock()
        AllegroText.Text = "Allegro"
        AllegroText.FontSize = 24.0
        AllegroText.Margin = New Thickness(4.0)
        AllegroText.FontFamily = New FontFamily("files/Allegro.TTF#Allegro")

        Dim AngelinaText As TextBlock = New TextBlock()
        AngelinaText.Text = "Angelina"
        AngelinaText.FontSize = 24.0
        AngelinaText.Margin = New Thickness(4.0)
        AngelinaText.FontFamily = New FontFamily("files/Angelina.TTF#Angelina")

        Dim BrocsText As TextBlock = New TextBlock()
        BrocsText.Text = "BrockScript"
        BrocsText.FontSize = 24.0
        BrocsText.Margin = New Thickness(4.0)
        BrocsText.FontFamily = New FontFamily("files/Brocs.TTF#BrockScript")

        Dim FuturaLText As TextBlock = New TextBlock()
        FuturaLText.Text = "Futura Lt BT"
        FuturaLText.FontSize = 24.0
        FuturaLText.Margin = New Thickness(4.0)
        FuturaLText.FontFamily = New FontFamily("files/FuturaL.TTF#Futura Lt BT")

        Dim GlashouseText As TextBlock = New TextBlock()
        GlashouseText.Text = "Glass Houses"
        GlashouseText.FontSize = 24.0
        GlashouseText.Margin = New Thickness(4.0)
        GlashouseText.FontFamily = New FontFamily("files/Glashouse.TTF#Glass Houses")

        FontStackPanel.Children.Add(AllegroText)
        FontStackPanel.Children.Add(AngelinaText)
        FontStackPanel.Children.Add(BrocsText)
        FontStackPanel.Children.Add(FuturaLText)
        FontStackPanel.Children.Add(GlashouseText)

        FontStackPanel.SetValue(Canvas.TopProperty, 0.0)
        FontStackPanel.SetValue(Canvas.LeftProperty, 0.0)
        DynamicItemContainer.Children.Clear()
        DynamicItemContainer.Children.Add(FontStackPanel)
        StatusBar.Text = ToolTipService.GetToolTip(FontFile).ToString()
    End Sub '   ReadFontsFromFile
End Class   '   PageDynamic
' End Namespace 
' ..\RichInternetApplication\3-DynamicResource\PageDynamic.xaml.cs
