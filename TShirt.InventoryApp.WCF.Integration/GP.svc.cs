using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TShirt.InventoryApp.WCF.Integration.Models;
using TShirt.InventoryApp.WCF.Integration.Services;

namespace TShirt.InventoryApp.WCF.Integration
{
   
    public class GP : IGp
    {
        private TransactionMakeInvoice transaction;
        public string TransactionIn()
        {
            transaction = new TransactionMakeInvoice();
            transaction.GenerateDocument();
            return string.Empty;
        }

        public string Message()
        {
            return "Hello boy..!";
        }
    }
}
