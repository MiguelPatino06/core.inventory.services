using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Data.SqlClient;

namespace TShirt.InventoryApp.Integration.From.GP
{
    class TransaccionOfferts : Transaccion, ITransaccion
    {
        private string mItmbs;

        public TransaccionOfferts(string strcon, string nin, string nout, string hin, string hout, string itbms)
            : base("TransaccionOfferts", 4, strcon, nin, nout, hin, hout,itbms)
        {
            mItmbs = itbms;
        }

        void thread()
        {
            DataTable Transactions = getAllPricesLevels();
            string prevDoc = "";
            int total = Transactions.Rows.Count;


            // esrcibo en el nowin porque van para premiumsoft
            foreach (DataRow row in Transactions.Rows)
            {
                if (prevDoc != row["PRCLEVEL"].ToString().Trim())
                {

                    DataTable TransactionDetail = getPriceLeveData(row["PRCLEVEL"].ToString().Trim());
                    string ADJUST = row["PRCLEVEL"].ToString().Trim();

                    string FileName = "GP_OFER_{0}.xml";

                    IEvent e = new InfoEvent("", "", "Iniciando la creación del archivo XML '" + String.Format(FileName, ADJUST) + "'.");
                    e.Publish();
                    // xml //
                    XmlDocument doc = new XmlDocument();

                    XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    XmlElement root = doc.DocumentElement;
                    doc.InsertBefore(xmlDeclaration, root);

                    /* Lines */
                    XmlElement Lines = doc.CreateElement(string.Empty, "raiz", string.Empty);
                    /* Lines */

                    Models.Oferts offerts = new Models.Oferts();

                    foreach (DataRow det in TransactionDetail.Rows)
                    {
                        Models.PriceList pl = new Models.PriceList();


                        string DOCNUMBR = det["PRCLEVEL"].ToString().Trim();
                        //string USERID = "UNKNOWN";

                        string DOCDATE = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                        string TIME = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":00";
                        string ITEMNMBR = det["ITEMNMBR"].ToString().Trim();

                        try
                        {

                            XmlElement Item = doc.CreateElement(string.Empty, "lista_de_precio", string.Empty);

                            /* document */
                            //XmlElement document = doc.CreateElement(string.Empty, "codigoalmacen", string.Empty);
                            pl.Almacen = det["PRCLEVEL"].ToString().Trim(); // XmlText document_text = doc.CreateTextNode(det["PRCLEVEL"].ToString().Trim());
                            //document.AppendChild(document_text);
                            //Item.AppendChild(document);
                            /* document */

                            /* realizador */
                            //XmlElement realizador = doc.CreateElement(string.Empty, "codigo", string.Empty);
                            pl.Codigo = det["ITEMNMBR"].ToString().Trim(); //XmlText realizador_text = doc.CreateTextNode(det["ITEMNMBR"].ToString().Trim());

                            //realizador.AppendChild(realizador_text);
                            //Item.AppendChild(realizador);
                            /* realizador */

                            /* motivo */
                            //XmlElement motivo = doc.CreateElement(string.Empty, "nombre", string.Empty);
                            pl.Descripcion = det["ITEMDESC"].ToString().Trim();//XmlText motivo_text = doc.CreateTextNode(det["ITEMDESC"].ToString().Trim());

                            //motivo.AppendChild(motivo_text);
                            //Item.AppendChild(motivo);
                            /* motivo */

                            /* fecha */
                            //XmlElement fecha = doc.CreateElement(string.Empty, "nombrelista", string.Empty);
                            pl.NombreLista = det["PRCLEVEL"].ToString().Trim();// XmlText fecha_text = doc.CreateTextNode(det["PRCLEVEL"].ToString().Trim());

                            //fecha.AppendChild(fecha_text);
                            //Item.AppendChild(fecha);
                            /* fecha */


                            //XmlElement element10 = doc.CreateElement(string.Empty, "unidad", string.Empty);
                            pl.UofM = det["UOFM"].ToString().Trim();// XmlText text8 = doc.CreateTextNode(det["UOFM"].ToString().Trim());
                            //element10.AppendChild(text8);
                            //Item.AppendChild(element10);

                            //XmlElement element9 = doc.CreateElement(string.Empty, "cantidad", string.Empty);
                            pl.Cantidad = det["QTYBSUOM"].ToString().Trim().Replace(",", ".");// XmlText text7 = doc.CreateTextNode(det["QTYBSUOM"].ToString().Trim().Replace(",", "."));
                                                                                              // element9.AppendChild(text7);
                                                                                              //Item.AppendChild(element9);

                            /* precio */
                            //XmlElement precio = doc.CreateElement(string.Empty, "precio_neto", string.Empty);
                            pl.PrecioNeto = (decimal) det["PRICE"];// XmlText precio_text = doc.CreateTextNode(det["PRICE"].ToString().Trim().Replace(",", "."));
                            //precio.AppendChild(precio_text);
                            //Item.AppendChild(precio);
                            /* precio */

                            /* cost */
                            //XmlElement cost = doc.CreateElement(string.Empty, "precio_con_itbms", string.Empty);
                            pl.PrecioConITBMS = (decimal)det["PRICEWITMBS"];// XmlText cost_text = doc.CreateTextNode(det["PRICEWITMBS"].ToString().Trim().Replace(",", "."));
                            //cost.AppendChild(cost_text);
                            //Item.AppendChild(cost);
                            /* costo */

                            /* cost */
                            //XmlElement cost2 = doc.CreateElement(string.Empty, "costo", string.Empty);
                            pl.Costo = (decimal)det["CURRCOST"]; // XmlText cost_text2 = doc.CreateTextNode(det["CURRCOST"].ToString().Trim().Replace(",", "."));
                            //cost2.AppendChild(cost_text2);
                            //Item.AppendChild(cost2);
                            /* costo */

                            //XmlElement element7 = doc.CreateElement(string.Empty, "porcentaje_itbms", string.Empty);
                            pl.PrecioITBMS = (decimal)det["ITMBSPCT"];// XmlText text5 = doc.CreateTextNode(det["ITMBSPCT"].ToString().Trim().Replace(",", "."));
                            //element7.AppendChild(text5);
                            //Item.AppendChild(element7);

                            //XmlElement element8 = doc.CreateElement(string.Empty, "tipo", string.Empty);
                            pl.Tipo = (int)Convert.ToInt32(det["ITEMTYPE"]);// XmlText text6 = doc.CreateTextNode(det["ITEMTYPE"].ToString().Trim());
                                                           //element8.AppendChild(text6);
                                                           //Item.AppendChild(element8);





                            offerts.Add(pl);
                            //Lines.AppendChild(Item);





                        }
                        catch (Exception ex)
                        {
                            IEvent err = new ErrorEvent("", "", "No pudo crear el archivo xml correctamente. Mensaje: " + ex.Message);
                            err.Publish();
                        }
                    }
                    doc.AppendChild(Lines);

                    if (this.NowPathIn.ToCharArray().Last() == '/' || this.NowPathIn.ToCharArray().Last() == '\\')
                    {
                        if (!System.IO.File.Exists(this.mNowPathOut + string.Format(FileName, ADJUST)))
                            Models.Oferts.SaveAs(this.mNowPathOut + string.Format(FileName, ADJUST), offerts);// doc.Save(this.mNowPathOut + string.Format(FileName, ADJUST));
                    }
                    else
                    {
                        if (!System.IO.File.Exists(this.mNowPathOut + "/" + string.Format(FileName, ADJUST)))
                            Models.Oferts.SaveAs(this.mNowPathOut + "/" + string.Format(FileName, ADJUST),offerts);//doc.Save(this.mNowPathOut + "/" + string.Format(FileName, ADJUST));
                    }
                    total--;

                    ObserverManager.Instance.addSubject(new ProgressSubject(total, Transactions.Rows.Count - total));

                    IEvent e2 = new InfoEvent("", "", "El archivo '" + String.Format(FileName, ADJUST) + "'. fue creado correctamente");
                    e2.Publish();
                }

                prevDoc = row["PRCLEVEL"].ToString().Trim();
            }
            ObserverManager.Instance.addSubject(new ProgressSubject(0, 0));
        }
        void ITransaccion.Execute()
        {
            System.Threading.Thread th = new System.Threading.Thread(thread);
            th.Start();
        }

