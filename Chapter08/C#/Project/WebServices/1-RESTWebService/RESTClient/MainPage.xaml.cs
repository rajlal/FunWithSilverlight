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
using System.Security;
using System.IO;
using System.Threading;

namespace RESTClient
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
        }
        SynchronizationContext syncContext;

        private void Button_Click(object sender, RoutedEventArgs e)
        {

          
        }
        string statusString;
        //private void RequestStreamCallback(IAsyncResult ar)
        //{
        //    HttpWebRequest request = ar.AsyncState as HttpWebRequest;
        //    request.ContentType = "application/atom+xml";
        //    // Make async call for response.  Callback will be called on a background thread.
        //    request.BeginGetResponse(new AsyncCallback(ResponseCallback), request);

        //}
       

      
       



    }
}
