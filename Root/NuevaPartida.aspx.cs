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
    public partial class NuevaPartida : System.Web.UI.Page
    {
        protected WFOCEntities db = new WFOCEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlWorkflow.Items.Clear();
                foreach (WfcpWorkflow wf in db.WfcpWorkflow.Where(x => x.id == 2))
                {
                    ddlWorkflow.Items.Add(new ListItem() { Value = wf.id.ToString(), Text = wf.nombre, Enabled = wf.activo });
                }

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
                    btnDelete.Visible = true;
                    btnEdit.Visible = true;
                    btnDetalle.Visible = false;
                    btnAdd.Visible = true;
                    break;
                default:
                    btnDelete.Visible = false;
                    btnEdit.Visible = false;
                    btnDetalle.Visible = true;
                    btnAdd.Visible = false;
                    break;
            }
            load_registros(-1, -1, 0);
        }

        protected void load_registros(Int32 selectedIndex, Int32 editIndex, Int32 pageIndex)
        {
            try
            {
                hdnDocumentoId.Value = selectedIndex < 0 ? "0" : hdnDocumentoId.Value;
                btnEdit.Enabled = selectedIndex < 0 ? false : true;
                btnDetalle.Enabled = selectedIndex < 0 ? false : true;
                btnDelete.Enabled = selectedIndex < 0 ? false : true;


                int.TryParse(txtFiltro.Text.Trim(), out int folio);
                Int32.TryParse(ddlRolesUsuario.SelectedValue, out Int32 _rolId);
                WfcpRol rol = db.WfcpRol.Where(x => x.id == _rolId).FirstOrDefault();

                gvDocumentos.DataSource = (from a in db.WfcpDocumento
                                           where a.WorkflowId == 2 && a.usuario == User.Identity.Name 
                                           && a.id == (folio > 0 ? folio : a.id)
                                           join e in db.WfcpDocumento_Estatus on a.id equals e.DocumentoId into es
                                           from x in es.DefaultIfEmpty()
                                           where x.id == a.WfcpDocumento_Estatus.OrderByDescending(x => x.id).FirstOrDefault().id
                                           //&& a.WfcpDocumento_Estatus.OrderByDescending(x => x.id).FirstOrDefault().WfcpEstatus.valido
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
                btnEdit.Enabled = false;
                btnDetalle.Enabled = false;
                btnDelete.Enabled = false;
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvDocumentos.SelectedIndex > -1 && Int32.TryParse(gvDocumentos.DataKeys[gvDocumentos.SelectedIndex].Value.ToString(), out Int32 _id) && _id > 0)
                {
                    WfcpDocumento documento = db.WfcpDocumento.Where(x => x.id == _id).FirstOrDefault();
                    if (documento != null)
                    {
                        db.Entry(documento).State = System.Data.Entity.EntityState.Deleted;
                        db.SaveChanges();
                        load_registros(-1, -1, 0);
                    }
                    else
                    {
                        alerta("El registro no se encontró o ya habia sido eliminado previamente.");
                    }
                }
                else
                {
                    alerta("Selecciona un registro");
                }
            }
            catch (Exception)
            {
                alerta("El registro ya se encuentra en proceso de revisión.");
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Session["Documentos.DocumentoId"] = hdnDocumentoId.Value;
                Response.Redirect("~/NuevaPartidaDetalle.aspx", true);
            }
            catch (Exception ex)
            {
                alerta(ex.Message);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                hdnDocumentoId.Value = "";
                hdnLastRolAccepted.Value = "";
                txtDescripcion.Text = "";
                txtFecha.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                WfcpUsuario usuario = db.WfcpUsuario.Where(x => x.usuario == User.Identity.Name).FirstOrDefault();
                if (usuario != null)
                {
                    hdnUsuarioId.Value = usuario.id.ToString();
                    txtUsuario.Text = usuario.usuario;
                }
                else
                {
                    hdnUsuarioId.Value = "";
                    txtUsuario.Text = "";
                }
                modalAgregar.Show();
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
                        hdnUsuarioId.Value = documento.usuario.ToString();
                        txtUsuario.Text = documento.usuario;
                        txtFecha.Text = documento.fecha.ToString("yyyy/MM/dd HH:mm");
                        load_registros(e.NewSelectedIndex, -1, gvDocumentos.PageIndex);
                    }
                    else
                    {
                        limpiar_form();
                        alerta("El registro ya no existe en base de datos");
                    }
                }
                else
                {
                    limpiar_form();
                    alerta("Selecciona un registro");
                }
            }
            catch (Exception ex)
            {
                limpiar_form();
                alerta(ex.Message);
            }

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar_form();
            modalAgregar.Hide();
        }

        protected void limpiar_form()
        {
            ddlWorkflow.SelectedIndex = -1;
            txtDocumento.Text = "";
            txtDescripcion.Text = "";
            txtFecha.Text = "";
            txtUsuario.Text = "";
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                long.TryParse(txtDocumento.Text.Trim(), out long _documento);
                DateTime.TryParse(txtFecha.Text, out DateTime _fecha);
                WfcpDocumento documento = new WfcpDocumento() { id = 0, documento = _documento, descripcion = txtDescripcion.Text.Trim(), fecha = _fecha, usuario = User.Identity.Name, fecha_actualizacion = DateTime.Now, WorkflowId = int.Parse(ddlWorkflow.SelectedValue) };
                db.Entry(documento).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();

                hdnDocumentoId.Value = documento.id.ToString();
                hdnUsuarioId.Value = documento.usuario.ToString();

                btnEdit_Click(null, null);
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
                Response.Redirect("~/NuevaPartidaDetalle.aspx", true);
            }
            catch (Exception ex)
            {
                alerta(ex.Message);
            }
        }
    }
}