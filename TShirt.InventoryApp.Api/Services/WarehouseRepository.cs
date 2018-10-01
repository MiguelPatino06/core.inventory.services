using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
  public class WarehouseRepository : IWarehouseRepository
  {

    DatabaseHelper context = new DatabaseHelper();

    public Warehouse Add(Warehouse warehouse)
    {
      var addWarehouse = context.Warehouses.Add(warehouse);
      context.SaveChanges();
      warehouse.Id = addWarehouse.Id;

      return warehouse;
    }

    public IEnumerable<Warehouse> GetAll()
    {
      return context.Warehouses.OrderBy(a=> a.Code).ToList();
    }

    public Warehouse GetById(int id)
    {
      try
      {
        return context.Warehouses.FirstOrDefault(a => a.Id == id);
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
        Warehouse warehouse = context.Warehouses.Find(id);
        context.Warehouses.Remove(warehouse);
        context.SaveChanges();
        success = true;
      }
      catch (Exception)
      {
        success = false;
      }
      return success;
    }

    public bool Update(Warehouse warehouse)
    {
      bool success = true;
      try
      {
        var existingWarehouse = context.Warehouses.First(a => a.Id == warehouse.Id);
        if (existingWarehouse != null)
          existingWarehouse.Name = warehouse.Name;

        context.Entry(existingWarehouse).State = EntityState.Modified;
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