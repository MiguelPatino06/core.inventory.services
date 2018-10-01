using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Security.Policy;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Dynamics.GP.eConnect;


namespace TShirt.InventoryApp.Service.FileWath
{
    public partial class FileWatcherService : ServiceBase
    {
        string wathO = ConfigurationManager.AppSettings["FilePathO"];
        string wathD = ConfigurationManager.AppSettings["FilePathD"];
        string wathE = ConfigurationManager.AppSettings["FilePathE"];
        string pathBD = ConfigurationManager.AppSettings["ConectDB"];


        static ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["TSGVL"];
        string pathBDServer = settings.ConnectionString;



        public FileWatcherService()
        {
            InitializeComponent();
            fileSystemWatcher.Created += watcher_Created;
        }

        protected override void OnStart(string[] args)
        {
            fileSystemWatcher.Path = wathO;
        }

        protected override void OnStop()
        {
        }

        
        //public void watcher_Created()
        public void watcher_Created(object sender, FileSystemEventArgs e)
        {
            string sSource;
            string sLog;
            string sEvent;
            string documentType;
            string sName = "RCT2001";
      
            string pathFile = e.FullPath; 



            try
            {
                Thread.Sleep(70000);
                //Then we need to check file is exist or not which is created.  


                if (CheckFileExistance(wathO, e.Name))
                {
                    bool res = true;
                    try
                    {

                    XDocument xml = XDocument.Load(pathFile);  //lee xml para obtener datos y llenar lineas de tablas SElSI                      
                    string value = File.ReadAllText(pathFile); //lee contenido XML para enviar a GP

                    eConnectMethods eConCall = new eConnectMethods();

                        documentType = e.Name.Substring(0, 3);

                        if (documentType == "AJP" || documentType == "RCT" || documentType == "TXI" || documentType == "CMB" || documentType == "MST")
                        {
                            res = eConCall.eConnect_EntryPoint(pathBD, EnumTypes.ConnectionStringType.SqlClient,
                                value, EnumTypes.SchemaValidationType.None, "");



                            if (res)
                            {
                                System.IO.File.Move(pathFile, wathD + e.Name);

                                //Update codigo 
                                int number = 0;
                                int totalQty = 0;
                                string qry = string.Empty;
                                bool _result = false;
                                int numberDocument = 0;
                                string DocumentCode = string.Empty;
                                switch (documentType)
                                {
                                    case "AJP":
                                       
                                        var q = (from b in xml.Descendants("taIVTransactionHeaderInsert")
                                            select new {code = (string) b.Element("IVDOCNBR") ?? string.Empty})
                                            .FirstOrDefault();

                                        if (q != null) DocumentCode = q.code;

                                        numberDocument = GetNumber(DocumentCode);
                                        number = numberDocument + 1;
                                            //obtine numero de la secuencia y lo incremente en 1

                                        qry = "UPDATE selSI_IV40100 SET selSI_NextNumber ='" + "00000" +
                                              number.ToString() + "' WHERE selSI_IV_Type_ID ='AJP'";
                                        _result = InsertLineInTSGLV(qry); //actualiza secuencia de numero 
                                         
                                      
                                        //qry = string.Empty;
                                        //qry +=
                                        //    "INSERT INTO SElSI_IV_TRX_DATE (ivdoctyp,docnumbr,docdate,inuser) VALUES ";
                                        //qry += "('1', 'AJP" + "00000" + numberDocument + "','" + DateTime.Now +
                                        //       "','dbo')";
                                        //_result = InsertLineInTSGLV(qry); //guarda Header


                                        //var _line = (from b in xml.Descendants("taIVTransactionLotInsert")
                                        //    select new
                                        //    {
                                        //        totalUnits = (string) b.Element("SERLTQTY") ?? string.Empty,

                                        //    }).ToList();

                                        //totalQty += _line.Sum(item => Convert.ToInt32(item.totalUnits));
                                        //    //suma totalUnits


                                        //qry = "";
                                        //qry += "INSERT INTO selSI_IV_TRX_HDR (IVDOCTYP,DOCNUMBR,TotalUnits) VALUES ";
                                        //qry += "('1', 'AJP" + "00000" + numberDocument + "','" + totalQty + "')";

                                        //_result = InsertLineInTSGLV(qry); //guarda Linea


                                        break;
                                    case "RCT":

                                        var y = (from b in xml.Descendants("taPopRcptHdrInsert")
                                                 select new { code = (string)b.Element("POPRCTNM") ?? string.Empty })
                                           .FirstOrDefault();

                                        if (y != null) DocumentCode = y.code;

                                        numberDocument = GetNumber(DocumentCode);
                                        number = numberDocument + 1;

                                        qry = "UPDATE pop40100 SET POPRCTNM ='" + "RCT" +
                                              number.ToString() + "' WHERE INDEX1 = 1";
                                        _result = InsertLineInTSGLV(qry); //actualiza secuencia de numero 

                                        break;
                                    case "TXI":

                                        var z = (from b in xml.Descendants("taIVTransferHeaderInsert")
                                                 select new { code = (string)b.Element("IVDOCNBR") ?? string.Empty })
                                           .FirstOrDefault();

                                        if (z != null) DocumentCode = z.code;

                                        numberDocument = GetNumber(DocumentCode);
                                        number = numberDocument + 1;
                                        //obtine numero de la secuencia y lo incremente en 1

                                        qry = "UPDATE selSI_IV40100 SET selSI_NextNumber ='" + "000" +
                                              number.ToString() + "' WHERE selSI_IV_Type_ID ='TXI'";
                                        _result = InsertLineInTSGLV(qry); //actualiza secuencia de numero 
                                        break;

                                    case "MST":

                                        var x = (from b in xml.Descendants("taIVTransactionHeaderInsert")
                                                 select new { code = (string)b.Element("IVDOCNBR") ?? string.Empty })
                                           .FirstOrDefault();

                                        if (x != null) DocumentCode = x.code;

                                        numberDocument = GetNumber(DocumentCode);
                                        number = numberDocument + 1;
                                        //obtine numero de la secuencia y lo incremente en 1

                                        qry = "UPDATE selSI_IV40100 SET selSI_NextNumber ='" + "000" +
                                              number.ToString() + "' WHERE selSI_IV_Type_ID ='MST'";
                                        _result = InsertLineInTSGLV(qry); //actualiza secuencia de numero 
                                        break;

                                    case "CMB":

                                        var w = (from b in xml.Descendants("taIVTransactionHeaderInsert")
                                                 select new { code = (string)b.Element("IVDOCNBR") ?? string.Empty })
                                            .FirstOrDefault();

                                        if (w != null) DocumentCode = w.code;

                                        numberDocument = GetNumber(DocumentCode);
                                        number = numberDocument + 1;
                                        //obtine numero de la secuencia y lo incremente en 1

                                        qry = "UPDATE selSI_IV40100 SET selSI_NextNumber ='" + "00000" +
                                              number.ToString() + "' WHERE selSI_IV_Type_ID ='CMB'";
                                        _result = InsertLineInTSGLV(qry); //actualiza secuencia de numero 

                                        break;
                                }

                            }
                            else
                            {
                                System.IO.File.Move(pathFile, wathE + e.Name);
                            }
                        }
                        else
                        {
                            sSource = "GP Services TShirt";
                            sLog = "Application";
                            sEvent = "Archivo no Reconocido, las tres primera iniciales del nombre del archivo deben coincidir con el tipo de documento";


                            if (!EventLog.SourceExists(sSource))
                                EventLog.CreateEventSource(sSource, sLog);

                            EventLog.WriteEntry(sSource, sEvent);

                        }

                    }
                    catch (Exception ex)
                    {


                        sSource = "GP Services TShirt";
                        sLog = "Application";
                        sEvent = ex.Message;


                        if (!EventLog.SourceExists(sSource))
                            EventLog.CreateEventSource(sSource, sLog);

                        EventLog.WriteEntry(sSource, sEvent);

                        throw;
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public int GetNumber(string number)
        {
            int value = 0;
            try
            {
                int lenght = number.Length - 3;
                string result = number.Substring(3, lenght);
                value = int.Parse(result);

            }
            catch (Exception ex)
            {
                value = 0;
            }
            return value;
        }


        private bool CheckFileExistance(string FullPath, string FileName)
        {
            // Get the subdirectories for the specified directory.'  
            bool IsFileExist = false;
            DirectoryInfo dir = new DirectoryInfo(FullPath);
            if (!dir.Exists)
                IsFileExist = false;
            else
            {
                string FileFullPath = Path.Combine(FullPath, FileName);
                if (File.Exists(FileFullPath))
                    IsFileExist = true;
            }
            return IsFileExist;
        }

        public bool InsertLineInTSGLV(string query)
        {
            string sSource;
            string sLog;
            string sEvent;
            bool result = true;     
            SqlConnection connection = new SqlConnection(pathBDServer);
            SqlCommand command = new SqlCommand();
           
            string sql;
            try
            {

                command.CommandType = CommandType.Text;
                command.CommandText = query;
                command.Connection = connection;

                connection.Open();
                command.ExecuteNonQuery();
                               
            }
            catch (Exception ex)
            {
                sSource = "Services TShirt";
                sLog = "Application";
                sEvent = ex.Message;

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, sEvent);


                result = false;
                throw;
                
            }
            finally
            {
                connection.Close();
            }

            return result;

        }


        public string getValue(string query)
        {

            string valueSelect = string.Empty;
            SqlConnection connection = new SqlConnection(pathBDServer);
            SqlCommand command;
            SqlDataReader dataReader;
            string sql;
            try
            {
                connection.Open();
            
                command = new SqlCommand(query, connection);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    valueSelect = dataReader.GetValue(0).ToString().Trim();
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
                connection.Close();
            }

            return valueSelect;

        }

    }
}
