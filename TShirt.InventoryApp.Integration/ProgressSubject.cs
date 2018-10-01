using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TShirt.InventoryApp.Integration
{
    class ProgressSubject: ISubject
    {
        #region Miembros de ISubject

        int tot;
        int prg;

        public ProgressSubject(int total, int prog)
        {
            tot = total;
            prg = prog;
        }

        string ISubject.toString()
        {
            return tot.ToString() + "/" + prg.ToString();
        }

        #endregion
    }
}
