Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes

' Namespace CommonControls

Partial Public Class PageGrid
    Inherits UserControl

    Public Sub New()

        InitializeComponent()
        CreateGrid()
    End Sub '   New

    Private Sub CreateGrid()

        '  Create a 2x2 dynamic Grid   
        Dim DynamicGrid As Grid = New Grid()
        DynamicGrid.ShowGridLines = True
        DynamicGrid.Height = 100
        DynamicGrid.Margin = New Thickness(10)
        DynamicGrid.ColumnDefinitions.Add(New ColumnDefinition())
        DynamicGrid.ColumnDefinitions.Add(New ColumnDefinition())
        DynamicGrid.RowDefinitions.Add(New RowDefinition())
        DynamicGrid.RowDefinitions.Add(New RowDefinition())

        DynamicGrid.ColumnDefinitions(0).Width = New GridLength(50)
        DynamicGrid.ColumnDefinitions(1).Width = New GridLength(210)
        DynamicGrid.RowDefinitions(0).Height = New GridLength(50)
        DynamicGrid.RowDefinitions(1).Height = New GridLength(50)

        Dim TextDynamic As TextBlock = New TextBlock()
        TextDynamic.Text = "TextBlock in Dynamic Grid"
        TextDynamic.HorizontalAlignment = HorizontalAlignment.Center
        TextDynamic.VerticalAlignment = VerticalAlignment.Center

        DynamicGrid.Children.Add(TextDynamic)
        Grid.SetColumn(TextDynamic, 1)
        Grid.SetRow(TextDynamic, 1)

        LayoutRoot.Children.Add(DynamicGrid)
        Grid.SetColumn(DynamicGrid, 1)
        Grid.SetRow(DynamicGrid, 1)
    End Sub '   CreateGrid
End Class   '   PageGrid
' End Namespace 
' ..\RichInternetApplication\1-CommonControls\Pages\1-Grid.xaml.cs
