<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestMultipleBadge.aspx.cs" Inherits="FunBadge.Web._Default" %>

<%@ Register src="FunBadge.ascx" tagname="SilverlightControl" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Fun Badge Server Control</title>
  <link rel="stylesheet" type="text/css" href="Style.css" />
</head>

<body>

    <form id="form1" runat="server">

    <!-- Runtime errors from Silverlight will be displayed here.
	This will contain debugging information and should be removed or hidden when debugging is completed -->
	   <asp:ScriptManager ID="ScriptManager1" runat="server">
          </asp:ScriptManager><div class="Content"><center>
	  <div class="ApplicationContainer" id="silverlightControlHost">
	      <uc1:SilverlightControl ID="SilverlightControl3" runat="server" />
	      <uc1:SilverlightControl ID="SilverlightControl1" runat="server" StartValue="1" BackgroundColor="AliceBlue"/>
          <uc1:SilverlightControl ID="SilverlightControl2" runat="server" StartValue="2"/>
          <uc1:SilverlightControl ID="SilverlightControl4" runat="server" StartValue="3" BackgroundColor="AliceBlue" />
          <uc1:SilverlightControl ID="SilverlightControl5" runat="server" StartValue="4"/>
          <uc1:SilverlightControl ID="SilverlightControl6" runat="server" StartValue="5"  BackgroundColor="#FFF0F8FF"/>
          <uc1:SilverlightControl ID="SilverlightControl7" runat="server" StartValue="6"/>
          <uc1:SilverlightControl ID="SilverlightControl8" runat="server" StartValue="7" BackgroundColor="#FFF0F8FF"/>
          <uc1:SilverlightControl ID="SilverlightControl9" runat="server" StartValue="9"/>    
	</div></center>
   <div class="ContentHelp">ASP.NET User Control (ASCX) <UL>
    <LI>Silverlight control inside a User Control
    <LI>Property value travel from User Control to Silverlight 
    <LI>From Silverlight control to Actual XAML in XAP
    </UL>
    </div>
    </div>
    
    </form>
    
</body>
</html>
