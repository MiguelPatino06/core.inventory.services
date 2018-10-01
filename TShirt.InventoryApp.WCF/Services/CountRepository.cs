using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using TShirt.InventoryApp.WCF.Contract;
using TShirt.InventoryApp.WCF.Helpers;
using TShirt.InventoryApp.WCF.Interface;
using TShirt.InventoryApp.WCF.Models.Count;

//using TShirt.InventoryApp.WCF.Models.Count;

namespace TShirt.InventoryApp.WCF.Services
{
    public class CountRepository : ICountRepository
    {

        DatabaseHelper context = new DatabaseHelper();

        public List<Plan> Items { get; private set; }

        public List<Plan> GetAll()
        {
            Items = new List<Plan>();
            
            try
            {
                Items = context.CountPlans.Where(a => a.Status == "1").Select(a=> new Plan()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    Status = a.Status,
                    DateCreated = a.DateCreated,
                    Warehouse = a.Warehouse,
                    Value2 = a.Value2,
                    Value3 = a.Value3,
                    Value4 = a.Value4,
                    Value5 = a.Value5,
                    UserUpdated = a.UserUpdated,
                    DateUpdated = a.DateUpdated
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new FaultException<ExceptionMessage>(new ExceptionMessage(ex.Message));
            }
            return Items;
        }

        public List<Contract.CountPlanDetail> GetById(int id)
        {
            try
            {
                return context.CountPlanDetailExtends.Where(a => a.IdCountPlan == id).Select(b=> new Contract.CountPlanDetail()
                {
                    Id = b.Id,
                    IdCountPlan = b.IdCountPlan,
                    Name = b.Name,
                    Description = b.Description,
                    ProductCode = b.ProductCode,
                    Quantity = b.Quantity,
                    TotalCounted = b.TotalCounted,
                    BarCode = b.BarCode,
                    ProductDescription = b.ProductDescription,
                    TotalProduct = b.TotalProduct                  
                }).ToList();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string Save(List<Contract.CountPlanDetailItem> items)
        {
            var detail = new Models.Count.CountPlanDetailItem();
            try
            {
                if (items == null) return "No Items";
                foreach (var row in items)
                {
                    detail.Id = row.Id;
                    detail.CountPlanId = row.CountPlanId;
                    detail.UserCode = row.UserCode;
                    detail.DateCreated = row.DateCreated;
                    detail.Quantity = row.Quantity;
                    detail.ProductCode = row.ProductCode;
                    detail.Count = row.Count;

                    row.DateCreated = DateTime.Now.ToString("dd/MM/yyyy hh:mm");
                    context.CountPlanDetailItems.Add(detail);
                }
                context.SaveChanges();

                return "OK";
            }
            catch (Exception ex)
            {
                return "Error " + ex.Message;
            }
        }

        public List<Contract.CountPlanDetailItem> GetByPlanAndProduct(int plan, string product)
        {
            return context.CountPlanDetailItems.Where(a => a.CountPlanId == plan && a.ProductCode == product).Select(b=> new Contract.CountPlanDetailItem()
            {
                Count = b.Count,
                CountPlanId = b.CountPlanId,
                DateCreated = b.DateCreated,
                Id = b.Id,
                ProductCode = b.ProductCode,
                Quantity = b.Quantity,
                UserCode = b.UserCode
            }).ToList();
        }

        public bool Update(Contract.CountPlan items)
        {
            try
            {
                //XmlCountPlan xml = new XmlCountPlan();
                var plan = new Models.Count.CountPlan();
                var planDetails = new Models.Count.CountPlanDetail();

                if (items == null) return false;

                int planId = items.Id;

                bool createNewPlan = Convert.ToBoolean(items.Value2); //este campo es asignado true/false si hay 
                                                                      //que crear un nuevo plan cuando el porcentaje es mayor a 3%

                var existingCount = context.CountPlans.First(a => a.Id == items.Id);


                if (existingCount != null)
                {

                    //UPDATE PLAN
                    var planDescription = existingCount.Description;
                    existingCount.Status = "1";
                    existingCount.UserUpdated = items.UserUpdated;
                    existingCount.DateUpdated = DateTime.Now.ToString("dd/MM/yyyy hh:mm");

                    context.Entry(existingCount).State = EntityState.Modified;
                    context.SaveChanges();
                    //UPDATE PLAN



                    if (createNewPlan) //crea plan con productos no contados
                    {
                        var lastOrDefault = context.CountPlans.OrderByDescending(a => a.Id).Select(a => a.Id).First();
                        if (lastOrDefault != null)
                        {
                            int lastPlan = lastOrDefault + 1;
                            plan.Name = "Plan Conteo " + lastPlan;
                            plan.Description = "Productos no contados, " + planDescription;
                            plan.Status = "0";
                            plan.DateCreated = DateTime.Now.ToString("dd/MM/yyyy hh:mm");

                            context.CountPlans.Add(plan);
                            context.SaveChanges();

                            int newPlanId = plan.Id;
                            var details = context.CountPlanDetailExtends.Where(a => a.IdCountPlan == planId).ToList();
                            if (details != null)
                            {
                                foreach (var row in details)
                                {
                                    planDetails.CountPlanId = newPlanId;
                                    planDetails.ProductCode = row.ProductCode;
                                    planDetails.Quantity = row.Quantity - row.TotalProduct;
                                    planDetails.DateCreated = DateTime.Now.ToString("dd/MM/yyyy hh:mm");
                                    planDetails.UserIdCreated = 0;
                                    if (planDetails.Quantity > 0)
                                    {
                                        context.CountPlanDetails.Add(planDetails);
                                        context.SaveChanges();
                                    }
                                }

                            }
                        }
                    }

                    //bool result = xml.Create(planId); //Crea XML


                }
                else
                    return false;


                return true;
            }
            catch (Exception ex)
            {
                Debug.Write("Error " + ex.Message);
                return false;
            }
        }

    }
}