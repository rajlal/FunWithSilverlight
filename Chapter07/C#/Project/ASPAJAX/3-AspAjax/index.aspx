<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Silverlight with ASP AJAX</title>
<link rel="stylesheet" type="text/css" href="Style.css" />
</head>

<body>
    <!-- Runtime errors from Silverlight will be displayed here.
	This will contain debugging information and should be removed or hidden when debugging is completed -->
	  <div class="Content">
	  <div class="ApplicationContainer" id="silverlightControlHost">
        <form id="form1" runat="server"><center>
 		<asp:ScriptManager ID="ScriptManager1" runat="server" />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate><asp:Silverlight id="Silverlight1" runat="server" Source="Xaml/Smiley.xaml" Width="400px" Height="270px" MinimumVersion="3.0.40624.0" ScaleMode="Stretch"/>
                </ContentTemplate>        
            <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonServer" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ButtonDynamic" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="ButtonData" EventName="Click" />
            </Triggers>
            </asp:UpdatePanel>
    </center><div>
 <asp:Button ID="ButtonServer" runat="server" Text="Server Xaml" Width="110px" OnClick="ButtonServer_Click" />
 &nbsp;&nbsp;<asp:Button ID="ButtonDynamic" runat="server" Text="Dynamic Xaml" Width="110px" OnClick="ButtonDynamic_Click" />
&nbsp;&nbsp;<asp:Button ID="ButtonData" runat="server" Text="Dynamic Data from DB" Width="160px" OnClick="ButtonData_Click" /></div>
    </form>
    </div>
    <div class="ContentHelp"> Silverlight with ASP.NET AJAX<UL>
    <LI>XElement for individual XML Node
    <LI>XDocument to create a complete XML File
    <LI>Other options for other XML tags;
    </UL>
    </div>
    </div>
</body>
</html>
