using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TShirt.InventoryApp.Integration
{
    public abstract class Transaccion{

        protected string mName;
        protected int mId;

        protected string mNowPathIn;
        protected string mNowPathOut;

        protected string mHistPathIn;
        protected string mHistPathOut;

        protected string mStringConnection;

        protected string mITBMS;

        public string NowPathOut
        {
            get
            {
                return this.mNowPathOut;
            }
        }

        public string ITBMS
        {
            get
            {
                return this.mITBMS;
            }
        }

        public string HistPathIn
        {
            get
            {
                return this.mHistPathIn;
            }
        }

        public string HistPathOut
        {
            get
            {
                return this.mHistPathOut;
            }
        }

        public string Name
        {
            get
            {
                return this.mName;
            }
        }

        public int Id
        {
            get
            {
                return this.mId;
            }
        }

        public string NowPathIn
        {
            get
            {
                return this.mNowPathIn;
            }
        }

        public string StringConnection
        {
            get
            {
                return this.mStringConnection;
            }
        }

        public Transaccion(string name, int id, string strcon, string nin, string nout, string hin, string hout,string itbms) 
        {
            this.mId = id;
            this.mName = name;
            this.mStringConnection = strcon;

            this.mITBMS = itbms;
            mNowPathIn = nin;
            mNowPathOut = nout;
            
            mHistPathIn = hin;
            mHistPathOut = hout;
        }
            
    }


    public interface ITransaccion
    {
        void Execute();
    }
}
