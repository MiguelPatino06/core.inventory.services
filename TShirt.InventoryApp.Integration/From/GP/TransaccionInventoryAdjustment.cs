using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Data.SqlClient;

namespace TShirt.InventoryApp.Integration.From.GP
{
    class TransaccionInventoryAdjustment : Transaccion, ITransaccion
    {
        private string mItmbs;

        public TransaccionInventoryAdjustment(string strcon, string nin, string nout, string hin, string hout,string itbms)
            : base("TransaccionInventoryAdjustment", 4, strcon, nin, nout, hin, hout,"")
        {
            mItmbs = itbms;
        }

        void thread()
        {
            DataTable Transactions = getAllLocation();
            string prevDoc = "";
            int total = Transactions.Rows.Count;


            // esrcibo en el nowin porque van para premiumsoft
            foreach (DataRow row in Transactions.Rows)
            {
                if (prevDoc != row["LOCNCODE"].ToString().Trim())
                {              
                
                    DataTable TransactionDetail = getInventorySnapshot(row["LOCNCODE"].ToString().Trim(),mItmbs);
                    string ADJUST = row["LOCNCODE"].ToString().Trim();

                    string FileName = "GP_INI_{0}.xml";

                    IEvent e = new InfoEvent("", "", "Iniciando la creación del archivo XML '" + String.Format(FileName, ADJUST) + "'.");
                    e.Publish();
                    // xml //
                    XmlDocument doc = new XmlDocument();

                    XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    XmlElement root = doc.DocumentElement;
                    doc.InsertBefore(xmlDeclaration, root);

                    /* Lines */
                    XmlElement Lines = doc.CreateElement(string.Empty, "lines", string.Empty);
                    /* Lines */
                    /////////
                    Models.InventoryAdjustment ia = new Models.InventoryAdjustment();

                    foreach (DataRow det in TransactionDetail.Rows)
                    {

                        string DOCNUMBR = det["LOCNCODE"].ToString().Trim();
                        string USERID = "UNKNOWN";

                        string DOCDATE = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day +" 00:00";
                        string TIME = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":00";
                        string ITEMNMBR = det["ITEMNMBR"].ToString().Trim();

                        Models.ItemAvailability iav = new Models.ItemAvailability();
                        try
                        {
                            if (det["PRCLEVEL"].ToString().Trim() == "GALAPAGO")
                            { 
                                XmlElement Item = doc.CreateElement(string.Empty, "articulo", string.Empty);

                                ///* document */
                                //XmlElement document = doc.CreateElement(string.Empty, "documento", string.Empty);
                                iav.Codigoalmacen = det["LOCNCODE"].ToString().Trim();//XmlText document_text = doc.CreateTextNode(det["LOCNCODE"].ToString().Trim());
                                //document.AppendChild(document_text);
                                //Item.AppendChild(document);
                                ///* document */

                                ///* realizador */
                                //XmlElement realizador = doc.CreateElement(string.Empty, "realizador", string.Empty);
                                iav.Realizador = USERID;//XmlText realizador_text = doc.CreateTextNode("UNKNOWN");
                                //realizador.AppendChild(realizador_text);
                                //Item.AppendChild(realizador);
                                ///* realizador */

                                ///* motivo */
                                //XmlElement motivo = doc.CreateElement(string.Empty, "motivo", string.Empty);
                                iav.Motivo = "ESTATUS ACTUAL DE INVENTARIO";//XmlText motivo_text = doc.CreateTextNode("ESTATUS ACTUAL DE INVENTARIO");
                                //motivo.AppendChild(motivo_text);
                                //Item.AppendChild(motivo);
                                ///* motivo */

                                ///* fecha */
                                //XmlElement fecha = doc.CreateElement(string.Empty, "fecha", string.Empty);
                                iav.Fecha = DOCDATE;//XmlText fecha_text = doc.CreateTextNode(DOCDATE);
                                //fecha.AppendChild(fecha_text);
                                //Item.AppendChild(fecha);
                                ///* fecha */

                                ///* hora */
                                //XmlElement hora = doc.CreateElement(string.Empty, "hora", string.Empty);
                                iav.Hora = TIME;//XmlText hora_text = doc.CreateTextNode(TIME);
                                //hora.AppendChild(hora_text);
                                //Item.AppendChild(hora);
                                ///* hora */

                                ///* codigo */
                                //XmlElement codigo = doc.CreateElement(string.Empty, "codigo", string.Empty);
                                iav.Codigo = ITEMNMBR;//XmlText codigo_text = doc.CreateTextNode(ITEMNMBR);
                                //codigo.AppendChild(codigo_text);
                                //Item.AppendChild(codigo);
                                ///* codigo */

                                ///* nombre */
                                //XmlElement nombre = doc.CreateElement(string.Empty, "nombre", string.Empty);
                                iav.Nombre = det["ITEMDESC"].ToString().Trim();//XmlText nombre_text = doc.CreateTextNode(det["ITEMDESC"].ToString().Trim());
                                //nombre.AppendChild(nombre_text);
                                //Item.AppendChild(nombre);
                                ///* nombre */

                                ///* grupo */
                                //XmlElement grupo = doc.CreateElement(string.Empty, "grupo", string.Empty);
                                iav.Grupo = det["ITMCLSCD"].ToString().Trim();//XmlText grupo_text = doc.CreateTextNode(det["ITMCLSCD"].ToString().Trim());
                                //grupo.AppendChild(grupo_text);
                                //Item.AppendChild(grupo);
                                ///* grupo */

                                ///* tipoproceso */
                                //XmlElement tipoproceso = doc.CreateElement(string.Empty, "tipoproceso", string.Empty);
                                iav.Tipoproceso = 1;//XmlText tipoproceso_text = doc.CreateTextNode("1");
                                //tipoproceso.AppendChild(tipoproceso_text);
                                //Item.AppendChild(tipoproceso);
                                ///* tipoproceso */

                                ///* codigoalmacen */
                                //XmlElement codigoalmacen = doc.CreateElement(string.Empty, "codigoalmacen", string.Empty);
                                iav.Codigoalmacen = det["LOCNCODE"].ToString().Trim();//XmlText codigoalmacen_text = doc.CreateTextNode(det["LOCNCODE"].ToString().Trim());
                                //codigoalmacen.AppendChild(codigoalmacen_text);
                                //Item.AppendChild(codigoalmacen);
                                ///* codigoalmacen */


                                ///* usuario */
                                //XmlElement usuario = doc.CreateElement(string.Empty, "descripcion", string.Empty);
                                iav.Descripcion = row["LOCNDSCR"].ToString().Trim();//XmlText usuario_text = doc.CreateTextNode(row["LOCNDSCR"].ToString().Trim());
                                //usuario.AppendChild(usuario_text);
                                //Item.AppendChild(usuario);
                                ///* usuario */

                                ///* ubicacion */
                                //XmlElement ubicacion = doc.CreateElement(string.Empty, "usuario", string.Empty);
                                iav.Usuario = "UNKNOWN"; //XmlText ubicacion_text = doc.CreateTextNode("UNKNOWN");
                                //ubicacion.AppendChild(ubicacion_text);
                                //Item.AppendChild(ubicacion);
                                ///* ubicacion */


                                ///* descripción */
                                //XmlElement descripcion = doc.CreateElement(string.Empty, "ubicacion", string.Empty);
                                iav.Ubicacion = row["ADDRESS1"].ToString().Trim();//XmlText descripcion_text = doc.CreateTextNode(row["ADDRESS1"].ToString().Trim());
                                //descripcion.AppendChild(descripcion_text);
                                //Item.AppendChild(descripcion);
                                ///* descripción */



                                ///* precio */
                                //XmlElement precio = doc.CreateElement(string.Empty, "precio_neto", string.Empty);
                                iav.Precio_neto = (decimal)Convert.ToDecimal(det["PRICE"].ToString().Trim().Replace(",", "."));//XmlText precio_text = doc.CreateTextNode(det["PRICE"].ToString().Trim().Replace(",", "."));
                                //precio.AppendChild(precio_text);
                                //Item.AppendChild(precio);
                                ///* precio */

                                ///* cost */
                                //XmlElement cost = doc.CreateElement(string.Empty, "precio_con_itbms", string.Empty);
                                iav.Precio_con_itbms = (decimal)Convert.ToDecimal(det["PRICEWITMBS"].ToString().Trim().Replace(",", ".")); //XmlText cost_text = doc.CreateTextNode(det["PRICEWITMBS"].ToString().Trim().Replace(",", "."));
                                //cost.AppendChild(cost_text);
                                //Item.AppendChild(cost);
                                ///* costo */

                                ///* costo */
                                //#region "COSTO NODE"
                                //XmlElement costo = doc.CreateElement(string.Empty, "costo", string.Empty);
                                iav.Costo = (decimal)Convert.ToDecimal(det["CURRCOST"].ToString().Trim().Replace(",", "."));//XmlText costo_text = doc.CreateTextNode(det["CURRCOST"].ToString().Trim().Replace(",", "."));
                                //costo.AppendChild(costo_text);
                                //Item.AppendChild(costo);
                                //#endregion
                                ///* costo */

                                //XmlElement element7 = doc.CreateElement(string.Empty, "porcentaje_itbms", string.Empty);
                                iav.Porcentaje_itbms = (decimal)Convert.ToDecimal(det["ITMBSPCT"].ToString().Trim().Replace(",", ".")); //XmlText text5 = doc.CreateTextNode(det["ITMBSPCT"].ToString().Trim().Replace(",", "."));
                                //element7.AppendChild(text5);
                                //Item.AppendChild(element7);

                                //XmlElement element8 = doc.CreateElement(string.Empty, "tipo", string.Empty);
                                iav.Tipo = (int)Convert.ToInt32(det["ITEMTYPE"].ToString().Trim());//XmlText text6 = doc.CreateTextNode(det["ITEMTYPE"].ToString().Trim());
                                //element8.AppendChild(text6);
                                //Item.AppendChild(element8);

                                //XmlElement element9 = doc.CreateElement(string.Empty, "cantidad", string.Empty);
                                iav.Cantidad = (decimal)Convert.ToDecimal(det["DISP"].ToString().Trim().Replace(",", "."));//XmlText text7 = doc.CreateTextNode(det["DISP"].ToString().Trim().Replace(",", "."));
                                //element9.AppendChild(text7);
                                //Item.AppendChild(element9);

                                //XmlElement element10 = doc.CreateElement(string.Empty, "unidad_de_medida", string.Empty);
                                iav.Unidad_de_medida = det["UOFM"].ToString().Trim();//XmlText text8 = doc.CreateTextNode(det["UOFM"].ToString().Trim());
                                //element10.AppendChild(text8);
                                //Item.AppendChild(element10);


                                ia.Add(iav);//Lines.AppendChild(Item);
                            }




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
                            Models.InventoryAdjustment.SaveAs(this.mNowPathOut + string.Format(FileName, ADJUST), ia);// doc.Save(this.mNowPathOut + string.Format(FileName, ADJUST));
                    }
                    else
                    {
                        if (!System.IO.File.Exists(this.mNowPathOut + "/" + string.Format(FileName, ADJUST)))
                            Models.InventoryAdjustment.SaveAs(this.mNowPathOut + "/" + string.Format(FileName, ADJUST), ia);// doc.Save(this.mNowPathOut + "/" + string.Format(FileName, ADJUST));
                    }
                    total--;

                    ObserverManager.Instance.addSubject(new ProgressSubject(total, Transactions.Rows.Count - total));
                    
                    IEvent e2 = new InfoEvent("", "", "El archivo '" + String.Format(FileName, ADJUST) + "'. fue creado correctamente");
                    e2.Publish();
                }
               
                prevDoc = row["LOCNCODE"].ToString().Trim();
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


        private DataTable getAllLocation()
        {
            SqlCommand command = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable data = new DataTable("LOCATIONS");
            command.Connection = new SqlConnection(this.mStringConnection);

            // command.Parameters.AddWithValue("@P", "0");

            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM IV40700";


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

        private DataTable getInventorySnapshot(String LOCATION,string itbms)
        {
            SqlCommand command = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable data = new DataTable("ITEMS");
            command.Connection = new SqlConnection(this.mStringConnection);

            command.Parameters.AddWithValue("@ITMBS", itbms);
            command.Parameters.AddWithValue("@LOCATION", LOCATION);

            command.CommandType = CommandType.Text;
            string query = "";

            query += "SELECT ";
            query += "IV.LOCNCODE ";
            query += ",IV.ITEMNMBR ";
            query += ",(IV.QTYONHND-IV.ATYALLOC) AS DISP    ";
            query += ",ITEM.UOFM ";
            query += ",ITEM.ITEMDESC   ";
            query += ",ITEM.ITEMTYPE   ";
            query += ",ITEM.ITMCLSCD   ";
            query += ",ITEM.CURRCOST   ";
            query += ",ITEM.QTYBSUOM   ";
            query += ",ITEM.PRICE   ";
            query += ",ITEM.PRICEWITMBS   ";
            query += ",ITEM.ITMBSPCT   ";
            query += ",ITEM.PRCLEVEL   ";
            query += "FROM IV00102 AS IV ";
            query += "INNER JOIN (SELECT DISTINCT IV.ITEMNMBR   ";
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
            query += "INNER JOIN IV00101 AS IV ON IV.ITEMNMBR=PRCLVL.ITEMNMBR    ";
            query += "INNER JOIN IV00108 AS PRCLST ON PRCLVL.ITEMNMBR=PRCLST.ITEMNMBR AND PRCLVL.PRCLEVEL=PRCLST.PRCLEVEL AND PRCLVL.UOFM=PRCLST.UOFM   ";
            query += "INNER JOIN IV00105 AS CUR ON CUR.ITEMNMBR = IV.ITEMNMBR AND PRCLST.CURNCYID = CUR.CURNCYID   ";
            query += "INNER JOIN IV00106 AS UOFM ON IV.ITEMNMBR=UOFM.ITEMNMBR AND PRCLST.UOFM=UOFM.UOFM   ";
            query += "WHERE PRCLST.PRCLEVEL='GALAPAGO' ";
            query += "GROUP BY IV.ITEMNMBR   ";
            query += ",IV.ITEMDESC,IV.ITEMTYPE   ";
            query += ",PRCLST.UOFM   ";
            query += ",ITMCLSCD   ";
            query += ",CURRCOST   ";
            query += ",IV.PRICMTHD   ";
            query += ",PRCLST.PRCLEVEL) AS ITEM ON ITEM.ITEMNMBR=IV.ITEMNMBR ";
            query += "WHERE IV.LOCNCODE <> '' AND IV.LOCNCODE = @LOCATION "; 

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

        private DataTable getItemsData(string ITEMNMBR)
        {

            SqlCommand command = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable data = new DataTable("ITEMMSTR");
            command.Connection = new SqlConnection(this.mStringConnection);

            command.Parameters.AddWithValue("@ITMBS", this.mItmbs);
            command.Parameters.AddWithValue("@ITEMNMBR", ITEMNMBR);

            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT IV.ITEMNMBR " +
                ",IV.ITEMDESC,IV.ITEMTYPE,IV.SELNGUOM " +
                ",ITMCLSCD " +
                ",CURRCOST " +
                ",CASE IV.PRICMTHD " +
                "WHEN 1 THEN ROUND(UOMPRICE,IV.DECPLCUR) " +
                "WHEN 2 THEN ROUND(CUR.LISTPRCE*(UOMPRICE/100),IV.DECPLCUR-1) " +
                "WHEN 3 THEN ROUND(IV.CURRCOST + (IV.CURRCOST * (UOMPRICE/100)),IV.DECPLCUR-1) " +
                "WHEN 4 THEN ROUND(IV.STNDCOST + (IV.STNDCOST * (UOMPRICE/100)),IV.DECPLCUR-1) " +
                "WHEN 5 THEN ROUND(IV.CURRCOST + ((IV.CURRCOST * (UOMPRICE/100))/((100-UOMPRICE)/100)),IV.DECPLCUR-1) " +
                "WHEN 6 THEN ROUND(IV.STNDCOST + ((IV.STNDCOST * (UOMPRICE/100))/((100-UOMPRICE)/100)),IV.DECPLCUR-1) " +
                "END AS PRICE " +
                ",CASE IV.PRICMTHD " +
                "WHEN 1 THEN ISNULL(UOMPRICE+(UOMPRICE*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)),ROUND(UOMPRICE,IV.DECPLCUR)) " +
                "WHEN 2 THEN ISNULL(CUR.LISTPRCE*(UOMPRICE/100) /**/ + ((CUR.LISTPRCE*(UOMPRICE/100))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)),ROUND(CUR.LISTPRCE*(UOMPRICE/100),IV.DECPLCUR-1) ) " +
                "WHEN 3 THEN ISNULL(IV.CURRCOST + (IV.CURRCOST * (UOMPRICE/100)) /**/ + ((IV.CURRCOST + (IV.CURRCOST * (UOMPRICE/100)))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)),ROUND(IV.CURRCOST + (IV.CURRCOST * (UOMPRICE/100)),IV.DECPLCUR-1)) " +
                "WHEN 4 THEN ISNULL(IV.STNDCOST + (IV.STNDCOST * (UOMPRICE/100)) /**/ + (IV.STNDCOST + (IV.STNDCOST * (UOMPRICE/100))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)),ROUND(IV.STNDCOST + (IV.STNDCOST * (UOMPRICE/100)),IV.DECPLCUR-1)) " +
                "WHEN 5 THEN ISNULL(IV.CURRCOST + ((IV.CURRCOST * (UOMPRICE/100))/((100-UOMPRICE)/100)) /**/ + ((IV.CURRCOST + ((IV.CURRCOST * (UOMPRICE/100))/((100-UOMPRICE)/100)))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)),ROUND(IV.CURRCOST + ((IV.CURRCOST * (UOMPRICE/100))/((100-UOMPRICE)/100)),IV.DECPLCUR-1)) " +
                "WHEN 6 THEN ISNULL(IV.STNDCOST + ((IV.STNDCOST * (UOMPRICE/100))/((100-UOMPRICE)/100)) /**/+ ((IV.STNDCOST + ((IV.STNDCOST * (UOMPRICE/100))/((100-UOMPRICE)/100)))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)),ROUND(IV.STNDCOST + ((IV.STNDCOST * (UOMPRICE/100))/((100-UOMPRICE)/100)),IV.DECPLCUR-1)) " +
                "END AS PRICEWITMBS " +
                ",ISNULL((SELECT TXDTLPCT FROM TX00201 WHERE TAXDTLID=@ITMBS),0) AS ITMBSPCT " +
                "FROM IV00101 AS IV " +
                "INNER JOIN IV00108 AS PRCLST ON IV.ITEMNMBR=PRCLST.ITEMNMBR AND IV.PRCLEVEL=PRCLST.PRCLEVEL AND IV.PRCHSUOM=PRCLST.UOFM " +
                "INNER JOIN IV00105 AS CUR ON CUR.ITEMNMBR = IV.ITEMNMBR AND PRCLST.CURNCYID = CUR.CURNCYID WHERE IV.ITEMNMBR=@ITEMNMBR";


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
