using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
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
    public partial class NuevaPartidaDetalle : System.Web.UI.Page
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
                        ddlRolesUsuario.Items.Clear();
                        foreach (WfcpUsuarioInRol rol in db.WfcpUsuarioInRol.Where(x => x.WfcpUsuario.usuario == User.Identity.Name && x.activo == true).OrderBy(x => x.RolId).ToList())
                        {
                            ddlRolesUsuario.Items.Add(new ListItem() { Value = rol.WfcpRol.id.ToString(), Text = rol.WfcpRol.nombre });
                        }

                        if (ddlRolesUsuario.Items.Count == 0)
                        {
                            Response.Redirect("~/SinAcceso.aspx");
                        }

                        set_documento(_DocumentoId);
                        load_historico();
                        load_combos();

                        ddlRolesUsuario_SelectedIndexChanged(null, null);
                    }
                    else
                    {
                        Response.Redirect("~/NuevaPartida.aspx");
                    }
                }
                else
                {
                    Response.Redirect("~/NuevaPartida.aspx");
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
                    if (documento.WorkflowId != 2)
                    {
                        alerta("WorkFlow no corresponde");
                        botones_visibles(false);
                        return;
                    }
                    WfcpDocumento_Estatus ultimo_estatus = wf.WfcpDocumento_Estatus.Where(x => x.DocumentoId == DocumentoId).OrderByDescending(x => x.id).FirstOrDefault();

                    hdnDocumentoId.Value = documento.id.ToString();
                    txtId.Text = documento.id.ToString();
                    txtWorkflow.Text = documento.WfcpWorkflow.nombre;
                    txtDocumento.Text = documento.documento.ToString();
                    txtDescripcion.Text = documento.descripcion;
                    txtFecha.Text = documento.fecha.ToString("yyyy/MM/dd HH:mm");
                    txtUsuario.Text = documento.usuario;
                    txtEstatus.Text = ultimo_estatus != null ? ultimo_estatus.WfcpEstatus.descripcion : "Nuevo";
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
                                              Estatus = p.WfcpEstatus.descripcion,
                                              Fecha = p.fecha_estatus,
                                              Usuario = p.usuario,
                                              Rol = p.WfcpRol.nombre
                                          }
                          ).Distinct().ToList();
                gvHistorico.DataBind();
            }
            catch (Exception ex)
            {
                alerta("Historico: " + ex.Message);
            }
        }

        protected void load_combos()
        {
            ddlMoneda.Items.Clear();
            foreach (WfcpMoneda moneda in db.WfcpMoneda)
            {
                ddlMoneda.Items.Add(new ListItem() { Value = moneda.id.ToString(), Text = moneda.nombre, Enabled = moneda.activo });
            }
            ddlMoneda.Items.Insert(0, new ListItem() { Value = "0", Text = "SELECCIONA UN MONEDA", Selected = true });

            ddlTipoProcuracion.Items.Clear();
            foreach (WfcpTipoProcuracion tipo_procuracion in db.WfcpTipoProcuracion)
            {
                ddlTipoProcuracion.Items.Add(new ListItem() { Value = tipo_procuracion.id.ToString(), Text = tipo_procuracion.descripcion, Enabled = tipo_procuracion.activo });
            }
            ddlTipoProcuracion.Items.Insert(0, new ListItem() { Value = "0", Text = "SELECCIONA UN TIPO PROCURACIÓN", Selected = true });

            ddlUnidadMedida.Items.Clear();
            foreach (WfcpUnidadMedida um in db.WfcpUnidadMedida)
            {
                ddlUnidadMedida.Items.Add(new ListItem() { Value = um.id.ToString(), Text = um.descripcion, Enabled = um.activo });
            }
            ddlUnidadMedida.Items.Insert(0, new ListItem() { Value = "0", Text = "SELECCIONA UNA UNIDAD DE MEDIDA", Selected = true });
        }

        protected void ddlRolesUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                db = new WFOCEntities();
                Int32.TryParse(hdnDocumentoId.Value, out Int32 _DocumentoId);
                Int32.TryParse(ddlRolesUsuario.SelectedValue, out Int32 _rolId);
                WfcpRol rol = db.WfcpRol.Where(x => x.id == _rolId).FirstOrDefault();
                WfcpDocumento_Estatus ultimo_estatus = db.WfcpDocumento_Estatus.Where(x => x.DocumentoId == _DocumentoId).OrderByDescending(x => x.id).FirstOrDefault();
                switch (ddlRolesUsuario.SelectedValue)
                {
                    case "0":
                        if (txtUsuario.Text.Trim() == User.Identity.Name)
                        {
                            btnAutorizar.Visible = false;
                            btnRechazar.Visible = false;
                            if (ultimo_estatus == null)
                                botones_visibles(true);
                            else
                                botones_visibles(false);
                        }
                        else
                        {
                            btnAutorizar.Visible = false;
                            btnRechazar.Visible = false;
                            botones_visibles(false);
                        }
                        break;
                    default:
                        botones_visibles(false);
                        if (ultimo_estatus != null && ultimo_estatus.WfcpEstatus.valido && ultimo_estatus.RolId == rol.id -1)
                        {
                            btnAutorizar.Visible = true;
                            btnRechazar.Visible = true;
                        }
                        else
                        {
                            btnAutorizar.Visible = false;
                            btnRechazar.Visible = false;
                        }
                        break;
                }
                load_registros(-1, -1, 0);
            }
            catch (Exception ex)
            {
                btnAutorizar.Visible = false;
                btnRechazar.Visible = false;
                botones_visibles(false);
                alerta(ex.Message);
            }
        }

        protected void botones_visibles(Boolean bit)
        {
            btnDelete.Visible = bit;
            btnEdit.Visible = bit;
            btnDetalle.Visible = !bit;
            btnAdd.Visible = bit;
            btnCancelarDocumento.Visible = bit;
            btnGuardarDocumento.Visible = bit;
            btnValidarDocumento.Visible = bit;
        }

        protected void bloquear_controles(Boolean bit)
        {
            txtNoProveedor.Enabled = !bit;
            txtDocumentoOC.Enabled = !bit;
            ddlTipoProcuracion.Enabled = !bit;
            txtNoParte.Enabled = !bit;
            txtTarget.Enabled = !bit;
            txtPrecio.Enabled = !bit;
            ddlMoneda.Enabled = !bit;
            txtFechaPuntoQuiebre.Enabled = !bit;
            ddlUnidadMedida.Enabled = !bit;
            txtJustificacion.Enabled = !bit;
            btnAgregar.Visible = !bit;
        }

        protected void load_registros(int selectedIndex, int editIndex, int pageIndex)
        {
            Int32.TryParse(hdnDocumentoId.Value, out Int32 _DocumentoId);
            hdnNuevaPartidaId.Value = selectedIndex < 0 ? "0" : hdnNuevaPartidaId.Value;
            btnEdit.Enabled = selectedIndex < 0 ? false : true;
            btnDetalle.Enabled = selectedIndex < 0 ? false : true;
            btnDelete.Enabled = selectedIndex < 0 ? false : true;
            try
            {
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
                                                      row_number = i + 1,
                                                      a.id,
                                                      a.DocumentoId,
                                                      a.no_proveedor,
                                                      a.documento_oc,
                                                      a.tipo_procuracion,
                                                      a.no_parte,
                                                      a.target,
                                                      a.precio,
                                                      a.moneda,
                                                      a.fecha_punto_quiebre,
                                                      a.unidad_medida,
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
                                                  });
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

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvDocumentosDetalle.SelectedIndex > -1 && int.TryParse(gvDocumentosDetalle.DataKeys[gvDocumentosDetalle.SelectedIndex].Value.ToString(), out int id) && id > 0)
                {
                    WfcpNuevaPartida detalle = db.WfcpNuevaPartida.Where(x => x.id == id).FirstOrDefault();
                    if (detalle != null)
                    {
                        hdnNuevaPartidaId.Value = detalle.id.ToString();
                        txtNoProveedor.Text = detalle.no_proveedor.ToString();
                        txtDocumentoOC.Text = detalle.documento_oc.ToString();
                        ddlTipoProcuracion.SelectedValue = detalle.TipoProcuracionId.ToString();
                        txtNoParte.Text = detalle.no_parte;
                        txtTarget.Text = detalle.target.ToString();
                        txtPrecio.Text = detalle.precio.ToString();
                        ddlMoneda.SelectedValue = detalle.MonedaId.ToString();
                        txtFechaPuntoQuiebre.Text = detalle.fecha_punto_quiebre.ToString("yyyy/MM/dd");
                        ddlUnidadMedida.SelectedValue = detalle.UnidadMedidaId.ToString();
                        txtJustificacion.Text = detalle.justificacion;
                        btnAgregar.Enabled = true;
                        bloquear_controles(false);
                        modalAgregar.Show();
                    }
                    else
                    {
                        limpiar_form();
                        alerta("No se encontro el registro de Nueva Partida");
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
                alerta(ex.Message);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvDocumentosDetalle.SelectedIndex > -1 && Int32.TryParse(gvDocumentosDetalle.DataKeys[gvDocumentosDetalle.SelectedIndex].Value.ToString(), out Int32 _id) && _id > 0)
                {
                    WfcpNuevaPartida detalle = db.WfcpNuevaPartida.Where(x => x.id == _id).FirstOrDefault();
                    if (detalle != null)
                    {
                        db.Entry(detalle).State = System.Data.Entity.EntityState.Deleted;
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
            catch (Exception ex)
            {
                alerta(ex.Message);
            }
        }

        protected void limpiar_form()
        {
            hdnNuevaPartidaId.Value = "";
            txtNoProveedor.Text = "";
            txtDocumentoOC.Text = "";
            ddlTipoProcuracion.SelectedIndex = -1;
            txtNoParte.Text = "";
            txtTarget.Text = "";
            txtPrecio.Text = "";
            ddlMoneda.SelectedIndex = -1;
            txtFechaPuntoQuiebre.Text = "";
            ddlUnidadMedida.SelectedIndex = -1;
            txtJustificacion.Text = "";
        }

        protected void gvDocumentosDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            load_registros(-1, -1, gvDocumentosDetalle.PageIndex);
        }

        protected void gvDocumentosDetalle_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            load_registros(e.NewSelectedIndex, -1, gvDocumentosDetalle.PageIndex);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (captura_valida())
                {
                    Int32.TryParse(hdnDocumentoId.Value, out Int32 _DocumentoId);
                    Int32.TryParse(hdnNuevaPartidaId.Value, out Int32 _NuevaPartidaId);
                    Int32.TryParse(txtNoProveedor.Text.Trim(), out Int32 _NoProveedor);
                    Int64.TryParse(txtDocumentoOC.Text.Trim(), out Int64 _documento_oc);
                    Int32.TryParse(ddlMoneda.SelectedValue, out Int32 _MonedaId);
                    Double.TryParse(txtPrecio.Text.Trim(), out Double _Precio);
                    Int32.TryParse(ddlTipoProcuracion.SelectedValue, out Int32 _TipoProcuracionId);
                    Int32.TryParse(ddlUnidadMedida.SelectedValue, out Int32 _UnidadMedidaId);
                    Double.TryParse(txtTarget.Text.Trim(), out Double _Target);
                    DateTime.TryParse(txtFechaPuntoQuiebre.Text.Trim(), out DateTime _Fecha_punto_quiebre);

                    WfcpNuevaPartida detalle = db.WfcpNuevaPartida.Where(x => x.id == _NuevaPartidaId).FirstOrDefault();
                    detalle = detalle != null ? detalle : new WfcpNuevaPartida() { DocumentoId = _DocumentoId };

                    detalle.DocumentoId = _DocumentoId;
                    detalle.documento_oc = _documento_oc;
                    detalle.fecha_actualizacion = DateTime.Now;
                    detalle.fecha_punto_quiebre = _Fecha_punto_quiebre;
                    detalle.justificacion = txtJustificacion.Text.Trim();
                    detalle.MonedaId = _MonedaId;
                    detalle.no_parte = txtNoParte.Text.Trim();
                    detalle.precio = _Precio;
                    detalle.usuario = User.Identity.Name;
                    detalle.TipoProcuracionId = _TipoProcuracionId;
                    detalle.UnidadMedidaId = _UnidadMedidaId;
                    detalle.target = _Target;
                    detalle.no_proveedor = _NoProveedor;

                    db.Entry(detalle).State = detalle.id > 0 ? System.Data.Entity.EntityState.Modified : System.Data.Entity.EntityState.Added;
                    db.SaveChanges();

                    load_registros(-1, -1, gvDocumentosDetalle.PageIndex);
                }
                else
                {
                    modalAgregar.Show();
                }
            }
            catch (Exception ex)
            {
                alerta(ex.Message);
                modalAgregar.Show();
            }
        }

        protected Boolean captura_valida()
        {
            if (txtNoProveedor.Text.Trim() == "" || !(Int32.TryParse(txtNoProveedor.Text.Trim(), out Int32 _no_proveedor)))
            {
                alerta("Captura Número de Proveedor válido");
                txtNoProveedor.Focus();
                return false;
            }
            if (txtDocumentoOC.Text.Trim() == "" || !(long.TryParse(txtDocumentoOC.Text.Trim(), out long _documento_oc)))
            {
                alerta("Captura Documento OC valido");
                txtDocumentoOC.Focus();
                return false;
            }
            if (ddlTipoProcuracion.SelectedValue == "0" || ddlTipoProcuracion.SelectedValue == "")
            {
                alerta("Selecciona una Tipo de Procuración");
                ddlTipoProcuracion.Focus();
                return false;
            }
            if (txtNoParte.Text.Trim() == "")
            {
                alerta("Captura Numero de Parte");
                txtNoParte.Focus();
                return false;
            }
            if (txtTarget.Text.Trim() == "" || !(Double.TryParse(txtTarget.Text.Trim(), out Double _target)))
            {
                alerta("Captura Target");
                txtTarget.Focus();
                return false;
            }
            if (txtPrecio.Text.Trim() == "" || !(Double.TryParse(txtPrecio.Text.Trim(), out Double _precio)))
            {
                alerta("Captura Precio");
                txtPrecio.Focus();
                return false;
            }

            if (ddlMoneda.SelectedValue == "0" || ddlMoneda.SelectedValue == "")
            {
                alerta("Selecciona una Moneda");
                ddlMoneda.Focus();
                return false;
            }

            if (txtFechaPuntoQuiebre.Text.Trim() == "" || !(DateTime.TryParse(txtFechaPuntoQuiebre.Text.Trim(), out DateTime _fecha_punto_quiebre)))
            {
                alerta("Captura Fecha de Punto de Quiebre");
                txtFechaPuntoQuiebre.Focus();
                return false;
            }
            if (ddlUnidadMedida.SelectedValue == "0" || ddlUnidadMedida.SelectedValue == "")
            {
                alerta("Selecciona una Unidad de Medida");
                ddlUnidadMedida.Focus();
                return false;
            }
            if (txtJustificacion.Text.Trim() == "")
            {
                alerta("Captura Justificación");
                txtJustificacion.Focus();
                return false;
            }
            return true;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                limpiar_form();
                btnAgregar.Enabled = true;
                modalAgregar.Show();
            }
            catch (Exception ex)
            {
                alerta(ex.Message);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar_form();
            btnAgregar.Enabled = false;
        }

        protected void btnCancelarDocumento_Click(object sender, EventArgs e)
        {
            try
            {
                if (Int32.TryParse(hdnDocumentoId.Value, out Int32 _DocumentoId) && _DocumentoId > 0)
                {
                    WfcpDocumento documento = db.WfcpDocumento.Where(x => x.id == _DocumentoId).FirstOrDefault();
                    if (documento != null)
                    {
                        WfcpDocumento_Estatus estatus = new WfcpDocumento_Estatus() { id = 0, DocumentoId = documento.id, EstatusId = 20, fecha_estatus = DateTime.Now, metodo_autorizacion = "WEB", RolId = int.Parse(ddlRolesUsuario.SelectedValue), usuario = User.Identity.Name };
                        db.Entry(estatus).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();

                        load_historico();
                        ddlRolesUsuario_SelectedIndexChanged(null, null);
                    }
                    else
                    {
                        alerta("No se pudo identificar el registro de Documento");
                    }
                }
                else
                {
                    alerta("No se pudo identificar el registro de Documento");
                }
            }
            catch (Exception ex)
            {
                alerta(ex.Message);
            }
        }

        protected void btnGuardarDocumento_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvDocumentosDetalle.Rows.Count > 0)
                {
                    if (Int32.TryParse(hdnDocumentoId.Value, out Int32 _DocumentoId) && _DocumentoId > 0)
                    {
                        WfcpDocumento documento = db.WfcpDocumento.Where(x => x.id == _DocumentoId).FirstOrDefault();
                        if (documento != null)
                        {
                            if (documento.ZFM_SA_CREA_SOL)
                            {
                                WfcpDocumento_Estatus estatus = new WfcpDocumento_Estatus() { id = 0, DocumentoId = documento.id, EstatusId = 21, fecha_estatus = DateTime.Now, metodo_autorizacion = "WEB", RolId = int.Parse(ddlRolesUsuario.SelectedValue), usuario = User.Identity.Name };
                                db.Entry(estatus).State = System.Data.Entity.EntityState.Added;
                                db.SaveChanges();
                                ////////////////////db.spSendMail_Documento(documento.id);
                                if (EnviarMail_SolicitarAutorizacion(documento.id))
                                {
                                    set_documento(_DocumentoId);
                                    load_historico();
                                    ddlRolesUsuario_SelectedIndexChanged(null, null);
                                }
                                else
                                {
                                    alerta("Ocurrio un problema al enviar la notificación de solicitud de autorización por mail.");
                                }
                            }
                            else
                            {
                                alerta("Las Partidas del documento aun no han sido validadas en su totalidad.");
                            }
                        }
                        else
                        {
                            alerta("No se pudo identificar el registro de Documento");
                        }
                    }
                    else
                    {
                        alerta("No se pudo identificar el registro de Documento");
                    }
                }
                else
                {
                    alerta("No hay partidas registradas");
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
                if (gvDocumentosDetalle.SelectedIndex > -1 && int.TryParse(gvDocumentosDetalle.DataKeys[gvDocumentosDetalle.SelectedIndex].Value.ToString(), out int id) && id > 0)
                {
                    WfcpNuevaPartida detalle = db.WfcpNuevaPartida.Where(x => x.id == id).FirstOrDefault();
                    if (detalle != null)
                    {
                        hdnNuevaPartidaId.Value = detalle.id.ToString();
                        txtNoProveedor.Text = detalle.no_proveedor.ToString();
                        txtDocumentoOC.Text = detalle.documento_oc.ToString();
                        ddlTipoProcuracion.SelectedValue = detalle.TipoProcuracionId.ToString();
                        txtNoParte.Text = detalle.no_parte;
                        txtTarget.Text = detalle.target.ToString();
                        txtPrecio.Text = detalle.precio.ToString();
                        ddlMoneda.SelectedValue = detalle.MonedaId.ToString();
                        txtFechaPuntoQuiebre.Text = detalle.fecha_punto_quiebre.ToString("yyyy/MM/dd");
                        ddlUnidadMedida.SelectedValue = detalle.UnidadMedidaId.ToString();
                        txtJustificacion.Text = detalle.justificacion;
                        btnAgregar.Enabled = false;
                        bloquear_controles(true);
                        modalAgregar.Show();
                    }
                    else
                    {
                        limpiar_form();
                        alerta("No se encontro el registro de Documento");
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
                alerta(ex.Message);
            }
        }


        protected void btnAutorizar_Click(object sender, EventArgs e)
        {
            try
            {
                Int32.TryParse(hdnDocumentoId.Value, out Int32 _DocumentoId);
                WfcpDocumento documento = db.WfcpDocumento.Where(x => x.id == _DocumentoId).FirstOrDefault();
                WfcpRol rol = db.WfcpRol.Where(x => x.id.ToString() == ddlRolesUsuario.SelectedValue).FirstOrDefault();

                WfcpDocumento_Estatus estatus = new WfcpDocumento_Estatus() { id = 0, DocumentoId = documento.id, fecha_estatus = DateTime.Now, metodo_autorizacion = "WEB", RolId = rol.id, usuario = User.Identity.Name };

                switch (rol.id)
                {
                    case 1:
                        estatus.EstatusId = 22;
                        db.Entry(estatus).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                        break;
                    case 2:
                        estatus.EstatusId = 24;
                        db.Entry(estatus).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                        break;
                    case 3:
                        estatus.EstatusId = 26;
                        db.Entry(estatus).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                        break;
                    case 4:
                        estatus.EstatusId = 28;
                        db.Entry(estatus).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                        break;
                    default:
                        alerta("No tiene los permisos necesarios para Autorizar el Documento");
                        break;
                }
                if (EnviarMail_SolicitarAutorizacion(documento.id))
                {
                    set_documento(documento.id);
                    load_historico();
                    ddlRolesUsuario_SelectedIndexChanged(null, null);
                }
                else
                {
                    alerta("Ocurrio un problema al enviar la notificación de solicitud de autorización por mail.");
                }

            }
            catch (Exception ex)
            {
                alerta("Ocurrio un problema al Guardar la información: " + ex.Message);
            }
        }

        protected void btnRechazar_Click1(object sender, EventArgs e)
        {
            try
            {
                Int32.TryParse(hdnDocumentoId.Value, out Int32 _DocumentoId);
                WfcpDocumento documento = db.WfcpDocumento.Where(x => x.id == _DocumentoId).FirstOrDefault();
                WfcpRol rol = db.WfcpRol.Where(x => x.id.ToString() == ddlRolesUsuario.SelectedValue).FirstOrDefault();

                WfcpDocumento_Estatus estatus = new WfcpDocumento_Estatus() { id = 0, DocumentoId = documento.id, fecha_estatus = DateTime.Now, metodo_autorizacion = "WEB", RolId = rol.id, usuario = User.Identity.Name };

                switch (rol.id)
                {
                    case 1:
                        estatus.EstatusId = 23;
                        db.Entry(estatus).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                        break;
                    case 2:
                        estatus.EstatusId = 25;
                        db.Entry(estatus).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                        break;
                    case 3:
                        estatus.EstatusId = 27;
                        db.Entry(estatus).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                        break;
                    case 4:
                        estatus.EstatusId = 29;
                        db.Entry(estatus).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                        break;
                    default:
                        alerta("No tiene los permisos necesarios para Rechazar el Documento");
                        break;
                }

                set_documento(documento.id);
                load_historico();
                ddlRolesUsuario_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                alerta("Ocurrio un problema al Guardar la información: " + ex.Message);
            }
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

        protected bool EnviarMail_SolicitarAutorizacion(Int32 DocumentoId)
        {
            try
            {
                using (WFOCEntities ctx = new WFOCEntities())
                {
                    WfcpDocumento_Estatus ultimo_estatus = ctx.WfcpDocumento_Estatus.Where(x => x.DocumentoId == DocumentoId).OrderByDescending(x => x.id).FirstOrDefault();
                    HtmlDocument doc = new HtmlDocument();
                    doc.Load(Server.MapPath("~/Email/NuevaPartida.html"));
                    string html = doc.ParsedText;
                    html = html.Replace("@Folio", ultimo_estatus.DocumentoId.ToString());
                    html = html.Replace("@Descripcion", ultimo_estatus.WfcpDocumento.descripcion.ToString());
                    html = html.Replace("@Fecha", ultimo_estatus.fecha_estatus.ToString("yyyy/MM/dd HH:mm"));
                    html = html.Replace("@Usuario", ultimo_estatus.WfcpDocumento.usuario);
                    html = html.Replace("@Estatus", ultimo_estatus.WfcpEstatus.descripcion);
                    html = html.Replace("@Link", "http://" + BaseSiteUrl + "/NuevaPartidaDetalle.aspx?id=" + ultimo_estatus.DocumentoId.ToString());

                    StringBuilder sb = new StringBuilder("<table>");
                    sb.Append("<tr><th>Id</th><th>Partida</th><th>No. Proveedor</th><th>Documento OC</th><th>Tipo Procuración</th><th>No. Parte</th><th>Target</th><th>Precio</th><th>Moneda</th><th>Fecha PuntoQuiebre</th><th>Unidad Medida</th><th>Justificacion</th><th>Usuario</th><th>Fecha Registro</th></tr>");
                    int row_number = 1;
                    foreach (WfcpNuevaPartida item in ctx.WfcpNuevaPartida.Where(x => x.DocumentoId == DocumentoId).ToList())
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + item.id.ToString() + "</td>");
                        sb.Append("<td>" + row_number++.ToString() + "</td>");
                        sb.Append("<td>" + item.no_proveedor.ToString() + "</td>");
                        sb.Append("<td>" + item.documento_oc.ToString() + "</td>");
                        sb.Append("<td>" + item.WfcpTipoProcuracion.descripcion.ToString() + "</td>");
                        sb.Append("<td>" + item.no_parte.ToString() + "</td>");
                        sb.Append("<td>" + item.target.ToString() + "</td>");
                        sb.Append("<td>" + item.precio.ToString() + "</td>");
                        sb.Append("<td>" + item.WfcpMoneda.nombre.ToString() + "</td>");
                        sb.Append("<td>" + item.fecha_punto_quiebre.ToString("yyyy/MM/dd HH:mm") + "</td>");
                        sb.Append("<td>" + item.WfcpUnidadMedida.descripcion.ToString() + "</td>");
                        sb.Append("<td>" + item.justificacion.ToString() + "</td>");
                        sb.Append("<td>" + item.usuario.ToString() + "</td>");
                        sb.Append("<td>" + item.fecha_actualizacion.ToString("yyyy/MM/dd HH:mm") + "</td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                    html = html.Replace("@TablaDetalle", sb.ToString());

                    Int32 _RolId = 0;
                    String asunto = "SOLICITUD DE AUTORIZACIÓN DE NUEVA PARTIDA";
                    switch (ultimo_estatus.EstatusId)
                    {
                        case 21:
                            _RolId = 1;
                            break;
                        case 22:
                            _RolId = 2;
                            break;
                        case 24:
                            _RolId = 3;
                            break;
                        case 26: //SOLO PARA NUEVAS PARTIDAS
                            _RolId = 4;
                            break;
                        case 28:
                            _RolId = 0;
                            asunto = "AUTORIZACIÓN DE NUEVAS PARTIDAS COMPLETADO";
                            break;
                        default:
                            _RolId = 0;
                            break;
                    }

                    string para = "";
                    string ccp = "";
                    foreach (WfcpUsuarioInRol uInrol in ctx.WfcpUsuarioInRol.Where(x => x.RolId == _RolId && x.activo).ToList()) para += uInrol.WfcpUsuario.correo + ";";

                    ctx.spSendMail_WfcpDocumento(para, ccp, asunto, html);
                    return true;
                }

            }
            catch (Exception ex)
            {
                alerta(ex.Message);
                return false;
            }
        }


        protected void btnValidarDocumento_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvDocumentosDetalle.Rows.Count <= 0)
                {
                    alerta("No hay partidas para validar.");
                    return;
                }
                bool mostrar_errores = false;
                Int32.TryParse(txtId.Text.Trim(), out Int32 _DocumentoId);
                WfcpDocumento documento = db.WfcpDocumento.Where(x => x.id == _DocumentoId).FirstOrDefault();
                if (documento != null)
                {
                    SAPConector30 sap = new SAPConector30();
                    List<ZMMTT_ME32L_SOL_POS> list = new List<ZMMTT_ME32L_SOL_POS>();
                    foreach (WfcpNuevaPartida item in db.WfcpNuevaPartida.Where(x => x.DocumentoId == _DocumentoId).ToList())
                    {
                        list.Add(new ZMMTT_ME32L_SOL_POS() { LIFNR = item.no_proveedor.ToString(), PURCHASINGDOCUMENT = item.documento_oc.ToString(), ITEM_CAT = item.WfcpTipoProcuracion.nombre, MATERIAL = item.no_parte, TARGET_QTY= item.target, PRICE = item.precio, BASE = 1000 });
                    }
                    Tuple<List<ET_RETURN>, List<ZMMTT_ME32L_DATA_PRICE>, List<ZMMTT_ME32L_DATA_POS>> tupla = sap.ZFM_SA_CREA_SOL("I", null, list);
                    if (tupla.Item1 != null && tupla.Item1.Where(x => x.TYPE == "E").Any())
                    {
                        mostrar_errores = true;
                        List<WfcpLog_ET_RETURN> log = new List<WfcpLog_ET_RETURN>();
                        List<ET_RETURN> errores = tupla.Item1.Where(x => x.TYPE == "E").ToList();
                        foreach (ET_RETURN error in errores)
                        {
                            log.Add(new WfcpLog_ET_RETURN() { DocumentoId = _DocumentoId, fecha_insert = DateTime.Now, IDENTIFICADOR = error.ID, MESSAGE = error.MESSAGE, NUMBER = error.NUMBER, TYPE = error.TYPE });
                        }
                        db.WfcpLog_ET_RETURN.AddRange(log);
                        db.SaveChanges();
                        documento.ZFM_SA_CREA_SOL = false;
                    }
                    else
                    {
                        foreach (ZMMTT_ME32L_DATA_POS precio_sap in tupla.Item3)
                        {
                            WfcpNuevaPartida precio_sql = db.WfcpNuevaPartida.Where(x => x.DocumentoId == _DocumentoId && (SqlFunctions.Replicate("0", 10 - x.no_proveedor.ToString().Length) + x.no_proveedor.ToString()) == precio_sap.LIFNR && x.documento_oc.ToString() == precio_sap.PURCHASINGDOCUMENT && x.no_parte == precio_sap.MATERIAL).FirstOrDefault();
                            if (precio_sql != null)
                            {
                                precio_sql.ITEM_NO = precio_sap.ITEM_NO;
                                precio_sql.LIFNR = precio_sap.LIFNR;
                                precio_sql.PSTYP = precio_sap.PSTYP;
                                precio_sql.KTMNG = precio_sap.KTMNG;
                                precio_sql.NETPR = precio_sap.NETPR;
                                precio_sql.PEINH = precio_sap.PEINH;
                                precio_sql.BSTAE = precio_sap.BSTAE;
                                precio_sql.ETFZ1 = precio_sap.ETFZ1;
                                precio_sql.ABUEB = precio_sap.ABUEB;
                                precio_sql.LOEKZ = precio_sap.LOEKZ;
                                precio_sql.fecha_validacion = DateTime.Now;
                                db.Entry(precio_sql).State = System.Data.Entity.EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        documento.ZFM_SA_CREA_SOL = true;
                    }
                }
                else
                {
                    alerta("No se pudo identificar el Documento");
                }
                db.Entry(documento).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                if (mostrar_errores)
                    btnVerLogValidaciones_Click(null, null);
                else
                {
                    WfcpLog_ET_RETURN log_exitoso = new WfcpLog_ET_RETURN() { DocumentoId = _DocumentoId, fecha_insert = DateTime.Now, IDENTIFICADOR = "0", MESSAGE = "Registros Validados Exitosamente", NUMBER = 0, TYPE = "S" };
                    db.Entry(log_exitoso).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    load_registros(-1, -1, 0);
                }
            }
            catch (Exception ex)
            {
                alerta(ex.Message);
            }
        }

        protected void btnVerLogValidaciones_Click(object sender, EventArgs e)
        {
            try
            {
                gvLogValidaciones.DataSource = db.WfcpLog_ET_RETURN.Where(x => x.DocumentoId.ToString() == txtId.Text.Trim().ToString()).OrderByDescending(o => o.id).ToList();
                gvLogValidaciones.DataBind();
                modalLog.Show();
            }
            catch (Exception ex)
            {
                alerta(ex.Message);
            }
        }

        protected void btnCancelarLog_Click(object sender, EventArgs e)
        {
            gvLogValidaciones.DataBind();
            modalLog.Hide();
        }
    }
}