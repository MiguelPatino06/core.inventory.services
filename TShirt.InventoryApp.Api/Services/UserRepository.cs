using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public class UserRepository : IUserRepository
    {
        DatabaseHelper context = new DatabaseHelper();

        public IEnumerable<User> GetAll()
        {
            return context.Users.ToList();
        }

        public User GetByCode(string code, string pass)
        {
            try
            {
                var user = new User();
                var _user = context.Users.FirstOrDefault(a => a.Code.ToLower() == code.ToLower() && a.Password == pass);

                if (_user.Id > 0)
                {

                    var nameRol = context.Rols.First(a => a.Id == _user.RolId).Name;

                    user.Id = _user.Id;
                    user.Code = _user.Code;
                    user.Name = _user.Name;
                    user.Observation = _user.Observation;
                    user.DateCreated = _user.DateCreated;
                    user.RolId = _user.RolId;
                    user.Value1 = _user.Value1;
                    user.Value2 = _user.Value2;
                    user.Value3 = _user.Value3;
                    user.Value4 = _user.Value4;
                    user.Value5 = nameRol; //Name Rol
                    user.Password = _user.Password;
                }
                else
                    user = null;
               
           
                return user;

            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}