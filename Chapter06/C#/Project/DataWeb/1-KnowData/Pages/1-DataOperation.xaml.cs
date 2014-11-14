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
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;

namespace KnowData
{
    public partial class DataOperation : UserControl
    {
        private string CurrentDataStructure="Array";
        private string[] myStringArray ;
        ScientistList MyScientistList = new ScientistList();
        Dictionary<string, string> ScientistDictionary = new Dictionary<string, string>();
        private ObservableCollection<Scientist> ocScientist;
        public ObservableCollection<Scientist> ObservableCollectionScientist
        {
            get
            {
                if (ocScientist == null)
                {
                    ocScientist = new ObservableCollection<Scientist>();
                }
                return ocScientist;
            }
        }

        public DataOperation()
        {
            InitializeComponent();
        }
        private void wmSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "")
            {
                txtSearch.Text = "Search...";
                txtSearch.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
        private void wmSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Search...")
            txtSearch.Text = "";

            txtSearch.Foreground = new SolidColorBrush(Colors.Black);
        }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentDataStructure = "Array";
            CreateArray();
            StatusBar.Text = "Selected: Array Operations";
           
            btnAdd.Visibility = Visibility.Collapsed;
            btnDel.Visibility = Visibility.Collapsed;
        }
        private void SetDataStructure(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = (TextBlock)sender;
            CurrentDataStructure = t.Text;
            StatusBar.Text = "Selected: " + ToolTipService.GetToolTip(t).ToString(); ;
            myDisplayList.Width = 160;

            if (CurrentDataStructure == "Array")
            {
                btnAdd.Visibility = Visibility.Collapsed;
                btnDel.Visibility = Visibility.Collapsed;
               
                CreateArray();
            }
            else if (CurrentDataStructure == "List")
            {
                btnAdd.Visibility = Visibility.Visible;
                btnDel.Visibility = Visibility.Visible;
               
                CreateList();  
            }
            else if (CurrentDataStructure == "Dictionary")
            {
                btnAdd.Visibility = Visibility.Visible;
                btnDel.Visibility = Visibility.Visible;
                myDisplayList.Width = 100;
                CreateDictionary();
            }
            else if (CurrentDataStructure == "ObservableCollection")
            {
                btnAdd.Visibility = Visibility.Visible;
                btnDel.Visibility = Visibility.Visible;
            
                CreateObservableCollection();
            }
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentDataStructure == "Array")
                CreateArray();
            else if (CurrentDataStructure == "List")
                CreateList();
            else if (CurrentDataStructure == "Dictionary")
                CreateDictionary();
            else if (CurrentDataStructure == "ObservableCollection")
                CreateObservableCollection();
        }
        private void Change_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentDataStructure == "Array")
                ChangeArray();
            else if (CurrentDataStructure == "List")
                ChangeList();
            else if (CurrentDataStructure == "Dictionary")
                ChangeDictionary();
            else if (CurrentDataStructure == "ObservableCollection")
                ChangeObservableCollection();
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }
        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentDataStructure == "Array")
                SortArray();
            else if (CurrentDataStructure == "List")
                SortList();
            else if (CurrentDataStructure == "Dictionary")
                SortDictionary();
            else if (CurrentDataStructure == "ObservableCollection")
                SortObservableCollection();
        }
        private void wmSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            Search();
        }
        private void Search()
        {
            if (CurrentDataStructure == "Array")
                SearchArray();
            else if (CurrentDataStructure == "List")
                SearchList();
            else if (CurrentDataStructure == "Dictionary")
                SearchDictionary();
            else if (CurrentDataStructure == "ObservableCollection")
                SearchObservableCollection();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentDataStructure == "List")
                 AddList();
            else if (CurrentDataStructure == "Dictionary")
                AddDictionary();
            else if (CurrentDataStructure == "ObservableCollection")
                AddObservableCollection();
          
        }
        private void Del_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentDataStructure == "List")
                RemoveList();
            else if (CurrentDataStructure == "Dictionary")
                RemoveDictionary();
            else if (CurrentDataStructure == "ObservableCollection")
                RemoveObservableCollection();
        }

        #region Array
        private void CreateArray()
        {
            myStringArray = new string[5] { "Albert Einstein", "Leonardo da Vinci", "Thomas Edison", "Isaac Newton", "Galileo Galilei" };

            UpdateDisplay("Array");
        }
        private void SortArray()
        {

            Array.Sort(myStringArray);

            myDisplayList.Items.Clear();
            foreach (string s in myStringArray)
            {
                ListBoxItem lbI = new ListBoxItem();
                lbI.FontSize = 12;
                lbI.Content = s;
                myDisplayList.Items.Add(lbI);
            }
        }
        private void ChangeArray()
        {

            Array.Reverse(myStringArray);
            myDisplayList.Items.Clear();
            foreach (string s in myStringArray)
            {
                ListBoxItem lbI = new ListBoxItem();
                lbI.Content = s;
                lbI.FontSize = 12;
                myDisplayList.Items.Add(lbI);
            }
        }
        private void SearchArray()
        {

            myDisplayList.Items.Clear();
            string searchText = "";
            if ((txtSearch.Text != "Search...") && (txtSearch.Text.Length > 0))
                searchText = txtSearch.Text;

            foreach (string s in myStringArray)
            {
                ListBoxItem lbI = new ListBoxItem();
                lbI.FontSize = 12;
                if (s.ToLower().Contains(searchText.ToLower()))
                {
                    lbI.Content = s + " <-Search";
                }
                else
                    lbI.Content = s;

                myDisplayList.Items.Add(lbI);
            }

        }

        #endregion 
        #region List
        private void ResetList()
        {
            MyScientistList.Clear();
            MyScientistList.Add(new Scientist("Einstein", "Images/Einstein.jpg"));
            MyScientistList.Add(new Scientist("Leonardo", "Images/Leonardo.jpg"));
            MyScientistList.Add(new Scientist("Edison", "Images/Edison.jpg"));
            MyScientistList.Add(new Scientist("Newton", "Images/Newton.jpg"));
            MyScientistList.Add(new Scientist("Galileo", "Images/Galileo.jpg"));
        }
        private void CreateList()
        {
            ResetList();
            myDisplayList.Items.Clear();
            UpdateDisplay("List");

         }
        private void SortList()
        {
            ScientistNameComparer snc = new ScientistNameComparer();
            MyScientistList.Sort(snc);
            UpdateDisplay("List");
        }
        private void ChangeList()
        {
            if (MyScientistList.Count > 0)
            {
                Scientist s = MyScientistList.ElementAt(0);
                MyScientistList.RemoveAt(0);
                MyScientistList.Add(s);
            }
            UpdateDisplay("List");

        }
        private void SearchList()
        {
           
            string searchText = "";
            ScientistNameComparer snc = new ScientistNameComparer();

            if ((txtSearch.Text != "Search...") && (txtSearch.Text.Length > 0))
                searchText = txtSearch.Text;

            Scientist searchScientist = new Scientist(searchText,"");
            myDisplayList.Items.Clear();
            foreach (Scientist s in MyScientistList)
            {
                ListBoxItem lbi = new ListBoxItem();
                StackPanel sp = new StackPanel();
                Image si = new Image();
                si.Source = new BitmapImage(new Uri(s.ImageUri, UriKind.Relative));
                si.Height = 29;
                TextBlock tb = new TextBlock();
               
                tb.FontSize = 12;
                sp.Orientation = Orientation.Horizontal;
                sp.Children.Add(si);
                sp.Children.Add(tb);
                lbi.Content = sp;

                if (s.Name.ToLower().Contains(searchScientist.Name.ToLower())) 
                {
                    tb.Text = "  " + s.Name + " <-Search";
                }
                else
                {
                    tb.Text = "  " + s.Name;
                }
                myDisplayList.Items.Add(lbi);

            }
            
            }
        private void AddList()
        {
            int insertIndex = myDisplayList.Items.Count ;
            if (myDisplayList.SelectedIndex > -1)
            {
                insertIndex = myDisplayList.SelectedIndex+1;
            }
            MyScientistList.Insert(insertIndex, new Scientist("Darwin", "Images/Darwin.jpg"));
           
            UpdateDisplay("List");
        }
        private void RemoveList()
        {
            if (myDisplayList.Items.Count > 0)
            {
                int removeIndex = myDisplayList.Items.Count-1;
                if (myDisplayList.SelectedIndex > -1)
                {
                    removeIndex = myDisplayList.SelectedIndex;
                }
                MyScientistList.RemoveAt(removeIndex);
            }
            UpdateDisplay("List");
        }
        #endregion
        #region Dictionary
        private void CreateDictionary()
        {
            ScientistDictionary.Clear();
            ScientistDictionary.Add("Einstein", "Images/Einstein.jpg");
            ScientistDictionary.Add("Leonardo", "Images/Leonardo.jpg");
            ScientistDictionary.Add("Edison", "Images/Edison.jpg");
            ScientistDictionary.Add("Newton", "Images/Newton.jpg");
            ScientistDictionary.Add("Galileo", "Images/Galileo.jpg");
            UpdateDisplay("Dictionary");

        }
        private void SortDictionary()
        {
            List<KeyValuePair<string, string>> MyDictionarySList = new List<KeyValuePair<string, string>>(ScientistDictionary);
            ScientistNameDictComparer snc = new ScientistNameDictComparer();
            MyDictionarySList.Sort(snc);
            ScientistDictionary = MyDictionarySList.ToDictionary(s => s.Key, s => s.Value);
            UpdateDisplay("Dictionary");
        }
        private void ChangeDictionary()
        {
            if (ScientistDictionary.Count > 0)
            {
                Scientist s = new Scientist(ScientistDictionary.First().Key, ScientistDictionary.First().Value);
                ScientistDictionary.Remove(s.Name);
                ScientistDictionary.Add(s.Name,s.ImageUri);
            }
            UpdateDisplay("Dictionary");

        }
        private void SearchDictionary()
        {
            string searchText = "";
            if ((txtSearch.Text != "Search...") && (txtSearch.Text.Length > 0))
                searchText = txtSearch.Text;

            if (!ScientistDictionary.ContainsKey(searchText))
            {
                myDisplayList.Items.Clear();
                Dictionary<string, string>.KeyCollection keyCollection = ScientistDictionary.Keys;

                foreach (string s in keyCollection)
                {
                    ListBoxItem lbi = new ListBoxItem();
                    TextBlock tb = new TextBlock();
                    
                    if (s.ToLower().Contains(searchText.ToLower()))
                        tb.Text = "  " + s + " <-Search";
                    else
                        tb.Text = "  " + s;
                    
                    tb.FontSize = 12;
                    lbi.Content = tb;
                    lbi.MouseLeftButtonUp += new MouseButtonEventHandler(GetImagefromDictionary);
                    myDisplayList.Items.Add(lbi);
                }
                string value;
                if (ScientistDictionary.Count > 0)
                {
                    if (ScientistDictionary.TryGetValue(ScientistDictionary.ElementAt(0).Key, out value))
                    {
                        imgDictionary.Source = new BitmapImage(new Uri(value, UriKind.Relative));
                        myDisplayList.SelectedIndex = 0;
                    }
                }
            }

        }
        private void AddDictionary()
        {
            if (!ScientistDictionary.ContainsKey("Darwin"))
            {
                ScientistDictionary.Add("Darwin", "Images/Darwin.jpg");
            }
            UpdateDisplay("Dictionary");

        }
        private void RemoveDictionary()
        {
            ListBoxItem lbi = new ListBoxItem();

            if (myDisplayList.Items.Count == 0) return;
            if (myDisplayList.SelectedIndex > -1)
                lbi = (ListBoxItem)myDisplayList.SelectedItem;
            else
                lbi = (ListBoxItem)myDisplayList.Items[myDisplayList.Items.Count - 1];

            TextBlock tb = (TextBlock)lbi.Content;

            if (ScientistDictionary.ContainsKey(tb.Text.Trim()))
            {
                ScientistDictionary.Remove(tb.Text.Trim());
            }
            UpdateDisplay("Dictionary");
        }
        #endregion
        #region Observable Collection
        private void CreateObservableCollection()
        {
            ObservableCollectionScientist.Clear();
            ObservableCollectionScientist.Add(new Scientist("Einstein", "Images/Einstein.jpg"));
            ObservableCollectionScientist.Add(new Scientist("Leonardo", "Images/Leonardo.jpg"));
            ObservableCollectionScientist.Add(new Scientist("Edison", "Images/Edison.jpg"));
            ObservableCollectionScientist.Add(new Scientist("Newton", "Images/Newton.jpg"));
            ObservableCollectionScientist.Add(new Scientist("Galileo", "Images/Galileo.jpg"));
            UpdateDisplay("ObservableCollection");
        }
        private void SortObservableCollection()
        {
            IEnumerable<Scientist> sl=ocScientist.OrderBy(st => st.Name).ToList();
            myDisplayList.Items.Clear();
            ocScientist.Clear();
            foreach (Scientist s in sl)
            {
                ListBoxItem lbi = new ListBoxItem();
                StackPanel sp = new StackPanel();
                Image si = new Image();
                si.Source = new BitmapImage(new Uri(s.ImageUri, UriKind.Relative));
                si.Height = 29;
                TextBlock tb = new TextBlock();
                tb.Text = "  " + s.Name;
                tb.FontSize = 12;
                sp.Orientation = Orientation.Horizontal;
                sp.Children.Add(si);
                sp.Children.Add(tb);
                lbi.Content = sp;
                myDisplayList.Items.Add(lbi);
                ocScientist.Add(s);
            }

            if (MyScientistList.Count > 0)
                myDisplayList.SelectedIndex = 0;
        }
        private void ChangeObservableCollection()
        {
          if (ObservableCollectionScientist.Count > 0)
           {
            Scientist s = new Scientist(ObservableCollectionScientist[0].Name,ObservableCollectionScientist[0].ImageUri);                
            ObservableCollectionScientist.RemoveAt(0);
            ObservableCollectionScientist.Add(s);
          }
          UpdateDisplay("ObservableCollection");
        }
        private void SearchObservableCollection()
        {

            string searchText = "";
            ScientistNameComparer snc = new ScientistNameComparer();

            if ((txtSearch.Text != "Search...") && (txtSearch.Text.Length > 0))
                searchText = txtSearch.Text;

            myDisplayList.Items.Clear();
            foreach (Scientist s in ObservableCollectionScientist)
            {
                ListBoxItem lbi = new ListBoxItem();
                StackPanel sp = new StackPanel();
                Image si = new Image();
                si.Source = new BitmapImage(new Uri(s.ImageUri, UriKind.Relative));
                si.Height = 29;
                TextBlock tb = new TextBlock();

                tb.FontSize = 12;
                sp.Orientation = Orientation.Horizontal;
                sp.Children.Add(si);
                sp.Children.Add(tb);
                lbi.Content = sp;

                if (s.Name.ToLower().Contains(searchText.ToLower()))
                {
                    tb.Text = "  " + s.Name + " <-Search";
                }
                else
                {
                    tb.Text = "  " + s.Name;
                }
                myDisplayList.Items.Add(lbi);

            }

        }
        private void AddObservableCollection()
        {
            int insertIndex = myDisplayList.Items.Count;
            if (myDisplayList.SelectedIndex > -1)
            {
                insertIndex = myDisplayList.SelectedIndex + 1;
            }
            ObservableCollectionScientist.Insert(insertIndex, new Scientist("Darwin", "Images/Darwin.jpg"));

            UpdateDisplay("ObservableCollection");
        }
        private void RemoveObservableCollection()
        {
            if (myDisplayList.Items.Count > 0)
            {
                int removeIndex = myDisplayList.Items.Count - 1;
                if (myDisplayList.SelectedIndex > -1)
                {
                    removeIndex = myDisplayList.SelectedIndex;
                }
                ObservableCollectionScientist.RemoveAt(removeIndex);
            }
            UpdateDisplay("ObservableCollection");
        }
        #endregion
        private void UpdateDisplay(string type)
        {
            myDisplayList.Items.Clear();

            if (type == "Array")
            {
                foreach (string s in myStringArray)
                {
                    ListBoxItem lbI = new ListBoxItem();
                    lbI.Content = s;
                    lbI.FontSize = 12;
                    myDisplayList.Items.Add(lbI);
                }
                if (myStringArray.Length > 0)
                    myDisplayList.SelectedIndex = 0;
            }
            else if (type == "List")
            {
                foreach (Scientist s in MyScientistList)
                {
                    ListBoxItem lbi = new ListBoxItem();
                    StackPanel sp = new StackPanel();
                    Image si = new Image();
                    si.Source = new BitmapImage(new Uri(s.ImageUri, UriKind.Relative));
                    si.Height = 29;
                    TextBlock tb = new TextBlock();
                    tb.Text = "  " + s.Name;
                    tb.FontSize = 12;
                    sp.Orientation = Orientation.Horizontal;
                    sp.Children.Add(si);
                    sp.Children.Add(tb);
                    lbi.Content = sp;
                    myDisplayList.Items.Add(lbi);
                }

                if (MyScientistList.Count>0 )
                    myDisplayList.SelectedIndex=0;
            }
            else if (type == "Dictionary")
            {
                myDisplayList.Items.Clear();
                Dictionary<string, string>.KeyCollection keyCollection = ScientistDictionary.Keys;

                foreach (string s in keyCollection)
                {
                    ListBoxItem lbi = new ListBoxItem();
                    TextBlock tb = new TextBlock();
                    tb.Text = "  " + s;
                    tb.FontSize = 12;
                    lbi.Content = tb;
                    lbi.MouseLeftButtonUp += new MouseButtonEventHandler(GetImagefromDictionary);
                    myDisplayList.Items.Add(lbi);
                }
                string value;
                if (ScientistDictionary.Count > 0)
                {
                    if (ScientistDictionary.TryGetValue(ScientistDictionary.ElementAt(0).Key, out value))
                    {
                        imgDictionary.Source = new BitmapImage(new Uri(value, UriKind.Relative));
                        myDisplayList.SelectedIndex = 0;
                    }
                }
               
            }
            else if (type == "ObservableCollection")
            {
                foreach (Scientist s in ObservableCollectionScientist)
                {
                    ListBoxItem lbi = new ListBoxItem();
                    StackPanel sp = new StackPanel();
                    Image si = new Image();
                    si.Source = new BitmapImage(new Uri(s.ImageUri, UriKind.Relative));
                    si.Height = 29;
                    TextBlock tb = new TextBlock();
                    tb.Text = "  " + s.Name;
                    tb.FontSize = 12;
                    sp.Orientation = Orientation.Horizontal;
                    sp.Children.Add(si);
                    sp.Children.Add(tb);
                    lbi.Content = sp;
                    myDisplayList.Items.Add(lbi);
                }

                if (MyScientistList.Count > 0)
                    myDisplayList.SelectedIndex = 0;

            }

        }
        private void GetImagefromDictionary(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBoxItem lb = (ListBoxItem)sender;
            TextBlock tb = (TextBlock)lb.Content;
            string value;
            if (ScientistDictionary.TryGetValue( tb.Text.Trim(), out value))
            {
                imgDictionary.Source = new BitmapImage( new Uri(value,UriKind.Relative));
            }
        } 
    }
    public class ScientistNameDictComparer : IComparer<KeyValuePair<string, string>>
    {
        public int Compare(KeyValuePair<string, string> stringfirstPair, KeyValuePair<string, string> stringnextPair)
        {
            return stringfirstPair.Value.CompareTo(stringnextPair.Value);
        }
    }
    public class ScientistNameComparer : IComparer<Scientist>
    {
        public int Compare(Scientist x, Scientist y)
        {
            Scientist first = (Scientist)x;
            Scientist second = (Scientist)y;
            return first.Name.ToLower().CompareTo(second.Name.ToLower());
        }
    }
    public class Scientist 
    {
        public string ImageUri { get; set; }
        public string Name { get; set; }
        public Scientist(String name, String imageUri)
        {
            this.Name = name;
            this.ImageUri = imageUri;
        }
    } 
    public class ScientistList : List<Scientist>
    {
        Scientist si;
        public Scientist Val { get { return si; } set { si = value; } }
        public ScientistList()
        {
        }
    }

}
