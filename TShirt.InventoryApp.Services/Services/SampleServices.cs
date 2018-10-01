using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TShirt.InventoryApp.Services.Models;
using TShirt.InventoryApp.Services.Properties;

namespace TShirt.InventoryApp.Services.Services
{
  public class SampleServices
  {
    private string PATHSERVER { get; set; }

    HttpClient client;

    public Sample Items { get; private set; }

    public SampleServices()
    {
      client = new HttpClient();
      client.MaxResponseContentBufferSize = 256000;
      PATHSERVER = Settings.Default.PathFile;
    }

    public async Task<Sample> GetById(int id)
    {
      Items = new Sample();
      string url = "http://" + PATHSERVER + "/tshirt/Sample/Get?id=";
      string uri = string.Concat(url, id);

      try
      {
        var result = await client.GetAsync(uri);
        if (result.IsSuccessStatusCode)
        {
          var content = await result.Content.ReadAsStringAsync();
          Items = JsonConvert.DeserializeObject<Sample>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return Items;
    }

      public async Task<List<ViewSampleSumProduct>> GetAll()
      {
          var ListItems = new List<ViewSampleSumProduct>();
          string uri = "http://" + PATHSERVER + "/tshirt/Sample/GetList";

          try
          {
              var result = await client.GetAsync(uri);
              if (result.IsSuccessStatusCode)
              {
                  var content = await result.Content.ReadAsStringAsync();
                  ListItems = JsonConvert.DeserializeObject<List<ViewSampleSumProduct>>(content);
              }
          }
          catch (Exception ex)
          {
              Debug.WriteLine(@"				ERROR {0}", ex.Message);
          }
          return ListItems;
      }


      public async Task<Tuple<List<ViewSampleSumProduct>, int>> GetAllWeb(int aPage, int aElementsPerPage,
          string aSearchString)
      {
          var ListItems = new List<ViewSampleSumProduct>();
          string uri = "http://" + PATHSERVER + "/tshirt/Sample/GetList";
          int count = 0;
          try
          {
              var result = await client.GetAsync(uri);
              if (result.IsSuccessStatusCode)
              {
                  var content = await result.Content.ReadAsStringAsync();
                  ListItems = JsonConvert.DeserializeObject<List<ViewSampleSumProduct>>(content);
                  count = ListItems.Count;
                  int _id = 0;
                  if (!string.IsNullOrEmpty(aSearchString))
                  {
                      try
                      {
                          _id = int.Parse(aSearchString);
                      }
                      catch (Exception ex)
                      {
                          
                      }
                  }

                  ListItems = _id > 0 ? ListItems.Where(b=> b.Id == _id).OrderByDescending(a => a.Id).Skip(aPage * aElementsPerPage).Take(aElementsPerPage).ToList() : ListItems.OrderByDescending(a => a.Id).Skip(aPage * aElementsPerPage).Take(aElementsPerPage).ToList();


                }
          }
          catch (Exception ex)
          {
              Debug.WriteLine(@"				ERROR {0}", ex.Message);
          }
          return Tuple.Create(ListItems, count);
      }


      public async Task<int> Save(Sample items)
    {
      int sample = 0;
      string url = "http://" + PATHSERVER + "/tshirt/Sample/Save";

      try
      {
        var json = JsonConvert.SerializeObject(items);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage result = null;

        result = await client.PostAsync(url, content);

        if (result.IsSuccessStatusCode)
        {
          var x = await result.Content.ReadAsStringAsync();
          sample = JsonConvert.DeserializeObject<int>(x);

        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return sample;
    }

    public async Task<bool> Update(Sample items)
    {
      bool sample = false;
      string url = "http://" + PATHSERVER + "/tshirt/Sample/Update";

      try
      {
        var json = JsonConvert.SerializeObject(items);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage result = null;

        result = await client.PostAsync(url, content);

        if (result.IsSuccessStatusCode)
        {
          var x = await result.Content.ReadAsStringAsync();
          sample = JsonConvert.DeserializeObject<bool>(x);

        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return sample;
    }

    public async Task<List<ViewSampleSumProduct>> GetList()
    {
      var items = new List<ViewSampleSumProduct>();
      string uri = "http://" + PATHSERVER + "/tshirt/Sample/GetList";

      try
      {
        var result = await client.GetAsync(uri);
        if (result.IsSuccessStatusCode)
        {
          var content = await result.Content.ReadAsStringAsync();
          items = JsonConvert.DeserializeObject<List<ViewSampleSumProduct>>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return items;
    }

  }
}
