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
Imports System.Windows.Browser
Imports System.Windows.Media.Imaging
Imports System.Collections.ObjectModel

' Namespace KnowData

Partial Public Class DataOperation
    Inherits UserControl
    Private CurrentDataStructure As String = "Array"
    Private myStringArray() As String

    Dim MyScientistList As ScientistList = New ScientistList()

    Dim ScientistDictionary As Dictionary(Of String, String) = New Dictionary(Of String, String)
    Private ocScientist As ObservableCollection(Of Scientist)

    Public ReadOnly Property ObservableCollectionScientist() As ObservableCollection(Of Scientist)
        Get
            If (ocScientist Is Nothing) Then
                ocScientist = New ObservableCollection(Of Scientist)
            End If

            Return ocScientist
        End Get
    End Property

    Public Sub New()
        InitializeComponent()
    End Sub '   New

    Private Sub wmSearch_LostFocus(sender As Object, e As RoutedEventArgs)

        txtSearch.Text = "Search..."
        txtSearch.Foreground = New SolidColorBrush(Colors.Gray)
    End Sub '   wmSearch_LostFocus

    Private Sub wmSearch_GotFocus(sender As Object, e As RoutedEventArgs)

        If (txtSearch.Text = "Search...") Then
            txtSearch.Text = ""
        End If

        txtSearch.Foreground = New SolidColorBrush(Colors.Black)
    End Sub '   wmSearch_GotFocus

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        CurrentDataStructure = "Array"
        CreateArray()
        StatusBar.Text = "Selected: Array Operations"

        btnAdd.Visibility = Visibility.Collapsed
        btnDel.Visibility = Visibility.Collapsed
    End Sub '   LayoutRoot_Loaded

    Private Sub SetDataStructure(sender As Object, e As MouseButtonEventArgs)

        Dim t As TextBlock = CType(sender, TextBlock)

        CurrentDataStructure = t.Text
        StatusBar.Text = "Selected: " + ToolTipService.GetToolTip(t).ToString()
        myDisplayList.Width = 160

        If (CurrentDataStructure = "Array") Then
            btnAdd.Visibility = Visibility.Collapsed
            btnDel.Visibility = Visibility.Collapsed
            CreateArray()
        ElseIf (CurrentDataStructure = "List") Then
            btnAdd.Visibility = Visibility.Visible
            btnDel.Visibility = Visibility.Visible
            CreateList()
        ElseIf (CurrentDataStructure = "Dictionary") Then
            btnAdd.Visibility = Visibility.Visible
            btnDel.Visibility = Visibility.Visible
            myDisplayList.Width = 100
            CreateDictionary()
        ElseIf (CurrentDataStructure = "ObservableCollection") Then
            btnAdd.Visibility = Visibility.Visible
            btnDel.Visibility = Visibility.Visible
            CreateObservableCollection()
        End If
    End Sub '   SetDataStructure

    Private Sub Create_Click(sender As Object, e As RoutedEventArgs)

        If (CurrentDataStructure = "Array") Then
            CreateArray()
        ElseIf (CurrentDataStructure = "List") Then
            CreateList()
        ElseIf (CurrentDataStructure = "Dictionary") Then
            CreateDictionary()
        ElseIf (CurrentDataStructure = "ObservableCollection") Then
            CreateObservableCollection()
        End If
    End Sub '   Create_Click

    Private Sub Change_Click(sender As Object, e As RoutedEventArgs)

        If (CurrentDataStructure = "Array") Then
            ChangeArray()
        ElseIf (CurrentDataStructure = "List") Then
            ChangeList()
        ElseIf (CurrentDataStructure = "Dictionary") Then
            ChangeDictionary()
        ElseIf (CurrentDataStructure = "ObservableCollection") Then
            ChangeObservableCollection()
        End If
    End Sub '   Change_Click

    Private Sub Search_Click(sender As Object, e As RoutedEventArgs)

        Search()
    End Sub '   Search_Click

    Private Sub Sort_Click(sender As Object, e As RoutedEventArgs)

        If (CurrentDataStructure = "Array") Then
            SortArray()
        ElseIf (CurrentDataStructure = "List") Then
            SortList()
        ElseIf (CurrentDataStructure = "Dictionary") Then
            SortDictionary()
        ElseIf (CurrentDataStructure = "ObservableCollection") Then
            SortObservableCollection()
        End If
    End Sub '   Sort_Click

    Private Sub wmSearch_KeyUp(sender As Object, e As KeyEventArgs)

        If (e.Key = Key.Enter) Then
            Search()
        End If
    End Sub '   wmSearch_KeyUp

    Private Sub Search()

        If (CurrentDataStructure = "Array") Then
            SearchArray()
        ElseIf (CurrentDataStructure = "List") Then
            SearchList()
        ElseIf (CurrentDataStructure = "Dictionary") Then
            SearchDictionary()
        ElseIf (CurrentDataStructure = "ObservableCollection") Then
            SearchObservableCollection()
        End If
    End Sub '   Search

    Private Sub Add_Click(sender As Object, e As RoutedEventArgs)

        If (CurrentDataStructure = "List") Then
            AddList()
        ElseIf (CurrentDataStructure = "Dictionary") Then
            AddDictionary()
        ElseIf (CurrentDataStructure = "ObservableCollection") Then
            AddObservableCollection()
        End If
    End Sub '   Add_Click

    Private Sub Del_Click(sender As Object, e As RoutedEventArgs)

        If (CurrentDataStructure = "List") Then
            RemoveList()
        ElseIf (CurrentDataStructure = "Dictionary") Then
            RemoveDictionary()
        ElseIf (CurrentDataStructure = "ObservableCollection") Then
            RemoveObservableCollection()
        End If
    End Sub '   Del_Click

