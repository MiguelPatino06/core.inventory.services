using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using TShirt.InventoryApp.Services.Models;

namespace TShirt.InventoryApp.Web.Documents
{
    public class Inventory
    {
        public string GenerateDocument(Document document)
        {

            string eConnectConnectionString =
                string.Format(
                    "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TSGVL;Data Source=10.1.92.203;user id=applegreen;password=galapago;Trusted_Connection=True");

            XDocument doc = new XDocument(new XElement("eConenct",
                new XElement("IVInventoryTransactionType",
                    new XElement("taIVTransactionHeaderInsert",
                        new XElement("BACHNUMB", "LOTEGR"),
                        new XElement("IVDOCNBR", document.Id),
                        new XElement("IVDOCTYP", 2),
                        new XElement("DOCDATE", "2017-08-04 21:41:09"),
                        new XElement("POSTTOGL", string.Empty),
                        new XElement("MDFUSRID", string.Empty),
                        new XElement("PTDUSRID", string.Empty),
                        new XElement("POSTEDDT", string.Empty),
                        new XElement("NOTETEXT", string.Empty),
                        new XElement("USRDEFND1", string.Empty),
                        new XElement("USRDEFND2", string.Empty),
                        new XElement("USRDEFND3", string.Empty),
                        new XElement("USRDEFND4", string.Empty),
                        new XElement("USRDEFND5", string.Empty)),
                    new XElement("taIVTransactionLineInsert_Items", from c in document.Details
                        select
                            new XElement("taIVTransactionLineInsert",
                                new XElement("IVDOCNBR", c.DocumentId),
                                new XElement("IVDOCTYP", string.Empty),
                                new XElement("ITEMNMBR", c.ProductCode),
                                new XElement("Reason_Code", string.Empty),
                                new XElement("LNSEQNBR", string.Empty),
                                new XElement("UOFM", string.Empty),
                                new XElement("TRXQTY", c.Quantity),
                                new XElement("UNITCOST", string.Empty),
                                new XElement("TRXLOCTN", string.Empty),
                                new XElement("IVIVINDX", string.Empty),
                                new XElement("InventoryAccount", string.Empty),
                                new XElement("IVIVOFIX", string.Empty),
                                new XElement("InventoryAccountOffSet", string.Empty)
                                )))));

            return doc.ToString();

        }
    }
}