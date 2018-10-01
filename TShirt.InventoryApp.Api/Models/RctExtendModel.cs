using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TShirt.InventoryApp.Api.Models
{
    public class RctExtendModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string ProviderCode { get; set; }
        public string Lot { get; set; }
        public string DateCreated { get; set; }
        public int UserId { get; set; }
        public List<Detail> Details { get; set; }
    }

    public class Detail
    {
        public int Id { get; set; }
        public int RctId { get; set; }
        public string OrderCode { get; set; }
        public string Status { get; set; }
        public string ProductCode { get; set; }
        public string Warehouse { get; set; }
        public int Quantity { get; set; }

    }
}