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

namespace CommonControls
{
    public partial class PageGrid : UserControl
    {
        public PageGrid()
        {
            InitializeComponent();
            CreateGrid();

        }
        private void CreateGrid()
        {
            
            // Create a 2x2 dynamic Grid   
            Grid DynamicGrid = new Grid();
            DynamicGrid.ShowGridLines = true;
            DynamicGrid.Height = 100;
            DynamicGrid.Margin = new Thickness(10);
            DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
            DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
            DynamicGrid.RowDefinitions.Add(new RowDefinition());
            DynamicGrid.RowDefinitions.Add(new RowDefinition());

            DynamicGrid.ColumnDefinitions[0].Width = new GridLength(50);
            DynamicGrid.ColumnDefinitions[1].Width = new GridLength(210);
            DynamicGrid.RowDefinitions[0].Height = new GridLength(50);
            DynamicGrid.RowDefinitions[1].Height = new GridLength(50);


            TextBlock TextDynamic = new TextBlock();
            TextDynamic.Text = "TextBlock in Dynamic Grid";
            TextDynamic.HorizontalAlignment = HorizontalAlignment.Center;
            TextDynamic.VerticalAlignment = VerticalAlignment.Center;


            DynamicGrid.Children.Add(TextDynamic);
            Grid.SetColumn(TextDynamic, 1);
            Grid.SetRow(TextDynamic, 1);

            LayoutRoot.Children.Add(DynamicGrid);
            Grid.SetColumn(DynamicGrid, 1);
            Grid.SetRow(DynamicGrid, 1);
        }


    }
}
