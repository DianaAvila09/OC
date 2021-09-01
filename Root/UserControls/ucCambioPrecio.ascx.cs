using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Root.Model;

namespace Root.UserControls
{
    public partial class ucCambioPrecio : System.Web.UI.UserControl
    {
        public WfcpCambioPrecio cambio_precio
        {
            get {
                int.TryParse(hdnCambioPrecioId.Value, out int _id);
                long.TryParse(txtDocumentoOC.Text.Trim(), out long _documento_oc);
                float.TryParse(txtPrecio.Text.Trim(), out float _precio);
                DateTime.TryParse(txtFechaPuntoQuiebre.Text.Trim(), out DateTime _fecha_punto_quiebre);
                WfcpCambioPrecio cambio = new WfcpCambioPrecio()
                {
                    id = _id,
                    documento_oc = _documento_oc,
                    no_parte = txtNoParte.Text.Trim(),
                    precio = _precio,
                    MonedaId = int.Parse(ddlMoneda.SelectedValue),
                    fecha_punto_quiebre = _fecha_punto_quiebre,
                    justificacion = txtJustificacion.Text.Trim(),
                    usuario = HttpContext.Current.User.Identity.Name,
                    fecha_actualizacion = DateTime.Now
                };
                return cambio;
            }
            set {
                hdnCambioPrecioId.Value = value.id.ToString();
                txtDocumentoOC.Text = value.documento_oc.ToString();
                txtNoParte.Text = value.no_parte;
                txtPrecio.Text = value.precio.ToString();
                ddlMoneda.SelectedValue = value.MonedaId.ToString();
                txtFechaPuntoQuiebre.Text = value.fecha_punto_quiebre.ToString("yyyy/MM/dd HH:mm");
                txtJustificacion.Text = value.justificacion;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                using (WFOCEntities db = new WFOCEntities())
                {
                    ddlMoneda.Items.Clear();
                    foreach (WfcpMoneda moneda in db.WfcpMoneda)
                    {
                        ddlMoneda.Items.Add(new ListItem() { Value = moneda.id.ToString(), Text = moneda.nombre, Enabled = moneda.activo });
                    }
                }
            }
        }
    }
}