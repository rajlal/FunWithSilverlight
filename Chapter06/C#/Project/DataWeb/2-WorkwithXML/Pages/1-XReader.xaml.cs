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
    public partial class XReader : UserControl
    {
        NewsList MyNewsList = new NewsList();
        string FileSelectType = "Embedded XML";
        SelectedNewsItem CurrentSelectedNewsItem = new SelectedNewsItem();
        public XReader()
        {
            InitializeComponent();
            CurrentSelectedNewsItem.Desc = "No Items Selected";
            CurrentSelectedNewsItem.Link="http://silverlightfun.com";
            stackNewsDetails.DataContext = CurrentSelectedNewsItem;
        }
        private void UpdateDisplay()
        {
            myDisplayList.Items.Clear();
           
                foreach (NewsItem s in MyNewsList)
                {
                    ListBoxItem lbi = new ListBoxItem();
                    StackPanel sp = new StackPanel();
                    TextBlock si = new TextBlock();
                    si.Text = " " + s.Title.Substring(0,17)+ "..";
                    si.FontSize = 11;
                    si.FontFamily = new FontFamily("Verdana");

                    ToolTip tt = new ToolTip();
                    tt.Content = s.Title;
                    ToolTipService.SetToolTip(si, tt);

                    TextBlock tb = new TextBlock();
                    tb.Text = " " + s.Date.Substring(0,16);
                    tb.FontSize = 8;
                    tb.Foreground = new SolidColorBrush(Colors.Gray);
                    sp.Orientation = Orientation.Vertical;
                    sp.Children.Add(si);
                    sp.Children.Add(tb);
                    lbi.Content = sp;
                    lbi.MouseLeftButtonUp += new MouseButtonEventHandler(SetItem);
                    myDisplayList.Items.Add(lbi);
                }
                myDisplayList.SelectedIndex = 0;
                CurrentSelectedNewsItem.Desc = MyNewsList[0].Desc;
                CurrentSelectedNewsItem.Link = MyNewsList[0].Link;
           
        }
        private void SetItem(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBoxItem lb = (ListBoxItem)sender;
            StackPanel sp = (StackPanel)lb.Content;
            TextBlock sText= (TextBlock)sp.Children[0];
            ToolTip tt = (ToolTip)ToolTipService.GetToolTip(sText);
            string titleText = tt.Content.ToString();

            foreach (NewsItem n in MyNewsList)
            {
                if (n.Title.ToLower() == titleText.ToLower())
                {
                    CurrentSelectedNewsItem.Desc = n.Desc;
                    CurrentSelectedNewsItem.Link = n.Link;
                    break;
                }
            }
        } 
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            LoadXAPFile();            
        }
        private void FileSelect(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = (TextBlock)sender;
            FileSelectType  = t.Text;
            StatusBar.Text = "Selected: " + ToolTipService.GetToolTip(t).ToString();
            if (FileSelectType == "Embedded XML")
            {
                LoadXAPFile();
                StatusInfo.Text = "SilverlightNews.xml";
            }
            else if (FileSelectType == "Local XML")
            {
                LoadLocalFile("Files/LocalSilverlightNews.xml");
                StatusInfo.Text = "Files/LocalSilverlightNews.xml";
            }
            else if (FileSelectType == "XHTML")
            {
                LoadXHTMLFile("Files/SilverlightNews.htm");
                StatusInfo.Text = "Files/SilverlightNews.htm";
            }
            else if (FileSelectType == "Remote XML")
            {
                LoadRemoteFile("http://silverlightfun.com/Samples/Chapter-05/WebSilverlightNews.xml");
                StatusInfo.Text = "News.xml @ silverlightfun.com";
            }
           
        }
        private void LoadXAPFile()
        {
            MyNewsList.Clear();
            Uri u = new Uri("Files/SilverlightNews.xml", UriKind.Relative);
            XmlReader xReader = XmlReader.Create("Files/SilverlightNews.xml");
            xReader.ReadToFollowing("item");

            while (!xReader.EOF)
            {
                NewsItem n = new NewsItem();
                xReader.ReadToFollowing("title");
                n.Title = xReader.ReadElementContentAsString();
                xReader.ReadToFollowing("link");
                n.Link = xReader.ReadElementContentAsString();
                xReader.ReadToFollowing("pubDate");
                n.Date = xReader.ReadElementContentAsString();
                xReader.ReadToFollowing("description");
                n.Desc = xReader.ReadElementContentAsString();

                MyNewsList.Add(n);
                xReader.ReadToFollowing("item"); // Moves the reader back to the element node.
            }
            UpdateDisplay();

        }
        private void LoadLocalFile(string localxmlfile)
        {
            WebClient xmlClient = new WebClient();
            xmlClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(LocalFileLoaded);
            xmlClient.DownloadStringAsync(new Uri(localxmlfile, UriKind.RelativeOrAbsolute));
        }
        private void LocalFileLoaded(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string xmlData = e.Result;
                 XmlReader xReader = XmlReader.Create(new StringReader(xmlData));
                MyNewsList.Clear();

                xReader.ReadToFollowing("item");
                while (!xReader.EOF)
                {
                    NewsItem n = new NewsItem();
                    xReader.ReadToFollowing("title");
                    n.Title = xReader.ReadElementContentAsString();
                    xReader.ReadToFollowing("link");
                    n.Link = xReader.ReadElementContentAsString();
                    xReader.ReadToFollowing("pubDate");
                    n.Date = xReader.ReadElementContentAsString();
                    xReader.ReadToFollowing("description");
                    n.Desc = xReader.ReadElementContentAsString();
                    MyNewsList.Add(n);
                    xReader.ReadToFollowing("item"); // Moves the reader back to the element node.
                }
                UpdateDisplay();
            }
        }
        private void LoadXHTMLFile(string localXhtmlFile)
        {
            MyNewsList.Clear();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            settings.XmlResolver = new XmlPreloadedResolver(new XmlXapResolver(), XmlKnownDtds.Xhtml10);

            XmlReader xReader = XmlReader.Create(localXhtmlFile, settings);

            xReader.ReadToFollowing("item");

            while (!xReader.EOF)
            {
                NewsItem n = new NewsItem();
                xReader.ReadToFollowing("title");
                n.Title = xReader.ReadElementContentAsString();
                xReader.ReadToFollowing("link");
                n.Link = xReader.ReadElementContentAsString();
                xReader.ReadToFollowing("pubDate");
                n.Date = xReader.ReadElementContentAsString();
                xReader.ReadToFollowing("description");
                n.Desc = xReader.ReadElementContentAsString();

                MyNewsList.Add(n);
                xReader.ReadToFollowing("item"); // Moves the reader back to the element node.
            }

            UpdateDisplay();
        }
        private void LoadRemoteFile(string remoteXmlfile)
        {
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(RemoteFileLoaded);
            c.OpenReadAsync(new Uri(remoteXmlfile));
        }
        private void RemoteFileLoaded(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                XmlReader xReader = XmlReader.Create(e.Result);
                MyNewsList.Clear();

                xReader.ReadToFollowing("item");
                while (!xReader.EOF)
                {
                    NewsItem n = new NewsItem();
                    xReader.ReadToFollowing("title");
                    n.Title = xReader.ReadElementContentAsString();
                    xReader.ReadToFollowing("link");
                    n.Link = xReader.ReadElementContentAsString();
                    xReader.ReadToFollowing("pubDate");
                    n.Date = xReader.ReadElementContentAsString();
                    xReader.ReadToFollowing("description");
                    n.Desc = xReader.ReadElementContentAsString();
                    MyNewsList.Add(n);
                    xReader.ReadToFollowing("item"); // Moves the reader back to the element node.
                }
                UpdateDisplay();
            }
        }
    }
 public class NewsList : List<NewsItem>
    {
        NewsItem si;
        public NewsItem Val { get { return si; } set { si = value; } }
        public NewsList()
        {}
    }
 public class NewsItem 
    {
    private string title;
    private string link;
    private string date;
    private string desc;

    public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
              
            }
        }
    public string Desc
     {
         get
         {
             return desc;
         }
         set
         {
             desc = value;

         }
     }
    public string Date
     {
         get
         {
             return date;
         }
         set
         {
             date = value;

         }
     }
    public string Link
        {
            get
            {
                return link;
            }
            set
            {
                link = value;
               
            }
        }
    public NewsItem()
    { }
    public NewsItem(String title, String link, string date, string desc)
        {
            this.Title = title;
            this.Link = link;
            this.Date = date;
            this.Desc = desc;
        }
      
    }
 public class SelectedNewsItem : NewsItem,INotifyPropertyChanged
 {
     private string desc;
     private string link;
     // Declare the PropertyChanged event.
     public event PropertyChangedEventHandler PropertyChanged;
     // Create the property that will be the source of the binding.
     public string Desc
     {
         get { return desc; }
         set
         {
             desc = value;
             NotifyPropertyChanged("Desc");
         }
     }
     public string Link
     {
         get { return link; }
         set
         {
             link = value;
             NotifyPropertyChanged("Link");
         }
     }
     public void NotifyPropertyChanged(string propertyName)
     {
         if (PropertyChanged != null)
         {
             PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
         }
     }
 }
}
