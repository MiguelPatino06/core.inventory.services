using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
  public class ProductRepository : IProductRepository
  {
    DatabaseHelper context = new DatabaseHelper();

      public Product GetByCode(string code)
      {
          //return context.Products.FirstOrDefault(a => a.Code == code);
          Product prod = new Product();
          using (TSGVLEntities db = new TSGVLEntities())
          {
              var p = db.IV00101.FirstOrDefault(a => a.ITEMNMBR.Trim() == code);
              if (p != null)
              {
                  prod.Id = 0;
                  prod.Code = p.ITEMNMBR.Trim();
                  prod.Description = p.ITEMDESC.Trim();
              }
          }
          return prod;
      }

      public List<Product> Search(string code)
      {
          //return context.Products.Where(a => a.Code.ToLower().Contains(code.ToLower()) || a.Description.ToLower().Contains(code.ToLower())).ToList();
          using (TSGVLEntities db = new TSGVLEntities())
          {
              return db.IV00101.Where(a => a.ITEMNMBR.Trim().ToLower().Contains(code.ToLower()) || a.ITEMDESC.Trim().ToLower().Contains(code.ToLower())).Select(a => new Product()
              {
                  Id = 0,
                  Code = a.ITEMNMBR.Trim(),
                  Description = a.ITEMDESC.Trim(),
              }).ToList();
          }

      }
  }
}