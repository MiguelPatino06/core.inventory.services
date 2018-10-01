using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using TShirt.InventoryApp.Api.Services;
using TShirt.InventoryApp.Api.Properties;
using TShirt.InventoryApp.Api.Utils;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.Ajax.Utilities;
using TShirt.InventoryApp.Api.Helpers;

namespace TShirt.InventoryApp.Api.Models.Xml
{
  public class XmlCountPlan
  {
    private readonly ICountRepository _countRepository = new CountRepository();

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
            string IVDOCNBR = string.Format("AJP{0}",
                common.getNextNumber("selSI_NextNumber", "selSI_IV40100", "selSI_IV_TYPE_ID", "AJP"));

            List<ViewCountPlanDetail> detail = _countRepository.GetById(id);

            //Wrapper
            XDocument doc =
                new XDocument(
                    new XElement("eConnect",
                        new XElement("IVInventoryTransactionType",
                            new XElement("taIVTransactionLotInsert_Items", string.Empty),
                            new XElement("taIVTransactionLineInsert_Items", string.Empty),
                            new XElement("taIVTransactionHeaderInsert",
                                new XElement("BACHNUMB", "AJP" + DateTime.Now.ToString("ddMMHHmmss")),
                                new XElement("IVDOCNBR", IVDOCNBR),
                                new XElement("IVDOCTYP", 1),
                                new XElement("DOCDATE", DateTime.Now.ToShortDateString())
                                )
                            )
                        )
                    );

            List<ViewCountPlanDetail> itemsList = detail.Where(a => a.TotalProduct > 0).ToList();

            foreach (ViewCountPlanDetail row in itemsList)
            {
                if ((row.TotalProduct > 0) && (row.Quantity != row.TotalProduct))
                {
                    //Evaluamos si se incrementa o disminuye la cantidad
                    int qty;
                    int adjType;
                    bool negative = true;
                    lote = "CONT" + DateTime.Now.ToString("dd/MM/yy");
                    if (row.TotalProduct > row.Quantity) //Valor positivo
                    {
                        //adjType = 1; //Decrease
                        negative = false;
                        //if (row.Quantity == 0)
                        //{
                        adjType = 0;
                        lote = "CONT" + DateTime.Now.ToString("dd/MM/yy");
                        dateRec = DateTime.Now.ToString("dd/MM/yy");
                        qty = (row.TotalProduct - row.Quantity);
                        createLot = 1;
                        //}
                        //else
                        //{
                        //    qty = (row.TotalProduct - row.Quantity);
                        //    queryGroup = GetLotIV00300(row.ProductCode.Trim(), row.Warehouse.Trim(),
                        //        Decimal.Parse(qty.ToString()));
                        //    if (queryGroup == null) //if not existe lot 
                        //    {
                        //        adjType = 0;
                        //        lote = "CONT" + DateTime.Now.ToString("dd/MM/yy");
                        //        dateRec = DateTime.Now.ToString("dd/MM/yy");
                        //        createLot = 1;
                        //    }
                        //    else
                        //    {
                        //        adjType = 0;
                        //        lote = queryGroup.lotnumbr.Trim();
                        //        dateRec = queryGroup.DATERECD.ToString("dd/MM/yy HH:mm:ss");
                        //        createLot = 0;
                        //    }
                        //}

                    }
                    else //Valor Negativo
                    {

                        adjType = 0; //Increase

                        //if (row.Quantity == 0)
                        //{
                        //    adjType = 1;
                        //    lote = "CONT" + DateTime.Now.ToString("dd/MM/yy");
                        //    qty = row.TotalProduct;
                        //    negative = false;
                        //    createLot = 1;
                        //}
                        //else
                        //{
                        bool createItemPositive = false;
                        bool createLineX = false;
                        qty = (row.Quantity - row.TotalProduct);
                        queryGroup = GetLotIV00300(row.ProductCode.Trim(), row.Warehouse.Trim(), qty);


                        if (queryGroup == null) //if not existe lot 
                        {
                            adjType = 0;
                            lote = "CONT" + DateTime.Now.ToString("dd/MM/yy");
                            createLot = 1;
                            negative = false;
                        }
                        else
                        {
                            if (queryGroup.Count == 1)
                            {
                                createItemPositive = queryGroup.FirstOrDefault().IsPositive;
                                createLineX = queryGroup.FirstOrDefault().EqualQuantiy;

                            }


                            if (createLineX)
                            {
                                negative = true;
                            }
                            else if (createItemPositive)
                            {
                                qty = Convert.ToInt32(queryGroup.FirstOrDefault().qtyrecvd);
                                adjType = 0;
                                lote = "CONT" + DateTime.Now.ToString("dd/MM/yy");
                                createLot = 1;
                                negative = false;
                            }
                            else
                            {
                                adjType = 1;
                                negative = true;
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
                                        new XElement("TRXQTY", (item.qtyrecvd*-1).ToString().Replace(".00000","")),
                                        new XElement("TRXLOCTN", row.Warehouse.Trim()));

                                    doc.Descendants()
                                        .Where(p => p.Name.LocalName == "taIVTransactionLineInsert_Items")
                                        .FirstOrDefault()
                                        .Add(line);

                                }
                            }
                        }
                    }

                    if (negative == false)
                    {
                        XElement lot = new XElement("taIVTransactionLotInsert",
                            new XElement("IVDOCNBR", IVDOCNBR),
                            new XElement("IVDOCTYP", 1),
                            new XElement("ITEMNMBR", row.ProductCode),
                            new XElement("LOTNUMBR", lote),
                            new XElement("SERLTQTY", qty.ToString().Replace(".00000", "")),
                            new XElement("ADJTYPE", adjType),
                            new XElement("LOCNCODE", row.Warehouse.Trim()),
                            new XElement("DATERECD", dateRec),
                            new XElement("AUTOCREATELOT", createLot));

                        doc.Descendants()
                            .Where(p => p.Name.LocalName == "taIVTransactionLotInsert_Items")
                            .FirstOrDefault()
                            .Add(lot);

                        XElement line = new XElement("taIVTransactionLineInsert",
                            new XElement("IVDOCNBR", IVDOCNBR),
                            new XElement("IVDOCTYP", 1),
                            new XElement("ITEMNMBR", row.ProductCode),
                            new XElement("TRXQTY", (negative) ? (qty*-1).ToString().Replace(".00000", "") : qty.ToString().Replace(".00000", "")),
                            new XElement("TRXLOCTN", row.Warehouse.Trim()));

                        doc.Descendants()
                            .Where(p => p.Name.LocalName == "taIVTransactionLineInsert_Items")
                            .FirstOrDefault()
                            .Add(line);

                    }


                }
            }

            doc.Save(string.Format("{0}AJP{1}.xml", Settings.Default.XmlFolderPath,
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

    public class SumIV
    {

        public decimal qtyrecvd { get; set; }
        public string lotnumbr { get; set;  }
        public DateTime DATERECD { get; set; }
        public bool IsPositive { get; set; }
        public bool  EqualQuantiy { get; set; }
    }
}