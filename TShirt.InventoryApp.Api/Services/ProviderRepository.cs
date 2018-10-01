using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public class ProviderRepository : IProviderRepository
    {

        DatabaseHelper context = new DatabaseHelper();

        public Provider GetByCode(string code)
        {
            return context.Providers.FirstOrDefault(a => a.Code == code);
        }

        public IEnumerable<OrderTShirt> GetOrdersByProviderCode(string code)
        {
            var items = new List<OrderTShirt>();
            var list = context.Orders.Where(a => a.ProviderCode == code).ToList();
            foreach (var row in list)
            {
                var exist = context.OrderDetailProducts.Any(a => a.OrderCode == row.Code);

                items.Add( new OrderTShirt()
                {
                    Code = row.Code,
                    Description = row.Description,
                    IsSelected = exist,
                    Value1 = row.Value1
                });
            }
            return items;
        }

        public IEnumerable<Provider> GetProviderByName(string name)
        {
            return context.Providers.Where(a => a.Name.ToUpper().Contains(name.ToUpper().ToString()));
        }
    }
}