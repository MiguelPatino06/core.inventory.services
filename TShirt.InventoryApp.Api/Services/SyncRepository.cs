using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using TShirt.InventoryApp.Api.Helpers;
using TShirt.InventoryApp.Api.Models;

namespace TShirt.InventoryApp.Api.Services
{
    public class SyncRepository : ISyncRepository
    {
        private DatabaseHelper context = new DatabaseHelper();

        public bool execute(string processName)
        {
            bool result = true;           
            try
            {
                switch (processName)
                {
                    case "IV10300":
                        result = syncIV10300();
                        break;
                    case "POP10100":
                        result = syncPOP10100();
                        break;
                }
            }
            catch (Exception ex)
            {
                result = false;
                Debug.Write(@"Error " + ex.Message);
            }
            return result;
        }

        private bool syncIV10300()
        {
            CultureInfo culture;
            culture = CultureInfo.CreateSpecificCulture("es-PA");
            Thread.CurrentThread.CurrentCulture = culture;
            string query = string.Empty;
            List<string> arrayNotDelete = new List<string>();
            try
            {
                using (TSGVLEntities db = new TSGVLEntities())
                {
                    db.Database.CommandTimeout = 0;

                    using (var conn = new SQLiteConnection(
                        @"Data Source=" + Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                            ConfigurationManager.AppSettings["filePath"])))
                    {
                        conn.Open();

                        DatabaseHelper context = new DatabaseHelper();

                        using (var cmd = new SQLiteCommand(conn))
                        {
                            //IV10300: Conteo de Stock "Master"
                            var _idStt = new[] {1, 2};

                            //List<IV10300> _IV10300 = db.IV10300.Where(a => a.STCKCNTSTTS == 1 && a.STCKCNTSTTS == 2).ToList();
                            List<IV10300> _IV10300 = db.IV10300.Where(a=> _idStt.Contains(a.STCKCNTSTTS)).ToList();
                            List<Models.CountPlan> _list = (from cp in context.CountPlans
                                                            select cp).ToList();                          

                            if (_IV10300 != null)
                            {

                                List<IV10300> iv10300 = new List<IV10300>();


                                #region "Elimina conteo que estan inactivos en GP (estatus 1) y se encuentran en la bd intermedia"

                                foreach (var item in _list)
                                {
                                    if (_IV10300.Where(r => r.STCKCNTSTTS == 1).Any(a => a.STCKCNTID.Trim() == item.Name.Trim()))
                                    {
                                        query = "DELETE FROM countplandetailitem ";
                                        query += " WHERE  countPlanId = '" + item.Id + "'";
                                        removeData(conn, cmd, query);

                                        query = "";

                                        //remove status 0 CountPlandetail
                                        query = "DELETE FROM countplandetail ";
                                        query += " WHERE  countPlanId = '" + item.Id + "'";
                                        removeData(conn, cmd, query);

                                        query = "";

                                        //remove status 0 CountPlan
                                        query = "DELETE FROM countplan WHERE Id = '" + item.Id + "'";
                                        removeData(conn, cmd, query);
                                    }
                                }

                                #endregion


                                #region "Eliminia de la bd los conteos que fueron procesados"

                                //remove status 0 CountPlandetailItem
                                query = "DELETE FROM countplandetailitem ";
                                query += " WHERE  countPlanId IN (SELECT ID FROM countplan WHERE status = '0')";
                                removeData(conn, cmd, query);

                                query = "";

                                //remove status 0 CountPlandetail
                                query = "DELETE FROM countplandetail ";
                                query += " WHERE  countPlanId IN (SELECT ID FROM countplan WHERE status = '0')";
                                removeData(conn, cmd, query);

                                query = "";

                                //remove status 0 CountPlan
                                query = "DELETE FROM countplan WHERE status = '0'"; 
                                removeData(conn, cmd, query);

                                #endregion


                                arrayNotDelete = getLisNotDeleteCountPlan(); //Get countPlan no deleted

                                var myListInsert = string.Empty;

                                if (arrayNotDelete.Count > 0)
                                {
                                    iv10300 = (from cp in _IV10300
                                        where cp.STCKCNTSTTS == 2 && !arrayNotDelete.Contains(cp.STCKCNTID.Trim())
                                        select cp).ToList();
                                }
                                else
                                    iv10300 = _IV10300.Where(a => a.STCKCNTSTTS == 2).ToList();


                                //Get CountPlan from GP and filter to no delete ID in arrayNotDelete
                                using (var transaction = conn.BeginTransaction())
                                {
                                    arrayNotDelete.Clear(); //limpia los conteos
                                    foreach (var item in iv10300)
                                    {
                                        cmd.CommandText =
                                            "INSERT INTO CountPlan (Name, Description, Status, DateCreated, Warehouse) VALUES ('" +
                                            item.STCKCNTID.Trim() + "','" +
                                            item.STCKCNTDSCRPTN.Trim().Replace("'", "''") +
                                            "','" + item.STCKCNTSTTS + "','" + item.LSTCNTDT.ToShortDateString().Trim() +
                                            "','" + item.LOCNCODE + "')";
                                        cmd.ExecuteNonQuery();

                                        arrayNotDelete.Add(item.STCKCNTID.Trim()); //agrega el nombre del conteo para luego agregar los detalles
                                    }

                                    transaction.Commit();
                                }
                            }
                            else
                            {
                                return false;
                                Debug.Write("Fail load IV10300");
                            }


                            
                            if (_list != null)
                            {

                                _list = (from cp in context.CountPlans
                                         where arrayNotDelete.Contains(cp.Name)
                                         select cp).ToList();

                                foreach (Models.CountPlan countPlan in _list)
                                {
                                    var _IV10301 = (from _iv10300 in db.IV10301
                                        join _iv00102 in db.IV00102
                                            on new {_iv10300.ITEMNMBR, _iv10300.LOCNCODE} equals new
                                            {_iv00102.ITEMNMBR, _iv00102.LOCNCODE}
                                        where _iv10300.STCKCNTID.Trim().Equals(countPlan.Name)
                                        select new
                                        {
                                            _ITEMNMBR = _iv10300.ITEMNMBR,
                                            _CAPTUREDQTY = (_iv10300.CAPTUREDQTY - _iv00102.ATYALLOC),
                                            _COUNTDATE = _iv10300.COUNTDATE,
                                            _LOCNCODE = _iv10300.LOCNCODE
                                        }).ToList();


                                    //List <IV10301> _IV10301 = db.IV10301
                                    //    .Where(stl => stl.STCKCNTID.Trim().Equals(countPlan.Name))
                                    //    .ToList();

                                    if (_IV10301 != null)
                                    {
                                        using (var transaction = conn.BeginTransaction())
                                        {
                                            foreach (var item in _IV10301)
                                            {
                                                cmd.CommandText =
                                                    "INSERT INTO CountPlanDetail (CountPlanId, ProductCode, Quantity, DateCreated, Warehouse) VALUES (" +
                                                    countPlan.Id + ",'" + item._ITEMNMBR.Trim() + "'," + item._CAPTUREDQTY +
                                                    ",'" + item._COUNTDATE.ToShortDateString().Trim() + "','" +
                                                    item._LOCNCODE.Trim() + "')";
                                                cmd.ExecuteNonQuery();
                                            }

                                            transaction.Commit();
                                        }
                                    }
                                    else
                                    {
                                        Debug.Write(string.Format("Fail load IV10301 with STCKCNTID: {0}",
                                            countPlan.Name));
                                    }
                                }
                            }
                            else
                            {
                                return false;
                                Debug.Write("Fail load CountPlan");
                            }






                        } //Cmd Sqlite 

                        conn.Close();

                    } //Conn Sqlite
                }
            }
            catch (Exception ex)
            {
                return false;
                Debug.Write(@"Error " + ex.Message);
            }

