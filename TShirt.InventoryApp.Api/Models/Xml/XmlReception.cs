using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using TShirt.InventoryApp.Api.Services;
using TShirt.InventoryApp.Api.Properties;
using TShirt.InventoryApp.Api.Utils;
using System.Diagnostics;

namespace TShirt.InventoryApp.Api.Models.Xml
{
  public class XmlReception
  {
    private readonly IOrderRepository _orderRepository = new OrderRepository();

    public string Create(string[] code)
    {

      string success = string.Empty;

      try
      {
        //Get IVDOCNBR
        Common common = new Common();
        string IVDOCNBR = common.getNextNumber("POPRCTNM", "POP40100", "INDEX1", "1");
        char[] MyChar = { 'R', 'C', 'T'};

        string codeRct = IVDOCNBR.TrimStart(MyChar).Trim();

         int numberRct = int.Parse(codeRct);
        //IVDOCNBR = IVDOCNBR.TrimStart(MyChar);

        List<ViewOrderExtend> detail = _orderRepository.GetOrdersDetailsArray(code).ToList();

        //Wrapper
        XDocument doc =
          new XDocument(
            new XElement("eConnect",
              new XElement("POPReceivingsType",
                new XElement("taPopRcptLotInsert_Items", string.Empty),
                new XElement("taPopRcptLineInsert_Items", string.Empty),
                new XElement("taPopRcptHdrInsert",
                  new XElement("POPRCTNM", "RCT" + numberRct.ToString()),
                  new XElement("POPTYPE", 3),
                  new XElement("VNDDOCNM", 3), //TODO falta
                  new XElement("receiptdate", DateTime.Now.ToShortDateString()),
                  new XElement("BACHNUMB", detail.FirstOrDefault().Value2),
                  new XElement("VENDORID", detail.FirstOrDefault().ProviderCode.Trim())
                )
              )
            )
          );

        int sequence = 0;
          int sequenceLine = 0;
        foreach (ViewOrderExtend row in detail)
        {

          if (row.TotalProduct > 0)
          {
            sequence += 1;
            sequenceLine = 16384*sequence;
            XElement lot = new XElement("taPopRcptLotInsert",
            new XElement("POPRCTNM", "RCT" + numberRct.ToString()),
            new XElement("ITEMNMBR", row.ProductCode.Trim()),
            new XElement("SERLTNUM", sequenceLine),
            new XElement("SERLTQTY", row.TotalProduct),
            new XElement("RCPTLNNM", sequenceLine));

            doc.Descendants().Where(p => p.Name.LocalName == "taPopRcptLotInsert_Items").FirstOrDefault().Add(lot);

            XElement line = new XElement("taPopRcptLineInsert",
              new XElement("POPTYPE", 3),
              new XElement("POPRCTNM", "RCT" + numberRct.ToString()),
              new XElement("PONUMBER", row.Code),
              new XElement("ITEMNMBR", row.ProductCode),
              new XElement("VENDORID", row.ProviderCode),
              new XElement("VNDITNUM", row.OrderValue3),
              new XElement("QTYSHPPD", row.TotalProduct),
              new XElement("QTYINVCD", row.TotalProduct),
              new XElement("AUTOCOST", 1),
              new XElement("LOCNCODE", row.OrderValue1));

            doc.Descendants().Where(p => p.Name.LocalName == "taPopRcptLineInsert_Items").FirstOrDefault().Add(line);
          }
        }

        doc.Save(string.Format("{0}RCT{1}.xml", Settings.Default.XmlFolderPath, DateTime.Now.ToString("yyyyMMddTHHmmss")));

        success = "RCT" + numberRct;
      }
      catch (Exception ex)
      {
        //Debug.WriteLine(@"				ERROR {0}", ex.Message);
          success = "ERROR " + ex.Message;
      }

      return success;
    }

  }
}