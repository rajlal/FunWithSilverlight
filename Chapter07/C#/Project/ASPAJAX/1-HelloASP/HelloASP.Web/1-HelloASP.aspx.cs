using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _2_HelloASP.Web
{
    public partial class __HelloASP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SilverlightFromASPCode.Text = ResolveClientUrl("~/ClientBin/HelloASP_Server.xap");

        }
    }
}
