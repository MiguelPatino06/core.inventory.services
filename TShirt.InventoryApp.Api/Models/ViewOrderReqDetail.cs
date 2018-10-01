using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
    [Table("ViewOrder")]
    public class ViewOrderReqDetail
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string OrderReqCode { get; set; }
        public string Observation { get; set; }
        public string ProductCode { get; set; }
        public string DescriptionProduct { get; set; }
        public string ProductCodeChanged { get; set; }
        public string DescriptionProductChanged { get; set; }
        public int Quantity { get; set; }
        public string DateProductChanged { get; set; }
        public string UserUpdated { get; set; }

    }
}