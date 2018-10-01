using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetByCode(string user, string pass);
    }
}
