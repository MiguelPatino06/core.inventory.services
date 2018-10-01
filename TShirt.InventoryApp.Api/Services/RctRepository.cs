using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public class RctRepository : IRctRepository
    {
        DatabaseHelper context = new DatabaseHelper();
        OrderRepository _order = new OrderRepository();


        public RctExtendModel Add(RctExtendModel rct)
        {
            try
            {
                Rct _rct = new Rct();
                RctDetail _detail = new RctDetail();
                var _date = DateTime.Now.ToString("dd/MM/yyyy/hh:mm");
                var _lote = rct.Lot;

                _rct.Lot = _lote;
                _rct.DateCreated = _date;
                _rct.ProviderCode = rct.ProviderCode;
                _rct.UserId = rct.UserId;

                var addRct = context.Rcts.Add(_rct);
                var result = context.SaveChanges();

                int rctId = addRct.Id;


                //add warehouse to OrderDetail
                foreach (var item in rct.Details)
                {
                    bool updateOrderDetail = _order.UpdateOrderDetail(item.OrderCode, item.ProductCode, item.Warehouse);
                }

                var list = new List<Detail>();
                //add RctDetail
                foreach (var detail in rct.Details.Select(a=> a.OrderCode).Distinct().ToList())
                {

                    string _status = "Abierta";
                    _detail.RctId = rctId;
                    _detail.OrderCode = detail;   
                    context.RctDetails.Add(_detail);
                    context.SaveChanges();


                    //UPDATE STATUS ORDER
                    var statusOrder = rct.Details.Any(a => a.OrderCode == detail && a.Status == "0");
                    if (!statusOrder)
                    {
                        _status = "Cerrada";
                        bool updateOrder = _order.UpdateOrderStatus(detail, "1");
                    }

                    list.Add(new Detail() { OrderCode = detail, Status = _status} );

                }



                //UPDATE ORDER LOTE
                var ordersGroup = rct.Details.GroupBy(a => a.OrderCode)
                   .Select(grp => grp.First())
                   .ToList();

                foreach (var item in ordersGroup)
                {
                    
                    var _orders = context.Orders.FirstOrDefault(a => a.Code == item.OrderCode);
                    _orders.Value2 = _lote;

                    context.Entry(_orders).State = EntityState.Modified;
                    context.SaveChanges();
                }
               


                rct.Id = rctId;              
                rct.DateCreated = _date;
                rct.Details = list;

                return rct;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public IEnumerable<Rct> GetAll()
        {
            return context.Rcts.ToList();
        }

        public Rct GetByCode(string  code)
        {
            try
            {
                return context.Rcts.First(a => a.Code == code);

            }
            catch (Exception)
            {
                return null;
            }

        }

        public bool Delete(int id)
        {
            bool success = true;
            try
            {
                Rct rct = context.Rcts.Find(id);
                context.Rcts.Remove(rct);
                context.SaveChanges();
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }

        public bool Update(Rct rct)
        {
            bool success = true;
            try
            {
                var existingRct = context.Rcts.First(a => a.Id == rct.Id);
                if (existingRct != null)
                {
                    existingRct.Lot = rct.Lot;
                }
                    

                context.Entry(existingRct).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }
    }
}