using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models;
using TShirt.InventoryApp.Api.Models.Xml;

namespace TShirt.InventoryApp.Api.Services
{
    public class CountRepository : ICountRepository
    {
        DatabaseHelper context = new DatabaseHelper();


        public IEnumerable<CountPlan> GetAll()
        {
            try
            {
                return context.CountPlans.Where(a => a.Status == "2").ToList();
            }
            catch (Exception ex)
            {
                
                throw;
            }

        }

        public List<ViewCountPlanDetail> GetById(int id)
        {
            try
            {
                var qry = "SELECT CPD.Id AS Id,";
                qry += "CP.Id AS IdCountPlan,";
                qry += "CP.Name,";
                qry += "CP.Description,";
                qry += "CPD.ProductCode,";
                qry += "CPD.Quantity,";
                qry += "CPD.TotalCounted,";
                qry += "P.Barcode,";
                qry += "P.Description AS ProductDescription,";
                qry += "CASE  WHEN SUM(CPDI.Quantity) > 0 THEN  SUM(CPDI.Quantity) ELSE 0 END AS TotalProduct,";
                qry += "CP.Warehouse ";
                qry += " FROM CountPlan CP INNER JOIN";
                qry += " CountPlanDetail CPD ON  CP.Id = CPD.CountPlanId INNER JOIN";
                qry += " Product P ON CPD.ProductCode = P.Code LEFT OUTER JOIN";
                qry += " CountPlanDetailItem CPDI ON ((CPD.CountPlanId = CPDI.CountPlanId) AND (P.Code = CPDI.ProductCode ))";
                qry += " WHERE CP.Id = '" + id  +"' " ;
                qry += " GROUP BY CP.Id, CP.Name, CP.Description, CPD.ProductCode, CPD.Quantity,  CPD.TotalCounted, P.Barcode, P.Description";

                var list = context.Database.SqlQuery<ViewCountPlanDetail>(qry).ToList();

               
                return list;
                //return context.CountPlanDetailExtends.Where(a => a.IdCountPlan == id).ToList();

            }
            catch (Exception ex)
            {               
                throw;
            }
        }


