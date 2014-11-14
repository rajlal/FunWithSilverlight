using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace FunBadge.Web
{
    public partial class SilverlightControl : System.Web.UI.UserControl
    {
        private int _startValue =0;
        private string _backgroundColor = "White";
       
        public int StartValue
        {
            get { return _startValue; }
            set { _startValue = value; }
        }
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Silverlight1.Source = "ClientBin/FunBadge.xap";
            Silverlight1.InitParameters = "StartValue=" +_startValue.ToString() + ",BackgroundColor=" +_backgroundColor;
        }
    }
}