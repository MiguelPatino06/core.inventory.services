using System.Collections.Generic;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public interface IRctRepository
    {
        RctExtendModel Add(RctExtendModel rct);
        IEnumerable<Rct> GetAll();
        Rct GetByCode(string code);
        bool Delete(int id);
        bool Update(Rct rct);
    }
}