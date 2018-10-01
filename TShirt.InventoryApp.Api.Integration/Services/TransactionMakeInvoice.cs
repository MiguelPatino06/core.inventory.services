using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;
using System.Globalization;
using TShirt.InventoryApp.Api.Integration.Utils;

namespace TShirt.InventoryApp.Api.Integration.Services
{
    public class TransactionMakeInvoice
    {
        private string file;
        private string fname;

        private string server;
        private string db;


        //public TransactionMakeInvoice(string strcon, string nin, string nout, string hin, string hout, string fil, string srv, string datab,string itbms)
        //{
        //    file = fil;
        //    fname = file.Split('\\').Last().Split('/').Last().Replace(nin, "");
        //    server = "10.1.92.203"; // srv;
        //    db = datab;

        //    GenerateDocument();
        //}


        private void GenerateDocument()
        {
            //DataSet data = parseXML(file);
            //<add name="TSGVLEntities" connectionString="metadata=res://*/ModelTShirt.csdl|res://*/ModelTShirt.ssdl|res://*/ModelTShirt.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=10.1.92.203;initial catalog=TSGVL;user id=applegreen;password=galapago;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" /></connectionStrings>
            string eConnectConnectionString = string.Format("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TSGVL;Data Source=10.1.92.203;user id=applegreen;password=galapago");

            string DOCID = "FAC";


            //Now we create the eConnect XML Document
            XmlDocument eConnectXMLDocument = new XmlDocument();
            XmlNode NodeeConnect;
            XmlNode NodeSchema;
            XmlNode taSopHdrNode;
            XmlNode taSopLine_ItemsNode;
            XmlNode taSopLineNode;
            XmlNode taCreateSopPaymentInsertRecord_Items;
            XmlNode taCreateSopPaymentInsertRecord;

            XmlNode NodeElement;

            //if (data.Tables["HEADER"].Rows.Count > 0 && data.Tables["LINES"].Rows.Count > 0)
            //{

                //DataRow header = data.Tables["HEADER"].Rows[0];


                //if (header["DOCID"].ToString().Length <= 0)
                //{
                //    DOCID = "NC"; //CHANGED GPHelper.returnDOCID(this.StringConnection).Trim();
                //}
                //else
                //{
                //    DOCID = "NC"; //CHANGED header["DOCID"].ToString();
                //}

                //if (header["DOCNUMBE"].ToString() == "")
                //{
                  
                //    header["DOCNUMBE"] = "";
                //}
                NodeeConnect = eConnectXMLDocument.CreateElement("eConnect");
                NodeSchema = eConnectXMLDocument.CreateElement("SOPTransactionType");

                //Header
                taSopHdrNode = eConnectXMLDocument.CreateElement("taSopHdrIvcInsert");

                NodeElement = eConnectXMLDocument.CreateElement("SOPTYPE");
                NodeElement.InnerText = "3"; // INVOICE
                taSopHdrNode.AppendChild(NodeElement);

                NodeElement = eConnectXMLDocument.CreateElement("DOCID");
                NodeElement.InnerText = DOCID;
                taSopHdrNode.AppendChild(NodeElement);

                NodeElement = eConnectXMLDocument.CreateElement("DEFPRICING");
                NodeElement.InnerText = "1";
                taSopHdrNode.AppendChild(NodeElement);

                NodeElement = eConnectXMLDocument.CreateElement("CREATETAXES");
                NodeElement.InnerText = "1";
                taSopHdrNode.AppendChild(NodeElement);

                NodeElement = eConnectXMLDocument.CreateElement("DEFTAXSCHDS");
                NodeElement.InnerText = "1";
                taSopHdrNode.AppendChild(NodeElement);

                NodeElement = eConnectXMLDocument.CreateElement("BACHNUMB");
                NodeElement.InnerText = DOCID + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
                taSopHdrNode.AppendChild(NodeElement);

                NodeElement = eConnectXMLDocument.CreateElement("SOPNUMBE");
                NodeElement.InnerText = "1023"; //CHANGED header["DOCNUMBE"].ToString();
                taSopHdrNode.AppendChild(NodeElement);

                NodeElement = eConnectXMLDocument.CreateElement("ORIGNUMB");
                NodeElement.InnerText = "1024"; //CHANGED header["ORIGNUMB"].ToString();
                taSopHdrNode.AppendChild(NodeElement);

                NodeElement = eConnectXMLDocument.CreateElement("ORIGTYPE");
                NodeElement.InnerText = "1025"; //CHANGED header["ORIGTYPE"].ToString();
                taSopHdrNode.AppendChild(NodeElement);

                //if (header["TAXSCHID"].ToString().Length <= 0)
                //{
                //    NodeElement = eConnectXMLDocument.CreateElement("TAXSCHID");
                //    NodeElement.InnerText = "1024"; //CHANGED this.ITBMS;
                //    taSopHdrNode.AppendChild(NodeElement);
                //}
                //else
                //{
                    NodeElement = eConnectXMLDocument.CreateElement("TAXSCHID");
                    NodeElement.InnerText = "1024"; //CHANGED header["TAXSCHID"].ToString();
                    taSopHdrNode.AppendChild(NodeElement);
                //}




                #region Lines
                // Details

                taSopLine_ItemsNode = eConnectXMLDocument.CreateElement("taSopLineIvcInsert_Items");

                //foreach (DataRow item in data.Tables["LINES"].Rows)
                //{
                    taSopLineNode = eConnectXMLDocument.CreateElement("taSopLineIvcInsert");
                    NodeElement = eConnectXMLDocument.CreateElement("SOPTYPE");
                    NodeElement.InnerText = "3"; //return
                    taSopLineNode.AppendChild(NodeElement);

                    NodeElement = eConnectXMLDocument.CreateElement("DEFPRICING");
                    NodeElement.InnerText = "1";
                    taSopHdrNode.AppendChild(NodeElement);

                    NodeElement = eConnectXMLDocument.CreateElement("SOPNUMBE");
                    NodeElement.InnerText = "1010"; //CHANGED header["DOCNUMBE"].ToString();
                    taSopLineNode.AppendChild(NodeElement);

                    NodeElement = eConnectXMLDocument.CreateElement("CUSTNMBR");
                    NodeElement.InnerText = "1"; //CHANGED header["CUSTOMER"].ToString();
                    taSopLineNode.AppendChild(NodeElement);

                    NodeElement = eConnectXMLDocument.CreateElement("DOCDATE");
                    NodeElement.InnerText = "2017-08-02";
                    taSopLineNode.AppendChild(NodeElement);

                    NodeElement = eConnectXMLDocument.CreateElement("LOCNCODE");
                    NodeElement.InnerText = "1"; //CHANGED item["LOCNCODE"].ToString();
                    taSopLineNode.AppendChild(NodeElement);

                    NodeElement = eConnectXMLDocument.CreateElement("ITEMNMBR");
                    NodeElement.InnerText = "1"; //CHANGEDitem["ITEMNMBR"].ToString();
                    taSopLineNode.AppendChild(NodeElement);

                    NodeElement = eConnectXMLDocument.CreateElement("UNITPRCE");
                    NodeElement.InnerText = "1"; //CHANGED item["UNITPRCE"].ToString();
                    taSopLineNode.AppendChild(NodeElement);

                    NodeElement = eConnectXMLDocument.CreateElement("XTNDPRCE");
                    NodeElement.InnerText = "25";
                    taSopLineNode.AppendChild(NodeElement);

                    NodeElement = eConnectXMLDocument.CreateElement("QUANTITY");
                    NodeElement.InnerText = "23"; //CHANGED item["QUANTITY"].ToString();
                    taSopLineNode.AppendChild(NodeElement);


                    NodeElement = eConnectXMLDocument.CreateElement("MRKDNAMT");
                    NodeElement.InnerText = "1"; //CHANGED item["MRKDNAMT"].ToString();
                    taSopLineNode.AppendChild(NodeElement);

                    NodeElement = eConnectXMLDocument.CreateElement("UOFM");
                    NodeElement.InnerText = "0"; //CHANGED item["UOFM"].ToString();
                    taSopLineNode.AppendChild(NodeElement);

                    NodeElement = eConnectXMLDocument.CreateElement("USERDEF1");
                    NodeElement.InnerText = "1"; //CHANGED item["USERDEF1"].ToString();
                    taSopLineNode.AppendChild(NodeElement);

                    NodeElement = eConnectXMLDocument.CreateElement("USERDEF2");
                    NodeElement.InnerText = "2"; //CHANGED item["USERDEF2"].ToString();
                    taSopLineNode.AppendChild(NodeElement);

                    NodeElement = eConnectXMLDocument.CreateElement("USERDEF3");
                    NodeElement.InnerText = "3"; //CHANGED item["USERDEF3"].ToString();
                    taSopLineNode.AppendChild(NodeElement);

                    NodeElement = eConnectXMLDocument.CreateElement("USERDEF4");
                    NodeElement.InnerText = "4"; //CHANGED item["USERDEF4"].ToString();
                    taSopLineNode.AppendChild(NodeElement);

                    NodeElement = eConnectXMLDocument.CreateElement("USERDEF5");
                    NodeElement.InnerText = "5"; //CHANGED item["USERDEF5"].ToString();
                    taSopLineNode.AppendChild(NodeElement);

                    taSopLine_ItemsNode.AppendChild(taSopLineNode);
                //}
                #endregion
                // Payments
                // Details
                Decimal totalPaid = 0;

                //taCreateSopPaymentInsertRecord_Items = eConnectXMLDocument.CreateElement("taCreateSopPaymentInsertRecord_Items");

                //foreach (DataRow item in data.Tables["PAYMENTS"].Rows)
                //{
                //    taCreateSopPaymentInsertRecord = eConnectXMLDocument.CreateElement("taCreateSopPaymentInsertRecord");
                //    NodeElement = eConnectXMLDocument.CreateElement("SOPTYPE");
                //    NodeElement.InnerText = "3"; //return
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    NodeElement = eConnectXMLDocument.CreateElement("SOPNUMBE");
                //    NodeElement.InnerText = header["DOCNUMBE"].ToString();
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    NodeElement = eConnectXMLDocument.CreateElement("CUSTNMBR");
                //    NodeElement.InnerText = header["CUSTOMER"].ToString();
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);


