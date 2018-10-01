using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using System.Xml;

namespace TShirt.InventoryApp.Integration.To.GR
{
     class TransactionAddInventoryMovement : Transaccion, ITransaccion
    {
        private string file;
        private string fname;

        private string server;
        private string db;

        private string store;

        public TransactionAddInventoryMovement(string strcon, string nin, string nout, string hin, string hout, string fil, string srv, string datab, string itbms,string stre)
            : base("TransactionAddInventoryMovement", 11, strcon, nin, nout, hin, hout, itbms)
        {
            file = fil;
            fname = file.Split('\\').Last().Split('/').Last().Replace(nin, "").Replace("\\", "");
            server = srv;
            db = datab;
            store = stre;
        }

        #region Miembros de ITransaccion

        void ITransaccion.Execute()
        {
            // GenerateDocument();
            Models.InventoryAdjustment item = getFromFile(file);
            bool res = false;
            foreach (Models.ItemAvailability ia in item.Items)
            {
                if(ia.Codigoalmacen.Trim().ToUpper() == store.Trim().ToUpper())
                {
                    if(ItemExist(ia.Codigo))
                    {
                        Models.Item itm = GetItem(ia.Codigo.Trim());

                        itm.Qty_Inventory = ia.Cantidad;
                        itm.PricewithoutVAT = ia.Precio_neto;
                        itm.PriceincludingVAT = ia.Precio_con_itbms;
                        itm.VAT = ia.Porcentaje_itbms;

                        res = InsertItem(itm);

                        if (!res)
                            break;
                        
                    }
                }
            }
            
                if (res)
                {
                    try {
                    if (File.Exists(this.HistPathOut + "\\OK_" + fname))
                        File.Delete(this.HistPathOut + "\\OK_" + fname);

                    File.Move(file, this.HistPathOut + "\\OK_" + fname); } catch { }
                }
                else
                {
                    try {
                    if (File.Exists(this.HistPathOut + "\\ERR_" + fname))
                        File.Delete(this.HistPathOut + "\\ERR_" + fname);

                    File.Move(file, this.HistPathOut + "\\ERR_" + fname); } catch { }
                }
        }

        #endregion

       
        private Models.InventoryAdjustment getFromFile(string filename)
        {
            return Models.InventoryAdjustment.LoadFrom(filename);
        }

        private bool ItemExist(string item)
        {
            bool res = false;

            SqlCommand cmd = new SqlCommand("SELECT COUNT([No]) FROM [Item] WHERE [No]=@No", new SqlConnection(this.StringConnection));
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@No", item);

            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                res = ((int)cmd.ExecuteScalar()) > 0;

            }
            catch (Exception ex)
            {
                //MessageBox.Show("TransactionAddInventoryMovement::" + ex.Message);
                IEvent e = new ErrorEvent("", "", "TransactionAddInventoryMovement::" + ex.Message);
                e.Publish();
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                    cmd.Connection.Close();
            }

            return res;
        }

