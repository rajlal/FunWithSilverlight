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


namespace ASMXClient
{
    public partial class MainPage : UserControl
    {
        SilverlightResourceASMXService.SilverlightResourceSoapClient localService = new SilverlightResourceASMXService.SilverlightResourceSoapClient();
            
        public MainPage()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ButtonGet.Content = "Calling...";
                ButtonGet.IsEnabled = false;

                localService.GetResourceCompleted += new EventHandler<SilverlightResourceASMXService.GetResourceCompletedEventArgs>(localService_GetResourceCompleted);
                localService.GetResourceAsync(Convert.ToInt32(txtNum.Value));
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message.ToString();
            }
            finally
                {
                  
            }

        }
        private void localService_GetResourceCompleted(object sender, SilverlightResourceASMXService.GetResourceCompletedEventArgs e)
       {

           if (e.Result.Title.ToString() == "Error: No data for that ID")
           {
               txtMessage.Text = "No data for that ID";
               txtAuthor.Text = "Author: n/a" ;
               txtTitle.Text = e.Result.Title.ToString();
               txtType.Text = "Type: n/a";
               lnkWeb.NavigateUri = new Uri(e.Result.URL.ToString());
               imgResource.Source = new BitmapImage(new Uri(e.Result.Image.ToString(), UriKind.Absolute));
           
           }
           else
           {
               txtMessage.Text = "";
               txtAuthor.Text = "Author: " + e.Result.Author.ToString();
               txtTitle.Text = e.Result.Title.ToString();
               txtType.Text = "Type: " + e.Result.Type.ToString();
               lnkWeb.NavigateUri = new Uri(e.Result.URL.ToString());
               imgResource.Source = new BitmapImage(new Uri(e.Result.Image.ToString(), UriKind.Absolute));
           }
           ButtonGet.Content = "Call Web Service";
           ButtonGet.IsEnabled = true;
          
        }
    }
}
