using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class AvanceFinanciero : System.Web.UI.Page
    {
        //public List<datos> listado {get;set;}
        public string listaMUB { get; set; }
        public string listaMUO { get; set; }
        public string listaLRR { get; set; }
        public string listaLRC { get; set; }
        public string listaRI { get; set; }
        public string listaPPC { get; set; }
        public string listaPPP { get; set; }
        public string listaE { get; set; }
        public string listaGO { get; set; }
        public string listaRSAO { get; set; }
        public string rol { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string permiso = Util.getPermiso().ToString();
            string username = Util.getusuario();

            Session["username"] = username;
            Session["permiso"] = permiso;
            rol = permiso;

            List<datos> listadoMUB = new List<datos>();
            List<datos> listadoMUO = new List<datos>();
            List<datos> listadoLRR = new List<datos>();
            List<datos> listadoLRC = new List<datos>();
            List<datos> listadoRI = new List<datos>();
            List<datos> listadoPPC = new List<datos>();
            List<datos> listadoPPP = new List<datos>();
            List<datos> listadoE = new List<datos>();
            List<datos> listadoGO = new List<datos>();
            List<datos> listadoRSAO = new List<datos>();

            if (permiso == "Distribuidor")
            {
                var tablas = new XPQuery<TablaFinancieraResultados>(session).Where(x => (x.IdAuditoria != null || x.IdAuditoria == null) && x.completa == true && x.IdAuditoria.Distribuidor.nombredist == username && x.respuestaMUO != "" && x.respuestaPPC != "" && x.respuestaE != "" && x.respuestaGO != "").ToList();
                tablas = tablas.OrderBy(x => x.fechaRealizada).ToList();
                
                foreach (var item in tablas)
                {
                    datos mub = new datos();
                    mub.x = item.fechaRealizada.ToString("dd/MMMM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                    mub.y = item.respuestaMUB != "" ? double.Parse(item.respuestaMUB.Replace("%","")) : 0;
                    listadoMUB.Add(mub);

                    datos muo = new datos();
                    muo.x = item.fechaRealizada.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                    muo.y = item.respuestaMUO != "" ? double.Parse(item.respuestaMUO.Replace("%", "")) : 0;
                    listadoMUO.Add(muo);

                    datos lrr = new datos();
                    lrr.x = item.fechaRealizada.ToString("dd/MMMM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                    lrr.y = item.respuestaLRR != "" ? double.Parse(item.respuestaLRR.Replace("%", "")) : 0;
                    listadoLRR.Add(lrr);

                    datos lrc = new datos();
                    lrc.x = item.fechaRealizada.ToString("dd/MMMM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                    lrc.y = item.respuestaLRC != "" ? double.Parse(item.respuestaLRC.Replace("%", "")) : 0;
                    listadoLRC.Add(lrc);

                    datos ri = new datos();
                    ri.x = item.fechaRealizada.ToString("dd/MMMM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                    ri.y = item.respuestaRI != "" ? double.Parse(item.respuestaRI.Replace("%", "")) : 0;
                    listadoRI.Add(ri);

                    datos ppc = new datos();
                    ppc.x = item.fechaRealizada.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                    ppc.y = item.respuestaPPC != "" ? double.Parse(item.respuestaPPC.Replace("%", "")) : 0;
                    listadoPPC.Add(ppc);

                    datos ppp = new datos();
                    ppp.x = item.fechaRealizada.ToString("MMMM yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                    ppp.y = item.respuestaPPP != "" ? double.Parse(item.respuestaPPP.Replace("%", "")) : 0;
                    listadoPPP.Add(ppp);

                    datos en = new datos();
                    en.x = item.fechaRealizada.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                    en.y = item.respuestaE != "" ? double.Parse(item.respuestaE.Replace("%", "")) : 0;
                    listadoE.Add(en);

                    datos go = new datos();
                    go.x = item.fechaRealizada.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                    go.y = item.respuestaGO != "" ? double.Parse(item.respuestaGO.Replace("%", "")) : 0;
                    listadoGO.Add(go);

                    datos rsao = new datos();
                    rsao.x = item.fechaRealizada.ToString("MMMM yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                    rsao.y = item.respuestaRAO != "" ? double.Parse(item.respuestaRAO.Replace("%", "")) : 0;
                    listadoRSAO.Add(rsao);
                }

                listaMUB = JsonConvert.SerializeObject(listadoMUB);
                listaMUO = JsonConvert.SerializeObject(listadoMUO);
                listaLRR = JsonConvert.SerializeObject(listadoLRR);
                listaLRC = JsonConvert.SerializeObject(listadoLRC);
                listaRI = JsonConvert.SerializeObject(listadoRI);
                listaPPC = JsonConvert.SerializeObject(listadoPPC);
                listaPPP = JsonConvert.SerializeObject(listadoPPP);
                listaE = JsonConvert.SerializeObject(listadoE);
                listaGO = JsonConvert.SerializeObject(listadoGO);
                listaRSAO = JsonConvert.SerializeObject(listadoRSAO);
            }

        }

        [WebMethod]
        public static string traeDistribuidores()
        {
            DevExpress.Xpo.Session session = Util.getsession();

            string nombreActual = (string)HttpContext.Current.Session["username"];

            string permiso = (string)HttpContext.Current.Session["permiso"];

            List<Usuario> listaDistribuidores = new List<Usuario>();
            switch (permiso)
            {
                case "GerenteCuenta":
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName == nombreActual).ToList();
                    break;
                case "GerenteDesarrolloComercial":
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.Jefe.UserName == nombreActual).ToList();
                    //listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName == nombreActual).ToList();
                    break;
                case "Distribuidor":
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.UserName == nombreActual).ToList();
                    break;
                case "GerenteVenta":
                    Usuario u = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
                    string z = u.ZonaPertenece.zona;
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.ZonaPertenece.zona == z && x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
                    break;
                case "Admin":
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
                    break;
                case "Evaluador":
                    if (nombreActual == "alonso.sierra.siller@clarios.com")
                        listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
                    break;

                case "DCAdmin":
                    listaDistribuidores = new XPQuery<Usuario>(session)
                        .FirstOrDefault(x => x.UserName == nombreActual)
                        .DistribuidoresSupervisa.ToList();

                    break;
            }

            List<dtodistr> lstdistr = new List<dtodistr>();
            foreach (var item in listaDistribuidores)
            {
                dtodistr oDist = new dtodistr();
                oDist.id = item.Oid.ToString();
                oDist.nombre = item.UserName;

                lstdistr.Add(oDist);
            }

            lstdistr = lstdistr.OrderBy(x => x.nombre).ToList();

            return JsonConvert.SerializeObject(lstdistr);
        }

        [WebMethod]
        public static string traeListas(string dist)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            var tablas = new XPQuery<TablaFinancieraResultados>(session).Where(x => (x.IdAuditoria != null || x.IdAuditoria == null) && x.completa == true && x.IdAuditoria.Distribuidor.nombredist == dist && x.respuestaMUO != "" && x.respuestaPPC != "" && x.respuestaE != "" && x.respuestaGO != "").ToList();
            tablas = tablas.OrderBy(x => x.fechaRealizada).ToList();

            var tablasG = new XPQuery<TablaFinancieraResultados>(session).Where(x => (x.IdAuditoria != null || x.IdAuditoria == null) && x.IdAuditoria.Distribuidor.nombredist != "PRUEBA" && x.completa == true && x.respuestaMUO != "" && x.respuestaPPC != "" && x.respuestaE != "" && x.respuestaGO != "" && x.realizo != "RIMSA").ToList();
            tablasG = tablasG.OrderBy(x => x.fechaRealizada).ToList();

            List<datos> listadoMUO = new List<datos>();
            List<datos> listadoPPC = new List<datos>();
            List<datos> listadoE = new List<datos>();
            List<datos> listadoGO = new List<datos>();

            List<double> avgMUO = new List<double>();
            List<double> avgPPC = new List<double>();
            List<double> avgE = new List<double>();
            List<double> avgGO = new List<double>();

            if (tablas.Count() > 0)
            {
                foreach (var item in tablas)
                {
                    datos muo = new datos();
                    muo.x = item.fechaRealizada.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                    muo.y = item.respuestaMUO != "" ? double.Parse(item.respuestaMUO.Replace("%", "")) : 0;
                    listadoMUO.Add(muo);

                    datos ppc = new datos();
                    ppc.x = item.fechaRealizada.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                    ppc.y = item.respuestaPPC != "" ? double.Parse(item.respuestaPPC.Replace("%", "")) : 0;
                    listadoPPC.Add(ppc);

                    datos en = new datos();
                    en.x = item.fechaRealizada.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                    en.y = item.respuestaE != "" ? double.Parse(item.respuestaE.Replace("%", "")) : 0;
                    listadoE.Add(en);

                    datos go = new datos();
                    go.x = item.fechaRealizada.ToString("dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                    go.y = item.respuestaGO != "" ? double.Parse(item.respuestaGO.Replace("%", "")) : 0;
                    listadoGO.Add(go);

                    var sumMUO = tablasG.Sum(x => double.Parse(x.respuestaMUO.Replace("%", "")));
                    var sumPPC = tablasG.Sum(x => double.Parse(x.respuestaPPC.Replace("%", "")));
                    var sumEN = tablasG.Sum(x => double.Parse(x.respuestaE.Replace("%", "")));
                    var sumGO = tablasG.Sum(x => double.Parse(x.respuestaGO.Replace("%", "")));

                    var avgmuo = Math.Round(sumMUO / tablasG.Count(), 2);
                    avgMUO.Add(avgmuo);
                    var avgppc = Math.Round(sumPPC / tablasG.Count(), 2);
                    avgPPC.Add(avgppc);
                    var avgen = Math.Round(sumEN / tablasG.Count(), 2);
                    avgE.Add(avgen);
                    var avggo = Math.Round(sumGO / tablasG.Count(), 2);
                    avgGO.Add(avggo);
                }

               



                var resp = new
                {
                    muo = listadoMUO,
                    ppc = listadoPPC,
                    e = listadoE,
                    go = listadoGO,
                    avgMUO = avgMUO,
                    avgPPC = avgPPC,
                    avgEN = avgE,
                    avgGO = avgGO
                };

                return JsonConvert.SerializeObject(resp);
            }

            return "-1";
        }
    }

    public class datos
    {
        public string x { get; set; }
        public double y { get; set; }
    }
}