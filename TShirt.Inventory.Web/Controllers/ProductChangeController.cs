using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using PagedList;
using TShirt.InventoryApp.Services.Services;
using TShirt.InventoryApp.Services.Models;
using WebGrease.Css.Extensions;
//using TShirt.InventoryApp.Integration;
//using TShirt.InventoryApp.Integration.From.GP;
using TShirt.InventoryApp.Web.ServiceReferenceGP;
using Document = TShirt.InventoryApp.Services.Models.Document;
using DocumentDetail = TShirt.InventoryApp.Services.Models.DocumentDetail;

namespace TShirt.InventoryApp.Web.Controllers
{
  public class ProductChangeController : Controller
  {


    private ProductChangeServices _services;
    private ProductServices _servicesProduct;

    public async Task<ActionResult> Index(int? aPage, int? aItemsPerPage, string aStringSearch)
    {
      //Documents.Inventory dcInventory = new Documents.Inventory();
      //ServiceReferenceGP.GpClient objecClient = new GpClient();

      //var _list = new List<DocumentDetail>()
      //{
      //    new DocumentDetail { DocumentId = 1, ProductCode = "TTC3700C01", Quantity = "23"},
      //    new DocumentDetail { DocumentId = 1, ProductCode = "TTC3700C0110A62", Quantity = "23"}
      //};

      //var document = new Document();
      //document.Id = 1;
      //document.Details = _list;

      //string resultDocument = dcInventory.GenerateDocument(document);


      //var result = objecClient.TransactionIn(resultDocument);



      ViewBag._StringSearch = aStringSearch;
      ViewBag._ItemsPerPage = aItemsPerPage.ToString();

      var _PageNumber = (aPage ?? 1);
      var _ItemsPerPage = (aItemsPerPage ?? 10);

      var page = await ListOrders(_PageNumber, _ItemsPerPage, aStringSearch);

      Session["Section"] = "ProductChange";

      return View(page);

    }

    public async Task<ActionResult> Add(string code)
    {
      _services = new ProductChangeServices();
      var order = new OrderReqExtend();

      if (!string.IsNullOrEmpty(code))
      {
        order = await _services.GetDetailByCode(code);
      }
      else
      {
        return View("Index");
      }


      return View(order);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Add(OrderReqExtend aModel)
    {
      _services = new ProductChangeServices();
      var order = new OrderReqExtend();

      if (aModel != null)
      {
        var result = await _services.Save(aModel);
        if (result)
        {
          return RedirectToAction("Index");
        }
      }

      return View("Index");
    }




    public async Task<ActionResult> OrderDetails(List<OrderReqDetailExtend> aModel, string order, string prod, string prodChan, string prodChanNam, int? qty)
    {
      if (!string.IsNullOrEmpty(order))
      {
        _services = new ProductChangeServices();
        var result = new OrderReqExtend();
        var list = new List<OrderReqDetailExtend>();
        result = await _services.GetDetailByCode(order);
        if (result != null)
        {
          var op = Session["OptionCRUD"].ToString();

          if (op == "INS")
          {
            result.Detail.Where(a => a.ProductCode == prod).ForEach(p =>
            {
              p.ProductCodeChanged = prodChan;
              p.ProductNameChanged = prodChanNam;
              if (qty != null) p.Quantity = (int)qty;
            });
          }
          else if (op == "DEL")
          {
            result.Detail.Where(a => a.ProductCode == prod).ForEach(p =>
            {
              p.ProductCodeChanged = null;
              p.ProductNameChanged = null;
              p.Quantity = null;
            });
          }


          //return PartialView("_Details", (IEnumerable<OrderReqDetailExtend>)result.Detail);
          return PartialView("_Details", result.Detail);
        }
      }

      //return PartialView("_Details", (IEnumerable<OrderReqDetailExtend>)aModel);
      return PartialView("_Details", aModel);
    }


    public ActionResult Edit(string orderCode, string productCode, string productName, string op)
    {
      var aModel = new OrderReqDetailExtend();
      aModel.OrderReqCode = orderCode;
      aModel.ProductCode = productCode;
      aModel.ProductName = productName;

      Session["OptionCRUD"] = op;

      ViewBag.Option = op;

      ViewData["searchProd"] = GetSearchProducts();

      return PartialView("_Edit", aModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "OrderReqCode, ProductCode, ProductCodeChanged, ProductNameChanged")]OrderReqDetailExtend aModel, int? ProductQty)
    {

      if (aModel != null)
      {
        string url = Url.Action("OrderDetails", "ProductChange", new { order = aModel.OrderReqCode, prod = aModel.ProductCode, prodChan = aModel.ProductCodeChanged, prodChanNam = aModel.ProductNameChanged, qty = ProductQty });
        return Json(new { success = true, url = url });
      }

      return PartialView("_Edit", aModel);
    }


    private async Task<StaticPagedList<OrderReqExtend>> ListOrders(int aPage, int aItemsPerPage, string aStringSearch)
    {
      var _count = 0;
      _services = new ProductChangeServices();

      List<OrderReqExtend> x = new List<OrderReqExtend>();

      var orders = await _services.GetAllWeb(aPage - 1, aItemsPerPage, aStringSearch);

      x = orders.Item1;

      return new StaticPagedList<OrderReqExtend>(x, aPage, aItemsPerPage, orders.Item2);
    }


    [HttpPost]
    public async Task<ActionResult> GetProduct(string search)
    {
      //_servicesProduct = new ProductServices();

      //List<Product> products = await _servicesProduct.Search(search);


      //return Json(products.ToList().FirstOrDefault(), JsonRequestBehavior.AllowGet);



      _servicesProduct = new ProductServices();

      List<Product> products = await _servicesProduct.Search(search);

      int countResult = products.Count;

      var result = new { Result = products, CountProduct = products.Count };


      return Json(result, JsonRequestBehavior.AllowGet);

    }


    [HttpPost]
    public async Task<ActionResult> GetClient(string search)
    {

      _servicesProduct = new ProductServices();

      List<Product> products = await _servicesProduct.Search(search);

      int countResult = products.Count;

      var result = new { Result = products, CountProduct = products.Count };


      return Json(result, JsonRequestBehavior.AllowGet);

    }

    public List<SelectListItem> GetSearchProducts()
    {
      List<SelectListItem> listItem = new List<SelectListItem>();
      listItem.Add(new SelectListItem { Value = "0", Text = "", Selected = true });
      return listItem;
    }

    #region Json
    [HttpPost]
    public async Task<ActionResult> GetOrder(string id)
    {
      _services = new ProductChangeServices();

      var products = await _services.GetOrderByCode(id);
      return Json(products, JsonRequestBehavior.AllowGet);
    }

    #endregion

  }
}