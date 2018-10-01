using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
    [Table("ViewSampleSumProduct")]
    public class ViewSampleSumProduct
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Client { get; set; }
        public string Observation { get; set; }
        public string DateCreated { get; set; }
        public int Total { get; set; }
    }
}