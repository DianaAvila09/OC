using Root.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Root
{
    public partial class SiteNA : System.Web.UI.MasterPage
    {
        WFOCEntities db = new WFOCEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                WfcpUsuario usuario = db.WfcpUsuario.Where(x => x.usuario == HttpContext.Current.User.Identity.Name).FirstOrDefault();
                if (usuario != null && usuario.WfcpUsuarioInRol.Any())
                {
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    lblUsuario.Text = HttpContext.Current.User.Identity.Name;
                }
            }
        }
    }
}