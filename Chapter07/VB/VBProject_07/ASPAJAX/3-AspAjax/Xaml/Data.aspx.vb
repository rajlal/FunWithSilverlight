Imports System
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Text
Imports System.Data.SqlClient

Public Partial Class Xaml_Data
        Inherits System.Web.UI.Page
    protected Sub Page_Load(sender As Object, e As EventArgs)

        Dim query As String  = "SELECT * FROM Resource"

        Dim mydataAccess As clsDataAccess  = New clsDataAccess()

        mydataAccess.openConnection()

        Dim mydr As SqlDataReader  = mydataAccess.getData(query)
        Dim countBooks As Single  = 0
        Dim countBlogs As Single  = 0
        Dim countTutorials As Single  = 0
        Dim countVideos As Single  = 0


        If (mydr.HasRows) Then

            While (mydr.Read())

                If (Convert.ToInt32(mydr.GetValue(5)) = 1) Then
                    countBooks += 1
                End If

                If (Convert.ToInt32(mydr.GetValue(5)) = 2) Then
                    countBlogs += 1
                End If

                If (Convert.ToInt32(mydr.GetValue(5)) = 3) Then
                    countTutorials += 1
                End If

                If (Convert.ToInt32(mydr.GetValue(5)) = 4) Then
                    countVideos += 1
                End If
            End While   '
        End If


        mydr.Close()
        mydataAccess.closeConnection()

        Dim Max As Single  = countBooks
        Dim Max2 As Single  = countVideos

        If (countBlogs > countBooks) Then
            Max = countBlogs
        End If

        If (countTutorials > countVideos) Then
            Max2 = countTutorials
        End If

        If (Max2 > Max ) Then
            Max = Max2
        End If


        countBooks = (countBooks/Max) * 200
        countBlogs = (countBlogs/Max) * 200
        countTutorials = (countTutorials/Max) * 200
        countVideos = (countVideos/Max) * 200

        Dim stringGraph As StringBuilder  = New StringBuilder("<?xml version='1.0' encoding='utf-8' ?>")

        stringGraph.Append("<Canvas Width='400' Height='300'  xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' Background='WhiteSmoke'>")
        stringGraph.Append("<TextBlock Text='Silverlight Resources' Canvas.Top='30' Canvas.Left='135'></TextBlock>")
        stringGraph.Append("<Rectangle Canvas.Top='" + (200 - countBooks + 70) + "' Canvas.Left='85' Width='50' Height='" + countBooks + "'  Stroke='Gainsboro' Fill='#FF4682B4' StrokeThickness='1'></Rectangle>")
        stringGraph.Append("<TextBlock Text='Books' FontSize='11' Canvas.Top='270' Canvas.Left='90'></TextBlock>")
        stringGraph.Append("<Rectangle Canvas.Top='" + (200 - countBlogs + 70) + "' Canvas.Left='145' Width='50' Height='" + countBlogs + "'  Stroke='Gainsboro' Fill='#FF4682B4'  StrokeThickness='1'></Rectangle>")
        stringGraph.Append("<TextBlock Text='Blogs' FontSize='11' Canvas.Top='270' Canvas.Left='152'></TextBlock>")
        stringGraph.Append("<Rectangle Canvas.Top='" + (200 - countTutorials + 70) + "' Canvas.Left='205' Width='50' Height='" + countTutorials + "'  Stroke='Gainsboro' Fill='#FFFF8C00'  StrokeThickness='1'></Rectangle>")
        stringGraph.Append("<TextBlock Text='Tutorial' FontSize='11' Canvas.Top='270' Canvas.Left='207'></TextBlock>")
        stringGraph.Append("<Rectangle Canvas.Top='" + (200 - countVideos + 70) + "' Canvas.Left='265' Width='50' Height='" + countVideos + "'  Stroke='Gainsboro' Fill='#FF4682B4'  StrokeThickness='1'></Rectangle>")
        stringGraph.Append("<TextBlock Text='Video' FontSize='11'  Canvas.Top='270' Canvas.Left='275'></TextBlock>")
        stringGraph.Append("</Canvas>")

        Response.Write(stringGraph)
    End Sub '   Page_Load
End Class   '   Xaml_Data
' ..\Project_07\ASPAJAX\3-AspAjax\Xaml\Data.aspx.cs
