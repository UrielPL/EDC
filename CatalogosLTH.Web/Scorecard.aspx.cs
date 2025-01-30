using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class Scorecard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        [WebMethod]
        public static string getScore()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string permiso = Util.getPermiso().ToString();
            string nombreActual = Util.getusuario();
            List<scorecardDTO> lista = new List<scorecardDTO>();

            if (permiso == "Distribuidor")
            {
                var scoreActual = new XPQuery<Scorecards>(session).Where(x => x.distribuidor.nombredist == nombreActual).OrderByDescending(x => x.OID).FirstOrDefault();

                if (scoreActual != null)
                {
                    scorecardDTO obj = new scorecardDTO();
                    obj.distribuidor = nombreActual;
                    obj.fecha = scoreActual.fechaCarga.ToString("dd/MM/yyyy");
                    obj.sc_autoauditoria = scoreActual.sc_autoauditoria.ToString();
                    obj.sc_auditedc3 = scoreActual.sc_auditoriaedc3.ToString();
                    obj.sc_tablafinanciera = scoreActual.sc_tablafinanciera.ToString();
                    obj.sc_proyecto = scoreActual.sc_proyecto.ToString();
                    obj.sc_proyecto2 = scoreActual.sc_proyecto2.ToString();
                    obj.sc_proyecto3 = scoreActual.sc_proyecto3.ToString();
                    obj.scorecard = scoreActual.scorecard.ToString();
                    obj.puntajeFinal = scoreActual.puntaje_final.ToString();

                    lista.Add(obj);
                    var json = JsonConvert.SerializeObject(lista);

                    return json;
                }
            }
            else if (permiso == "GerenteCuenta")
            {
                var gerente = new XPQuery<Usuario>(session).Where(x => x.UserName == nombreActual).FirstOrDefault();
                var dependientes = gerente.Dependientes.ToList();

                
                foreach(var dep in dependientes)
                {
                    var scoreActual = new XPQuery<Scorecards>(session).Where(x => x.distribuidor.nombredist == dep.UserName)
                                                                        .OrderByDescending(x => x.OID)
                                                                        .FirstOrDefault();

                    if (scoreActual != null)
                    {
                        scorecardDTO obj = new scorecardDTO();
                        obj.distribuidor = scoreActual.distribuidor.nombredist;
                        obj.fecha = scoreActual.fechaCarga.ToString("dd/MM/yyyy");
                        obj.sc_autoauditoria = scoreActual.sc_autoauditoria.ToString();
                        obj.sc_auditedc3 = scoreActual.sc_auditoriaedc3.ToString();
                        obj.sc_tablafinanciera = scoreActual.sc_tablafinanciera.ToString();
                        obj.sc_proyecto = scoreActual.sc_proyecto.ToString();
                        obj.sc_proyecto2 = scoreActual.sc_proyecto2.ToString();
                        obj.sc_proyecto3 = scoreActual.sc_proyecto3.ToString();
                        obj.scorecard = scoreActual.scorecard.ToString();
                        obj.puntajeFinal = scoreActual.puntaje_final.ToString();

                        lista.Add(obj);
                    }
                }

                var json = JsonConvert.SerializeObject(lista);

                return json;
            }
            else if (permiso == "Admin")
            {
                var scores = new XPQuery<Scorecards>(session).ToList();

                if (scores != null)
                {
                    foreach(var scoreActual in scores)
                    {
                        scorecardDTO obj = new scorecardDTO();
                        obj.distribuidor = scoreActual.distribuidor.nombredist;
                        obj.fecha = scoreActual.fechaCarga.ToString("dd/MM/yyyy");
                        obj.sc_autoauditoria = scoreActual.sc_autoauditoria.ToString();
                        obj.sc_auditedc3 = scoreActual.sc_auditoriaedc3.ToString();
                        obj.sc_tablafinanciera = scoreActual.sc_tablafinanciera.ToString();
                        obj.sc_proyecto = scoreActual.sc_proyecto.ToString();
                        obj.sc_proyecto2 = scoreActual.sc_proyecto2.ToString();
                        obj.sc_proyecto3 = scoreActual.sc_proyecto3.ToString();
                        obj.scorecard = scoreActual.scorecard.ToString();
                        obj.puntajeFinal = scoreActual.puntaje_final.ToString();

                        lista.Add(obj);
                        
                    }

                    var json = JsonConvert.SerializeObject(lista);
                    return json;
                }
            }

            return JsonConvert.SerializeObject(lista);
        }
    }

    public class scorecardDTO
    {
        public string distribuidor { get; set; }

        public string fecha { get; set; }
        public string sc_autoauditoria { get; set; }
        public string sc_auditedc3 { get; set; }
        public string sc_tablafinanciera { get; set; }
        public string sc_proyecto { get; set; }
        public string sc_proyecto2 { get; set; }
        public string sc_proyecto3 { get; set; }
        public string scorecard { get; set; }
        public string puntajeFinal { get; set; }
    }
}