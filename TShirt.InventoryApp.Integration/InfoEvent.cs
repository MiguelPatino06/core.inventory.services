using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TShirt.InventoryApp.Integration
{
    class InfoEvent : Event,ISubject, IEvent
    {
        public InfoEvent(string filed, string sqlc, string msg): base("INFO_EVENT",0,"INFO_EVENT.log","",msg)
        {}
        
        void IEvent.Publish()
        {
            ObserverManager.Instance.addSubject(this);
            
            WriteToEventLog(this.Message, eEventLogType.ELT_INFORMATION);
            
            
        }

   
        #region Miembros de ISubject

        string ISubject.toString()
        {
            return this.Name + " - " + this.Message + '\n' + '\n';
        }

        #endregion
    }
}
