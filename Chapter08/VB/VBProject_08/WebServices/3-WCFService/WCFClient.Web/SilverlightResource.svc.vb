Imports System
Imports System.Linq
Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.Collections.Generic
Imports System.Text
Imports System.Data.SqlClient

' Namespace WCFClient.Web

    <ServiceContract(Namespace := "")>
    <AspNetCompatibilityRequirements(RequirementsMode := AspNetCompatibilityRequirementsMode.Allowed)>
    Public Class SilverlightResource

    Private oResource As ResourceInfo

        Public Sub New()

        oResource.Author = ""
        oResource.Title = ""
        oResource.Image = ""
        oResource.URL = ""
        oResource.Type = ""
        End Sub '   New

    <OperationContract()>
    Public Function GetResource(id As Integer) As ResourceInfo

        GetResourceData(id)

        Dim localResource As ResourceInfo = New ResourceInfo()

        localResource.Author = oResource.Author
        localResource.Title = oResource.Title
        localResource.Image = oResource.Image
        localResource.URL = oResource.URL
        localResource.Type = oResource.Type
        Return localResource
    End Function  '   GetResource

    Private Sub GetResourceData(ResourceId As Integer)

        '  This is where you use your business components.
        '  Method calls on Business components are used to populate the data.

        oResource.Title = "Silverlight 3 How To"
        oResource.Author = "Rajesh Lal"
        oResource.Image = "http://silverlightfun.com/Samples/Chapter-07/AdoData/images/S3HT.jpg"
        oResource.URL = "http://www.amazon.com/Silverlight-3-How-Rajesh-Lal/dp/0672330628"

        Try

            Dim query As String = "SELECT * FROM Resource where ID=" + CStr(ResourceId)

            Dim mydataAccess As clsDataAccess = New clsDataAccess()

            mydataAccess.openConnection()

            Dim mydr As SqlDataReader = mydataAccess.getData(query)

            If (mydr.HasRows) Then

                While (mydr.Read())

                    oResource.Title = mydr.GetValue(1).ToString()
                    oResource.Author = mydr.GetValue(2).ToString()
                    oResource.Image = mydr.GetValue(3).ToString()
                    oResource.URL = mydr.GetValue(4).ToString()

                    Select Case (Convert.ToInt32(mydr.GetValue(5)))
                        Case 1
                            oResource.Type = "Book"
                        Case 2
                            oResource.Type = "Blog"
                        Case 3
                            oResource.Type = "Tutorial"
                        Case 4
                            oResource.Type = "Video"
                    End Select
                End While   '
            Else
                oResource.Title = "Error: No data for that ID"
                oResource.Author = "Try other ID's between 1 and 16"
                oResource.Image = "http://silverlightfun.com/Samples/Chapter-07/AdoData/images/error.jpg"
                oResource.URL = "http://www.amazon.com/Silverlight-3-How-Rajesh-Lal/dp/0672330628"
            End If

            mydr.Close()
            mydataAccess.closeConnection()
        Catch e As Exception
            oResource.Title = "Error: " + e.Message
            oResource.Author = "Occured while connecting to the Database "
            oResource.Image = "http://silverlightfun.com/Samples/Chapter-07/AdoData/images/error.jpg"
            oResource.URL = "http://www.amazon.com/Silverlight-3-How-Rajesh-Lal/dp/0672330628"
        End Try
    End Sub '   GetResourceData

    Public Structure ResourceInfo

        Public Property Author As String
        Public Property Title As String
        Public Property Image As String
        Public Property URL As String
        Public Property Type As String
    End Structure
    '  Add more operations here and mark them with (OperationContract)
End Class   '   SilverlightResource
' End Namespace
' ..\Project_08\WebServices\3-WCFService\WCFClient.Web\SilverlightResource.svc.cs
