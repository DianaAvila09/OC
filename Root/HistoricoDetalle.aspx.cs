using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HtmlAgilityPack;
using Root.Model;

namespace Root
{
    public partial class HistoricoDetalle : System.Web.UI.Page
    {
        protected WFOCEntities db;

        public void alerta(String mensaje)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), DateTime.Now.ToString(), "alert('" + Regex.Replace(mensaje, @"'+", "") + "');", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            db = new WFOCEntities();
            if (!Page.IsPostBack)
            {
                if (Request.QueryString.ToString().ToUpper().ToString().Contains("ID="))
                {
                    Int32.TryParse(Request.QueryString["id"], out Int32 _DocumentoId);
                    Session["Documentos.DocumentoId"] = _DocumentoId;
                }
                if (Session["Documentos.DocumentoId"] != null)
                {
                    int.TryParse(Session["Documentos.DocumentoId"].ToString(), out int _DocumentoId);
                    if (_DocumentoId > 0)
                    {
                        set_documento(_DocumentoId);
                        load_historico();
                        load_registros(-1, -1, 0);
                    }
                    else
                    {
                        Response.Redirect("~/Historico.aspx");
                    }
                }
                else
                {
                    Response.Redirect("~/Historico.aspx");
                }
            }
        }

        protected void set_documento(Int32 DocumentoId)
        {
            try
            {
                using (WFOCEntities wf = new WFOCEntities())
                {
                    WfcpDocumento documento = wf.WfcpDocumento.Where(x => x.id == DocumentoId).FirstOrDefault();
                    WfcpDocumento_Estatus ultimo_estatus = wf.WfcpDocumento_Estatus.Where(x => x.DocumentoId == DocumentoId).OrderByDescending(x => x.id).FirstOrDefault();
                    hdnDocumentoId.Value = documento.id.ToString();
                    hdnWorkflowId.Value = documento.WorkflowId.ToString();
                    txtId.Text = documento.id.ToString();
                    txtWorkflow.Text = documento.WfcpWorkflow.nombre;
                    txtDocumento.Text = documento.documento.ToString();
                    txtDescripcion.Text = documento.descripcion;
                    txtFecha.Text = documento.fecha.ToString("yyyy/MM/dd HH:mm");
                    txtUsuario.Text = documento.usuario;// "MAGNA\\hmedina";// ;
                    txtEstatus.Text = ultimo_estatus != null ? ultimo_estatus.WfcpEstatus.descripcion : "Nuevo";
                    switch (documento.WorkflowId)
                    {
                        case 1:
                            lblWorkflow.Text = "DETALLE DE CAMBIO DE PRECIO";
                            break;
                        case 2:
                            lblWorkflow.Text = "DETALLE DE NUEVA PARTIDA";
                            break;
                        default:
                            lblWorkflow.Text = "PROCESO NO IDENTIFICADO";
                            break;
                    }
                    lblWorkflow2.Text = documento.WfcpWorkflow.nombre;
            }

            }
            catch (Exception ex)
            {
                alerta(ex.Message);
            }
        }

        protected void load_historico()
        {
            int.TryParse(txtId.Text.Trim(), out int _DocumentoId);
            try
            {
                gvHistorico.DataSource = (from p in db.WfcpDocumento_Estatus
                                          where p.DocumentoId == _DocumentoId
                                          select new
                                          {
                                              p.id,
                                              Estatus = p.WfcpEstatus.descripcion,
                                              Fecha = p.fecha_estatus,
                                              Usuario = p.usuario,
                                              Rol = p.WfcpRol.nombre
                                          }
                          ).Distinct().OrderBy(x=>x.id).ToList();
                gvHistorico.DataBind();
            }
            catch (Exception ex)
            {
                alerta("Historico: " + ex.Message);
            }
        }

        protected void load_registros(int selectedIndex, int editIndex, int pageIndex)
        {
            Int32.TryParse(hdnDocumentoId.Value, out Int32 _DocumentoId);
            try
            { 
                switch (hdnWorkflowId.Value)
                {
                    case "1": 
                        gvDocumentosDetalle.DataSource = (from a in db.WfcpCambioPrecio
                                                          where a.DocumentoId == _DocumentoId
                                                          join pln in db.WfcpDocumento_Proveedor on a.documento_oc equals pln.documento_oc into pl
                                                          from p in pl.DefaultIfEmpty()
                                                          select new
                                                          {
                                                              a.id,
                                                              a.DocumentoId,
                                                              a.documento_oc,
                                                              a.no_parte,
                                                              a.precio,
                                                              moneda = a.WfcpMoneda.nombre,
                                                              a.fecha_punto_quiebre,
                                                              a.justificacion,
                                                              a.usuario,
                                                              a.fecha_actualizacion,
                                                              a.ITEM_NO,
                                                              a.KTMGN,
                                                              a.MEINS,
                                                              a.NETPBR,
                                                              a.PEINH,
                                                              p.NAME_VENDOR
                                                          }).OrderBy(x => x.id).ToList().AsEnumerable().Select((a, i) => new {
                                                              Id = a.id,
                                                              Partida = i + 1,
                                                              Documento_OC = a.documento_oc,
                                                              No_Parte = a.no_parte,
                                                              Precio = a.precio,
                                                              Moneda = a.moneda,
                                                              Fecha_PuntoQuiebre = a.fecha_punto_quiebre.ToString("yyyy/MM/dd HH:mm"),
                                                              Justificacion = a.justificacion,
                                                              Usuario = a.usuario,
                                                              Fecha_Registro = a.fecha_actualizacion.ToString("yyyy/MM/dd HH:mm"),
                                                              Item_No = a.ITEM_NO,
                                                              Unidad_Medida = a.MEINS,
                                                              Precio_Anterior = a.NETPBR,
                                                              Proveedor = a.NAME_VENDOR
                                                          });
                        break;
                    case "2":
                        gvDocumentosDetalle.DataSource = (from a in db.WfcpNuevaPartida
                                                          where a.DocumentoId == _DocumentoId
                                                          select new
                                                          {
                                                              a.id,
                                                              a.DocumentoId,
                                                              a.no_proveedor,
                                                              a.documento_oc,
                                                              tipo_procuracion = a.WfcpTipoProcuracion.descripcion,
                                                              a.no_parte,
                                                              a.target,
                                                              a.precio,
                                                              moneda = a.WfcpMoneda.nombre,
                                                              a.fecha_punto_quiebre,
                                                              unidad_medida = a.WfcpUnidadMedida.descripcion,
                                                              a.justificacion,
                                                              a.usuario,
                                                              a.fecha_actualizacion,
                                                              a.ITEM_NO,
                                                              a.LIFNR,
                                                              a.PSTYP,
                                                              a.KTMNG,
                                                              a.NETPR,
                                                              a.PEINH,
                                                              a.BSTAE,
                                                              a.ETFZ1,
                                                              a.ABUEB,
                                                              a.LOEKZ
                                                          }).OrderBy(x => x.id).ToList().AsEnumerable().Select((a, i) => new {
                                                              Id = a.id,
                                                              Partida = i + 1,
                                                              No_Proveedor = a.no_proveedor,
                                                              Documento_OC = a.documento_oc,
                                                              Tipo_Procuracion = a.tipo_procuracion,
                                                              No_Parte = a.no_parte,
                                                              Target = a.target,
                                                              Precio = a.precio,
                                                              Moneda = a.moneda,
                                                              Fecha_PuntoQuiebre = a.fecha_punto_quiebre.ToString("yyyy/MM/dd HH:mm"),
                                                              Unidad_Medida = a.unidad_medida,
                                                              Justificacion = a.justificacion,
                                                              Usuario = a.usuario,
                                                              Fecha_Registro = a.fecha_actualizacion.ToString("yyyy/MM/dd HH:mm"),
                                                              Item_No = a.ITEM_NO
                                                          });
                        break;
                    default:
                        gvDocumentosDetalle.DataSource = null;
                        break;
                }
                

                gvDocumentosDetalle.SelectedIndex = selectedIndex;
                gvDocumentosDetalle.EditIndex = editIndex;
                gvDocumentosDetalle.PageIndex = pageIndex;
                gvDocumentosDetalle.DataBind();
            }
            catch (Exception ex)
            {
                alerta(ex.Message);
            }
        }

        protected void gvDocumentosDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            load_registros(-1, -1, gvDocumentosDetalle.PageIndex);
        }

        protected void gvDocumentosDetalle_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            load_registros(e.NewSelectedIndex, -1, gvDocumentosDetalle.PageIndex);
        }

        public static string BaseSiteUrl
        {
            get
            {
                HttpContext context = HttpContext.Current;
                string baseUrl = context.Request.Url.Authority + context.Request.ApplicationPath.TrimEnd('/');
                return baseUrl;
            }

        }

    }
}