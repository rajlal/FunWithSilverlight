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
using System.ServiceModel.Syndication;
using System.Xml;

namespace RSSandAtom
{
    public partial class Syndication : UserControl
    {
        public Syndication()
        {
            InitializeComponent();
        }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            GetFeed("http://silverlight.net/blogs/news/rss.aspx");
        }
        private void SelectAction(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = (TextBlock)sender;
            string SelectType  = t.Text;
            StatusBar.Text = ToolTipService.GetToolTip(t).ToString();
            if (SelectType == "Load RSS")
            {
             
                GetFeed("http://silverlight.net/blogs/news/rss.aspx");
            }
            else if (SelectType == "Load Atom")
            {
                   GetFeed("http://silverlight.net/blogs/microsoft/atom.aspx");
            }
            else if (SelectType == "XML to RSS")
            {
                  SyndicationFeedFormatter s = GetBlog("rss");
                  FeedList.ItemsSource = s.Feed.Items;
                  StatusInfo.Text = "Rss20FeedFormatter(feed)";
            }
            else if (SelectType == "XML to Atom")
            {
                SyndicationFeedFormatter s = GetBlog("atom");
                FeedList.ItemsSource = s.Feed.Items;
                StatusInfo.Text = "Atom10FeedFormatter(feed)";
            }
        }
        void GetFeed(string SyndicationFeed)
        {
            
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(ProcessFeed);
            c.OpenReadAsync(new Uri(SyndicationFeed));
        }
        void ProcessFeed(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                XmlReader rdr = XmlReader.Create(e.Result);
                SyndicationFeed feed = SyndicationFeed.Load(rdr);
                FeedList.ItemsSource = feed.Items;
            }
            catch (Exception)
            {
                StatusInfo.Text = "Security error !";
            }
        }
        public SyndicationFeedFormatter GetBlog(string format)
        {
            SyndicationFeed feed = new SyndicationFeed("Archived News", "Silverlight Blogs and News", new Uri("http://silverlight.net/blogs/news/default.aspx"));
            feed.Description = new TextSyndicationContent("Silverlight News Blog");
            SyndicationItem item1 = new SyndicationItem(
                "New Silverlight Toolkit Video",
                "Todd Miranda demonstrates how to use Silverlight Themes. Watch this and other Silverlight videos on the Learn page.",
                new Uri("http://silverlight.net/learn/videocat.aspx?cat=2#HDI2Controls"),
                "ItemOneID",
                DateTime.Now);

            SyndicationItem item2 = new SyndicationItem(
                "Seven New Community Gallery Entries",
                "Play the Silverlight Gobang Game, practice Creating Particle Effects in Silverlight, dance with a 3D Silver Mouse, and more! Find inspiration and upload your Silverlight projects to share with the community in the Gallery.",
                new Uri("http://silverlight.net/themes/silverlight/community/gallerydetail.aspx?cat=Silverlight2"),
                "ItemTwoID",
                 DateTime.Now);

            SyndicationItem item3 = new SyndicationItem(
                "16 New Silverlight Showcases",
                "Improve your tonal memory with TwinNotes, create your own bracelets using Brighton’s Interactive Charm Builder, browse the selection of movies at Ramp DVD Store, and more in the Silverlight Showcase.",
                new Uri("http://silverlight.net/Showcase"),
                "ItemThreeID",
                 DateTime.Now);

            List<SyndicationItem> items = new List<SyndicationItem>();
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);

            feed.Items = items;

            if (format == "rss")
                return new Rss20FeedFormatter(feed);
            else if (format == "atom")
                return new Atom10FeedFormatter(feed);
            else return null;
        }
   }
    
}
