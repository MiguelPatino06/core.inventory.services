using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms.Internals;

namespace TShirt.InventoryApp.Services.Mobile.Models
{
    [Preserve(AllMembers = true)]
    public class CountPlan
    {
        [JsonConstructor]
        public CountPlan()
        {


        }

        //[JsonConstructor]
        //public CountPlan(int id, string name, string description, string status, string dateCreated, string warehouse, string value2, string value3, string value4, string value5, string userUpdated, string dateUpdated, List<CountPlanDetail> details)
        //{
        //    this.Id = Id;
        //    this.Name = name;
        //    this.Description = description;
        //    this.Status = status;
        //    this.DateCreated = dateCreated;
        //    this.Warehouse = warehouse;
        //    this.Value2 = value2;
        //    this.Value3 = value3;
        //    this.Value4 = value4;
        //    this.Value5 = value5;
        //    this.UserUpdated = userUpdated;
        //    this.DateUpdated = dateUpdated;
        //    this.Details = details;

        //}
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "DateCreated")]
        public string DateCreated { get; set; }

        [JsonProperty(PropertyName = "Warehouse")]
        public string Warehouse { get; set; }

        [JsonProperty(PropertyName = "Value2")]
        public string Value2 { get; set; }

        [JsonProperty(PropertyName = "Value3")]
        public string Value3 { get; set; }

        [JsonProperty(PropertyName = "Value4")]
        public string Value4 { get; set; }

        [JsonProperty(PropertyName = "Value5")]
        public string Value5 { get; set; }

        [JsonProperty(PropertyName = "UserUpdated")]
        public string UserUpdated { get; set; }

        [JsonProperty(PropertyName = "DateUpdated")]
        public string DateUpdated { get; set; }

        [JsonProperty(PropertyName = "Details")]
        public List<CountPlanDetail> Details { get; set; }
  }
}
