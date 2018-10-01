using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public class DocumentRepository : IDocumentRepository
    {
        DatabaseHelper context = new DatabaseHelper();


        public Document GetById(int id)
        {
            var document = new Document();
            try
            {
                document = context.Documents.FirstOrDefault(a => a.Id == id);

                var _details = (from dt in context.DocumentDetails
                    join pro in context.Products on dt.ProductCode equals pro.Code
                    where dt.Id == id
                    select new DocumentDetail()
                    {
                        Id = dt.Id,
                        DocumentId = dt.DocumentId,
                        ProductCode = dt.ProductCode,
                        Quantity = dt.Quantity,
                        Value1 = dt.Value1,
                        Value2 = dt.Value2,
                        Value3 = dt.Value3,
                        Value4 = dt.Value4,
                        Value5 = dt.Value5,
                        ProductName = pro.Description
                    }).ToList();

                document.Details = _details;
                

            }
            catch (Exception ex)
            {
                document = null;
                Debug.Write(@"Error " + ex.Message);
            }
            return document;
        }

        public List<Document> GetAll()
        {
            var list = new List<Document>();
            try
            {
                list.AddRange(context.Documents.ToList().Select(row => GetById(row.Id)));
            }
            catch (Exception ex)
            {
                list = null;
                Debug.Write(@"Error " + ex.Message);
            }
            return list;
        }

        public int Save(Document items)
        {
            int result = 0;
            try
            {
                var document = new Document
                {
                    ProcessType = items.ProcessType,
                    Lot = items.Lot,
                    Code = items.Code,
                    DateCreated = items.DateCreated,
                    WarehouseO = items.WarehouseO,
                    WarehouseD = items.WarehouseD,
                    Value1 = items.Value1,
                    Value2 = items.Value2,
                    Value3 = items.Value3
                };

                context.Documents.Add(document);
                context.SaveChanges();

                result = document.Id;

                if (result > 0)
                {
                    var detail = new DocumentDetail();
                    foreach (var row in items.Details)
                    {
                        detail.DocumentId = result;
                        detail.ProductCode = row.ProductCode;
                        detail.Quantity = row.Quantity;
                        detail.Value1 = row.Value1;
                        detail.Value2 = row.Value2;
                        detail.Value3 = row.Value3;
                        detail.Value4 = row.Value4;
                        detail.Value5 = row.Value5;

                        context.DocumentDetails.Add(detail);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                if (result > 0)
                {
                    var x = context.Documents.FirstOrDefault(a => a.Id == result);
                    context.Documents.Remove(x);
                    context.SaveChanges();
                    Debug.Write(@"Error " + "Delete Document " + result.ToString());

                    var y = context.DocumentDetails.FirstOrDefault(a => a.DocumentId == result);
                    context.DocumentDetails.Remove(y);
                    context.SaveChanges();
                    Debug.Write(@"Error " + "Delete Document Details " + result.ToString());                  
                }
                Debug.Write(@"Error " + ex.Message);
                result = 0;

            }
            return result;
        }

    }
}