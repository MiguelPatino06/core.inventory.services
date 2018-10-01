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
    public class PriceList
    {
        #region Atributos
        private string mAlmacen = string.Empty;

        private string mCodigo = string.Empty;

        private string mDescripcion = string.Empty;

        private string mNombreLista = string.Empty;

        private string mUofM = string.Empty;

        private string mCantidad = "0";

        private decimal mPrecioNeto = 0;

        private decimal mPrecioITBMS = 0;

        private decimal mCosto = 0;

        private decimal mPrecioConITBMS = 0;

        private int mTipo = 0;
        #endregion Atributos

        #region Propiedades
        public string Almacen { get { return mAlmacen; }set { mAlmacen = value; } }
        
        public string Codigo { get { return mCodigo; }set { mCodigo = value; } }
        
        public string Descripcion { get { return mDescripcion; }set { mDescripcion = value; } }
        
        public string NombreLista { get { return mNombreLista; } set { mNombreLista = value; } }
        
        public string UofM { get { return mUofM; }set { mUofM = value; } }
        
        public string Cantidad { get { return mCantidad; }set { mCantidad = value; } }
        
        public decimal PrecioNeto { get { return mPrecioNeto; }set { mPrecioNeto = value; } }
        
        public decimal PrecioITBMS { get { return mPrecioITBMS; }set { mPrecioITBMS = value; } }
        
        public decimal Costo { get { return mCosto; }set { mCosto = value; } }
        
        public decimal PrecioConITBMS { get { return mPrecioConITBMS; }set { mPrecioConITBMS = value; } }
        
        public int Tipo { get { return mTipo; }set { mTipo = value; } }
        #endregion Propiedades

        #region Utilidades

        // Convierte la clase en una cadena serializada en XML
        override public string ToString()
        {
            string tResult = String.Empty;
            try
            {
                XmlSerializer xmlCustomerSerializer = new XmlSerializer(typeof(PriceList));

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

        // Guarda un Price List en un XML
        public static bool SaveAs(string filename, PriceList item)
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


        // Carga un Price List desde un XML
        public static PriceList LoadFrom(string filename)
        {
            PriceList tPriceList = null;

            try
            {
                var serializer = new XmlSerializer(typeof(PriceList));
                FileStream fs = new FileStream(filename, FileMode.Open);
                StreamReader stream = new StreamReader(fs, Encoding.Unicode);
                tPriceList = (PriceList)serializer.Deserialize(stream);
                fs.Close();
            }
            catch
            { }

            return tPriceList;
        }


        #endregion Utilidades
    }


    [Serializable]
    public class Oferts
    {
        private List<PriceList> mItemAvailability = new List<PriceList>();

        [XmlArray("Items"), XmlArrayItem(typeof(PriceList), ElementName = "Price")]
        public List<PriceList> Items
        {
            get
            {
                return mItemAvailability;
            }
            set
            {
               
                mItemAvailability.AddRange(value);
            }
        }

        public void Add(PriceList pl)
        {
            mItemAvailability.Add(pl);
        }

        #region Utilidades

        // Convierte la clase en una cadena serializada en XML
        override public string ToString()
        {
            string tResult = String.Empty;
            try
            {
                XmlSerializer xmlCustomerSerializer = new XmlSerializer(typeof(Oferts));

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

        // Guarda un Oferts en un XML
        public static bool SaveAs(string filename, Oferts oferts)
        {
            bool tResult = false;
            try
            {
                System.IO.File.WriteAllText(filename, oferts.ToString(),Encoding.Unicode);
                tResult = true;
            }
            catch
            {
                tResult = false;
            }

            return tResult;
        }


        // Carga un Oferts desde un XML
        public static Oferts LoadFrom(string filename)
        {
            Oferts tOferts = null;

            try
            {
                var serializer = new XmlSerializer(typeof(Oferts));
                FileStream fs = new FileStream(filename, FileMode.Open);
                StreamReader stream = new StreamReader(fs, Encoding.Unicode);
                tOferts = (Oferts)serializer.Deserialize(stream);
                fs.Close();
            }
            catch
            { }

            return tOferts;
        }


        #endregion Utilidades
    }
}
