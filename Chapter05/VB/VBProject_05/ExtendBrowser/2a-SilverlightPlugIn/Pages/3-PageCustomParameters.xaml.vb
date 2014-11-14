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

Partial Public Class PageCustomParameters
    Inherits UserControl

    Public Sub New(e As StartupEventArgs)

        InitializeComponent()

        For Each key As String In e.InitParams.Keys
            txtValues.Text += key + ": " + e.InitParams(key).ToString() + "\n"
        Next    '   key
    End Sub '   New

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

    End Sub '   LayoutRoot_Loaded
End Class   '   PageCustomParameters
' End Namespace   '   SilverlightPlugIn
' ..\Project_05\ExtendBrowser\2a-SilverlightPlugIn\Pages\3-PageCustomParameters.xaml.cs
