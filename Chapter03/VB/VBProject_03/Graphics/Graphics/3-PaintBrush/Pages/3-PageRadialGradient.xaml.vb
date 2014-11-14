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

    Public Partial class PageRadialGradient
        Inherits UserControl

        Public Sub New()

            InitializeComponent()
        End Sub '   New


        private Sub ShowMultiple(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            CanvasMultiple.Visibility = Visibility.Visible
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   ShowMultiple


        private Sub ShowRadial(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            CanvasRadial.Visibility = Visibility.Visible
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()

        End Sub '   ShowRadial


        private Sub ShowSun(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            CanvasSun.Visibility = Visibility.Visible
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   ShowSun

      

        private Sub ShowOrb(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            CanvasOrb.Visibility = Visibility.Visible
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   ShowOrb


        private Sub ShowSunrise(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            CanvasSunrise.Visibility = Visibility.Visible
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   ShowSunrise

        private Sub CollapseAll()

            CanvasRadial.Visibility = Visibility.Collapsed
            CanvasMultiple.Visibility = Visibility.Collapsed
            CanvasSun.Visibility = Visibility.Collapsed
            CanvasOrb.Visibility = Visibility.Collapsed
            CanvasSunrise.Visibility = Visibility.Collapsed

        End Sub '   CollapseAll


        private Sub sliderOrigin_ValueChanged(sender As Object, e As Routedpropertychangedeventargs(Of double))

            Try

                Dim sliderValue As Double  = sliderOrgin.Value / 100.0
                RadialOrigin.GradientOrigin = New Point(sliderValue, sliderValue)
                Dim t As ToolTip  = New ToolTip()
            t.Content = "Gradient Origin:" + CStr(sliderValue) + "/" + CStr(sliderValue)
                ToolTipService.SetToolTip(sliderOrgin, t)
                radialOriginText.Text = "Origin:" + string.Format("{0:N}", sliderValue) + "," + string.Format("{0:N}", sliderValue)

            Catch ex As Exception

            End Try 

        End Sub '   sliderOrigin_ValueChanged


        private Sub sliderCenter_ValueChanged(sender As Object, e As Routedpropertychangedeventargs(Of double))

            Try

                Dim sliderValue As Double  = sliderCenter.Value / 100.0
                RadialCenter.Center = New Point(sliderValue, sliderValue)
                Dim t As ToolTip  = New ToolTip()
            t.Content = "Gradient Center:" + CStr(sliderValue) + "/" + CStr(sliderValue)
                ToolTipService.SetToolTip(sliderCenter, t)
                radialCenterText.Text = "Center:" + string.Format("{0:N}", sliderValue) + "," + string.Format("{0:N}", sliderValue)

            Catch ex As Exception

            End Try 

        End Sub '   sliderCenter_ValueChanged


        private Sub sliderRadius_ValueChanged(sender As Object, e As Routedpropertychangedeventargs(Of double))

            Try

                Dim sliderValue As Double  = sliderRadius.Value / 100.0
                RadialRadius.RadiusX = sliderValue
                RadialRadius.RadiusY = sliderValue
                Dim t As ToolTip  = New ToolTip()
            t.Content = "Radius:" + CStr(sliderValue) + "," + CStr(sliderValue)
                ToolTipService.SetToolTip(sliderRadius, t)
                radialRadiusXText.Text = "RadiusX:" + string.Format("{0:N}", sliderValue)
                radialRadiusYText.Text = "RadiusY:" + string.Format("{0:N}", sliderValue)

            Catch ex As Exception

            End Try 

        End Sub '   sliderRadius_ValueChanged

    End Class   '   PageRadialGradient

' End Namespace 
' ..\Graphics\G\Graphics\Graphics\3-PaintBrush\Pages\3-PageRadialGradient.xaml.cs
