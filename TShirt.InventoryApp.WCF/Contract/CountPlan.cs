using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TShirt.InventoryApp.WCF.Contract
{
    [DataContract]
    public class CountPlan
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string DateCreated { get; set; }
        [DataMember]
        public string Warehouse { get; set; }
        [DataMember]
        public string Value2 { get; set; }
        [DataMember]
        public string Value3 { get; set; }
        [DataMember]
        public string Value4 { get; set; }
        [DataMember]
        public string Value5 { get; set; }
        [DataMember]
        public string UserUpdated { get; set; }
        [DataMember]
        public string DateUpdated { get; set; }
        [DataMember]
        public List<Contract.CountPlanDetail> Details { get; set; }

    }
}