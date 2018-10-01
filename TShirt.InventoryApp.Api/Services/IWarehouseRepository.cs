using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public interface IWarehouseRepository
    {
        Warehouse Add(Warehouse warehouse);
        IEnumerable<Warehouse> GetAll();
        Warehouse GetById(int id);
        bool Delete(int id);
        bool Update(Warehouse warehouse);


    }
}