            return true;
        }

        private bool syncPOP10100()
        {
            CultureInfo culture;
            culture = CultureInfo.CreateSpecificCulture("es-PA");
            Thread.CurrentThread.CurrentCulture = culture;
            string query = string.Empty;
            List<string> arrayNotDelete = new List<string>();
            try
            {
                using (TSGVLEntities db = new TSGVLEntities())
                {
                    db.Database.CommandTimeout = 0;

                    using (var conn = new SQLiteConnection(
                        @"Data Source=" + Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                            ConfigurationManager.AppSettings["filePath"])))
                    {
                        conn.Open();

                        DatabaseHelper context = new DatabaseHelper();

                        using (var cmd = new SQLiteCommand(conn))
                        {

                            List<POP10100> _POP10100 = db.POP10100.Where(p => p.POSTATUS == 2 && p.STATGRP == 1).ToList();

                            if (_POP10100 != null)
                            {

                                query = "DELETE FROM OrderTShirt";
                                removeData(conn, cmd, query);


                                using (var transaction = conn.BeginTransaction())
                                {
                                    foreach (var item in _POP10100)
                                    {
                                        cmd.CommandText =
                                             "INSERT INTO OrderTShirt (Code, ProviderCode, DateCreated) VALUES ('" +
                                             item.PONUMBER.Trim() + "','" + item.VENDORID.Trim() + "','" +
                                             item.DOCDATE.ToShortDateString().Trim() + "')";
                                        cmd.ExecuteNonQuery();
                                    }

                                    transaction.Commit();
                                }
                            }
                            else
                            {
                                return false;
                                Debug.Write("Fail load POP10100");
                            }


                            List<Models.OrderTShirt> _OrderTShirt = context.Orders.ToList();

                            if (_OrderTShirt != null)
                            {

                                 query = "DELETE FROM OrderDetail";
                                removeData(conn, cmd, query);


                                foreach (Models.OrderTShirt orderTShirt in _OrderTShirt)
                                {

                                    List<POP10110> _POP10110 = db.POP10110.Where(p => p.PONUMBER.Trim().Equals(orderTShirt.Code)).ToList();
                                    if (_POP10110 != null)
                                    {
                                        using (var transaction = conn.BeginTransaction())
                                        {
                                            foreach (var item in _POP10110)
                                            {
                                                cmd.CommandText =
                                                    "INSERT INTO OrderDetail (OrderCode, ProductCode, Quantity, Value2, Value3, DateCreated, OrderTShirt_id, InitQuantity) VALUES ('" +
                                                    item.PONUMBER.Trim() + "','" +
                                                    item.ITEMNMBR.Trim().Replace("'", "''") + "'," +
                                                    item.QTYORDER.ToString().Replace(",", ".") + ",'" +
                                                    item.LOCNCODE.Trim() + "','" +
                                                    item.VNDITNUM.Trim() + "','" +
                                                    item.REQDATE.ToShortDateString().Trim() + "'," +
                                                    orderTShirt.Id + "," +
                                                    item.QTYORDER.ToString().Replace(",", ".") + ")";
                                                cmd.ExecuteNonQuery();
                                            }

                                            transaction.Commit();
                                        }
                                    }
                                    else
                                    {
                                        Debug.Write(string.Format("Fail load POP10110 with PONUMBER: {0}", orderTShirt.Code));
                                    }
                                }



                                //var _data = (from ord in context.Orders
                                //    join orddet in context.OrderDetails on ord.Code equals orddet.OrderCode
                                //    where ord.Value1 == "0"
                                //    select orddet.ProductCode).Distinct().ToArray();

                                //var _data = context.Products.ToList().Distinct().Select(a=> a.Code).ToArray();

                                ////List<IV00101> _IV00101 = db.IV00101.Where(a => _data.Contains(a.ITEMNMBR.Trim())).ToList();
                                //var _IV00101 = (from c in db.IV00101
                                //                where  !_data.Contains(c.ITEMNMBR.Trim())
                                //    select c).Distinct().ToList();

                                //if (_IV00101 != null)
                                //{

                                //    //removeData(conn, cmd, "DELETE FROM Product");

                                //    using (var transaction = conn.BeginTransaction())
                                //    {
                                //        foreach (var item in _IV00101)
                                //        {
                                //            cmd.CommandText =
                                //                "INSERT INTO Product (Code, BarCode, Description) VALUES ('" +
                                //                item.ITEMNMBR.Trim() + "','" + item.ITEMNMBR.Trim() + "','" +
                                //                item.ITEMDESC.Trim().Replace("'", "''") + "')";
                                //            cmd.ExecuteNonQuery();
                                //        }

                                //        transaction.Commit();

                                //    }
                                //}

                            }

                        } //Cmd Sqlite 

                        conn.Close();

                    } //Conn Sqlite
                }
            }
            catch (Exception ex)
            {
               // conn.Close();
                return false;
                Debug.Write(@"Error " + ex.Message);
            }

            return true;
        }



        private void removeData(SQLiteConnection conn, SQLiteCommand cmd, string qry)
        {
            using (var transaction = conn.BeginTransaction())
            {
                cmd.CommandText = qry;
                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
        }

        private List<string> getLisNotDeleteCountPlan()
        {
            var qry = "SELECT Name";
            qry += " FROM countplan";
            //qry += " INNER JOIN countPlanDetailItem cpdi ON cp.Id = cpdi.CountPlanId";
            //qry += " GROUP BY cp.Name";

            var list = context.Database.SqlQuery<string>(qry).ToList();

            return list;

        }

    }
}