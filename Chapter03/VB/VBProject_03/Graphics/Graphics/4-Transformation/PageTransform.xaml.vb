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

' Namespace Transformation

    Public Partial class PageTransform
        Inherits UserControl

        Public Sub New()

            InitializeComponent()
        End Sub '   New


    Private Sub sliderRotate_ValueChanged(sender As Object, e As Routedpropertychangedeventargs(Of Double))

        transformRotate.Angle = sliderRotate.Value
        Dim t As ToolTip = New ToolTip()
        t.Content = "Rotate at an Angle:" + CStr(sliderRotate.Value)
        ToolTipService.SetToolTip(sliderRotate, t)
        RotateAngle.Text = "Angle: " + String.Format("{0:0}", sliderRotate.Value)
    End Sub '   sliderRotate_ValueChanged


    Private Sub sliderScaleX_ValueChanged(sender As Object, e As Routedpropertychangedeventargs(Of Double))

        Try

            valueTransformScale.ScaleX = sliderScaleX.Value / 100.0
            Dim t As ToolTip = New ToolTip()
            t.Content = "Scale percentage:" + CStr(sliderScaleX.Value)
            ToolTipService.SetToolTip(sliderScaleX, t)
            ScaleX.Text = "ScaleX: " + String.Format("{0:N}", valueTransformScale.ScaleX)

        Catch ex As Exception

        End Try

    End Sub '   sliderScaleX_ValueChanged

        private Sub sliderScaleY_ValueChanged(sender As Object, e As Routedpropertychangedeventargs(Of double))

            Try

            valueTransformScale.ScaleY = sliderScaleY.Value / 100.0
            Dim t As ToolTip  = New ToolTip()
            t.Content = "Scale percentage:" + CStr(sliderScaleY.Value)
            ToolTipService.SetToolTip(sliderScaleY, t)
            ScaleY.Text = "ScaleY: " + string.Format("{0:N}", valueTransformScale.ScaleY)

            Catch ex As Exception

            End Try 

        End Sub '   sliderScaleY_ValueChanged


        private Sub sliderSkewX_ValueChanged(sender As Object, e As Routedpropertychangedeventargs(Of double))

            Try

                valueSkewTransform.AngleX= sliderSkewX.Value 
                Dim t As ToolTip  = New ToolTip()
            t.Content = "Skew Angle:" + CStr(sliderSkewX.Value)
                ToolTipService.SetToolTip(sliderSkewX, t)
                SkewX.Text = "AngleX: " + string.Format("{0:N}", valueSkewTransform.AngleX)

            Catch ex As Exception

            End Try 

        End Sub '   sliderSkewX_ValueChanged

        private Sub sliderSkewY_ValueChanged(sender As Object, e As Routedpropertychangedeventargs(Of double))

            Try

                valueSkewTransform.AngleY = sliderSkewY.Value
                Dim t As ToolTip  = New ToolTip()
            t.Content = "Skew Angle:" + CStr(sliderSkewY.Value)
                ToolTipService.SetToolTip(sliderSkewY, t)
                SkewY.Text = "AngleY: " + string.Format("{0:N}", valueSkewTransform.AngleY)

            Catch ex As Exception

            End Try 

        End Sub '   sliderSkewY_ValueChanged


        private Sub sliderTranslateX_ValueChanged(sender As Object, e As Routedpropertychangedeventargs(Of double))

            valueTranslateTransform.X = sliderTranslateX.Value
            Dim t As ToolTip  = New ToolTip()
        t.Content = "Move X:" + CStr(sliderTranslateX.Value)
            ToolTipService.SetToolTip(sliderTranslateX, t)
            TranslateX.Text = "Move X: " + string.Format("{0:N}", valueTranslateTransform.X)

        End Sub '   sliderTranslateX_ValueChanged

        private Sub sliderTranslateY_ValueChanged(sender As Object, e As Routedpropertychangedeventargs(Of double))

            valueTranslateTransform.Y= sliderTranslateY.Value
            Dim t As ToolTip  = New ToolTip()
        t.Content = "Move Y:" + CStr(sliderTranslateY.Value)
            ToolTipService.SetToolTip(sliderTranslateY, t)
            TranslateY.Text = "Move Y: " + string.Format("{0:N}", valueTranslateTransform.Y)
        End Sub '   sliderTranslateY_ValueChanged




        private Sub ShowScale(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            CanvasScale.Visibility = Visibility.Visible
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   ShowScale

        private Sub ShowSkew(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            CanvasSkew.Visibility = Visibility.Visible
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   ShowSkew

        private Sub ShowRotate(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            CanvasRotate.Visibility = Visibility.Visible
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   ShowRotate

        private Sub ShowTranslate(sender As Object, e As Mousebuttoneventargs)

            CollapseAll()
            CanvasTranslate.Visibility = Visibility.Visible
            StatusBar.Text = ToolTipService.GetToolTip(CType(sender, TextBlock)).ToString()
        End Sub '   ShowTranslate

        private Sub CollapseAll()

            CanvasRotate.Visibility = Visibility.Collapsed
            CanvasScale.Visibility = Visibility.Collapsed
            CanvasSkew.Visibility = Visibility.Collapsed
            CanvasTranslate.Visibility = Visibility.Collapsed
        End Sub '   CollapseAll


    End Class   '   PageTransform

' End Namespace 
' ..\Graphics\G\Graphics\Graphics\4-Transformation\PageTransform.xaml.cs
