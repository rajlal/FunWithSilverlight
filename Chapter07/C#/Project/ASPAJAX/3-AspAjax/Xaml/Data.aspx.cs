using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;

public partial class Xaml_Data : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
     
        string query = "SELECT * FROM Resource";

        clsDataAccess mydataAccess = new clsDataAccess();
        mydataAccess.openConnection();
        SqlDataReader mydr = mydataAccess.getData(query);
        float countBooks = 0;
        float countBlogs = 0;
        float countTutorials = 0;
        float countVideos = 0; 
        if (mydr.HasRows)
        {
            while (mydr.Read())
            {
                if (Convert.ToInt32(mydr.GetValue(5)) == 1) countBooks++;
                if (Convert.ToInt32(mydr.GetValue(5)) == 2) countBlogs++;
                if (Convert.ToInt32(mydr.GetValue(5)) == 3) countTutorials++;
                if (Convert.ToInt32(mydr.GetValue(5)) == 4) countVideos++;
            }
        }

        mydr.Close();
        mydataAccess.closeConnection();
        float Max = countBooks;
        float Max2 = countVideos;
        
        if (countBlogs > countBooks) Max = countBlogs;
        if (countTutorials > countVideos) Max2 = countTutorials;
        if (Max2 > Max ) Max = Max2;

        countBooks = (countBooks/Max) * 200;
        countBlogs = (countBlogs/Max) * 200;
        countTutorials = (countTutorials/Max) * 200;
        countVideos = (countVideos/Max) * 200;
 
        StringBuilder stringGraph = new StringBuilder("<?xml version='1.0' encoding='utf-8' ?>");
        stringGraph.Append("<Canvas Width='400' Height='300'  xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' Background='WhiteSmoke'>");
        stringGraph.Append("<TextBlock Text='Silverlight Resources' Canvas.Top='30' Canvas.Left='135'></TextBlock>");
        stringGraph.Append("<Rectangle Canvas.Top='" + (200 - countBooks + 70) + "' Canvas.Left='85' Width='50' Height='" + countBooks + "'  Stroke='Gainsboro' Fill='#FF4682B4' StrokeThickness='1'></Rectangle>");
        stringGraph.Append("<TextBlock Text='Books' FontSize='11' Canvas.Top='270' Canvas.Left='90'></TextBlock>");
        stringGraph.Append("<Rectangle Canvas.Top='" + (200 - countBlogs + 70) + "' Canvas.Left='145' Width='50' Height='" + countBlogs + "'  Stroke='Gainsboro' Fill='#FF4682B4'  StrokeThickness='1'></Rectangle>");
        stringGraph.Append("<TextBlock Text='Blogs' FontSize='11' Canvas.Top='270' Canvas.Left='152'></TextBlock>");
        stringGraph.Append("<Rectangle Canvas.Top='" + (200 - countTutorials + 70) + "' Canvas.Left='205' Width='50' Height='" + countTutorials + "'  Stroke='Gainsboro' Fill='#FFFF8C00'  StrokeThickness='1'></Rectangle>");
        stringGraph.Append("<TextBlock Text='Tutorial' FontSize='11' Canvas.Top='270' Canvas.Left='207'></TextBlock>");
        stringGraph.Append("<Rectangle Canvas.Top='" + (200 - countVideos + 70) + "' Canvas.Left='265' Width='50' Height='" + countVideos + "'  Stroke='Gainsboro' Fill='#FF4682B4'  StrokeThickness='1'></Rectangle>");
        stringGraph.Append("<TextBlock Text='Video' FontSize='11'  Canvas.Top='270' Canvas.Left='275'></TextBlock>");
        stringGraph.Append("</Canvas>");

        Response.Write(stringGraph);
        
    }
}
