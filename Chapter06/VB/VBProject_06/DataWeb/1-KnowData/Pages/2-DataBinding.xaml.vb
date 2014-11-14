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
Imports System.Windows.Media.Imaging
Imports System.ComponentModel
Imports System.Windows.Data

' Namespace KnowData

Partial Public Class DataBinding
    Inherits UserControl

    Dim myScientist As SelectedScientist = New SelectedScientist()
    Dim MyScientistList As ScientistListB = New ScientistListB()
    Dim CurrentSelectedScientist As ScientistB = New ScientistB("Not selected", "Images/NoImage.jpg")
    Dim CurrentSelectedScientistIndex As Integer = 0

    Public Sub New()
        InitializeComponent()
        '  for one way binding
        ScientistDetails.DataContext = CurrentSelectedScientist

        '  for two way binding
        Dim MyBinding As Binding = New Binding()
        MyBinding.Path = New PropertyPath("SelectedIndex")
        MyBinding.Mode = BindingMode.TwoWay
        MyBinding.Source = myScientist
        myDisplayListLeft.SetBinding(ListBox.SelectedIndexProperty, MyBinding)
        myDisplayListRight.SetBinding(ListBox.SelectedIndexProperty, MyBinding)
    End Sub '   New

    Private Sub ResetList()

        MyScientistList.Clear()
        MyScientistList.Add(New ScientistB("Einstein", "Images/Einstein.jpg"))
        MyScientistList.Add(New ScientistB("Leonardo", "Images/Leonardo.jpg"))
        MyScientistList.Add(New ScientistB("Edison", "Images/Edison.jpg"))
        MyScientistList.Add(New ScientistB("Newton", "Images/Newton.jpg"))
        MyScientistList.Add(New ScientistB("Galileo", "Images/Galileo.jpg"))
    End Sub '   ResetList

    Private Sub CreateList()

        ResetList()
        UpdateDisplay("List")
    End Sub '   CreateList

    Private Sub UpdateDisplay(type As String)

        myDisplayList.Items.Clear()

        For Each s As ScientistB In MyScientistList

            Dim lbi As ListBoxItem = New ListBoxItem()
            Dim sp As StackPanel = New StackPanel()
            Dim si As Image = New Image()

            si.Source = New BitmapImage(New Uri(s.ImageUri, UriKind.Relative))
            si.Height = 29

            Dim tb As TextBlock = New TextBlock()

            tb.Text = "  " + s.Name
            tb.FontSize = 12
            sp.Orientation = Orientation.Horizontal
            sp.Children.Add(si)
            sp.Children.Add(tb)
            lbi.Content = sp
            AddHandler lbi.MouseLeftButtonUp, AddressOf SetItem  'MouseButtonEventHandler()
            myDisplayList.Items.Add(lbi)
        Next    '   s

        '  For Two Way
        myDisplayListLeft.Items.Clear()
        myDisplayListRight.Items.Clear()

        For Each s As ScientistB In MyScientistList

            Dim lbiLeft As ListBoxItem = New ListBoxItem()

            lbiLeft.Content = s.Name
            AddHandler lbiLeft.MouseLeftButtonUp, AddressOf SetItemTwoWay               ' MouseButtonEventHandler()

            Dim lbiRight As ListBoxItem = New ListBoxItem()

            lbiRight.Content = s.Name
            AddHandler lbiRight.MouseLeftButtonUp, AddressOf SetItemTwoWay                   ' MouseButtonEventHandler()

            myDisplayListLeft.Items.Add(lbiLeft)
            myDisplayListRight.Items.Add(lbiRight)
        Next    '   s
    End Sub '   UpdateDisplay

    Private Sub SetItemTwoWay(sender As Object, e As System.Windows.Input.MouseButtonEventArgs)

        Dim lb As ListBoxItem = CType(sender, ListBoxItem)

        CurrentSelectedScientistIndex = myDisplayListLeft.SelectedIndex
    End Sub '   SetItemTwoWay

    Private Sub SetItem(sender As Object, e As System.Windows.Input.MouseButtonEventArgs)

        Dim lb As ListBoxItem = CType(sender, ListBoxItem)

        Dim sp As StackPanel = CType(lb.Content, StackPanel)
        Dim sImage As Image = CType(sp.Children(0), Image)
        Dim sText As TextBlock = CType(sp.Children(1), TextBlock)

        CurrentSelectedScientist.Name = sText.Text.Trim()
        CurrentSelectedScientist.ImageUri = "Images/" + sText.Text.Trim() + ".jpg"
    End Sub '   SetItem

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        CreateList()
    End Sub '   LayoutRoot_Loaded

    Private Sub WaySelect(sender As Object, e As MouseButtonEventArgs)

        Dim t As TextBlock = CType(sender, TextBlock)
        Dim SelectedWay As String = t.Text

        StatusBar.Text = "Selected: " + ToolTipService.GetToolTip(t).ToString()

        If (SelectedWay = "OneWay Binding") Then
            CanvasOneWay.Visibility = Visibility.Visible
            CanvasTwoWay.Visibility = Visibility.Collapsed
            BindingText.Text = "Binding: " + "Selected Scientist Instance"
        ElseIf (SelectedWay = "TwoWay Binding") Then
            CanvasOneWay.Visibility = Visibility.Collapsed
            CanvasTwoWay.Visibility = Visibility.Visible
            BindingText.Text = "Binding: " + "Selected Index"
        End If
    End Sub '   WaySelect

    Private Sub myDisplayList_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

        myScientist.SelectedIndex = myDisplayListLeft.SelectedIndex
    End Sub '   myDisplayList_SelectionChanged
