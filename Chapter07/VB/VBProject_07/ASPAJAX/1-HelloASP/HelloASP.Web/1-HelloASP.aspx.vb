Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace _2_HelloASP.Web

    Partial Public Class __HelloASP
        Inherits System.Web.UI.Page

        Private Sub Page_Load(sender As Object, e As EventArgs)

            SilverlightFromASPCode.Text = ResolveClientUrl("../ClientBin/HelloASP_Server.xap")
        End Sub '   Page_Load
    End Class   '   __HelloASP
End Namespace   '   _2_HelloASP.Web
' ..\Project_07\ASPAJAX\1-HelloASP\HelloASP.Web\1-HelloASP.aspx.cs
