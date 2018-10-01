using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TShirt.InventoryApp.Services.Models;
using TShirt.InventoryApp.Services.Properties;

namespace TShirt.InventoryApp.Services.Services
{
  public class RctServices
  {
    HttpClient client;
    private string PATHSERVER { get; set; }

    public RctServices()
    {
      client = new HttpClient();
      client.MaxResponseContentBufferSize = 256000;
      PATHSERVER = Settings.Default.PathFile;
    }


    public async Task<RctExtendModel> Add(RctExtendModel model)
    {
      var _rct = new RctExtendModel();
      string url = "http://" + PATHSERVER + "/tshirt/rct/PostRct";

      try
      {
        var json = JsonConvert.SerializeObject(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage result = null;

        result = await client.PostAsync(url, content);

        if (result.IsSuccessStatusCode)
        {
          var x = await result.Content.ReadAsStringAsync();
          _rct = JsonConvert.DeserializeObject<RctExtendModel>(x);
        }
      }
      catch (Exception ex)
      {
        _rct = null;
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return _rct;

    }


  }
}
