<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="2-GetFlickr.aspx.vb" Inherits="RESTClient.Web.Get_Flickr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>GET Using Flickr REST API</title>
    <link rel="stylesheet" type="text/css" href="Style.css" />
    <script type="text/javascript" src="Silverlight.js"></script>
    <script type="text/javascript">
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
              appSource = sender.getHost().Source;
            }
            
            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
              return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " +  appSource + "\n" ;

            errMsg += "Code: "+ iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {           
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " +  args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }
    </script>
</head>
<body>

    <!-- Runtime errors from Silverlight will be displayed here.
	This will contain debugging information and should be removed or hidden when debugging is completed -->
	  <div class="Content">
	  <div class="ApplicationContainer" id="silverlightControlHost">
		<object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
		  <param name="source" value="ClientBin/GetFlickr.xap"/>
		  <param name="onError" value="onSilverlightError" />
		  <param name="background" value="white" />
		  <param name="minRuntimeVersion" value="3.0.40624.0" />
		  <param name="autoUpgrade" value="true" />
		  <a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0" style="text-decoration:none">
 			  <img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight" style="border-style:none"/>
		  </a>
	    </object>
		<iframe style='visibility:hidden;height:0;width:0;border:0px'></iframe></div>
       <div id= "ContentText" class="ContentHelp">
    <div style="background:#EEEFFF;cursor:hand;height:22px;" >
    <a href="1-Index.aspx" style="vertical-align:middle;" ><img src="home.png" border="0" width="16" height="16"/></a>&nbsp;&nbsp;
    <a href="2-GetFlickr.aspx">GET Flickr</a>&nbsp;&nbsp;
    <a href="3-PostTwitter.aspx">POST Twitter</a></div><br />
   Silverlight Call to REST APIs
<UL>
    <LI>HTTPWebRequest and HTTPWebResponse
    <LI>Use Silverlight as a Server Control
    <LI>Use Media Control
    </UL>
    </div>
    </div>
</body>