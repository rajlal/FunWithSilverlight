<%@ Page Language="vb" AutoEventWireup="true" CodeFile="3-MediaPlayerControl.aspx.vb" Inherits="_1_HelloASP.Web.__MediaPlayerControl" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ASP.NET MediaPlayer Control</title>
<link rel="stylesheet" type="text/css" href="Style.css" />
</head>
<body>
     <form id="form1" runat="server" style="height:100%">
         <div class="Content">
    <div  class="ApplicationContainer"  id="silverlightControlHost">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:MediaPlayer id="MediaPlayer1" runat="server" MediaSource="~/Videos/SilverLight_IntroHQ.mp4" MediaSkinSource="~/MediaSkins/Basic.xaml" Width="400px" Height="300px" AutoPlay="true"   EnableGPUAcceleration="true"  EnableCacheVisualization="false" ScaleMode=Stretch />
      </div>
      <div>
         <br />&nbsp;<b>Skin: </b>&nbsp;
        <asp:LinkButton ID="Basic" runat="server" onclick="Skin_Click">Basic</asp:LinkButton>&nbsp;
        <asp:LinkButton ID="Classic" runat="server" onclick="Skin_Click">Classic</asp:LinkButton>&nbsp;
        <asp:LinkButton ID="Console" runat="server" onclick="Skin_Click">Console</asp:LinkButton>&nbsp;
        <asp:LinkButton ID="Expression" runat="server" onclick="Skin_Click">Expression</asp:LinkButton>&nbsp;
        <asp:LinkButton ID="Futuristic" runat="server" onclick="Skin_Click">Futuristic</asp:LinkButton>&nbsp;
        <asp:LinkButton ID="Professional" runat="server" onclick="Skin_Click">Prof.</asp:LinkButton>&nbsp;
        <asp:LinkButton ID="Simple" runat="server" onclick="Skin_Click">Simple</asp:LinkButton><br />&nbsp;<b>Settings: </b>
          &nbsp;&nbsp;<asp:LinkButton ID="DisableGPU" runat="server" ToolTip="GPU Acceleration" Text="Disable GPU Acc." 
              onclick="DisableGPU_Click"/>
          &nbsp;&nbsp;<asp:LinkButton ID="CacheVisual" runat="server"  ToolTip="Cache Visualization" Text="Enable Cache Vis." 
              onclick="CacheVisual_Click" 
              oncheckedchanged="CacheVisual_Click"/>
          &nbsp;&nbsp;<asp:LinkButton ID="HighQuality" runat="server"  ToolTip="Quality" Text="Regular Quality" 
               Checked=true onclick="HighQuality_Click"/><br />
          <br /></div>
     <div id= "ContentText" class="ContentHelp">
    <div style="background:#EEEFFF;" style="cursor:hand;">
    <a href="index.aspx">Standard Object</a>&nbsp;&nbsp;
    <a href="1-HelloASP.aspx">Server Code</a>&nbsp;&nbsp;
    <a href="2-ASPNETSilverlight.aspx">Silverlight Control</a>&nbsp;&nbsp;
    <a href="3-MediaPlayerControl.aspx">Media Control</a></div><br />
  Hello ASP.NET
<UL>
    <LI>Set Silverlight Source from Server 
    <LI>Use Silverlight as a Server Control
    <LI>Use Media Control
    </UL>
    </div>
    </div>
    </form>
</body>
</html>
