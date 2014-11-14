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

namespace WorkwithXML
{
    public partial class XWriter : UserControl
    {
        public XWriter()
        {
            InitializeComponent();
            }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            CreateNode();            
        }
        private void CreateSelect(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = (TextBlock)sender;
            string FileSelectType  = t.Text;
            StatusBar.Text = "Selected: " + ToolTipService.GetToolTip(t).ToString();
            if (FileSelectType == "XML Element")
            {
                CreateNode();
            }
            else if (FileSelectType == "Create XML")
            {
                CreateXML(); 
            }
            else if (FileSelectType == "XML File")
            {
                LoadRemoteFile("http://silverlightfun.com/Samples/Chapter-05/WebSilverlightNews.xml");
            }
        }
        private void CreateNode()
        {
            StringBuilder output = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("\t");
            settings.OmitXmlDeclaration = true;
            settings.NewLineOnAttributes = true;

            using (XmlWriter writer = XmlWriter.Create(output, settings))
            {
                writer.WriteStartElement("items");
                writer.WriteStartElement("item");
                writer.WriteElementString("title", "Element - New Silverlight Toolkit Video");
                writer.WriteElementString("link", "http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls");
                writer.WriteElementString("pubDate", "Thu, 26 Mar 2009 17:13:00 GMT");
                writer.WriteElementString("description", "Element - Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Flush();
            }

            txtXML.Text = output.ToString();


        }
        private void CreateXML()
        {
            StringBuilder output = new StringBuilder();

            String xmlString = @"<items>
                      <item>
                          <title>XML New Silverlight Toolkit Video</title>
                          <link>http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls</link>
                          <pubDate>Thu, 26 Mar 2009 17:13:00 GMT</pubDate>
                          <description>XML Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.</description>
                        </item>
                       </items>";


            using (XmlReader xReader = XmlReader.Create(new StringReader(xmlString)))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
          
                using (XmlWriter writer = XmlWriter.Create(output))
                {
                     while (xReader.Read())
                    {
                        if (xReader.NodeType == XmlNodeType.Element)
                        {
                            writer.WriteRaw("\n");
                            writer.WriteStartElement(xReader.Name);
                            writer.WriteAttributes(xReader, false);
                            if (xReader.IsEmptyElement)
                                writer.WriteEndElement();
                        }
                        else if (xReader.NodeType == XmlNodeType.Text)
                        {
                            writer.WriteString(xReader.Value);
                            
                        }
                        else if (xReader.NodeType == XmlNodeType.EndElement)
                        {
                            if (xReader.Name == "item")
                                writer.WriteRaw("\n");
                            writer.WriteEndElement();
                           
                        }
                     
                    }
                }
            }
            txtXML.Text= output.ToString();

        }
        private void LoadRemoteFile(string remoteXmlfile)
        {
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(RemoteFileReadandWrite);
            c.OpenReadAsync(new Uri(remoteXmlfile));
        }
        private void RemoteFileReadandWrite(object sender, OpenReadCompletedEventArgs e)
        {
            StringBuilder output = new StringBuilder();
            if (e.Error == null)
            {
                XmlReader xReader = XmlReader.Create(e.Result);
                 using (XmlWriter writer = XmlWriter.Create(output))
                    {
                        writer.WriteRaw("\n"); 
                        writer.WriteStartElement("items");
                        xReader.ReadToFollowing("item");
                        while (!xReader.EOF)
                        {
                            writer.WriteNode(xReader, false);
                            xReader.ReadToFollowing("item");
                        }
                     }
              
            }
            txtXML.Text = output.ToString();
        }
    }
 
}
