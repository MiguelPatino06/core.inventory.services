using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TShirt.InventoryApp.Integration
{
    class WarningEvent:Event,ISubject, IEvent
    {
        public WarningEvent(string filed, string sqlc, string msg)
            : base("WARNING_EVENT", 0, "WARNING_EVENT.log", "", msg)
        {}

        void IEvent.Publish()
        {
            ObserverManager.Instance.addSubject(this);
            WriteToEventLog(this.Message, eEventLogType.ELT_WARNING);
  
        }

        #region Miembros de ISubject

        string ISubject.toString()
        {
            return this.Name + " - " + this.Message + '\n' + '\n';
        }

        #endregion
    }
}
