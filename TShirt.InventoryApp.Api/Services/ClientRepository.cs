using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public class ClientRepository: IClientRepository
    {
        DatabaseHelper context = new DatabaseHelper();

        public Client GetbyCode(string code)
        {
            return context.Clients.FirstOrDefault(a => a.Code == code);
        }

        public List<Client> List(string code)
        {
            return context.Clients.Where(a => a.Code.ToLower().Contains(code.ToLower()) || a.Name.ToLower().Contains(code.ToLower())).ToList();

        }
    }
}