                //    NodeElement = eConnectXMLDocument.CreateElement("DOCDATE");
                //    NodeElement.InnerText = DateTime.Parse(item["DOCDATE"].ToString()).ToString("yyyy-MM-dd");
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    NodeElement = eConnectXMLDocument.CreateElement("DOCAMNT");
                //    NodeElement.InnerText = item["DOCAMNT"].ToString();
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    totalPaid += (Decimal)item["DOCAMNT"];

                //    NodeElement = eConnectXMLDocument.CreateElement("CHEKBKID");
                //    NodeElement.InnerText = item["CHEKBKID"].ToString();
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    NodeElement = eConnectXMLDocument.CreateElement("CARDNAME");
                //    NodeElement.InnerText = item["CARDNAME"].ToString();
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    NodeElement = eConnectXMLDocument.CreateElement("CHEKNMBR");
                //    NodeElement.InnerText = item["CHEKNMBR"].ToString();
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    NodeElement = eConnectXMLDocument.CreateElement("RCTNCCRD");
                //    NodeElement.InnerText = item["RCTNCCRD"].ToString();
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    NodeElement = eConnectXMLDocument.CreateElement("AUTHCODE");
                //    NodeElement.InnerText = item["AUTHCODE"].ToString();
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    NodeElement = eConnectXMLDocument.CreateElement("EXPNDATE");
                //    NodeElement.InnerText = ((DateTime)item["EXPNDATE"]).ToString("yyyy-MM-dd");
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    NodeElement = eConnectXMLDocument.CreateElement("PYMTTYPE");
                //    NodeElement.InnerText = item["PYMTTYPE"].ToString();
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    NodeElement = eConnectXMLDocument.CreateElement("DOCNUMBR");
                //    NodeElement.InnerText = item["DOCNUMBR"].ToString();
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    NodeElement = eConnectXMLDocument.CreateElement("USRDEFND1");
                //    NodeElement.InnerText = item["USERDEF1"].ToString();
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    NodeElement = eConnectXMLDocument.CreateElement("USERDEF2");
                //    NodeElement.InnerText = item["USERDEF2"].ToString();
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    NodeElement = eConnectXMLDocument.CreateElement("USERDEF3");
                //    NodeElement.InnerText = item["USERDEF3"].ToString();
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    NodeElement = eConnectXMLDocument.CreateElement("USERDEF4");
                //    NodeElement.InnerText = item["USERDEF4"].ToString();
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    NodeElement = eConnectXMLDocument.CreateElement("USERDEF5");
                //    NodeElement.InnerText = item["USERDEF5"].ToString();
                //    taCreateSopPaymentInsertRecord.AppendChild(NodeElement);

