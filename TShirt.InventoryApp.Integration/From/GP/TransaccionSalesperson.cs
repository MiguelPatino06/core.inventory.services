using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Data.SqlClient;

namespace TShirt.InventoryApp.Integration.From.GP
{
	class TransaccionSalesperson : Transaccion, ITransaccion
	{
		private string mItmbs;

		public TransaccionSalesperson(string strcon, string nin, string nout, string hin, string hout, string itbms)
			: base("TransaccionSalesperson", 4, strcon, nin, nout, hin, hout,itbms)
		{
			mItmbs = itbms;
		}

		void thread()
		{
			DataTable Salesperson = getSalespersons();

			int total = Salesperson.Rows.Count;

			////

			// esrcibo en el nowin porque van para premiumsoft
			foreach (DataRow row in Salesperson.Rows)
			{
				string FileName = "GP_SP_{0}.xml";

		  
	


				string SLPRSNID = row["SLPRSNID"].ToString().Trim();
				string SLPRSNFN = row["SLPRSNFN"].ToString().Trim();
				string SPRSNSMN = row["SPRSNSMN"].ToString().Trim();
				string SPRSNSLN = row["SPRSNSLN"].ToString().Trim();
				string ADDRESS1 = row["ADDRESS1"].ToString().Trim();
				string ADDRESS2 = row["ADDRESS2"].ToString().Trim();
				string ADDRESS3 = row["ADDRESS3"].ToString().Trim();
				string CITY = row["CITY"].ToString().Trim();
				string STATE = row["STATE"].ToString().Trim();
				string ZIP = row["ZIP"].ToString().Trim();
				string COUNTRY = row["COUNTRY"].ToString().Trim();
				string PHONE1 = row["PHONE1"].ToString().Trim();
				string PHONE2 = row["PHONE2"].ToString().Trim();
				string PHONE3 = row["PHONE3"].ToString().Trim();
				string FAX = row["FAX"].ToString().Trim();

				Models.Salesperson salesperson = new Models.Salesperson();


				IEvent e = new InfoEvent("", "", "Iniciando la creación del archivo XML '" + String.Format(FileName, SLPRSNID) + "'.");
				e.Publish();
				// xml //
				XmlDocument doc = new XmlDocument();

				/* Lines */
				XmlElement Raiz = doc.CreateElement(string.Empty, "raiz", string.Empty);
				/* Lines */

				try
				{
					//XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
					//XmlElement root = doc.DocumentElement;
					//doc.InsertBefore(xmlDeclaration, root);
					//doc.AppendChild(Raiz);
					//XmlElement element2 = doc.CreateElement(string.Empty, "vendedor", string.Empty);
					//Raiz.AppendChild(element2);

					//XmlElement element3 = doc.CreateElement(string.Empty, "codigo", string.Empty);
					salesperson.Code = SLPRSNID;//XmlText text1 = doc.CreateTextNode(SLPRSNID);
					//element3.AppendChild(text1);
					//element2.AppendChild(element3);

					//XmlElement element4 = doc.CreateElement(string.Empty, "nombres", string.Empty);
					salesperson.Firts_Name = SLPRSNFN;//XmlText text2 = doc.CreateTextNode(SLPRSNFN);
					//element4.AppendChild(text2);
					//element2.AppendChild(element4);

					//XmlElement element5 = doc.CreateElement(string.Empty, "apellidos", string.Empty);
					salesperson.Last_Name = SPRSNSMN + " " + SPRSNSLN;//XmlText text3 = doc.CreateTextNode(SPRSNSMN + " " + SPRSNSLN);
					//element5.AppendChild(text3);
					//element2.AppendChild(element5);

					//XmlElement element6 = doc.CreateElement(string.Empty, "ruc", string.Empty);
					salesperson.RUC = "";//XmlText text4 = doc.CreateTextNode("");
					//element6.AppendChild(text4);
					//element2.AppendChild(element6);

					//XmlElement element7 = doc.CreateElement(string.Empty, "address", string.Empty);
					salesperson.Address = (ADDRESS1 + " " + ADDRESS2 + " " + ADDRESS3).Trim();//XmlText text5 = doc.CreateTextNode(ADDRESS1 + " " + ADDRESS2 + " " + ADDRESS3);
					//element7.AppendChild(text5);
					//element2.AppendChild(element7);

					//XmlElement element8 = doc.CreateElement(string.Empty, "porcentaje_comision", string.Empty);
					salesperson.ComisionPCT = 0;//XmlText text6 = doc.CreateTextNode("0.00000");
					//element8.AppendChild(text6);
					//element2.AppendChild(element8);

					//XmlElement element9 = doc.CreateElement(string.Empty, "email", string.Empty);
					salesperson.EMail = "";//XmlText text7 = doc.CreateTextNode("");
					//element9.AppendChild(text7);
					//element2.AppendChild(element9);

					//XmlElement element10 = doc.CreateElement(string.Empty, "phone", string.Empty);
					salesperson.Phone = PHONE1;//XmlText text8 = doc.CreateTextNode(PHONE1);
					//element10.AppendChild(text8);
					//element2.AppendChild(element10);


					if (this.NowPathIn.ToCharArray().Last() == '/' || this.NowPathIn.ToCharArray().Last() == '\\')
					{
						if (!System.IO.File.Exists(this.mNowPathOut + string.Format(FileName, SLPRSNID)))
							Models.Salesperson.SaveAs(this.mNowPathOut + string.Format(FileName, SLPRSNID), salesperson);//doc.Save(this.mNowPathOut + string.Format(FileName, SLPRSNID));

					}
					else
					{
						if (!System.IO.File.Exists(this.mNowPathOut + "/" + string.Format(FileName, SLPRSNID)))
							Models.Salesperson.SaveAs(this.mNowPathOut + "/" + string.Format(FileName, SLPRSNID), salesperson); //doc.Save(this.mNowPathOut + "/" + string.Format(FileName, SLPRSNID));
					}
					total--;

					ObserverManager.Instance.addSubject(new ProgressSubject(total, Salesperson.Rows.Count - total));

					IEvent e2 = new InfoEvent("", "", "El archivo '" + String.Format(FileName, SLPRSNID) + "'. fue creado correctamente");
					e2.Publish();
				}
				catch (Exception ex)
				{
					IEvent err = new ErrorEvent("", "", "No pudo crear el archivo xml correctamente. Mensaje: " + ex.Message);
					err.Publish();
				}
			}
			ObserverManager.Instance.addSubject(new ProgressSubject(0, 0));
		}

