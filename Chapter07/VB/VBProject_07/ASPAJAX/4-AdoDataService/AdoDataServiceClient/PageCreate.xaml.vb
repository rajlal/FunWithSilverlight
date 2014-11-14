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

Partial Public Class PageCreate
    Inherits UserControl

    Dim svcContext As SilverlightResourceEntities

    Private resourceTypeBindingCollection As ObservableCollection(Of ResourceType)
    Private resourceBindingCollection As ObservableCollection(Of Resource)

    Dim h As HtmlWindow = HtmlPage.Window

    Public Sub New()
        InitializeComponent()
        ' getResources_Click(null, null)
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

    Private Sub CreateResource_Click(sender As Object, e As RoutedEventArgs)

        '  Instantiate the data service context and clear any existing bindings.
        ResetBindingData()

        '  Define a URI that returns the product with the specified ID.
        Dim ResId As Integer = Convert.ToInt32(Me.cmbResourceType.SelectedIndex) + 1

        Dim resourceTypeUri As Uri = New Uri(svcContext.BaseUri.AbsoluteUri + "/ResourceType(" + CStr(ResId) + ")")

        '  Begin a query operation retrieve the Product object
        '  that is required to add a link to the new resource_Detail.
        svcContext.BeginExecute(Of ResourceType)(resourceTypeUri, AddressOf OnQueryCompleted, Nothing)
    End Sub '   CreateResource_Click

    Private Sub OnQueryCompleted(result As IAsyncResult)

        '  Use the Dispatcher to ensure that the
        '  asynchronous call returns in the correct thread.
        Dispatcher.BeginInvoke(
            Sub()
                '  Get the resource returned by the completed query.
                Dim queryResult As IEnumerable(Of ResourceType) = svcContext.EndExecute(Of ResourceType)(result)

                Dim returnedResourceType As ResourceType = queryResult.First()

                '  Get the currently selected resource.
                ' resources currentresource = (resources)resourcesGrid.SelectedItem

                '  Create a new resource_Details object with the supplied FK values.
                Dim newItem As Resource = Resource.CreateResource(0)

                newItem.Title = txtTitle.Text
                newItem.Author = txtAuthor.Text
                newItem.Image = txtImage.Text
                newItem.URL = txtWebsite.Text

                resourceBindingCollection.Add(newItem)

                '  Add the new item to the context.
                svcContext.AddToResource(newItem)

                '  Add the relationship between the resource and the new item.
                returnedResourceType.Resource.Add(newItem)
                svcContext.AddLink(returnedResourceType, "Resource", newItem)

                '  Set the reference to the resource from the item.
                newItem.ResourceType = returnedResourceType
                svcContext.SetLink(newItem, "ResourceType", returnedResourceType)
                Save()
            End Sub
        )
    End Sub '   OnQueryCompleted

    Private Sub Save()

        '  Start the saving changes operation. This needs to be a
        '  batch operation in case we are added a new object with
        '  a new relationship.
        svcContext.BeginSaveChanges(SaveChangesOptions.Batch, AddressOf OnChangesSaved, svcContext)
        createResource.Content = "Saving..."
        createResource.IsEnabled = False
    End Sub '   Save

    Private Sub OnChangesSaved(result As IAsyncResult)

        '  Use the Dispatcher to ensure that the
        '  asynchronous call returns in the correct thread.
        Dispatcher.BeginInvoke(
            Sub()
                svcContext = CType(result.AsyncState, SilverlightResourceEntities)

                Try
                    '  Complete the save changes operation and display the response.
                    WriteOperationResponse(svcContext.EndSaveChanges(result))
                Catch ex As DataServiceRequestException
                    '  Display the error from the response.
                    WriteOperationResponse(ex.Response)
                Catch ex As InvalidOperationException
                    messageTextBlock.Text = ex.Message
                Finally
                    '  Reload the binding collection to display any new resources.
                    createResource.Content = "Create Resource"
                    createResource.IsEnabled = True
                End Try
            End Sub
                    )
    End Sub '   OnChangesSaved

    Private Sub WriteOperationResponse(response As DataServiceResponse)

        messageTextBlock.Text = String.Empty

        Dim i As Integer = 1

        If (response.IsBatchResponse) Then
            messageTextBlock.Text = String.Format("Batch operation response code: {0}\t", response.BatchStatusCode)
        End If

        For Each change As ChangeOperationResponse In response
            messageTextBlock.Text += String.Format("\tChange {0} code: {1}\t", i.ToString(), change.StatusCode.ToString())

            If (change.Error IsNot Nothing) Then
                String.Format("\tChange {0} error: {1}\t", i.ToString(), change.Error.Message)
            End If

            i += 1
        Next    '   ChangeOperationResponse
    End Sub '   WriteOperationResponse

    Private Sub Reset_Click(sender As Object, e As RoutedEventArgs)

        txtTitle.Text = ""
        txtAuthor.Text = ""
        txtImage.Text = ""
        txtWebsite.Text = ""
    End Sub '   Reset_Click

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
End Class   '   PageCreate
' End Namespace   '   AdoDataServiceClient
' ..\Project_07\ASPAJAX\4-AdoDataService\AdoDataServiceClient\PageCreate.xaml.cs
