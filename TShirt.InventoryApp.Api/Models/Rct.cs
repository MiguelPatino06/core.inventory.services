using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
    [Table("Rct")]
    public class Rct
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Code { get; set; }
        public string ProviderCode { get; set; }
        public string Lot { get; set; }
        public string DateCreated { get; set; }
        public int UserId { get; set; }
        
    }
}