using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models;
using TShirt.InventoryApp.Api.Models.Xml;
using WebGrease.Css.Extensions;

namespace TShirt.InventoryApp.Api.Services
{
    public class OrderRepository : IOrderRepository 
    {

      
        DatabaseHelper context = new DatabaseHelper();
        public OrderTShirt GetOrderByCode(string code)
        {
            var _order = new OrderTShirt();
            try
            {
               
                _order = context.Orders.FirstOrDefault(a => a.Code == code);

                if (_order != null)
                {
                    _order.Details = context.OrderDetails.Where(a => a.OrderCode == code).ToList();
                }
                else
                    _order = null;

            }
            catch (Exception ex)
            {
                _order = null;
            }

            return _order;
        }

        public IEnumerable<ViewOrderExtend> GetOrdersDetails(string code)
        {         
           // return context.ViewOrders.Where(a => a.Code == code);

            var qry = "SELECT Od.Id,";
            qry += "Ord.Id AS IdOrder,";
            qry += "Ord.Code,";
            qry += "Ord.Description,";
            qry += "Ord.ProviderCode,";
            qry += "Ord.Value1,";
            qry += "Ord.Value2,";
            qry += "Pro.Name AS ProviderName,";
            qry += "Pro.Barcode AS ProviderBarcode,";
            qry += "Od.ProductCode,";
            qry += "Prod.Id AS IdProduct,";
            qry += "Prod.Description AS ProductName,";
            qry += "Prod.BarCode AS BarcodeProduct,";
            qry += "Od.Value1 AS OrderValue1,";
            qry += "Od.Value2 AS OrderValue2,";
            qry += "Od.Value3 AS OrderValue3,";
            qry += "Od.Value4 AS OrderValue4,";
            qry += "Od.Value5 AS OrderValue5,";
            qry += "Od.Quantity,";
            qry += "CASE WHEN odp.Status = 0 THEN SUM(Odp.Quantity) ELSE 0 END AS TotalProduct ";
            qry += "FROM OrderTShirt AS Ord INNER JOIN ";
            qry += "OrderDetail AS Od ON RTRIM(Ord.Code) = RTRIM(Od.OrderCode) INNER JOIN ";
            qry += "Provider AS Pro ON RTRIM(Ord.ProviderCode) = RTRIM(Pro.Code) LEFT OUTER JOIN ";
            qry +=
                "OrderDetailProduct AS Odp ON(RTRIM((Od.OrderCode) = RTRIM(Odp.OrderCode)) AND(RTRIM(Odp.ProductCode) = RTRIM(Od.ProductCode)) AND odp.Status = 0) INNER JOIN ";
            qry += "Product AS Prod ON RTRIM(Od.ProductCode) = RTRIM(Prod.Code) ";
            qry += "WHERE Ord.code = '" + code + "'";
            qry +=
                " GROUP BY Od.Id, Ord.Id, Ord.Code, Ord.Description, Ord.ProviderCode, Pro.Name, Pro.Barcode, Od.ProductCode, Prod.Id, Prod.Description, ";
            qry += "Prod.BarCode, Od.Quantity";


            var list = context.Database.SqlQuery<ViewOrderExtend>(qry).ToList();


            return list;

        }



