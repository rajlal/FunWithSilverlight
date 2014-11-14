Imports System
Imports System.Windows
Imports System.Windows.Controls
' Namespace CustomizeControl

    Public Partial class PageTemplateVSM
        Inherits UserControl

        Public Sub New()

            InitializeComponent()
        End Sub '   New
        private Sub Button_Click(sender As Object, e As Routedeventargs)

            VistaButton.IsEnabled =  Not VistaButton.IsEnabled

            If (VistaButton.IsEnabled) Then
                Disable.Content = "Disable"
            else
                Disable.Content = "Enable"
            End If
        End Sub '   Button_Click
    End Class   '   PageTemplateVSM
' End Namespace
' ..\ExtendControl\E\ExtendControl\ExtendControl\1-CustomizeControl\Pages\4-TemplateVSM.xaml.cs
