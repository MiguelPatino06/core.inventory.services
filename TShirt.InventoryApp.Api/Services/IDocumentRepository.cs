using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public interface IDocumentRepository
    {
        Document GetById(int id);
        List<Document> GetAll();
        int Save(Document items);

    }
}
