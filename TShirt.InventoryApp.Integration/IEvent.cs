using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TShirt.InventoryApp.Integration
{

    public enum eEventLogType
    {
        ELT_INFORMATION,
        ELT_WARNING,
        ELT_ERROR
    };


    public abstract class Event
    {
         string mName;
         int mId;
         string mFileDump;
         string mSQLCom;
         string mMessage;
         DateTime mDateTime;

         public string Name
         {
             get { return this.mName; }
         }

         public int Id
         {
             get { return this.mId; }
         }

         public string FileDump
         {
             get { return this.mFileDump; }
         }

         public string SQLCom
         {
             get { return this.mSQLCom; }
         }

         public string Message
         {
             get { return this.mMessage; }
         }

         public DateTime Dateti
         {
             get { return this.mDateTime; }
         }

        public Event(string name, int id,string filed,string sqlc,string msg)
         {
            this.mName = name;
            this.mId = id;
            this.mFileDump = filed;
            this.mSQLCom = sqlc;
            this.mMessage = msg;

            this.mDateTime = DateTime.Now;
        }

        
        public void WriteToEventLog(string message, eEventLogType elt)
        {
            string cs = "Green Retail Integration Service";
            string sLog = "Green Retail";

            if (!EventLog.SourceExists(cs))
                EventLog.CreateEventSource(cs, sLog);

            switch (elt)
            {
                case eEventLogType.ELT_ERROR:
                    EventLog.WriteEntry(cs, message, EventLogEntryType.Error);
                    break;
                case eEventLogType.ELT_INFORMATION:
                    EventLog.WriteEntry(cs, message, EventLogEntryType.Information);
                    break;
                case eEventLogType.ELT_WARNING:
                    EventLog.WriteEntry(cs, message, EventLogEntryType.Warning);
                    break;
            };
            
        }



    }

    public interface IEvent
    {

        void Publish();



    }
}
