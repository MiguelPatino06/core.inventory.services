using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace TShirt.InventoryApp.Integration.From.GP
{
    public sealed class TransaccionCustomer :Transaccion, ITransaccion
    {

        public TransaccionCustomer(string strcon,string nin,string nout, string hin, string hout)
            : base("TransaccionCustomer", 4, strcon,nin,nout,hin,hout,"")
        { }

        void thread()
        {
            DataTable customerData = getAllCustomers();

            int total = customerData.Rows.Count;

            // esrcibo en el nowin porque van para premiumsoft
            foreach (DataRow row in customerData.Rows)
            {
                string FileName = "GP_CUST_{0}.xml";



                string CUSTNMBR = row["CUSTNMBR"].ToString().Trim();
                IEvent e = new InfoEvent("", "", "Iniciando la creación del archivo XML '" + String.Format(FileName, CUSTNMBR) + "'.");
                e.Publish();
                // xml //
                XmlDocument doc = new XmlDocument();

                /* Lines */
                XmlElement Raiz = doc.CreateElement(string.Empty, "raiz", string.Empty);
                /* Lines */


                try
                {
                    Models.Customer customer = new Models.Customer();

                    customer.No = row["CUSTNMBR"].ToString().Trim();
                    customer.Name = row["CUSTNAME"].ToString().Trim();
                    customer.VAT_Registration_No = row["TXRGNNUM"].ToString().Trim();
                    customer.Phone = row["PHONE1"].ToString().Trim();
                    customer.EMail = "";
                    customer.VAT = 7;
                    customer.VAT_Bus_Posting_Group = "01";
                    customer.Creation_date = "1900-01-01 00:00:00.00";
                    customer.Last_modified_date = "1991-03-15 00:00:00";
                    customer.Address1 = row["ADDRESS1"].ToString().Trim();

                    if (this.NowPathIn.ToCharArray().Last() == '/' || this.NowPathIn.ToCharArray().Last() == '\\')
                    {
                        if (!System.IO.File.Exists(this.mNowPathOut + string.Format(FileName, CUSTNMBR)))
                            Models.Customer.SaveAs(this.mNowPathOut + string.Format(FileName, CUSTNMBR), customer); //doc.Save(this.mNowPathOut + string.Format(FileName, LOCNCODE));
                    }
                    else
                    {
                        if (!System.IO.File.Exists(this.mNowPathOut + "/" + string.Format(FileName, CUSTNMBR)))
                            Models.Customer.SaveAs(this.mNowPathOut + "/" + string.Format(FileName, CUSTNMBR), customer); //doc.Save(this.mNowPathOut + "/" + string.Format(FileName, LOCNCODE));
                    }
                    total--;

                    ObserverManager.Instance.addSubject(new ProgressSubject(total, customerData.Rows.Count - total));

                    IEvent e2 = new InfoEvent("", "", "El archivo '" + String.Format(FileName, CUSTNMBR) + "'. fue creado correctamente");
                    e2.Publish();
                }
                catch (Exception ex)
                {
                    IEvent err = new ErrorEvent("", "", "No pudo crear el archivo xml correctamente. Mensaje: " + ex.Message);
                    err.Publish();
                }
            }
            ObserverManager.Instance.addSubject(new ProgressSubject(0, 0));
        }

        void ITransaccion.Execute()
        {
            System.Threading.Thread th = new System.Threading.Thread(thread);
            th.Start();
        }

        private DataTable getAllCustomers()
        {
            SqlCommand command = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable data = new DataTable("CUSTOMER");
            command.Connection = new SqlConnection(this.mStringConnection);

            // command.Parameters.AddWithValue("@P", "0");

            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM RM00101 WHERE CUSTNMBR LIKE 'CL9000%'";


            try
            {
                adapter.SelectCommand = command;
                adapter.Fill(data);
            }
            catch (Exception ex)
            {
                IEvent e = new ErrorEvent("", "", ex.Message);
                e.Publish();
            }
            finally
            {
                //nothing
            }
            return data;

        }


    }
}
