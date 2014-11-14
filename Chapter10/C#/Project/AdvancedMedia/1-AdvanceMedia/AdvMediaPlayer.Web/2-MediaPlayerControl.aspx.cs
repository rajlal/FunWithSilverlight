using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdvMediaPlayer.Web
{
    public partial class _2_MediaPlayerControl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Skin_Click(object sender, EventArgs e)
        {
            LinkButton l = (LinkButton)sender;
            string selectedSkin = l.Text;
            if (l.Text == "Prof.") selectedSkin = "Professional";
            MediaPlayer1.MediaSkinSource = "~/MediaSkins/" + selectedSkin + ".xaml";
        }
    }
}