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
using System.ComponentModel;

namespace KnowData
{
    public partial class DataTemplate : UserControl
    {
        ScientistList MyScientistList = new ScientistList();

        public DataTemplate()
        {
            InitializeComponent();
        }

        private void ShowTemplate(object sender, MouseButtonEventArgs e)
        {
            TextBlock t = (TextBlock)sender;
            string SelectedWay = t.Text;
            StatusBar.Text = ToolTipService.GetToolTip(t).ToString();
            if (SelectedWay == "ItemTemplate")
            {
                CanvasDataTemplate2.Visibility = Visibility.Collapsed;
                CanvasItemTemplate.Visibility = Visibility.Visible;
                CanvasDataTemplate.Visibility = Visibility.Collapsed;
            }
            else if (SelectedWay == "DataTemplate1")
            {
                CanvasDataTemplate2.Visibility = Visibility.Collapsed;
                CanvasItemTemplate.Visibility = Visibility.Collapsed;
                CanvasDataTemplate.Visibility = Visibility.Visible;
            }
            else if (SelectedWay == "DataTemplate2")
            {
                CanvasDataTemplate2.Visibility = Visibility.Visible;
                CanvasItemTemplate.Visibility = Visibility.Collapsed;
                CanvasDataTemplate.Visibility = Visibility.Collapsed;
              
            }
           
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            CreateScientistList();
            myDisplayList.DataContext = MyScientistList;
            myDisplayListTemplate.DataContext= MyScientistList;
            myDisplayListTemplate2.DataContext = MyScientistList;
        }
        private void CreateScientistList()
        {
            MyScientistList.Clear();
            MyScientistList.Add(new Scientist("Albert Einstein", "Images/Einstein.jpg"));
            MyScientistList.Add(new Scientist("Leonardo da Vinci", "Images/Leonardo.jpg"));
            MyScientistList.Add(new Scientist("Thomas Edison", "Images/Edison.jpg"));
            MyScientistList.Add(new Scientist("Isaac Newton", "Images/Newton.jpg"));
            MyScientistList.Add(new Scientist("Galileo Galilei", "Images/Galileo.jpg"));
        }
    }
    

    //public class ScientistList : List<Scientist>
    //{
    //    Scientist si;
    //    public Scientist Val { get { return si; } set { si = value; } }
    //    public ScientistList()
    //    {
    //    }
    //}
    //public class Scientist
    //{
    //    private string name;
    //    private string imageuri;
    //    public string Name
    //    {
    //        get
    //        {
    //            return name;
    //        }
    //        set
    //        {
    //            name = value;
            
    //        }
    //    }
    //    public string ImageUri
    //    {
    //        get
    //        {
    //            return imageuri;
    //        }
    //        set
    //        {
    //            imageuri = value;
    //        }
    //    }
    //    public Scientist(String name, String imageUri)
    //    {
    //        this.Name = name;
    //        this.ImageUri = imageUri;
    //    }
     
    //}
}
