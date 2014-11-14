using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections;
using System.Text;
using System.Windows.Browser;
using System.Windows.Media.Imaging;
using Microsoft.Windows.Controls;
using System.Windows.Resources;

namespace LocalStorage
{
    public partial class PageIsolatedEmbeddedFiles : UserControl
    {
        public PageIsolatedEmbeddedFiles()
        {
            InitializeComponent();
            
        }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            ReadIsolatedStoreTreeView();
        }
        private void ClearAll(object sender, RoutedEventArgs e)
        {
            try
            {
                var store = IsolatedStorageFile.GetUserStoreForApplication();
                store.Remove();
                treeIsolatedRoot.Items.Clear();
            }
            catch (IsolatedStorageException)
            {
                StatusBar.Text = "Error while accessing store";
            }
        }
        private void GetQuota(object sender, RoutedEventArgs e)
        {
            try
            {
                var store = IsolatedStorageFile.GetUserStoreForApplication();
                StatusQuota.Text = "Quota: " + String.Format("{0:###,###}", store.AvailableFreeSpace / 1024) + " KB /" + String.Format("{0:###,###}", store.Quota / 1024) + " KB";
            }
            catch (IsolatedStorageException)
            {
                StatusBar.Text = "Unable to access store";

            }
        }
        private void IncreaseQuota(object sender, RoutedEventArgs e)
        {
            IncreaseQuotaBy(5);
        }
        private void IncreaseQuotaBy(int sizeinMB)
        {
            try
            {
                var store = IsolatedStorageFile.GetUserStoreForApplication();
                Int64 addQuota = 1024000 * sizeinMB;
                Int64 availableQuota = store.AvailableFreeSpace;

                // Increase quota 
                if (store.IncreaseQuotaTo(store.Quota + addQuota))
                {
                    StatusBar.Text = "Quota increased by "+sizeinMB+" MB";
                    GetQuota(null, null);
                }
                else
                {
                    StatusBar.Text = "Quota not increased ";
                }
            }
            catch (IsolatedStorageException)
            {
                StatusBar.Text = "Unable to access store";

            }
        }
        private void ReadIsolatedStoreTreeView()
        {
            try
            {
                IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
                treeIsolatedRoot.Items.Clear();
                // Gather file information
                string[] directoriesInTheRoot = store.GetDirectoryNames();
                string[] filesInTheRoot = store.GetFileNames();

                foreach (string dir in directoriesInTheRoot)
                {
                    TreeViewItem tv1 = new TreeViewItem();
                    tv1.Name = dir;
                    tv1.Header = "[" + dir + "]"; 
                    tv1.IsExpanded = true;
                    treeIsolatedRoot.Items.Add(tv1);
                }
                foreach (string fileName in filesInTheRoot)
                {
                    string lfilename;
                    if (fileName.Length > 20)
                        lfilename = fileName.Substring(0, 8) + "..." + fileName.Substring(fileName.Length - 8);
                    else
                        lfilename = fileName;
         

                    TreeViewItem tv1 = new TreeViewItem();
                    tv1.Name = fileName;
                    tv1.Header = lfilename;
                    tv1.Cursor = Cursors.Hand;
                    treeIsolatedRoot.Items.Add(tv1);
                }

                StatusQuota.Text = "Quota: " + String.Format("{0:###,###}", store.AvailableFreeSpace / 1024) + " KB /" + String.Format("{0:###,###}", store.Quota / 1024) + " KB";
            }
            catch (IsolatedStorageException)
            {
                StatusBar.Text = "Unable to access store";

            }
        }
        private void treeIsolated_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
           TreeViewItem tv1 = new TreeViewItem();
           tv1 = (TreeViewItem)treeIsolated.SelectedItem;
           string tv1Name = tv1.Name;
           if (tv1Name.ToLower().EndsWith(".jpg") || tv1Name.ToLower().EndsWith(".jpeg") || tv1Name.ToLower().EndsWith(".png"))
           {
               DisplayfromIsolatedStorage(tv1Name,"image");
           }
            else if (tv1Name.ToLower().EndsWith(".wmv"))
           {
               DisplayfromIsolatedStorage(tv1Name, "video");
           }
            else 
            {
                DisplayfromIsolatedStorage(tv1Name, "text");
            }

        }
        private void DisplayfromIsolatedStorage(string fName, string type)
        {
            videoContainer.Stop();
            videoContainer.Visibility = Visibility.Collapsed;
            imageContainer.Visibility = Visibility.Collapsed;
            textContainer.Visibility = Visibility.Collapsed;

            string localfName = "";
            if (fName.Length > 20)
                localfName = fName.Substring(0, 8) + "..." + fName.Substring(fName.Length - 8);
            else
                localfName = fName;

            var store = IsolatedStorageFile.GetUserStoreForApplication();
            if (store.FileExists(fName))
            {
                if (type == "image")
                {

                    Stream isoStream = store.OpenFile(fName, FileMode.Open, FileAccess.Read);
                    BitmapImage bi = new BitmapImage();
                    bi.SetSource(isoStream);
                    imageContainer.Source = bi;
                    imageContainer.Visibility = Visibility.Visible;
                
                }
                else if (type == "video")
                {

                    Stream isoStream = store.OpenFile(fName, FileMode.Open, FileAccess.Read);
                    videoContainer.SetSource(isoStream);
                    videoContainer.Play();
                    videoContainer.Visibility = Visibility.Visible;
                }
                else
                {
                    StreamReader reader = new StreamReader(store.OpenFile(fName, FileMode.Open, FileAccess.Read));                    
                    textContainer.Text = reader.ReadToEnd();
                    textContainer.Visibility = Visibility.Visible;
                }
                StatusBar.Text = "Isolated Store:" + localfName;

            }
            else
            {
                StatusBar.Text = "File does not exists:" + localfName;
            }

            

        }
        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = (TextBlock)sender;
            SaveSilverlightFileIsolatedStore(t.Text);

        }
        private void SaveSilverlightFileIsolatedStore(string fname)
        {

            try
            {
                IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();

                // Load an image resource file embedded in the application assembly.
                Uri uri = new Uri(@"LocalStorage;component/Files/" + fname, UriKind.Relative);
                StreamResourceInfo sri = Application.GetResourceStream(uri);
                Stream fileStream = sri.Stream;
                int fileLength = (int)fileStream.Length;
                if (fileLength > store.AvailableFreeSpace)
                {
                    HtmlPage.Window.Alert("Please increase the Quota !");
                }
                else
                {
                    byte[] data = new byte[fileLength];
                    fileStream.Read(data, 0, fileLength);
                    fileStream.Close();
                    IsolatedStorageFileStream mediaFile = store.CreateFile(fname);
                    mediaFile.Write(data, 0, fileLength);
                    mediaFile.Close();
                    ReadIsolatedStoreTreeView();
                    mediaFile.Close();
                }

            }
            catch (IsolatedStorageException)
            {
                StatusBar.Text = "Error while accessing store";
            }

        }

    }
 

}
