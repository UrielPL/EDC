using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class EdicionObjetivosMinutas : System.Web.UI.Page
    {
        public static mdl_distribuidor dist { get; set; }
        public static mdl_DistribuidoresMinutas distMin { get; set; }
        public static DevExpress.Xpo.Session session { get; set; }
        public static string nombredist { get; set; }
        public static string FY { get; set; }
        public List<int[]> lstObjetivos = new List<int[]>();
        protected void Page_Load(object sender, EventArgs e)
        {
            session = Util.getsession();

            nombredist = Request.QueryString["distribuidor"];
            FY = Request.QueryString["FY"];

            var temp = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x=>x.nombredist == nombredist);
            if (temp != null)
                dist = temp;
            else
                distMin = new XPQuery<mdl_DistribuidoresMinutas>(session).FirstOrDefault(x=>x.nombredist == nombredist);

            string year = FY;
            string yearPrev = (int.Parse(year) - 1).ToString();
            string[] meses = { "Octubre-" + yearPrev, "Noviembre-" + yearPrev, "Diciembre-" + yearPrev, "Enero-" + year, "Febrero-" + year,
                                    "Marzo-" + year, "Abril-" + year, "Mayo-" + year, "Junio-" + year, "Julio-" + year, "Agosto-" + year,
                                    "Septiembre-" + year };

            int[] objTemp = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 12; i++)
            {
                mdl_Objetivos tempObj = new mdl_Objetivos(session);
                if (temp != null)
                {
                    tempObj = new XPQuery<mdl_Objetivos>(session).FirstOrDefault(x => x.MesesObjetivos.mes == meses[i] && x.Distribuidor.nombredist == dist.nombredist);
                }
                else
                {
                    tempObj = new XPQuery<mdl_Objetivos>(session).FirstOrDefault(x => x.MesesObjetivos.mes == meses[i] && x.DistribuidorMinutas.nombredist == distMin.nombredist);
                }
                if (tempObj != null)
                {
                    objTemp[i] = tempObj.cant;
                }
            }
            lstObjetivos.Add(objTemp);
        }

        [WebMethod]
        public static void GuardaObjetivos(string newObjetivos)
        {
            var values = newObjetivos.Split(',');
            string year = FY;
            string yearPrev = (int.Parse(year) - 1).ToString();

            string[] meses = { "Octubre-" + yearPrev, "Noviembre-" + yearPrev, "Diciembre-" + yearPrev, "Enero-" + year, "Febrero-" + year,
                                    "Marzo-" + year, "Abril-" + year, "Mayo-" + year, "Junio-" + year, "Julio-" + year, "Agosto-" + year,
                                    "Septiembre-" + year };

            for (int i = 0; i < 12; i++)
            {
                var temp = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == nombredist);
                mdl_Objetivos tempObj = new mdl_Objetivos(session);
                if (temp != null)
                {
                    tempObj = new XPQuery<mdl_Objetivos>(session).FirstOrDefault(x => x.MesesObjetivos.mes == meses[i] && x.Distribuidor.nombredist == dist.nombredist);
                }
                else
                {
                    tempObj = new XPQuery<mdl_Objetivos>(session).FirstOrDefault(x => x.MesesObjetivos.mes == meses[i] && x.DistribuidorMinutas.nombredist == distMin.nombredist);
                }
                if (tempObj != null)
                {
                    tempObj.cant = int.Parse(values[i]);
                    tempObj.Save();
                }
            }
        }
    }
}