#Region "Array"
    Private Sub CreateArray()

        myStringArray = New String() {"Albert Einstein", "Leonardo da Vinci", "Thomas Edison", "Isaac Newton", "Galileo Galilei"}

        UpdateDisplay("Array")
    End Sub '   CreateArray

    Private Sub SortArray()

        Array.Sort(myStringArray)

        myDisplayList.Items.Clear()

        For Each s As String In myStringArray

            Dim lbI As ListBoxItem = New ListBoxItem()
            lbI.FontSize = 12
            lbI.Content = s
            myDisplayList.Items.Add(lbI)
        Next    '   s
    End Sub '   SortArray

    Private Sub ChangeArray()

        Array.Reverse(myStringArray)
        myDisplayList.Items.Clear()

        For Each s As String In myStringArray

            Dim lbI As ListBoxItem = New ListBoxItem()

            lbI.Content = s
            lbI.FontSize = 12
            myDisplayList.Items.Add(lbI)
        Next    '   s
    End Sub '   ChangeArray

    Private Sub SearchArray()

        myDisplayList.Items.Clear()

        Dim searchText As String = ""

        If ((txtSearch.Text <> "Search...") AndAlso (txtSearch.Text.Length > 0)) Then
            searchText = txtSearch.Text
        End If

        For Each s As String In myStringArray

            Dim lbI As ListBoxItem = New ListBoxItem()
            lbI.FontSize = 12

            If (s.ToLower().Contains(searchText.ToLower())) Then
                lbI.Content = s + " <-Search"
            Else
                lbI.Content = s
            End If

            myDisplayList.Items.Add(lbI)
        Next    '   s
    End Sub '   SearchArray
#End Region

