<%@ Page Language="vb" 
         AutoEventWireup="true" 
         CodeBehind="1-2-MediaPlayerControl.aspx.vb" 
         Inherits="AdvMediaPlayer.Web._2_MediaPlayerControl" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>ASP.NET MediaPlayer Control</title>
<link rel="stylesheet" type="text/css" href="Style.css" />
</head>
<body>
  <form id="form1" runat="server" style="height:100%">< 
    <div class="Content">
      <div  class="ApplicationContainer"  id="silverlightControlHost">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:MediaPlayer id="MediaPlayer1" runat="server" MediaSource="Butterfly.wmv" MediaSkinSource="~/MediaSkins/Basic.xaml" Width="400px" Height="300px" AutoPlay="true"   EnableGPUAcceleration="true"  EnableCacheVisualization="false" ScaleMode="Stretch" />
      </div>
      <div>
        <table runat="server" id="SkinTable" width="100%">
          <tr>
            <td>&nbsp;&nbsp;<asp:LinkButton ID="Basic" runat="server" onclick="Skin_Click" Font-Bold="true" Font-Italic="true">Basic</asp:LinkButton>&nbsp;</td>
            <td><asp:LinkButton ID="Classic" runat="server" onclick="Skin_Click">Classic</asp:LinkButton>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;&nbsp;<asp:LinkButton ID="Console" runat="server" onclick="Skin_Click">Console</asp:LinkButton>&nbsp;</td>
            <td><asp:LinkButton ID="Expression" runat="server" onclick="Skin_Click" >Expression</asp:LinkButton>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;&nbsp;<asp:LinkButton ID="Futuristic" runat="server" onclick="Skin_Click">Futuristic</asp:LinkButton>&nbsp;</td>
            <td><asp:LinkButton ID="Professional" runat="server" onclick="Skin_Click">Professional</asp:LinkButton>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;&nbsp;<asp:LinkButton ID="Simple" runat="server" onclick="Skin_Click">Simple</asp:LinkButton><br />&nbsp;</td>
            <td><asp:LinkButton ID="AudioGray" runat="server" onclick="Skin_Click">AudioGray</asp:LinkButton><br />&nbsp;</td>
          </tr>
        </table>   
      </div>&nbsp;&nbsp;Advanced Media Player
      <ul>
        <li><a href="1-1-PlayerProgress.html">Media Player with Progressbar</a>     </li>
        <li><a href="1-2-MediaPlayerControl.aspx"><b><i>MediaPlayer Control with Skins</i></b></a></li>
        <li><a href="1-3-EncoderMediaPlayer.html">Custom Encoder Media Player</a>   </li>
      </ul> 
    </div>
  </form>
</body>
</html>

