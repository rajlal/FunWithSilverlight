using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml;
using System.Windows.Browser;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Xml.Resolvers;
using System.Windows.Markup;

namespace WorkwithXML
{
    public partial class XLinq : UserControl
    {
        public XLinq()
        {
            InitializeComponent();
            }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            stackDocument.Visibility = Visibility.Collapsed;
            stackOthers.Visibility = Visibility.Collapsed;
            stackElement.Visibility = Visibility.Visible;        
        }
        private void CreateSelect(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = (TextBlock)sender;
            string FileSelectType  = t.Text;
            StatusBar.Text = ToolTipService.GetToolTip(t).ToString();
            if (FileSelectType == "XElement")
            {
                stackDocument.Visibility = Visibility.Collapsed;
                stackOthers.Visibility = Visibility.Collapsed;
                stackElement.Visibility = Visibility.Visible;    
            }
            else if (FileSelectType == "XDocument")
            {
                stackElement.Visibility = Visibility.Collapsed;
                stackOthers.Visibility = Visibility.Collapsed;
                stackDocument.Visibility = Visibility.Visible;
            }
            else if (FileSelectType == "XOthers")
            {
                stackOthers.Visibility = Visibility.Visible;
                stackElement.Visibility = Visibility.Collapsed;
                stackDocument.Visibility = Visibility.Collapsed;   
             }
        }