#Region "List"
    Private Sub ResetList()

        MyScientistList.Clear()
        MyScientistList.Add(New Scientist("Einstein", "Images/Einstein.jpg"))
        MyScientistList.Add(New Scientist("Leonardo", "Images/Leonardo.jpg"))
        MyScientistList.Add(New Scientist("Edison", "Images/Edison.jpg"))
        MyScientistList.Add(New Scientist("Newton", "Images/Newton.jpg"))
        MyScientistList.Add(New Scientist("Galileo", "Images/Galileo.jpg"))
    End Sub '   ResetList

    Private Sub CreateList()

        ResetList()
        myDisplayList.Items.Clear()
        UpdateDisplay("List")
    End Sub '   CreateList

    Private Sub SortList()

        Dim snc As ScientistNameComparer = New ScientistNameComparer()

        MyScientistList.Sort(snc)
        UpdateDisplay("List")
    End Sub '   SortList

    Private Sub ChangeList()


        If (MyScientistList.Count > 0) Then

            Dim s As Scientist = MyScientistList.ElementAt(0)

            MyScientistList.RemoveAt(0)
            MyScientistList.Add(s)
        End If

        UpdateDisplay("List")
    End Sub '   ChangeList

    Private Sub SearchList()

        Dim searchText As String = ""
        Dim snc As ScientistNameComparer = New ScientistNameComparer()


        If ((txtSearch.Text <> "Search...") AndAlso (txtSearch.Text.Length > 0)) Then
            searchText = txtSearch.Text
        End If

        Dim searchScientist As Scientist = New Scientist(searchText, "")

        myDisplayList.Items.Clear()

        For Each s As Scientist In MyScientistList

            Dim lbi As ListBoxItem = New ListBoxItem()

            Dim sp As StackPanel = New StackPanel()
            Dim si As Image = New Image()

            si.Source = New BitmapImage(New Uri(s.ImageUri, UriKind.Relative))
            si.Height = 29

            Dim tb As TextBlock = New TextBlock()

            tb.FontSize = 12
            sp.Orientation = Orientation.Horizontal
            sp.Children.Add(si)
            sp.Children.Add(tb)
            lbi.Content = sp

            If (s.Name.ToLower().Contains(searchScientist.Name.ToLower())) Then
                tb.Text = "  " + s.Name + " <-Search"
            Else
                tb.Text = "  " + s.Name
            End If

            myDisplayList.Items.Add(lbi)

        Next    '   s
    End Sub '   SearchList

    Private Sub AddList()

        Dim insertIndex As Integer = myDisplayList.Items.Count

        If (myDisplayList.SelectedIndex > -1) Then
            insertIndex = myDisplayList.SelectedIndex + 1
        End If

        MyScientistList.Insert(insertIndex, New Scientist("Darwin", "Images/Darwin.jpg"))

        UpdateDisplay("List")
    End Sub '   AddList

    Private Sub RemoveList()

        If (myDisplayList.Items.Count > 0) Then

            Dim removeIndex As Integer = myDisplayList.Items.Count - 1

            If (myDisplayList.SelectedIndex > -1) Then
                removeIndex = myDisplayList.SelectedIndex
            End If

            MyScientistList.RemoveAt(removeIndex)
        End If

        UpdateDisplay("List")
    End Sub '   RemoveList

