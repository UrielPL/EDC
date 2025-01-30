using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Web;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class CatalogoActividades : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = getsession();


            var actividades = new XPQuery<mdl_actividad>(session).ToList();
            
            List<mdl_actividad> listaActividad = actividades;

            var data = from ba in listaActividad
                       orderby ba.IdActividad ascending
                       select new { IdActividad = ba.IdActividad, Code = ba.Code, Texto = ba.Texto,Pilar=ba.PilarID.nombrepil,Nivel=ba.NivelID.nombreniv};

           
            ASPxGridView1.DataSource = data.ToList();
            ASPxGridView1.AutoGenerateColumns = true;
            ASPxGridView1.KeyFieldName = "IdActividad";
            ASPxGridView1.DataBind();
            foreach (GridViewDataColumn columna in ASPxGridView1.Columns)
            {
                string col2 = columna.FieldName.ToString();
                if (col2 == "IdActividad")
                {
                    columna.Visible = false;
                }

            }
        }
        public DevExpress.Xpo.Session getsession()
        {
            /*DevExpress.Xpo.Session session = new DevExpress.Xpo.Session();
            session.ConnectionString = MySqlConnectionProvider.GetConnectionString("172.93.106.146", "lth", "output", "lth2");*/
            DevExpress.Xpo.Session session = new DevExpress.Xpo.Session();
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
            {
                session.ConnectionString = ConfigurationManager.
                   ConnectionStrings["ConnectionString"].ConnectionString;
            }

            return session;
        }

        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            int indice = ASPxGridView1.FocusedRowIndex;
            
            if (indice >= 0)
            {
                string idactividad = ASPxGridView1.GetRowValues(indice, new string[] { "IdActividad" }).ToString();

                Response.Redirect("~/DetalleActividad.aspx?idactividad=" + idactividad);
              
            }
        }
    }
}