using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models;
using TShirt.InventoryApp.Api.Models.Xml;

namespace TShirt.InventoryApp.Api.Services
{
    public class ProductTransferRepository : IProductTransferRepository
    {
        private DatabaseHelper context = new DatabaseHelper();


        public ProductTransfer Save(ProductTransfer productTransfer)
        {
            XmlWarehouseTranfer createXml = new XmlWarehouseTranfer();
            int idTransferencia = 0;
            try
            {

                var _productTransfer = context.ProductTransfer.Add(productTransfer);
                context.SaveChanges();
                productTransfer.Id = _productTransfer.Id;
                idTransferencia = _productTransfer.Id;

                foreach (ProductTransferDetail detail in productTransfer.products)
                {
                    detail.ProductTransfer_Id = _productTransfer.Id;
                    var _id = context.ProductTransferDetail.Add(detail);
                }

                context.SaveChanges();

                //Update Amounts
                IWarehouseProductRepository _warehouseProductRepository = new WarehouseProductRepository();

                foreach (ProductTransferDetail detail in productTransfer.products)
                {
                    _warehouseProductRepository.UpdateAmountByCodes(productTransfer.WarehouseOrigin,
                        productTransfer.WarehouseDestiny, detail.Quantity, detail.ProductCode);
                }


                //save XML
                string doc = createXml.Create(idTransferencia);

                return productTransfer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public TransferDetail GetById(int id)
        {
            var transfer = new TransferDetail();
            try
            {

                transfer = (from pt in context.ProductTransfer
                            join wo in context.Warehouses on pt.WarehouseOrigin equals wo.Code
                            join wd in context.Warehouses on pt.WarehouseDestiny equals wd.Code
                            where pt.Id == id
                            select new TransferDetail()
                            {
                                Id = pt.Id,
                                WarehouseOrigin = wo.Code,
                                WarehouseDestiny = wd.Code,
                                Status = pt.Status,
                                DateCreated = pt.DateCreated.ToString(),
                                Observation = pt.Observation
                            }).FirstOrDefault();

                if (transfer != null)
                {
                    var details = (from dt in context.ProductTransferDetail
                                   where dt.ProductTransfer_Id == id
                                   select new ProductTransferDetailExtend
                                   {
                                       Quantity = dt.Quantity,
                                       ProductCode = dt.ProductCode,
                                       ProductDescription = dt.ProductDescription
                                   }).ToList();

                    if (details != null)
                    {
                        transfer.products = details;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.Write(@"Error " + ex.Message);
            }

            return transfer;
        }

        public IEnumerable<TransferDetail> GetRequests()
        {
            var request = (from pt in context.ProductTransfer.AsEnumerable()
                join wo in context.Warehouses on pt.WarehouseOrigin equals wo.Code
                join wd in context.Warehouses on pt.WarehouseDestiny equals wd.Code
                select new TransferDetail
                {
                    Id = pt.Id,
                    WarehouseOrigin = wo.Code,
                    WarehouseDestiny = wd.Code,
                    DateCreated = pt.DateCreated,
                    Status = pt.Status,
                    Observation = pt.Observation
                }).ToList();
            return request;
        }

        public IEnumerable<TransferDetail> GetRequests(int code)
        {
            XmlWarehouseTranfer createXml = new XmlWarehouseTranfer();
            string doc = createXml.Create(code);
            return null;
            //var request = GetRequests().Where(t => t.Id == code).ToList();
            //return request;
        }
    }
}