using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace TShirt.InventoryApp.WCF.Contract
{
    [DataContract]
    public class CountPlanDetailItem
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CountPlanId { get; set; }
        [DataMember]
        public string UserCode { get; set; }
        [DataMember]
        public string DateCreated { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public int Count { get; set; }
    }

}