using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace WCFClient.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SilverlightResource
    {
        private ResourceInfo Resource;
        public SilverlightResource()
        {
            Resource.Author = "";
            Resource.Title = "";
            Resource.Image = "";
            Resource.URL = "";
            Resource.Type = "";
        }
        [OperationContract]
        public ResourceInfo GetResource(int id)
        {
            GetResourceData(id);
            ResourceInfo localResource = new ResourceInfo();
            localResource.Author = Resource.Author;
            localResource.Title = Resource.Title;
            localResource.Image = Resource.Image;
            localResource.URL = Resource.URL;
            localResource.Type = Resource.Type;
            return localResource;
        }
        private void GetResourceData(int ResourceId)
        {
            // This is where you use your business components. 
            // Method calls on Business components are used to populate the data.

            Resource.Title = "Silverlight 3 How To";
            Resource.Author = "Rajesh Lal";
            Resource.Image = "http://silverlightfun.com/Samples/Chapter-07/AdoData/images/S3HT.jpg";
            Resource.URL = "http://www.amazon.com/Silverlight-3-How-Rajesh-Lal/dp/0672330628";

            try
            {
                string query = "SELECT * FROM Resource where ID=" + ResourceId;

                clsDataAccess mydataAccess = new clsDataAccess();
                mydataAccess.openConnection();
                SqlDataReader mydr = mydataAccess.getData(query);
                if (mydr.HasRows)
                {
                    while (mydr.Read())
                    {
                        Resource.Title = mydr.GetValue(1).ToString();
                        Resource.Author = mydr.GetValue(2).ToString();
                        Resource.Image = mydr.GetValue(3).ToString();
                        Resource.URL = mydr.GetValue(4).ToString();
                        if (Convert.ToInt32(mydr.GetValue(5)) == 1) Resource.Type = "Book";
                        if (Convert.ToInt32(mydr.GetValue(5)) == 2) Resource.Type = "Blog";
                        if (Convert.ToInt32(mydr.GetValue(5)) == 3) Resource.Type = "Tutorial";
                        if (Convert.ToInt32(mydr.GetValue(5)) == 4) Resource.Type = "Video";
              
                    }
                }
                else
                {
                    Resource.Title = "Error: No data for that ID";
                    Resource.Author = "Try other ID's between 1 and 16";
                    Resource.Image = "http://silverlightfun.com/Samples/Chapter-07/AdoData/images/error.jpg";
                    Resource.URL = "http://www.amazon.com/Silverlight-3-How-Rajesh-Lal/dp/0672330628";
                }

                mydr.Close();
                mydataAccess.closeConnection();
            }
            catch (Exception e)
            {
                Resource.Title = "Error: " + e.Message;
                Resource.Author = "Occured while connecting to the Database ";
                Resource.Image = "http://silverlightfun.com/Samples/Chapter-07/AdoData/images/error.jpg";
                Resource.URL = "http://www.amazon.com/Silverlight-3-How-Rajesh-Lal/dp/0672330628";
            }
        }
        public struct ResourceInfo
        {
            public string Author { get; set; }
            public string Title { get; set; }
            public string Image { get; set; }
            public string URL { get; set; }
            public string Type { get; set; }
        }
        // Add more operations here and mark them with [OperationContract]
    }
}
