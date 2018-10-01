using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public interface IRolRepository
    {
        Rol Add(Rol rol);
        IEnumerable<Rol> GetAll();
        Rol GetById(int id);
        bool Delete(int id);
        bool Update(Rol rol);
    }
}