#End Region
#Region "Dictionary"
    Private Sub CreateDictionary()

        ScientistDictionary.Clear()
        ScientistDictionary.Add("Einstein", "Images/Einstein.jpg")
        ScientistDictionary.Add("Leonardo", "Images/Leonardo.jpg")
        ScientistDictionary.Add("Edison", "Images/Edison.jpg")
        ScientistDictionary.Add("Newton", "Images/Newton.jpg")
        ScientistDictionary.Add("Galileo", "Images/Galileo.jpg")
        UpdateDisplay("Dictionary")
    End Sub '   CreateDictionary

    Private Sub SortDictionary()

        Dim MyDictionarySList As List(Of KeyValuePair(Of String, String)) = New List(Of KeyValuePair(Of String, String))(ScientistDictionary)

        Dim snc As ScientistNameDictComparer = New ScientistNameDictComparer()

        MyDictionarySList.Sort(snc)
        ScientistDictionary = MyDictionarySList.ToDictionary(Function(s) s.Key, Function(s) s.Value)
        UpdateDisplay("Dictionary")
    End Sub '   SortDictionary

    Private Sub ChangeDictionary()

        If (ScientistDictionary.Count > 0) Then

            Dim s As Scientist = New Scientist(ScientistDictionary.First().Key, ScientistDictionary.First().Value)

            ScientistDictionary.Remove(s.Name)
            ScientistDictionary.Add(s.Name, s.ImageUri)
        End If

        UpdateDisplay("Dictionary")
    End Sub '   ChangeDictionary

    Private Sub SearchDictionary()

        Dim searchText As String = ""

        If ((txtSearch.Text <> "Search...") AndAlso (txtSearch.Text.Length > 0)) Then
            searchText = txtSearch.Text
        End If

        If (Not ScientistDictionary.ContainsKey(searchText)) Then
            myDisplayList.Items.Clear()
            Dim keyCollection As Dictionary(Of String, String).KeyCollection = ScientistDictionary.Keys

            For Each s As String In keyCollection

                Dim lbi As ListBoxItem = New ListBoxItem()

                Dim tb As TextBlock = New TextBlock()

                If (s.ToLower().Contains(searchText.ToLower())) Then
                    tb.Text = "  " + s + " <-Search"
                Else
                    tb.Text = "  " + s
                End If

                tb.FontSize = 12
                lbi.Content = tb
                AddHandler lbi.MouseLeftButtonUp, AddressOf GetImagefromDictionary '  ()MouseButtonEventHandler
                myDisplayList.Items.Add(lbi)
            Next    '   s

            Dim value As String = ""

            If (ScientistDictionary.Count > 0) Then
                If (ScientistDictionary.TryGetValue(ScientistDictionary.ElementAt(0).Key, value)) Then
                    imgDictionary.Source = New BitmapImage(New Uri(value, UriKind.Relative))
                    myDisplayList.SelectedIndex = 0
                End If
            End If
        End If
    End Sub '   SearchDictionary

    Private Sub AddDictionary()

        If (Not ScientistDictionary.ContainsKey("Darwin")) Then
            ScientistDictionary.Add("Darwin", "Images/Darwin.jpg")
        End If

        UpdateDisplay("Dictionary")
    End Sub '   AddDictionary

    Private Sub RemoveDictionary()

        Dim lbi As ListBoxItem = New ListBoxItem()

        If (myDisplayList.Items.Count > 0) Then
            If (myDisplayList.SelectedIndex > -1) Then
                lbi = CType(myDisplayList.SelectedItem, ListBoxItem)
            Else
                lbi = CType(myDisplayList.Items(myDisplayList.Items.Count - 1), ListBoxItem)
            End If

            Dim tb As TextBlock = CType(lbi.Content, TextBlock)

            If (ScientistDictionary.ContainsKey(tb.Text.Trim())) Then
                ScientistDictionary.Remove(tb.Text.Trim())
            End If

            UpdateDisplay("Dictionary")
        End If
    End Sub '   RemoveDictionary

