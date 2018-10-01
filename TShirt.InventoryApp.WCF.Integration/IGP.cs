using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TShirt.InventoryApp.WCF.Integration.Models;

namespace TShirt.InventoryApp.WCF.Integration
{
    
    [ServiceContract]
    public interface IGp
    {
        [OperationContract]
        string TransactionIn();

        string Message();
    }
}