        public IEnumerable<ViewOrderExtend> GetOrdersDetailsArray(string[] codes)
        {
            string _orders = string.Join("','", codes);

            var qry = "SELECT Od.Id,";
            qry += "Ord.Id AS IdOrder,";
            qry += "Ord.Code,";
            qry += "Ord.Description,";
            qry += "Ord.ProviderCode,";
            qry += "Ord.Value1,";
            qry += "Ord.Value2,";
            qry += "Pro.Name AS ProviderName,";
            qry += "Pro.Barcode AS ProviderBarcode,";
            qry += "Od.ProductCode,";
            qry += "Prod.Id AS IdProduct,";
            qry += "Prod.Description AS ProductName,";
            qry += "Prod.BarCode AS BarcodeProduct,";
            qry += "Od.Value1 AS OrderValue1,";
            qry += "Od.Value2 AS OrderValue2,";
            qry += "Od.Value3 AS OrderValue3,";
            qry += "Od.Value4 AS OrderValue4,";
            qry += "Od.Value5 AS OrderValue5,";
            qry += "Odp.Quantity,";
            qry += "CASE WHEN odp.Status = 0 THEN SUM(Odp.Quantity)ELSE 0 END AS TotalProduct ";
            qry += "FROM OrderTShirt AS Ord INNER JOIN ";
            qry += "OrderDetail AS Od ON RTRIM(Ord.Code) = RTRIM(Od.OrderCode) INNER JOIN ";
            qry += "Provider AS Pro ON RTRIM(Ord.ProviderCode) = RTRIM(Pro.Code) LEFT OUTER JOIN ";
            qry +=
                "OrderDetailProduct AS Odp ON(RTRIM((Od.OrderCode) = RTRIM(Odp.OrderCode)) AND(RTRIM(Odp.ProductCode) = RTRIM(Od.ProductCode)) AND odp.Status = 0 ) INNER JOIN ";
            qry += "Product AS Prod ON RTRIM(Od.ProductCode) = RTRIM(Prod.Code) ";
            qry += "WHERE Ord.code IN ('" + _orders + "') AND Odp.Quantity <> 'NULL' ";
            qry +=
                " GROUP BY Od.Id, Ord.Id, Ord.Code, Ord.Description, Ord.ProviderCode, Pro.Name, Pro.Barcode, Od.ProductCode, Prod.Id, Prod.Description, ";
            qry += "Prod.BarCode, Odp.Quantity";


            var list = context.Database.SqlQuery<ViewOrderExtend>(qry).ToList();


            return list;

        }


        //public IEnumerable<OrderDetailProduct> GetOrderDetailProductByCode(string code)
        //{
        //    return context.OrderDetailProducts.Where(a => a.OrderCode == code);
        //}


        public IEnumerable<OrderDetailProduct> GetOrderDetailProductByCode(List<OrderDetailProduct> codes)
        {
            string[] myList = codes.Select(a => a.OrderCode).ToArray();

            //return context.ViewOrders.Where(a => myList.Contains(a.Code));

            return context.OrderDetailProducts.Where(a => myList.Contains(a.OrderCode));
        }
    

