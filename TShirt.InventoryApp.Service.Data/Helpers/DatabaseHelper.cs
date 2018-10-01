using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using TShirt.InventoryApp.Service.Data.Properties;

namespace TShirt.InventoryApp.Service.Data.Helpers
{
    public class DatabaseHelper : DbContext
    {
        public DatabaseHelper() :
            base(new SQLiteConnection()
            {
                ConnectionString =
                    new SQLiteConnectionStringBuilder() {DataSource = Properties.Settings.Default.DataSourcePath, ForeignKeys = true}
                        .ConnectionString
            }, true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<TShirt.InventoryApp.Service.Data.Models.WarehouseProduct> WarehouseProduct { get; set; }
        public DbSet<TShirt.InventoryApp.Service.Data.Models.Product> Products { get; set; }
        public DbSet<TShirt.InventoryApp.Service.Data.Models.Warehouse> Warehouses { get; set; }
        public DbSet<TShirt.InventoryApp.Service.Data.Models.Provider> Providers { get; set; }
        public DbSet<TShirt.InventoryApp.Service.Data.Models.Client> Clients { get; set; }
        public DbSet<TShirt.InventoryApp.Service.Data.Models.CountPlan> CountPlan { get; set; }
        public DbSet<TShirt.InventoryApp.Service.Data.Models.OrderTShirt> OrderTShirt { get; set; }
        public DbSet<TShirt.InventoryApp.Service.Data.Models.OrderReq> OrderReq { get; set; }
  }
}