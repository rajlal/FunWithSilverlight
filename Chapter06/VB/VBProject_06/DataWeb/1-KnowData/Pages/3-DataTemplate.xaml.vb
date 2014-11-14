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
Imports System.ComponentModel

' Namespace KnowData

Partial Public Class DataTemplate
    Inherits UserControl

    Dim MyScientistList As ScientistList = New ScientistList()

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub ShowTemplate(sender As Object, e As MouseButtonEventArgs)

        Dim t As TextBlock = CType(sender, TextBlock)
        Dim SelectedWay As String = t.Text

        StatusBar.Text = ToolTipService.GetToolTip(t).ToString()

        If (SelectedWay = "ItemTemplate") Then
            CanvasDataTemplate2.Visibility = Visibility.Collapsed
            CanvasItemTemplate.Visibility = Visibility.Visible
            CanvasDataTemplate.Visibility = Visibility.Collapsed
        ElseIf (SelectedWay = "DataTemplate1") Then
            CanvasDataTemplate2.Visibility = Visibility.Collapsed
            CanvasItemTemplate.Visibility = Visibility.Collapsed
            CanvasDataTemplate.Visibility = Visibility.Visible

        ElseIf (SelectedWay = "DataTemplate2") Then
            CanvasDataTemplate2.Visibility = Visibility.Visible
            CanvasItemTemplate.Visibility = Visibility.Collapsed
            CanvasDataTemplate.Visibility = Visibility.Collapsed
        End If
    End Sub '   ShowTemplate

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        CreateScientistList()
        myDisplayList.DataContext = MyScientistList
        myDisplayListTemplate.DataContext = MyScientistList
        myDisplayListTemplate2.DataContext = MyScientistList
    End Sub '   LayoutRoot_Loaded

    Private Sub CreateScientistList()

        MyScientistList.Clear()
        MyScientistList.Add(New Scientist("Albert Einstein", "Images/Einstein.jpg"))
        MyScientistList.Add(New Scientist("Leonardo da Vinci", "Images/Leonardo.jpg"))
        MyScientistList.Add(New Scientist("Thomas Edison", "Images/Edison.jpg"))
        MyScientistList.Add(New Scientist("Isaac Newton", "Images/Newton.jpg"))
        MyScientistList.Add(New Scientist("Galileo Galilei", "Images/Galileo.jpg"))
    End Sub '   CreateScientistList
End Class   '   DataTemplate

' public class ScientistList : List<Scientist>
' {
'     Scientist si
'     public Scientist Val { get { return si; } set { si = value; } }
'     public ScientistList()
'     {
'     }
' }
' public class Scientist
' {
'     private string name
'     private string imageuri
'     public string Name
'     {
'         get
'         {
'             return name
'         }
'         set
'         {
'             name = value

'         }
'     }
'     public string ImageUri
'     {
'         get
'         {
'             return imageuri
'         }
'         set
'         {
'             imageuri = value
'         }
'     }
'     public Scientist(String name, String imageUri)
'     {
'         this.Name = name
'         this.ImageUri = imageUri
'     }

' }

'Public Class Scientist

'    Public Property _ImageUri As String
'    Public Property _Name As String

'    Public Property Name() As String
'        Get

'            Return _Name
'        End Get
'        Set(value As String)

'            _Name = value
'            '  Call NotifyPropertyChanged when the source property is updated.
'            'NotifyPropertyChanged("Name")
'        End Set
'    End Property

'    Public Property ImageUri() As String
'        Get

'            Return _Imageuri
'        End Get
'        Set(value As String)

'            _Imageuri = value
'            '  Call NotifyPropertyChanged when the source property is updated.
'            'NotifyPropertyChanged("ImageUri")
'        End Set
'    End Property

'    Public Sub New(name As String, imageUri As String)
'        Me._Name = name
'        Me._ImageUri = imageUri
'    End Sub '   New
'End Class   '   Scientist

'Public Class ScientistList
'    Inherits List(Of Scientist)

'    Dim _si As Scientist

'    Public Property Val() As Scientist
'        Get
'            Return _si
'        End Get
'        Set(value As Scientist)
'            _si = value
'        End Set
'    End Property

'    Public Sub New()
'    End Sub '   New
'End Class   '   ScientistList

' End Namespace   '   KnowData
' ..\Project_06\DataWeb\1-KnowData\Pages\3-DataTemplate.xaml.cs
