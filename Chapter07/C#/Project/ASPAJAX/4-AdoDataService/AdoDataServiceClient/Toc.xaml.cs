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
using System.Windows.Browser;

namespace AdoDataServiceClient
{
    public partial class Toc : UserControl
    {
        HtmlWindow h = HtmlPage.Window;
      
        public Toc()
        {
            InitializeComponent();
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {

            h.Navigate(new Uri("1-Create.aspx", UriKind.Relative));

        }
        private void Read_Click(object sender, RoutedEventArgs e)
        {

           h.Navigate(new Uri("2-Read.aspx", UriKind.Relative));
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {

           h.Navigate(new Uri("3-Update.aspx", UriKind.Relative));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

            h.Navigate(new Uri("4-Delete.aspx", UriKind.Relative));
        }
    }
}
