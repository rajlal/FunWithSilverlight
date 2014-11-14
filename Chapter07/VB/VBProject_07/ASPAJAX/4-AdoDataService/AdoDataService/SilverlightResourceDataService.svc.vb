Imports System
Imports System.Collections.Generic
Imports System.Data.Services
Imports System.Linq
Imports System.ServiceModel.Web
Imports System.Web

' Namespace AdoDataService

    Public Class SilverlightResourceDataService
        Inherits DataService(Of SilverlightResourceEntities)
        '  This method is called only once to initialize service-wide policies.
    Public Shared Sub InitializeService(config As IDataServiceConfiguration)

        '  TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
        '  Examples:
        config.SetEntitySetAccessRule("Resource", EntitySetRights.All)
        config.SetEntitySetAccessRule("ResourceType", EntitySetRights.All)
        '  config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All)
    End Sub '   InitializeService
End Class   '   SilverlightResourceDataService
' End Namespace   '   AdoDataService
' ..\Project_07\ASPAJAX\4-AdoDataService\AdoDataService\SilverlightResourceDataService.svc.cs
