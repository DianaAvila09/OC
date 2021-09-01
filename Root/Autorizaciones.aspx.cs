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
    public partial class Autorizaciones : System.Web.UI.Page
    {
        protected WFOCEntities db = new WFOCEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlRolesUsuario.Items.Clear();
                foreach (WfcpUsuarioInRol rol in db.WfcpUsuarioInRol.Where(x => x.WfcpUsuario.usuario == User.Identity.Name && x.activo == true).OrderBy(x => x.RolId).ToList())
                {
                    ddlRolesUsuario.Items.Add(new ListItem() { Value = rol.WfcpRol.id.ToString(), Text = rol.WfcpRol.nombre });
                }
                ddlRolesUsuario_SelectedIndexChanged(null, null);
            }
        }

        protected void ddlRolesUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (ddlRolesUsuario.SelectedValue)
            {
                case "0":
                    btnDetalle.Visible = false;
                    break;
                default:
                    btnDetalle.Visible = true;
                    break;
            }
            load_registros(-1, -1, 0);
        }

        protected void load_registros(Int32 selectedIndex, Int32 editIndex, Int32 pageIndex)
        {
            try
            {
                btnDetalle.Enabled = selectedIndex < 0 ? false : true;

                int.TryParse(txtFiltro.Text.Trim(), out int folio);
                Int32.TryParse(ddlRolesUsuario.SelectedValue, out Int32 _rolId);
                WfcpRol rol = db.WfcpRol.Where(x => x.id == _rolId).FirstOrDefault();
                List<Int32> estatus_visible = new List<Int32>();
                switch (_rolId)
                {
                    case 1: //Compras
                        estatus_visible.Add(11);
                        estatus_visible.Add(21);
                        break;
                    case 2://Finanzas
                        //////////////estatus_visible.Add(12); ahora lo ve Negocios 
                        estatus_visible.Add(18);//Aceptado por Negocios
                        estatus_visible.Add(22);
                        break;
                    case 3://AGM
                        estatus_visible.Add(14);
                        estatus_visible.Add(24);
                        break;
                    case 4://GM
                        estatus_visible.Add(26);
                        break;
                    case 5://Negocios
                        estatus_visible.Add(12);
                        break;
                    default:
                        break;
                }

                gvDocumentos.DataSource = (from a in db.WfcpDocumento
                                           where a.id == (folio > 0 ? folio : a.id)
                                           //&& a.WorkflowId == 2 && a.usuario == User.Identity.Name
                                           join e in db.WfcpDocumento_Estatus on a.id equals e.DocumentoId into es
                                           from x in es.DefaultIfEmpty()
                                           where x.id == a.WfcpDocumento_Estatus.OrderByDescending(x => x.id).FirstOrDefault().id
                                           && estatus_visible.Contains(x.EstatusId)
                                           select new
                                           {
                                               a.id,
                                               workflow = a.WfcpWorkflow.nombre,
                                               a.documento,
                                               a.descripcion,
                                               a.fecha,
                                               a.usuario,
                                               estatus = (x == null ? "Nuevo" : x.WfcpEstatus.descripcion)
                                           }).OrderBy(x => x.id).ToList();

                gvDocumentos.SelectedIndex = selectedIndex;
                gvDocumentos.EditIndex = editIndex;
                gvDocumentos.PageIndex = pageIndex;
                gvDocumentos.DataBind();
            }
            catch (Exception ex)
            {
                btnDetalle.Enabled = false;
                alerta(ex.Message);
            }
        }

        protected void alerta(String mensaje)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), DateTime.Now.ToString(), "alert('" + Regex.Replace(mensaje, @"'+", "") + "');", true);
        }

        protected void btnFiltro_Click(object sender, EventArgs e)
        {
            load_registros(-1, -1, 0);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
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
                            Response.Redirect("~/CambioPrecioDetalle.aspx", true);
                            break;
                        case 2:
                            Response.Redirect("~/NuevaPartidaDetalle.aspx", true);
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
                            Response.Redirect("~/CambioPrecioDetalle.aspx", true);
                            break;
                        case 2:
                            Response.Redirect("~/NuevaPartidaDetalle.aspx", true);
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