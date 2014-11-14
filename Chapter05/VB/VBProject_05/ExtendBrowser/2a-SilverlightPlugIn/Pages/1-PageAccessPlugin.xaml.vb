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
Imports System.Windows.Interop

' Namespace SilverlightPlugIn

Partial Public Class PageAccessPlugin
    Inherits UserControl
    Public Sub New()

        InitializeComponent()
    End Sub '   New

    Private Sub LayoutRoot_Loaded(sender As Object, e As RoutedEventArgs)

        Dim host As SilverlightHost = Application.Current.Host

        '  Read-only properties of the Host object.
        Dim pluginBackground As Color = host.Background
        Dim source As Uri = host.Source

        txtValues.Text = "Background: " + pluginBackground.ToString() + Environment.NewLine
        txtValues.Text += ("Source: " + source.ToString().Substring(source.ToString().LastIndexOf("/"c) + 1) + Environment.NewLine)

        '  The Settings object, which represents Web browser settings.
        Dim oSettings As Settings = host.Settings

        txtValues.Text += ("EnableFrameRateCounter: " + oSettings.EnableFrameRateCounter.ToString() + Environment.NewLine)
        txtValues.Text += ("EnableRedrawRegions: " + oSettings.EnableRedrawRegions.ToString() + Environment.NewLine)
        txtValues.Text += ("MaxFrameRate: " + oSettings.MaxFrameRate.ToString() + Environment.NewLine)

        '  Read-only properties of the Settings object.
        Dim windowless As Boolean = oSettings.Windowless
        Dim htmlAccessEnabled As Boolean = oSettings.EnableHTMLAccess

        txtValues.Text += "Windowless Property: " + windowless.ToString() + Environment.NewLine
        txtValues.Text += "HtmlAccessEnabled Property: " + htmlAccessEnabled.ToString() + Environment.NewLine

        '  The Content object, which represents the plug-in display area.
        Dim oContent As Content = host.Content

        '  The read/write IsFullScreen property of the Content object.
        '  See also the Content.FullScreenChanged event.
        Dim isFullScreen As Boolean = oContent.IsFullScreen

        txtValues.Text += "IsFullScreen Property: " + isFullScreen.ToString() + Environment.NewLine
    End Sub '   LayoutRoot_Loaded
End Class   '   PageAccessPlugin
' End Namespace   '   SilverlightPlugIn
' ..\Project_05\ExtendBrowser\2a-SilverlightPlugIn\Pages\1-PageAccessPlugin.xaml.cs
