Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

Public Partial Class _Default
        Inherits System.Web.UI.Page
    protected Sub Page_Load(sender As Object, e As EventArgs)
    End Sub '   Page_Load

    protected Sub ButtonServer_Click(sender As Object, e As EventArgs)


        If (Silverlight1.Source = "Xaml/SmileyP.xaml") Then
            Silverlight1.Source = "Xaml/Smiley.xaml"
        else
            Silverlight1.Source = "Xaml/SmileyP.xaml"
        End If
    End Sub '   ButtonServer_Click
    protected Sub ButtonDynamic_Click(sender As Object, e As EventArgs)


        If (Silverlight1.Source = "Xaml/SmileyDynamic.ashx") Then
            Silverlight1.Source = "Xaml/SmileyP.xaml"
        else
            Silverlight1.Source = "Xaml/SmileyDynamic.ashx"
        End If
    End Sub '   ButtonDynamic_Click

    protected Sub ButtonData_Click(sender As Object, e As EventArgs)


        If (Silverlight1.Source = "Xaml/Data.aspx") Then
            Silverlight1.Source = "Xaml/SmileyP.xaml"
        else
            Silverlight1.Source = "Xaml/Data.aspx"
        End If
    End Sub '   ButtonData_Click
End Class   '   _Default
' ..\Project_07\ASPAJAX\3-AspAjax\index.aspx.cs