        public OrderDetailProduct AddOrderDetailProduct(OrderDetailProduct detail)
        {
            try
            {
                var _detail = context.OrderDetailProducts.Add(detail);
                context.SaveChanges();
                detail.Id = _detail.Id;

                return detail;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public IEnumerable<ViewOrder> GetOrdersDetailsArray(List<string> items)
        //{
        //   // var elements = items.Where(a => (bool)a.IsSelected).Select(r=> r.Code);
        //    return context.ViewOrders.Where(a => items.Contains(a.Code));
        //}

        public IEnumerable<ViewOrderExtend> GetOrdersDetails(List<OrderTShirt> items)
        {
 //           string[] myList = items.Select(a => a.Code).ToArray();
 //           string query = string.Empty;


 //           using (TSGVLEntities db = new TSGVLEntities())
 //           {
 //               db.Database.CommandTimeout = 0;

 //               using (var conn = new SQLiteConnection(
 //                   @"Data Source=" + Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
 //                       ConfigurationManager.AppSettings["filePath"])))
 //               {
 //                   conn.Open();

 //                   var _context = new DatabaseHelper();

 //                   using (var cmd = new SQLiteCommand(conn))
 //                   {

 //                       var _qry = "Select * from OrderTshirt ";
 //                       _qry += myList.Count() > 1 ? "WHERE Code IN ('" + String.Join("','", myList) + "')" : "where Code = '" + myList[0] + "'";
 //                       var _OrderTShirt = context.Database.SqlQuery<Models.OrderTShirt>(_qry).ToList();
 //                       //List<Models.OrderTShirt> _OrderTShirt = context.Orders.ToList();


 //                       if (_OrderTShirt != null)
 //                       {

 //                           query = "DELETE FROM OrderDetail ";
 //                           query += myList.Count() > 1 ? "WHERE Ordercode IN ('" + String.Join("','", myList) + "')" : "where OrderCode = '" + myList[0] + "'";
 //                           removeData(conn, cmd, query);


 //                           foreach (Models.OrderTShirt orderTShirt in _OrderTShirt)
 //                           {

 //                               List<POP10110> _POP10110 = db.POP10110.Where(p => p.PONUMBER.Trim().Equals(orderTShirt.Code))
 //.ToList();
 //                               if (_POP10110 != null)
 //                               {
 //                                   using (var transaction = conn.BeginTransaction())
 //                                   {
 //                                       foreach (var item in _POP10110)
 //                                       {
 //                                           cmd.CommandText =
 //                                               "INSERT INTO OrderDetail (OrderCode, ProductCode, Quantity, DateCreated, OrderTShirt_id, InitQuantity) VALUES ('" +
 //                                               item.PONUMBER.Trim() + "','" +
 //                                               item.ITEMNMBR.Trim().Replace("'", "''") + "'," + item.QTYORDER.ToString().Replace(",", ".") +
 //                                               ",'" + item.REQDATE.ToShortDateString().Trim() + "'," +
 //                                               orderTShirt.Id + "," + item.QTYORDER.ToString().Replace(",", ".") + ")";
 //                                           cmd.ExecuteNonQuery();
 //                                       }

 //                                       transaction.Commit();
 //                                   }
 //                               }
 //                               else
 //                               {
 //                                   Debug.Write(string.Format("Fail load POP10110 with PONUMBER: {0}", orderTShirt.Code));
 //                               }
 //                           }
 //                       }




 //                   }
 //                   conn.Close();
 //               }
 //           }


            string[] myList = items.Select(a => a.Code).ToArray();


            var qry = "SELECT Od.Id,";
            qry += "Ord.Id AS IdOrder,";
            qry += "Ord.Code,";
            qry += "Ord.Description,";
            qry += "Ord.ProviderCode,";
            qry += "Ord.Value1,";
            qry += "Ord.Value2,";
            qry += "Pro.Name AS ProviderName,";
            qry += "Pro.Barcode AS ProviderBarcode,";
            qry += "Od.ProductCode,";
            qry += "Prod.Id AS IdProduct,";
            qry += "Prod.Description AS ProductName,";
            qry += "Prod.BarCode AS BarcodeProduct,";
            qry += "Od.Value1 AS OrderValue1,";
            qry += "Od.Value2 AS OrderValue2,";
            qry += "Od.Value3 AS OrderValue3,";
            qry += "Od.Value4 AS OrderValue4,";
            qry += "Od.Value5 AS OrderValue5,";
            qry += "Od.Quantity,";
            qry += "CASE WHEN odp.Status = 0 THEN SUM(Odp.Quantity)ELSE 0 END AS TotalProduct ";
            qry += "FROM OrderTShirt AS Ord INNER JOIN ";
            qry += "OrderDetail AS Od ON RTRIM(Ord.Code) = RTRIM(Od.OrderCode) INNER JOIN ";
            qry += "Provider AS Pro ON RTRIM(Ord.ProviderCode) = RTRIM(Pro.Code) LEFT OUTER JOIN ";
            qry +=
                "OrderDetailProduct AS Odp ON(RTRIM((Od.OrderCode) = RTRIM(Odp.OrderCode)) AND(RTRIM(Odp.ProductCode) = RTRIM(Od.ProductCode)) AND odp.Status = 0) INNER JOIN ";
            qry += "Product AS Prod ON RTRIM(Od.ProductCode) = RTRIM(Prod.Code) ";
            qry += myList.Count() > 1 ? "WHERE Ord.code IN ('" + String.Join("','", myList) + "')" : "where Ord.code = '" + myList[0] + "'"; 
            qry +=
                " GROUP BY Od.Id, Ord.Id, Ord.Code, Ord.Description, Ord.ProviderCode, Pro.Name, Pro.Barcode, Od.ProductCode, Prod.Id, Prod.Description, ";
            qry += "Prod.BarCode, Od.Quantity";


            var list = context.Database.SqlQuery<ViewOrderExtend>(qry).ToList();


            return list;

        }

        private void removeData(SQLiteConnection conn, SQLiteCommand cmd, string qry)
        {
            using (var transaction = conn.BeginTransaction())
            {
                cmd.CommandText = qry;
                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
        }

        public bool UpdateOrderDetail(string order, string product, string warehouse)
        {
            bool success = true;
            try
            {
                var existingOrderDetail = context.OrderDetails.First(a => a.OrderCode == order && a.ProductCode == product);
                if (existingOrderDetail != null)
                {
                    existingOrderDetail.Value1 = warehouse;
                    context.Entry(existingOrderDetail).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }


        public bool UpdateOrderStatus(string order, string status)
        {
            bool success = true;
            try
            {
                var existingOrder = context.Orders.First(a => a.Code == order);
                if (existingOrder != null)
                {
                    existingOrder.Value1 = status;
                    context.Entry(existingOrder).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }


        //public bool SaveOrderAndGenerateXML(string code)
        public RctExtendModel SaveOrderAndGenerateXML(RctExtendModel rct)   
        {
            bool result = true;
            int lastDetail = 0;
            XmlReception xml = new XmlReception();
            var list = new List<Detail>();
            Rct _rct = new Rct();
            try
            {
                _rct.Lot = rct.Lot;

                string[] arrayCode =  rct.Details.Select(a => a.OrderCode).Distinct().ToArray();
                
                //obtiene el Status del ultimo registro 
                var orderDetailProduct = context.OrderDetailProducts.Where(a => arrayCode.Contains(a.OrderCode) && a.Status == 0).ToList();

                if (orderDetailProduct.Count > 0)
                {
                    lastDetail = orderDetailProduct.Max(b => b.Status) + 1;
                    var getforUpdate = orderDetailProduct.Where(a => a.Status == 0);


                    foreach (var items in rct.Details)
                    {
                        var getQry = context.OrderDetails.FirstOrDefault(b => b.OrderCode == items.OrderCode && b.ProductCode == items.ProductCode);
                        if (getQry != null)
                        {
                            getQry.Quantity = (items.Quantity <= getQry.Quantity) ? (getQry.Quantity - items.Quantity) : 0;
                            getQry.Value1 = items.Warehouse;
                            context.Entry(getQry).State = EntityState.Modified;
                        }
                    }

                    context.SaveChanges();


                    foreach (var detail in arrayCode)
                    {

                        string _status = "Abierta";

                        var statusOrder = context.OrderDetails.Any(a => a.OrderCode == detail && a.Quantity > 1);
                        if (!statusOrder)
                        {
                            _status = "Cerrada";
                            //bool updateOrder = UpdateOrderStatus(detail, "1");
                        }

                        list.Add(new Detail() {OrderCode = detail, Status = _status});


                        var orders = context.Orders.FirstOrDefault(a => a.Code == detail);
                        orders.Value1 = (_status == "Cerrada") ? "1" : "0";
                        orders.Value2 = rct.Lot;

                        context.Entry(orders).State = EntityState.Modified;
                        context.SaveChanges();


                    }



                    string createXML = xml.Create(arrayCode); //Crea XML

                    if (createXML != "ERROR")
                    {
                        var qry = "UPDATE ";
                        qry += " OrderDetailProduct";
                        qry += " SET";
                        qry += " Status = '1'";
                        qry += " WHERE OrderCode IN ('" + String.Join("','", arrayCode) + "') AND Status ='0'";
                        var list2 = context.Database.ExecuteSqlCommand(qry); //.SqlQuery<OrderDetailProduct>(qry);


                        rct.Code = createXML;
                        rct.DateCreated = DateTime.Now.ToString("dd/MM/yyyy");
                        rct.Details = list;
                    }
                    else
                        rct = null;

                }
            }
            catch (Exception ex)
            {
                rct.Code = ex.Message;
            }
            return rct;
        }


    }
}