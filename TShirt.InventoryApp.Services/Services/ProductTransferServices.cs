using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Services.Models;
using TShirt.InventoryApp.Services.Properties;

namespace TShirt.InventoryApp.Services.Services
{
  public class ProductTransferServices
  {
    private string PATHSERVER { get; set; }
    HttpClient client;

    public ProductTransferServices()
    {
      client = new HttpClient();
      client.MaxResponseContentBufferSize = 256000;
      PATHSERVER = Settings.Default.PathFile;
    }

    public async Task<WarehouseProduct> GetWarehouseProduct(string warehouseCode, string productCode)
    {
      WarehouseProduct warehouseProduct = null;
      string url = "http://" + PATHSERVER + "/tshirt/warehouseproduct/GetWarehouseProductByCodes";
      string _warehouseCode = "?warehouseCode=" + warehouseCode;
      string _productCode = "&productCode=" + productCode;
      string uri = string.Concat(url, _warehouseCode, _productCode);

      try
      {
        HttpResponseMessage result = null;

        result = await client.GetAsync(uri);

        if (result.IsSuccessStatusCode)
        {
          var x = await result.Content.ReadAsStringAsync();
          warehouseProduct = JsonConvert.DeserializeObject<WarehouseProduct>(x);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return warehouseProduct;
    }

    public async Task<List<WarehouseProduct>> GetByCriteria(string warehouseCode, string criteria)
    {
      List<WarehouseProduct> list = null;
      string url = "http://" + PATHSERVER + "/tshirt/warehouseproduct/GetByCriteria";
      string _warehouseCode = "?warehouseCode=" + warehouseCode;
      string _criteria = "&criteria=" + criteria;
      string uri = string.Concat(url, _warehouseCode, _criteria);

      try
      {
        HttpResponseMessage result = null;

        result = await client.GetAsync(uri);

        if (result.IsSuccessStatusCode)
        {
          var x = await result.Content.ReadAsStringAsync();
          list = JsonConvert.DeserializeObject<List<WarehouseProduct>>(x);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return list;
    }



    public async Task<ProductTransfer> SaveProductTransfer(ProductTransfer productTransfer)
    {
      var _productTransfer = productTransfer;
      string url = "http://" + PATHSERVER + "/tshirt/producttransfer/save";
      try
      {
        string json = JsonConvert.SerializeObject(_productTransfer);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        Debug.WriteLine(json);

        HttpResponseMessage result = null;

        result = await client.PostAsync(url, content);

        if (result.IsSuccessStatusCode)
        {
          var x = await result.Content.ReadAsStringAsync();
          _productTransfer = JsonConvert.DeserializeObject<ProductTransfer>(x);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return _productTransfer;
    }

    //Tuple permite devolver en un metodo dos resultados
    public async Task<Tuple<List<TransferDetail>, int>> GetRequests(int aPage, int aElementsPerPage, string aSearchString)
    {
      var items = new List<TransferDetail>();
      string url = "http://" + PATHSERVER + "/tshirt/producttransfer/GetRequests";
      string _code = "?code=" + aSearchString;
      string uri = string.Concat(url, _code);

      int count = 0;
      try
      {
        var result = await client.GetAsync(uri);
        if (result.IsSuccessStatusCode)
        {
          var content = await result.Content.ReadAsStringAsync();
          items = JsonConvert.DeserializeObject<List<TransferDetail>>(content);
          count = items.Count;
          items = items.OrderByDescending(a => a.Id).Skip(aPage * aElementsPerPage).Take(aElementsPerPage).ToList();
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return Tuple.Create(items, count);
    }

    public async Task<TransferDetail> Get(int id)
    {

      TransferDetail transferDetail = null;
      string url = "http://" + PATHSERVER + "/tshirt/producttransfer/Get";
      string _id = "?id=" + id;
      string uri = string.Concat(url, _id);

      try
      {
        HttpResponseMessage result = null;

        result = await client.GetAsync(uri);

        if (result.IsSuccessStatusCode)
        {
          var x = await result.Content.ReadAsStringAsync();
          transferDetail = JsonConvert.DeserializeObject<TransferDetail>(x);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return transferDetail;
    }
  }
}
