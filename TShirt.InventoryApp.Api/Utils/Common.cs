using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.SqlServer.Server;

namespace TShirt.InventoryApp.Api.Utils
{
  public class Common
  {

    //    public string getNextNumber(string selSI_IV_TYPE_ID)
    //{
    //  string nextNumber = string.Empty;
    //  ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["TSGVL"];
    //  SqlConnection cnn = new SqlConnection(settings.ConnectionString);
    //  SqlCommand command;
    //  SqlDataReader dataReader;
    //  string sql;
    //  try
    //  {
    //    cnn.Open();
    //    sql = "Select selSI_NextNumber from selSI_IV40100 where selSI_IV_TYPE_ID='" + selSI_IV_TYPE_ID + "'";
    //    command = new SqlCommand(sql, cnn);
    //    dataReader = command.ExecuteReader();
    //    while (dataReader.Read())
    //    {
    //      nextNumber = dataReader.GetValue(0).ToString().Trim();
    //    }

    //    dataReader.Close();
    //    command.Dispose();
    //  }
    //  catch (Exception ex)
    //  {
    //    System.Diagnostics.Debug.WriteLine("ex " + ex.Message);
    //    return null;
    //  }
    //  finally
    //  {
    //    cnn.Close();
    //  }

    //  return nextNumber;

    //}
    public string getNextNumber(string fieldName, string tableName, string searchName, string valueSearch)
    {
      string nextNumber = string.Empty;
      ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["TSGVL"];
      SqlConnection cnn = new SqlConnection(settings.ConnectionString);
      SqlCommand command;
      SqlDataReader dataReader;
      string sql;
      try
      {
        cnn.Open();
        sql = "SELECT " + fieldName + " FROM "+ tableName + " WHERE " + searchName +  " ='" + valueSearch + "'";
        command = new SqlCommand(sql, cnn);
        dataReader = command.ExecuteReader();
        while (dataReader.Read())
        {
          nextNumber = dataReader.GetValue(0).ToString().Trim();
        }

        dataReader.Close();
        command.Dispose();
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine("ex " + ex.Message);
        return null;
      }
      finally
      {
        cnn.Close();
      }

      return nextNumber;

    }
  }
}