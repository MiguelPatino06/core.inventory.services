using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Data.SqlClient;
//using Microsoft.Dexterity.Applications.DynamicsDictionary;
//using Microsoft.Dexterity.Applications;
//using Microsoft.Dexterity.Bridge;

namespace TShirt.InventoryApp.Integration.From.GP
{
    class TransaccionLoadItem :Transaccion, ITransaccion
    {
        private string mItmbs;


        public TransaccionLoadItem(string strcon, string nin, string nout, string hin, string hout,string itmbs)
            : base("TransaccionLoadItem", 2, strcon, nin, nout, hin, hout,itmbs)
        {
            this.mItmbs = itmbs;
        }

        void thread()
        {
            DataTable itemsData = getAllItemsData();
            int total = itemsData.Rows.Count;

            // esrcibo en el nowin porque van para premiumsoft
            foreach (DataRow row in itemsData.Rows)
            {
                string FileName = "GP_TINV_{0}.xml";


                string ITEMNMBR = row["ITEMNMBR"].ToString().Trim();
                string ITEMDESC = row["ITEMDESC"].ToString().Trim();
                string ITMCLSCD = row["ITMCLSCD"].ToString().Trim();
                string CURRCOST = row["CURRCOST"].ToString().Trim();
                string PRICE = row["PRICE"].ToString().Trim();
                string PRICEWITMBS = row["PRICEWITMBS"].ToString().Trim();
                string ITMBSPCT = row["ITMBSPCT"].ToString().Trim();
                int ITEMTYPE = Int16.Parse(row["ITEMTYPE"].ToString());
                string UOFM = row["UOFM"].ToString().Trim();

                IEvent e = new InfoEvent("", "", "Iniciando la creación del archivo XML '" + String.Format(FileName, ITEMNMBR) + "'.");
                e.Publish();

                // xml //
                XmlDocument doc = new XmlDocument();

                /* Lines */
                XmlElement Raiz = doc.CreateElement(string.Empty, "raiz", string.Empty);
                /* Lines */

                Models.Item item = new Models.Item();


                try
                {
                    //XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    //XmlElement root = doc.DocumentElement;

                    //doc.InsertBefore(xmlDeclaration, root);
                    //doc.AppendChild(Raiz);

                    //XmlElement element2 = doc.CreateElement(string.Empty, "item", string.Empty);
                    //Raiz.AppendChild(element2);

                    //XmlElement element3 = doc.CreateElement(string.Empty, "codigo", string.Empty);
                    item.No = ITEMNMBR;//XmlText text1 = doc.CreateTextNode(ITEMNMBR);
                    //element3.AppendChild(text1);
                    //element2.AppendChild(element3);

                    //XmlElement element4 = doc.CreateElement(string.Empty, "nombre", string.Empty);
                    item.Description = ITEMDESC; //XmlText text2 = doc.CreateTextNode(ITEMDESC);
                    //element4.AppendChild(text2);
                    //element2.AppendChild(element4);

                    //XmlElement element5 = doc.CreateElement(string.Empty, "codigogrupo", string.Empty);
                    //XmlText text3 = doc.CreateTextNode(ITMCLSCD);
                    //element5.AppendChild(text3);
                    //element2.AppendChild(element5);

                    //XmlElement element6 = doc.CreateElement(string.Empty, "costo", string.Empty);
                    //XmlText text4 = doc.CreateTextNode(CURRCOST.ToString().Replace(",","."));
                    //element6.AppendChild(text4);
                    //element2.AppendChild(element6);

                    //XmlElement element7 = doc.CreateElement(string.Empty, "precio_neto", string.Empty);
                    item.PricewithoutVAT = (decimal) Convert.ToDecimal(PRICE);//XmlText text5 = doc.CreateTextNode(PRICE.ToString().Replace(",", "."));
                    //element7.AppendChild(text5);
                    //element2.AppendChild(element7);

                    //XmlElement element8 = doc.CreateElement(string.Empty, "precio_con_itbms", string.Empty);
                    item.PriceincludingVAT = (decimal) Convert.ToDecimal(PRICEWITMBS);//XmlText text6 = doc.CreateTextNode(PRICEWITMBS.ToString().Replace(",", "."));
                    //element8.AppendChild(text6);
                    //element2.AppendChild(element8);

                    //XmlElement element9 = doc.CreateElement(string.Empty, "porcentaje_itbms", string.Empty);
                    item.VAT = (decimal)Convert.ToDecimal(ITMBSPCT.ToString());//XmlText text7 = doc.CreateTextNode(ITMBSPCT.ToString().Replace(",", "."));
                    //element9.AppendChild(text7);
                    //element2.AppendChild(element9);

                    //XmlElement element10 = doc.CreateElement(string.Empty, "tipo", string.Empty);
                    //XmlText text8 = doc.CreateTextNode(ITEMTYPE.ToString());
                    //element10.AppendChild(text8);
                    //element2.AppendChild(element10);

                    //XmlElement element11 = doc.CreateElement(string.Empty, "unidad_de_medida", string.Empty);
                    //XmlText text9 = doc.CreateTextNode(UOFM);
                    //element11.AppendChild(text9);
                    //element2.AppendChild(element11);

                    if (this.NowPathIn.ToCharArray().Last() == '/' || this.NowPathIn.ToCharArray().Last() == '\\')
                    {
                        if (!System.IO.File.Exists(this.mNowPathOut + string.Format(FileName, ITEMNMBR)))
                            Models.Item.SaveAs(this.mNowPathOut + string.Format(FileName, ITEMNMBR), item); // doc.Save(this.mNowPathOut + string.Format(FileName, ITEMNMBR));
                    }
                    else
                    {
                        if (!System.IO.File.Exists(this.mNowPathOut + "/" + string.Format(FileName, ITEMNMBR)))
                            Models.Item.SaveAs(this.mNowPathOut + "/" + string.Format(FileName, ITEMNMBR), item); // doc.Save(this.mNowPathOut + "/" + string.Format(FileName, ITEMNMBR));
                    }

                    total--;

                    ObserverManager.Instance.addSubject(new ProgressSubject(total, itemsData.Rows.Count - total));

                    IEvent e2 = new InfoEvent("", "", "El archivo '" + String.Format(FileName, ITEMNMBR) + "'. fue creado correctamente");
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

        private DataTable getAllItemsData()
        {   

            SqlCommand command = new SqlCommand();
            SqlDataAdapter adapter = new SqlDataAdapter();

            DataTable data = new DataTable("ITEMMSTR");
            command.Connection = new SqlConnection(this.mStringConnection);

            command.Parameters.AddWithValue("@ITMBS", this.mItmbs);

            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT DISTINCT IV.ITEMNMBR " +
            ",IV.ITEMDESC,IV.ITEMTYPE,PRCLST.UOFM,ITMCLSCD,CURRCOST,MAX(UOFM.QTYBSUOM) AS QTYBSUOM " +
            ",CASE IV.PRICMTHD " +
            "WHEN 1 THEN ROUND(MAX(PRCLST.UOMPRICE) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1) " +
            "WHEN 2 THEN ROUND((MAX(CUR.LISTPRCE)*(MAX(PRCLST.UOMPRICE)/100)) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)  " +
            "WHEN 3 THEN ROUND((MAX(IV.CURRCOST) + (MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100))) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1) " +
            "WHEN 4 THEN ROUND((MAX(IV.STNDCOST) + (MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1) " +
            "WHEN 5 THEN ROUND((MAX(IV.CURRCOST) + ((MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100))) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1) " +
            "WHEN 6 THEN ROUND((MAX(IV.STNDCOST) + ((MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100))) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1) " +
            "END AS PRICE " +
            ",CASE IV.PRICMTHD " +
            "WHEN 1 THEN ISNULL((MAX(PRCLST.UOMPRICE)+(MAX(PRCLST.UOMPRICE)*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100))) * MAX(UOFM.QTYBSUOM),ROUND(MAX(PRCLST.UOMPRICE) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)) " +
            "WHEN 2 THEN ISNULL((MAX(CUR.LISTPRCE)*(MAX(PRCLST.UOMPRICE)/100) /**/ + ((MAX(CUR.LISTPRCE)*(MAX(PRCLST.UOMPRICE)/100))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100))) * MAX(UOFM.QTYBSUOM),ROUND((MAX(CUR.LISTPRCE)*(MAX(PRCLST.UOMPRICE)/100))* MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1) ) " +
            "WHEN 3 THEN ISNULL((MAX(IV.CURRCOST) + (MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100)) /**/ + ((MAX(IV.CURRCOST) + (MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100)))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)))* MAX(UOFM.QTYBSUOM),ROUND((MAX(IV.CURRCOST) + (MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100)))* MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)) " +
            "WHEN 4 THEN ISNULL((MAX(IV.STNDCOST) + (MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100)) /**/ + (MAX(IV.STNDCOST) + (MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)))* MAX(UOFM.QTYBSUOM),ROUND((MAX(IV.STNDCOST) + (MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100)))* MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)) " +
            "WHEN 5 THEN ISNULL((MAX(IV.CURRCOST) + ((MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)) /**/ + ((MAX(IV.CURRCOST) + ((MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)))* MAX(UOFM.QTYBSUOM),ROUND((MAX(IV.CURRCOST) + ((MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)))* MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)) " +
            "WHEN 6 THEN ISNULL((MAX(IV.STNDCOST) + ((MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)) /**/+ ((MAX(IV.STNDCOST) + ((MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)))* MAX(UOFM.QTYBSUOM),ROUND((MAX(IV.STNDCOST) + ((MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)))* MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)) " +
            "END AS PRICEWITMBS " +
            ",ISNULL((SELECT TXDTLPCT FROM TX00201 WHERE TAXDTLID=@ITMBS),0) AS ITMBSPCT " +
            ",PRCLST.PRCLEVEL " +
            "FROM IV00107 AS PRCLVL " +
            "INNER JOIN IV00101 AS IV ON IV.ITEMNMBR=PRCLVL.ITEMNMBR AND PRCLVL.PRCLEVEL='GALAPAGO' " +
            "INNER JOIN IV00108 AS PRCLST ON PRCLVL.ITEMNMBR=PRCLST.ITEMNMBR AND PRCLVL.PRCLEVEL=PRCLST.PRCLEVEL AND PRCLVL.UOFM=PRCLST.UOFM " +
            "INNER JOIN IV00105 AS CUR ON CUR.ITEMNMBR = IV.ITEMNMBR AND PRCLST.CURNCYID = CUR.CURNCYID " +
            "INNER JOIN IV00106 AS UOFM ON IV.ITEMNMBR=UOFM.ITEMNMBR AND PRCLST.UOFM=UOFM.UOFM " +
            "GROUP BY IV.ITEMNMBR " +
            ",IV.ITEMDESC,IV.ITEMTYPE " +
            ",PRCLST.UOFM " +
            ",ITMCLSCD " +
            ",CURRCOST " +
            ",IV.PRICMTHD " +
            ",PRCLST.PRCLEVEL ";


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