		void ITransaccion.Execute()
		{
			System.Threading.Thread th = new System.Threading.Thread(thread);
			th.Start();
		}

		private DataTable getTransactionInfo(string DocumentNumber)
		{
			SqlCommand command = new SqlCommand();
			SqlDataAdapter adapter = new SqlDataAdapter();

			DataTable data = new DataTable("GL_ADJUSTMENTS");
			command.Connection = new SqlConnection(this.mStringConnection);

			command.Parameters.AddWithValue("@DOCUMENT", DocumentNumber);

			command.CommandType = CommandType.Text;
			command.CommandText = "SELECT IV.DOCNUMBR " +
				",ISNULL(GL.USWHPSTD,'UNKNOWN') AS USERID " +
				", CASE DOCTYPE " +
				"       WHEN 1 THEN 'AJUSTE' ELSE 'DESCONOCIDO' " +
				"  END AS DOCTYPE " +
				",IV.DOCDATE " +
				",CONVERT(time, ISNULL(GL.DEX_ROW_TS,0)) AS [TIME] " +
				",IV.ITEMNMBR " +
				",IV.UOFM " +
				",'UNKNOWN' AS FAMILY " +
				",CASE WHEN TRXQTY>0 THEN '1' ELSE '2' END AS PROCESS " +
				",IV.TRXLOCTN " +
				",ISNULL((SELECT USERNAME FROM DYNAMICS..SY01400 WHERE USERID=  GL.USWHPSTD),'UNKNOWN') AS USERNAME " +
				",'UBICACION' AS LOCATION " +
				",ISNULL(CASE IVM.PRICMTHD " +
				"      WHEN 1 THEN ROUND(UOMPRICE,IVM.DECPLCUR) " +
				"      WHEN 2 THEN ROUND(CUR.LISTPRCE*(UOMPRICE/100),IV.DECPLCUR-1) " +
				"      WHEN 3 THEN ROUND(IVM.CURRCOST + (IVM.CURRCOST * (UOMPRICE/100)),IVM.DECPLCUR-1) " +
				"      WHEN 4 THEN ROUND(IVM.STNDCOST + (IVM.STNDCOST * (UOMPRICE/100)),IVM.DECPLCUR-1) " +
				"      WHEN 5 THEN ROUND(IVM.CURRCOST + ((IVM.CURRCOST * (UOMPRICE/100))/((100-UOMPRICE)/100)),IVM.DECPLCUR-1) " +
				"      WHEN 6 THEN ROUND(IVM.STNDCOST + ((IVM.STNDCOST * (UOMPRICE/100))/((100-UOMPRICE)/100)),IVM.DECPLCUR-1) " +
				"END,0) AS PRICE " +
				",IVM.CURRCOST AS COST " +
				",TRXQTY AS QTY " +
				"FROM IV30300 AS IV " +
				"LEFT JOIN GL10000 AS GL ON IV.TRXSORCE = GL.ORTRXSRC AND GL.DTAControlNum=IV.DOCNUMBR " +
				"INNER JOIN IV00101 AS IVM ON IV.ITEMNMBR = IVM.ITEMNMBR " +
				"INNER JOIN IV00108 AS PRCLST ON IV.ITEMNMBR=PRCLST.ITEMNMBR AND IVM.PRCLEVEL=PRCLST.PRCLEVEL AND IVM.PRCHSUOM=PRCLST.UOFM " +
				"INNER JOIN IV00105 AS CUR ON CUR.ITEMNMBR = IV.ITEMNMBR AND PRCLST.CURNCYID = CUR.CURNCYID " +
				"WHERE DOCTYPE=1 AND IV.DOCNUMBR=@DOCUMENT ORDER BY IV.DEX_ROW_ID ASC";


			try
			{
				adapter.SelectCommand = command;
				adapter.Fill(data);
			}
			catch (Exception ex)
			{
				IEvent e = new ErrorEvent("", "", ex.Message);
				e.Publish();
			}
			finally
			{
				//nothing
			}
			return data;
		}


