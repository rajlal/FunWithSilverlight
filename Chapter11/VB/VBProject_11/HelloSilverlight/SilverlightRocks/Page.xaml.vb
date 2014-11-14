Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Xml
Imports System.Windows.Browser
Imports System.Windows.Markup
Imports System.Windows.Shapes
Imports Microsoft.VisualBasic

Partial Public Class Page
    Inherits UserControl

    Dim ResetFlag As Boolean

    Public Sub New()

        InitializeComponent()
    End Sub

    Private Sub LayoutRoot_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        Dim s As UIElement = CType(e.OriginalSource, UIElement)
        Dim MousePosition As Point = e.GetPosition(LayoutRoot)

        AddSmiley(MousePosition.X, MousePosition.Y)
    End Sub '   LayoutRoot_MouseLeftButtonUp

    Private Sub Smiley_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        If (HtmlPage.Window.Confirm("Delete Smiley?")) Then
            RemoveSmiley(sender)
        End If
    End Sub '   Smiley_MouseLeftButtonUp

    Private Sub Reset_MouseLeftButtonUp(sender As Object, e As MouseButtonEventArgs)

        RemoveAllSmiley()
    End Sub '   Reset_MouseLeftButtonUp

    Private Sub AddSmiley(x As Double, y As Double)

        Dim oxmlReader As XmlReader

        If (CType(x, Integer) Mod 2 = 0) Then
            oxmlReader = XmlReader.Create("files/smiley.xaml")
        Else
            oxmlReader = XmlReader.Create("/SilverlightApplication1;component/files/smileypink.xaml")
        End If

        oxmlReader.MoveToContent()

        Dim MySmiley As UIElement = CType(XamlReader.Load(oxmlReader.ReadOuterXml()), UIElement)

        MySmiley.SetValue(Canvas.TopProperty, y)
        MySmiley.SetValue(Canvas.LeftProperty, x)
        '
        'MySmiley.MouseLeftButtonDown += New MouseButtonEventHandler(Smiley_MouseLeftButtonUp)
        '
        AddHandler MySmiley.MouseLeftButtonDown, AddressOf Smiley_MouseLeftButtonUp
        LayoutRoot.Children.Add(MySmiley)

        If (Not ResetFlag) Then
            ResetText()
        End If
    End Sub '   AddSmiley

    Private Sub ResetText()

        ResetFlag = True
        Reset.Text = "(Click To Reset)"
        '
        'Reset.MouseLeftButtonDown += New MouseButtonEventHandler(Reset_MouseLeftButtonUp)
        '
        AddHandler Reset.MouseLeftButtonDown, AddressOf Reset_MouseLeftButtonUp
    End Sub '   ResetText

    Private Sub RemoveSmiley(sender As Object)

        LayoutRoot.Children.Remove(CType(sender, UIElement))
    End Sub '   RemoveSmiley

    Private Sub RemoveAllSmiley()

        If (HtmlPage.Window.Confirm("Delete All Smiley?")) Then

            Dim TotalCount As Integer = LayoutRoot.Children.Count

            For i As Integer = TotalCount - 1 To 2 Step -1

                LayoutRoot.Children.RemoveAt(i)
            Next    '   i
            ResetFlag = False
            Reset.Text = "(Click Anywhere)"
            '
            'Reset.MouseLeftButtonDown -= New MouseButtonEventHandler(Reset_MouseLeftButtonUp)
            '
            RemoveHandler Reset.MouseLeftButtonDown, AddressOf Reset_MouseLeftButtonUp
        End If
    End Sub '   RemoveAllSmiley
End Class