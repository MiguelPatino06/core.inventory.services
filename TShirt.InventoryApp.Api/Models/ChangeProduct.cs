using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
    [SQLite.Net.Attributes.Table("ChangeProduct")]
    public class ChangeProduct
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Code { get; set; }
        public string ClientCode { get; set; }
        public string DateCreated { get; set; }
        public string Observation { get; set; }

    }
}