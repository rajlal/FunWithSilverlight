using System;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using System.Windows.Markup;
using System.Windows.Browser;
using System.Windows.Media.Imaging;

namespace DynamicResource
{
    public partial class PageDynamic : UserControl
    {
        public PageDynamic()
        {
            InitializeComponent();
        }
        private void AddSmiley(double x, double y)
        {
            XmlReader xmlReader;

            if ((int)x % 2 == 0)
                xmlReader = XmlReader.Create("files/smiley.xaml");
            else
                xmlReader = XmlReader.Create("/DynamicResource;component/files/smileypink.xaml");

            xmlReader.MoveToContent();
            UIElement MySmiley = (UIElement)XamlReader.Load(xmlReader.ReadOuterXml());
            MySmiley.SetValue(Canvas.TopProperty, y);
            MySmiley.SetValue(Canvas.LeftProperty, x);
           
            DynamicItemContainer.Children.Add(MySmiley);
        }     
        private void RemoveSmiley(Object sender)
        {
            DynamicItemContainer.Children.Remove((UIElement)sender);
        }
        private void RemoveAllSmiley()
        {
            if (HtmlPage.Window.Confirm("Delete All Smiley?"))
            {
                int TotalCount = DynamicItemContainer.Children.Count;
                for (int i = TotalCount - 1; i > 1; i--)
                {
                    DynamicItemContainer.Children.RemoveAt(i);
                }
            }
        }
        private void ReadXamlFromResource(object sender, MouseButtonEventArgs e)
        {
            XmlReader xmlReader;
            xmlReader = XmlReader.Create("/DynamicResource;component/files/smiley.xaml");
            xmlReader.MoveToContent();
            UIElement MySmiley = (UIElement)XamlReader.Load(xmlReader.ReadOuterXml());
            MySmiley.SetValue(Canvas.TopProperty, 45.0);
            MySmiley.SetValue(Canvas.LeftProperty, 65.0);

            DynamicItemContainer.Children.Clear();
            DynamicItemContainer.Children.Add(MySmiley);
            StatusBar.Text = ToolTipService.GetToolTip(XAMLResource).ToString();
        }
        private void ReadXamlFromFile(object sender, MouseButtonEventArgs e)
        {
            XmlReader xmlReader;

            xmlReader = XmlReader.Create("files/smileypink.xaml");
            xmlReader.MoveToContent();

            UIElement MySmiley = (UIElement)XamlReader.Load(xmlReader.ReadOuterXml());
            MySmiley.SetValue(Canvas.TopProperty, 45.0);
            MySmiley.SetValue(Canvas.LeftProperty, 65.0);

            DynamicItemContainer.Children.Clear();
            DynamicItemContainer.Children.Add(MySmiley);
            StatusBar.Text = ToolTipService.GetToolTip(XAMLFile).ToString();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            HtmlPage.Plugin.Focus();
            ListDynamic.Focus();
            ListDynamic.SelectedIndex = 0;
        }
        private void ReadXAMLFromCode(object sender, MouseButtonEventArgs e)
        {
            string XamlCode = "";
            XamlCode = "<Canvas xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' x:Name='Smiley' Width='68.6579' Height='65.359' Clip='F1 M 0,0L 68.6579,0L 68.6579,65.359L 0,65.359L 0,0' Canvas.Left='65' Canvas.Top='45'>";
            XamlCode = XamlCode + "<Canvas x:Name='Group' Width='68.6579' Height='65.359' >";
            XamlCode = XamlCode + "<Ellipse x:Name='Ellipse' Width='68.6579' Height='65.359' Canvas.Left='0' Canvas.Top='0' Stretch='Fill' StrokeLineJoin='Round' Stroke='#FF000000' Fill='LightGreen'/>";
            XamlCode = XamlCode + "<Path x:Name='Path' Width='7.04111' Height='6.55237' Canvas.Left='18.2232' Canvas.Top='21.4838' Stretch='Fill' StrokeLineJoin='Round' Stroke='#FF000000' Fill='#FF060725' Data='F1 M 21.7438,21.9838C 23.412,21.9838 24.7643,23.2267 24.7643,24.76C 24.7643,26.2932 23.412,27.5362 21.7438,27.5362C 20.0756,27.5362 18.7232,26.2932 18.7232,24.76C 18.7232,23.2267 20.0756,21.9838 21.7438,21.9838 Z '/>";
            XamlCode = XamlCode + "<Path x:Name='Path_0' Width='7.0412' Height='6.55249' Canvas.Left='43.6955' Canvas.Top='21.4838' Stretch='Fill' StrokeLineJoin='Round' Stroke='#FF000000' Fill='#FF07081F' Data='F1 M 47.2161,21.9838C 48.8843,21.9838 50.2367,23.2268 50.2367,24.76C 50.2367,26.2933 48.8843,27.5363 47.2161,27.5363C 45.5479,27.5363 44.1955,26.2933 44.1955,24.76C 44.1955,23.2268 45.5478,21.9838 47.2161,21.9838 Z '/>";
            XamlCode = XamlCode + "<Path x:Name='Path_1' Width='20.6681' Height='7.00052' Canvas.Left='24.1972' Canvas.Top='41.719' Stretch='Fill' StrokeLineJoin='Round' Stroke='#FF1A1D4B' Data='F1 M 44.3652,42.219C 42.745,45.7453 38.952,48.2195 34.5312,48.2195C 30.1104,48.2195 26.3174,45.7453 24.6972,42.219'/>";
            XamlCode = XamlCode + "</Canvas>";
            XamlCode = XamlCode + "</Canvas>";
            UIElement MySmiley = (UIElement)XamlReader.Load(XamlCode);
            MySmiley.SetValue(Canvas.TopProperty, 45.0);
            MySmiley.SetValue(Canvas.LeftProperty, 65.0);
            DynamicItemContainer.Children.Clear();
            DynamicItemContainer.Children.Add(MySmiley);
            StatusBar.Text = ToolTipService.GetToolTip(XAMLCode).ToString();
        }
        private void ReadImageFromAssembly(object sender, MouseButtonEventArgs e)
        {
            Image Img = new Image();
            Img.Source =new BitmapImage(new Uri("files/silverlight.png", UriKind.Relative));
            Img.SetValue(Canvas.TopProperty, 20.0);
            Img.SetValue(Canvas.LeftProperty, 20.0);
            DynamicItemContainer.Children.Clear();
            DynamicItemContainer.Children.Add(Img);
            StatusBar.Text = ToolTipService.GetToolTip(ImageFile).ToString();
        }
        private void ReadMediaFromFile(object sender, MouseButtonEventArgs e)
        {
            MediaElement MyMedia = new MediaElement();
            MyMedia.Width = 120.0;
            MyMedia.Height = 120.0;
            MyMedia.Source = new Uri("files/SilverlightAnimated.wmv", UriKind.Relative);
            MyMedia.SetValue(Canvas.TopProperty, 40.0);
            MyMedia.SetValue(Canvas.LeftProperty, 40.0);
            MyMedia.AutoPlay = true;
            DynamicItemContainer.Children.Clear();
            DynamicItemContainer.Children.Add(MyMedia);
            MyMedia.Play();
            StatusBar.Text = ToolTipService.GetToolTip(MediaFile).ToString();
        }
        private void ReadAudioFromFile(object sender, MouseButtonEventArgs e)
        {
            MediaElement AudioSample = new MediaElement();
            AudioSample.Source = new Uri("files/Audio.wma", UriKind.Relative);
            AudioSample.AutoPlay = true;
            DynamicItemContainer.Children.Clear();
            DynamicItemContainer.Children.Add(AudioSample);
            TextBlock AudioPlaying = new TextBlock();

            AudioPlaying.SetValue(Canvas.TopProperty, 40.0);
            AudioPlaying.SetValue(Canvas.LeftProperty, 40.0);
            AudioPlaying.Text = "Audio Playing \n(Turn Your Speaker On)";
            DynamicItemContainer.Children.Add(AudioPlaying);
            StatusBar.Text = ToolTipService.GetToolTip(AudioFile).ToString();
        }
        private void ReadEmbeddedFonts(object sender, MouseButtonEventArgs e)
        {
             StackPanel FontStackPanel = new StackPanel(); 
             FontStackPanel.Orientation= Orientation.Vertical;

             TextBlock ArialText = new TextBlock();
             ArialText.Text ="Arial";
             ArialText.FontSize = 14.0;
             ArialText.Margin = new Thickness(1.0);
             ArialText.FontFamily = new FontFamily("Arial");

             TextBlock ArialBlackText = new TextBlock();
             ArialBlackText.Text ="Arial Black";
             ArialBlackText.FontSize = 14.0;
             ArialBlackText.Margin = new Thickness(1.0); 
             ArialBlackText.FontFamily = new FontFamily("Arial Black");

             TextBlock ComicSansMSText = new TextBlock();
             ComicSansMSText.Text ="Comic Sans MS";
             ComicSansMSText.FontSize = 14.0;
             ComicSansMSText.Margin = new Thickness(1.0);
             ComicSansMSText.FontFamily = new FontFamily("Comic Sans MS");

             TextBlock CourierNewText = new TextBlock();
             CourierNewText.Text ="Courier New";
             CourierNewText.FontSize = 14.0;
             CourierNewText.Margin = new Thickness(1.0);
             CourierNewText.FontFamily = new FontFamily("Courier New");

             TextBlock GeorgiaText = new TextBlock();
             GeorgiaText.Text ="Georgia";
             GeorgiaText.FontSize = 14.0;
             GeorgiaText.Margin = new Thickness(1.0);
             GeorgiaText.FontFamily = new FontFamily("Georgia");

             TextBlock LucidaGrandeText = new TextBlock();
             LucidaGrandeText.Text ="Lucida Grande";
             LucidaGrandeText.FontSize = 14.0;
             LucidaGrandeText.Margin = new Thickness(1.0);
             LucidaGrandeText.FontFamily = new FontFamily("Lucida Grande");

             TextBlock LucidaSansUnicodeText = new TextBlock();
             LucidaSansUnicodeText.Text ="Lucida Sans Unicode";
             LucidaSansUnicodeText.FontSize = 14.0;
             LucidaSansUnicodeText.Margin = new Thickness(1.0);
             LucidaSansUnicodeText.FontFamily = new FontFamily("Lucida Sans Unicode");

             TextBlock TimesNewRomanText = new TextBlock();
             TimesNewRomanText.Text = "Times New Roman";
             TimesNewRomanText.FontSize = 14.0;
             TimesNewRomanText.Margin = new Thickness(1.0);
             TimesNewRomanText.FontFamily = new FontFamily("Times New Roman");

             TextBlock TrebuchetMSText = new TextBlock();
             TrebuchetMSText.Text ="Trebuchet MS";
             TrebuchetMSText.FontSize = 14.0;
             TrebuchetMSText.Margin = new Thickness(1.0);
             TrebuchetMSText.FontFamily = new FontFamily("Trebuchet MS");

             TextBlock VerdanaText = new TextBlock();
             VerdanaText.Text ="Verdana";
             VerdanaText.FontSize = 14.0;
             VerdanaText.Margin = new Thickness(1.0);
             VerdanaText.FontFamily = new FontFamily("Verdana");

             FontStackPanel.Children.Add(ArialText);
             FontStackPanel.Children.Add(ArialBlackText);
             FontStackPanel.Children.Add(ComicSansMSText);
             FontStackPanel.Children.Add(CourierNewText);
             FontStackPanel.Children.Add(GeorgiaText);
             FontStackPanel.Children.Add(LucidaGrandeText);
             FontStackPanel.Children.Add(LucidaSansUnicodeText);
             FontStackPanel.Children.Add(TimesNewRomanText);
             FontStackPanel.Children.Add(TrebuchetMSText);
             FontStackPanel.Children.Add(VerdanaText);
             
             FontStackPanel.SetValue(Canvas.TopProperty, 0.0);
             FontStackPanel.SetValue(Canvas.LeftProperty, 0.0);
             DynamicItemContainer.Children.Clear();
             DynamicItemContainer.Children.Add(FontStackPanel);
             StatusBar.Text = ToolTipService.GetToolTip(EmbeddedFont).ToString();
        }
        private void ReadFontsFromFile(object sender, MouseButtonEventArgs e)
         {
             StackPanel FontStackPanel = new StackPanel();
             FontStackPanel.Orientation = Orientation.Vertical;
             TextBlock AllegroText = new TextBlock();
             AllegroText.Text = "Allegro";
             AllegroText.FontSize = 24.0;
             AllegroText.Margin = new Thickness(4.0);
             AllegroText.FontFamily = new FontFamily("files/Allegro.TTF#Allegro");

             TextBlock AngelinaText = new TextBlock();
             AngelinaText.Text = "Angelina";
             AngelinaText.FontSize = 24.0;
             AngelinaText.Margin = new Thickness(4.0);
             AngelinaText.FontFamily = new FontFamily("files/Angelina.TTF#Angelina");

             TextBlock BrocsText = new TextBlock();
             BrocsText.Text = "BrockScript";
             BrocsText.FontSize = 24.0;
             BrocsText.Margin = new Thickness(4.0);
             BrocsText.FontFamily = new FontFamily("files/Brocs.TTF#BrockScript");

             TextBlock FuturaLText = new TextBlock();
             FuturaLText.Text = "Futura Lt BT";
             FuturaLText.FontSize = 24.0;
             FuturaLText.Margin = new Thickness(4.0);
             FuturaLText.FontFamily = new FontFamily("files/FuturaL.TTF#Futura Lt BT");

             TextBlock GlashouseText = new TextBlock();
             GlashouseText.Text = "Glass Houses";
             GlashouseText.FontSize = 24.0;
             GlashouseText.Margin = new Thickness(4.0);
             GlashouseText.FontFamily = new FontFamily("files/Glashouse.TTF#Glass Houses");
    
             FontStackPanel.Children.Add(AllegroText);
             FontStackPanel.Children.Add(AngelinaText);
             FontStackPanel.Children.Add(BrocsText);
             FontStackPanel.Children.Add(FuturaLText);
             FontStackPanel.Children.Add(GlashouseText);

             FontStackPanel.SetValue(Canvas.TopProperty, 0.0);
             FontStackPanel.SetValue(Canvas.LeftProperty, 0.0);
             DynamicItemContainer.Children.Clear();
             DynamicItemContainer.Children.Add(FontStackPanel);
             StatusBar.Text = ToolTipService.GetToolTip(FontFile).ToString();
         }
    }
}
