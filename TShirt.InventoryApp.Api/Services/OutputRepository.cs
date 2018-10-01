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
  public class OutputRepository : IOutputRepository
  {
    DatabaseHelper context = new DatabaseHelper();
    //

    public Output GetById(int id)
    {
      Output output = null;

      try
      {
        output = context.Outputs.FirstOrDefault(x => x.Id == id);

        if (output != null)
        {
          var _details = (from od in context.OutputDetails
                          join p in context.Products on od.ProductCode equals p.Code
                          join w in context.Warehouses on od.Warehouse equals w.Code
                          where od.OutputId == id
                          select new
                          {
                            Id = od.Id,
                            ProductCode = od.ProductCode,
                            Quantity = od.Quantity,
                            OutputId = od.OutputId,
                            ProductDescription = p.Description,
                            Warehouse = w.Code,
                            WarehouseName = w.Name
                          }).ToList();


          if (_details != null)
          {
            List<OutputDetail> details = _details.Select(x => new OutputDetail
            {
              Id = x.Id,
              ProductCode = x.ProductCode,
              OutputId = x.OutputId,
              ProductDescription = x.ProductDescription,
              Quantity = x.Quantity,
              Warehouse = x.Warehouse,
              WarehouseName = x.WarehouseName
            }).ToList();

            output.Details = details;
          }
        }
      }
      catch (Exception ex)
      {
        output = null;
        Debug.Write(@"Error " + ex.Message);
      }

      return output;

    }

      public int Save(Output output)
      {
          int id = 0;
          XmlOutput xmlOutput = new XmlOutput();
          try
          {


              context.Outputs.Add(output);
              context.SaveChanges();

              id = output.Id;

              if (id > 0)
              {
                  foreach (OutputDetail detail in output.Details)
                  {
                      detail.OutputId = id;
                      context.OutputDetails.Add(detail);
                  }

                  context.SaveChanges();

                  bool result = xmlOutput.Create(id); //Create XML

                  //Update Amounts
                  //IWarehouseProductRepository _warehouseProductRepository = new WarehouseProductRepository();

                  //foreach (OutputDetail detail in output.Details)
                  //{
                  //  _warehouseProductRepository.UpdateAmountByCode(detail.Warehouse, detail.Quantity, detail.ProductCode);
                  //}


              }
          }
          catch (Exception ex)
          {
              id = 0;
              Debug.Write(@"Error " + ex.Message);
          }
          return id;
      }

      public IEnumerable<Output> GetList(int quantity)
    {
      List<Output> outputs = null;

      try
      {
        var _outputs = (from o in context.Outputs
                        select new
                        {
                          Id = o.Id,
                          Order = o.Order,
                          Observation = o.Observation,
                          DateCreated = o.DateCreated,
                          Status = o.Status,
                          Warehouse = (from od in context.OutputDetails
                                           join w in context.Warehouses on od.Warehouse equals w.Code
                                           where od.OutputId == o.Id
                                           select w).FirstOrDefault().Code
                        })
                        .Take(quantity)
                        .ToList();
                        

        if (_outputs != null)
        {
          outputs = _outputs.Select(x => new Output
          {
            Id = x.Id,
            Order = x.Order,
            Observation = x.Observation,
            DateCreated = x.DateCreated,
            Status = x.Status,
            Details = new List<OutputDetail> {
              new OutputDetail { Warehouse = x.Warehouse }
            }
          })
          .ToList();
        }
      }
      catch (Exception e)
      {
        outputs = null;
        Debug.Write(@"Error " + e.Message);
      }

      return outputs;
    }

    public IEnumerable<Output> GetListById(int id)
    {
      List<Output> outputs = null;

      try
      {
        var output = GetById(id);
        if (output != null)
        {
          outputs = new List<Output>();
          outputs.Add(output);
        }
      }
      catch (Exception e)
      {
        outputs = null;
        Debug.Write(@"Error " + e.Message);
      }

      return outputs;
    }

  }
}
