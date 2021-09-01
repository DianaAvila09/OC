using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Root.Model
{
    public class SAPConector30
    {
        protected String Entorno;
        public SAPConector30()
        {
            Entorno = ConfigurationManager.AppSettings["Entorno"].ToString();
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="I_MODE">P=Cambio de Precio, I=Crear posiciones</param>
        /// <returns></returns>
        public Tuple<List<ET_RETURN>,List<ZMMTT_ME32L_DATA_PRICE>, List<ZMMTT_ME32L_DATA_POS>> ZFM_SA_CREA_SOL(String I_MODE, List<ZMMTT_ME32L_SOL_PRICE> IT_PRICE, List<ZMMTT_ME32L_SOL_POS> IT_POS)
        {
            RfcDestination SapRfcDestination = RfcDestinationManager.GetDestination(Entorno);
            RfcRepository SapRfcRepository = SapRfcDestination.Repository;
            try
            {
                IRfcFunction Bapi = SapRfcRepository.CreateFunction("ZFM_SA_CREA_SOL");
                Bapi.SetValue("I_MODE", I_MODE);

                if (I_MODE == "P" && IT_PRICE != null)
                {
                    IRfcTable IT_PRICEs = Bapi.GetTable("IT_PRICE");
                    foreach (ZMMTT_ME32L_SOL_PRICE price in IT_PRICE)
                    {
                        IT_PRICEs.Append();
                        IT_PRICEs.SetValue("PURCHASINGDOCUMENT", price.PURCHASINGDOCUMENT);
                        IT_PRICEs.SetValue("MATERIAL", price.MATERIAL);
                        IT_PRICEs.SetValue("PRICE", price.PRICE);
                        IT_PRICEs.SetValue("BASE", price.BASE);
                        IT_PRICEs.SetValue("VALID_FROM", price.VALID_FROM);
                    }
                }
                else if (IT_POS != null)
                {
                    IRfcTable IT_POSs = Bapi.GetTable("IT_POS");
                    foreach (ZMMTT_ME32L_SOL_POS pos in IT_POS)
                    {
                        IT_POSs.Append();
                        IT_POSs.SetValue("PURCHASINGDOCUMENT", pos.PURCHASINGDOCUMENT);
                        IT_POSs.SetValue("LIFNR", pos.LIFNR.PadLeft(10,'0'));
                        IT_POSs.SetValue("ITEM_CAT", pos.ITEM_CAT);
                        IT_POSs.SetValue("MATERIAL", pos.MATERIAL);
                        IT_POSs.SetValue("TARGET_QTY", pos.TARGET_QTY);
                        IT_POSs.SetValue("PRICE", pos.PRICE);
                        IT_POSs.SetValue("BASE", pos.BASE);
                    }
                }

                Bapi.Invoke(SapRfcDestination);

                IRfcTable ET_DATA_PRICE = Bapi.GetTable("ET_DATA_PRICE");
                IRfcTable ET_DATA_POS = Bapi.GetTable("ET_DATA_POS");
                IRfcTable ET_RETURN = Bapi.GetTable("ET_RETURN");

                DataTable DT_RETURN = ConvertToDT(ET_RETURN);
                DataTable DT_DATA_PRICE = ConvertToDT(ET_DATA_PRICE);
                DataTable DT_DATA_POS = ConvertToDT(ET_DATA_POS);

                List<ET_RETURN> lET_RETURN = new List<ET_RETURN>();
                List<ZMMTT_ME32L_DATA_PRICE> lDT_DATA_PRICE = new List<ZMMTT_ME32L_DATA_PRICE>();
                List<ZMMTT_ME32L_DATA_POS> lDT_DATA_POS = new List<ZMMTT_ME32L_DATA_POS>();

                foreach (DataRow row in DT_RETURN.Rows)
                {
                    Int32.TryParse(row["NUMBER"].ToString(), out Int32 _NUMBER);
                    lET_RETURN.Add(new ET_RETURN() { ID = row["ID"].ToString(), MESSAGE = row["MESSAGE"].ToString(), TYPE = row["TYPE"].ToString(), NUMBER = _NUMBER });
                }

                foreach (DataRow row in DT_DATA_PRICE.Rows)
                {
                    Int32.TryParse(row["ITEM_NO"].ToString(), out Int32 _ITEM_NO);
                    Double.TryParse(row["KTMNG"].ToString(), out Double _KTMNG);
                    //Int32.TryParse(row["MEINS"].ToString(), out Int32 _MEINS);
                    Double.TryParse(row["NETPR"].ToString(), out Double _NETPR);
                    Double.TryParse(row["PEINH"].ToString(), out Double _PEINH);
                    lDT_DATA_PRICE.Add(new ZMMTT_ME32L_DATA_PRICE() { PURCHASINGDOCUMENT = row["PURCHASINGDOCUMENT"].ToString(), MATERIAL = row["MATERIAL"].ToString(), ITEM_NO = _ITEM_NO, KTMNG = _KTMNG, MEINS = row["MEINS"].ToString(), NETPR = _NETPR, PEINH = _PEINH });
                }

                foreach (DataRow row in DT_DATA_POS.Rows)
                {
                    Int32.TryParse(row["ITEM_NO"].ToString(), out Int32 _ITEM_NO);
                    Double.TryParse(row["KTMNG"].ToString(), out Double _KTMNG);
                    Double.TryParse(row["NETPR"].ToString(), out Double _NETPR);
                    Double.TryParse(row["PEINH"].ToString(), out Double _PEINH);
                    Double.TryParse(row["ETFZ1"].ToString(), out Double _ETFZ1);
                    lDT_DATA_POS.Add(new ZMMTT_ME32L_DATA_POS() {
                        PURCHASINGDOCUMENT = row["PURCHASINGDOCUMENT"].ToString(),
                        ITEM_NO = _ITEM_NO,
                        LIFNR = row["LIFNR"].ToString(),
                        PSTYP = row["PSTYP"].ToString(),
                        MATERIAL = row["MATERIAL"].ToString(),
                        KTMNG = _KTMNG,
                        NETPR = _NETPR,
                        PEINH = _PEINH,
                        BSTAE = row["BSTAE"].ToString(),
                        ETFZ1 = _ETFZ1,
                        ABUEB = row["ABUEB"].ToString(),
                        LOEKZ = row["LOEKZ"].ToString(),
                    });
                }

                return new Tuple<List<ET_RETURN>, List<ZMMTT_ME32L_DATA_PRICE>, List<ZMMTT_ME32L_DATA_POS>>(lET_RETURN, lDT_DATA_PRICE, lDT_DATA_POS);
            }
            catch (Exception ex)
            {
                List<ET_RETURN> lET_RETURN = new List<ET_RETURN>();
                lET_RETURN.Add(new ET_RETURN() { ID = "000", MESSAGE=ex.Message, NUMBER = 0, TYPE = "E" });
                return new Tuple<List<ET_RETURN>, List<ZMMTT_ME32L_DATA_PRICE>, List<ZMMTT_ME32L_DATA_POS>>(lET_RETURN, null, null);
            }
            finally
            {
                SapRfcRepository = null;
                SapRfcDestination = null;
            }
        }

        public DataTable ConvertToDT(IRfcTable TablaSAP)
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