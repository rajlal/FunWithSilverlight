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

namespace HelloBrowser
{
    public partial class PageCookies : UserControl
    {
        public PageCookies()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (HtmlPage.BrowserInformation.CookiesEnabled)
            {
                if ((bool)chkSaveCookie.IsChecked)
                {
                    // for secure connection 6th parameter ="secure"
                    SaveCookie("name", txtName.Text, 7, "/", "", "");
                    SaveCookie("email", txtEmail.Text, 7, "/", "", "");
                    SaveCookie("web", txtWeb.Text, 7, "/", "", "");
                    txtStatus.Text = "Information saved in Browser Cookies\nRefresh Page or Click Return to see the values";
                }
                else
                    txtStatus.Text = "Information not saved";

                partLogin.Visibility = Visibility.Collapsed;
                partStatus.Visibility = Visibility.Visible;
            }
        }
        private string ReadCookie(string key)
        {
            if (HtmlPage.BrowserInformation.CookiesEnabled)
            {

                string[] arrayCookies = HtmlPage.Document.Cookies.Split(';');

                foreach (string cookie in arrayCookies)
                {
                    string[] cookieKeyValues = cookie.Trim().Split('=');
                    if (cookieKeyValues.Length == 2)
                    {
                        if (cookieKeyValues[0].ToString() == key)
                            return cookieKeyValues[1];
                    }
                }
            }
            return "";
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            txtName.Text = ReadCookie("name");
            txtEmail.Text = ReadCookie("email");
            txtWeb.Text = ReadCookie("web");
            partLogin.Visibility = Visibility.Visible;
            partStatus.Visibility = Visibility.Collapsed;
        }
        private void SaveCookie(string key, string value, int expires, string path, string domain, string secure)
        {
            HtmlDocument myDoc = HtmlPage.Document;
            DateTime expireDate = DateTime.Now + TimeSpan.FromDays(expires);
            DateTime expiration = DateTime.UtcNow + TimeSpan.FromDays(expires);
            string cookie ="";

                cookie = String.Format("{0}={1};expires={2};path={3};domain={4}", key, value, expiration.ToString("R"), path, domain);
            
            HtmlPage.Document.SetProperty("cookie", cookie);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteCookie("name", "/", "");
            DeleteCookie("email", "/", "");
            DeleteCookie("web", "/", "");
            txtStatus.Text = "Cookies Deleted !\nRefresh Page or Click Return to go back";
        }
        private void DeleteCookie(string key, string path, string domain)
        {
            DateTime expiration = DateTime.UtcNow - TimeSpan.FromDays(1);
            string cookie = String.Format("{0}=;expires={1};path={2};domain={3}", key, expiration.ToString("R"), path, domain);
            HtmlPage.Document.SetProperty("cookie", cookie);
            }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            string documentUri = HtmlPage.Document.DocumentUri.OriginalString;
            documentUri = documentUri.Split('?')[0];
            HtmlWindow h = HtmlPage.Window;
            h.Navigate(new Uri(documentUri));
        }
       
    }
}
