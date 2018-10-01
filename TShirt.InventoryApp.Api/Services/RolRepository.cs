using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public class RolRepository : IRolRepository
    {
        DatabaseHelper context = new DatabaseHelper();

        public Rol Add(Rol rol)
        {
            try
            {
                var _date = DateTime.Now.ToString("ddMMyyyyhhmm");
                rol.DateCreated = _date;
                var addRol = context.Rols.Add(rol);
                context.SaveChanges();
                rol.Id = addRol.Id;

                return rol;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public IEnumerable<Rol> GetAll()
        {
            return context.Rols.ToList();
        }

        public Rol GetById(int id)
        {
            try
            {

                Rol _rol = new Rol();

                var user = context.Users.Where(a => a.RolId == id).ToList();
                _rol = context.Rols.FirstOrDefault(a => a.Id == id);
                _rol.RolUser = user;


                return _rol;

            }
            catch (Exception)
            {
                return null;
            }

        }

        public bool Delete(int id)
        {
            bool success = true;
            try
            {
                Rol rol = context.Rols.Find(id);
                context.Rols.Remove(rol);
                context.SaveChanges();
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }

        public bool Update(Rol rol)
        {
            bool success = true;
            try
            {
                var existingRol = context.Rols.First(a => a.Id == rol.Id);
                if (existingRol != null)
                    existingRol.Name = rol.Name;

                context.Entry(existingRol).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }
    }
}