#End Region
#Region "Observable Collection"
    Private Sub CreateObservableCollection()

        ObservableCollectionScientist.Clear()
        ObservableCollectionScientist.Add(New Scientist("Einstein", "Images/Einstein.jpg"))
        ObservableCollectionScientist.Add(New Scientist("Leonardo", "Images/Leonardo.jpg"))
        ObservableCollectionScientist.Add(New Scientist("Edison", "Images/Edison.jpg"))
        ObservableCollectionScientist.Add(New Scientist("Newton", "Images/Newton.jpg"))
        ObservableCollectionScientist.Add(New Scientist("Galileo", "Images/Galileo.jpg"))
        UpdateDisplay("ObservableCollection")
    End Sub '   CreateObservableCollection

    Private Sub SortObservableCollection()

        Dim sl As IEnumerable(Of Scientist) = ocScientist.OrderBy(Function(st) st.Name).ToList()

        myDisplayList.Items.Clear()
        ocScientist.Clear()

        For Each s As Scientist In sl

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
            myDisplayList.Items.Add(lbi)
            ocScientist.Add(s)
        Next    '   s

        If (MyScientistList.Count > 0) Then
            myDisplayList.SelectedIndex = 0
        End If
    End Sub '   SortObservableCollection

    Private Sub ChangeObservableCollection()

        If (ObservableCollectionScientist.Count > 0) Then

            Dim s As Scientist = New Scientist(ObservableCollectionScientist(0).Name, ObservableCollectionScientist(0).ImageUri)

            ObservableCollectionScientist.RemoveAt(0)
            ObservableCollectionScientist.Add(s)
        End If

        UpdateDisplay("ObservableCollection")
    End Sub '   ChangeObservableCollection

    Private Sub SearchObservableCollection()

        Dim searchText As String = ""
        Dim snc As ScientistNameComparer = New ScientistNameComparer()

        If ((txtSearch.Text <> "Search...") AndAlso (txtSearch.Text.Length > 0)) Then
            searchText = txtSearch.Text
        End If

        myDisplayList.Items.Clear()

        For Each s As Scientist In ObservableCollectionScientist

            Dim lbi As ListBoxItem = New ListBoxItem()

            Dim sp As StackPanel = New StackPanel()
            Dim si As Image = New Image()

            si.Source = New BitmapImage(New Uri(s.ImageUri, UriKind.Relative))
            si.Height = 29

            Dim tb As TextBlock = New TextBlock()

            tb.FontSize = 12
            sp.Orientation = Orientation.Horizontal
            sp.Children.Add(si)
            sp.Children.Add(tb)
            lbi.Content = sp

            If (s.Name.ToLower().Contains(searchText.ToLower())) Then
                tb.Text = "  " + s.Name + " <-Search"
            Else
                tb.Text = "  " + s.Name
            End If

            myDisplayList.Items.Add(lbi)
        Next    '   s
    End Sub '   SearchObservableCollection

    Private Sub AddObservableCollection()

        Dim insertIndex As Integer = myDisplayList.Items.Count

        If (myDisplayList.SelectedIndex > -1) Then
            insertIndex = myDisplayList.SelectedIndex + 1
        End If

        ObservableCollectionScientist.Insert(insertIndex, New Scientist("Darwin", "Images/Darwin.jpg"))

        UpdateDisplay("ObservableCollection")
    End Sub '   AddObservableCollection

    Private Sub RemoveObservableCollection()

        If (myDisplayList.Items.Count > 0) Then

            Dim removeIndex As Integer = myDisplayList.Items.Count - 1

            If (myDisplayList.SelectedIndex > -1) Then
                removeIndex = myDisplayList.SelectedIndex
            End If

            ObservableCollectionScientist.RemoveAt(removeIndex)
        End If

        UpdateDisplay("ObservableCollection")
    End Sub '   RemoveObservableCollection

