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
    public class UserServices
    {
        HttpClient client;
        private string PATHSERVER { get; set; }
        public User Item { get; private set; }

        public UserServices()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            PATHSERVER = "10.1.92.207:81"; //  Settings.Default.PathFile;
        }

        public async Task<User> GetProviderName(string code, string pass)
        {
            Item = new User();
            string url = "http://" + PATHSERVER + "/tshirt/user/Getuser";
            string _code = "?code=" + code;
            string _pass = "&pass=" + pass;
            string uri = string.Concat(url, _code, _pass);

            try
            {
                var result = await client.GetAsync(uri);
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    Item = JsonConvert.DeserializeObject<User>(content);
                }
                else
                    Item = null;
            }
            catch (Exception ex)
            {
                Item = null;
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }
            return Item;
        }
    }
}
