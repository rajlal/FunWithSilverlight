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
    public partial class PageCreate : UserControl
    {
        SilverlightResourceEntities svcContext;
        private ObservableCollection<ResourceType> resourceTypeBindingCollection; 
        private ObservableCollection<Resource> resourceBindingCollection;
        HtmlWindow h = HtmlPage.Window;
        public PageCreate()
        {
            InitializeComponent();
            //getResources_Click(null, null);
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
            svcContext =  new SilverlightResourceEntities(new Uri("SilverlightResourceDataService.svc", UriKind.Relative));
             resourceBindingCollection.Clear();
             resourceTypeBindingCollection.Clear();
        }
        private void CreateResource_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate the data service context and clear any existing bindings.
            ResetBindingData();

            // Define a URI that returns the product with the specified ID.
            int ResId = Convert.ToInt32(this.cmbResourceType.SelectedIndex) + 1;
            Uri resourceTypeUri = new Uri(svcContext.BaseUri.AbsoluteUri + "/ResourceType(" + ResId + ")");

            // Begin a query operation retrieve the Product object 
            // that is required to add a link to the new resource_Detail.
            svcContext.BeginExecute<ResourceType>(resourceTypeUri, OnQueryCompleted, null); 

        }
        private void OnQueryCompleted(IAsyncResult result)
       {
           // Use the Dispatcher to ensure that the 
           // asynchronous call returns in the correct thread.
           Dispatcher.BeginInvoke(() =>
           {
               // Get the resource returned by the completed query.
               IEnumerable<ResourceType> queryResult = svcContext.EndExecute<ResourceType>(result);
               ResourceType returnedResourceType = queryResult.First();

               // Get the currently selected resource.
               //resources currentresource = (resources)resourcesGrid.SelectedItem;

               // Create a new resource_Details object with the supplied FK values.
               Resource newItem = Resource.CreateResource(0);
               newItem.Title = txtTitle.Text;
               newItem.Author = txtAuthor.Text;
               newItem.Image = txtImage.Text;
               newItem.URL = txtWebsite.Text;

               resourceBindingCollection.Add(newItem);

               // Add the new item to the context.
               svcContext.AddToResource(newItem);

                 // Add the relationship between the resource and the new item.
               returnedResourceType.Resource.Add(newItem);
               svcContext.AddLink(returnedResourceType, "Resource", newItem);

               // Set the reference to the resource from the item.
               newItem.ResourceType = returnedResourceType;
               svcContext.SetLink(newItem, "ResourceType", returnedResourceType);
               Save();
           }
           );
       }        
        private void Save()
        {
            // Start the saving changes operation. This needs to be a 
            // batch operation in case we are added a new object with 
            // a new relationship.
            svcContext.BeginSaveChanges(SaveChangesOptions.Batch, OnChangesSaved, svcContext);
            createResource.Content = "Saving...";
            createResource.IsEnabled = false;
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
                    createResource.Content = "Create Resource";
                    createResource.IsEnabled = true;
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
        
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            txtTitle.Text = "";
            txtAuthor.Text = "";
            txtImage.Text = "";
            txtWebsite.Text = "";
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {

            h.Navigate(new Uri("1-Create.aspx",UriKind.Relative));

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
