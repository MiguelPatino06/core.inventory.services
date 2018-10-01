using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TShirt.InventoryApp.Integration.Models
{
    public class Warehouse
    {
        #region Atributos
        private string mCodigo = string.Empty;
        private string mDescripcion = string.Empty;
        #endregion Atributos

        #region Propiedades
        public string Codigo
        { get { return mCodigo; } set { mCodigo = value; } }

        public string Descripcion { get { return mDescripcion; } set { mDescripcion = value; } }
        #endregion Propiedades

        #region Utilidades

        // Convierte la clase en una cadena serializada en XML
        override public string ToString()
        {
            string tResult = String.Empty;
            try
            {
                XmlSerializer xmlCustomerSerializer = new XmlSerializer(typeof(Warehouse));

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
        public static bool SaveAs(string filename, Warehouse warehouse)
        {
            bool tResult = false;
            try
            {
                System.IO.File.WriteAllText(filename, warehouse.ToString(),Encoding.Unicode);
                tResult = true;
            }
            catch
            {
                tResult = false;
            }

            return tResult;
        }


        // Carga un item desde un XML
        public static Warehouse LoadFrom(string filename)
        {
            Warehouse tWarehouse = null;

            try
            {
                var serializer = new XmlSerializer(typeof(Warehouse));
                FileStream fs = new FileStream(filename, FileMode.Open);
                StreamReader stream = new StreamReader(fs, Encoding.Unicode);
                tWarehouse = (Warehouse)serializer.Deserialize(stream);
                fs.Close();
            }
            catch
            { }

            return tWarehouse;
        }


        #endregion Utilidades
    }
}
