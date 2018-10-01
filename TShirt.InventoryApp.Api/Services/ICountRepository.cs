using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    interface ICountRepository
    {

        IEnumerable<CountPlan> GetAll();
        List<ViewCountPlanDetail> GetById(int id);
        string Save(List<CountPlanDetailItem> items);
        List<ViewCountPlanDetailItem> GetByPlanAndProduct(int plan, string product);
        bool Update(CountPlan items);
        ViewCountPlanDetailPage GetByIdPage(int id);
        ViewCountPlanDetailPage GetByIdPageTakeSkip(int id, int _take, int _skip);

    }
}
