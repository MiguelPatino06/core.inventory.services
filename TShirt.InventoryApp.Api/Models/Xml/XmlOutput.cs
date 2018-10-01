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
    public class XmlOutput
    {
        private readonly IOutputRepository _outRepository = new OutputRepository();

        public bool Create(int id)
        {
            bool success = false;
            List<SumIV> queryGroup = new List<SumIV>();
            string dateRec = string.Empty; //Date Recepcion
            int createLot = 0;
            string lote = string.Empty;

            try
            {
                //Get IVDOCNBR
                Common common = new Common();
                string IVDOCNBR = string.Format("MST{0}",
                    common.getNextNumber("selSI_NextNumber", "selSI_IV40100", "selSI_IV_TYPE_ID", "MST"));

                Output _output = _outRepository.GetById(id);

                //Wrapper
                XDocument doc =
                    new XDocument(
                        new XElement("eConnect",
                            new XElement("IVInventoryTransactionType",
                                new XElement("taIVTransactionLotInsert_Items", string.Empty),
                                new XElement("taIVTransactionLineInsert_Items", string.Empty),
                                new XElement("taIVTransactionHeaderInsert",
                                    new XElement("BACHNUMB", "MST" + DateTime.Now.ToString("ddMMHHmmss")),
                                    new XElement("IVDOCNBR", IVDOCNBR),
                                    new XElement("IVDOCTYP", 1),
                                    new XElement("DOCDATE", DateTime.Now.ToShortDateString())
                                    )
                                )
                            )
                        );

                List<OutputDetail> itemsList = _output.Details.Where(a => a.Quantity > 0).ToList();

                foreach (OutputDetail row in itemsList)
                {

                    //Evaluamos si se incrementa o disminuye la cantidad
                    int qty;
                    int adjType;
                    bool negative = true;
                    lote = "MST" + DateTime.Now.ToString("dd/MM/yy");

                    bool createItemPositive = false;
                    bool createLineX = false;
                    //qty = (row.Quantity);
                    queryGroup = GetLotIV00300(row.ProductCode.Trim(), row.Warehouse.Trim(), row.Quantity);



                    adjType = 1;
                    createLot = 0;

                    foreach (var item in queryGroup)
                    {
                        XElement lot = new XElement("taIVTransactionLotInsert",
                            new XElement("IVDOCNBR", IVDOCNBR),
                            new XElement("IVDOCTYP", 1),
                            new XElement("ITEMNMBR", row.ProductCode),
                            new XElement("LOTNUMBR", item.lotnumbr.Trim()),
                            new XElement("SERLTQTY", item.qtyrecvd.ToString().Replace(".00000", "")),
                            new XElement("ADJTYPE", adjType),
                            new XElement("LOCNCODE", row.Warehouse.Trim()),
                            new XElement("DATERECD", item.DATERECD.ToString("dd/MM/yy HH:mm:ss")),
                            new XElement("AUTOCREATELOT", createLot));

                        doc.Descendants()
                            .Where(p => p.Name.LocalName == "taIVTransactionLotInsert_Items")
                            .FirstOrDefault()
                            .Add(lot);

                        XElement line = new XElement("taIVTransactionLineInsert",
                            new XElement("IVDOCNBR", IVDOCNBR),
                            new XElement("IVDOCTYP", 1),
                            new XElement("ITEMNMBR", row.ProductCode),
                            new XElement("TRXQTY", (item.qtyrecvd * -1).ToString().Replace(".00000", "")),
                            new XElement("TRXLOCTN", row.Warehouse.Trim()));

                        doc.Descendants()
                            .Where(p => p.Name.LocalName == "taIVTransactionLineInsert_Items")
                            .FirstOrDefault()
                            .Add(line);
                    }
                }

                doc.Save(string.Format("{0}MST{1}.xml", Settings.Default.XmlFolderPath,
                    DateTime.Now.ToString("yyyyMMddTHHmmss")));

                success = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return success;

        }

        private List<SumIV> GetLotIV00300(string product, string warehouse, decimal qty)
        {
            List<SumIV> sumiv = new List<SumIV>();
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

                    decimal sumProduct = result.Sum(x => x.qtyrecvd); // determina si hay cantidad suficiente en los lotes de los productos

                    if (result != null)
                    {
                        //Si la cantidad es igual a la del inventario no se genera linea e documento
                        if (sumProduct == qty)
                        {
                            sumiv.Add(new SumIV()
                            {
                                lotnumbr = null,
                                qtyrecvd = 0,
                                DATERECD = DateTime.Now,
                                IsPositive = false,
                                EqualQuantiy = true,
                            });
                        }
                        else if (qty > sumProduct) //Si la cantidad es mayor a la existente en inventario genera un ajusta positivo con la diferencia
                        {
                            sumiv.Add(new SumIV()
                            {
                                lotnumbr = null,
                                qtyrecvd = qty - sumProduct,
                                DATERECD = DateTime.Now,
                                IsPositive = true,
                                EqualQuantiy = false,
                            });
                        }
                        else
                        {
                            foreach (var items in result)
                            {
                                sumiv.Add(new SumIV()
                                {
                                    lotnumbr = items.lotnumbr.Trim(),
                                    qtyrecvd = (items.qtyrecvd <= qty) ? items.qtyrecvd : qty,
                                    DATERECD = items.DATERECD,
                                    IsPositive = false,
                                    EqualQuantiy = false
                                });


                                if (items.qtyrecvd < qty)
                                {
                                    qty = qty - items.qtyrecvd;
                                }
                                else
                                {
                                    break;
                                }

                            }
                            var firstOrDefault = sumiv.FirstOrDefault();
                            if (firstOrDefault != null && string.IsNullOrEmpty(firstOrDefault.lotnumbr.Trim()))
                                sumiv = null;
                        }
                    }
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