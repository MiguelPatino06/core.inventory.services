using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TShirt.InventoryApp.Api.Models
{
    public class OrderReqExtend
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string ClientCode { get; set; }
        public string DateCreated { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public string Value5 { get; set; }
        public string Observation { get; set;  }
        public string ClientName { get; set; }
        [NotMapped]
        public List<OrderReqDetailExtend> Detail { get; set; }

    }
}