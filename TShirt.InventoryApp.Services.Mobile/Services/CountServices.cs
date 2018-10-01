using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Services.Mobile.Properties;
using Xamarin.Forms.Internals;

namespace TShirt.InventoryApp.Services.Mobile.Services
{

  public class CountServices
  {

    HttpClient client;
    private string PATHSERVER { get; set; }

    public List<CountPlan> Items { get; private set; }

  
    public CountServices()
    {
      client = new HttpClient();
      client.MaxResponseContentBufferSize = 256000;
      client.Timeout = TimeSpan.FromSeconds(180);
      PATHSERVER = Resources.PathServer;
    }

      [Preserve]
      public async Task<List<CountPlan>> GetAll()
      {
            List<CountPlan> Items = new List<CountPlan>();
   
          string uri = "http://" + PATHSERVER + "/tshirt/Count/GetAll";

          try
          {

                //Items2.Add(new CountPlan()
                //{
                //    Id = 3,
                //    Name = "T - SHIRT GILDAN",
                //    Description = "Inv.T - Shirt Gildan(D / C)",
                //    Status = "2",
                //});
                //Items2.Add(new CountPlan()
                //{
                //    Id = 4,
                //    Name = "T - SHIRT GILDAN44",
                //    Description = "Inv.T - Shirt Gildan(D / C)44",
                //    Status = "2",
                //});

                // string result2 = @"[{'Id':3,'Name':'T - SHIRT GILDAN','Description':'Inv.T - Shirt Gildan(D / C)','Status':'2','DateCreated':'1 / 1 / 00','Warehouse':'DEPGARAN   ','Value2':null,'Value3':null,'Value4':null,'Value5':null,'UserUpdated':null,'DateUpdated':null,'Details':null},{'Id':8,'Name':'MOTOR OUTDOORS','Description':'CAMISA OUTDOORS TASLAN','Status':'2','DateCreated':'1 / 1 / 00','Warehouse':'GALXPRAM   ','Value2':null,'Value3':null,'Value4':null,'Value5':null,'UserUpdated':null,'DateUpdated':null,'Details':null}]";

                //Items2 = JsonConvert.DeserializeObject<List<CountPlan>>(result2, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, SerializationBinder = new DefaultSerializationBinder() });
                var result = await client.GetAsync(uri);

                if (result.IsSuccessStatusCode)
                {

                    var content = await result.Content.ReadAsStringAsync();
                   Items = JsonConvert.DeserializeObject<List<CountPlan>>(content);
                }
            }
          catch (Exception ex)
          {
              Debug.WriteLine(@"				ERROR {0}", ex.Message);
          }

          return Items;
      }

      public async Task<List<ViewCountPlanDetail>> GetById(int id)
    {
      var plan = new List<ViewCountPlanDetail>();
      string url = "http://" + PATHSERVER + "/tshirt/Count/GetCountById?id=";
      string uri = string.Concat(url, id);
      try
      {
        var result = await client.GetAsync(uri);
        if (result.IsSuccessStatusCode)
        {
          var content = await result.Content.ReadAsStringAsync();
          plan = JsonConvert.DeserializeObject<List<ViewCountPlanDetail>>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return plan;
    }


    public async Task<string> SaveDetail(List<CountPlanDetailItem> items)
    {
      string plan = string.Empty;
      string url = "http://" + PATHSERVER + "/tshirt/Count/SaveDetails";

      try
      {
        var json = JsonConvert.SerializeObject(items);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage result = null;

        result = await client.PostAsync(url, content);

        if (result.IsSuccessStatusCode)
        {
          var x = await result.Content.ReadAsStringAsync();
          plan = JsonConvert.DeserializeObject<string>(x);

        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return plan;
    }


    public async Task<List<CountPlanDetailItem>> GetByPlanAndProduct(int id, string product)
    {
      var plan = new List<CountPlanDetailItem>();
      string url = "http://" + PATHSERVER + "/tshirt/Count/GetCountByPlanAndProduct?";
      string parameter1 = "id=" + id;
      string parameter2 = "&product=" + product;
      string uri = string.Concat(url, parameter1, parameter2);
      try
      {
        var result = await client.GetAsync(uri);
        if (result.IsSuccessStatusCode)
        {
          var content = await result.Content.ReadAsStringAsync();
          plan = JsonConvert.DeserializeObject<List<CountPlanDetailItem>>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return plan;
    }


    public async Task<bool> SaveCountPlan(CountPlan item)
    {
      bool plan = true;
      string url = "http://" + PATHSERVER + "/tshirt/Count/SavePlan";

      try
      {
        var json = JsonConvert.SerializeObject(item);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage result = null;

        result = await client.PostAsync(url, content);

        if (result.IsSuccessStatusCode)
        {
          var x = await result.Content.ReadAsStringAsync();
          plan = JsonConvert.DeserializeObject<bool>(x);

        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return plan;
    }

  }
}
