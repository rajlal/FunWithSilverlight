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

namespace LocalStorage
{
    public partial class PageIsolatedStore : UserControl
    {
        public PageIsolatedStore()
        {
            InitializeComponent();   
        }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
          ReadIsolatedStoreTreeView();
         }
        private void AddFile(object sender, RoutedEventArgs e)
        {
            try
            {
                var store = IsolatedStorageFile.GetUserStoreForApplication();
                int counter = store.GetFileNames().Length + 1;
                IsolatedStorageFileStream rootFile = store.CreateFile("File" +counter+ ".txt");
                rootFile.Close();
                ReadIsolatedStoreTreeView();
 
            }
            catch (IsolatedStorageException)
            {
                StatusBar.Text = "Unable to access store";

            }
        }
        private void ClearAll(object sender, RoutedEventArgs e)
        {
            try
            {
                var store = IsolatedStorageFile.GetUserStoreForApplication();
                store.Remove();
                ReadIsolatedStoreTreeView();
            }
            catch (IsolatedStorageException)
            {
                StatusBar.Text = "Unable to access store";

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
            try
            {
                var store = IsolatedStorageFile.GetUserStoreForApplication();
                Int64 addQuota = 1024000;
                Int64 availableQuota = store.AvailableFreeSpace;

                    // Increase quota 
                    if (store.IncreaseQuotaTo(store.Quota + addQuota))
                    {
                        StatusBar.Text = "Quota increased by 1 MB";
                        GetQuota(null,null);
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
        private void ResetQuota(object sender, RoutedEventArgs e)
        {
           StatusBar.Text= "AppData\\LocalLow\\Microsoft\\Silverlight\\is";
           ToolTip t = new ToolTip();
           t.Content = "To Reset Quota Delete \n[User\\AppData\\LocalLow\\Microsoft\\Silverlight\\is] \nfolder";
           ToolTipService.SetToolTip(StatusBar, t);
           HtmlPage.Window.Alert("To Reset Quota Delete \n[User\\AppData\\LocalLow\\Microsoft\\Silverlight\\is] \nfolder");
        }
        private void AddDirectory(object sender, RoutedEventArgs e)
        {
            try
            {
                    var store = IsolatedStorageFile.GetUserStoreForApplication();
                    int counter = store.GetDirectoryNames().Length + 1;
                    store.CreateDirectory("Folder" + counter.ToString());
                    ReadIsolatedStoreTreeView();
            }
            catch (IsolatedStorageException)
            {
                StatusBar.Text = "Unable to access store";
            }
            catch (Exception )
            {
                StatusBar.Text = "Error!" ;
            }
        }
        private void ReadIsolatedStoreTreeView()
        {
            try
            {
                var store = IsolatedStorageFile.GetUserStoreForApplication();
                treeIsolatedRoot.Items.Clear();
                // Gather file information
                string[] directoriesInTheRoot = store.GetDirectoryNames();
                string[] filesInTheRoot = store.GetFileNames();

                foreach (string dir in directoriesInTheRoot)
                {
                    TreeViewItem tv1 = new TreeViewItem();
                    tv1.Name = dir;
                    tv1.Header = "[" + dir + "]";
                    if (dir == "Folder1")
                    {
                        string searchpath = Path.Combine("Folder1", "*.*");
                        string[] filesInSubDirs = store.GetFileNames(searchpath);
                        foreach (string fileName in filesInSubDirs)
                        {
                            tv1.Items.Add(fileName);
                        }
                    }
                    tv1.IsExpanded = true;
                    treeIsolatedRoot.Items.Add(tv1);
                }
                foreach (string fileName in filesInTheRoot)
                {
                    TreeViewItem tv1 = new TreeViewItem();
                    tv1.Name = fileName;
                    tv1.Header = fileName;
                    treeIsolatedRoot.Items.Add(tv1);
                }
            }
            catch (IsolatedStorageException)
            {
                StatusBar.Text = "Unable to access store";

            }
        }
        private void AddFileFolder(object sender, RoutedEventArgs e)
        {
             try
            {
                    var store = IsolatedStorageFile.GetUserStoreForApplication();

                    if (store.GetDirectoryNames().Length == 0)
                    {
                        AddDirectory(null, null);
                    }
                    string[] subFiles = store.GetFileNames(Path.Combine("Folder1", "*"));
                    int counter = subFiles.Length + 1;
                    IsolatedStorageFileStream subFolderFile = store.CreateFile(Path.Combine("Folder1", "Subfile" + counter + ".txt"));
                    subFolderFile.Close();
                    ReadIsolatedStoreTreeView();
            }
            catch (IsolatedStorageException)
            {
                StatusBar.Text = "Unable to access store";
            }
            catch (Exception )
            {
                StatusBar.Text = "Error!" ;
            }
        }
    }

}