                //    taCreateSopPaymentInsertRecord_Items.AppendChild(taCreateSopPaymentInsertRecord);
                //}

                //NodeElement = eConnectXMLDocument.CreateElement("PYMTRCVD");
                //NodeElement.InnerText = totalPaid.ToString().Replace(",",".").Replace(".",".");
                //taSopHdrNode.AppendChild(NodeElement);

                //NodeSchema.AppendChild(taSopLine_ItemsNode);
                //NodeSchema.AppendChild(taCreateSopPaymentInsertRecord_Items);
                //NodeSchema.AppendChild(taSopHdrNode);

                // Document
                NodeeConnect.AppendChild(NodeSchema);
                eConnectXMLDocument.AppendChild(NodeeConnect);



                if (!GPHelper.eConnectSendToGP(eConnectXMLDocument.OuterXml, eConnectConnectionString))
                {
                    Console.WriteLine("Se detectó un error en la transacción e connect, se procederá con el rollback de la transación");
                }
                else
                {
                    Console.WriteLine("La transacción culminó satisfactoriament");

                    //if (File.Exists(this.HistPathIn + "\\OK_" + this.fname.Replace("\\", "")))
                    //{
                    //    File.Delete(this.HistPathIn + "\\OK_" + this.fname.Replace("\\", ""));
                    //}
                    //File.Move(this.file, this.HistPathIn + "\\OK_" + this.fname.Replace("\\", ""));                
                }

