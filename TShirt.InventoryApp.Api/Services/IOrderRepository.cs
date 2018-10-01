using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public interface IOrderRepository
    {

        OrderTShirt GetOrderByCode(string code);
        IEnumerable<ViewOrderExtend> GetOrdersDetails(string code);
        IEnumerable<ViewOrderExtend> GetOrdersDetailsArray(string[] codes);
        IEnumerable<OrderDetailProduct> GetOrderDetailProductByCode(List<OrderDetailProduct> codes);
        OrderDetailProduct AddOrderDetailProduct(OrderDetailProduct detail);
        //IEnumerable<ViewOrder> GetOrdersDetailsArray(List<string> items);
        IEnumerable<ViewOrderExtend> GetOrdersDetails(List<OrderTShirt> items);
        bool UpdateOrderDetail(string order, string product, string warehouse);
        bool UpdateOrderStatus(string order, string status);
        RctExtendModel SaveOrderAndGenerateXML(RctExtendModel rct);
    }
}
