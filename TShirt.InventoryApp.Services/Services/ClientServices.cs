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
    public class ClientServices
    {
        HttpClient client;
        private string PATHSERVER { get; set; }

        public ClientServices()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            PATHSERVER = Settings.Default.PathFile;
        }

        public async Task<List<Client>> Search(string code)
        {
            var items = new List<Client>();
            string url = "http://" + PATHSERVER + "/tshirt/client/search?code=";
            string uri = string.Concat(url, code);
            try
            {
                var result = await client.GetAsync(uri);
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    items = JsonConvert.DeserializeObject<List<Client>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
            return items;
        }

        public async Task<Client> GetByCode(string code)
        {
            var items = new Client();
            string url = "http://" + PATHSERVER + "/tshirt/client/getbycode?code=";
            string uri = string.Concat(url, code);
            try
            {
                var result = await client.GetAsync(uri);
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    items = JsonConvert.DeserializeObject<Client>(content);
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
