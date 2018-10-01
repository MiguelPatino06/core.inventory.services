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
    public class Salesperson
    {
        #region Atributos
        private  string mCode = string.Empty;
        private  string mFirts_Name = string.Empty;
        private  string mLast_Name = string.Empty;
        private  string mRUC = string.Empty;
        private  string mName_of_Receipt = string.Empty;
        private int mEmployment_Type = 0;
        private string mAddress = string.Empty;
        private decimal mComisionPCT = 0;
        private string mEMail = string.Empty;
        private string mPhone = string.Empty;
        #endregion Atributos

        #region Propiedades
        public string Code { get { return mCode; }set { mCode = value; } }
        public string Firts_Name { get { return mFirts_Name; }set { mFirts_Name = value; } }
        public string Last_Name { get { return mLast_Name; } set { mLast_Name = value; } }
        public string RUC { get { return mRUC; }set { mRUC = value; } }
        public string Name_of_Receipt { get { return mName_of_Receipt; } set { mName_of_Receipt = value; } }
        public int Employment_Type { get { return mEmployment_Type; }set { mEmployment_Type = value; } }
        public string Address { get { return mAddress; }set { mAddress = value; } }
        public decimal ComisionPCT { get { return mComisionPCT; }set { mComisionPCT = value; } }
        public string EMail { get { return mEMail; } set { mEMail = value; } }
        public string Phone { get { return mPhone; }set { mPhone = value; } }
        #endregion Propiedades

        #region Utilidades

        // Convierte la clase en una cadena serializada en XML
        override public string ToString()
        {
            string tResult = String.Empty;
            try
            {
                XmlSerializer xmlCustomerSerializer = new XmlSerializer(typeof(Salesperson));

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

        // Guarda un vendedor en un XML
        public static bool SaveAs(string filename, Salesperson salesperson)
        {
            bool tResult = false;
            try
            {
                System.IO.File.WriteAllText(filename, salesperson.ToString(),Encoding.Unicode);
                tResult = true;
            }
            catch
            {
                tResult = false;
            }

            return tResult;
        }


        // Carga un vendedor desde un XML
        public static Salesperson LoadFrom(string filename)
        {
            Salesperson tSalesperson = null;

            try
            {
                var serializer = new XmlSerializer(typeof(Salesperson));
                FileStream fs = new FileStream(filename, FileMode.Open);
                StreamReader stream = new StreamReader(fs, Encoding.Unicode);
                tSalesperson = (Salesperson)serializer.Deserialize(stream);
                fs.Close();
            }
            catch
            { }

            return tSalesperson;
        }


        #endregion Utilidades
    }
}
