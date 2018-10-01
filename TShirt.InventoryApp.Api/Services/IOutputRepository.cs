using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public interface IOutputRepository
  {
        Output GetById(int id);
        int Save(Output output);
        IEnumerable<Output> GetList(int quantity);
        IEnumerable<Output> GetListById(int id);
  }
}