        private void CreateXElement(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = (TextBlock)sender;
            string SelectSubType = t.Text;
            StatusInfo.Text = ToolTipService.GetToolTip(t).ToString();
            if (SelectSubType == "Create XML")
            {
                XElementCreate();
            }
            else if (SelectSubType == "Load XML")
            {

                XElementLoadXML();
            }
            else if (SelectSubType == "Create XAML")
            {
                XElementLoadXAML();
            }
            else if (SelectSubType == "Load XAML")
            {
                XElementLoadXAMLFile();
            }
        }
        private void XElementCreate()
        {
            canvasXElement.Visibility = Visibility.Collapsed;
            txtXElement.Visibility = Visibility.Visible;
            StringBuilder output = new StringBuilder();
            XElement xmlTree = new XElement("item",
                new XElement("title", "Local New Silverlight Toolkit Video"),
                new XElement("link", "http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"),
                new XElement("pubDate", "Thu, 26 Mar 2009 17:13:00 GMT"),
                new XElement("description", "Local Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.")
            );

            output.Append(xmlTree);
            txtXElement.Text = output.ToString();
        }
        private void XElementLoadXML()
        {
            canvasXElement.Visibility = Visibility.Collapsed;
            txtXElement.Visibility = Visibility.Visible;
            StringBuilder output = new StringBuilder();
            Uri u = new Uri("Files/SilverlightNews.xml", UriKind.Relative);
            XmlReader xReader = XmlReader.Create("Files/SilverlightNews.xml");
            XElement xmlTree = XElement.Load(xReader);
            output.Append(xmlTree);
            txtXElement.Text = output.ToString();
        }
        private void XElementLoadXAML()
        {
           txtXElement.Visibility = Visibility.Collapsed;
           canvasXElement.Visibility = Visibility.Visible;
           XElement smileyElement = XElement.Parse(
                    @"<Canvas Margin='60,40,40,40' xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' x:Name='Smiley' Width='68.6579' Height='65.359' Clip='F1 M 0,0L 68.6579,0L 68.6579,65.359L 0,65.359L 0,0'>
		                <Canvas Width='68.6579' Height='65.359' Canvas.Left='0' Canvas.Top='0'>
			                <Ellipse  Width='68.6579' Height='65.359' Canvas.Left='0' Canvas.Top='0' Stretch='Fill' StrokeLineJoin='Round' Stroke='#FF000000' Fill='#FFF7FF08'/>
			                <Path Width='7.04111' Height='6.55237' Canvas.Left='18.2232' Canvas.Top='21.4838' Stretch='Fill' StrokeLineJoin='Round' Stroke='#FF000000' Fill='#FF060725' Data='F1 M 21.7438,21.9838C 23.412,21.9838 24.7643,23.2267 24.7643,24.76C 24.7643,26.2932 23.412,27.5362 21.7438,27.5362C 20.0756,27.5362 18.7232,26.2932 18.7232,24.76C 18.7232,23.2267 20.0756,21.9838 21.7438,21.9838 Z '/>
			                <Path Width='7.0412' Height='6.55249' Canvas.Left='43.6955' Canvas.Top='21.4838' Stretch='Fill' StrokeLineJoin='Round' Stroke='#FF000000' Fill='#FF07081F' Data='F1 M 47.2161,21.9838C 48.8843,21.9838 50.2367,23.2268 50.2367,24.76C 50.2367,26.2933 48.8843,27.5363 47.2161,27.5363C 45.5479,27.5363 44.1955,26.2933 44.1955,24.76C 44.1955,23.2268 45.5478,21.9838 47.2161,21.9838 Z '/>
			                <Path Width='20.6681' Height='7.00052' Canvas.Left='24.1972' Canvas.Top='41.719' Stretch='Fill' StrokeLineJoin='Round' Stroke='#FF1A1D4B' Data='F1 M 44.3652,42.219C 42.745,45.7453 38.952,48.2195 34.5312,48.2195C 30.1104,48.2195 26.3174,45.7453 24.6972,42.219'/>
		                </Canvas>
                        </Canvas>");

           UIElement MySmiley = (UIElement)XamlReader.Load(smileyElement.ToString());
            canvasXElement.Children.Add(MySmiley);            
        }
        private void XElementLoadXAMLFile()
        {
            canvasXElement.Visibility = Visibility.Visible;
            txtXElement.Visibility = Visibility.Collapsed;
            XmlReader xReader;
            xReader = XmlReader.Create("files/smiley.xaml");
            xReader.MoveToContent();
            UIElement MySmiley = (UIElement)XamlReader.Load(xReader.ReadOuterXml());
            canvasXElement.Children.Add(MySmiley);
        }

        private void CreateXDocument(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = (TextBlock)sender;
            string SelectSubType = t.Text;
            StatusInfo.Text = ToolTipService.GetToolTip(t).ToString();
            if (SelectSubType == "Create XML")
            {
                XDocumentCreate();
            }
            else if (SelectSubType == "Load XML")
            {
                XDocumentLoadXML();
            }
            else if (SelectSubType == "Remote XML")
            {
                XDocumentLoadRemoteXML();
            }
        }
        private void XDocumentCreate()
        {
            StringBuilder output = new StringBuilder();


            XDocument srcTree = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("Sample Silverlight Blog Item"),
                new XElement("items",
                    new XElement("item", 
                        new XElement("title", "XAP New Silverlight Toolkit Video"),
                        new XElement("link", "http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"),
                        new XElement("pubDate", "Thu, 26 Mar 2009 17:13:00 GMT"),
                        new XElement("description", "XAP Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.")
                ))
            );
            output.Append(srcTree.Declaration + Environment.NewLine);
            output.Append(srcTree);
            txtXDocument.Text = output.ToString();
        }
        private void XDocumentLoadXML()
        {
            StringBuilder output = new StringBuilder();
            string str =
            @"<?xml version='1.0' encoding='utf-8' ?>
                <!-- Sample Silverlight Blog Items --> 
                 <items>
                    <title>Archived News</title>
                    <link>http://silverlight.net/blogs/news/default.aspx</link>
                    <description>Silverlight News Blog</description>
                    <item>
                      <title>XAP New Silverlight Toolkit Video</title>
                      <link>http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls</link>
                      <pubDate>Thu, 26 Mar 2009 17:13:00 GMT</pubDate>
                      <description>XAP Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.</description>
                    </item></items>";

            XDocument doc = XDocument.Parse(str);
            output.Append(doc + Environment.NewLine);
            txtXDocument.Text = output.ToString();
        }
        private void XDocumentLoadRemoteXML()
        {
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(RemoteFileLoaded);
            c.OpenReadAsync(new Uri("http://silverlightfun.com/Samples/Chapter-05/WebSilverlightNews.xml"));
        }
        private void RemoteFileLoaded(object sender, OpenReadCompletedEventArgs e)
        {
            StringBuilder output = new StringBuilder();
            if (e.Error == null)
            {
               XmlReader xReader = XmlReader.Create(e.Result);
               XDocument doc = XDocument.Load(xReader);
               output.Append(doc + Environment.NewLine);
               txtXDocument.Text = output.ToString();
            }
        }

