using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TShirt.InventoryApp.Integration
{
    public class ErrorEvent : Event,ISubject, IEvent
    {
        public ErrorEvent(string filed, string sqlc, string msg)
            : base("ERROR_EVENT", 0, "ERROR_EVENT.log", "", msg)
        {}
        
        void IEvent.Publish()
        {
            ObserverManager.Instance.addSubject(this);
            WriteToEventLog(this.Message, eEventLogType.ELT_ERROR);
            
        }

        #region Miembros de ISubject

        string ISubject.toString()
        {
            return this.Name + " - " + this.Message + '\n' + '\n';
        }

        #endregion
    
    
    }
}
