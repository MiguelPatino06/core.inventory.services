using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace TShirt.InventoryApp.Integration.From.GP
{
    public sealed class TransaccionWareHouse :Transaccion, ITransaccion
    {

        public TransaccionWareHouse(string strcon,string nin,string nout, string hin, string hout)
            : base("TransaccionWareHouse", 4, strcon,nin,nout,hin,hout,"")
        { }

        void thread()
        {
            DataTable warehouseData = getAllWarehouseData();

            int total = warehouseData.Rows.Count;

            // esrcibo en el nowin porque van para premiumsoft
            foreach (DataRow row in warehouseData.Rows)
            {
                string FileName = "GP_TDEPO_{0}.xml";


                string LOCNCODE = row["LOCNCODE"].ToString().Trim();
                string LOCNDSCR = row["LOCNDSCR"].ToString().Trim();

                IEvent e = new InfoEvent("", "", "Iniciando la creación del archivo XML '" + String.Format(FileName, LOCNCODE) + "'.");
                e.Publish();
                // xml //
                XmlDocument doc = new XmlDocument();

                /* Lines */
                XmlElement Raiz = doc.CreateElement(string.Empty, "raiz", string.Empty);
                /* Lines */


                try
                {
                    Models.Warehouse ware = new Models.Warehouse();

                    //XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    //XmlElement root = doc.DocumentElement;
                    //doc.InsertBefore(xmlDeclaration, root);
                    //doc.AppendChild(Raiz);
                    //XmlElement element2 = doc.CreateElement(string.Empty, "warehouse", string.Empty);
                    //Raiz.AppendChild(element2);

                    //XmlElement element3 = doc.CreateElement(string.Empty, "codigo", string.Empty);
                    ware.Codigo = LOCNCODE.Trim(); // XmlText text1 = doc.CreateTextNode(LOCNCODE.Trim());
                    //element3.AppendChild(text1);
                    //element2.AppendChild(element3);

                    //XmlElement element4 = doc.CreateElement(string.Empty, "descripcion", string.Empty);
                    ware.Descripcion = LOCNDSCR.Trim(); //XmlText text2 = doc.CreateTextNode(LOCNDSCR.Trim());
                    //element4.AppendChild(text2);
                    //element2.AppendChild(element4);


                    if (this.NowPathIn.ToCharArray().Last() == '/' || this.NowPathIn.ToCharArray().Last() == '\\')
                    {
                        if (!System.IO.File.Exists(this.mNowPathOut + string.Format(FileName, LOCNCODE)))
                            Models.Warehouse.SaveAs(this.mNowPathOut + string.Format(FileName, LOCNCODE), ware); //doc.Save(this.mNowPathOut + string.Format(FileName, LOCNCODE));
                    }
                    else
                    {
                        if (!System.IO.File.Exists(this.mNowPathOut + "/" + string.Format(FileName, LOCNCODE)))
                            Models.Warehouse.SaveAs(this.mNowPathOut + "/" + string.Format(FileName, LOCNCODE), ware); //doc.Save(this.mNowPathOut + "/" + string.Format(FileName, LOCNCODE));
                    }
                    total--;

                    ObserverManager.Instance.addSubject(new ProgressSubject(total, warehouseData.Rows.Count - total));

                    IEvent e2 = new InfoEvent("", "", "El archivo '" + String.Format(FileName, LOCNCODE) + "'. fue creado correctamente");
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

        private DataTable getAllWarehouseData()
        {
            SqlCommand command = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable data = new DataTable("WAREHOUSE");
            command.Connection = new SqlConnection(this.mStringConnection);

            // command.Parameters.AddWithValue("@P", "0");

            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT LOCNCODE, LOCNDSCR FROM IV40700 WHERE LTRIM(RTRIM(UPPER(ADDRESS3)))='TIENDA GALAPAGO'";


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
