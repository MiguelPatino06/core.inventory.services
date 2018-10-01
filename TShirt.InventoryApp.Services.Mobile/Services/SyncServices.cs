using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using TShirt.InventoryApp.Services.Mobile.Properties;

namespace TShirt.InventoryApp.Services.Mobile.Services
{
  public class SyncServices
  {
    private string PATHSERVER { get; set; }
    HttpClient client;

    public SyncServices()
    {
      client = new HttpClient();
      client.MaxResponseContentBufferSize = 256000;
      PATHSERVER = Resources.PathServer;
    }

    public async Task<bool> Execute(string processName)
    {
      string url = "http://" + PATHSERVER + "/tshirt/sync/execute";
      string _processName = "?processName=" + processName;
      string uri = string.Concat(url, _processName);

      try
      {
        HttpResponseMessage response = null;
        response = await client.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
          return true;
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return false;
    }
  }
}
