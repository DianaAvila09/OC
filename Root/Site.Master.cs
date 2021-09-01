using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Root.Model;
namespace Root
{
    public partial class Site : System.Web.UI.MasterPage
    {
        WFOCEntities db = new WFOCEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            //lblUsuario.Text = HttpContext.Current.User.Identity.Name;
            if (!Page.IsPostBack)
            {
                string ctrlname = Page.Request.Params["__EVENTTARGET"];
                WfcpUsuario usuario = db.WfcpUsuario.Where(x => x.usuario == HttpContext.Current.User.Identity.Name).FirstOrDefault();
                if (usuario != null)
                {
                    lnkHistorico.Visible = true;
                    if (usuario.WfcpUsuarioInRol.Any())
                    {
                        if (HttpContext.Current.Request.Url.AbsoluteUri.ToUpper().Contains("AUTORIZACIONES.ASPX"))
                        {
                            lnkAutorizaciones.CssClass = "nav-link font-weight-bold";
                            lnkCambioPrecio.CssClass = "nav-link";
                            lnkNuevaPartida.CssClass = "nav-link";
                        }
                        else if (HttpContext.Current.Request.Url.AbsoluteUri.ToUpper().Contains("CAMBIOPRECIO.ASPX"))
                        {
                            lnkAutorizaciones.CssClass = "nav-link";
                            lnkCambioPrecio.CssClass = "nav-link font-weight-bold";
                            lnkNuevaPartida.CssClass = "nav-link";
                        }
                        else if (HttpContext.Current.Request.Url.AbsoluteUri.ToUpper().Contains("NUEVAPARTIDA.ASPX"))
                        {
                            lnkAutorizaciones.CssClass = "nav-link";
                            lnkCambioPrecio.CssClass = "nav-link";
                            lnkNuevaPartida.CssClass = "nav-link font-weight-bold";
                        }
                        else
                        {
                            lnkAutorizaciones.CssClass = "nav-link";
                            lnkCambioPrecio.CssClass = "nav-link";
                            lnkNuevaPartida.CssClass = "nav-link";
                        }

                        lblUsuario.Text = usuario.nombre;
                        foreach (WfcpUsuarioInRol uInrol in usuario.WfcpUsuarioInRol.Where(x => x.activo).ToList())
                        {
                            switch (uInrol.RolId)
                            {
                                case 0: //Comprador
                                    lnkCambioPrecio.Visible = true;
                                    lnkNuevaPartida.Visible = true;
                                    break;
                                //case 1://Compras
                                //    break;
                                //case 2://Finanzas
                                //    break;
                                //case 3://AGM
                                //    break;
                                //case 4://GM
                                //    break;
                                default:
                                    lnkAutorizaciones.Visible = true;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Response.Redirect("SinAcceso.aspx");
                    }
                }

            }
        }
    }
}