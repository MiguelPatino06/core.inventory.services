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
    public class ConfigurationRepository : IConfigurationRepository
    {

        DatabaseHelper context = new DatabaseHelper();

        public Configuration Get()
        {
            var configuration = new Configuration();
            try
            {
                configuration = context.Configurations.FirstOrDefault();
            }
            catch (Exception ex)
            {
                configuration = null;
                Debug.Write(@"Error " + ex.Message);
            }
            return configuration;
        }

        public bool Save(Configuration items)
        {
            bool save = true;
            try
            {
                context.Entry(items).State = EntityState.Modified;
                context.SaveChanges();

            }
            catch (Exception ex)
            {
                save = false;
                Debug.Write(@"Error " + ex.Message);
            }
            return save;
        }

    }
}