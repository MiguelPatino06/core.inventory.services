using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TShirt.InventoryApp.WCF.Contract;

namespace TShirt.InventoryApp.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICountService" in both code and config file together.
    [ServiceContract]
    public interface ICountService
    {
        [OperationContract]
        List<Plan> GetAll();

        [OperationContract]
        List<CountPlanDetail> GetById(int id);

        [OperationContract]
        string Save(List<CountPlanDetailItem> items);

        [OperationContract]
        List<CountPlanDetailItem> GetByPlanAndProduct(int plan, string product);

        [OperationContract]
        bool Update(CountPlan items);
    }
}