        private Models.Item GetItem(string code)
        {
            Models.Item itm = new Models.Item();

            DataTable itemTable = new DataTable("ITEM");

            SqlCommand cmd = new SqlCommand("SELECT * FROM [Item] WHERE [No]=@No", new SqlConnection(this.StringConnection));
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@No",code.Trim());

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = cmd;

            try
            {
                adapter.Fill(itemTable);
                if (itemTable.Rows.Count > 0)
                {
                    itm.No = (string)itemTable.Rows[0]["No"];
                    itm.Description = (string)itemTable.Rows[0]["Description"];
                    itm.Descripcion2 = (string)itemTable.Rows[0]["Descripcion 2"];
                    itm.PriceincludingVAT = (decimal)itemTable.Rows[0]["Price including VAT"];
                    itm.PricewithoutVAT = (decimal)itemTable.Rows[0]["Price without VAT"];
                    itm.VAT = (decimal)itemTable.Rows[0]["VAT"];
                    itm.Qty_Inventory = (decimal)itemTable.Rows[0]["Qty. Inventory"];
                }
                
            }
            catch
            { }

            return itm;
        }
        private bool InsertItem(Models.Item item)
        {
            bool res = false;
            SqlCommand command;
            command = new SqlCommand("dbo.spUpdateItem", new SqlConnection(this.StringConnection));

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@No", item.No);
            command.Parameters.AddWithValue("@Description", item.Description);
            command.Parameters.AddWithValue("@Descripcion2", item.Descripcion2);
            command.Parameters.AddWithValue("@Qty_Inventory", item.Qty_Inventory);
            command.Parameters.AddWithValue("@Is_Use", 1);
            command.Parameters.AddWithValue("@Billed_Units", 0);
            command.Parameters.AddWithValue("@Qty_Purshase", 0);
            command.Parameters.AddWithValue("@Qty_Sales", 0);
            command.Parameters.AddWithValue("@Amount_Purchases", 0);
            command.Parameters.AddWithValue("@Amount_Sales", 0);
            command.Parameters.AddWithValue("@Qty_Positive_adjustment", 0);
            command.Parameters.AddWithValue("@Qty_Negative_adjustment", 0);
            command.Parameters.AddWithValue("@Amount_Positive_adjustment", 0);
            command.Parameters.AddWithValue("@Amount_Negative_adjustment", 0);
            command.Parameters.AddWithValue("@Qty_in_Order", 0);
            command.Parameters.AddWithValue("@Qty_transfer", 0);
            command.Parameters.AddWithValue("@Transfer_amount", 0);
            command.Parameters.AddWithValue("@Price_without_VAT", item.PricewithoutVAT);
            command.Parameters.AddWithValue("@Price_including_VAT", item.PriceincludingVAT);
            command.Parameters.AddWithValue("@VAT", item.VAT);
            command.Parameters.AddWithValue("@Qty_in_Transit", 0);
            command.Parameters.AddWithValue("@Qty_Sale_Returns", 0);
            command.Parameters.AddWithValue("@Qty_Purchase_Returns", 0);
            command.Parameters.AddWithValue("@Allow_Negative_Inventory", 0);
            command.Parameters.AddWithValue("@Qty_Negative", 0);
            command.Parameters.AddWithValue("@Auto_Code", "");
            command.Parameters.AddWithValue("@Variant", "");
            command.Parameters.AddWithValue("@Weight", 0);
            command.Parameters.AddWithValue("@Base_Unit_of_Measure_Purched", "");
            command.Parameters.AddWithValue("@Base_Unit_of_Measure_Sales", "");
            command.Parameters.AddWithValue("@Barcode", "");
            command.Parameters.AddWithValue("@Allow_Discount", 1);
            command.Parameters.AddWithValue("@Cost_Unity", 0);
            command.Parameters.AddWithValue("@Cost_Standard", "0");
            command.Parameters.AddWithValue("@Last_cost", 0);
            command.Parameters.AddWithValue("@date_last_cost", DateTime.Now);
            command.Parameters.AddWithValue("@Cost_indirect", 0);
            command.Parameters.AddWithValue("@Cost_Ajust", 0);
            command.Parameters.AddWithValue("@Profit", 0);
            command.Parameters.AddWithValue("@Profit_percentage", 0);
            command.Parameters.AddWithValue("@Special_Groups", "");
            command.Parameters.AddWithValue("@Sustitute", "");
            command.Parameters.AddWithValue("@Cross_References", "");
            command.Parameters.AddWithValue("@Additional_text", "");
            command.Parameters.AddWithValue("@Imagen", DBNull.Value).SqlDbType = SqlDbType.Image;
            command.Parameters.AddWithValue("@Translations", "");
            command.Parameters.AddWithValue("@Block", 0);
            command.Parameters.AddWithValue("@No_Vendor", "");
            command.Parameters.AddWithValue("@Reorder_point", 0);
            command.Parameters.AddWithValue("@Qty_Reorder_point", 0);
            command.Parameters.AddWithValue("@Inventory_Security", 0);
            command.Parameters.AddWithValue("@Reserve", 0);
            command.Parameters.AddWithValue("@Lot_size", 0);
            command.Parameters.AddWithValue("@No_Serial", "");
            command.Parameters.AddWithValue("@No_Lot", "");
            command.Parameters.AddWithValue("@Calculation_Due", "");
            command.Parameters.AddWithValue("@Due_Date", DateTime.Now);
            command.Parameters.AddWithValue("@Creation_date", DateTime.Now);
            command.Parameters.AddWithValue("@Created_by_User", "");
            command.Parameters.AddWithValue("@Last_modified_date", DateTime.Now);
            command.Parameters.AddWithValue("@Last_modified_by_the_user", "");
            command.Parameters.AddWithValue("@Warning_shortages", 0);
            command.Parameters.AddWithValue("@Location", "");
            command.Parameters.AddWithValue("@Start_Date_counting", DateTime.Now);
            command.Parameters.AddWithValue("@End_Date_counting", DateTime.Now);
            command.Parameters.AddWithValue("@VAT_Prod_Posting_Group", "01");
            command.Parameters.AddWithValue("@VAT_Bus_Posting_Group", "01");
            command.Parameters.AddWithValue("@Item_Category_Code", "01");
            command.Parameters.AddWithValue("@Product_Group_Code", "01");


            try
            {
                if (command.Connection.State != ConnectionState.Open)
                    command.Connection.Open();

                res = (command.ExecuteNonQuery() >= 0);

            }
            catch (Exception ex)
            {
                res = false;
  
                IEvent e = new ErrorEvent("", "", "TransactionAddInventoryMovement::" + ex.Message);
                e.Publish();

            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed)
                    command.Connection.Close();
            }


            return res;
        }

     
    }
}
