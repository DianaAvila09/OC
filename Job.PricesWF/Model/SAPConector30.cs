using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Job.PricesWF.Model
{
    public class SAPConector30
    {
        protected static String Entorno = ConfigurationManager.AppSettings["Entorno"].ToString();

        public bool Validar_Usuario(string user, string pwd)
        {
            RfcDestination SapRfcDestination = RfcDestinationManager.GetDestination(Entorno);
            RfcRepository SapRfcRepository = SapRfcDestination.Repository;
            try
            {
                IRfcFunction Bapi = SapRfcRepository.CreateFunction("SUSR_LOGIN_CHECK_RFC");
                Bapi.SetValue("BNAME", user);
                Bapi.SetValue("PASSWORD", pwd);
                Bapi.SetValue("USE_NEW_EXCEPTION", 9);
                Bapi.Invoke(SapRfcDestination);
                return true;
            }
            finally
            {
                SapRfcRepository = null;
                SapRfcDestination = null;
            }
        }

        public static List<ET_RETURN> ZFM_SA_ACT_PRECIO(List<ZMMTT_ME32L_ACT_PRICE> IT_LIST)
        {
            RfcDestination SapRfcDestination = RfcDestinationManager.GetDestination(Entorno);
            RfcRepository SapRfcRepository = SapRfcDestination.Repository;
            try
            {
                IRfcFunction Bapi = SapRfcRepository.CreateFunction("ZFM_SA_ACT_PRECIO");

                IRfcTable IT_LISTs = Bapi.GetTable("IT_LIST");
                foreach (ZMMTT_ME32L_ACT_PRICE price in IT_LIST)
                {
                    IT_LISTs.Append();
                    IT_LISTs.SetValue("PURCHASINGDOCUMENT", price.PURCHASINGDOCUMENT);
                    IT_LISTs.SetValue("ITEM_NO", price.ITEM_NO);
                    IT_LISTs.SetValue("MATERIAL", price.MATERIAL);
                    IT_LISTs.SetValue("PRICE", price.PRICE);
                    IT_LISTs.SetValue("BASE", price.BASE);
                    IT_LISTs.SetValue("VALID_FROM", price.VALID_FROM);
                }

                Bapi.Invoke(SapRfcDestination);

                IRfcTable ET_RETURN = Bapi.GetTable("ET_RETURN");

                DataTable DT_RETURN = ConvertToDT(ET_RETURN);

                List<ET_RETURN> lET_RETURN = new List<ET_RETURN>();

                foreach (DataRow row in DT_RETURN.Rows)
                {
                    Int32.TryParse(row["NUMBER"].ToString(), out Int32 _NUMBER);
                    lET_RETURN.Add(new ET_RETURN() { ID = row["ID"].ToString(), MESSAGE = row["MESSAGE"].ToString(), TYPE = row["TYPE"].ToString(), NUMBER = _NUMBER });
                }

                return lET_RETURN;
            }
            catch (Exception ex)
            {
                List<ET_RETURN> lET_RETURN = new List<ET_RETURN>();
                lET_RETURN.Add(new ET_RETURN() { ID = "000", MESSAGE = ex.Message, NUMBER = 0, TYPE = "E" });
                return lET_RETURN;
            }
            finally
            {
                SapRfcRepository = null;
                SapRfcDestination = null;
            }
        }

        public static List<ET_RETURN> ZFM_SA_NEW_POS(List<ZMMTT_ME32L_SOL_POS> IT_LIST)
        {
            RfcDestination SapRfcDestination = RfcDestinationManager.GetDestination(Entorno);
            RfcRepository SapRfcRepository = SapRfcDestination.Repository;
            try
            {
                IRfcFunction Bapi = SapRfcRepository.CreateFunction("ZFM_SA_NEW_POS");

                IRfcTable IT_LISTs = Bapi.GetTable("IT_LIST");
                foreach (ZMMTT_ME32L_SOL_POS price in IT_LIST)
                {
                    IT_LISTs.Append();
                    IT_LISTs.SetValue("PURCHASINGDOCUMENT", price.PURCHASINGDOCUMENT);
                    IT_LISTs.SetValue("LIFNR", price.LIFNR.PadLeft(10,'0'));
                    IT_LISTs.SetValue("ITEM_CAT", price.ITEM_CAT);
                    IT_LISTs.SetValue("MATERIAL", price.MATERIAL);
                    IT_LISTs.SetValue("TARGET_QTY", price.TARGET_QTY);
                    IT_LISTs.SetValue("PRICE", price.PRICE);
                    IT_LISTs.SetValue("BASE", price.BASE);
                }

                Bapi.Invoke(SapRfcDestination);

                IRfcTable ET_RETURN = Bapi.GetTable("ET_RETURN");

                DataTable DT_RETURN = ConvertToDT(ET_RETURN);

                List<ET_RETURN> lET_RETURN = new List<ET_RETURN>();

                foreach (DataRow row in DT_RETURN.Rows)
                {
                    Int32.TryParse(row["NUMBER"].ToString(), out Int32 _NUMBER);
                    lET_RETURN.Add(new ET_RETURN() { ID = row["ID"].ToString(), MESSAGE = row["MESSAGE"].ToString(), TYPE = row["TYPE"].ToString(), NUMBER = _NUMBER });
                }

                return lET_RETURN;
            }
            catch (Exception ex)
            {
                List<ET_RETURN> lET_RETURN = new List<ET_RETURN>();
                lET_RETURN.Add(new ET_RETURN() { ID = "000", MESSAGE = ex.Message, NUMBER = 0, TYPE = "E" });
                return lET_RETURN;
            }
            finally
            {
                SapRfcRepository = null;
                SapRfcDestination = null;
            }
        }

        public static DataTable ConvertToDT(IRfcTable TablaSAP)
        {
            DataTable DT = new DataTable();
            for (int i = 0; i < TablaSAP.ElementCount; i++)
            {
                RfcElementMetadata MetaDato = TablaSAP.GetElementMetadata(i);
                DT.Columns.Add(MetaDato.Name);
            }
            foreach (IRfcStructure row in TablaSAP)
            {
                DataRow dr = DT.NewRow();
                for (int i = 0; i < TablaSAP.ElementCount; i++)
                {
                    RfcElementMetadata MetaDatos = TablaSAP.GetElementMetadata(i);
                    if (MetaDatos.DataType == RfcDataType.BCD && MetaDatos.Name == "ABC")
                    {
                        dr[i] = row.GetInt(MetaDatos.Name);
                    }
                    else
                    {
                        dr[i] = row.GetString(MetaDatos.Name);
                    }
                }
                DT.Rows.Add(dr);
            }
            return DT;
        }
    }
}