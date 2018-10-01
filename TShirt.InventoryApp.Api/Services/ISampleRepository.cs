using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public interface ISampleRepository
    {
        Sample GetById(int id);
        int Save(Sample items);
        bool Update(Sample items);
        List<ViewSampleSumProduct> GetList();
    }
}
