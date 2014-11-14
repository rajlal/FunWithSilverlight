using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;

using System.Configuration;

/// <summary>
/// Summary description for clsDataAccess
/// </summary>
public class clsDataAccess // Class defination
{
    public clsDataAccess()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    SqlConnection mycon = new SqlConnection(ConfigurationSettings.AppSettings["ConnectionString"]);
    public bool openConnection() // Opens database connection with Granth in SQL SERVER
    {
        mycon.Open();
        return true;
    }
    public void closeConnection() // Closes database connection with Granth in SQL SERVER
    {
        mycon.Close();
        mycon = null;
    }
    public SqlDataReader getData(string query) // Getdata from the table required(given in query)in datareader
    {
        SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.CommandText = query;
        sqlCommand.Connection = mycon;
        SqlDataReader myr = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
        return myr;

    }
    public void saveData(string query) // Save data usually,inserts and updates the data in table given in query
    {
        SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.CommandText = query;
        sqlCommand.Connection = mycon;
        sqlCommand.ExecuteNonQuery();
        sqlCommand.Dispose();

    }
    public void saveNewData(string query) // Save data usually,inserts and updates the data in table given in query
    {
        SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.CommandText = query;
        sqlCommand.Connection = mycon;
        sqlCommand.ExecuteNonQuery();
        sqlCommand.Dispose();

    }
    public int DeleteData(string query) // Delete data in database depending on the tablename given in query.
    {
        SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.CommandText = query;
        sqlCommand.Connection = mycon;
        return sqlCommand.ExecuteNonQuery();

    }
    public DataSet getDatabyPaging(string query) // Get data by paging using datagrid which returns the dataset in datagris
    {
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, mycon);
        DataSet dataSet = new DataSet();
        sqlDataAdapter.Fill(dataSet, "Media");
        return dataSet;

    }
    public int getCheck(string query) // check a particular value to see the validity of mediaid and userid.This method is called in media and user class.
    {
        int i;
        SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.CommandText = query;
        sqlCommand.Connection = mycon;
        i = Convert.ToInt32(sqlCommand.ExecuteScalar());
        return i;
    }
    public SqlDataAdapter getDataforUpdate(string query) // Get data by paging using datagrid which returns the dataset in datagris
    {
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, mycon);
        DataSet dataSet = new DataSet();
        //sqlDataAdapter.Fill(dataSet,"NewData");
        return sqlDataAdapter;
    }
    public string getValue(string query, int j) // Get a value of limit from the database table Employees to check before issuing media.
    {
        string i = "0";

        SqlCommand sqlCommand = new SqlCommand();
        sqlCommand.CommandText = query;
        sqlCommand.Connection = mycon;
        SqlDataReader myReader = sqlCommand.ExecuteReader();

        if (myReader.Read() == true)
        {

            i = myReader.GetValue(j).ToString();

        }
        myReader.Close();
        return i;
    }
    public int CreateNewItem(string Title, string URL, int StarType)
    {
        // Execute SQL Command
        SqlCommand sqlCmd = new SqlCommand();

        AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
        AddParamToSQLCmd(sqlCmd, "@Title", SqlDbType.NText, 255, ParameterDirection.Input, Title);
        AddParamToSQLCmd(sqlCmd, "@URL", SqlDbType.NText, 255, ParameterDirection.Input, URL);
        AddParamToSQLCmd(sqlCmd, "@StarType", SqlDbType.Int, 0, ParameterDirection.Input, StarType);
        SetCommandType(sqlCmd, CommandType.StoredProcedure, "AddRatings_Item_Create");

        sqlCmd.Connection = mycon;
        Object result = null;
        result = sqlCmd.ExecuteScalar();

        return ((int)sqlCmd.Parameters["@ReturnValue"].Value);
    }
    public int UpdateItem(string Title, string URL, int StarType)
    {

        // Execute SQL Command
        SqlCommand sqlCmd = new SqlCommand();

        AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
        AddParamToSQLCmd(sqlCmd, "@Title", SqlDbType.NText, 255, ParameterDirection.Input, Title);
        AddParamToSQLCmd(sqlCmd, "@URL", SqlDbType.NText, 255, ParameterDirection.Input, URL);
        AddParamToSQLCmd(sqlCmd, "@StarType", SqlDbType.Int, 0, ParameterDirection.Input, StarType);
        SetCommandType(sqlCmd, CommandType.StoredProcedure, "AddRatings_Item_Update");

        sqlCmd.Connection = mycon;
        Object result = null;

        result = sqlCmd.ExecuteScalar();
        int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
        return returnValue;
    }
    private void SetCommandType(SqlCommand sqlCmd, CommandType cmdType, string cmdText)
    {
        sqlCmd.CommandType = cmdType;
        sqlCmd.CommandText = cmdText;
    }
    private void AddParamToSQLCmd(SqlCommand sqlCmd, string paramId, SqlDbType sqlType, int paramSize, ParameterDirection paramDirection, object paramvalue)
    {
        // Validate Parameter Properties
        if (sqlCmd == null)
            throw (new ArgumentNullException("sqlCmd"));
        if (paramId == string.Empty)
            throw (new ArgumentOutOfRangeException("paramId"));

        // Add Parameter
        SqlParameter newSqlParam = new SqlParameter();
        newSqlParam.ParameterName = paramId;
        newSqlParam.SqlDbType = sqlType;
        newSqlParam.Direction = paramDirection;

        if (paramSize > 0)
            newSqlParam.Size = paramSize;

        if (paramvalue != null)
            newSqlParam.Value = paramvalue;

        sqlCmd.Parameters.Add(newSqlParam);
    }
    public bool DeleteItem(int ItemID)
    {
        // Execute SSQL Command
        SqlCommand sqlCmd = new SqlCommand();

        AddParamToSQLCmd(sqlCmd, "@ReturnValue", SqlDbType.Int, 0, ParameterDirection.ReturnValue, null);
        AddParamToSQLCmd(sqlCmd, "@ItemIdToDelete", SqlDbType.Int, 0, ParameterDirection.Input, ItemID);

        SetCommandType(sqlCmd, CommandType.StoredProcedure, "AddRatings_Item_Delete");
        sqlCmd.Connection = mycon;
        Object result = null;
        result = sqlCmd.ExecuteScalar();
        int returnValue = (int)sqlCmd.Parameters["@ReturnValue"].Value;
        return (returnValue == 0 ? true : false);
    }


}
