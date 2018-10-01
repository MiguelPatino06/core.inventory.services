using PagedList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using TShirt.InventoryApp.Services.Models;
using TShirt.InventoryApp.Services.Services;
using TShirt.InventoryApp.Web.Models;

namespace TShirt.InventoryApp.Web.Controllers
{
  public class ProductTransferController : Controller
  {
    private WarehouseServices _warehouseServices;
    private ProductTransferServices _productTransferServices;
    private static List<Warehouse> warehouses;

    // GET: ProductTransfer
    public async Task<ActionResult> Index()
    {
      /*
      string body = Utils.CreateSampleHtml("042");
      Utils.SendEmail("Prueba email", body, "edinson.jgm@gmail.com", true);
      */
      ProductTransferModels model = new ProductTransferModels();
      model.warehouseOrigin = "TSHIRT";
      ViewData["warehouses"] = await getWarehouses();
      Session["Section"] = "ProductTransfer";
      return View(model);
    }

    [HttpPost]
    public ActionResult Index(ProductTransferModels model)
    {
      List<SelectListItem> list = new List<SelectListItem> {
        new SelectListItem { Value = "0", Text = "Buscar por:" },
        new SelectListItem { Value = "1", Text = "Código"},
        new SelectListItem { Value = "2", Text = "Nombre" }
      };
      ViewData["searchType"] = list;
      return View("ProductTransfer",model);
    }

    public async Task<List<SelectListItem>> getWarehouses()
    {
      List<SelectListItem> list = new List<SelectListItem>();

      try
      {
        _warehouseServices = new WarehouseServices();

        var result = await _warehouseServices.GetListWarehouse();
        if (result == null)
        {
          ModelState.AddModelError("", "Error al cargar la lista de almacenes");
        }
        else
        {
          warehouses = result;
          foreach (Warehouse warehouse in warehouses)
          {
            list.Add(new SelectListItem { Value = warehouse.Code.ToString(), Text = warehouse.Code });
          }
        }
      }
      catch (System.Exception)
      {
        return null;
        throw;
      }

      return list;
    }

    public async Task<JsonResult> getWarehouseProductList(string warehouseCode, string criteria)
    {
      List<WarehouseProduct> list = new List<WarehouseProduct>();

      try
      {
        _productTransferServices = new ProductTransferServices();
        var result = await _productTransferServices.GetByCriteria(warehouseCode, criteria);
        if (result == null)
        {
          ModelState.AddModelError("", "Error al buscar el producto");
        }
        else
        {
          list = result;
        }
      }
      catch (System.Exception)
      {
        return null;
      }

      return Json(list, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public async Task<ActionResult> SaveTransfer(ProductTransferModels model) {

      ProductTransfer productTransfer = new ProductTransfer()
      {
        warehouseOrigin = model.warehouseOrigin,
        warehouseDestiny = model.warehouseDestiny,
        observation = model.observation,
        status = "0",
        products = model.products,
        dateCreated = DateTime.Now.ToString("dd/MM/yyyy hh:mm")
      };

      try
      {
        _productTransferServices = new ProductTransferServices();
        var result = await _productTransferServices.SaveProductTransfer(productTransfer);
        if (result == null)
        {
          ModelState.AddModelError("", "Error al buscar el producto");
        }

      }
      catch (System.Exception)
      {
        return null;
      }

      return RedirectToAction("getAll");

    }

    public async Task<ActionResult> getAll(int? aPage, int? aItemsPerPage, string aStringSearch)
    {
      ViewBag._StringSearch = aStringSearch;
      ViewBag._ItemsPerPage = aItemsPerPage.ToString();

      var _PageNumber = (aPage ?? 1);
      var _ItemsPerPage = (aItemsPerPage ?? 5);

      var page = await ListTransfer(_PageNumber, _ItemsPerPage, aStringSearch);

      Session["Section"] = "ProductTransfer";

      return View("List", page);

    }

    private async Task<StaticPagedList<TransferDetail>> ListTransfer(int aPage, int aItemsPerPage, string aStringSearch)
    {
      _productTransferServices = new ProductTransferServices();

      List<TransferDetail> x = new List<TransferDetail>();

      var transfers = await _productTransferServices.GetRequests(aPage - 1, aItemsPerPage, aStringSearch);

      x = transfers.Item1;

      return new StaticPagedList<TransferDetail>(x, aPage, aItemsPerPage, transfers.Item2);
    }

    public async Task<ActionResult> Detail(string id)
    {
      TransferDetail transfer = new TransferDetail();

      try
      {
        _productTransferServices = new ProductTransferServices();

        var result = await _productTransferServices.Get(int.Parse(id));
        if (result == null)
        {
          ModelState.AddModelError("", "Error al cargar la lista de almacenes");
        }
        else
        {
          transfer = result;
        }
      }
      catch (System.Exception)
      {
        return null;
        throw;
      }

      return View("Detail", transfer);
      
    }

  }
}