using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models;


namespace TShirt.InventoryApp.Api.Services
{
    public class WarehouseProductRepository : IWarehouseProductRepository
    {


        private DatabaseHelper context = new DatabaseHelper();

        public WarehouseProduct Add(WarehouseProduct warehouseProduct)
        {
            var addWarehouse = context.WarehouseProduct.Add(warehouseProduct);
            context.SaveChanges();
            return warehouseProduct;
        }

        public IEnumerable<WarehouseProduct> GetAll()
        {
            return context.WarehouseProduct.ToList();
        }

        public WarehouseProduct GetByCodes(string warehouseCode, string productCode)
        {
            try
            {
                WarehouseProduct _warehouseProduct = null;
                using (TSGVLEntities db = new TSGVLEntities())
                {
                    //var _result = context.WarehouseProduct.Join(context.Warehouses, wp => wp.WarehouseCode, w => w.Code,
                    //    (wp, w) => new {wp, w})
                    //    .Join(context.Products, x => x.wp.ProductCode, p => p.Code, (x, p) => new {x, p})
                    //    .Where(cond => cond.x.wp.Quantity > 0)
                    //    .FirstOrDefault(
                    //        result => result.x.wp.WarehouseCode == warehouseCode && result.p.Code.Equals(productCode));



                    //if (_result != null)
                    //{
                    //    _warehouseProduct = new WarehouseProduct()
                    //    {
                    //        Id = _result.x.wp.Id,
                    //        WarehouseCode = _result.x.wp.WarehouseCode,
                    //        ProductCode = _result.x.wp.ProductCode,
                    //        Quantity = _result.x.wp.Quantity,
                    //        Warehouse = _result.x.w,
                    //        Product = _result.p
                    //    };
                    //}



                    _warehouseProduct = (from wp in db.IV00102
                        join w in db.IV40700 on wp.LOCNCODE.Trim() equals w.LOCNCODE.Trim()
                        join p in db.IV00101 on wp.ITEMNMBR.Trim() equals p.ITEMNMBR.Trim()
                        where
                            wp.ITEMNMBR.Trim() == productCode && wp.LOCNCODE.Trim() == warehouseCode &&
                            (wp.QTYONHND - wp.ATYALLOC) > 0
                        select new WarehouseProduct
                        {
                            Id = 0,
                            WarehouseCode = wp.LOCNCODE.Trim(),
                            ProductCode = wp.ITEMNMBR.Trim(),
                            Quantity = wp.QTYONHND - wp.ATYALLOC,
                            Warehouse = new Warehouse
                            {
                                Id = 0,
                                Code = w.LOCNCODE.Trim(),
                                Name = w.LOCNDSCR.Trim()
                            },
                            Product = new Product
                            {
                                Id = 0,
                                Code = p.ITEMNMBR.Trim(),
                                BarCode = p.ITEMNMBR.Trim(),
                                Description = p.ITEMDESC.Trim()
                            }
                        }).FirstOrDefault();

                }

                return _warehouseProduct;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("e " + e.Message);
                return null;
            }
        }

        public IEnumerable<WarehouseProduct> GetListByString(string warehouseCode, string str)
        {
            List<WarehouseProduct> result = new List<WarehouseProduct>();

            try
            {
                str = str.ToUpper();
                using (TSGVLEntities db = new TSGVLEntities())
                {

                    //context.Database.CommandTimeout = 0;

                    //var _result = context.WarehouseProduct.Where(wp => wp.WarehouseCode.Equals(warehouseCode) && wp.Quantity > 0)
                    //              .ToList()
                    //              .Join(context.Products, wp => wp.ProductCode, p => p.Code, (wp, p) => new { wp, p })
                    //              .ToList()
                    //              .Where(f => f.p.Code.ToUpper().Contains(str) || f.p.Description.ToUpper().Contains(str))
                    //              .ToList();

                    //if (_result != null)
                    //{
                    //  result = _result.Select(r => new WarehouseProduct()
                    //  {
                    //    Id = r.wp.Id,
                    //    WarehouseCode = r.wp.WarehouseCode,
                    //    ProductCode = r.wp.ProductCode,
                    //    Quantity = r.wp.Quantity,
                    //    Product = r.p
                    //  })
                    //  .ToList();
                    //}

                    result = (from wp in db.IV00102
                        join w in db.IV40700 on wp.LOCNCODE.Trim() equals w.LOCNCODE.Trim()
                        join p in db.IV00101 on wp.ITEMNMBR.Trim() equals p.ITEMNMBR.Trim()
                        where
                            (p.ITEMNMBR.Trim().ToUpper().Contains(str) || p.ITEMDESC.Trim().ToUpper().Contains(str)) &&
                            wp.LOCNCODE.Trim() == warehouseCode
                        select new WarehouseProduct
                        {
                            Id = 0,
                            WarehouseCode = wp.LOCNCODE.Trim(),
                            ProductCode = wp.ITEMNMBR.Trim(),
                            Quantity = wp.QTYONHND - wp.ATYALLOC,
                            Warehouse = new Warehouse
                            {
                                Id = 0,
                                Code = w.LOCNCODE.Trim(),
                                Name = w.LOCNDSCR.Trim()
                            },
                            Product = new Product
                            {
                                Id = 0,
                                Code = p.ITEMNMBR.Trim(),
                                BarCode = p.ITEMNMBR.Trim(),
                                Description = p.ITEMDESC.Trim()
                            }
                        }).ToList();
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("e " + e.Message);
                return null;
            }

            return result;
        }

        public bool UpdateAmountByCodes(string warehouseOrigin, string warehouseDestiny, long quantity, string productCode)
        {
            try
            {

                long amount = 0;

                //Add Amount
                WarehouseProduct destiny = this.GetByCodes(warehouseDestiny, productCode);

                if (destiny != null)
                {
                    destiny.Quantity += quantity;
                }
                else
                {
                    destiny = new WarehouseProduct()
                    {
                        ProductCode = productCode,
                        WarehouseCode = warehouseDestiny,
                        Quantity = quantity
                    };
                }

                context.WarehouseProduct.AddOrUpdate(destiny);

                //Subtract Amount
                WarehouseProduct origin = this.GetByCodes(warehouseOrigin, productCode);
                origin.Quantity -= quantity;

                context.WarehouseProduct.AddOrUpdate(origin);

                context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpdateAmountByCode(string warehouse, long quantity, string productCode)
        {
            try
            {
                //Subtract Amount
                WarehouseProduct origin = this.GetByCodes(warehouse, productCode);
                origin.Quantity -= quantity;

                context.WarehouseProduct.AddOrUpdate(origin);

                context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


    }
}