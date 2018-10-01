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
using TShirt.InventoryApp.WCF.Models.Count;


namespace TShirt.InventoryApp.WCF.Helpers
{
    public class DatabaseHelper : DbContext
    {
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


        public DbSet<CountPlan> CountPlans { get; set; }
        public DbSet<ViewCountPlanDetail> CountPlanDetailExtends { get; set; }
        public DbSet<CountPlanDetailItem> CountPlanDetailItems { get; set; }
        public DbSet<CountPlanDetail> CountPlanDetails { get; set; }


    }
}