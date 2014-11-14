using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Services.Client;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using AdoDataServiceClient.SilverlightResourceReference;
using System.Windows.Media.Imaging;
using System.Windows.Browser;
 
namespace AdoDataServiceClient
{
    public partial class PageDelete: UserControl
    {
        SilverlightResourceEntities svcContext;
        private ObservableCollection<ResourceType> resourceTypeBindingCollection; 
        private ObservableCollection<Resource> resourceBindingCollection;
        HtmlWindow h = HtmlPage.Window;
        
        public PageDelete()
        {
            InitializeComponent();
            getResources_Click(null, null);
        }
        private void ResetBindingData()
        {
            if (resourceBindingCollection == null)
            {
                resourceBindingCollection = new ObservableCollection<Resource>();
            }
            if (resourceTypeBindingCollection == null)
            {
                resourceTypeBindingCollection = new ObservableCollection<ResourceType>();
            }
            // Create a new data service context.
            svcContext = new SilverlightResourceEntities(new Uri("SilverlightResourceDataService.svc", UriKind.Relative));
            resourceBindingCollection.Clear();
            resourceTypeBindingCollection.Clear();
        }
        private void getResources_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the data service context and clear any existing bindings.
            getResources.Content = "Loading...";
            getResources.IsEnabled = false;
            ResetBindingData();

            // Define the filter condition based on the Resource ID.
            int ResId = Convert.ToInt32(this.cmbResourceType.SelectedIndex) + 1;
            string filterExpression = "Id eq " + ResId.ToString();

            // Define a query to return the specifed ResourceType and related resources.
            DataServiceQuery<ResourceType> query =  svcContext.ResourceType.AddQueryOption("$filter", filterExpression).Expand("Resource");

            try
            {
                // Begin the query execution.
                query.BeginExecute(OnQueryComplete, query);
            }
            catch (Exception ex)
            {
                messageTextBlock.Text = ex.Message;
            }
        }
        private void OnQueryComplete(IAsyncResult result)
        {
            // Use the Dispatcher to ensure that the 
            // asynchronous call returns in the correct thread.
            Dispatcher.BeginInvoke(() =>
            {
                // Get the original query back from the result.
                DataServiceQuery<ResourceType> query =
                    result.AsyncState as DataServiceQuery<ResourceType>;

                try
                {
                    ResourceType returnedResourceType =
                        query.EndExecute(result).FirstOrDefault();

                    if (returnedResourceType != null)
                    {
                        // Load the retuned resources into the binding collection.
                        foreach (Resource res in returnedResourceType.Resource)
                        {
                            resourceBindingCollection.Add(res);
                        }

                        // Bind the grid control to the collection and update the layout.
                        this.resourceGrid.DataContext = resourceBindingCollection;
                        this.resourceGrid.UpdateLayout();
                    }
                }
                catch (DataServiceQueryException ex)
                {
                    this.messageTextBlock.Text = string.Format("Error: {0} - {1}",
                        ex.Response.StatusCode.ToString(), ex.Response.Error.Message);
                }
                finally {
                    getResources.Content = "Look up";
                    getResources.IsEnabled = true;
                }

            }
            );
        }
        private void DeleteResource_Click(object sender, RoutedEventArgs e)
        {
            if (System.Windows.Browser.HtmlPage.Window.Confirm("Ok to delete?"))
            { 
                Resource deletedResource = (Resource)resourceGrid.SelectedItem;
                // Delete the selected resource.
                svcContext.DeleteObject(deletedResource);
                // Remove the deleted resource from the binding collection.
                resourceGrid.SelectedIndex = resourceGrid.SelectedIndex == 0 ? 1 : resourceGrid.SelectedIndex - 1;
                resourceBindingCollection.Remove(deletedResource);

                //Save back
                Save();
            }
        }
        private void Save()
        {
            // Start the saving changes operation. This needs to be a 
            // batch operation in case we are added a new object with 
            // a new relationship.
            svcContext.BeginSaveChanges(SaveChangesOptions.Batch, OnChangesSaved, svcContext);
            deleteResource.Content = "Saving...";
            deleteResource.IsEnabled = false;
        }
        private void ResourceImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            Image i = (Image)sender;
            i.Source = new BitmapImage(new Uri("http://silverlightfun.com/Samples/Chapter-07/AdoData/images/error.jpg", UriKind.Absolute));
            ToolTip t = new ToolTip();
            t.Content = new TextBlock()
            {
                FontFamily = new FontFamily("Arial"),
                FontSize = 12,
                Text = "Error retrieving Image",
                TextWrapping = TextWrapping.Wrap
            };
            ToolTipService.SetToolTip(i, t);
        }
     
        private void OnChangesSaved(IAsyncResult result)
        {
            // Use the Dispatcher to ensure that the 
            // asynchronous call returns in the correct thread.
            Dispatcher.BeginInvoke(() =>
            {
                svcContext = result.AsyncState as SilverlightResourceEntities;

                try
                {
                    // Complete the save changes operation and display the response.
                    WriteOperationResponse(svcContext.EndSaveChanges(result));
                }
                catch (DataServiceRequestException ex)
                {
                    // Display the error from the response.
                    WriteOperationResponse(ex.Response);
                }
                catch (InvalidOperationException ex)
                {
                    messageTextBlock.Text = ex.Message;
                }
                finally
                {
                    // Reload the binding collection to display any new resources.
                    deleteResource.Content = "Delete Selected";
                    deleteResource.IsEnabled = true;
                }
            }
            );
        }
        private void WriteOperationResponse(DataServiceResponse response)
        {
            messageTextBlock.Text = string.Empty;
            int i = 1;

            if (response.IsBatchResponse)
            {
                messageTextBlock.Text = string.Format("Batch operation response code: {0}\t",   response.BatchStatusCode);
            }
            foreach (ChangeOperationResponse change in response)
            {
                messageTextBlock.Text += string.Format("\tChange {0} code: {1}\t",  i.ToString(), change.StatusCode.ToString());
                if (change.Error != null)
                {
                    string.Format("\tChange {0} error: {1}\t", i.ToString(), change.Error.Message);
                }
                i++;
            }
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {

            h.Navigate(new Uri("1-Create.aspx", UriKind.Relative));

        }
        private void Read_Click(object sender, RoutedEventArgs e)
        {

            HtmlWindow h = HtmlPage.Window;
            h.Navigate(new Uri("2-Read.aspx", UriKind.Relative));
        }
        private void Update_Click(object sender, RoutedEventArgs e)
        {

            h.Navigate(new Uri("3-Update.aspx", UriKind.Relative));
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {

            h.Navigate(new Uri("4-Delete.aspx", UriKind.Relative));
        }
        private void Home_Click(object sender, RoutedEventArgs e)
        {

            h.Navigate(new Uri("index.aspx", UriKind.Relative));
        }

    }
}
