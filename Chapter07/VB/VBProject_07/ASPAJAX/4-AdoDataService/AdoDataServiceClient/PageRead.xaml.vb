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
Imports System.Data.Services.Client
Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports AdoDataServiceClient.SilverlightResourceReference
Imports System.Windows.Media.Imaging
Imports System.Windows.Browser

' Namespace AdoDataServiceClient

Partial Public Class PageRead
    Inherits UserControl

    Dim svcContext As SilverlightResourceEntities

    Private resourceTypeBindingCollection As ObservableCollection(Of ResourceType)
    Private resourceBindingCollection As ObservableCollection(Of Resource)

    Dim h As HtmlWindow = HtmlPage.Window

    Public Sub New()
        InitializeComponent()
        getResources_Click(Nothing, Nothing)
    End Sub '   New

    Private Sub ResetBindingData()


        If (resourceBindingCollection Is Nothing) Then
            resourceBindingCollection = New ObservableCollection(Of Resource)()
        End If


        If (resourceTypeBindingCollection Is Nothing) Then
            resourceTypeBindingCollection = New ObservableCollection(Of ResourceType)()
        End If
        '  Create a new data service context.
        svcContext = New SilverlightResourceEntities(New Uri("SilverlightResourceDataService.svc", UriKind.Relative))
        resourceBindingCollection.Clear()
        resourceTypeBindingCollection.Clear()
    End Sub '   ResetBindingData

    Private Sub getResources_Click(sender As Object, e As RoutedEventArgs)

        '  Instantiate the data service context and clear any existing bindings.
        getResources.Content = "Loading..."
        getResources.IsEnabled = False
        ResetBindingData()

        '  Define the filter condition based on the Resource ID.
        Dim ResId As Integer = Convert.ToInt32(Me.cmbResourceType.SelectedIndex) + 1
        Dim filterExpression As String = "Id eq " + ResId.ToString()

        '  Define a query to return the specifed ResourceType and related resources.
        Dim query As DataServiceQuery(Of ResourceType) = svcContext.ResourceType.AddQueryOption("$filter", filterExpression).Expand("Resource")

        Try
            '  Begin the query execution.
            query.BeginExecute(AddressOf OnQueryComplete, query)

        Catch ex As Exception

            messageTextBlock.Text = ex.Message
        End Try
    End Sub '   getResources_Click

    Private Sub OnQueryComplete(result As IAsyncResult)

        '  Use the Dispatcher to ensure that the
        '  asynchronous call returns in the correct thread.
        Dispatcher.BeginInvoke(
            Sub()
                '  Get the original query back from the result.
                Dim query As DataServiceQuery(Of ResourceType) = CType(result.AsyncState, DataServiceQuery(Of ResourceType))

                Try

                    Dim returnedResource As ResourceType = query.EndExecute(result).FirstOrDefault()

                    If (returnedResource IsNot Nothing) Then
                        '  Load the retuned resources into the binding collection.

                        For Each res As Resource In returnedResource.Resource

                            resourceBindingCollection.Add(res)
                        Next    '   res

                        '  Bind the grid control to the collection and update the layout.
                        Me.resourceGrid.DataContext = resourceBindingCollection
                        Me.resourceGrid.UpdateLayout()
                    End If
                Catch ex As DataServiceQueryException
                    Me.messageTextBlock.Text = String.Format("Error: {0} - {1}",
                        ex.Response.StatusCode.ToString(), ex.Response.Error.Message)
                Finally
                    getResources.IsEnabled = True
                End Try
            End Sub
        )
    End Sub '   OnQueryComplete

    Private Sub ResourceImageFailed(sender As Object, e As ExceptionRoutedEventArgs)

        Dim i As Image = CType(sender, Image)

        i.Source = New BitmapImage(New Uri("http://silverlightfun.com/Samples/Chapter-07/AdoData/images/error.jpg", UriKind.Absolute))

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

    Private Sub Create_Click(sender As Object, e As RoutedEventArgs)

        h.Navigate(New Uri("1-Create.aspx", UriKind.Relative))
    End Sub '   Create_Click

    Private Sub Read_Click(sender As Object, e As RoutedEventArgs)

        Dim h As HtmlWindow = HtmlPage.Window

        h.Navigate(New Uri("2-Read.aspx", UriKind.Relative))
    End Sub '   Read_Click

    Private Sub Update_Click(sender As Object, e As RoutedEventArgs)

        h.Navigate(New Uri("3-Update.aspx", UriKind.Relative))
    End Sub '   Update_Click

    Private Sub Delete_Click(sender As Object, e As RoutedEventArgs)

        h.Navigate(New Uri("4-Delete.aspx", UriKind.Relative))
    End Sub '   Delete_Click

    Private Sub Home_Click(sender As Object, e As RoutedEventArgs)

        h.Navigate(New Uri("index.aspx", UriKind.Relative))
    End Sub '   Home_Click
End Class   '   PageRead
' End Namespace   '   AdoDataServiceClient
' ..\Project_07\ASPAJAX\4-AdoDataService\AdoDataServiceClient\PageRead.xaml.cs
