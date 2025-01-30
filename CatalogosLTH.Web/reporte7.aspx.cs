using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class reporte7 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string nombreActual = Util.getusuario();//Nombre usuario 
            string permiso = Util.getPermiso().ToString();//Permiso usuario
            Usuario u = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
            string z = "";
            ViewState["permiso"] = permiso;
            DataTable dt = new DataTable();
            if (permiso == "Distribuidor")
            {
                Response.Redirect("tablero1.aspx");
            }
            else if (permiso == "GerenteCuenta")
            {
                z = u.ZonaPertenece.zona;
                dt = llenaGridZona(z);
            }
            else
            {
                dt = llenaGridAdmin();  
            }
            
            ASPxGridView1.DataSource = dt;
            ASPxGridView1.DataBind();

        }

        public DataTable llenaGridAdmin()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            var distribuidores = new XPQuery<mdl_distribuidor>(session).Where(x=>x.EnDesarrollo==false);

            double promPySN = distribuidores.Where(x => x.zonastr == "Norte").Average(x => x.avanceProd);
            double promInfN = distribuidores.Where(x => x.zonastr == "Norte").Average(x => x.avanceInfra);
            double promAdN = distribuidores.Where(x => x.zonastr == "Norte").Average(x => x.avanceAdministracion);
            double promEjN = distribuidores.Where(x => x.zonastr == "Norte").Average(x => x.avanceEjecucion);
            double promPlN = distribuidores.Where(x => x.zonastr == "Norte").Average(x => x.avancePlaneacion);

            double promPySS = distribuidores.Where(x => x.zonastr == "Sur").Average(x => x.avanceProd);
            double promInfS = distribuidores.Where(x => x.zonastr == "Sur").Average(x => x.avanceInfra);
            double promAdS = distribuidores.Where(x => x.zonastr == "Sur").Average(x => x.avanceAdministracion);
            double promEjS = distribuidores.Where(x => x.zonastr == "Sur").Average(x => x.avanceEjecucion);
            double promPlS = distribuidores.Where(x => x.zonastr == "Sur").Average(x => x.avancePlaneacion);

            double promPySC = distribuidores.Where(x => x.zonastr == "CAM").Average(x => x.avanceProd);
            double promInfC = distribuidores.Where(x => x.zonastr == "CAM").Average(x => x.avanceInfra);
            double promAdC = distribuidores.Where(x => x.zonastr == "CAM").Average(x => x.avanceAdministracion);
            double promEjC = distribuidores.Where(x => x.zonastr == "CAM").Average(x => x.avanceEjecucion);
            double promPlC = distribuidores.Where(x => x.zonastr == "CAM").Average(x => x.avancePlaneacion);

            object[] vN = new object[6] { "Norte", promPySN.ToString("0.00"), promInfN.ToString("0.00"), promAdN.ToString("0.00"), promEjN.ToString("0.00"), promPlN.ToString("0.00") };
            object[] vS = new object[6] { "Sur", promPySS.ToString("0.00"), promInfS.ToString("0.00"), promAdS.ToString("0.00"), promEjS.ToString("0.00"), promPlS.ToString("0.00") };
            object[] vC = new object[6] { "CAM", promPySC.ToString("0.00"), promInfC.ToString("0.00"), promAdC.ToString("0.00"), promEjC.ToString("0.00"), promPlC.ToString("0.00") };


            DataTable dt = new DataTable();
            dt.Columns.Add("Zona");
            dt.Columns.Add("% AVANCE PRODUCTOS Y SERVICIOS");
            dt.Columns.Add("% AVANCE INFRAESTRUCTURA");
            dt.Columns.Add("% AVANCE ADMINISTRACIÓN");
            dt.Columns.Add("% AVANCE EJECUCIÓN");
            dt.Columns.Add("% AVANCE PLANEACIÓN");

            dt.Rows.Add(vN);
            dt.Rows.Add(vS);
            dt.Rows.Add(vC);

            //ASPxGridView1.DataSource = dt;
            return dt;
        }
        public DataTable llenaGridZona(string z)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            var distribuidores = new XPQuery<mdl_distribuidor>(session);

            double promPySN = distribuidores.Where(x => x.zonastr == z).Average(x => x.avanceProd);
            double promInfN = distribuidores.Where(x => x.zonastr == z).Average(x => x.avanceInfra);
            double promAdN = distribuidores.Where(x => x.zonastr == z).Average(x => x.avanceAdministracion);
            double promEjN = distribuidores.Where(x => x.zonastr == z).Average(x => x.avanceEjecucion);
            double promPlN = distribuidores.Where(x => x.zonastr == z).Average(x => x.avancePlaneacion);

            object[] vN = new object[6] { z, promPySN, promInfN, promAdN, promEjN, promPlN };

            DataTable dt = new DataTable();
            dt.Columns.Add("Zona");
            dt.Columns.Add("% AVANCE PRODUCTOS Y SERVICIOS");
            dt.Columns.Add("% AVANCE INFRAESTRUCTURA");
            dt.Columns.Add("% AVANCE ADMINISTRACIÓN");
            dt.Columns.Add("% AVANCE EJECUCIÓN");
            dt.Columns.Add("% AVANCE PLANEACIÓN");

            dt.Rows.Add(vN);
            
            return dt;
        }
    }
}