﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="2-ASPNETSilverlight.aspx.cs" Inherits="_1_HelloASP.Web.__ASPNETSilverlight" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="Style.css" />

</head>
<body>
    <form id="form1" runat="server" style="height:100%">
         <div class="Content">
    <div  class="ApplicationContainer"  id="silverlightControlHost">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <br /><br /><br />
        <center><asp:Silverlight id="Silverlight1" runat="server" Source="Xaml/Smiley.xaml" Width="150px" Height="150px" MinimumVersion="3.0.40624.0" ScaleMode="Stretch"/>
        &nbsp;&nbsp;<asp:Silverlight id="Silverlight2" runat="server" Source="Xaml/Smileyp.xaml" Width="150px" Height="150px" MinimumVersion="3.0.40624.0" ScaleMode="Stretch" />
        <br /><br /><br /><asp:Button ID="Button1" runat="server" Text="Switch Silverlight Controls" onclick="Button1_Click" /></center>
         </div>
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
