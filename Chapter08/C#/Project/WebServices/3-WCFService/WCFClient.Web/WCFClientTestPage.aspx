<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>WCFClient</title>
   <link rel="stylesheet" type="text/css" href="Style.css" />
</head>

<body>

    <!-- Runtime errors from Silverlight will be displayed here.
	This will contain debugging information and should be removed or hidden when debugging is completed -->
	  <div class="Content">
	  <div class="ApplicationContainer" id="silverlightControlHost">
		<object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
		  <param name="source" value="ClientBin/WCFClient.xap"/>
		  <param name="onError" value="onSilverlightError" />
		  <param name="background" value="white" />
		  <param name="minRuntimeVersion" value="3.0.40624.0" />
		  <param name="autoUpgrade" value="true" />
		  <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0" style="text-decoration:none">
 			  <img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight" style="border-style:none"/>
		  </a>
	    </object>
		<iframe style='visibility:hidden;height:0;width:0;border:0px'></iframe>
    </div>
    <div class="ContentHelp">XML using XLinq<UL>
    <LI>XElement for individual XML Node
    <LI>XDocument to create a complete XML File
    <LI>Other options for other XML tags;
    </UL>
    </div>
    </div>
    
</body>
</html>
