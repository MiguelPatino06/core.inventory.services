using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TShirt.InventoryApp.Services.Mobile.Models
{
    public class UserSessionDataModel
    {
        public int Id { get; set; }
        public string UserFullName { get; set; }

        public DateTime? UserCreationDate { get; set; }


        // public List<RoleModel> RoleData { get; set; }
    }
}