            //}
        }
        
        private DataSet parseXML(string filename)
        {
            DataSet data = new DataSet("INVOICE");
            data.Tables.Add("HEADER");

            data.Tables["HEADER"].Columns.Add("DOCNUMBE", typeof(string));
            data.Tables["HEADER"].Columns.Add("DOCID", typeof(string));
            data.Tables["HEADER"].Columns.Add("ORIGNUMB", typeof(string));
            data.Tables["HEADER"].Columns.Add("ORIGTYPE", typeof(int));
            data.Tables["HEADER"].Columns.Add("TAXSCHID", typeof(string));
            data.Tables["HEADER"].Columns.Add("FRTSCHID", typeof(string));
            data.Tables["HEADER"].Columns.Add("MSCSCHID", typeof(string));
            data.Tables["HEADER"].Columns.Add("SHIPMTHD", typeof(string));
            data.Tables["HEADER"].Columns.Add("TAXAMNT", typeof(Decimal));
            data.Tables["HEADER"].Columns.Add("LOCNCODE", typeof(string));
            data.Tables["HEADER"].Columns.Add("DOCDATE", typeof(DateTime));
            data.Tables["HEADER"].Columns.Add("FREIGHT", typeof(Decimal));
            data.Tables["HEADER"].Columns.Add("MISCAMNT", typeof(Decimal));
            data.Tables["HEADER"].Columns.Add("TRDISAMT", typeof(Decimal));
            data.Tables["HEADER"].Columns.Add("CUSTOMER", typeof(string));
            data.Tables["HEADER"].Columns.Add("CSTPONBR", typeof(string));
            data.Tables["HEADER"].Columns.Add("SUBTOTAL", typeof(Decimal));
            data.Tables["HEADER"].Columns.Add("DOCAMNT", typeof(Decimal));
            data.Tables["HEADER"].Columns.Add("SALSTERR", typeof(string));
            data.Tables["HEADER"].Columns.Add("SLPRSNID", typeof(string));
            data.Tables["HEADER"].Columns.Add("FRTTXAMT", typeof(Decimal));
            data.Tables["HEADER"].Columns.Add("COO", typeof(string));
            data.Tables["HEADER"].Columns.Add("INVOICE", typeof(string));
            data.Tables["HEADER"].Columns.Add("DATEGEN", typeof(DateTime));
            data.Tables["HEADER"].Columns.Add("SERIALPRINTER", typeof(string));

            data.Tables.Add("LINES");
            data.Tables["LINES"].Columns.Add("LOCNCODE", typeof(string));
            data.Tables["LINES"].Columns.Add("ITEMNMBR", typeof(string));
            data.Tables["LINES"].Columns.Add("UNITPRCE", typeof(Decimal));
            data.Tables["LINES"].Columns.Add("XTNDPRCE", typeof(Decimal));
            data.Tables["LINES"].Columns.Add("QUANTITY", typeof(Decimal));
            data.Tables["LINES"].Columns.Add("MRKDNAMT", typeof(Decimal));
            data.Tables["LINES"].Columns.Add("UOFM", typeof(string));
            data.Tables["LINES"].Columns.Add("USERDEF1", typeof(string));
            data.Tables["LINES"].Columns.Add("USERDEF2", typeof(string));
            data.Tables["LINES"].Columns.Add("USERDEF3", typeof(string));
            data.Tables["LINES"].Columns.Add("USERDEF4", typeof(string));
            data.Tables["LINES"].Columns.Add("USERDEF5", typeof(string));

            data.Tables.Add("PAYMENTS");
            data.Tables["PAYMENTS"].Columns.Add("DOCAMNT", typeof(Decimal));
            data.Tables["PAYMENTS"].Columns.Add("PYMTTYPE", typeof(int));
            data.Tables["PAYMENTS"].Columns.Add("CHEKBKID", typeof(string));
            data.Tables["PAYMENTS"].Columns.Add("CHEKNMBR", typeof(string));
            data.Tables["PAYMENTS"].Columns.Add("DOCDATE", typeof(DateTime));
            data.Tables["PAYMENTS"].Columns.Add("DOCNUMBR", typeof(string));
            data.Tables["PAYMENTS"].Columns.Add("CARDNAME", typeof(string));
            data.Tables["PAYMENTS"].Columns.Add("RCTNCCRD", typeof(string));
            data.Tables["PAYMENTS"].Columns.Add("EXPNDATE", typeof(DateTime));
            data.Tables["PAYMENTS"].Columns.Add("AUTHCODE", typeof(string));
            data.Tables["PAYMENTS"].Columns.Add("USERDEF1", typeof(string));
            data.Tables["PAYMENTS"].Columns.Add("USERDEF2", typeof(string));
            data.Tables["PAYMENTS"].Columns.Add("USERDEF3", typeof(string));
            data.Tables["PAYMENTS"].Columns.Add("USERDEF4", typeof(string));
            data.Tables["PAYMENTS"].Columns.Add("USERDEF5", typeof(string));

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(filename);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return data;
            }


