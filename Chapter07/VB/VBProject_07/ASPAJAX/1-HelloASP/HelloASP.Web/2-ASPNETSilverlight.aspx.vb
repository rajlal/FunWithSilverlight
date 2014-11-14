Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace _1_HelloASP.Web

    Partial Public Class __ASPNETSilverlight
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(sender As Object, e As EventArgs)
        End Sub '   Page_Load

        Protected Sub Button1_Click(sender As Object, e As EventArgs)

            If (Silverlight1.Source = "Xaml/SmileyP.xaml") Then
                Silverlight1.Source = "Xaml/Smiley.xaml"
                Silverlight2.Source = "Xaml/SmileyP.xaml"
            Else
                Silverlight1.Source = "Xaml/SmileyP.xaml"
                Silverlight2.Source = "Xaml/Smiley.xaml"
            End If
        End Sub '   Button1_Click
    End Class   '   __ASPNETSilverlight
End Namespace   '   _1_HelloASP.Web
' ..\Project_07\ASPAJAX\1-HelloASP\HelloASP.Web\2-ASPNETSilverlight.aspx.cs
