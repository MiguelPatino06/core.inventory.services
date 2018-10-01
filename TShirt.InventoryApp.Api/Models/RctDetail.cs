using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
    [Table("RctDetail")]
    public class RctDetail
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int RctId { get; set; }
        public string OrderCode { get; set; }


    }
}