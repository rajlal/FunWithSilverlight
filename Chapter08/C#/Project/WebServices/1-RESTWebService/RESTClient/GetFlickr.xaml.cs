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
using System.Threading;
using System.Security;
using System.IO;
using System.Windows.Media.Imaging;
using System.Xml;

namespace RESTClient
{
    public partial class GetFlickr : UserControl
    {
        SynchronizationContext syncContext;
        string statusString;
        PhotoList MyPhotoList = new PhotoList();
        string CurrentPhotoId = "";
        int CurrentIndex = 0;
       
        public GetFlickr()
        {
            InitializeComponent();
        }
        private void ButtonGet_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the data service context and clear any existing bindings.
            ButtonGet.Content = "Loading...";
            ButtonGet.IsEnabled = false;
            MyPhotoList.Clear();

           // Grab SynchronizationContext while on UI Thread   
            syncContext = SynchronizationContext.Current;
            // Create request   
            string searchPhotos = txtMessage.Text;
            HttpWebRequest request =
                WebRequest.Create(new Uri("http://api.flickr.com/services/rest/?method=flickr.photos.search&api_key=eaa91b4b29442d95cb58e286fd43e106&tags=" + searchPhotos, UriKind.Absolute))
                    as HttpWebRequest;
            // Make async call for request stream.  Callback will be called on a background thread.  
            request.Method = "GET"; // we could also use POST if we needed
            // we can also set custom headers on our request as well
            // req.Headers.Headers.Add("x-made-by-silverlight");
            request.BeginGetResponse(new AsyncCallback(ResponseCallback), request);
        }
        private void GetPhoto(string id)
        {
            CurrentPhotoId = id;
            // Instantiate the data service context and clear any existing bindings.
            ButtonGet.Content = "Image...";
            ButtonGet.IsEnabled = false;

            // Grab SynchronizationContext while on UI Thread   
            syncContext = SynchronizationContext.Current;
            // Create request   
            string idPhoto = id;
            HttpWebRequest request =
                WebRequest.Create(new Uri("http://api.flickr.com/services/rest/?method=flickr.photos.getSizes&api_key=eaa91b4b29442d95cb58e286fd43e106&photo_id=" + idPhoto, UriKind.Absolute))
                    as HttpWebRequest;
            // Make async call for request stream.  Callback will be called on a background thread.  
            request.Method = "GET"; // we could also use POST if we needed
            // we can also set custom headers on our request as well
            // req.Headers.Headers.Add("x-made-by-silverlight");
            request.BeginGetResponse(new AsyncCallback(ResponseCallback), request);
        
        }

        private void ResponseCallback(IAsyncResult ar)
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
            syncContext.Post(ExtractResponse, response);
            
        }
        private void ExtractResponse(object state)
        {
            HttpWebResponse response = state as HttpWebResponse;

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                StreamReader responseReader = new StreamReader(response.GetResponseStream());
                string txtMessageText = responseReader.ReadToEnd();
                XmlReader flickrResult = XmlReader.Create(new StringReader(txtMessageText));
                flickrResult.ReadToFollowing("photo");
                if (flickrResult.EOF)
                {
                    flickrResult = XmlReader.Create(new StringReader(txtMessageText));
                    flickrResult.ReadToFollowing("size");
                    while (!flickrResult.EOF)
                    {
                        if (flickrResult.GetAttribute("label") == "Thumbnail")
                        {
                            foreach (PhotoItem p in MyPhotoList)
                            {
                                if (flickrResult.GetAttribute("source").Contains(p.Id))
                                {
                                    p.ImageUrl = flickrResult.GetAttribute("source");
                                    p.Url = flickrResult.GetAttribute("url");
                                }
                                CurrentIndex++;
                            }
                        }
                        flickrResult.ReadToFollowing("size");

                    }
                    UpdateDisplay();
                }
                else
                {
                    while (!flickrResult.EOF)
                    {
                        PhotoItem p = new PhotoItem();
                        p.Id = flickrResult.GetAttribute("id");
                        p.Title = flickrResult.GetAttribute("title");
                        MyPhotoList.Add(p);
                        flickrResult.ReadToFollowing("photo"); // Moves the reader back to the element node.
                    }

                    foreach (PhotoItem p in MyPhotoList)
                    {
                        GetPhoto(p.Id);
                    }
                    ButtonGet.IsEnabled = true;
                    ButtonGet.Content = "GET";
                    UpdateDisplay();

                }
            }
            else
            {
                txtMessage.Text = "Get failed: " + statusString;
                ButtonGet.IsEnabled = true;
                ButtonGet.Content = "GET";
                  
            }
     
        }
        private void UpdateDisplay()
        {
            this.PhotoGrid.Visibility = Visibility.Collapsed;
            this.PhotoGrid.DataContext = MyPhotoList;
            this.PhotoGrid.UpdateLayout();
            this.PhotoGrid.Visibility = Visibility.Visible;
            
        }
        private void ResourceImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Image i = (Image)sender;
            i.Source = new BitmapImage(new Uri("images/error.png", UriKind.Relative));
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
    public class PhotoList : List<PhotoItem>
    {
        PhotoItem si;
        public PhotoItem Val { get { return si; } set { si = value; } }
        public PhotoList() 
        { }
    }
    public class PhotoItem
    {
        private string title;
        private string id;
        private string imageurl ="images/loading.png";
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
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;

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
      
        public PhotoItem()
        { }
        public PhotoItem(String title, String id, string imageurl, string url)
        {
            this.Title = title;
            this.Id= id;
            this.ImageUrl = imageurl;
            this.Url = url;
        }

    }
}