End Class   '   DataBinding

Public Class ScientistListB
    Inherits List(Of ScientistB)

    Private _si As ScientistB

    Public Property Val() As ScientistB
        Get
            Return _si
        End Get
        Set(value As ScientistB)
            _si = value
        End Set
    End Property

    Public Sub New()

    End Sub '   New
End Class   '   ScientistListB

Public Class ScientistB
    Implements INotifyPropertyChanged
    'Class 'ScientistB' must implement 'Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs)' for interface 'System.ComponentModel.INotifyPropertyChanged'

    Private _Name As String
    Private _Imageuri As String

    Public Property Name() As String
        Get

            Return _Name
        End Get
        Set(value As String)

            _Name = value
            '  Call NotifyPropertyChanged when the source property is updated.
            NotifyPropertyChanged("Name")
        End Set
    End Property

    Public Property ImageUri() As String
        Get

            Return _Imageuri
        End Get
        Set(value As String)

            _Imageuri = value
            '  Call NotifyPropertyChanged when the source property is updated.
            NotifyPropertyChanged("ImageUri")
        End Set
    End Property

    Public Sub New(name As String, imageUri As String)

        Me._Name = name
        Me._ImageUri = imageUri
    End Sub '   New

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Public Sub NotifyPropertyChanged(propertyName As String)

        'If (PropertyChanged IsNot Nothing) Then
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        'End If
    End Sub '   NotifyPropertyChanged
End Class   '   ScientistB

Public Class SelectedScientist
    Implements INotifyPropertyChanged

    Private _selectedIndex As Integer

    '  Declare the PropertyChanged event.
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    '  Create the property that will be the source of the binding.
    Public Property SelectedIndex() As Integer
        Get
            Return _selectedIndex
        End Get
        Set(value As Integer)

            _selectedIndex = value
            NotifyPropertyChanged("SelectedIndex")
        End Set
    End Property

    Public Sub NotifyPropertyChanged(propertyName As String)

        'If (PropertyChanged IsNot Nothing) Then
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        'End If
    End Sub '   NotifyPropertyChanged
End Class   '   SelectedScientist
' End Namespace   '   KnowData
' ..\Project_06\DataWeb\1-KnowData\Pages\2-DataBinding.xaml.cs
