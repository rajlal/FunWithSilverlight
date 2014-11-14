using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
   }
    protected void ButtonServer_Click(object sender, EventArgs e)
    {
        if (Silverlight1.Source == "Xaml/SmileyP.xaml")
        {
            Silverlight1.Source = "Xaml/Smiley.xaml";
        }
        else
        {
            Silverlight1.Source = "Xaml/SmileyP.xaml";
        }
    }
    protected void ButtonDynamic_Click(object sender, EventArgs e)
    {
        if (Silverlight1.Source == "Xaml/SmileyDynamic.ashx")
        {
            Silverlight1.Source = "Xaml/SmileyP.xaml";
        }
        else
        {
            Silverlight1.Source = "Xaml/SmileyDynamic.ashx";
        }
    }
    protected void ButtonData_Click(object sender, EventArgs e)
    {
        if (Silverlight1.Source == "Xaml/Data.aspx")
        {
            Silverlight1.Source = "Xaml/SmileyP.xaml";
        }
        else
        {
            Silverlight1.Source = "Xaml/Data.aspx";
        }
    }
}