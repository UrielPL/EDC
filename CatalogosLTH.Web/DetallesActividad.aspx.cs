using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class DetallesActividad : System.Web.UI.Page
    {
        public v_Actividad actividad { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            if(Request.QueryString["act"] != null)
            {
                string id = Request.QueryString["act"];

                actividad = new XPQuery<v_Actividad>(session).Where(x => x.Id.ToString() == id).First();
            }
        }
    }
}