		private DataTable getAllPricesLevels()
		{
			SqlCommand command = new SqlCommand();
			SqlDataAdapter adapter = new SqlDataAdapter();

			DataTable data = new DataTable("PRCLVL");
			command.Connection = new SqlConnection(this.mStringConnection);

			// command.Parameters.AddWithValue("@P", "0");

			command.CommandType = CommandType.Text;
			command.CommandText = "SELECT * FROM IV40800";


			try
			{
				adapter.SelectCommand = command;
				adapter.Fill(data);
			}
			catch (Exception ex)
			{
				IEvent e = new ErrorEvent("", "", ex.Message);
				e.Publish();
			}
			finally
			{
				//nothing
			}
			return data;

		}

		private DataTable getPriceLeveData(string PRCLVL)
		{

			SqlCommand command = new SqlCommand();
			SqlDataAdapter adapter = new SqlDataAdapter();

			DataTable data = new DataTable("ITEMMSTR");
			command.Connection = new SqlConnection(this.mStringConnection);

			command.Parameters.AddWithValue("@ITMBS", this.mItmbs);
			command.Parameters.AddWithValue("@PRCLVL", PRCLVL);

			command.CommandType = CommandType.Text;
 
			String query = ""; 
			query += "SELECT DISTINCT IV.ITEMNMBR   ";
			query += ",IV.ITEMDESC,IV.ITEMTYPE,PRCLST.UOFM,ITMCLSCD,CURRCOST,MAX(UOFM.QTYBSUOM) AS QTYBSUOM   ";
			query += ",CASE IV.PRICMTHD   ";
			query += "WHEN 1 THEN ROUND(MAX(PRCLST.UOMPRICE) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)   ";
			query += "WHEN 2 THEN ROUND((MAX(CUR.LISTPRCE)*(MAX(PRCLST.UOMPRICE)/100)) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)    ";
			query += "WHEN 3 THEN ROUND((MAX(IV.CURRCOST) + (MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100))) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)   ";
			query += "WHEN 4 THEN ROUND((MAX(IV.STNDCOST) + (MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)   ";
			query += "WHEN 5 THEN ROUND((MAX(IV.CURRCOST) + ((MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100))) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)   ";
			query += "WHEN 6 THEN ROUND((MAX(IV.STNDCOST) + ((MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100))) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1)   ";
			query += "END AS PRICE   ";
			query += ",CASE IV.PRICMTHD   ";
			query += "WHEN 1 THEN ISNULL((MAX(PRCLST.UOMPRICE)+(MAX(PRCLST.UOMPRICE)*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100))) * MAX(UOFM.QTYBSUOM),ROUND(MAX(PRCLST.UOMPRICE) * MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1))   ";
			query += "WHEN 2 THEN ISNULL((MAX(CUR.LISTPRCE)*(MAX(PRCLST.UOMPRICE)/100) /**/ + ((MAX(CUR.LISTPRCE)*(MAX(PRCLST.UOMPRICE)/100))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100))) * MAX(UOFM.QTYBSUOM),ROUND((MAX(CUR.LISTPRCE)*(MAX(PRCLST.UOMPRICE)/100))* MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1) )   ";
			query += "WHEN 3 THEN ISNULL((MAX(IV.CURRCOST) + (MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100)) /**/ + ((MAX(IV.CURRCOST) + (MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100)))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)))* MAX(UOFM.QTYBSUOM),ROUND((MAX(IV.CURRCOST) + (MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100)))* MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1))   ";
			query += "WHEN 4 THEN ISNULL((MAX(IV.STNDCOST) + (MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100)) /**/ + (MAX(IV.STNDCOST) + (MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)))* MAX(UOFM.QTYBSUOM),ROUND((MAX(IV.STNDCOST) + (MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100)))* MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1))   ";
			query += "WHEN 5 THEN ISNULL((MAX(IV.CURRCOST) + ((MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)) /**/ + ((MAX(IV.CURRCOST) + ((MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)))* MAX(UOFM.QTYBSUOM),ROUND((MAX(IV.CURRCOST) + ((MAX(IV.CURRCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)))* MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1))   ";
			query += "WHEN 6 THEN ISNULL((MAX(IV.STNDCOST) + ((MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)) /**/+ ((MAX(IV.STNDCOST) + ((MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)))*((SELECT TXDTLPCT  FROM TX00201 WHERE TAXDTLID=@ITMBS)/100)))* MAX(UOFM.QTYBSUOM),ROUND((MAX(IV.STNDCOST) + ((MAX(IV.STNDCOST) * (MAX(PRCLST.UOMPRICE)/100))/((100-MAX(PRCLST.UOMPRICE))/100)))* MAX(UOFM.QTYBSUOM),AVG(IV.DECPLCUR)-1))   ";
			query += "END AS PRICEWITMBS   ";
			query += ",ISNULL((SELECT TXDTLPCT FROM TX00201 WHERE TAXDTLID=@ITMBS),0) AS ITMBSPCT   ";
			query += ",PRCLST.PRCLEVEL   ";
			query += "FROM IV00107 AS PRCLVL   ";
			query += "INNER JOIN IV00101 AS IV ON IV.ITEMNMBR=PRCLVL.ITEMNMBR  ";
			query += "INNER JOIN IV00108 AS PRCLST ON PRCLVL.ITEMNMBR=PRCLST.ITEMNMBR AND PRCLVL.PRCLEVEL=PRCLST.PRCLEVEL AND PRCLVL.UOFM=PRCLST.UOFM   ";
			query += "INNER JOIN IV00105 AS CUR ON CUR.ITEMNMBR = IV.ITEMNMBR AND PRCLST.CURNCYID = CUR.CURNCYID   ";
			query += "INNER JOIN IV00106 AS UOFM ON IV.ITEMNMBR=UOFM.ITEMNMBR AND PRCLST.UOFM=UOFM.UOFM   ";
			query += "WHERE PRCLST.PRCLEVEL=@PRCLVL ";
			query += "GROUP BY IV.ITEMNMBR   ";
			query += ",IV.ITEMDESC,IV.ITEMTYPE   ";
			query += ",PRCLST.UOFM   ";
			query += ",ITMCLSCD   ";
			query += ",CURRCOST   ";
			query += ",IV.PRICMTHD  ";
			query += ",PRCLST.PRCLEVEL";
			command.CommandText = query;
			try
			{
				adapter.SelectCommand = command;
				adapter.Fill(data);
			}
			catch (Exception ex)
			{
				IEvent e = new ErrorEvent("", "", ex.Message);
				e.Publish();
			}
			finally
			{
				//nothing
			}
			return data;

		}

		private DataTable getSalespersons()
		{
			SqlCommand command = new SqlCommand();
			SqlDataAdapter adapter = new SqlDataAdapter();

			DataTable data = new DataTable("SALESPERSON");
			command.Connection = new SqlConnection(this.mStringConnection);

			// command.Parameters.AddWithValue("@P", "0");

			command.CommandType = CommandType.Text;
			command.CommandText = "SELECT * FROM RM00301 WHERE INACTIVE=0";


			try
			{
				adapter.SelectCommand = command;
				adapter.Fill(data);
			}
			catch (Exception ex)
			{
				IEvent e = new ErrorEvent("", "", ex.Message);
				e.Publish();
			}
			finally
			{
				//nothing
			}
			return data;
		}



	}
}
