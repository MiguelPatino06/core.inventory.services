using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.WCF.Contract;

namespace TShirt.InventoryApp.WCF.Interface
{
    public interface ICountRepository
    {
        List<Plan> GetAll();
        List<CountPlanDetail> GetById(int id);
        string Save(List<CountPlanDetailItem> items);
        List<CountPlanDetailItem> GetByPlanAndProduct(int plan, string product);
        bool Update(CountPlan items);
    }
}
