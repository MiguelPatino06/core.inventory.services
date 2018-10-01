using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
//using TShirt.InventoryApp.Api.Services;
//using TShirt.InventoryApp.Api.Properties;
//using TShirt.InventoryApp.Api.Utils;
using System.Diagnostics;
using TShirt.InventoryApp.WCF.Interface;
using TShirt.InventoryApp.WCF.Models.Count;
using TShirt.InventoryApp.WCF.Services;

namespace TShirt.InventoryApp.WCF.Utils
{
  public class XmlCountPlan
  {
    private readonly ICountRepository _countRepository = new CountRepository();

    public bool Create(int id)
    {

      bool success = false;

      try
      {
        //Get IVDOCNBR
        Common common = new Common();
        string IVDOCNBR = string.Format("AJP{0}", common.GetNextNumber("AJP"));
        string folderPath = ConfigurationManager.AppSettings["XmlFolderPath"].ToString();

        List<Contract.CountPlanDetail> detail = _countRepository.GetById(id);

        //Wrapper
        XDocument doc =
          new XDocument(
            new XElement("eConnect",
              new XElement("IVInventoryTransactionType",
                new XElement("taIVTransactionLotInsert_Items", string.Empty),
                new XElement("taIVTransactionLineInsert_Items", string.Empty),
                new XElement("taIVTransactionHeaderInsert",
                  new XElement("BACHNUMB", "TSHIRT"),
                  new XElement("IVDOCNBR", IVDOCNBR),
                  new XElement("IVDOCTYP", 1),
                  new XElement("DOCDATE", DateTime.Now.ToShortDateString()),
                  new XElement("POSTTOGL", 0),
                  new XElement("USRDEFND1", string.Empty),
                  new XElement("USRDEFND2", string.Empty),
                  new XElement("USRDEFND3", string.Empty),
                  new XElement("USRDEFND4", string.Empty),
                  new XElement("USRDEFND5", string.Empty)
                )
              )
            )
          );

        foreach (Contract.CountPlanDetail row in detail)
        {
          XElement lot = new XElement("taIVTransactionLotInsert",
            new XElement("IVDOCNBR", IVDOCNBR),
            new XElement("IVDOCTYP", 1),
            new XElement("ITEMNMBR", row.ProductCode),
            new XElement("SERLTNUMBR", "AJP"),
            new XElement("LOTNUMBR", string.Format("AJP{0}", DateTime.Now.ToString("yyyyMMddTHHmmss"))),
            new XElement("SERLTQTY", row.Quantity),
            new XElement("LOCNCODE", "TSHIRT"));

          doc.Descendants().Where(p => p.Name.LocalName == "taIVTransactionLotInsert_Items").FirstOrDefault().Add(lot);

          XElement line = new XElement("taIVTransactionLineInsert",
            new XElement("IVDOCNBR", IVDOCNBR),
            new XElement("IVDOCTYP", 1),
            new XElement("ITEMNMBR", row.ProductCode),
            new XElement("Reason_Code", row.ProductCode),
            new XElement("LNSEQNBR", string.Empty),
            new XElement("TRXQTY", row.TotalCounted),
            new XElement("UNITCOST", string.Empty),
            new XElement("TRXLOCTN", "FLEXO"),
            new XElement("IVIVINDX", string.Empty),
            new XElement("InventoryAccount", string.Empty),
            new XElement("IVIVOFIX", string.Empty),
            new XElement("InventoryAccountOffSet", string.Empty));

          doc.Descendants().Where(p => p.Name.LocalName == "taIVTransactionLineInsert_Items").FirstOrDefault().Add(line);
        }

        doc.Save(string.Format("{0}CountPlan{1}.xml", folderPath, DateTime.Now.ToString("yyyyMMddTHHmmss")));

        success = true;
      }
      catch (Exception ex)
      {
        Debug.WriteLine(@"				ERROR {0}", ex.Message);
      }

      return success;
    }

  }
}