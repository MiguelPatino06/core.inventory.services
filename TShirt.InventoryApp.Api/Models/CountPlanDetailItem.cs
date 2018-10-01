using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
    [Table("CountPlanDetailItem")]
    public class CountPlanDetailItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CountPlanId { get; set; }
        public string UserCode { get; set; }
        public string DateCreated { get; set; }
        public int Quantity { get; set; }
        public string ProductCode { get; set; }
        public int Count { get; set; }
    }
}