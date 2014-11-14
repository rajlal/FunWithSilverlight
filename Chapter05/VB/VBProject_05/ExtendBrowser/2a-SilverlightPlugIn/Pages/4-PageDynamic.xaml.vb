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

' Namespace SilverlightPlugIn

Partial Public Class PageDynamic
    Inherits UserControl

    Public Sub New(RectangleColor As String, VariableCustom As String)

        InitializeComponent()
        txtParameter.Text = "Custom Value: " + VariableCustom
        recParameter.Fill = New SolidColorBrush(getColorFromHex(RectangleColor))
    End Sub '   New

    Public Function getColorFromHex(s As String) As Color

        Dim a As Byte = System.Convert.ToByte(s.Substring(0, 2), 16)
        Dim r As Byte = System.Convert.ToByte(s.Substring(2, 2), 16)
        Dim g As Byte = System.Convert.ToByte(s.Substring(4, 2), 16)
        Dim b As Byte = System.Convert.ToByte(s.Substring(6, 2), 16)

        Return Color.FromArgb(a, r, g, b)
    End Function  '   getColorFromHex
End Class   '   PageDynamic
' End Namespace   '   SilverlightPlugIn
' ..\Project_05\ExtendBrowser\2a-SilverlightPlugIn\Pages\4-PageDynamic.xaml.cs
