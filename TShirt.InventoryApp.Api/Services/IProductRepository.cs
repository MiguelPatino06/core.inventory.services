using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
  public interface IProductRepository
  {
      Product GetByCode(string code);
      List<Product> Search(string code);
  }
}