        private DataTable getTransactionInfo(string DocumentNumber)
        {
            SqlCommand command = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable data = new DataTable("GL_ADJUSTMENTS");
            command.Connection = new SqlConnection(this.mStringConnection);

            command.Parameters.AddWithValue("@DOCUMENT", DocumentNumber);

            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT IV.DOCNUMBR " +
                ",ISNULL(GL.USWHPSTD,'UNKNOWN') AS USERID " +
                ", CASE DOCTYPE " +
                "       WHEN 1 THEN 'AJUSTE' ELSE 'DESCONOCIDO' " +
                "  END AS DOCTYPE " +
                ",IV.DOCDATE " +
                ",CONVERT(time, ISNULL(GL.DEX_ROW_TS,0)) AS [TIME] " +
                ",IV.ITEMNMBR " +
                ",IV.UOFM " +
                ",'UNKNOWN' AS FAMILY " +
                ",CASE WHEN TRXQTY>0 THEN '1' ELSE '2' END AS PROCESS " +
                ",IV.TRXLOCTN " +
                ",ISNULL((SELECT USERNAME FROM DYNAMICS..SY01400 WHERE USERID=  GL.USWHPSTD),'UNKNOWN') AS USERNAME " +
                ",'UBICACION' AS LOCATION " +
                ",ISNULL(CASE IVM.PRICMTHD " +
                "      WHEN 1 THEN ROUND(UOMPRICE,IVM.DECPLCUR) " +
                "      WHEN 2 THEN ROUND(CUR.LISTPRCE*(UOMPRICE/100),IV.DECPLCUR-1) " +
                "      WHEN 3 THEN ROUND(IVM.CURRCOST + (IVM.CURRCOST * (UOMPRICE/100)),IVM.DECPLCUR-1) " +
                "      WHEN 4 THEN ROUND(IVM.STNDCOST + (IVM.STNDCOST * (UOMPRICE/100)),IVM.DECPLCUR-1) " +
                "      WHEN 5 THEN ROUND(IVM.CURRCOST + ((IVM.CURRCOST * (UOMPRICE/100))/((100-UOMPRICE)/100)),IVM.DECPLCUR-1) " +
                "      WHEN 6 THEN ROUND(IVM.STNDCOST + ((IVM.STNDCOST * (UOMPRICE/100))/((100-UOMPRICE)/100)),IVM.DECPLCUR-1) " +
                "END,0) AS PRICE " +
                ",IVM.CURRCOST AS COST " +
                ",TRXQTY AS QTY " +
                "FROM IV30300 AS IV " +
                "LEFT JOIN GL10000 AS GL ON IV.TRXSORCE = GL.ORTRXSRC AND GL.DTAControlNum=IV.DOCNUMBR " +
                "INNER JOIN IV00101 AS IVM ON IV.ITEMNMBR = IVM.ITEMNMBR " +
                "INNER JOIN IV00108 AS PRCLST ON IV.ITEMNMBR=PRCLST.ITEMNMBR AND IVM.PRCLEVEL=PRCLST.PRCLEVEL AND IVM.PRCHSUOM=PRCLST.UOFM " +
                "INNER JOIN IV00105 AS CUR ON CUR.ITEMNMBR = IV.ITEMNMBR AND PRCLST.CURNCYID = CUR.CURNCYID " +
                "WHERE DOCTYPE=1 AND IV.DOCNUMBR=@DOCUMENT ORDER BY IV.DEX_ROW_ID ASC";


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


        private DataTable getAllPricesLevels()
        {
            SqlCommand command = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable data = new DataTable("PRCLVL");
            command.Connection = new SqlConnection(this.mStringConnection);

            // command.Parameters.AddWithValue("@P", "0");

            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM IV40800";


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

        private DataTable getPriceLeveData(string PRCLVL)
        {

            SqlCommand command = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable data = new DataTable("ITEMMSTR");
            command.Connection = new SqlConnection(this.mStringConnection);

            command.Parameters.AddWithValue("@ITMBS", this.mItmbs);
            command.Parameters.AddWithValue("@PRCLVL", PRCLVL);

            command.CommandType = CommandType.Text;
 
            String query = ""; 
            query += "SELECT DISTINCT IV.ITEMNMBR   ";
            query += ",IV.ITEMDESC,IV.ITEMTYPE,PRCLST.UOFM,ITMCLSCD,CURRCOST,MAX(UOFM.QTYBSUOM) AS QTYBSUOM   ";
            query += ",CASE IV.PRICMTHD   ";
            query += "WHEN 1 THEN ROUND(MAX(PRCLST.UOMPRICE) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)   ";
            query += "WHEN 2 THEN ROUND((MAX(CUR.LISTPRCE)*(MAX(PRCLST.UOMPRICE)/100)) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)    ";
            query += "WHEN 3 THEN ROUND((MAX(IV.CURRCOST) + (MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100))) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)   ";
            query += "WHEN 4 THEN ROUND((MAX(IV.STNDCOST) + (MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)   ";
            query += "WHEN 5 THEN ROUND((MAX(IV.CURRCOST) + ((MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100))) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)   ";
            query += "WHEN 6 THEN ROUND((MAX(IV.STNDCOST) + ((MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100))) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)   ";
            query += "END AS PRICE   ";
            query += ",CASE IV.PRICMTHD   ";
            query += "WHEN 1 THEN ISNULL((MAX(PRCLST.UOMPRICE)+(MAX(PRCLST.UOMPRICE)*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100))) * MAX(UOFM.QTYBSUOM),ROUND(MAX(PRCLST.UOMPRICE) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1))   ";
            query += "WHEN 2 THEN ISNULL((MAX(CUR.LISTPRCE)*(MAX(PRCLST.UOMPRICE)/100) /**/ + ((MAX(CUR.LISTPRCE)*(MAX(PRCLST.UOMPRICE)/100))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100))) * MAX(UOFM.QTYBSUOM),ROUND((MAX(CUR.LISTPRCE)*(MAX(PRCLST.UOMPRICE)/100))* MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1) )   ";
            query += "WHEN 3 THEN ISNULL((MAX(IV.CURRCOST) + (MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100)) /**/ + ((MAX(IV.CURRCOST) + (MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100)))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)))* MAX(UOFM.QTYBSUOM),ROUND((MAX(IV.CURRCOST) + (MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100)))* MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1))   ";
            query += "WHEN 4 THEN ISNULL((MAX(IV.STNDCOST) + (MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100)) /**/ + (MAX(IV.STNDCOST) + (MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)))* MAX(UOFM.QTYBSUOM),ROUND((MAX(IV.STNDCOST) + (MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100)))* MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1))   ";
            query += "WHEN 5 THEN ISNULL((MAX(IV.CURRCOST) + ((MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)) /**/ + ((MAX(IV.CURRCOST) + ((MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)))* MAX(UOFM.QTYBSUOM),ROUND((MAX(IV.CURRCOST) + ((MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)))* MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1))   ";
            query += "WHEN 6 THEN ISNULL((MAX(IV.STNDCOST) + ((MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)) /**/+ ((MAX(IV.STNDCOST) + ((MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)))* MAX(UOFM.QTYBSUOM),ROUND((MAX(IV.STNDCOST) + ((MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)))* MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1))   ";
            query += "END AS PRICEWITMBS   ";
            query += ",ISNULL((SELECT TXDTLPCT FROM TX00201 WHERE TAXDTLID=@ITMBS),0) AS ITMBSPCT   ";
            query += ",PRCLST.PRCLEVEL   ";
            query += "FROM IV00107 AS PRCLVL   ";
            query += "INNER JOIN IV00101 AS IV ON IV.ITEMNMBR=PRCLVL.ITEMNMBR  ";
            query += "INNER JOIN IV00108 AS PRCLST ON PRCLVL.ITEMNMBR=PRCLST.ITEMNMBR AND PRCLVL.PRCLEVEL=PRCLST.PRCLEVEL AND PRCLVL.UOFM=PRCLST.UOFM   ";
            query += "INNER JOIN IV00105 AS CUR ON CUR.ITEMNMBR = IV.ITEMNMBR AND PRCLST.CURNCYID = CUR.CURNCYID   ";
            query += "INNER JOIN IV00106 AS UOFM ON IV.ITEMNMBR=UOFM.ITEMNMBR AND PRCLST.UOFM=UOFM.UOFM   ";
            query += "WHERE PRCLST.PRCLEVEL=@PRCLVL ";
            query += "GROUP BY IV.ITEMNMBR   ";
            query += ",IV.ITEMDESC,IV.ITEMTYPE   ";
            query += ",PRCLST.UOFM   ";
            query += ",ITMCLSCD   ";
            query += ",CURRCOST   ";
            query += ",IV.PRICMTHD  ";
            query += ",PRCLST.PRCLEVEL";
            command.CommandText = query;
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