        public ViewCountPlanDetailPage GetByIdPage(int id)
        {
            try
            {
                var qry = "SELECT CPD.Id AS Id,";
                qry += "CP.Id AS IdCountPlan,";
                qry += "CP.Name,";
                qry += "CP.Description,";
                qry += "CPD.ProductCode,";
                qry += "CPD.Quantity,";
                qry += "CPD.TotalCounted,";
                qry += "P.Barcode,";
                qry += "P.Description AS ProductDescription,";
                qry += "CASE  WHEN SUM(CPDI.Quantity) > 0 THEN  SUM(CPDI.Quantity) ELSE 0 END AS TotalProduct,";
                qry += "CP.Warehouse ";
                qry += " FROM CountPlan CP INNER JOIN";
                qry += " CountPlanDetail CPD ON  CP.Id = CPD.CountPlanId INNER JOIN";
                qry += " Product P ON CPD.ProductCode = P.Code LEFT OUTER JOIN";
                qry += " CountPlanDetailItem CPDI ON ((CPD.CountPlanId = CPDI.CountPlanId) AND (P.Code = CPDI.ProductCode ))";
                qry += " WHERE CP.Id = '" + id + "' ";
                qry += " GROUP BY CP.Id, CP.Name, CP.Description, CPD.ProductCode, CPD.Quantity,  CPD.TotalCounted, P.Barcode, P.Description";

                var list = context.Database.SqlQuery<ListItems>(qry).ToList();

                              
               return new ViewCountPlanDetailPage()
               {
                   Listrows = list.Take(10).ToList(),
                   Count = list.Count()
               };
                //return context.CountPlanDetailExtends.Where(a => a.IdCountPlan == id).ToList();

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ViewCountPlanDetailPage GetByIdPageTakeSkip(int id, int _take, int _skip)
        {
            try
            {
                var qry = "SELECT CPD.Id AS Id,";
                qry += "CP.Id AS IdCountPlan,";
                qry += "CP.Name,";
                qry += "CP.Description,";
                qry += "CPD.ProductCode,";
                qry += "CPD.Quantity,";
                qry += "CPD.TotalCounted,";
                qry += "P.Barcode,";
                qry += "P.Description AS ProductDescription,";
                qry += "CASE  WHEN SUM(CPDI.Quantity) > 0 THEN  SUM(CPDI.Quantity) ELSE 0 END AS TotalProduct,";
                qry += "CP.Warehouse ";
                qry += " FROM CountPlan CP INNER JOIN";
                qry += " CountPlanDetail CPD ON  CP.Id = CPD.CountPlanId INNER JOIN";
                qry += " Product P ON CPD.ProductCode = P.Code LEFT OUTER JOIN";
                qry += " CountPlanDetailItem CPDI ON ((CPD.CountPlanId = CPDI.CountPlanId) AND (P.Code = CPDI.ProductCode ))";
                qry += " WHERE CP.Id = '" + id + "' ";
                qry += " GROUP BY CP.Id, CP.Name, CP.Description, CPD.ProductCode, CPD.Quantity,  CPD.TotalCounted, P.Barcode, P.Description";

                var list = context.Database.SqlQuery<ListItems>(qry).ToList();

                return new ViewCountPlanDetailPage()
                {
                    Listrows = list.Skip(_skip).Take(_take).ToList(),
                    Count = list.Count()
                };
                //return context.CountPlanDetailExtends.Where(a => a.IdCountPlan == id).ToList();

            }
            catch (Exception ex)
            {

                throw;
            }
        }



        public string Save(List<CountPlanDetailItem> items)
        {
            try
            {
                if (items == null) return "No Items";
                foreach (var row in items)
                {
                    row.DateCreated = DateTime.Now.ToString("dd/MM/yyyy hh:mm");
                    context.CountPlanDetailItems.Add(row);

                }
                context.SaveChanges();

                return "OK";


            }
            catch (Exception ex)
            {

                return "Error " + ex.Message;
            }
        }

        public List<ViewCountPlanDetailItem> GetByPlanAndProduct(int plan, string product)
        {

            List<ViewCountPlanDetailItem> items = new List<ViewCountPlanDetailItem>();
            var details = new ViewCountPlanDetailItem();

           
            var qry ="SELECT CP.Id, CP.CountPlanId, CP.UserCode, CP.DateCreated, CP.Quantity, CP.ProductCode, CP.Count, PR.Description ";
            qry += " FROM countPlanDetailItem CP INNER JOIN ";
            qry += " Product PR ON  CP.ProductCode = PR.Code";
            qry += " WHERE CP.CountPlanId = '" + plan +"' AND CP.ProductCode = '" + product + "'";

            var list = context.Database.SqlQuery<ViewCountPlanDetailItem>(qry).ToList();


            if (!list.Any())
            {
                list = new List<ViewCountPlanDetailItem>();
                //var existProductAndPlan = context.CountPlanDetails.Any(a => a.CountPlanId == plan && a.ProductCode == product);

                var descriptionProduct = (from cpd in context.CountPlanDetails
                    join pro in context.Products on cpd.ProductCode equals pro.Code
                                         where  cpd.CountPlanId == plan && cpd.ProductCode == product
                                         select new
                                        {
                                            _DescriptionProduct = pro.Description
                                        }).FirstOrDefault();

                
                //context.Products.FirstOrDefault(a => a.Code == product);
                if (descriptionProduct != null)
                {
                    list.Add(new ViewCountPlanDetailItem()
                    {
                        CountPlanId = plan,
                        ProductCode = product,
                        Quantity = 0,
                        Description = descriptionProduct._DescriptionProduct
                    });
                }
                else
                {
                    list = null;
                }

            }
        
           

            

            return list;

        }


        public bool Update(CountPlan items)
        {
            try
            {
                //var _lote = items.Value3; //Lote
                XmlCountPlan xml = new XmlCountPlan();
                var plan = new CountPlan();
                var planDetails = new CountPlanDetail();

                if (items == null) return false;

                int planId = items.Id;

                bool createNewPlan = Convert.ToBoolean(items.Value2); //este campo es asignado true/false si hay 
                //que crear un nuevo plan cuando el porcentaje es mayor a 3%

                var existingCount = context.CountPlans.First(a => a.Id == items.Id);


                if (existingCount != null)
                {

                    //UPDATE PLAN
                    var planName = existingCount.Name;
                    var planDescription = existingCount.Description;
                    var warehouse = existingCount.Warehouse;
                    existingCount.Status = "0";
                    //existingCount.Value3 = _lote;
                    existingCount.UserUpdated = items.UserUpdated;
                    existingCount.DateUpdated = DateTime.Now.ToString("dd/MM/yyyy hh:mm");

                    context.Entry(existingCount).State = EntityState.Modified;
                    context.SaveChanges();
                    //UPDATE PLAN


                    #region "CREATE NUEVO PLAN"                  


                    if (createNewPlan) //crea plan con productos no contados
                    {
                        var lastOrDefault = context.CountPlans.OrderByDescending(a => a.Id).Select(a => a.Id).First();
                        if (lastOrDefault != null)
                        {
                            //int lastPlan = lastOrDefault + 1;
                            plan.Warehouse = warehouse;
                            plan.Name = "Plan Conteo " + planName;
                            plan.Description = "Productos no contados, " + planDescription;
                            plan.Status = "2";
                            plan.DateCreated = DateTime.Now.ToString("dd/MM/yyyy hh:mm");

                            context.CountPlans.Add(plan);
                            context.SaveChanges();

                            int newPlanId = plan.Id;




                            //        //CODIGO NUEVO 14-1017
                            var qry = "SELECT CPD.Id AS Id,";
                            qry += "CP.Id AS IdCountPlan,";
                            qry += "CP.Name,";
                            qry += "CP.Description,";
                            qry += "CPD.ProductCode,";
                            qry += "CPD.Quantity,";
                            qry += "CPD.TotalCounted,";
                            qry += "P.Barcode,";
                            qry += "P.Description AS ProductDescription,";
                            qry +=
                                "CASE  WHEN SUM(CPDI.Quantity) > 0 THEN  SUM(CPDI.Quantity) ELSE 0 END AS TotalProduct,";
                            qry += "CP.Warehouse ";
                            qry += " FROM CountPlan CP INNER JOIN";
                            qry += " CountPlanDetail CPD ON  CP.Id = CPD.CountPlanId INNER JOIN";
                            qry += " Product P ON CPD.ProductCode = P.Code LEFT OUTER JOIN";
                            qry +=
                                " CountPlanDetailItem CPDI ON ((CPD.CountPlanId = CPDI.CountPlanId) AND (P.Code = CPDI.ProductCode ))";
                            qry += " WHERE CP.Id = '" + planId + "' ";
                            qry +=
                                " GROUP BY CP.Id, CP.Name, CP.Description, CPD.ProductCode, CPD.Quantity,  CPD.TotalCounted, P.Barcode, P.Description";

                            var details = context.Database.SqlQuery<ViewCountPlanDetail>(qry).ToList();
                            //FIN NUEVO 14-1017


                            //var details = context.CountPlanDetailExtends.Where(a => a.IdCountPlan == planId).ToList();

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

                    #endregion

                    bool result = xml.Create(planId); //Crea XML


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