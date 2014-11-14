<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Register src="FunBadge.ascx" tagname="SilverlightControl" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>FunBadge</title>
  <link rel="stylesheet" type="text/css" href="Style.css" />
</head>

<body>
 <form id="form1" runat="server">
    <!-- Runtime errors from Silverlight will be displayed here.
	This will contain debugging information and should be removed or hidden when debugging is completed -->
	  <div class="Content">
	     <asp:ScriptManager ID="ScriptManager1" runat="server">
          </asp:ScriptManager><div class="Content"><center>
	  <div class="ApplicationContainer" id="silverlightControlHost"><br /><br/><br />
	      <uc1:SilverlightControl ID="SilverlightControl1" runat="server" BackgroundColor="AliceBlue"/>
    </div>
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
