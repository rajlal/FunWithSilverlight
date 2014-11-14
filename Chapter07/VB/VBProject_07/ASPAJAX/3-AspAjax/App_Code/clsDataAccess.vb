Imports System
Imports System.Collections.Generic
Imports System.Web
Imports System.Data.SqlClient
Imports System.Data

Imports System.Configuration

''' <summary>
''' Summary description for clsDataAccess
''' </summary>
Public Class clsDataAccess  '  Class defination
    Public Sub New()
        '
        '  TODO: Add constructor logic here
        '
    End Sub '   New


    Dim mycon As SqlConnection = New SqlConnection(ConfigurationSettings.AppSettings("ConnectionString"))

    '  Opens database connection with Granth in SQL SERVER
    Public Function openConnection() As Boolean

        mycon.Open()
        Return True
    End Function  '   openConnection
    '  Closes database connection with Granth in SQL SERVER
    Public Sub closeConnection()

        mycon.Close()
        mycon = Nothing
    End Sub '   closeConnection
    '  Getdata from the table required(given in query)in datareader
    Public Function getData(query As String) As SqlDataReader

        Dim sqlCommand As SqlCommand = New SqlCommand()

        sqlCommand.CommandText = query
        sqlCommand.Connection = mycon

        Dim myr As SqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

        Return myr
    End Function  '   getData
    '  Save data usually,inserts and updates the data in table given in query
    Public Sub saveData(query As String)

        Dim sqlCommand As SqlCommand = New SqlCommand()

        sqlCommand.CommandText = query
        sqlCommand.Connection = mycon
        sqlCommand.ExecuteNonQuery()
        sqlCommand.Dispose()
    End Sub '   saveData
    '  Save data usually,inserts and updates the data in table given in query
    Public Sub saveNewData(query As String)

        Dim sqlCommand As SqlCommand = New SqlCommand()

        sqlCommand.CommandText = query
        sqlCommand.Connection = mycon
        sqlCommand.ExecuteNonQuery()
        sqlCommand.Dispose()
    End Sub '   saveNewData


    '  Delete data in database depending on the tablename given in query.
    Public Function DeleteData(query As String) As Integer

        Dim sqlCommand As SqlCommand = New SqlCommand()

        sqlCommand.CommandText = query
        sqlCommand.Connection = mycon
        Return sqlCommand.ExecuteNonQuery()
    End Function  '   DeleteData
    '  Get data by paging using datagrid which returns the dataset in datagris
    Public Function getDatabyPaging(query As String) As DataSet

        Dim sqlDataAdapter As SqlDataAdapter = New SqlDataAdapter(query, mycon)
        Dim dataSet As DataSet = New DataSet()

        sqlDataAdapter.Fill(dataSet, "Media")
        Return dataSet
    End Function  '   getDatabyPaging
    '  check a particular value to see the validity of mediaid and userid.This method is called in media and user class.
    Public Function getCheck(query As String) As Integer

        Dim i As Integer
        Dim sqlCommand As SqlCommand = New SqlCommand()

        sqlCommand.CommandText = query
        sqlCommand.Connection = mycon
        i = Convert.ToInt32(sqlCommand.ExecuteScalar())
        Return i
    End Function  '   getCheck
    '  Get data by paging using datagrid which returns the dataset in datagris
    Public Function getDataforUpdate(query As String) As SqlDataAdapter

        Dim sqlDataAdapter As SqlDataAdapter = New SqlDataAdapter(query, mycon)
        Dim dataSet As DataSet = New DataSet()
        ' sqlDataAdapter.Fill(dataSet,"NewData")
        Return sqlDataAdapter
    End Function  '   getDataforUpdate
    '  Get a value of limit from the database table Employees to check before issuing media.
    Public Function getValue(query As String, j As Integer) As String

        Dim i As String = "0"

        Dim sqlCommand As SqlCommand = New SqlCommand()

        sqlCommand.CommandText = query
        sqlCommand.Connection = mycon

        Dim myReader As SqlDataReader = sqlCommand.ExecuteReader()


        If (myReader.Read() = True) Then

            i = myReader.GetValue(j).ToString()
        End If

        myReader.Close()
        Return i
    End Function  '   getValue

    Public Function CreateNewItem(Title As String, URL As String, StarType As Integer) As Integer

        '  Execute SQL Command
        Dim sqlCmd As SqlCommand = New SqlCommand()

        AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, Nothing)
        AddParamToSQLCmd(sqlCmd, "@Title", SqlDbType.NText, 255, ParameterDirection.Input, Title)
        AddParamToSQLCmd(sqlCmd, "@URL", SqlDbType.NText, 255, ParameterDirection.Input, URL)
        AddParamToSQLCmd(sqlCmd, "@StarType", SqlDbType.Int, 0, ParameterDirection.Input, StarType)
        SetCommandType(sqlCmd, CommandType.StoredProcedure, "AddRatings_Item_Create")

        sqlCmd.Connection = mycon

        Dim result As Object = Nothing

        result = sqlCmd.ExecuteScalar()

        Return (CType(sqlCmd.Parameters("@ReturnValue").Value, Integer))
    End Function  '   CreateNewItem


    Public Function UpdateItem(Title As String, URL As String, StarType As Integer) As Integer

        '  Execute SQL Command
        Dim sqlCmd As SqlCommand = New SqlCommand()

        AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, Nothing)
        AddParamToSQLCmd(sqlCmd, "@Title", SqlDbType.NText, 255, ParameterDirection.Input, Title)
        AddParamToSQLCmd(sqlCmd, "@URL", SqlDbType.NText, 255, ParameterDirection.Input, URL)
        AddParamToSQLCmd(sqlCmd, "@StarType", SqlDbType.Int, 0, ParameterDirection.Input, StarType)
        SetCommandType(sqlCmd, CommandType.StoredProcedure, "AddRatings_Item_Update")

        sqlCmd.Connection = mycon

        Dim result As Object = Nothing

        result = sqlCmd.ExecuteScalar()

        Dim returnValue As Integer = CType(sqlCmd.Parameters("@ReturnValue").Value, Integer)

        Return returnValue
    End Function  '   UpdateItem


    Private Sub SetCommandType(sqlCmd As SqlCommand, cmdType As CommandType, cmdText As String)

        sqlCmd.CommandType = cmdType
        sqlCmd.CommandText = cmdText
    End Sub '   SetCommandType

    Private Sub AddParamToSQLCmd(sqlCmd As SqlCommand, paramId As String, sqlType As SqlDbType, paramSize As Integer, paramDirection As ParameterDirection, paramvalue As Object)

        '  Validate Parameter Properties

        If (sqlCmd Is Nothing) Then
            Throw (New ArgumentNullException("sqlCmd"))
        End If

        If (paramId = String.Empty) Then
            Throw (New ArgumentOutOfRangeException("paramId"))
        End If

        '  Add Parameter
        Dim newSqlParam As SqlParameter = New SqlParameter()

        newSqlParam.ParameterName = paramId
        newSqlParam.SqlDbType = sqlType
        newSqlParam.Direction = paramDirection


        If (paramSize > 0) Then
            newSqlParam.Size = paramSize
        End If


        If (paramvalue IsNot Nothing) Then
            newSqlParam.Value = paramvalue
        End If

        sqlCmd.Parameters.Add(newSqlParam)
    End Sub '   AddParamToSQLCmd

    Public Function DeleteItem(ItemID As Integer) As Boolean

        '  Execute SSQL Command
        Dim sqlCmd As SqlCommand = New SqlCommand()

        AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, Nothing)
        AddParamToSQLCmd(sqlCmd, "@ItemIdToDelete", SqlDbType.Int, 0, ParameterDirection.Input, ItemID)

        SetCommandType(sqlCmd, CommandType.StoredProcedure, "AddRatings_Item_Delete")
        sqlCmd.Connection = mycon

        Dim result As Object = Nothing

        result = sqlCmd.ExecuteScalar()

        Dim returnValue As Integer = CType(sqlCmd.Parameters("@ReturnValue").Value, Integer)

        Return (returnValue = 0)
    End Function  '   DeleteItem
End Class   '   clsDataAccess
' ..\Project_07\ASPAJAX\3-AspAjax\App_Code\clsDataAccess.cs
