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

' Namespace PaintBrushes

    Public Partial class PageLinearGradient
        Inherits UserControl

        Public Sub New()

            InitializeComponent()
        End Sub '   New

        private Sub ShowLinearGradient(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            CanvasLinear.Visibility = Visibility.Visible
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   ShowLinearGradient

        private Sub ShowSun(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            CanvasSun.Visibility = Visibility.Visible
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()

        End Sub '   ShowSun

        private Sub ShowShape(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            CanvasShape.Visibility = Visibility.Visible
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   ShowShape

        private Sub ShowPrism(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            CanvasPrism.Visibility = Visibility.Visible
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   ShowPrism

        private Sub CollapseAll()

            CanvasLinear.Visibility = Visibility.Collapsed
            CanvasPrism.Visibility = Visibility.Collapsed
            CanvasShape.Visibility = Visibility.Collapsed
            CanvasSun.Visibility = Visibility.Collapsed
        End Sub '   CollapseAll

    End Class   '   PageLinearGradient

' End Namespace 
' ..\Graphics\G\Graphics\Graphics\3-PaintBrush\Pages\2-PageLinearGradient.xaml.cs
