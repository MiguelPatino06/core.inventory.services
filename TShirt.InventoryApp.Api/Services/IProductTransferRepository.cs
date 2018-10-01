using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
  public interface IProductTransferRepository
  {
    ProductTransfer Save(ProductTransfer productTransfer);
    TransferDetail GetById(int id);
    IEnumerable<TransferDetail> GetRequests();
    IEnumerable<TransferDetail> GetRequests(int code);
  }
}