        private void CreateXOthers(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = (TextBlock)sender;
            string SelectSubType = t.Text;
            StatusInfo.Text = ToolTipService.GetToolTip(t).ToString();
            if (SelectSubType == "XAttribute")
            {
                CreateXAttribute();
            }
            else if (SelectSubType == "XComment")
            {
                CreateXComment();
            }
            else if (SelectSubType == "XDocumentType")
            {
                CreateXDocumentType();
            }
            else if (SelectSubType == "XProcessingInst")
            {
                CreateXProcessingInstructions();
            }
            else if (SelectSubType == "XCData")
            {
                CreateXCData();
            }
        }
        private void CreateXAttribute()
        {
            StringBuilder output = new StringBuilder();
             XElement xmlTree = new XElement("item",
                new XAttribute("flagread", "0"),
                new XElement("title", "Local New Silverlight Toolkit Video"),
                new XElement("link", "http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"),
                new XElement("pubDate", "Thu, 26 Mar 2009 17:13:00 GMT"),
                new XElement("description", "Local Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.")
                );
            output.Append("Attribute Added: " + xmlTree.FirstAttribute + Environment.NewLine);
            output.Append(xmlTree + Environment.NewLine);

            txtXMLOthers.Text = output.ToString();
        }
        private void CreateXComment()
        {
            StringBuilder output = new StringBuilder();
            XElement xmlTree = new XElement("item",
               new XComment("This is first item"),
               new XElement("title", "Local New Silverlight Toolkit Video"),
               new XElement("link", "http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"),
               new XElement("pubDate", "Thu, 26 Mar 2009 17:13:00 GMT"),
               new XElement("description", "Local Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.")
               );
            output.Append("Comment Added: This is first item" + Environment.NewLine);
            output.Append(xmlTree + Environment.NewLine);
            txtXMLOthers.Text = output.ToString();
        }
        private void CreateXDocumentType()
        {
            string internalSubset = @"<!ELEMENT items (item+)>
                                      <!ELEMENT item (title, link,pubDate,description)>
                                      <!ELEMENT title (#PCDATA)>
                                      <!ELEMENT link (#PCDATA)>
                                      <!ELEMENT pubDate (#PCDATA)>
                                      <!ELEMENT description (#PCDATA)>";


            StringBuilder output = new StringBuilder();
            XDocument doc = new XDocument(
                        new XDocumentType("items", null, null, internalSubset),
                        new XElement("items",
                        new XElement("item",
                                       new XElement("title", "Local New Silverlight Toolkit Video"),
                                       new XElement("link", "http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"),
                                       new XElement("pubDate", "Thu, 26 Mar 2009 17:13:00 GMT"),
                                       new XElement("description", "Local Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.")
                                    )
                                )
                        );
            doc.Declaration = new XDeclaration("1.0", "utf-8", "true");
            output.Append(doc + Environment.NewLine);
            txtXMLOthers.Text = output.ToString();
        }
        private void CreateXProcessingInstructions()
        {
            StringBuilder output = new StringBuilder();
            string target = "xml-stylesheet";
            string data = "href=\"style.css\" title=\"Compact\" type=\"text/css\"";

            XDocument doc = new XDocument(
            new XProcessingInstruction(target, data),
                    new XElement("items",
                        new XElement("item",
                                       new XElement("title", "Local New Silverlight Toolkit Video"),
                                       new XElement("link", "http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"),
                                       new XElement("pubDate", "Thu, 26 Mar 2009 17:13:00 GMT"),
                                       new XElement("description", "Local Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.")
                                    )
                                )
                        );
            doc.Declaration = new XDeclaration("1.0", "utf-8", "true");
            output.Append(doc + Environment.NewLine);
            txtXMLOthers.Text = output.ToString();

        } 
        private void CreateXCData()
        {
            StringBuilder output = new StringBuilder();
            XElement xmlTree = new XElement("item",
               new XElement("title", "Local New Silverlight Toolkit Video"),
               new XElement("link", "http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"),
               new XElement("pubDate", "Thu, 26 Mar 2009 17:13:00 GMT"),
               new XElement("description", "Local Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page."),
               new XElement("htmldescription", new XCData("<p>Todd Miranda demonstrates how to use <b>Silverlight Themes</b>. <br>Watch this and other Silverlight videos on the Learn page.</p>"))
            );
            output.Append("Attribute Added: " + xmlTree.FirstAttribute + Environment.NewLine);
            output.Append(xmlTree + Environment.NewLine);

            txtXMLOthers.Text = output.ToString();
        }
       

     
     }
 
}
