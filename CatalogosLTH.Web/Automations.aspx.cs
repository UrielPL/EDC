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
    public partial class Automations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            var resultado = new XPQuery<NuevaAuditoriaActividad>(session)
                .Where(naa => naa.Idaud != null
                              && naa.Idactividad != null
                              && naa.status == "En revisión"
                              && naa.Idaud.Distribuidor.iddistribuidor != 69
                              && naa.Evaluador.Nombre != "GERENTE PRUEBA"
                              )
                            .GroupBy(naa => new { naa.Evaluador.Nombre, naa.Evaluador.email })
                            .Select(g => new
                            {
                                Count = g.Count(),
                                Nombre = g.Key.Nombre,
                                Email = g.Key.email
                            })
                            .ToList();

            foreach(var item in resultado)
            {
                var cuerpo = Util.correoAvisaEvaluadorPendientes(item.Nombre, item.Count.ToString());
                List<string> lista = new List<string>();
                lista.Add(item.Email);
                Util.SendMail("", "Actividades por revisar", cuerpo, lista, "");

            }
        }
    }
}