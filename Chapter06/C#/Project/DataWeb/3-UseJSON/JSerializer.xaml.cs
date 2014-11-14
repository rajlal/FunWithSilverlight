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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace UseJSON
{
    public partial class JSerializer : UserControl
    {
        public JSerializer()
        {
            InitializeComponent();
        }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            stackDeserialize.Visibility = Visibility.Visible;
            stackSerialize.Visibility = Visibility.Collapsed;
            DeSerializeJSON();
        }
        private void SelectAction(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = (TextBlock)sender;
            string SelectType  = t.Text;
            StatusBar.Text = ToolTipService.GetToolTip(t).ToString();
            if (SelectType == "Serialize")
            {
                stackSerialize.Visibility = Visibility.Visible;
                stackDeserialize.Visibility = Visibility.Collapsed;
                SerializeJSON();
            }
            else if (SelectType == "De-Serialize")
            {
                stackSerialize.Visibility = Visibility.Collapsed;
                stackDeserialize.Visibility = Visibility.Visible;
                DeSerializeJSON();
            }
        }
        private void SerializeJSON()
        {
           
            BlogItemJSON myitem = new BlogItemJSON();
            myitem.title = "Seven New Community Gallery Entries";
            myitem.link = "http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2";
            myitem.pubDate = "Wed, 25 Mar 2009 23:43:00 GMT";
            myitem.description = "XAP Play the Silverlight Gobang Game, practice Creating Particle Effects in Silverlight, dance with a 3D Silver Mouse, and more! Find inspiration and upload your Silverlight projects to share with the community in the Gallery. ";

            //Create a stream to serialize the object to.
            MemoryStream mStream = new MemoryStream();

            // Serializer the BlogItem object to the stream.
            DataContractJsonSerializer Serializer = new DataContractJsonSerializer(typeof(BlogItemJSON));
            Serializer.WriteObject(mStream, myitem);
            byte[] json = mStream.ToArray();
            mStream.Close();
            jsonData.Text= Encoding.UTF8.GetString(json, 0, json.Length);
        }
        private void DeSerializeJSON()
        {
            BlogItemJSON deserializedBlogItem = new BlogItemJSON();
            string jsonString = "{'title':'Seven New Community Gallery Entries','link':'http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2','pubDate':'Wed, 25 Mar 2009 23:43:00 GMT','description':'XAP Play the Silverlight Gobang Game, practice Creating Particle Effects in Silverlight, dance with a 3D Silver Mouse, and more! Find inspiration and upload your Silverlight projects to share with the community in the Gallery. '}";
            MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            DataContractJsonSerializer Serializer = new DataContractJsonSerializer(deserializedBlogItem.GetType());
            deserializedBlogItem = Serializer.ReadObject(mStream) as BlogItemJSON;
            mStream.Close();
            itemTitle.Text = deserializedBlogItem.title;
            itemLink.NavigateUri = new Uri(deserializedBlogItem.link);
            ToolTip tt = new ToolTip();
            tt.Content = deserializedBlogItem.link;
            ToolTipService.SetToolTip(itemLink, tt);
            itempubDate.Text = deserializedBlogItem.pubDate;
            itemDesc.Text = deserializedBlogItem.description;
        }
   }
    [DataContract]
    public class BlogItemJSON
    {
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string link { get; set; }
        [DataMember]
        public string pubDate { get; set; }
        [DataMember]
        public string description { get; set; }
    }
}
