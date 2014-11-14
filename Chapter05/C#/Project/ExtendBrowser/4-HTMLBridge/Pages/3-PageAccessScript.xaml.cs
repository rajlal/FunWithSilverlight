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

namespace HTMLBridge
{
    public partial class PageAccessScript : UserControl
    {
        public PageAccessScript()
        {
            InitializeComponent();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
          //  HtmlDocument doc = HtmlPage.Document;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string strTime = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() ;

            try
            {
                HtmlPage.Window.Invoke("GetTime", strTime);
            }
            catch(Exception)
            {
                HtmlPage.Window.Alert("Error while calling JavaScript Method: GetTime() \nCheck if the function exist in the page");
            }
        }

    }
}
