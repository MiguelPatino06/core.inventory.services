using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public interface IClientRepository
    {
        Client GetbyCode(string search);
        List<Client> List(string search);

    }
}
