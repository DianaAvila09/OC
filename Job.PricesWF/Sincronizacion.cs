using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Job.PricesWF.Model;

namespace Job.PricesWF
{
    class Sincronizacion
    {
        public static void Ejecuta_CambiosPrecio()
        {
            try
            {
                using (WFOCEntities db = new WFOCEntities())
                {
                    //List<WfcpDocumento> documentos = (from d in db.WfcpDocumento
                    //                                  where d.WorkflowId == 1 // 1 = Cambio de Precio
                    //                                  join e in db.WfcpDocumento_Estatus on d.id equals e.DocumentoId into es
                    //                                  from x in es.DefaultIfEmpty()
                    //                                  where x.id == d.WfcpDocumento_Estatus.OrderByDescending(x => x.id).FirstOrDefault().id
                    //                                  && x.EstatusId == 16 //Aceptado por AGM
                    //                                  select d).OrderBy(x => x.id).ToList();
                    List<WfcpDocumento> documentos = (from d in db.WfcpDocumento
                                                      where d.WorkflowId == 1 // 1 = Cambio de Precio
                                                      && d.ZFM_SA_CREA_SOL == true
                                                      && d.ZFM_SA_ACT_PRECIO != true
                                                      join e in db.WfcpDocumento_Estatus on d.id equals e.DocumentoId
                                                      where e.id == d.WfcpDocumento_Estatus.OrderByDescending(x => x.id).FirstOrDefault().id
                                                      && e.EstatusId == 16 //Aceptado por AGM
                                                      select d).OrderBy(x => x.id).ToList();

                    foreach (WfcpDocumento documento in documentos)
                    {
                        try
                        {
                            List<WfcpCambioPrecio> cambios = db.WfcpCambioPrecio.Where(x => x.DocumentoId == documento.id).ToList();
                            List<ZMMTT_ME32L_ACT_PRICE> IT_LIST = new List<ZMMTT_ME32L_ACT_PRICE>();
                            foreach (WfcpCambioPrecio cambio in cambios)
                            {
                                IT_LIST.Add(new ZMMTT_ME32L_ACT_PRICE() { PURCHASINGDOCUMENT = cambio.documento_oc.ToString().PadLeft(10, '0'), ITEM_NO = cambio.ITEM_NO.Value, MATERIAL = cambio.no_parte, PRICE = cambio.precio, BASE = 1000, VALID_FROM = cambio.fecha_punto_quiebre });
                            }
                            List<ET_RETURN> ET_RETURN = SAPConector30.ZFM_SA_ACT_PRECIO(IT_LIST);

                            if (ET_RETURN.Any())
                            {
                                List<WfcpLog_ET_RETURN> log = new List<WfcpLog_ET_RETURN>();
                                foreach (ET_RETURN error in ET_RETURN)
                                {
                                    log.Add(new WfcpLog_ET_RETURN() { DocumentoId = documento.id, fecha_insert = DateTime.Now, IDENTIFICADOR = error.ID, MESSAGE = error.MESSAGE, NUMBER = error.NUMBER, TYPE = error.TYPE });
                                }
                                db.WfcpLog_ET_RETURN.AddRange(log);
                                db.SaveChanges();
                                documento.ZFM_SA_ACT_PRECIO = false;
                            }
                            else
                            {
                                WfcpLog_ET_RETURN log_exitoso = new WfcpLog_ET_RETURN() { DocumentoId = documento.id, fecha_insert = DateTime.Now, IDENTIFICADOR = "0", MESSAGE = "Cambio de Precio Ejecutado Correctamente", NUMBER = 0, TYPE = "S" };
                                db.Entry(log_exitoso).State = System.Data.Entity.EntityState.Added;
                                db.SaveChanges();
                                documento.ZFM_SA_ACT_PRECIO = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            WfcpLog_ET_RETURN log = new WfcpLog_ET_RETURN() { DocumentoId = documento.id, fecha_insert = DateTime.Now, IDENTIFICADOR = "0", MESSAGE = ex.Message, NUMBER = 0, TYPE = "E" };
                            db.Entry(log).State = System.Data.Entity.EntityState.Added;
                            db.SaveChanges();
                            documento.ZFM_SA_ACT_PRECIO = false;
                        }
                        finally
                        {
                            db.Entry(documento).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void Ejecuta_NuevasPartidas()
        {
            try
            {
                using (WFOCEntities db = new WFOCEntities())
                {
                    List<WfcpDocumento> documentos = (from d in db.WfcpDocumento
                                                      where d.WorkflowId == 2 // 2 = Nuevas Partidas
                                                      && d.ZFM_SA_CREA_SOL == true
                                                      && d.ZFM_SA_NEW_POS != true
                                                      join e in db.WfcpDocumento_Estatus on d.id equals e.DocumentoId
                                                      where e.id == d.WfcpDocumento_Estatus.OrderByDescending(x => x.id).FirstOrDefault().id
                                                      && e.EstatusId == 28 //Aceptado por GM
                                                      select d).OrderBy(x => x.id).ToList();

                    foreach (WfcpDocumento documento in documentos)
                    {
                        try
                        {
                            List<WfcpNuevaPartida> nuevas_partidas = db.WfcpNuevaPartida.Where(x => x.DocumentoId == documento.id).ToList();
                            List<ZMMTT_ME32L_SOL_POS> IT_LIST = new List<ZMMTT_ME32L_SOL_POS>();
                            foreach (WfcpNuevaPartida partida in nuevas_partidas)
                            {
                                IT_LIST.Add(new ZMMTT_ME32L_SOL_POS()
                                {
                                    PURCHASINGDOCUMENT = partida.documento_oc.ToString().PadLeft(10, '0'),
                                    LIFNR = partida.no_proveedor.ToString().PadLeft(10,'0'),
                                    ITEM_CAT = partida.WfcpTipoProcuracion.nombre,
                                    MATERIAL = partida.no_parte,
                                    TARGET_QTY = partida.target,
                                    PRICE = partida.precio,
                                    BASE = 1000
                                });
                            }
                            List<ET_RETURN> ET_RETURN = SAPConector30.ZFM_SA_NEW_POS(IT_LIST);

                            if (ET_RETURN.Any())
                            {
                                List<WfcpLog_ET_RETURN> log = new List<WfcpLog_ET_RETURN>();
                                foreach (ET_RETURN error in ET_RETURN)
                                {
                                    log.Add(new WfcpLog_ET_RETURN() { DocumentoId = documento.id, fecha_insert = DateTime.Now, IDENTIFICADOR = error.ID, MESSAGE = error.MESSAGE, NUMBER = error.NUMBER, TYPE = error.TYPE });
                                }
                                db.WfcpLog_ET_RETURN.AddRange(log);
                                db.SaveChanges();
                                documento.ZFM_SA_NEW_POS = false;
                            }
                            else
                            {
                                WfcpLog_ET_RETURN log_exitoso = new WfcpLog_ET_RETURN() { DocumentoId = documento.id, fecha_insert = DateTime.Now, IDENTIFICADOR = "0", MESSAGE = "Nuevas Partidas Ejecutado Correctamente", NUMBER = 0, TYPE = "S" };
                                db.Entry(log_exitoso).State = System.Data.Entity.EntityState.Added;
                                db.SaveChanges();
                                documento.ZFM_SA_NEW_POS = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            WfcpLog_ET_RETURN log = new WfcpLog_ET_RETURN() { DocumentoId = documento.id, fecha_insert = DateTime.Now, IDENTIFICADOR = "0", MESSAGE = ex.Message, NUMBER = 0, TYPE = "E" };
                            db.Entry(log).State = System.Data.Entity.EntityState.Added;
                            db.SaveChanges();
                            documento.ZFM_SA_NEW_POS = false;
                        }
                        finally
                        {
                            db.Entry(documento).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
