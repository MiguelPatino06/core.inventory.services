using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public class SampleRepository : ISampleRepository
    {
        DatabaseHelper context = new DatabaseHelper();


        public Sample GetById(int id)
        {
            Sample items;

            try
            {
                items = context.Samples.FirstOrDefault(a => a.Id == id);

                var details = (from det in context.SampleDetails
                    join pro in context.Products on det.ProductCode equals pro.Code into projoin
                    from pro in projoin.DefaultIfEmpty()
                    where det.SampleId == id
                    select new SampleDetailExtend()
                    {
                        Id = det.Id,
                        SampleId = det.SampleId,
                        ProductCode = det.ProductCode,
                        Quantity = det.Quantity,
                        User = det.User,
                        DateCreated = det.DateCreated,
                        Value1 = det.Value1,
                        Value2 = det.Value2,
                        Value3 = det.Value3,
                        Value4 = det.Value4,
                        Value5 = det.Value5,
                        ProductName = pro.Description
                    }).ToList();

                if (items != null) items.Details = details;
            }
            catch (Exception ex)
            {
                items = null;
                Debug.Write(@"Error " + ex.Message);
            }
            return items;

        }

        public int Save(Sample items)
        {
            int id = 0;
            try
            {
                var user = items.User;
                items.DateCreated = DateTime.Now.ToString("dd/MM/yyyy hh:mm");
                context.Samples.Add(items);
                context.SaveChanges();

                id = items.Id;

                if (id > 0)
                {
                    var detail = new SampleDetail();
                    foreach (var row in items.Details)
                    {
                        detail.SampleId = id;
                        detail.ProductCode = row.ProductCode;
                        detail.Quantity = row.Quantity;
                        detail.User = user;
                        detail.DateCreated = row.DateCreated;
                        detail.Value1 = row.Value1;
                        detail.Value2 = row.Value2;
                        detail.Value3 = row.Value3;
                        detail.Value4 = row.Value4;
                        detail.Value5 = row.Value5;

                        context.SampleDetails.Add(detail);
                        context.SaveChanges();                        
                    }
                }
            }
            catch (Exception ex)
            {
                id = 0;
                Debug.Write(@"Error " + ex.Message);
            }
            return id;
        }

        public bool Update(Sample items)
        {
            bool result = false;
            try
            {
                context.Entry(items).State = EntityState.Modified;
                context.SaveChanges();

                foreach (var row in items.Details)
                {
                    var details = new SampleDetail();

                    details.SampleId = row.SampleId;
                    details.ProductCode = row.ProductCode;
                    details.Quantity = row.Quantity;
                    details.User = row.User;
                    details.DateCreated = DateTime.Now.ToString("dd/MM/yyyy hh:mm");
                    details.Value1 = row.Value1;
                    details.Value2 = row.Value2;
                    details.Value3 = row.Value3;
                    details.Value4 = row.Value4;
                    details.Value5 = row.Value5;
                    //esta variable determina la accion a ejecutar en el registro
                    //null or empty  = Nothing
                    //            1  = Add
                    //            2  = Update
                    //            3  = Delete

                    int status = (string.IsNullOrEmpty(row.Value1)) ? 0 : int.Parse(row.Value1);
                    
                    switch (status)
                    {
                        case 1:
                            AddDetail(details);
                            break;
                        case 2:
                            details.Id = row.Id;
                            UpdateDetail(details);
                            break;
                        case 3:
                            details.Id = row.Id;
                            DeleteDetail(details);
                            break;

                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                Debug.Write(@"Error " + ex.Message);
            }
            return result;
        }


        public List<ViewSampleSumProduct> GetList()
        {
            return context.ViewSampleSumProducts.ToList();
        }

        private void AddDetail(SampleDetail detail)
        {
            try
            {
                context.SampleDetails.Add(detail);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                               throw;
            }
        }
        private void UpdateDetail(SampleDetail detail)
        {
            try
            {
                context.Entry(detail).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void DeleteDetail(SampleDetail detail)
        {
            try
            {

                context.SampleDetails.Remove(detail);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}