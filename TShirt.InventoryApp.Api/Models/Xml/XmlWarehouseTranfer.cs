using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml.Linq;
using TShirt.InventoryApp.Api.Properties;
using TShirt.InventoryApp.Api.Services;
using TShirt.InventoryApp.Api.Utils;

namespace TShirt.InventoryApp.Api.Models.Xml
{
    public class XmlWarehouseTranfer
    {

        private readonly IProductTransferRepository _repository = new ProductTransferRepository(); 
        public string Create(int code)
        {

            string success = string.Empty;

            try
            {
                //Get IVDOCNBR
                Common common = new Common();
                string IVDOCNBR = string.Format("TXI{0}", common.getNextNumber("selSI_NextNumber", "selSI_IV40100", "selSI_IV_TYPE_ID", "TXI"));


                TransferDetail result = _repository.GetById(code);

                //Wrapper
                XDocument doc =
                  new XDocument(
                    new XElement("eConnect",
                      new XElement("IVInventoryTransferType",
                        new XElement("taIVTransferLotInsert_Items", string.Empty),
                        new XElement("taIVTransferLineInsert_Items", string.Empty),
                        new XElement("taIVTransferHeaderInsert",
                          new XElement("BACHNUMB", IVDOCNBR),
                          new XElement("IVDOCNBR", IVDOCNBR),
                          new XElement("DOCDATE", DateTime.Now.ToShortDateString())
                        )
                      )
                    )
                  );

                int sequence = 0;
                int sequenceLine = 0;
                foreach (var row in result.products)
                {

                    var _lot = GetLotIV00300(row.ProductCode.Trim(), result.WarehouseOrigin);
                        sequence += 1;
                        //sequenceLine = 16384 * sequence;
                        XElement lot = new XElement("taIVTransferLotInsert",
                        new XElement("IVDOCNBR", IVDOCNBR),
                        new XElement("ITEMNMBR", row.ProductCode),
                        new XElement("LOTNUMBR", _lot.lotnumbr.Trim()),
                        new XElement("SERLTQTY", row.Quantity),
                        new XElement("LOCNCODE", result.WarehouseOrigin),
                        new XElement("QTYTYPE", 1),
                        new XElement("TOLOCNCODE", result.WarehouseDestiny));

                        doc.Descendants().Where(p => p.Name.LocalName == "taIVTransferLotInsert_Items").FirstOrDefault().Add(lot);

                        XElement line = new XElement("taIVTransferLineInsert",
                          new XElement("IVDOCNBR", IVDOCNBR),
                          new XElement("ITEMNMBR", row.ProductCode.Trim()),
                          new XElement("TRXQTY", row.Quantity),
                          new XElement("TRXLOCTN", result.WarehouseOrigin),
                          new XElement("TRNSTLOC", result.WarehouseDestiny));

                        doc.Descendants().Where(p => p.Name.LocalName == "taIVTransferLineInsert_Items").FirstOrDefault().Add(line);
                    //}
                }

                doc.Save(string.Format("{0}TXI{1}.xml", Settings.Default.XmlFolderPath, DateTime.Now.ToString("yyyyMMddHHmmss")));

                success = IVDOCNBR;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(@"				ERROR {0}", ex.Message);
                success = "ERROR";
            }

            return success;
        }



        private SumIV GetLotIV00300(string product, string warehouse)
        {
            SumIV sumiv = new SumIV();
            decimal tot = 0;

            CultureInfo culture;
            culture = CultureInfo.CreateSpecificCulture("es-PA");
            Thread.CurrentThread.CurrentCulture = culture;

            try
            {
                using (TSGVLEntities db = new TSGVLEntities())
                {
                    db.Database.CommandTimeout = 0;

                    var qry = "SELECT sum(QTYRECVD - QTYSOLD - ATYALLOC) as qtyrecvd, lotnumbr, DATERECD";
                    qry += " FROM IV00300";
                    qry += " WHERE itemnmbr='" + product + "' and locncode='" + warehouse + "' and qtytype='1' and ((QTYRECVD - QTYSOLD - ATYALLOC) > 0)";
                    qry += " GROUP BY itemnmbr, lotnumbr, DATERECD";

                    var result = db.Database.SqlQuery<SumIV>(qry).ToList();

                    sumiv = result.Count > 1 ? result.LastOrDefault() : result.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                sumiv = null;
                Debug.Write(@"Error " + ex.Message);
            }


            return sumiv;
        }

    }
}