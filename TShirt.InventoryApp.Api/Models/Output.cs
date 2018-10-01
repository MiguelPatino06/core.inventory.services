using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace TShirt.InventoryApp.Api.Models
{
  [SQLite.Net.Attributes.Table("Output")]
  public class Output
  {
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Order { get; set; }
    public string Observation { get; set; }
    public string DateCreated { get; set; }
    public string Status { get; set; }
    [NotMapped]
    public List<OutputDetail> Details { get; set; }
  }
}