#End Region
    Private Sub UpdateDisplay(type As String)

        myDisplayList.Items.Clear()

        If (type = "Array") Then
            For Each s As String In myStringArray

                Dim lbi As ListBoxItem = New ListBoxItem()
                lbI.Content = s
                lbI.FontSize = 12
                myDisplayList.Items.Add(lbI)
            Next    '   s

            If (myStringArray.Length > 0) Then
                myDisplayList.SelectedIndex = 0
            End If
        ElseIf (type = "List") Then
            For Each s As Scientist In MyScientistList

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
                myDisplayList.Items.Add(lbi)
            Next    '   s

            If (MyScientistList.Count > 0) Then
                myDisplayList.SelectedIndex = 0
            End If
        ElseIf (type = "Dictionary") Then
            myDisplayList.Items.Clear()
            Dim keyCollection As Dictionary(Of String, String).KeyCollection = ScientistDictionary.Keys

            For Each s As String In keyCollection

                Dim lbi As ListBoxItem = New ListBoxItem()

                Dim tb As TextBlock = New TextBlock()

                tb.Text = "  " + s
                tb.FontSize = 12
                lbi.Content = tb
                AddHandler lbi.MouseLeftButtonUp, AddressOf GetImagefromDictionary  'MouseButtonEventHandler()
                myDisplayList.Items.Add(lbi)
            Next    '   s

            Dim value As String = ""

            If (ScientistDictionary.Count > 0) Then
                If (ScientistDictionary.TryGetValue(ScientistDictionary.ElementAt(0).Key, value)) Then
                    imgDictionary.Source = New BitmapImage(New Uri(value, UriKind.Relative))
                    myDisplayList.SelectedIndex = 0
                End If
            End If
        ElseIf (type = "ObservableCollection") Then
            For Each s As Scientist In ObservableCollectionScientist

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
                myDisplayList.Items.Add(lbi)
            Next    '   s

            If (MyScientistList.Count > 0) Then
                myDisplayList.SelectedIndex = 0
            End If
        End If
    End Sub '   UpdateDisplay

    Private Sub GetImagefromDictionary(sender As Object, e As System.Windows.Input.MouseButtonEventArgs)

        Dim lb As ListBoxItem = CType(sender, ListBoxItem)

        Dim tb As TextBlock = CType(lb.Content, TextBlock)

        Dim value As String = ""

        If (ScientistDictionary.TryGetValue(tb.Text.Trim(), value)) Then
            imgDictionary.Source = New BitmapImage(New Uri(value, UriKind.Relative))
        End If
    End Sub '   GetImagefromDictionary
End Class   '   DataOperation

Public Class ScientistNameDictComparer
    Implements IComparer(Of KeyValuePair(Of String, String))
    'Error	1	Class 'ScientistNameDictComparer' must implement 'Function Compare(x As KeyValuePair(Of String, String), y As KeyValuePair(Of String, String)) As Integer' 
    'for interface 'System.Collections.Generic.IComparer(Of System.Collections.Generic.KeyValuePair(Of String, String))'.	'
    'C:\Users\Main OutDoor\Documents\Visual Studio 2010\Projects\S\Silverlight\VBProject_06\DataWeb\1-KnowData\Pages\1-DataOperation.xaml.vb	711	16	1-KnowData

    Public Function Compare(stringfirstPair As KeyValuePair(Of String, String), stringnextPair As KeyValuePair(Of String, String)) As Integer Implements IComparer(Of KeyValuePair(Of String, String)).Compare

        Return stringfirstPair.Value.CompareTo(stringnextPair.Value)
    End Function  '   Compare
End Class   '   ScientistNameDictComparer

Public Class ScientistNameComparer
    Implements IComparer(Of Scientist)

    Public Function Compare(x As Scientist, y As Scientist) As Integer Implements IComparer(Of Scientist).Compare

        Dim first As Scientist = CType(x, Scientist)
        Dim second As Scientist = CType(y, Scientist)

        Return first.Name.ToLower().CompareTo(second.Name.ToLower())
    End Function  '   Compare
End Class   '   ScientistNameComparer

Public Class Scientist

    Public Property ImageUri As String
    Public Property Name As String

    Public Sub New(name As String, imageUri As String)
        Me._Name = name
        Me._ImageUri = imageUri
    End Sub '   New
End Class   '   Scientist

Public Class ScientistList
    Inherits List(Of Scientist)

    Dim si As Scientist

    Public Sub New()
    End Sub '   New
End Class   '   ScientistList

' End Namespace   '   KnowData
' ..\Project_06\DataWeb\1-KnowData\Pages\1-DataOperation.xaml.cs
