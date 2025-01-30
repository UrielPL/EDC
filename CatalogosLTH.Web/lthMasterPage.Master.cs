using CatalogosLTH.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class lthMasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //   DevExpress.Xpo.Session session = Util.getsession();
            if (!IsPostBack)
            {
                string nombre = Util.getusuario();
                ViewState["nombreUsuario"] = nombre;
                if (Util.getrol() == "Distribuidor")
                {
                    ViewState["rol"] = "Distribuidor";
                }
                else
                {
                    ViewState["rol"] = "Evaluador";
                }
                TipoUsuario permiso = Util.getPermiso();
                ViewState["permiso"] = permiso.ToString();
            }
            

        }
        protected void salir (object sender, EventArgs e)
        {
            DevExpress.ExpressApp.Web.WebApplication.LogOff(Session);

        }
    }
}