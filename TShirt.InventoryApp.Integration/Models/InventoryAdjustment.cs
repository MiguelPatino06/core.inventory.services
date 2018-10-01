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
    public class ItemAvailability
    {
        #region Atributos
        /* document */
        private string mDocumento = string.Empty;

        /* realizador */
        private string mRealizador = string.Empty;

        /* motivo */
        private string mMotivo = "ESTATUS ACTUAL DE INVENTARIO";


        /* fecha */
        private string mFecha = "1900-01-01";

        /* hora */
        private string mHora = "00:00:00";

        /* codigo */
        private string mCodigo = string.Empty;

        /* nombre */
        private string mNombre = string.Empty;

        /* grupo */
        private string mGrupo = string.Empty;

        /* tipoproceso */
        private int mTipoproceso = 1;

        /* codigoalmacen */
        private string mCodigoalmacen=string.Empty;

        /* usuario */
        private string mDescripcion=string.Empty;

        /* ubicacion */
        private string mUsuario="UNKNOWN";

        /* descripción */
        private string mUbicacion=string.Empty;

        /* precio */
        private decimal mPrecio_neto = 0;

        /* cost */
        private decimal mPrecio_con_itbms = 0;

        /* costo */
        private decimal mCosto = 0;

        private decimal mPorcentaje_itbms = 0;

        private int mTipo = 0;

        private decimal mCantidad = 0;

        private string mUnidad_de_medida = string.Empty;
        #endregion Atributos

        #region Propiedades
        /* document */
        public string Documento { get { return mDocumento; }set { mDocumento = value; } }

        /* realizador */
        public string Realizador { get { return mRealizador; }set { mRealizador = value; } }

        /* motivo */
        public string Motivo { get { return mMotivo; }set { mMotivo = value; } }

        /* fecha */
        public string Fecha { get { return mFecha; }set { mFecha = value; } }

        /* hora */
        public string Hora { get { return mHora; } set { mHora = value; } }

        /* codigo */
        public string Codigo { get { return mCodigo; }set { mCodigo = value; } }

        /* nombre */
        public string Nombre { get { return mNombre; } set { mNombre = value; } }

        /* grupo */
        public string Grupo { get { return mGrupo; }set { mGrupo = value; } }

        /* tipoproceso */
        public int Tipoproceso { get { return mTipoproceso; }set { mTipoproceso = value; } }

        /* codigoalmacen */
        public string Codigoalmacen { get { return mCodigoalmacen; }set { mCodigoalmacen = value; } }

        /* usuario */
        public string Descripcion { get { return mDescripcion; }set { mDescripcion = value; } }

        /* ubicacion */
        public string Usuario { get { return mUsuario; }set { mUsuario = value; } }

        /* descripción */
        public string Ubicacion { get { return mUbicacion; } set { mUbicacion = value; } }
        
        /* precio */
        public decimal Precio_neto { get { return mPrecio_neto; }set { mPrecio_neto = value; } }

        /* cost */
        public decimal Precio_con_itbms { get { return mPrecio_con_itbms; } set { mPrecio_con_itbms = value; } }

        /* costo */
        public decimal Costo { get { return mCosto; }set { mCosto = value; } }

        public decimal Porcentaje_itbms { get { return mPrecio_con_itbms; }set { mPorcentaje_itbms = value; } }

        public int Tipo { get { return mTipo; }set { mTipo = value; } }

        public decimal Cantidad { get { return mCantidad; }set { mCantidad = value; } }

        public string Unidad_de_medida { get { return mUnidad_de_medida; }set { mUnidad_de_medida = value; } }
        #endregion Propiedades

        #region Utilidades

        // Convierte la clase en una cadena serializada en XML
        override public string ToString()
        {
            string tResult = String.Empty;
            try
            {
                XmlSerializer xmlCustomerSerializer = new XmlSerializer(typeof(ItemAvailability));

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
        public static bool SaveAs(string filename, ItemAvailability itemAvailability)
        {
            bool tResult = false;
            try
            {
                System.IO.File.WriteAllText(filename, itemAvailability.ToString(),Encoding.Unicode);
                tResult = true;
            }
            catch
            {
                tResult = false;
            }

            return tResult;
        }


        // Carga un item desde un XML
        public static ItemAvailability LoadFrom(string filename)
        {
            ItemAvailability tItemAvailability = null;

            try
            {
                var serializer = new XmlSerializer(typeof(ItemAvailability));
                FileStream fs = new FileStream(filename, FileMode.Open);
                StreamReader stream = new StreamReader(fs, Encoding.Unicode);
                tItemAvailability = (ItemAvailability)serializer.Deserialize(stream);
                fs.Close();
            }
            catch
            { }

            return tItemAvailability;
        }


        #endregion Utilidades
    }


    [Serializable]
    public class InventoryAdjustment
    {
        private List<ItemAvailability> mItemAvailability = new List<ItemAvailability>();

        [XmlArray("Items"), XmlArrayItem(typeof(ItemAvailability), ElementName = "Availability")]
        public List<ItemAvailability> Items
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

        public void Add(ItemAvailability ia)
        {
            mItemAvailability.Add(ia);
        }

        #region Utilidades

        // Convierte la clase en una cadena serializada en XML
        override public string ToString()
        {
            string tResult = String.Empty;
            try
            {
                XmlSerializer xmlCustomerSerializer = new XmlSerializer(typeof(InventoryAdjustment));

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
        public static bool SaveAs(string filename, InventoryAdjustment inventoryAdjustment)
        {
            bool tResult = false;
            try
            {
                System.IO.File.WriteAllText(filename, inventoryAdjustment.ToString(),Encoding.Unicode);
                tResult = true;
            }
            catch
            {
                tResult = false;
            }

            return tResult;
        }


        // Carga un item desde un XML
        public static InventoryAdjustment LoadFrom(string filename)
        {
            InventoryAdjustment tInventoryAdjustment = null;

            try
            {
                var serializer = new XmlSerializer(typeof(InventoryAdjustment));
                FileStream fs = new FileStream(filename, FileMode.Open);
                StreamReader stream = new StreamReader(fs, Encoding.Unicode);
                tInventoryAdjustment = (InventoryAdjustment)serializer.Deserialize(stream);
                fs.Close();
            }
            catch
            { }

            return tInventoryAdjustment;
        }


        #endregion Utilidades
    }
}
