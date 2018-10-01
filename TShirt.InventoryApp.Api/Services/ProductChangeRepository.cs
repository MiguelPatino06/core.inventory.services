using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models;
using TShirt.InventoryApp.Api.Models.Xml;

namespace TShirt.InventoryApp.Api.Services
{
    public class ProductChangeRepository : IProductChange
    {

        DatabaseHelper context = new DatabaseHelper();

        public OrderReqExtend GetOrderByCode(string code)
        {
            var order = new OrderReqExtend();
            try
            {
                order = (from or in context.OrderReqs
                    where or.Code == code
                    select new OrderReqExtend()
                    {
                        Id = or.Id,
                        Code = or.Code,
                        Description = or.Description,
                        Status = or.Status,
                        ClientCode = or.ClientCode,
                        DateCreated = or.DateCreated,
                        Value1 = or.Value1,
                        Value2 = or.Value2,
                        Value3 = or.Value3,
                        Value4 = or.Value4,
                        Value5 = or.Value5,
                        Observation = or.Observation,
                        ClientName = or.ClientCode
                    }).FirstOrDefault();

                if (order == null) //si no se encuentra en la bd intermedia busca en GP
                {
                    using (TSGVLEntities db = new TSGVLEntities())
                    {

                        var _sop10100 = db.SOP10100.FirstOrDefault(a => a.SOPNUMBE.Trim().Equals(code));

                        if (_sop10100 == null)
                        {
                            var _sop30200 = db.SOP30200.FirstOrDefault(a => a.SOPNUMBE.Trim() == code);
                            order = new OrderReqExtend
                            {
                                Id = 1,
                                ClientCode = _sop30200.CUSTNAME.Trim().Replace("'", "''"),
                                Code = _sop30200.SOPNUMBE.Trim(),
                                ClientName = _sop30200.CUSTNAME.Trim().Replace("'", "''"),
                                DateCreated = _sop30200.DOCDATE.ToString("dd/MM/yyyy"),
                                Status = _sop30200.SOPSTATUS.ToString()
                            };
                        }
                        else
                        {
                            order = new OrderReqExtend
                            {
                                Id = 1,
                                ClientCode = _sop10100.CUSTNAME.Trim().Replace("'", "''"),
                                Code = _sop10100.SOPNUMBE.Trim(),
                                ClientName = _sop10100.CUSTNAME.Trim().Replace("'", "''"),
                                DateCreated = _sop10100.DOCDATE.ToString("dd/MM/yyyy"),
                                Status = _sop10100.SOPSTATUS.ToString()
                            };
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.Write("Error " + ex.InnerException.Message);
                return null;
            }
            return order;
        }

        public List<OrderReqExtend> GetAll()
        {
            var order = new List<OrderReqExtend>();
            try
            {
                order = (from or in context.OrderReqs
                         join ord in context.OrderReqDetailProducts on or.Code.Trim() equals ord.OrderReqCode.Trim()
                         where ord.ProductCodeChanged != null
                         select new OrderReqExtend() 
                         {
                             Id = or.Id,
                             Code = or.Code,
                             Description = or.Description,
                             Status = or.Status,
                             ClientCode = or.ClientCode,
                             DateCreated = or.DateCreated,
                             Value1 = or.Value1,
                             Value2 = or.Value2,
                             Value3 = or.Value3,
                             Value4 = or.Value4,
                             Value5 = or.Value5,
                             Observation = or.Observation,
                             ClientName = string.Empty
                         }).ToList();
            }
            catch (Exception ex)
            {
                order = null;
                Debug.Write("Error " + ex.InnerException.Message);
            }
            return order;
        }


        public OrderReqExtend GetDetailByCode(string code)
        {
            try
            {
                var order = new OrderReqExtend();
             
                List<OrderReqDetailExtend> list = new List<OrderReqDetailExtend>();
                List<OrderReqDetail> detailInsert = new List<OrderReqDetail>();
                using (TSGVLEntities db = new TSGVLEntities())
                {

                    var anyOrder = context.OrderReqs.Any(a => a.Code == code);
                    if(!anyOrder)
                    {
                        var master = db.SOP10100.FirstOrDefault(p => p.SOPNUMBE.Trim().Equals(code));

                        if (master != null)
                        {
                            var _master = new OrderReq()
                            {
                                Code = master.SOPNUMBE.Trim(),
                                ClientCode = master.CUSTNAME.Trim().Replace("'", "''"),
                                DateCreated = master.DOCDATE.ToString("dd/MM/yyyy"),
                                Status = master.SOPSTATUS.ToString(),

                            };

                            context.OrderReqs.Add(_master);
                            context.SaveChanges();

                            order.Id = _master.Id;
                            order.Code = _master.Code;
                            order.DateCreated = _master.DateCreated;
                            order.Status = _master.Status;
                            order.ClientCode = _master.ClientCode;
                            order.ClientName = _master.ClientCode;

                            var detail = db.SOP10200.Where(p => p.SOPNUMBE.Trim().Equals(code)).ToList();
                            if (detail.Any())
                            {
                                foreach (var row in detail)
                                {
                                    var _detail = new OrderReqDetail()
                                    {
                                        OrderReqCode = row.SOPNUMBE.Trim(),
                                        Observation = row.ITEMDESC.Trim(),
                                        ProductCode = row.ITEMNMBR.Trim().Replace("'", "''"),
                                        Quantity = Convert.ToInt32(row.QUANTITY),
                                        Warehouse = row.LOCNCODE.Trim()
                                    };
                                    detailInsert.Add(_detail);

                                    context.OrderReqDetails.Add(_detail);
                                    context.SaveChanges();
                                }

                                list.AddRange(detailInsert.Select(items => new OrderReqDetailExtend()
                                {
                                    OrderReqCode = items.OrderReqCode,
                                    Observation = string.Empty,
                                    ProductCode = items.ProductCode,
                                    ProductCodeChanged = string.Empty,
                                    Quantity = items.Quantity,
                                    DateProductChanged = string.Empty,
                                    UserUpdated = string.Empty,
                                    ProductName = items.Observation,
                                    ProductNameChanged = string.Empty,
                                    QuantityChanged = 0,
                                    Warehouse = items.Warehouse
                                }));
                                order.Detail = list;
                            }
                        }
                        else
                        {
                            var master30200= db.SOP30200.FirstOrDefault(p => p.SOPNUMBE.Trim().Equals(code));
                            var _master2 = new OrderReq()
                            {
                                Code = master30200.SOPNUMBE.Trim(),
                                ClientCode = master30200.CUSTNAME.Trim().Replace("'", "''"),
                                DateCreated = master30200.DOCDATE.ToString("dd/MM/yyyy"),
                                Status = master30200.SOPSTATUS.ToString(),

                            };

                            context.OrderReqs.Add(_master2);
                            context.SaveChanges();

                            order.Id = _master2.Id;
                            order.Code = _master2.Code;
                            order.DateCreated = _master2.DateCreated;
                            order.Status = _master2.Status;
                            order.ClientCode = _master2.ClientCode;
                            order.ClientName = _master2.ClientCode;

                            var detail = db.SOP30300.Where(p => p.SOPNUMBE.Trim().Equals(code)).ToList();
                            if (detail.Any())
                            {
                                foreach (var row in detail)
                                {
                                    var _detail = new OrderReqDetail()
                                    {
                                        OrderReqCode = row.SOPNUMBE.Trim(),
                                        Observation = row.ITEMDESC.Trim(),
                                        ProductCode = row.ITEMNMBR.Trim().Replace("'", "''"),
                                        Quantity = Convert.ToInt32(row.QUANTITY),
                                        Warehouse = row.LOCNCODE.Trim()
                                    };
                                    detailInsert.Add(_detail);

                                    context.OrderReqDetails.Add(_detail);
                                    context.SaveChanges();
                                }

                                list.AddRange(detailInsert.Select(items => new OrderReqDetailExtend()
                                {
                                    OrderReqCode = items.OrderReqCode,
                                    Observation = string.Empty,
                                    ProductCode = items.ProductCode,
                                    ProductCodeChanged = string.Empty,
                                    Quantity = items.Quantity,
                                    DateProductChanged = string.Empty,
                                    UserUpdated = string.Empty,
                                    ProductName = items.Observation,
                                    ProductNameChanged = string.Empty,
                                    QuantityChanged = 0,
                                    Warehouse = items.Warehouse
                                }));
                                order.Detail = list;
                            }
                        }                     
                    }
                    else
                    {
                        order = GetOrderByCode(code);

                        var qyDetail = (from or in context.OrderReqDetails
                                      join orp in context.OrderReqDetailProducts on new { or.OrderReqCode, or.ProductCode } equals new { orp.OrderReqCode, orp.ProductCode }
                                      into rel
                                      from relleft in rel.DefaultIfEmpty()
                                      where or.OrderReqCode.Trim() == code
                                      select new OrderReqDetailExtend
                                      {
                                          Id = or.Id,
                                          OrderReqCode = or.OrderReqCode.Trim(),
                                          Observation = or.Observation,
                                          ProductCode = or.ProductCode,
                                          ProductName = or.Observation,
                                          ProductCodeChanged = relleft.ProductCodeChanged,
                                          Quantity = or.Quantity,
                                          DateProductChanged = relleft.DateProductChanged,
                                          UserUpdated = relleft.UserUpdated,
                                          QuantityChanged = relleft.Quantity == null ? 0 : relleft.Quantity,
                                          Warehouse = or.Warehouse
                                      }).ToList();

                        list.AddRange(qyDetail.Select(item => new OrderReqDetailExtend
                        {
                            Id = item.Id,
                            OrderReqCode = item.OrderReqCode.Trim(),
                            Observation = item.Observation,
                            ProductCode = item.ProductCode,
                            ProductCodeChanged = item.ProductCodeChanged,
                            Quantity = item.Quantity,
                            DateProductChanged = item.DateProductChanged,
                            UserUpdated = item.UserUpdated,
                            ProductName = item.ProductName, //db.IV00101.FirstOrDefault(a => a.ITEMNMBR.Trim() == item.ProductCode).ITEMDESC.Trim(),
                            ProductNameChanged = (item.ProductCodeChanged != null) ? db.IV00101.FirstOrDefault(a => a.ITEMNMBR.Trim() == item.ProductCodeChanged.ToString()).ITEMDESC.Trim() : string.Empty,
                            QuantityChanged = item.QuantityChanged,
                            Warehouse = item.Warehouse
                        }));

                        order.Detail = list;
                    }


                }
             
                return order;
            }
            catch (Exception ex)
            {
                return null;
                Debug.Write("Error " + ex.InnerException.Message);
            }

        }

        public List<OrderReqDetailExtend> GetAllDetail()
        {
            List<OrderReqDetailExtend> list = new List<OrderReqDetailExtend>();
            try
            {
                using (TSGVLEntities db = new TSGVLEntities())
                {
                    //var detail = (from or in context.OrderReqDetails
                    //    where or.ProductCodeChanged != null
                    //    select or).ToList();


                    var detail = (from or in context.OrderReqDetails
                                  join orp in context.OrderReqDetailProducts on  or.OrderReqCode  equals orp.OrderReqCode
                                  select new OrderReqDetailExtend
                                  {
                                      Id = or.Id,
                                      OrderReqCode = or.OrderReqCode.Trim(),
                                      Observation = or.Observation,
                                      ProductCode = or.ProductCode,
                                      ProductCodeChanged = orp.ProductCodeChanged,
                                      Quantity = or.Quantity,
                                      DateProductChanged = orp.DateProductChanged,
                                      UserUpdated = orp.UserUpdated,
                                      //ProductName = db.IV00101.FirstOrDefault(a => a.ITEMNMBR.Trim() == item.ProductCode).ITEMDESC.Trim(),
                                      //ProductNameChanged = (item.ProductCodeChanged != null) ? db.IV00101.FirstOrDefault(a => a.ITEMNMBR.Trim() == item.ProductCodeChanged.ToString()).ITEMDESC.Trim() : string.Empty,
                                      QuantityChanged = orp.Quantity == null ? 0 : orp.Quantity


                                  }).ToList();

                    list.AddRange(detail.Select(item =>
                    {
                        var productName = db.IV00101.FirstOrDefault(a => a.ITEMNMBR.Trim() == item.ProductCode);
                        var productNameChanged = db.IV00101.FirstOrDefault(a => a.ITEMNMBR.Trim() == item.ProductCodeChanged);
                        return new OrderReqDetailExtend
                                                            {
                                                                Id = item.Id,
                                                                OrderReqCode = item.OrderReqCode.Trim(),
                                                                Observation = item.Observation,
                                                                ProductCode = item.ProductCode,
                                                                ProductCodeChanged = item.ProductCodeChanged,
                                                                Quantity = item.Quantity,
                                                                DateProductChanged = item.DateProductChanged,
                                                                UserUpdated = item.UserUpdated,
                                                                ProductName = (productName!= null)? productName.ITEMDESC.Trim(): string.Empty,
                                                                ProductNameChanged = (productNameChanged != null)? productNameChanged.ITEMDESC.Trim() : string.Empty
                        };
                    }));
                }
            }
            catch (Exception ex)
            {
                list = null;
                Debug.Write("Error " + ex.InnerException.Message);
            }
            return list;
        }

        public bool UpdateOrder(OrderReqExtend items)
        {
            bool result = true;
            try
            {
                if (items != null)
                {
                    var getforUpdate = context.OrderReqs.FirstOrDefault(a => a.Code == items.Code);

                    if (getforUpdate != null)
                    {
                        getforUpdate.Status = items.Status;
                        getforUpdate.Observation = items.Observation;
                        context.Entry(getforUpdate).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                else
                    result = false;

            }
            catch (Exception ex)
            {
                result = false;
                Debug.Write("Error " + ex.Message);
               
            }
            return result;
        }

        public bool UpdateDetail(OrderReqDetailExtend items)
        {
            bool result = false;
            try
            {
                var getforUpdate = context.OrderReqDetailProducts.FirstOrDefault(a => a.OrderReqCode == items.OrderReqCode && a.ProductCode == items.ProductCode && a.Status == 0);

                int quantity = getforUpdate?.Quantity ?? items.Quantity;

                if (getforUpdate == null)
                {
                    OrderReqDetailProduct orderReq = new OrderReqDetailProduct
                    {
                        OrderReqCode = items.OrderReqCode,
                        ProductCode = items.ProductCode,
                        ProductCodeChanged = items.ProductCodeChanged,
                        Quantity = quantity, // items.Quantity,
                        DateProductChanged = DateTime.Now.ToString("dd/MM/yyyy hh:mm"),
                        UserUpdated = "Miguel Patiño",
                        Warehouse = items.Warehouse
                    };

                    context.OrderReqDetailProducts.Add(orderReq);
                    context.SaveChanges();
                }
                else
                {

                    getforUpdate.ProductCodeChanged = items.ProductCodeChanged;
                    getforUpdate.Quantity = quantity;
                    getforUpdate.DateProductChanged = DateTime.Now.ToString("dd/MM/yyyy hh:mm");
                    getforUpdate.UserUpdated = "Miguel Patiño";

                    context.Entry(getforUpdate).State = EntityState.Modified;
                    context.SaveChanges();


                }

                result = true;

            }
            catch (Exception ex)
            {
                result = false;
                Debug.Write("Error " + ex.Message);

            }
            return result;
        }

        public bool Save(OrderReqExtend items)
        {
            XmlChange xml = new XmlChange();
            var order = new OrderReqExtend();
            var detail = new OrderReqDetailExtend();
            bool isUpdated = true;
            try
            {


                //delete Order
                OrderReq ordertoDelete = context.OrderReqs.FirstOrDefault(a => a.Code == items.Code);
                context.Entry(ordertoDelete).State = EntityState.Deleted;
                context.SaveChanges();


                //delete OrderDetail
                List<OrderReqDetail> listOrderDetailToDelete = context.OrderReqDetails.Where(a => a.OrderReqCode == items.Code).ToList();
                context.OrderReqDetails.RemoveRange(listOrderDetailToDelete);
                context.SaveChanges();

                //Update status OrderReqDetailProduct
                var detailforUpdate = context.OrderReqDetailProducts.Where(a => a.OrderReqCode == items.Code && a.Status == 0).ToList();
                detailforUpdate.ForEach(a =>
                {
                    a.Status = 1;
                });
                context.SaveChanges();

                bool resultCreatexml = xml.Create(items.Code); //Crea XML

                //if (result)
                //{
                //    var detailforUpdate = context.OrderReqDetailProducts.Where(a=> a.OrderReqCode == items.Code && a.Status == 0).ToList();
                //    detailforUpdate.ForEach(a =>
                //    {
                //        a.Status = 1;
                //    });


                //    foreach (OrderReqDetailProduct row in detailforUpdate)
                //    {
                //        row.Status = 1;
                //        context.Entry(row).State = EntityState.Modified;
                //        context.SaveChanges();
                //    }

                //   // context.SaveChanges();

                //    bool resultCreatexml = xml.Create(items.Code); //Crea XML
                //}

            }
            catch (Exception)
            {
                isUpdated = false;
                throw;
            }
            return isUpdated;
        }

        public List<OrderReqDetailProduct> GetOrderRedDetailProducts(string code)
        {
            List<OrderReqDetailProduct> list = new List<OrderReqDetailProduct>();
            try
            {
                list = context.OrderReqDetailProducts.Where(a => a.OrderReqCode == code && a.Status == 0).ToList();
            }
            catch (Exception)
            {
                list = null;
                throw;
            }
            return list;
        }


        public List<OrderReqDetailProduct> GetOrderRedDetailProducts2(string code)
        {
            List<OrderReqDetailProduct> list = new List<OrderReqDetailProduct>();
            try
            {
                list = context.OrderReqDetailProducts.Where(a => a.OrderReqCode == code && a.Status == 1).ToList();
            }
            catch (Exception)
            {
                list = null;
                throw;
            }
            return list;
        }

        public List<OrderReqDetailExtend> GetOrderRedDetailExtendProduct(string code)
        {
            List<OrderReqDetailExtend> list = new List<OrderReqDetailExtend>(); 
            try
            {
                var details = context.OrderReqDetailProducts.Where(a => a.OrderReqCode == code).ToList();

                using (TSGVLEntities db = new TSGVLEntities())
                {
                    foreach (var rows in details)
                    {
                        list.Add(new OrderReqDetailExtend
                        {
                            Id = rows.Id,
                            OrderReqCode = rows.OrderReqCode,
                            Observation = string.Empty,
                            ProductCode = rows.ProductCode,
                            ProductCodeChanged = rows.ProductCodeChanged,
                            Quantity = rows.Quantity,
                            DateProductChanged = rows.DateProductChanged,
                            UserUpdated = rows.UserUpdated,
                            ProductName = db.IV00101.FirstOrDefault(a => a.ITEMNMBR.Trim() == rows.ProductCode)
                                ?.ITEMDESC
                                .Trim(),
                            ProductNameChanged = db.IV00101
                                .FirstOrDefault(a => a.ITEMNMBR.Trim() == rows.ProductCodeChanged)
                                ?.ITEMDESC.Trim(),
                            QuantityChanged = rows.Quantity,
                            Warehouse = rows.Warehouse
                        });
                    }
                }
                return list;
            }
            catch (Exception )
            {
                return null;
                throw;
            }
        }

    }
}