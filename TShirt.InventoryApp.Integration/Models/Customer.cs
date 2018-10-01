using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
///using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace TShirt.InventoryApp.Integration.Models
{
    [Serializable]
    public class Customer
    {

        #region Atributos
        // Código del cliente
        private string mNo = String.Empty;

        // Nombre del cliente
        private string mName = String.Empty;

        // Número de identificación fiscal
        private string mVAT_Registration_No = String.Empty;

        // Teléfono
        private string mPhone = String.Empty;

        // Correo
        private string mEMail = String.Empty;

        // Impuesto al valor agregado
        private decimal mVAT = 0;

        // Ggrupo de impuesto al valor agregado
        private string mVAT_Bus_Posting_Group = "IVA";

        // fecha de creación
        private string mCreation_date = "1900-01-01 00:00:00.00";

        // última modificación
        private string mLast_modified_date = "1991-03-15 00:00:00";

        // Dirección
        private string mAddress1 = "";

        #endregion Atributos

        #region Propiedades
        public string No { get { return mNo; }set { mNo = value; } }
        public string Name { get { return mName; }set { mName = value; } }
        public string VAT_Registration_No { get { return mVAT_Registration_No; }set { mVAT_Registration_No = value; } }
        public string Phone { get { return mPhone; } set { mPhone = value; } }
        public string EMail { get { return mEMail; } set { mEMail = value; } }        
        public decimal VAT { get { return mVAT; } set { mVAT = value; } }
        public string VAT_Bus_Posting_Group { get { return mVAT_Bus_Posting_Group; } set { mVAT_Bus_Posting_Group = value; } }
        public string Creation_date { get { return mCreation_date; } set { mCreation_date = value; } }
        public string Last_modified_date { get { return mLast_modified_date; }set { mLast_modified_date = value; } }
        public string Address1 { get { return mAddress1; } set { mAddress1 = value; } }
        #endregion

        #region Utilidades

        // Convierte la clase en una cadena serializada en XML
        override public string ToString()
        {
            string tResult = String.Empty;
            try
            {
                XmlSerializer xmlCustomerSerializer = new XmlSerializer(typeof(Customer));
                
                StringWriter stringWriter = new StringWriter();
                XmlWriter writer = XmlWriter.Create(stringWriter);
                xmlCustomerSerializer.Serialize(writer, this);
                tResult = stringWriter.ToString();

            }catch(Exception ex)
            {
                return ex.Message;
            }
            return tResult;

        }

       // Guarda un cliente en un XML
        public static bool SaveAs(string filename,Customer customer)
        {
            bool tResult = false;
            try
            {
                System.IO.File.WriteAllText(filename, customer.ToString(),Encoding.Unicode);
                tResult = true;
            }
            catch
            {
                tResult = false;
            }

            return tResult;
        }


        // Carga un cliente desde un XML
        public static Customer LoadFrom(string filename)
        {
            Customer tCustomer = null;

            try
            {
               
                    var serializer = new XmlSerializer(typeof(Customer));
                    FileStream fs = new FileStream(filename, FileMode.Open);
                    StreamReader stream = new StreamReader(fs, Encoding.Unicode);
                    tCustomer = (Customer)serializer.Deserialize(stream);
                fs.Close();
               
            }catch(Exception ex)
            {

                IEvent e = new ErrorEvent("", "", "Customer::" + ex.Message);
                e.Publish();
            }

            return tCustomer;
        }


        #endregion Utilidades

    }
}
