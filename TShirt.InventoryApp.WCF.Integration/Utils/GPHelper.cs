using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Dynamics.GP.eConnect;
using Microsoft.Dynamics.GP.eConnect.MiscRoutines;
using System.Diagnostics;

namespace TShirt.InventoryApp.WCF.Integration.Utils
{
    public sealed class GPHelper
    {
    //    public static string InvoiceDOCID(string con)
    //    {

    //    SqlCommand command = new SqlCommand();
    //    string res = "";

    //    command.Connection = new SqlConnection(con);

    //    command.CommandType = System.Data.CommandType.Text;

    //    command.CommandText = "SELECT INVDOCID FROM SOP40100";

    //    try{
    //        if(command.Connection.State != ConnectionState.Open)
    //        {
    //            command.Connection.Open();
    //        }
    //        res = (string) command.ExecuteScalar();

    //    }catch(Exception ex)
    //    {
    //        IEvent e = new ErrorEvent("","",ex.Message);
    //        e.Publish();
            
    //    }finally
    //    {
    //        command.Connection.Close();
    //    }

    //    return res;
    //}

        //public static string returnDOCID(string con)
        //{

        //    SqlCommand command = new SqlCommand();
        //    string res = "";

        //    command.Connection = new SqlConnection(con);

        //    command.CommandType = System.Data.CommandType.Text;

        //    command.CommandText = "SELECT RETDOCID FROM SOP40100";

        //    try
        //    {
        //        if (command.Connection.State != ConnectionState.Open)
        //        {
        //            command.Connection.Open();
        //        }
        //        res = (string)command.ExecuteScalar();

        //    }
        //    catch (Exception ex)
        //    {
        //        IEvent e = new ErrorEvent("", "", ex.Message);
        //        e.Publish();

        //    }
        //    finally
        //    {
        //        command.Connection.Close();
        //    }

        //    return res;
        //}

        //public static bool insertFiscalInfo(string SOPNUMBE, string COO, string DATEGEN, string SERIALPRINTER,string con)
        //{
        //    SqlCommand command = new SqlCommand();
        //    bool res = false;

        //    command.Connection = new SqlConnection(con);

        //    command.CommandType = System.Data.CommandType.Text;

        //    command.Parameters.AddWithValue("@INVOICE", SOPNUMBE);
        //    command.Parameters.AddWithValue("@COO", COO);
        //    command.Parameters.AddWithValue("@DATEGEN", DATEGEN);
        //    command.Parameters.AddWithValue("@SERIALPRINTER", SERIALPRINTER);

        //    command.CommandText = "INSERT INTO CSG_FISCAL_PRINT (COO,INVOICE,DATEGEN,SERIALPRINTER) VALUES(@COO,@INVOICE,@DATEGEN,@SERIALPRINTER);";

        //    try
        //    {
        //        if (command.Connection.State != ConnectionState.Open)
        //        {
        //            command.Connection.Open();
        //        }
        //        res = command.ExecuteNonQuery()>0;

        //    }
        //    catch (Exception ex)
        //    {
        //        IEvent e = new ErrorEvent("", "", ex.Message);
        //        e.Publish();

        //    }
        //    finally
        //    {
        //        command.Connection.Close();
        //    }

        //    return res;
        //}

        public static bool eConnectSendToGP(string eConnectDocument,string TrustedConnStr)
        {
            //string status;
            bool res = true;
            try
            {
                eConnectMethods eConCall = new eConnectMethods();

                res = eConCall.eConnect_EntryPoint(TrustedConnStr, EnumTypes.ConnectionStringType.SqlClient,
                    eConnectDocument, EnumTypes.SchemaValidationType.None, "");
                //'status = eConCall.CreateTransactionEntity(eConnect_ConnectionString, Xml)
            }
            catch (Exception ex)
            {
                string sSource;
                string sLog;
                string sEvent;

                sSource = "GP Integration TShirt";
                sLog = "Application";
                sEvent = ex.Message;

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, sEvent);
                EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 234);

            }
            finally
            {
            }
            return res;
       

        }

        //public static string eConnectGetNextSOPinvoiceNumbe(string DOCID, string con)
        //{
        //    string res = "";

        //    GetNextDocNumbers NextDocNumberObject = new GetNextDocNumbers();
        //    try
        //    {
        //        res = NextDocNumberObject.GetNextSOPNumber(GetNextDocNumbers.IncrementDecrement.Increment, DOCID, GetNextDocNumbers.SopType.SOPInvoice, con);
        //    }
        //    catch (Exception ex)
        //    {
        //        IEvent e = new ErrorEvent("", "", ex.Message);
        //        e.Publish();
        //        res = ex.Message;

        //    }
        //    return res;
        //}

        //public static string eConnectGetNextSOPReturnNumbe(string DOCID, string con)
        //{
        //    string res = "";

        //    GetNextDocNumbers NextDocNumberObject = new GetNextDocNumbers();
        //    try
        //    {
        //        res = NextDocNumberObject.GetNextSOPNumber(GetNextDocNumbers.IncrementDecrement.Increment, DOCID, GetNextDocNumbers.SopType.SOPReturn, con);
        //    }
        //    catch (Exception ex)
        //    {
        //        IEvent e = new ErrorEvent("", "", ex.Message);
        //        e.Publish();
        //        res = ex.Message;

        //    }
        //    return res;
        //}
    }
}
