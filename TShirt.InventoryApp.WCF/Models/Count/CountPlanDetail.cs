using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;


namespace TShirt.InventoryApp.WCF.Models.Count
{
    [Table("CountPlanDetail")]
    public class CountPlanDetail
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(CountPlan))]
        public int CountPlanId { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public int? TotalCounted { get; set; }
        public string DateCreated { get; set; }
        public int UserIdCreated { get; set; }
        public string Warehouse { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public string Value5 { get; set; }
        public string DateUpdated { get; set; }
        public int UserIdUpdated { get; set; }


    }
}