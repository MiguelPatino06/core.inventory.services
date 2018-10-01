using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TShirt.InventoryApp.Mobile.Infrastructure
{
    public class EnumTShirt
    {
        public enum OrderStatus
        {
            OPEN = 0,
            CLOSED = 1,
            INPROGRESS = 2,
            CANCELED = 3
        }

        public enum CountEstatus
        {
            SINDIFERENCIA = 0,
            DIFERENCIAMEDIA= 1,
            DIFERENCIAMAYOR = 2
        }

        public enum ProductTransferEstatus
        {
          Pendiente = 0,
          Aprobada = 1,
          Rechazada = 2
        }
  }
}
