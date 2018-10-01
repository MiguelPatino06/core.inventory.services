using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using TShirt.InventoryApp.Services.Models;
using TShirt.InventoryApp.Services.Services;

namespace TShirt.InventoryApp.Web.Controllers
{
  public class SampleController : Controller
  {
    // GET: Sample

    private ProductServices _services;
    private SampleServices _servicesSample;
    private ClientServices _clientServices;
    public async Task<ActionResult> Index(int? id)
    {
      _servicesSample = new SampleServices();
      var model = new Sample();

      ViewData["searchProd"] = getSearchProducts();
      ViewData["searchClient"] = getSearchClient();

      if (id != null)
        if (id > 0)
        {
          var aModel = await _servicesSample.GetById((int)id);
          return View(aModel);
        }



      model.Id = 0;

      Session["Section"] = "Sample";
      return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> Index(Sample aModel, string submitButton)
    {
      _servicesSample = new SampleServices();

      aModel.User = Session["UserSession"].ToString();

      var result = await _servicesSample.Save(aModel);

      if (result > 0)
        return RedirectToAction("List");

      return View();
    }


    public async Task<ActionResult> Order(int code)
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> GetProduct(string search)
    {
      _services = new ProductServices();

      List<Product> products = await _services.Search(search);

      int countResult = products.Count;

      var result = new { Result = products, CountProduct = products.Count };


      return Json(result, JsonRequestBehavior.AllowGet);
    }


    [HttpPost]
    public async Task<ActionResult> GetClient(string search)
    {
      _clientServices = new ClientServices();

      List<Client> client = await _clientServices.Search(search);

      int countResult = client.Count;

      var result = new { Result = client, countResult = client.Count };


      return Json(result, JsonRequestBehavior.AllowGet);
    }


    public async Task<ActionResult> List(int? aPage, int? aItemsPerPage, string aStringSearch)
    {
      ViewBag._StringSearch = aStringSearch;
      ViewBag._ItemsPerPage = aItemsPerPage.ToString();

      var _PageNumber = (aPage ?? 1);
      var _ItemsPerPage = (aItemsPerPage ?? 10);

      var page = await ListSample(_PageNumber, _ItemsPerPage, aStringSearch);

      return View(page);

    }

    private async Task<StaticPagedList<ViewSampleSumProduct>> ListSample(int aPage, int aItemsPerPage, string aStringSearch)
    {
      var _count = 0;
      _servicesSample = new SampleServices();

      List<ViewSampleSumProduct> x = new List<ViewSampleSumProduct>();

      var orders = await _servicesSample.GetAllWeb(aPage - 1, aItemsPerPage, aStringSearch);

      x = orders.Item1;

      return new StaticPagedList<ViewSampleSumProduct>(x, aPage, aItemsPerPage, orders.Item2);
    }


    public List<SelectListItem> getSearchProducts()
    {
      List<SelectListItem> listItem = new List<SelectListItem>();
      listItem.Add(new SelectListItem { Value = "0", Text = "", Selected = true });
      return listItem;
    }

    public List<SelectListItem> getSearchClient()
    {
      List<SelectListItem> listItem = new List<SelectListItem>();
      listItem.Add(new SelectListItem { Value = "0", Text = "", Selected = true });
      return listItem;
    }
  }
}