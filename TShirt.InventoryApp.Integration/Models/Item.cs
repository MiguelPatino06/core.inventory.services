using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TShirt.InventoryApp.Integration.Models
{
	[Serializable]
	public class Item
	{
        #region Atributos
        private string mNo = string.Empty;
        private string mDescription = string.Empty;
        private string mDescripcion2 = string.Empty;
        private decimal mVAT = 0;
        private decimal mPricewithoutVAT = 0;
        private decimal mPriceincludingVAT = 0;
        private decimal mQty_Inventory = 1;
        #endregion Atributos

        #region Propiedades
        public string No { get { return mNo; } set { mNo = value; } }
        public string Description { get { return mDescription; } set { mDescription = value; } }
        public string Descripcion2 { get { return mDescripcion2; }set { mDescripcion2 = value; } }
        public decimal VAT { get { return mVAT; } set { mVAT = value; } }
        public decimal PricewithoutVAT { get { return mPricewithoutVAT; }set { mPricewithoutVAT = value; } }
        public decimal PriceincludingVAT { get { return mPriceincludingVAT; }set { mPriceincludingVAT = value; } }
        public decimal Qty_Inventory { get { return mQty_Inventory; }set { mQty_Inventory = value; } }
        #endregion Propiedades

        #region Utilidades

        // Convierte la clase en una cadena serializada en XML
        override public string ToString()
        {
            string tResult = String.Empty;
            try
            {
                XmlSerializer xmlCustomerSerializer = new XmlSerializer(typeof(Item));

                StringWriter stringWriter = new StringWriter();
                XmlWriter writer = XmlWriter.Create(stringWriter);
                xmlCustomerSerializer.Serialize(writer, this);
                tResult = stringWriter.ToString();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return tResult;

        }

        // Guarda un item en un XML
        public static bool SaveAs(string filename, Item item)
        {
            bool tResult = false;
            try
            {
                System.IO.File.WriteAllText(filename, item.ToString(),Encoding.Unicode);
                tResult = true;
            }
            catch
            {
                tResult = false;
            }

            return tResult;
        }


        // Carga un item desde un XML
        public static Item LoadFrom(string filename)
        {
            Item tItem = null;

            try
            {
                var serializer = new XmlSerializer(typeof(Item));
                FileStream fs = new FileStream(filename, FileMode.Open);
                StreamReader stream = new StreamReader(fs, Encoding.Unicode);
                tItem = (Item)serializer.Deserialize(stream);
                fs.Close();
            }
            catch
            { }

            return tItem;
        }


        #endregion Utilidades
    }
}
