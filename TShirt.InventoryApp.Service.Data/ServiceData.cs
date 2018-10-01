using System;
using System.ServiceProcess;
using System.Linq;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Timers;
using TShirt.InventoryApp.Service.Data.Helpers;

namespace TShirt.InventoryApp.Service.Data
{
    public partial class ServiceData : ServiceBase
    {
        public System.Timers.Timer timer;

        public ServiceData()
        {
            InitializeComponent();
            Log("Iniciando Services");
        }

        protected override void OnStart(string[] args)
        {
            timer = new System.Timers.Timer();
            timer.Interval = Properties.Settings.Default.Intervalo;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);

            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Stop();
            Log("PROCESO FINALIZADO.");
            
        }


        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Log("Start Services");

            CultureInfo culture;
            culture = CultureInfo.CreateSpecificCulture("es-PA");
            Thread.CurrentThread.CurrentCulture = culture;
            SQLiteConnection conn = new SQLiteConnection(@"Data Source=" + Properties.Settings.Default.DataSourcePath);
            string query = string.Empty;
            List<string> arrayNotDelete = new List<string>();
            try
            {                
                using (TSGVLEntities db = new TSGVLEntities())
                {
                    db.Database.CommandTimeout = 0;

                    //using (conn = new SQLiteConnection(
                    //    @"Data Source=" + Properties.Settings.Default.DataSourcePath))
                    //{
                        conn.Open();

                        DatabaseHelper context = new DatabaseHelper();

                        using (var cmd = new SQLiteCommand(conn))
                        {
                        
                        #region POP10100: Purchase Order Work (header)
                        List<POP10100> _POP10100 = db.POP10100
                    .Where(p => p.STATGRP == 1)
                    .ToList();

                        if (_POP10100 != null)
                        {
                            Log("POP10100: Purchase Order Work (header) - Count: " + _POP10100.Count);

                            removeData(conn, cmd, "OrderTShirt");

                            removeData(conn, cmd, "OrderReq");

                            using (var transaction = conn.BeginTransaction())
                            {
                                foreach (var item in _POP10100)
                                {
                                    cmd.CommandText =
                                        "INSERT INTO OrderTShirt (Code, ProviderCode, DateCreated) VALUES ('" +
                                        item.PONUMBER.Trim() + "','" + item.VENDORID.Trim() + "','" +
                                        item.DOCDATE.ToShortDateString().Trim() + "')";
                                    cmd.ExecuteNonQuery();

                                    cmd.CommandText =
                                        "INSERT INTO OrderReq (Code, ClientCode, DateCreated,Status) VALUES ('" +
                                        item.PONUMBER.Trim() + "','" + item.VENDNAME.Trim().Replace("'", "''") +
                                        "','" + item.DOCDATE.ToShortDateString().Trim() + "','" + item.POSTATUS +
                                        "')";
                                    cmd.ExecuteNonQuery();
                                }

                                transaction.Commit();
                                Log("Sqlite OrderTShirt Save Changes");
                                Log("Sqlite OrderReq Save Changes");
                            }
                        }
                        else
                        {
                            Log("Fail load POP10100");
                        }
                        #endregion


                        #region POP10110: Purchase Order Line Work (line detail)

                        List<Models.OrderTShirt> _OrderTShirt = context.OrderTShirt.ToList();

                        if (_OrderTShirt != null)
                        {

                            removeData(conn, cmd, "OrderDetail");

                            removeData(conn, cmd, "OrderReqDetail");

                            Log("Sqlite OrderDetail Insert ");
                            Log("Sqlite OrderReqDetail Insert ");

                            foreach (Models.OrderTShirt orderTShirt in _OrderTShirt)
                            {
                                List<POP10110> _POP10110 = db.POP10110
                                    .Where(p => p.PONUMBER.Trim().Equals(orderTShirt.Code))
                                    .ToList();
                                if (_POP10110 != null)
                                {
                                    using (var transaction = conn.BeginTransaction())
                                    {
                                        foreach (var item in _POP10110)
                                        {
                                            cmd.CommandText =
                                                    "INSERT INTO OrderDetail (OrderCode, ProductCode, Quantity, Value2, Value3, DateCreated, OrderTShirt_id, InitQuantity) VALUES ('" +
                                                    item.PONUMBER.Trim() + "','" + item.ITEMNMBR.Trim().Replace("'", "''") + "'," + item.QTYORDER.ToString().Replace(",", ".") + ",'" + item.LOCNCODE.Trim() + "','"  +item.VNDITNUM.Trim() + "','" + item.REQDATE.ToShortDateString().Trim() + "'," + orderTShirt.Id + "," + item.QTYORDER.ToString().Replace(",", ".") + ")";
                                            cmd.ExecuteNonQuery();

                                            cmd.CommandText =
                                                "INSERT INTO OrderReqDetail (OrderReqCode, ProductCode, Quantity) VALUES ('" +
                                                item.PONUMBER.Trim() + "','" +
                                                item.ITEMNMBR.Trim().Replace("'", "''") + "'," + item.QTYORDER + ")";
                                            cmd.ExecuteNonQuery();
                                        }

                                        transaction.Commit();
                                        //Log("Sqlite OrderDetail Save Changes ");
                                        //Log("Sqlite OrderReqDetail Save Changes ");
                                    }
                                }
                                else
                                {
                                    Log(string.Format("Fail load POP10110 with PONUMBER: {0}", orderTShirt.Code));
                                }
                            }
                        }
                        else
                        {
                            Log("Fail load OrderTShirt");
                        }
                        #endregion

 
                        #region IV10300: Conteo de Stock "Master"
                        List<IV10300> _IV10300 = db.IV10300.Where(a => a.STCKCNTSTTS == 2).ToList();

                        if (_IV10300 != null)
                        {
                            Log("IV10300: Conteo de Stock Master - Count: " + _IV10300.Count);

                            query = "DELETE FROM countplan WHERE id NOT IN (SELECT";
                            query += " cp.id ";
                            query += " FROM countplan cp";
                            query += " INNER JOIN countPlanDetailItem cpdi ON cp.Id = cpdi.CountPlanId";
                            query += " GROUP BY cp.Id";
                            query += " ORDER BY cp.Id)";

                            removeDataQry(conn, cmd, query);

                            arrayNotDelete = context.Database.SqlQuery<string>("SELECT Name FROM countplan").ToList();

                            var myListInsert = (from cp in _IV10300
                                                where !arrayNotDelete.Contains(cp.STCKCNTID.Trim())
                                                select cp).ToList();


                            using (var transaction = conn.BeginTransaction())
                            {
                                foreach (var item in myListInsert)
                                {
                                    cmd.CommandText =
                                        "INSERT INTO CountPlan (Name, Description, Status, DateCreated, Warehouse) VALUES ('" +
                                        item.STCKCNTID.Trim() + "','" +
                                        item.STCKCNTDSCRPTN.Trim().Replace("'", "''") + "','" + item.STCKCNTSTTS +
                                        "','" + item.LSTCNTDT.ToShortDateString().Trim() + "','" + item.LOCNCODE +
                                        "')";
                                    cmd.ExecuteNonQuery();
                                }

                                transaction.Commit();
                                //Log("Sqlite CountPlan Save Changes ");
                            }
                        }
                        else
                        {
                            Log("Fail load IV10300");
                        }
                        #endregion
                       

                        #region IV10301: Linea de conteo stock "Detail"

                        List<Models.CountPlan> _list = (from cp in context.CountPlan
                                                        where !arrayNotDelete.Contains(cp.Name.Trim())
                                                        select cp).ToList();
                        if (_list != null)
                        {
                            //

                            query = "DELETE FROM countplanDetail WHERE Countplanid NOT IN (SELECT";
                            query += " cp.id ";
                            query += " FROM countplan cp";
                            query += " INNER JOIN countPlanDetailItem cpdi ON cp.Id = cpdi.CountPlanId";
                            query += " GROUP BY cp.Id";
                            query += " ORDER BY cp.Id)";

                            //removeData(conn, cmd, "CountPlanDetail");
                            removeDataQry(conn, cmd, query);

                            Log("Sqlite CountPlanDetail INSERT");
                            foreach (Models.CountPlan countPlan in _list)
                            {
                                List<IV10301> _IV10301 = db.IV10301
                                    .Where(stl => stl.STCKCNTID.Trim().Equals(countPlan.Name))
                                    .ToList();
                                if (_IV10301 != null)
                                {
                                    using (var transaction = conn.BeginTransaction())
                                    {
                                        foreach (var item in _IV10301)
                                        {
                                            cmd.CommandText =
                                                "INSERT INTO CountPlanDetail (CountPlanId, ProductCode, Quantity, DateCreated, Warehouse) VALUES (" +
                                                countPlan.Id + ",'" + item.ITEMNMBR.Trim() + "'," + item.CAPTUREDQTY +
                                                ",'" + item.COUNTDATE.ToShortDateString().Trim() + "','" +
                                                item.LOCNCODE.Trim() + "')";
                                            cmd.ExecuteNonQuery();
                                        }

                                        transaction.Commit();
                                        // Log("Sqlite CountPlanDetail Save Changes ");
                                    }
                                }
                                else
                                {
                                    Log(string.Format("Fail load IV10301 with STCKCNTID: {0}", countPlan.Name));
                                }
                            }
                        }
                        else
                        {
                            Log("Fail load CountPlan");
                        }
                        #endregion

                        #region IV40700: Configuración de Sitio 

                        //List<IV40700> _IV40700 = db.IV40700.ToList();

                        //if (_IV40700 != null)
                        //{
                        //    Log("IV40700: Configuración de Sitio - Count: " + _IV40700.Count);

                        //    removeData(conn, cmd, "Warehouse");

                        //    using (var transaction = conn.BeginTransaction())
                        //    {
                        //        foreach (var item in _IV40700)
                        //        {
                        //            cmd.CommandText =
                        //                "INSERT INTO Warehouse (Code, Name) VALUES ('" + item.LOCNCODE.Trim() +
                        //                "','" + item.LOCNDSCR.Trim() + "')";
                        //            cmd.ExecuteNonQuery();
                        //        }

                        //        transaction.Commit();
                        //    }
                        //    Log("Sqlite Warehouse Save Changes ");
                        //}
                        //else
                        //{
                        //    Log("Fail load IV40700");
                        //}
                        #endregion


                        #region ItemQuantities: Vista ItemQuantities

                        //var _ItemQuantities = db.ItemQuantities
                        //.Where(iq => iq.Código_de_ubicación.Length > 0)
                        //.Where(iq => !iq.Descripción_artículo.Contains("NO USAR"))
                        //.Select(item => new
                        //{
                        //    Código_de_ubicación = item.Código_de_ubicación,
                        //    Número_de_artículo = item.Número_de_artículo,
                        //    Cant__disponible = item.Cant__disponible
                        //})
                        //.ToList();

                        //if (_ItemQuantities != null)
                        //{

                        //    Log("_ItemQuantities: Vista ItemQuantities - Count: " + _ItemQuantities.Count);

                        //    removeData(conn, cmd, "WarehouseProduct");

                        //    using (var transaction = conn.BeginTransaction())
                        //    {
                        //        foreach (var item in _ItemQuantities)
                        //        {
                        //            cmd.CommandText =
                        //                "INSERT INTO WarehouseProduct (ProductCode, WarehouseCode, Quantity) VALUES ('" +
                        //                item.Número_de_artículo.Trim() + "','" + item.Código_de_ubicación.Trim() +
                        //                "'," + Decimal.ToDouble(item.Cant__disponible.GetValueOrDefault()) + ")";
                        //            cmd.ExecuteNonQuery();
                        //        }

                        //        transaction.Commit();
                        //        Log("Sqlite WarehouseProduct Save Changes ");
                        //    }
                        //}
                        //else
                        //{
                        //    Log("Fail load ItemQuantities");
                        //} 
                        #endregion


                        #region IV00101: Maestro de Articulos

                        List<IV00101> _IV00101 = db.IV00101
                    .Where(a => !a.ITEMDESC.Contains("NO USAR"))
                    .ToList();

                        if (_IV00101 != null)
                        {
                            Log("IV00101: Maestro de Articulos - Count: " + _IV00101.Count);

                            removeData(conn, cmd, "Product");

                            using (var transaction = conn.BeginTransaction())
                            {
                                foreach (var item in _IV00101)
                                {
                                    cmd.CommandText =
                                        "INSERT INTO Product (Code, BarCode, Description) VALUES ('" +
                                        item.ITEMNMBR.Trim() + "','" + item.ITEMNMBR.Trim() + "','" +
                                        item.ITEMDESC.Trim().Replace("'", "''") + "')";
                                    cmd.ExecuteNonQuery();
                                }

                                transaction.Commit();
                                Log("Sqlite Product Save Changes ");
                            }
                        }
                        else
                        {
                            Log("Fail load IV00101");
                        }
                        #endregion


                        #region PM00200:  Archivo maestro de proveedores

                        List<PM00200> _PM00200 = db.PM00200.ToList();

                        if (_PM00200 != null)
                        {
                            Log("PM00200: Archivo maestro de proveedores - Count: " + _PM00200.Count);

                            removeData(conn, cmd, "Provider");

                            using (var transaction = conn.BeginTransaction())
                            {
                                foreach (var item in _PM00200)
                                {
                                    cmd.CommandText =
                                        "INSERT INTO Provider (Code, BarCode, Name, Description) VALUES ('" +
                                        item.VENDORID.Trim() + "','" + item.VENDORID.Trim() + "','" +
                                        item.VENDNAME.Trim().Replace("'", "''") + "','" +
                                        item.VNDCHKNM.Trim().Replace("'", "''") + "')";
                                    cmd.ExecuteNonQuery();
                                }

                                transaction.Commit();
                                Log("Sqlite Provider Save Changes ");
                            }
                        }
                        else
                        {
                            Log("Fail load PM00200");
                        }
                        #endregion

                        
                        #region RM00101: CC Maestro de clientes

                        List<RM00101> _RM00101 = db.RM00101.ToList();

                        if (_RM00101 != null)
                        {
                            Log("RM00101: CC Maestro de clientes - Count: " + _RM00101.Count);

                            removeData(conn, cmd, "Client");

                            using (var transaction = conn.BeginTransaction())
                            {
                                foreach (var item in _RM00101)
                                {
                                    cmd.CommandText =
                                        "INSERT INTO Client (Code, Name, Description) VALUES ('" +
                                        item.CUSTNMBR.Trim() + "','" + item.CUSTNAME.Trim().Replace("'", "''") +
                                        "','" + item.STMTNAME.Trim().Replace("'", "''") + "')";
                                    cmd.ExecuteNonQuery();
                                }

                                transaction.Commit();
                                Log("Sqlite Client Save Changes ");
                            }
                        }
                        else
                        {
                            Log("Fail load RM00101");
                        } 
                        #endregion

                    } //Cmd Sqlite 
                    Log("FIN PROCESO ");

                    conn.Close();

                    //} //Conn Sqlite
                }
            }
            catch (Exception ex)
            {
                conn.Close();
                Log("Error Message" + ex.Message);
                Log("Error StackTrace" + ex.StackTrace);
                Log("Error InnerException" + ex.InnerException.Message);
            }
        }

        private void removeData(SQLiteConnection conn, SQLiteCommand cmd, string table)
        {

            using (var transaction = conn.BeginTransaction())
            {
                Log(string.Format("{0} - Sqlite Removing Rows", table));
                cmd.CommandText = string.Format("DELETE FROM {0}", table);
                cmd.ExecuteNonQuery();
                transaction.Commit();
                Log(string.Format("{0} - Sqlite Removing Rows - Finished", table));
            }
        }

        private void removeDataQry(SQLiteConnection conn, SQLiteCommand cmd, string qry)
        {

            using (var transaction = conn.BeginTransaction())
            {
                Log(qry);
                cmd.CommandText =qry;
                cmd.ExecuteNonQuery();
                transaction.Commit();

            }
        }


        public void Log(string Mensaje, Exception error = null)
        {
            if (error == null)
                log4net.LogManager.GetLogger("root").Info(Mensaje);
            else
            {
                log4net.LogManager.GetLogger("root").Error(Mensaje, error);
                Console.WriteLine(error.Message);
            }
            Console.WriteLine(Mensaje);
        }
    }
}
