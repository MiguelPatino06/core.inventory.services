using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using TShirt.InventoryApp.Services.Mobile.Properties;

namespace TShirt.InventoryApp.Services.Mobile.Services
{
  public class XmlServices
  {
    private string PATHSERVER { get; set; }
    HttpClient client;
    
    public XmlServices()
    {
      client = new HttpClient();
      client.MaxResponseContentBufferSize = 256000;
      PATHSERVER = Resources.PathServer;
    }

    public async Task<bool> XmlWrite(string documentType, int id)
    {
      string url = "http://" + PATHSERVER + "/tshirt/Xml/XmlWrite";
      string _documentType = "?documentType=" + documentType;
      string _id = "&id=" + id;
      string uri = string.Concat(url, _documentType, _id);

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
