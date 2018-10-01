using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.IO;
using System.Threading;
using Microsoft.Dynamics.GP.eConnect;

//using System.Configuration;

namespace TShirt.InventoryApp.Services.FileWatcher
{
    public partial class FileWatcherService : ServiceBase
    {
        string wathO = ConfigurationManager.AppSettings["FilePathO"];
        string wathD = ConfigurationManager.AppSettings["FilePathD"];


        string eConnectConnectionString = ConfigurationManager.AppSettings["ConectDB"];


        public FileWatcherService()
        {
            InitializeComponent();
            FSWatcher.Created += watcher_Created;
        }

        protected override void OnStart(string[] args)
        {

            FSWatcher.Path = wathO;
            FSWatcher.Created += watcher_Created;

        }

        protected override void OnStop()
        {
        }

        protected void ReadFileAndSendGp()
        {
            
        }

        string Message()
        {
            return "Hello";
        }
        void watcher_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("File created. Name: {0}", e.Name);
            string sSource;
            string sLog;
            string sEvent;
            try
            {
                Thread.Sleep(70000);
                //Then we need to check file is exist or not which is created.  
                if (CheckFileExistance(wathO, e.Name))
                {
                    bool res = true;
                    try
                    {
                        sSource = "GP Integration TShirt1";
                        sLog = "Application";
                        

                        if (!EventLog.SourceExists(sSource))
                            EventLog.CreateEventSource(sSource, sLog);

                        FileStream fs = new FileStream(wathO + "\\" + e.FullPath, FileMode.Open, FileAccess.Read);

                        sEvent = fs.ToString();
                        if (!EventLog.SourceExists(sSource))
                            EventLog.CreateEventSource(sSource, sLog);

                        EventLog.WriteEntry(sSource, sEvent);


                        //eConnectMethods eConCall = new eConnectMethods();
                        //res = eConCall.eConnect_EntryPoint(eConnectConnectionString, EnumTypes.ConnectionStringType.SqlClient,
                        //    fs.ToString(), EnumTypes.SchemaValidationType.None, "");
                    }
                    catch (Exception ex)
                    {


                        sSource = "GP Integration TShirt2";
                        sLog = "Application";
                        sEvent = ex.Message;

                        if (!EventLog.SourceExists(sSource))
                            EventLog.CreateEventSource(sSource, sLog);

                        EventLog.WriteEntry(sSource, sEvent);
                        //EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 234);


                        throw;
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

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
    }
}
