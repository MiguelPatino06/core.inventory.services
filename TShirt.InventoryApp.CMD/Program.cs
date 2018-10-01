using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Dynamics.GP.eConnect;

namespace TShirt.InventoryApp.CMD
{
    class Program
    {
        static void Main(string[] args)
        {


            string path1 = @"C:\APPLEGREEN\XMLFILE\firs.xml";
            string path2 = @"C:\APPLEGREEN\XMLFILE2\";


            System.IO.File.Move(path1, path2);

            //string eConnectConnectionString = ConfigurationManager.AppSettings["ConectDB"];  

            //string queryXML = "<eConnect>";
            //queryXML += "<IVInventoryTransactionType>";
            //queryXML += "<taIVTransactionLotInsert_Items>"; //LOT 
            //queryXML += "<taIVTransactionLotInsert>";
            //queryXML += "<IVDOCNBR>AJP</IVDOCNBR>";
            //queryXML += "<IVDOCTYP>1</IVDOCTYP>";    //TODOS DEBEN SER 1
            //queryXML += "<ITEMNMBR>TTC3700C0110A62</ITEMNMBR>";
            //queryXML += "<SERLTNUMBR>AJP</SERLTNUMBR>"; 
            ////queryXML += "<LOTNUMBR>AJPTEST</LOTNUMBR>"; //TODO VER SI LO ELIMINAMOS
            //queryXML += "<SERLTQTY>2</SERLTQTY>";
            //queryXML += "<LOCNCODE>TSHIRT</LOCNCODE>";
            //queryXML += "</taIVTransactionLotInsert>";
            //queryXML += "</taIVTransactionLotInsert_Items>";
            //queryXML += "<taIVTransactionLineInsert_Items>";  //LINE
            //queryXML += "<taIVTransactionLineInsert>";
            //queryXML += "<IVDOCNBR>AJP</IVDOCNBR>";
            //queryXML += "<IVDOCTYP>1</IVDOCTYP>";
            //queryXML += "<ITEMNMBR>TTC3700C0110A62</ITEMNMBR>";
            //queryXML += "<Reason_Code>TTC3700C0110A62</Reason_Code>";
            //queryXML += "<LNSEQNBR></LNSEQNBR>";
            //queryXML += "<TRXQTY>2</TRXQTY>";
            //queryXML += "<UNITCOST></UNITCOST>";
            //queryXML += "<TRXLOCTN>FLEXO</TRXLOCTN>";
            //queryXML += "<IVIVINDX></IVIVINDX>";
            //queryXML += "<InventoryAccount></InventoryAccount>";
            //queryXML += "<IVIVOFIX></IVIVOFIX>";
            //queryXML += "<InventoryAccountOffSet></InventoryAccountOffSet>";
            //queryXML += "</taIVTransactionLineInsert>";
            //queryXML += "</taIVTransactionLineInsert_Items>";
            //queryXML += "<taIVTransactionHeaderInsert>";       //HEADER
            //queryXML += "<BACHNUMB>TSHIRT</BACHNUMB>";
            //queryXML += "<IVDOCNBR>AJP</IVDOCNBR>";
            //queryXML += "<IVDOCTYP>1</IVDOCTYP>";
            //queryXML += "<DOCDATE>1900-01-01 00:00:00.000</DOCDATE>";
            //queryXML += "<POSTTOGL>0</POSTTOGL>";
            //queryXML += "<USRDEFND1>PRUEBA MIGUEL PATIÑO</USRDEFND1>";
            //queryXML += "<USRDEFND2></USRDEFND2>";
            //queryXML += "<USRDEFND3></USRDEFND3>";
            //queryXML += "<USRDEFND4></USRDEFND4>";
            //queryXML += "<USRDEFND5></USRDEFND5>";
            //queryXML += "</taIVTransactionHeaderInsert>";
            //queryXML += "</IVInventoryTransactionType>";
            //queryXML += "</eConnect>";


            //bool res = true;
            //try
            //{
            //    eConnectMethods eConCall = new eConnectMethods();

            //    res = eConCall.eConnect_EntryPoint(eConnectConnectionString, EnumTypes.ConnectionStringType.SqlClient,
            //        queryXML, EnumTypes.SchemaValidationType.None, "");
            //    //'status = eConCall.CreateTransactionEntity(eConnect_ConnectionString, Xml)
            //}
            //catch (Exception ex)
            //{

            //}

        }
    }
}
