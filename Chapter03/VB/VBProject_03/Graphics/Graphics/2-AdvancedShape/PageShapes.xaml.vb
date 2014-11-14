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
Imports System.Windows.Markup

' Namespace AdvancedShapes

    Public Partial class PageShapes
        Inherits UserControl

        Public Sub New()

            InitializeComponent()
    End Sub '   New

    Private Sub DynamicItemContainer_MouseMove(sender As Object, e As MouseEventArgs)

        Dim tt As ToolTip = New ToolTip()
        ' "
        tt.Content = "X: " + e.GetPosition(DynamicItemContainer).X.ToString() + " Y:" + e.GetPosition(DynamicItemContainer).Y.ToString()
        ToolTipService.SetToolTip(DynamicItemContainer, tt)
        StatusXY.Text = tt.Content.ToString()
    End Sub '   DynamicItemContainer_MouseMove

        private Sub Line_MouseEnter(sender As Object, e As Mouseeventargs)

        Dim l As Line = CType(sender, Line)

        If (l.Opacity = 1) Then
            l.Stroke = New SolidColorBrush(Colors.Blue)
            StatusBar.Text = ToolTipService.GetToolTip(l).ToString()
        End If
    End Sub '   Line_MouseEnter

    Private Sub Line_MouseLeave(sender As Object, e As Mouseeventargs)

        Dim l As Line = CType(sender, Line)

        If (l.Opacity = 1) Then
            l.Stroke = New SolidColorBrush(getColorFromHex("FFCCCCFF"))
        End If
    End Sub '   Line_MouseLeave

    Private Sub gridCheck_Click(sender As Object, e As Routedeventargs)

        updateGrid()
    End Sub '   gridCheck_Click

    Private Sub updateGrid()

        If (CType(chkGrid.IsChecked, Boolean)) Then
            CanvasGridLine.Visibility = Visibility.Visible
            StatusBar.Text = "Gridlines On: 1 unit = 20 px"
        Else
            CanvasGridLine.Visibility = Visibility.Collapsed
            StatusBar.Text = "Gridlines Off"
        End If
    End Sub '   updateGrid

    Public Function getColorFromHex(s As String) As Color

        Dim a As Byte = System.Convert.ToByte(s.Substring(0, 2), 16)
        Dim r As Byte = System.Convert.ToByte(s.Substring(2, 2), 16)
        Dim g As Byte = System.Convert.ToByte(s.Substring(4, 2), 16)
        Dim b As Byte = System.Convert.ToByte(s.Substring(6, 2), 16)
        Return Color.FromArgb(a, r, g, b)
    End Function '   Geometry_MouseLeftButtonUp

    Private Sub Geometry_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        ClearAll()
        CanvasGeometry.Visibility = Visibility.Visible
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   Curve_MouseLeftButtonUp

    Private Sub Curve_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        ClearAll()
        CanvasCurve.Visibility = Visibility.Visible
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   Arc_MouseLeftButtonUp

    Private Sub Arc_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        ClearAll()
        CanvasArc.Visibility = Visibility.Visible
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   YinYan_MouseLeftButtonUp

    Private Sub YinYan_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        ClearAll()
        CanvasYinYan.Visibility = Visibility.Visible
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   Lines_MouseLeftButtonUp

    Private Sub Lines_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        ClearAll()
        CanvasPolyLine.Visibility = Visibility.Visible
        CanvasLinePath.Visibility = Visibility.Visible
        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
    End Sub '   Clear_MouseLeftButtonUp

    Private Sub Clear_MouseLeftButtonUp(sender As Object, e As Mousebuttoneventargs)

        ClearAll()
    End Sub '   ClearAll

    Private Sub ClearAll()

        CanvasLinePath.Visibility = Visibility.Collapsed
        CanvasPolyLine.Visibility = Visibility.Collapsed
        CanvasGeometry.Visibility = Visibility.Collapsed
        CanvasCurve.Visibility = Visibility.Collapsed
        CanvasArc.Visibility = Visibility.Collapsed
        CanvasYinYan.Visibility = Visibility.Collapsed
    End Sub '   Path_Mousemove

    Private Sub Path_Mousemove(sender As Object, e As Mouseeventargs)

        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, Path)).ToString()
    End Sub '   Curve_MouseMove

    Private Sub Curve_MouseMove(sender As Object, e As Mouseeventargs)

        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, Path)).ToString()
    End Sub '   Polyline_Mousemove

    Private Sub Polyline_Mousemove(sender As Object, e As Mouseeventargs)

        StatusBar.Text = ToolTipService.GetToolTip(CType(sender, Polyline)).ToString()
    End Sub '   CG_Path_MouseMove

    Private Sub CG_Path_MouseMove(sender As Object, e As Mouseeventargs)

        StatusBar.Text = "Geometry Group with FillRule=EvenOdd"
    End Sub '   EG_Path_MouseMove

    Private Sub EG_Path_MouseMove(sender As Object, e As Mouseeventargs)

        StatusBar.Text = "Center=80,140 rX=50 rY=50"
    End Sub '   RG_Path_MouseMove

    Private Sub RG_Path_MouseMove(sender As Object, e As Mouseeventargs)

        StatusBar.Text = "Rect=40,20,80,60"
    End Sub '   Ellipse_MouseEnter

    Private Sub Ellipse_MouseEnter(sender As Object, e As Mouseeventargs)

        MainE.Opacity = 0.5
    End Sub '   Ellipse_MouseLeave

    Private Sub Ellipse_MouseLeave(sender As Object, e As Mouseeventargs)

        MainE.Opacity = 1.0
    End Sub '   YYp1_MouseEnter

    Private Sub YYp1_MouseEnter(sender As Object, e As Mouseeventargs)

        YYp1.Opacity = 0.5
    End Sub '   YYp1_MouseLeave

    Private Sub YYp1_MouseLeave(sender As Object, e As Mouseeventargs)

        YYp1.Opacity = 1.0
    End Sub '   YYp2_MouseEnter

    Private Sub YYp2_MouseEnter(sender As Object, e As Mouseeventargs)

        YYp2.Opacity = 0.5
    End Sub '   YYp2_MouseLeave

    Private Sub YYp2_MouseLeave(sender As Object, e As Mouseeventargs)

        YYp2.Opacity = 1.0
    End Sub '   YYp3_MouseEnter

    Private Sub YYp3_MouseEnter(sender As Object, e As Mouseeventargs)

        YYp3.Opacity = 0.5
    End Sub '   YYp3_MouseLeave

    Private Sub YYp3_MouseLeave(sender As Object, e As Mouseeventargs)

        YYp3.Opacity = 1.0
    End Sub  '   getColorFromHex
End Class   '   PageShapes
' End Namespace 
' ..\Graphics\G\Graphics\Graphics\2-AdvancedShape\PageShapes.xaml.cs
