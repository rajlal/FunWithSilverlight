using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _1_HelloASP.Web
{
    public partial class __MediaPlayerControl : System.Web.UI.Page
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



        protected void HighQuality_Click(object sender, EventArgs e)
        {

            if (MediaPlayer1.MediaSource == "~/Videos/SilverLight_Intro.wmv")
            {
                MediaPlayer1.MediaSource = "~/Videos/SilverLight_IntroHQ.mp4";
                HighQuality.Text = "Regular Quality";
            }
            else
            {
                MediaPlayer1.MediaSource = "~/Videos/SilverLight_Intro.wmv";
                HighQuality.Text = "High Quality";
            }
        }


        protected void DisableGPU_Click(object sender, EventArgs e)
        {
            if (MediaPlayer1.EnableGPUAcceleration)
            {
                MediaPlayer1.EnableGPUAcceleration = false;
                DisableGPU.Text = "Enable GPU Acc.";
            }
            else
            {
                MediaPlayer1.EnableGPUAcceleration = true;
                DisableGPU.Text = "Disable GPU Acc.";
            }
        }

        protected void CacheVisual_Click(object sender, EventArgs e)
        {
            if (!MediaPlayer1.EnableCacheVisualization)
            {
                MediaPlayer1.EnableCacheVisualization = true;
                CacheVisual.Text = "Disable Cache Vis.";
            }
            else
            {
                MediaPlayer1.EnableCacheVisualization = false;
                CacheVisual.Text = "Enable Cache Vis.";
            }
        }
    }
}
