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
Imports System.ServiceModel
Imports System.ServiceModel.DomainServices
Imports System.ServiceModel.DomainServices.Client


'Imports NetRIAService.Web
'Imports System.Windows.Ria.Data
Imports System.Windows.Media.Imaging

' Namespace NetRIAService

Partial Public Class NetRIAService
    Inherits UserControl

    Private _resourceContext As ResourceContext = New ResourceContext()

    Public Sub New()

        InitializeComponent()
        Dim loadop As LoadOperation(Of Resource) = Me._resourceContext.Load(Me._resourceContext.GetResourceQuery())
        Me.dataGrid.ItemsSource = loadop.Entities
    End Sub '   New

    Private Sub ResourceImageFailed(sender As Object, e As ExceptionRoutedEventArgs)

        Dim i As Image = CType(sender, Image)

        i.Source = New BitmapImage(New Uri("images/error.png", UriKind.Relative))

        Dim t As ToolTip = New ToolTip()

        t.Content = New TextBlock() With
        {
            .FontFamily = New FontFamily("Arial"),
            .FontSize = 12,
            .Text = "Error retrieving Image",
            .TextWrapping = TextWrapping.Wrap
        }
        ToolTipService.SetToolTip(i, t)
    End Sub '   ResourceImageFailed
End Class   '   NetRIAService
' End Namespace
' ..\Project_08\WebServices\4-NetRIAService\NetRIAService\NetRIAService.xaml.cs
