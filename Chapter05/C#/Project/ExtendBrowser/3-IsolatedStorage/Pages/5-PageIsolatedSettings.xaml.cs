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
using System.Windows.Browser;
using System.IO.IsolatedStorage;

namespace LocalStorage
{
    public partial class PageIsolatedSettings : UserControl
    {
        private IsolatedStorageSettings appSettings = IsolatedStorageSettings.ApplicationSettings;
    
         public PageIsolatedSettings()
        {
            InitializeComponent();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSettings();
         }
        private void go_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           HtmlWindow h = HtmlPage.Window;
           Image i = (Image)sender;
           string myUri = (string)appSettings[i.Name.Substring(5).ToLower()];
           h.Navigate(new Uri(myUri), "_blank");
           
        }
        private void delete_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Image i = (Image)sender;
            appSettings.Remove(i.Name.Substring(6).ToLower());
            LoadSettings();
        }

        private void LoadSettings()
        {
          
            txtHome.Text = "Home";
            txtEmail.Text = "Email";
            txtSearch.Text = "Search";
            txtSilverlight.Text = "Silverlight";

            if (appSettings.Contains("name"))
            {
                txtSettingsName.Text = (string)appSettings["name"];
                txtSettings.Text = (string)appSettings["name"];
            }
            if (appSettings.Contains("home")) txtHome.Text = "" + (string)appSettings["home"];
            if (appSettings.Contains("email")) txtEmail.Text = "" + (string)appSettings["email"];
            if (appSettings.Contains("search")) txtSearch.Text = "" + (string)appSettings["search"];
            if (appSettings.Contains("silverlight")) txtSilverlight.Text = "" + (string)appSettings["silverlight"];
            cmbSettings.SelectedIndex = 0;

        }
        private void ResetSettings()
        {
            appSettings.Clear();
            appSettings.Add("name", "Scott Guthrie");
            appSettings.Add("home", "http://weblogs.asp.net/Scottgu/");
            appSettings.Add("email", "http://hotmail.com");
            appSettings.Add("search", "http://google.com");
            appSettings.Add("silverlight", "http://silverlightfun.com");
            txtSettingsName.Text =(string)appSettings["name"];
            
        }
        private void SaveSettings()
        {
            ComboBoxItem cbi = (ComboBoxItem)cmbSettings.SelectedItem;
            appSettings[cbi.Content.ToString().ToLower()] = txtSettings.Text;
            LoadSettings();

            txtStatus.Foreground = new SolidColorBrush(Colors.Green);
            txtStatus.Text = "Saved !";

        }
        private void cmbSettings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                txtStatus.Foreground = new SolidColorBrush(Colors.Black);
                txtStatus.Text = "Edit Settings";
                ComboBoxItem cbi = (ComboBoxItem)cmbSettings.SelectedItem; 
                txtSettings.Text = (string)appSettings[ cbi.Content.ToString().ToLower()];
            }
            catch(Exception)
            {}
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            SaveSettings();            
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            ResetSettings();
            LoadSettings();
        }

        private void txtSettings_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtStatus.Foreground = new SolidColorBrush(Colors.Black);
            txtStatus.Text = "Edit Settings";
        }
    }
   
}
