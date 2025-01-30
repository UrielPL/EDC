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
    public partial class Reporte4 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DevExpress.Xpo.Session session = Util.getsession();
                string nombreActual = Util.getusuario();//Nombre usuario 
                string permiso = Util.getPermiso().ToString();//Permiso usuario
                ViewState["permiso"] = permiso;
            }

            llenatabla();

                                                                                                                    
        }

        public void llenatabla()
        {

            DevExpress.Xpo.Session session = Util.getsession();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            double actN = 0;
            double actS = 0;
            double actC = 0;

            if ((month >= 10 && year == 2016) || (month < 10 && year == 2017))
            {
                var zonaN = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == "Norte" && x.orden != 1);
                var zonaS = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == "Sur" && x.orden != 1);
                var zonaC = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == "CAM" && x.orden != 1);
                actN = zonaN.Sum(x => x.terminadas);
                actS = zonaS.Sum(x => x.terminadas);
                actC = zonaC.Sum(x => x.terminadas);

            }
            else
            {
                if (month >= 10) { year++; }

                var zonaN = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == "Norte" && x.Periodo.Periodo == year);
                var zonaS = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == "Sur" && x.Periodo.Periodo == year);
                var zonaC = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == "CAM" && x.Periodo.Periodo == year);
                actN = zonaN.Sum(x => x.terminadas);
                actS = zonaS.Sum(x => x.terminadas);
                actC = zonaC.Sum(x => x.terminadas);

            }




            object[] vN = new object[2] { "Norte", actN };
            object[] vS = new object[2] { "Sur", actS };
            object[] vC = new object[2] { "CAM", actC };


            DataTable dt = new DataTable();
            dt.Columns.Add("Zona");
            dt.Columns.Add("Total de Actividades terminadas en el año");

            dt.Rows.Add(vN);
            dt.Rows.Add(vS);
            dt.Rows.Add(vC);

            ASPxGridView1.DataSource = dt;
            ASPxGridView1.DataBind();
        }
    }
}