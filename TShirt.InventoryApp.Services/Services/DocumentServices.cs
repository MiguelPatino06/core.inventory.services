using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TShirt.InventoryApp.Services.Models;
using TShirt.InventoryApp.Services.Properties;

namespace TShirt.InventoryApp.Services.Services
{
  public class DocumentServices
  {
    HttpClient client;

    public List<Document> Items { get; private set; }
    private string PATHSERVER { get; set; }

    public DocumentServices()
    {
      client = new HttpClient();
      client.MaxResponseContentBufferSize = 256000;
      PATHSERVER = Settings.Default.PathFile;
    }


    public async Task<List<Document>> GetAll()
    {
      Items = new List<Document>();
      string uri = "http://" + PATHSERVER + "/tshirt/Document/GetList";

      try
      {
        var result = await client.GetAsync(uri);
        if (result.IsSuccessStatusCode)
        {
          var content = await result.Content.ReadAsStringAsync();
          Items = JsonConvert.DeserializeObject<List<Document>>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return Items;
    }

    public async Task<Document> GetById(int id)
    {
      var document = new Document();
      string url = "http://" + PATHSERVER + "/tshirt/Document/Get?id=";
      string uri = string.Concat(url, id);
      try
      {
        var result = await client.GetAsync(uri);
        if (result.IsSuccessStatusCode)
        {
          var content = await result.Content.ReadAsStringAsync();
          document = JsonConvert.DeserializeObject<Document>(content);
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return document;
    }


    public async Task<int> Save(Document items)
    {
      int document = 0;
      string url = "http://" + PATHSERVER + "/tshirt/Document/Save";

      try
      {
        var json = JsonConvert.SerializeObject(items);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage result = null;

        result = await client.PostAsync(url, content);

        if (result.IsSuccessStatusCode)
        {
          var x = await result.Content.ReadAsStringAsync();
          document = JsonConvert.DeserializeObject<int>(x);

        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }
      return document;
    }

  }
}
