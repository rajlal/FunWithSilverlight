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
using System.Xml.Serialization;

namespace WorkwithXML
{
    public partial class XSerializer : UserControl
    {
        public XSerializer()
        {
            InitializeComponent();
        }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            stackDeserialize.Visibility = Visibility.Visible;
            stackSerialize.Visibility = Visibility.Collapsed;
            DeSerializeXML();
        }
        private void FileSelect(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = (TextBlock)sender;
            string SelectType  = t.Text;
            StatusBar.Text = "Selected: " + ToolTipService.GetToolTip(t).ToString();
            if (SelectType == "Serialize")
            {
                stackSerialize.Visibility = Visibility.Visible;
                stackDeserialize.Visibility = Visibility.Collapsed;
                SerializeXML();
            }
            else if (SelectType == "De-Serialize")
            {
                stackSerialize.Visibility = Visibility.Collapsed;
                stackDeserialize.Visibility = Visibility.Visible;
                DeSerializeXML();
            }
        }
        private void SerializeXML()
        {
            blogitem myitem = new blogitem();
            myitem.title = "Seven New Community Gallery Entries";
            myitem.link = "http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2";
            myitem.pubDate = "Wed, 25 Mar 2009 23:43:00 GMT";
            myitem.desc = "XAP Play the Silverlight Gobang Game, practice Creating Particle Effects in Silverlight, dance with a 3D Silver Mouse, and more! Find inspiration and upload your Silverlight projects to share with the community in the Gallery. ";

            blog mysilverlightblog = new blog();
            mysilverlightblog.item = myitem;

            StringBuilder output = new StringBuilder();
            XDocument doc = new XDocument();
            XmlSerializer serializer = new XmlSerializer(typeof(blog));
            XmlWriter xWriter = XmlWriter.Create(output);
            serializer.Serialize(xWriter, mysilverlightblog);
            xmlData.Text = output.ToString();
           
        }
        private void DeSerializeXML()
        {
            String xmlString = @"<?xml version='1.0' encoding='utf-8'?>
                                    <items xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
                                     xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                                   <item>
                                      <title>New Silverlight Toolkit Video</title>
                                      <link>http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls</link>
                                      <pubDate>Thu, 26 Mar 2009 17:13:00 GMT</pubDate>
                                      <description>XAP Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.</description>
                                    </item>
                                  </items>";

           
            XmlReader xReader = XmlReader.Create(new StringReader(xmlString));
            XmlSerializer serializer = new XmlSerializer(typeof(blog));
            blog slbitem = (blog)serializer.Deserialize(xReader);
            
            itemTitle.Text = slbitem.item.title;
            itemLink.NavigateUri = new Uri(slbitem.item.link);
            ToolTip tt = new ToolTip();
            tt.Content = slbitem.item.link;
            ToolTipService.SetToolTip(itemLink, tt);
            itempubDate.Text = slbitem.item.pubDate;
            itemDesc.Text = slbitem.item.desc;

        }
 }
    [XmlRoot("items")]
    public class blog
    {
        [XmlElement("item")]
        public blogitem item { get; set; }
    }
    public class blogitem 
    {
        [XmlElement("title")]
        public string title { get; set; }
        [XmlElement("link")]
        public string link { get; set; }
        [XmlElement("pubDate")]
        public string pubDate { get; set; }
        [XmlElement("description")]
        public string desc { get; set; }
    }
    
}
