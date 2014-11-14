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
using System.Json;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace UseJSON
{
    public partial class JLoadSave : UserControl
    {
        public JLoadSave()
        {
            InitializeComponent();
        }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            ResetView();
            stackLoadText.Visibility = Visibility.Visible;
            LoadTextReader();
        }
        private void SelectAction(object sender, MouseButtonEventArgs e)
        {
            ResetView(); 
            TextBlock t = (TextBlock)sender;
            string SelectType  = t.Text;
            StatusBar.Text = ToolTipService.GetToolTip(t).ToString();
            if (SelectType == "Load TextReader")
            {
                stackLoadText.Visibility = Visibility.Visible;
                LoadTextReader();
            }
            else if (SelectType == "Load Stream")
            {
                stackLoadStream.Visibility = Visibility.Visible;
                LoadStream();
            }
            else if (SelectType == "Parse text")
            {
                stackParseText.Visibility = Visibility.Visible;
                ParseJSONText();
            }
            else if (SelectType == "Save")
            {
                stackSave.Visibility = Visibility.Visible;
                SaveJSON();
            }
        }
        private void SaveJSON()
        {
            JsonObject myJson = new JsonObject();
            myJson.Add("title", "Seven New Community Gallery Entries");
            myJson.Add("link", "http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2");
            myJson.Add("pubDate", "Wed, 25 Mar 2009 23:43:00 GMT");
            myJson.Add("description", "Created JSON, Play the Silverlight Gobang Game, practice Creating Particle Effects in Silverlight, dance with a 3D Silver Mouse, and more! Find inspiration and upload your Silverlight projects to share with the community in the Gallery. ");
            MemoryStream mStream = new MemoryStream();
            myJson.Save(mStream);
            byte[] json = mStream.ToArray();
            mStream.Close();
            txtSave.Text = Encoding.UTF8.GetString(json, 0, json.Length);
            StatusInfo.Text = "JsonObject.Save";
        }
        private void ParseJSONText()
        {
            string stringJson = "{'title':'Seven New Community Gallery Entries','link':'http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2','pubDate':'Wed, 25 Mar 2009 23:43:00 GMT','description':'Parsed JSON, Play the Silverlight Gobang Game, practice Creating Particle Effects in Silverlight, dance with a 3D Silver Mouse, and more! Find inspiration and upload your Silverlight projects to share with the community in the Gallery. '}";
            JsonObject blogitem = (JsonObject)JsonObject.Parse(stringJson);
            itemTitleP.Text = blogitem["title"];
            itemLinkP.NavigateUri = new Uri(blogitem["link"]);
            ToolTip tt = new ToolTip();
            tt.Content = blogitem["link"];
            ToolTipService.SetToolTip(itemLink, tt);
            itempubDateP.Text = blogitem["pubDate"];
            itemDescP.Text = blogitem["description"];

            StatusInfo.Text = "JSON Parsed from Text";
        }
        private void LoadStream()
        {
            LoadRemoteFile("http://silverlightfun.com/Samples/Chapter-05/WebSilverlightNewsItem.Json");
            StatusInfo.Text ="Remote JSON file";
        }
        private void LoadRemoteFile(string remoteJsonfile)
        {
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(RemoteFileLoaded);
            c.OpenReadAsync(new Uri(remoteJsonfile));
        }
        private void RemoteFileLoaded(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Stream responseStream = e.Result;
                JsonObject blogitem = (JsonObject)JsonObject.Load(responseStream); 
                itemTitle.Text =  blogitem["title"];
                itemLink.NavigateUri = new Uri(blogitem["link"]);
                ToolTip tt = new ToolTip();
                tt.Content = blogitem["link"];
                ToolTipService.SetToolTip(itemLink, tt);
                itempubDate.Text = blogitem["pubDate"];
                itemDesc.Text = blogitem["description"];

            }
        }
        private void LoadTextReader()
        {
            string stringJson = "{'title':'Seven New Community Gallery Entries','link':'http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2','pubDate':'Wed, 25 Mar 2009 23:43:00 GMT','description':'TextReader Play the Silverlight Gobang Game, practice Creating Particle Effects in Silverlight, dance with a 3D Silver Mouse, and more! Find inspiration and upload your Silverlight projects to share with the community in the Gallery. '}";
            JsonObject blogitem = (JsonObject)JsonObject.Load(new StringReader(stringJson));
            itemTitleS.Text = blogitem["title"];
            itemLinkS.NavigateUri = new Uri(blogitem["link"]);
            ToolTip tt = new ToolTip();
            tt.Content = blogitem["link"];
            ToolTipService.SetToolTip(itemLink, tt);
            itempubDateS.Text = blogitem["pubDate"];
            itemDescS.Text = blogitem["description"];

            StatusInfo.Text = "JSON from TextReader ";

        }
        private void ResetView()
        {
            stackLoadStream.Visibility = Visibility.Collapsed;
            stackLoadText.Visibility = Visibility.Collapsed;
            stackParseText.Visibility = Visibility.Collapsed;
            stackSave.Visibility = Visibility.Collapsed;
        }
       
 }
}
