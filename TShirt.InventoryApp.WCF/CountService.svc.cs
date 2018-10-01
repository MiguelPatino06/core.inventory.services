using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TShirt.InventoryApp.WCF.Contract;
using TShirt.InventoryApp.WCF.Interface;
using TShirt.InventoryApp.WCF.Services;

namespace TShirt.InventoryApp.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CountService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CountService.svc or CountService.svc.cs at the Solution Explorer and start debugging.
    public class CountService : ICountService
    {
       static ICountRepository repository = new CountRepository();

        public List<Plan> GetAll()
        {
            return repository.GetAll();
        }

        public List<CountPlanDetail> GetById(int id)
        {
            return repository.GetById(id);
        }

        public string Save(List<CountPlanDetailItem> items)
        {
            return repository.Save(items);
        }

        public List<CountPlanDetailItem> GetByPlanAndProduct(int plan, string product)
        {
            return repository.GetByPlanAndProduct(plan, product);
        }

        public bool Update(CountPlan items)
        {
            return repository.Update(items);
        }
    }
}
