using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TShirt.InventoryApp.Services.Mobile.Models;
using TShirt.InventoryApp.Services.Mobile.Properties;

namespace TShirt.InventoryApp.Services.Mobile.Services
{
  public class ProviderService
  {
    HttpClient client;
    public List<Provider> Items { get; private set; }
    private string PATHSERVER { get; set; }

    public ProviderService()
    {
      client = new HttpClient();
      client.MaxResponseContentBufferSize = 256000;
      PATHSERVER = Resources.PathServer;
    }

    public async Task<List<Provider>> GetProviderName(string name)
    {
      Items = new List<Provider>();
      string url = "http://" + PATHSERVER + "/tshirt/provider/GetProviderName?name=";
      string uri = string.Concat(url, name);
      try
      {
        var result = await client.GetAsync(uri);
        if (result.IsSuccessStatusCode)
        {
          var content = await result.Content.ReadAsStringAsync();
          Items = JsonConvert.DeserializeObject<List<Provider>>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return Items;
    }


  }
}
