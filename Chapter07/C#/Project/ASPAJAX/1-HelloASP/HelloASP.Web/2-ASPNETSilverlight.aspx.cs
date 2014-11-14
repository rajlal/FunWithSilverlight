using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _1_HelloASP.Web
{
    public partial class __ASPNETSilverlight : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Silverlight1.Source == "Xaml/SmileyP.xaml")
            {
                Silverlight1.Source = "Xaml/Smiley.xaml";
                Silverlight2.Source = "Xaml/SmileyP.xaml";
            }
            else
            {
                Silverlight1.Source = "Xaml/SmileyP.xaml";
                Silverlight2.Source = "Xaml/Smiley.xaml";
            }
        }
    }
}
