using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TShirt.InventoryApp.WCF.Contract
{
    [DataContract]
    public class ExceptionMessage
    {
        private string infoExceptionMessage;
        public ExceptionMessage(string Message)
        {
            this.infoExceptionMessage = Message;
        }
        [DataMember]
        public string errorMessageOfAction
        {
            get { return this.infoExceptionMessage; }
            set { this.infoExceptionMessage = value; }
        }
    }
}