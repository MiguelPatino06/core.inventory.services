using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace TShirt.InventoryApp.Integration.To.GR
{

    public class TransactionAddCustomer : Transaccion, ITransaccion
    {
        private string file;
        private string fname;

        private string server;
        private string db;

        public TransactionAddCustomer(string strcon, string nin, string nout, string hin, string hout, string fil, string srv, string datab, string itbms)
            : base("TransactionAddCustomer", 11, strcon, nin, nout, hin, hout, itbms)
        {
            file = fil;
            fname = file.Split('\\').Last().Split('/').Last().Replace(nin, "").Replace("\\","");
            server = srv;
            db = datab;
        }

        #region Miembros de ITransaccion

        void ITransaccion.Execute()
        {
           
            Models.Customer cust = getFromFile(file);
            if(cust!=null)
                if(CreateCustomer(cust.No, cust.Name, "", cust.VAT_Registration_No, "", cust.Phone, cust.EMail, cust.Address1, ""))
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

     
        private Models.Customer getFromFile(string filename)
        {
            return Models.Customer.LoadFrom(filename);
        }

        private bool CustomerExist(string code)
        {
            bool res = false;

            SqlCommand cmd = new SqlCommand("SELECT COUNT([No]) FROM [Customer] WHERE [No]=@No", new SqlConnection(this.StringConnection));
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@No", code);

            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                res = ((int)cmd.ExecuteScalar()) > 0;

            }
            catch (Exception ex)
            {
              
                IEvent ev = new ErrorEvent("", "", "TransactionAddCustomer::" + ex.Message);
                ev.Publish();
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                    cmd.Connection.Close();
            }

            return res;
        }


        private bool CreateCustomer(string code,string name,string name2,string ruc, string nfc,string phone,string email,string address1,string address2)
        {
            bool res = false;
            SqlCommand command;

            if (!CustomerExist(code))
                command = new SqlCommand("spInsertCustomer", new SqlConnection(this.StringConnection));
            else
                command = new SqlCommand("spUpdateCustomer", new SqlConnection(this.StringConnection));

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@No",code);
            command.Parameters.AddWithValue("@Name",name);
            command.Parameters.AddWithValue("@Name2",name2);
            command.Parameters.AddWithValue("@CodeAddress","");
            command.Parameters.AddWithValue("@VAT_Registration_No",ruc);
            command.Parameters.AddWithValue("@NCF",nfc);
            command.Parameters.AddWithValue("@Alias","");
            command.Parameters.AddWithValue("@Phone",phone);
            command.Parameters.AddWithValue("@EMail",email);
            command.Parameters.AddWithValue("@Credit_limit",0.00);
            command.Parameters.AddWithValue("@Customer_Invoice_No","");
            command.Parameters.AddWithValue("@Price_Group","01");
            command.Parameters.AddWithValue("@Currency","01");
            command.Parameters.AddWithValue("@Language","01");
            command.Parameters.AddWithValue("@Price_including_VAT",0.00);
            command.Parameters.AddWithValue("@Price_without_VAT",0.00);
            command.Parameters.AddWithValue("@VAT",0.00);
            command.Parameters.AddWithValue("@Salesperson","");
            command.Parameters.AddWithValue("@Discounts_Invoice",0.00);
            command.Parameters.AddWithValue("@Line_Discounts",0.00);
            command.Parameters.AddWithValue("@Reserve",0);
            command.Parameters.AddWithValue("@Payment_terms","");
            command.Parameters.AddWithValue("@Allow_discount",1);
            command.Parameters.AddWithValue("@Block",0);
            command.Parameters.AddWithValue("@VAT_Bus_Posting_Group","01");
            command.Parameters.AddWithValue("@Creation_date",DateTime.Now);
            command.Parameters.AddWithValue("@Created_by_user","");
            command.Parameters.AddWithValue("@Last_modified_date", DateTime.Now);
            command.Parameters.AddWithValue("@Last_modified_by_the_user","");
            command.Parameters.AddWithValue("@Address1",address1);
            command.Parameters.AddWithValue("@Address2",address2);

            try
            {
                if (command.Connection.State != ConnectionState.Open)
                    command.Connection.Open();
                int r = command.ExecuteNonQuery();

                res = ( r >= 0);

            }catch(Exception ex)
            {
                res = false;
                IEvent ev = new ErrorEvent("", "", "TransactionAddSalesPerson::" + ex.Message);
                ev.Publish();
                //MessageBox.Show("TransactionAddSalesPerson::" + ex.Message);
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
