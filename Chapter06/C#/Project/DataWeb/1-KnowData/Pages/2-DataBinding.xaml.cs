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
using System.ComponentModel;
using System.Windows.Data;

namespace KnowData
{
    public partial class DataBinding : UserControl
    {
        SelectedScientist myScientist = new SelectedScientist();
        ScientistListB MyScientistList = new ScientistListB();
        ScientistB CurrentSelectedScientist = new ScientistB("Not selected", "Images/NoImage.jpg");
        int CurrentSelectedScientistIndex = 0;
        public DataBinding()
        {
            InitializeComponent();
            // for one way binding
            ScientistDetails.DataContext = CurrentSelectedScientist;

            // for two way binding
            Binding MyBinding = new Binding();
            MyBinding.Path = new PropertyPath("SelectedIndex");
            MyBinding.Mode = BindingMode.TwoWay;
            MyBinding.Source = myScientist;
            myDisplayListLeft.SetBinding(ListBox.SelectedIndexProperty, MyBinding);
            myDisplayListRight.SetBinding(ListBox.SelectedIndexProperty, MyBinding);
        }
        private void ResetList()
        {
            MyScientistList.Clear();
            MyScientistList.Add(new ScientistB("Einstein", "Images/Einstein.jpg"));
            MyScientistList.Add(new ScientistB("Leonardo", "Images/Leonardo.jpg"));
            MyScientistList.Add(new ScientistB("Edison", "Images/Edison.jpg"));
            MyScientistList.Add(new ScientistB("Newton", "Images/Newton.jpg"));
            MyScientistList.Add(new ScientistB("Galileo", "Images/Galileo.jpg"));
        }
        private void CreateList()
        {
            ResetList(); 
            UpdateDisplay("List");

        }
        private void UpdateDisplay(string type)
        {
            myDisplayList.Items.Clear();
           
                foreach (ScientistB s in MyScientistList)
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
                    lbi.MouseLeftButtonUp += new MouseButtonEventHandler(SetItem);
                    myDisplayList.Items.Add(lbi);
                }

            // For Two Way
                myDisplayListLeft.Items.Clear();
                myDisplayListRight.Items.Clear();

                foreach (ScientistB s in MyScientistList)
                {
                    ListBoxItem lbiLeft = new ListBoxItem();
                    lbiLeft.Content = s.Name;
                    lbiLeft.MouseLeftButtonUp += new MouseButtonEventHandler(SetItemTwoWay);

                    ListBoxItem lbiRight = new ListBoxItem();
                    lbiRight.Content = s.Name;
                    lbiRight.MouseLeftButtonUp += new MouseButtonEventHandler(SetItemTwoWay);

                    myDisplayListLeft.Items.Add(lbiLeft);
                    myDisplayListRight.Items.Add(lbiRight);
                }

        }
        private void SetItemTwoWay(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBoxItem lb = (ListBoxItem)sender;
            CurrentSelectedScientistIndex= myDisplayListLeft.SelectedIndex;
        }
        private void SetItem(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListBoxItem lb = (ListBoxItem)sender;
            StackPanel sp = (StackPanel)lb.Content;
            Image sImage = (Image)sp.Children[0];
            TextBlock sText= (TextBlock)sp.Children[1];
            CurrentSelectedScientist.Name = sText.Text.Trim();
            CurrentSelectedScientist.ImageUri = "Images/"+sText.Text.Trim()+".jpg";
        } 
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            CreateList();            
    
        }
        private void WaySelect(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = (TextBlock)sender;
            string SelectedWay = t.Text;
            StatusBar.Text = "Selected: " + ToolTipService.GetToolTip(t).ToString();
            if (SelectedWay == "OneWay Binding")
            {
                CanvasOneWay.Visibility = Visibility.Visible;
                CanvasTwoWay.Visibility = Visibility.Collapsed;
                BindingText.Text = "Binding: " + "Selected Scientist Instance";
            }
            else if (SelectedWay == "TwoWay Binding")
            {
                CanvasOneWay.Visibility = Visibility.Collapsed;
                CanvasTwoWay.Visibility = Visibility.Visible;
                BindingText.Text = "Binding: " + "Selected Index";
            }
        }
        private void myDisplayList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            myScientist.SelectedIndex = myDisplayListLeft.SelectedIndex;
        }
    }
 public class ScientistListB : List<ScientistB>
    {
        ScientistB si;
        public ScientistB Val { get { return si; } set { si = value; } }
        public ScientistListB()
        {
        }
    }
 public class ScientistB  : INotifyPropertyChanged
    {
    private string name;
    private string imageuri;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                // Call NotifyPropertyChanged when the source property is updated.
                NotifyPropertyChanged("Name");
            }
        }
        public string ImageUri
        {
            get
            {
                return imageuri;
            }
            set
            {
                imageuri = value;
                // Call NotifyPropertyChanged when the source property is updated.
                NotifyPropertyChanged("ImageUri");
            }
        }
        public ScientistB(String name, String imageUri)
        {
            this.Name = name;
            this.ImageUri = imageUri;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
 public class SelectedScientist : INotifyPropertyChanged
 {
     private int selectedIndex;

     // Declare the PropertyChanged event.
     public event PropertyChangedEventHandler PropertyChanged;

     // Create the property that will be the source of the binding.
     public int SelectedIndex
     {
         get { return selectedIndex; }
         set
         {
             selectedIndex = value;
             NotifyPropertyChanged("SelectedIndex");
         }
     }
     public void NotifyPropertyChanged(string propertyName)
     {
         if (PropertyChanged != null)
         {
             PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
         }
     }
 }
}
