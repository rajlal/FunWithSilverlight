Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes

' Namespace Layout

    Public Partial class PageLayout
        Inherits UserControl

    Private SelectedItem As String = "Border"
    Private SelectedCanvas As String = "Border"
    Private myStoryboard As Storyboard = New Storyboard()

    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub showMargin(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        CommonItems.Visibility = Visibility.Visible
        CanvasMargin.Visibility = Visibility.Visible
        SelectedCanvas = "Margin"
        rectFixed.Fill = New SolidColorBrush(Colors.Green)
        rectBorderFixed.Fill = New SolidColorBrush(Colors.Green)
        layoutRectangleFixed.BorderThickness = New Thickness(1.0)
        layoutBorderFixed.BorderThickness = New Thickness(1.0)

        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   showMargin


    Private Sub showBorder(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        CommonItems.Visibility = Visibility.Visible
        CanvasBorder.Visibility = Visibility.Visible
        SelectedCanvas = "Border"

        rectFixed.Fill = New SolidColorBrush(getColorFromHex("FF1E90FF"))
        rectBorderFixed.Fill = New SolidColorBrush(getColorFromHex("FF1E90FF"))

        layoutRectangleFixed.BorderThickness = New Thickness(1.0)
        layoutBorderFixed.BorderThickness = New Thickness(1.0)
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   showBorder


    Private Sub showPadding(sender As Object, e As MouseButtonEventArgs)

        CollapseAll()
        CanvasPadding.Visibility = Visibility.Visible
        SelectedCanvas = "Padding"
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   showPadding


    Private Sub CollapseAll()

        CommonItems.Visibility = Visibility.Collapsed
        CanvasMargin.Visibility = Visibility.Collapsed
        CanvasBorder.Visibility = Visibility.Collapsed
        CanvasPadding.Visibility = Visibility.Collapsed
    End Sub '   CollapseAll


    Private Sub sliderBorder_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double))


        If (SelectedItem = "Border") Then
            layoutBorderFixed.BorderThickness = New Thickness(sliderBorder.Value)
        Else
            layoutRectangleFixed.BorderThickness = New Thickness(sliderBorder.Value)
        End If


        bThick.Text = String.Format("{0:0#}", sliderBorder.Value) + " px"
    End Sub '   sliderBorder_ValueChanged


    Private Sub ChangeOption(sender As Object, e As RoutedEventArgs)

        Try

            sliderBorder.Value = 1.0
            sliderMargin.Value = 1.0
            Dim rb As RadioButton = CType(sender, RadioButton)

            If (rb.Name = "FixedBorder") Then

                SelectedItem = "Border"
                layoutRectangleFixed.Visibility = Visibility.Collapsed
                layoutBorderFixed.Visibility = Visibility.Visible

                bWidth.Text = "150 px"
                bHeight.Text = "100 px"
                rWidth.Text = "n/a"
                rHeight.Text = "n/a"



            Else

                SelectedItem = "Rectangle"
                layoutBorderFixed.Visibility = Visibility.Collapsed
                layoutRectangleFixed.Visibility = Visibility.Visible
                bWidth.Text = "n/a"
                bHeight.Text = "n/a"
                rWidth.Text = "150 px"
                rHeight.Text = "100 px"
            End If

        Catch ex As Exception

        End Try


    End Sub '   ChangeOption

    Public Function getColorFromHex(s As String) As Color

        Dim a As Byte = System.Convert.ToByte(s.Substring(0, 2), 16)
        Dim r As Byte = System.Convert.ToByte(s.Substring(2, 2), 16)
        Dim g As Byte = System.Convert.ToByte(s.Substring(4, 2), 16)
        Dim b As Byte = System.Convert.ToByte(s.Substring(6, 2), 16)
        Return Color.FromArgb(a, r, g, b)
    End Function '   sliderMargin_ValueChanged


    Private Sub sliderMargin_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double))

        Try



            If (SelectedItem = "Border") Then
                rectBorderFixed.Margin = New Thickness(sliderMargin.Value)
            Else
                rectFixed.Margin = New Thickness(sliderMargin.Value)
            End If


            bMargin.Text = String.Format("{0:0#}", sliderMargin.Value) + " px"

        Catch ex As Exception

        End Try

    End Sub '   sliderFont_ValueChanged


    Private Sub sliderFont_ValueChanged(sender As Object, e As Routedpropertychangedeventargs(Of Double))

        Try

            txtPaddingFont.Text = "Fontsize: " + String.Format("{0:0#}", sliderFont.Value)
            txtPadding.FontSize = sliderFont.Value
            txtMargin.FontSize = sliderFont.Value

        Catch ex As Exception

        End Try

    End Sub '   sliderValue_ValueChanged


    Private Sub sliderValue_ValueChanged(sender As Object, e As Routedpropertychangedeventargs(Of Double))

        Try

            txtPadding.Padding = New Thickness(sliderValue.Value)
            txtMargin.Margin = New Thickness(sliderValue.Value)

            Dim pad As String = String.Format("{0:0#}", sliderValue.Value)

            txtPaddingValue.Text = "Padding: """ + pad + "," + pad + "," + pad + "," + pad + """"
            txtMarginValue.Text = "Padding: """ + pad + "," + pad + "," + pad + "," + pad + """"

        Catch ex As Exception

        End Try

    End Sub  '   getColorFromHex


End Class   '   PageLayout

' End Namespace 
' ..\Graphics\G\Graphics\Graphics\1-Layout\PageLayout.xaml.cs
