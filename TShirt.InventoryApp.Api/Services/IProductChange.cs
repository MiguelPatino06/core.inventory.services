using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public interface IProductChange
    {
        OrderReqExtend GetOrderByCode(string code);
        List<OrderReqExtend> GetAll();
        OrderReqExtend GetDetailByCode(string code);
        bool UpdateOrder(OrderReqExtend items);
        bool UpdateDetail(OrderReqDetailExtend items);
        bool Save(OrderReqExtend items);
        List<OrderReqDetailExtend> GetAllDetail();

        List<OrderReqDetailProduct> GetOrderRedDetailProducts(string code);
        List<OrderReqDetailExtend> GetOrderRedDetailExtendProduct(string code);
        List<OrderReqDetailProduct> GetOrderRedDetailProducts2(string code);
    }
}
