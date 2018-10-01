using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using TShirt.InventoryApp.Api.Models;


namespace TShirt.InventoryApp.Api.Helpers
{
    public class DatabaseHelper : DbContext
    {
        static DatabaseHelper()
        {
            //System.Data.Entity.Database.SetInitializer<DatabaseHelper>(null);
        }


        public DatabaseHelper() :
            base(new SQLiteConnection()
            {
                ConnectionString =
                    new SQLiteConnectionStringBuilder()
                    {
                        DataSource =
                            Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                ConfigurationManager.AppSettings["filePath"]),
                        ForeignKeys = true
                    }.ConnectionString
            }, true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);

           
        }

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Rol> Rols { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<ProviderOrder> ProviderOrders { get; set; }
        public DbSet<OrderTShirt> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ViewOrder> ViewOrders { get; set; }
        public DbSet<OrderDetailProduct> OrderDetailProducts { get; set; }
        public DbSet<Rct> Rcts { get; set; }
        public DbSet<RctDetail> RctDetails { get; set; }
        public DbSet<CountPlan> CountPlans { get; set; }
        public DbSet<CountPlanDetail> CountPlanDetails { get; set; }
        //public DbSet<ViewCountPlanDetail> CountPlanDetailExtends { get; set; }
        public DbSet<CountPlanDetailItem> CountPlanDetailItems { get; set; }
        public DbSet<OrderReq> OrderReqs { get; set; }
        public DbSet<OrderReqDetail> OrderReqDetails { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<WarehouseProduct> WarehouseProduct { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTransfer> ProductTransfer { get; set; }
        public DbSet<ProductTransferDetail> ProductTransferDetail { get; set; }
        public DbSet<Sample> Samples { get; set; }
        public DbSet<SampleDetail> SampleDetails { get; set; }
        public DbSet<ViewSampleSumProduct> ViewSampleSumProducts { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentDetail> DocumentDetails { get; set; }
        public DbSet<TShirt.InventoryApp.Api.Models.Configuration> Configurations { get; set; }
        public DbSet<Output> Outputs { get; set; }
        public DbSet<OutputDetail> OutputDetails { get; set; }
        public DbSet<ViewOrderReqDetail> ViewOrderReqDetails { get; set; }
        public DbSet<OrderReqDetailProduct> OrderReqDetailProducts { get; set; }
        public DbSet<ChangeProduct> ChangeProducts { get; set; }
    }
}