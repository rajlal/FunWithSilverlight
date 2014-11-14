Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Drawing

' Namespace FunBadge.Web

    Public Partial Class SilverlightControl
        Inherits System.Web.UI.UserControl
        Private _startValue As Integer = 0
        Private _backgroundColor As String = "White"

        Public Property StartValue() As Integer
            Get
                return _startValue
            End Get

            Set
                _startValue = value
            End Set
        End Property
        Public Property BackgroundColor() As String
            Get
                return _backgroundColor
            End Get

            Set
                _backgroundColor = value
            End Set
        End Property
        protected Sub Page_Load(sender As Object, e As EventArgs)

            Silverlight1.Source = "ClientBin/FunBadge.xap"
            Silverlight1.InitParameters = "StartValue=" +_startValue.ToString() + ",BackgroundColor=" +_backgroundColor
        End Sub '   Page_Load
    End Class   '   SilverlightControl
' End Namespace   '   FunBadge.Web
' ..\Project_07\ASPAJAX\2-ASPUserControl\FunBadge.Web\FunBadge.ascx.cs
