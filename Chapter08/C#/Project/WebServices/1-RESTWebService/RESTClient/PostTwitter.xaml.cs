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
using System.IO;
using System.Threading;
using System.Security;
using System.Windows.Media.Imaging;
using System.Xml;

namespace RESTClient
{
    public partial class PostTwitter : UserControl
    {
        SynchronizationContext syncContext;
        string statusString;
        string searchString;
        TwitterList MyTwitterList = new TwitterList();
       
        public PostTwitter()
        {
            InitializeComponent();
        }

        private void ButtonPost_Click(object sender, RoutedEventArgs e)
        {
            ButtonPost.Content = "Loading...";
            ButtonPost.IsEnabled = false;
            searchString = txtMessage.Text ;
          
            // Grab SynchronizationContext while on UI Thread   
            syncContext = SynchronizationContext.Current;

            // Create request   
            HttpWebRequest request =
                WebRequest.Create(new Uri("http://search.twitter.com/search.rss", UriKind.Absolute))
                    as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            // Make async call for request stream.  Callback will be called on a background thread.  
            IAsyncResult asyncResult =
                request.BeginGetRequestStream(new AsyncCallback(RequestStreamCallbackPost), request);

        }
        private void RequestStreamCallbackPost(IAsyncResult ar)
        {
            HttpWebRequest request = ar.AsyncState as HttpWebRequest;
            //request.ContentType = "application/atom+xml";
            Stream requestStream = request.EndGetRequestStream(ar);
            StreamWriter streamWriter = new StreamWriter(requestStream);
            streamWriter.Write("q=" + searchString);
            streamWriter.Write("&lang=en");
            streamWriter.Write("&rpp=12");

            streamWriter.Flush();
            // Close the stream.
            streamWriter.Close();
            // Make async call for response.  Callback will be called on a background thread.
            request.BeginGetResponse(new AsyncCallback(ResponseCallbackPost), request);

        }
        private void ResponseCallbackPost(IAsyncResult ar)
        {
            HttpWebRequest request = ar.AsyncState as HttpWebRequest;
            WebResponse response = null;
            try
            {
                response = request.EndGetResponse(ar);
            }
            catch (WebException we)
            {
                statusString = we.Status.ToString();
            }
            catch (SecurityException se)
            {
                statusString = se.Message;
                if (statusString == "")
                    statusString = se.InnerException.Message;
            }

            // Invoke onto UI thread  
            syncContext.Post(ExtractResponsePost, response);
        }

        private void ExtractResponsePost(object state)
        {
            HttpWebResponse response = state as HttpWebResponse;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader responseReader = new StreamReader(response.GetResponseStream());
                string txtMessageText = responseReader.ReadToEnd();
                XmlReader twitterResult = XmlReader.Create(new StringReader(txtMessageText));
                twitterResult.ReadToFollowing("item");

                while (!twitterResult.EOF)
                {

                    TwitterItem t = new TwitterItem();
                    twitterResult.ReadToFollowing("title");
                    t.Title = twitterResult.ReadElementContentAsString();
                    twitterResult.ReadToFollowing("link");
                    t.Url = twitterResult.ReadElementContentAsString();
                    twitterResult.ReadToFollowing("author");
                    t.Author = twitterResult.ReadElementContentAsString();
                    twitterResult.ReadToFollowing("google:image_link");
                    t.ImageUrl = twitterResult.ReadElementContentAsString();
                  
                    MyTwitterList.Add(t);
                    twitterResult.ReadToFollowing("item"); // Moves the reader back to the element node.
                }
            }
            else
             txtMessage.Text = "Post failed: " + statusString;

            ButtonPost.IsEnabled = true;
            ButtonPost.Content = "POST";
            UpdateDisplay();
     
        }
        private void UpdateDisplay()
        {
            this.TwitterGrid.DataContext = MyTwitterList;
            this.TwitterGrid.UpdateLayout();
        }
        private void ResourceImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Image i = (Image)sender;
            i.Source = new BitmapImage(new Uri("images/error.jpg", UriKind.Relative));
            ToolTip t = new ToolTip();
            t.Content = new TextBlock()
            {
                FontFamily = new FontFamily("Arial"),
                FontSize = 12,
                Text = "Error retrieving Image",
                TextWrapping = TextWrapping.Wrap
            };
            ToolTipService.SetToolTip(i, t);
        }
    }
    public class TwitterList : List<TwitterItem>
    {
        TwitterItem si;
        public TwitterItem Val { get { return si; } set { si = value; } }
        public TwitterList()
        { }
    }
    public class TwitterItem
    {
        private string title;
        private string author;
        private string imageurl = "images/loading.png";
        private string url = "http://silverlightfun.com";


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
        public string Author
        {
            get
            {
                return author;
            }
            set
            {
                author = value;

            }
        }
        public string ImageUrl
        {
            get
            {
                return imageurl;
            }
            set
            {
                imageurl = value;

            }
        }
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;

            }
        }

        public TwitterItem()
        { }
        public TwitterItem(String title, String author, string imageurl, string url)
        {
            this.Title = title;
            this.Author = author;
            this.ImageUrl = imageurl;
            this.Url = url;
        }

    }
}