            XmlNode header = doc.SelectSingleNode("/GPDocument/Head");

            DataRow HeaderRow = data.Tables["HEADER"].NewRow();

            foreach (XmlNode n in header.ChildNodes)
            {
                try
                {

                    switch (n.Name.Trim().ToUpper())
                    {
                        case "DOCUNMBE":

                            HeaderRow["DOCNUMBE"] = n.InnerText;
                            break;
                        case "DOCID":

                            HeaderRow["DOCID"] = n.InnerText;
                            break;
                        case "DOCNUMBE":

                            HeaderRow["DOCNUMBE"] = n.InnerText;
                            break;
                        case "ORIGNUMB":
                            HeaderRow["ORIGNUMB"] = n.InnerText;
                            break;
                        case "ORIGTYPE":
                            if (n.InnerText == "")
                                HeaderRow["ORIGTYPE"] = 0;
                            else
                                HeaderRow["ORIGTYPE"] = int.Parse(n.InnerText);
                            break;
                        case "TAXSCHID":
                            HeaderRow["TAXSCHID"] = n.InnerText;
                            break;
                        case "FRTSCHID":
                            HeaderRow["FRTSCHID"] = n.InnerText;
                            break;
                        case "MSCSCHID":
                            HeaderRow["MSCSCHID"] = n.InnerText;
                            break;
                        case "SHIPMTHD":
                            HeaderRow["SHIPMTHD"] = n.InnerText;
                            break;
                        case "TAXAMNT":
                            if (n.InnerText == "")
                                HeaderRow["TAXAMNT"] = 0;
                            else
                                HeaderRow["TAXAMNT"] = Decimal.Parse(n.InnerText.Replace(".", ".").Replace(",", "."));
                            break;
                        case "LOCNCODE":
                            HeaderRow["LOCNCODE"] = n.InnerText;
                            break;
                        case "DOCDATE":
                            if (n.InnerText == "")
                                HeaderRow["DOCDATE"] = "01-01-1991";
                            else
                                HeaderRow["DOCDATE"] = DateTime.Parse(n.InnerText);
                            break;
                        case "FREIGHT":
                            if (n.InnerText == "")
                                HeaderRow["FREIGHT"] = 0;
                            else
                                HeaderRow["FREIGHT"] = Decimal.Parse(n.InnerText.Replace(".", ".").Replace(",", "."));
                            break;
                        case "MISCAMNT":
                            if (n.InnerText == "")
                                HeaderRow["MISCAMNT"] = 0;
                            else
                                HeaderRow["MISCAMNT"] = Decimal.Parse(n.InnerText.Replace(".", ".").Replace(",", "."));
                            break;
                        case "TRDISAMT":
                            if (n.InnerText == "")
                                HeaderRow["TRDISAMT"] = 0;
                            else
                                HeaderRow["TRDISAMT"] = Decimal.Parse(n.InnerText.Replace(".", ".").Replace(",", "."));
                            break;
                        case "CUSTOMER":
                            HeaderRow["CUSTOMER"] = n.InnerText;
                            break;
                        case "CSTPONBR":
                            HeaderRow["CSTPONBR"] = n.InnerText;
                            break;
                        case "SUBTOTAL":
                            if (n.InnerText == "")
                                HeaderRow["SUBTOTAL"] = 0;
                            else
                                HeaderRow["SUBTOTAL"] = Decimal.Parse(n.InnerText.Replace(".", ".").Replace(",", "."));
                            break;
                        case "DOCAMNT":
                            if (n.InnerText == "")
                                HeaderRow["DOCAMNT"] = 0;
                            else
                                HeaderRow["DOCAMNT"] = Decimal.Parse(n.InnerText.Replace(".", ".").Replace(",", "."));
                            break;
                        case "SALSTERR":
                            HeaderRow["SALSTERR"] = n.InnerText;
                            break;
                        case "SLPRSNID":
                            HeaderRow["SLPRSNID"] = n.InnerText;
                            break;
                        case "FRTTXAMT":
                            if (n.InnerText == "")
                                HeaderRow["FRTTXAMT"] = 0;
                            else
                                HeaderRow["FRTTXAMT"] = Decimal.Parse(n.InnerText.Replace(".", ".").Replace(",", "."));
                            break;
                        case "COO":
                            HeaderRow["COO"] = n.InnerText;
                            break;
                        case "INVOICE":
                            HeaderRow["INVOICE"] = n.InnerText;
                            break;
                        case "DATEGEN":
                            if (n.InnerText == "")
                                HeaderRow["DATEGEN"] = "01-01-1991";
                            else
                                HeaderRow["DATEGEN"] = DateTime.Parse(n.InnerText);
                            break;
                        case "SERIALPRINTER":
                            HeaderRow["SERIALPRINTER"] = n.InnerText;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("El archivo está corrupto!." + ex.Message);
                   
                }
            }
            data.Tables["HEADER"].Rows.Add(HeaderRow);

            XmlNode lines = doc.SelectSingleNode("/GPDocument/Lines");
            foreach (XmlNode nde in lines.ChildNodes)
            {
                DataRow lnerow = data.Tables["LINES"].NewRow();

                foreach (XmlNode n in nde.ChildNodes)
                {
                    try
                    {
                        switch (n.Name.Trim().ToUpper())
                        {
                            case "LOCNCODE":
                                lnerow["LOCNCODE"] = n.InnerText;
                                break;
                            case "ITEMNMBR":
                                lnerow["ITEMNMBR"] = n.InnerText;
                                break;
                            case "UNITPRCE":
                                if (n.InnerText == "")
                                    lnerow["UNITPRCE"] = 0;
                                else
                                    lnerow["UNITPRCE"] = Decimal.Parse(n.InnerText.Replace(".", ".").Replace(",", "."));
                                break;
                            case "XTNDPRCE":
                                if (n.InnerText == "")
                                    lnerow["XTNDPRCE"] = 0;
                                else
                                    lnerow["XTNDPRCE"] = Decimal.Parse(n.InnerText.Replace(".", ".").Replace(",", "."));
                                break;
                            case "QUANTITY":
                                if (n.InnerText == "")
                                    lnerow["QUANTITY"] = 0;
                                else
                                    lnerow["QUANTITY"] = Decimal.Parse(n.InnerText.Replace(".", ".").Replace(",", "."));
                                break;
                            case "MRKDNAMT":
                                if (n.InnerText == "")
                                    lnerow["MRKDNAMT"] = 0;
                                else
                                    lnerow["MRKDNAMT"] = Decimal.Parse(n.InnerText.Replace(".", ".").Replace(",", "."));
                                break;
                            case "UOFM":
                                lnerow["UOFM"] = n.InnerText;
                                break;
                            case "USERDEF1":
                                lnerow["USERDEF1"] = n.InnerText;
                                break;
                            case "USERDEF2":
                                lnerow["USERDEF2"] = n.InnerText;
                                break;
                            case "USERDEF3":
                                lnerow["USERDEF3"] = n.InnerText;
                                break;
                            case "USERDEF4":
                                lnerow["USERDEF4"] = n.InnerText;
                                break;
                            case "USERDEF5":
                                lnerow["USERDEF5"] = n.InnerText;
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("El archivo está corrupto!." + ex.Message);                        
                    }
                }
                data.Tables["LINES"].Rows.Add(lnerow);
            }

            // Payments

            XmlNode payments = doc.SelectSingleNode("/GPDocument/Payments");
            foreach (XmlNode nde in payments.ChildNodes)
            {
                DataRow lnerow = data.Tables["PAYMENTS"].NewRow();

                foreach (XmlNode n in nde.ChildNodes)
                {
                    try
                    {
                        switch (n.Name.Trim().ToUpper())
                        {
                            case "DOCAMNT":
                                if (n.InnerText == "")
                                    lnerow["DOCAMNT"] = 0.00;
                                else
                                    lnerow["DOCAMNT"] = Decimal.Parse(n.InnerText.Replace(".", ".").Replace(",", "."));
                                break;
                            case "PYMTTYPE":
                                lnerow["PYMTTYPE"] = n.InnerText;
                                break;
                            case "CHEKBKID":
                                lnerow["CHEKBKID"] = n.InnerText;
                                break;
                            case "CHEKNMBR":
                                lnerow["CHEKNMBR"] = n.InnerText;
                                break;
                            case "DOCDATE":
                                if (n.InnerText == "")
                                    lnerow["DOCDATE"] = "1-1-1991";
                                else
                                    lnerow["DOCDATE"] = DateTime.Parse(n.InnerText);
                                break;
                            case "DOCNUMBR":
                                lnerow["DOCNUMBR"] = n.InnerText;
                                break;
                            case "CARDNAME":
                                lnerow["CARDNAME"] = n.InnerText;
                                break;
                            case "RCTNCCRD":
                                lnerow["RCTNCCRD"] = n.InnerText;
                                break;
                            case "EXPNDATE":
                                if (n.InnerText == "")
                                    lnerow["EXPNDATE"] = "1-1-1991";
                                else
                                    lnerow["EXPNDATE"] = DateTime.Parse(n.InnerText);
                                break;
                            case "AUTHCODE":
                                lnerow["AUTHCODE"] = n.InnerText;
                                break;
                            case "USERDEF1":
                                lnerow["USERDEF1"] = n.InnerText;
                                break;
                            case "USERDEF2":
                                lnerow["USERDEF2"] = n.InnerText;
                                break;
                            case "USERDEF3":
                                lnerow["USERDEF3"] = n.InnerText;
                                break;
                            case "USERDEF4":
                                lnerow["USERDEF4"] = n.InnerText;
                                break;
                            case "USERDEF5":
                                lnerow["USERDEF5"] = n.InnerText;
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("El archivo está corrupto!." + ex.Message);
                       // ((IEvent)(new ErrorEvent("", "", "El archivo está corrupto!." + ex.Message))).Publish();
                    }
                }
                data.Tables["PAYMENTS"].Rows.Add(lnerow);
            }



            return data;

        }
    }
}
