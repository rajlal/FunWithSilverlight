

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
'Imports System.Web.Ria
'Imports System.Web.Ria.Data
'Imports System.Web.DomainServices
Imports System.Data
'Imports System.Web.DomainServices.LinqToEntities

'Namespace NetRIAService.Web
'  Implements application logic using the SilverlightResourceEntities context.
'  TODO: Add your application logic to these methods or in additional methods.
<EnableClientAccess()>
Public Class ResourceService
    Inherits LinqToEntitiesDomainService(Of SilverlightResourceEntities)

    '  TODO: Consider
    '  1. Adding parameters to this method and constraining returned results, and/or
    '  2. Adding query methods taking different parameters.
    Public Function GetResource() As IQueryable(Of Resource)
        Return Me.Context.Resource
    End Function
End Class   '   ResourceService
'End Namespace
' ..\Project_08\WebServices\4-NetRIAService\NetRIAService.Web\ResourceService.cs
