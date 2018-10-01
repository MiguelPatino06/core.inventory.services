using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Api.Models;


namespace TShirt.InventoryApp.Api.Services
{
    public interface IProviderRepository
    {
        Provider GetByCode(string code);
        IEnumerable<OrderTShirt> GetOrdersByProviderCode(string code);
        IEnumerable<Provider> GetProviderByName(string name);


    }
}
