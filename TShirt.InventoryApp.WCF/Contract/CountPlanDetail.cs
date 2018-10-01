using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TShirt.InventoryApp.WCF.Contract
{
    [DataContract]
    public class CountPlanDetail
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdCountPlan { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public int? TotalCounted { get; set; }
        [DataMember]
        public string BarCode { get; set; }
        [DataMember]
        public string ProductDescription { get; set; }
        [DataMember]
        public int TotalProduct { get; set; }
    }
}