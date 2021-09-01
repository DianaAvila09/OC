using Root.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Root
{
    public partial class Historico : System.Web.UI.Page
    {
        protected WFOCEntities db = new WFOCEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    ddlWorkflow.Items.Clear();
                    foreach (var item in db.WfcpWorkflow)
                    {
                        ddlWorkflow.Items.Add(new ListItem() { Value = item.id.ToString(), Text = item.nombre, Enabled = item.activo });
                    }
                    ddlWorkflow_SelectedIndexChanged(null, null);
                }
                catch (Exception ex)
                {
                    alerta(ex.Message);
                }

            }
        }

        protected void alerta(String mensaje)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), DateTime.Now.ToString(), "alert('" + Regex.Replace(mensaje, @"'+", "") + "');", true);
        }

        protected void btnFiltro_Click(object sender, EventArgs e)
        {
            load_registros(-1,-1,0);
        }

        protected void load_registros(Int32 selectedIndex, Int32 editIndex, Int32 pageIndex)
        {
            try
            {
                btnDetalle.Enabled = selectedIndex < 0 ? false : true;
                Int32.TryParse(txtFiltro.Text.Trim(), out Int32 _folio);
                if (ddlEstatus.SelectedValue == "0")
                {
                    gvDocumentos.DataSource = (from d in db.WfcpDocumento.Where(x => x.WorkflowId.ToString() == ddlWorkflow.SelectedValue && x.id == (_folio > 0 ? _folio : x.id))
                                               join de in db.WfcpDocumento_Estatus on d.id equals de.DocumentoId
                                               where de.id == d.WfcpDocumento_Estatus.OrderByDescending(x => x.id).FirstOrDefault().id
                                               select new
                                               {
                                                   d.id,
                                                   workflow = d.WfcpWorkflow.nombre,
                                                   d.descripcion,
                                                   d.fecha,
                                                   d.usuario,
                                                   d.fecha_actualizacion,
                                                   estatus = de.WfcpEstatus.nombre,
                                                   estatus_desc = de.WfcpEstatus.descripcion,
                                                   de.fecha_estatus,
                                                   estatus_usuario = de.usuario,
                                                   de.metodo_autorizacion,
                                                   estatus_rol = de.WfcpRol.nombre
                                               }).OrderByDescending(x => x.id).ToList();

                }
                else
                {
                    gvDocumentos.DataSource = (from d in db.WfcpDocumento.Where(x => x.WorkflowId.ToString() == ddlWorkflow.SelectedValue && x.id == (_folio > 0 ? _folio : x.id))
                                               join de in db.WfcpDocumento_Estatus on d.id equals de.DocumentoId
                                               where d.WfcpDocumento_Estatus.OrderByDescending(x => x.id).Take(1).FirstOrDefault().EstatusId.ToString() == ddlEstatus.SelectedValue
                                               select new
                                               {
                                                   d.id,
                                                   workflow = d.WfcpWorkflow.nombre,
                                                   d.descripcion,
                                                   d.fecha,
                                                   d.usuario,
                                                   d.fecha_actualizacion,
                                                   estatus = de.WfcpEstatus.nombre,
                                                   estatus_desc = de.WfcpEstatus.descripcion,
                                                   de.fecha_estatus,
                                                   estatus_usuario = de.usuario,
                                                   de.metodo_autorizacion,
                                                   estatus_rol = de.WfcpRol.nombre
                                               }).OrderByDescending(x => x.id).ToList();
                }
                gvDocumentos.SelectedIndex = selectedIndex;
                gvDocumentos.EditIndex = editIndex;
                gvDocumentos.PageIndex = pageIndex;
                gvDocumentos.DataBind();
            }
            catch (Exception ex)
            {
                alerta(ex.Message);
            }
        }

        protected void gvDocumentos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            load_registros(-1, -1, e.NewPageIndex);
        } 

        protected void gvDocumentos_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            try
            {
                if (int.TryParse(gvDocumentos.DataKeys[e.NewSelectedIndex].Value.ToString(), out int id) && id > 0)
                {
                    WfcpDocumento documento = db.WfcpDocumento.Where(x => x.id == id).FirstOrDefault();
                    if (documento != null)
                    {
                        hdnDocumentoId.Value = documento.id.ToString();
                        load_registros(e.NewSelectedIndex, -1, gvDocumentos.PageIndex);
                    }
                    else
                    {
                        alerta("El registro ya no existe en base de datos");
                    }
                }
                else
                {
                    alerta("Selecciona un registro");
                }
            }
            catch (Exception ex)
            {
                alerta(ex.Message);
            }
        }

        protected void ddlWorkflow_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlEstatus.Items.Clear();
                foreach (var item in db.WfcpEstatus.Where(x => x.WorkflowId.ToString() == ddlWorkflow.SelectedValue))
                {
                    ddlEstatus.Items.Add(new ListItem() { Value = item.id.ToString(), Text = item.descripcion, Enabled = item.activo });
                }
                ddlEstatus.Items.Insert(0, new ListItem() { Value = "0", Text = "TODOS" });
            }
            catch (Exception ex)
            {
                alerta(ex.Message);
            }
        }

        protected void btnDetalle_Click(object sender, EventArgs e)
        {
            try
            {
                Session["Documentos.DocumentoId"] = hdnDocumentoId.Value;
                Int32.TryParse(hdnDocumentoId.Value, out Int32 DocumentoId);
                WfcpDocumento documento = db.WfcpDocumento.Where(x => x.id == DocumentoId).FirstOrDefault();
                if (documento != null)
                {
                    switch (documento.WorkflowId)
                    {
                        case 1:
                            Response.Redirect("~/HistoricoDetalle.aspx", true);
                            break;
                        case 2:
                            Response.Redirect("~/HistoricoDetalle.aspx", true);
                            break;
                        default:
                            alerta("WorkFlow no Idnetificado");
                            break;
                    }
                }
                else
                {
                    alerta("Documento no encontrado, veritifque que no hala sido eliminado");
                }

            }
            catch (Exception ex)
            {
                alerta(ex.Message);
            }
        }
    }
}