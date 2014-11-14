Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Services
Imports System.Data.SqlClient

'Namespace ASMXService

''' <summary>
''' Summary description for Resource Service
''' </summary>
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<System.ComponentModel.ToolboxItem(False)>
Public Class SilverlightResource
    Inherits WebService

    Private oResource As ResourceInfo

    Public Sub New()

        oResource.Author = ""
        oResource.Title = ""
        oResource.sImage = ""
        oResource.sURL = ""
        oResource.sType = ""
    End Sub '   New

    Private Sub GetResourceData(ResourceId As Integer)

        '  This is where you use your business components.
        '  Method calls on Business components are used to populate the data.

        oResource.Title = "Silverlight 3 How To"
        oResource.Author = "Rajesh Lal"
        oResource.sImage = "http://silverlightfun.com/Samples/Chapter-07/AdoData/images/S3HT.jpg"
        oResource.sURL = "http://www.amazon.com/Silverlight-3-How-Rajesh-Lal/dp/0672330628"

        Try

            Dim query As String = "SELECT * FROM Resource where ID=" + CStr(ResourceId)
            Dim mydataAccess As clsDataAccess = New clsDataAccess()

            mydataAccess.openConnection()

            Dim mydr As SqlDataReader = mydataAccess.getData(query)

            If (mydr.HasRows) Then
                While (mydr.Read())
                    oResource.Title = mydr.GetValue(1).ToString()
                    oResource.Author = mydr.GetValue(2).ToString()
                    oResource.sImage = mydr.GetValue(3).ToString()
                    oResource.sURL = mydr.GetValue(4).ToString()

                    Select Case (Convert.ToInt32(mydr.GetValue(5)))
                        Case 1
                            oResource.sType = "Book"
                        Case 2
                            oResource.sType = "Blog"
                        Case 3
                            oResource.sType = "Tutorial"
                        Case 4
                            oResource.sType = "Video"
                    End Select
                End While   '
            Else
                oResource.Title = "Error: No data for that ID"
                oResource.Author = "Try other ID's between 1 and 16"
                oResource.sImage = "http://silverlightfun.com/Samples/Chapter-07/AdoData/images/error.jpg"
                oResource.sURL = "http://www.amazon.com/Silverlight-3-How-Rajesh-Lal/dp/0672330628"
            End If

            mydr.Close()
            mydataAccess.closeConnection()
        Catch e As Exception
            oResource.Title = "Error: " + e.Message
            oResource.Author = "Occured while connecting to the Database "
            oResource.sImage = "http://silverlightfun.com/Samples/Chapter-07/AdoData/images/error.jpg"
            oResource.sURL = "http://www.amazon.com/Silverlight-3-How-Rajesh-Lal/dp/0672330628"
        End Try
    End Sub '   GetResourceData

    <WebMethod(Description:="This method call will get the Resource information from the database for a given Resource ID.", EnableSession:=False)>
    Public Function GetResource(id As Integer) As ResourceInfo

        GetResourceData(id)

        Dim localResource As ResourceInfo = New ResourceInfo()

        localResource.Author = oResource.Author
        localResource.Title = oResource.Title
        localResource.sImage = oResource.sImage
        localResource.sURL = oResource.sURL
        localResource.sType = oResource.sType
        Return localResource
    End Function  '   GetResource
End Class   '   SilverlightResource

Public Structure ResourceInfo

    Public Title As String
    Public Author As String
    Public sImage As String
    Public sURL As String
    Public sType As String
End Structure
'End Namespace
' ..\Project_08\WebServices\2-ASPNETWebService\ASMXWebService\SilverlightResource.asmx.cs
