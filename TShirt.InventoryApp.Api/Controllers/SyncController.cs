using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TShirt.InventoryApp.Api.Models;
using TShirt.InventoryApp.Api.Services;

namespace TShirt.InventoryApp.Api.Controllers
{
  public class SyncController : ApiController
  {
    private readonly ISyncRepository _syncRepository = new SyncRepository();

    [HttpGet]
    public bool execute(string processName)
    {
      return _syncRepository.execute(processName);
    }
  }
}
