using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class AvanceFinancieroGeneral : System.Web.UI.Page
    {
        public string avggMUO { get; set; }
        public string avggPPC { get; set; }
        public string avggE { get; set; }
        public string avggGO { get; set; }
        public string iframeUrl { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            string permiso = Util.getPermiso().ToString();
            string nombreActual = Util.getusuario();

            if (permiso == "Admin" || nombreActual == "rodolfoivan.giacoman@clarios.com")
            {
                string METABASE_SITE_URL = "http://25.55.174.57:3000";
                string METABASE_SECRET_KEY = "e1dcd64e8629514f3e5b188aa7edcd5af154993f670916ea29e5d9753e0d0d1e";

                var payload = new
                {
                    resource = new { dashboard = 2 },
                    @params = new { },
                    exp = Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds) + (10 * 60) // Expira en 10 minutos
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(METABASE_SECRET_KEY);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Claims = new Dictionary<string, object>
                    {
                        { "resource", payload.resource },
                        { "params", payload.@params },
                        { "exp", payload.exp }
                    },
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                string jwtToken = tokenHandler.WriteToken(token);

                iframeUrl = METABASE_SITE_URL + "/embed/dashboard/" + jwtToken + "#bordered=true&titled=true";

                Console.WriteLine("Iframe URL: " + iframeUrl);

                //List<double> avMUO = new List<double>();
                //List<double> avPPC = new List<double>();
                //List<double> avE = new List<double>();
                //List<double> avGO = new List<double>();
                //var fecha = DateTime.Parse("2024-01-01");

                ////var tablasG = new XPQuery<TablaFinancieraResultados>(session).Where(x => (x.IdAuditoria != null || x.IdAuditoria == null) && x.IdAuditoria.Distribuidor.nombredist != "PRUEBA" && x.completa == true && x.respuestaMUO != "" && x.respuestaPPC != "" && x.respuestaE != "" && x.respuestaGO != "" && x.realizo != "RIMSA").ToList();
                ////var tablasG = new XPQuery<TablaFinancieraResultados>(session)
                ////    .Where(x => (x.completa == true || x.estatus == "Aprobada") 
                ////    && x.respuestaMUO != "" 
                ////    && x.respuestaPPC != "" 
                ////    && x.respuestaE != "" 
                ////    && x.respuestaGO != "" 
                ////    &&  x.realizo != "RIMSA" 
                ////    && x.realizo != "admin" 
                ////    && x.realizo != "PRUEBA" 
                ////    && x.fechaRealizada > fecha).ToList();

                ////tablasG = tablasG.OrderBy(x => x.fechaRealizada).ToList();

                //var tablas = new XPQuery<TablaFinancieraResultados>(session)
                //    .Where(x => (x.respuestaE != "" && !x.respuestaE.StartsWith("0"))
                //    && (x.respuestaGO != "" && !x.respuestaGO.StartsWith("0"))
                //    && (x.respuestaMUO != "" && !x.respuestaMUO.StartsWith("0"))
                //    && (x.respuestaPPC != "" && !x.respuestaPPC.StartsWith("0"))
                //    && (x.estatus == "Aprobada" || x.completa == true)
                //    && (x.realizo != "PRUEBA" || x.realizo == null)
                //    && (x.realizo != "admin" || x.realizo == null)
                //    && (x.IdAuditoria.Distribuidor.nombredist != "PRUEBA" || x.IdAuditoria == null) 
                //    && x.fechaRealizada > fecha).ToList();


                //tablas = tablas.Where(x => x.realizo != "RIMSA").ToList();
                ////tablas = tablas.Where(x => double.Parse(x.respuestaE.Replace("%", "")) < 100.00 && double.Parse(x.respuestaGO.Replace("%", "")) < 100.00 && double.Parse(x.respuestaMUO.Replace("%", "")) < 100.00 && double.Parse(x.respuestaPPC.Replace("%", "")) < 100.00).ToList();

                //var agrupadasDist = tablas.GroupBy(x => x.distribuidor);
                //agrupadasDist = agrupadasDist.OrderBy(x => x.Key);


                //float sumMUO = 0;
                //float sumPPC = 0;
                //float sumEN = 0;
                //float sumGO = 0;

                //foreach (var a in agrupadasDist)
                //{
                //    foreach(var item in a)
                //    {
                //        var v = item.respuestaMUO != "NaN" ? float.Parse(item.respuestaMUO.Replace("%", "")) : 0;
                //        if (v > 0 && v < 100)
                //            sumMUO += v;

                //        var b = item.respuestaPPC != "NaN" ? float.Parse(item.respuestaPPC.Replace("%", "")) : 0;
                //        if (b > 0 && b < 100)
                //            sumPPC += b;

                //        var c = item.respuestaE != "NaN" ? float.Parse(item.respuestaE.Replace("%", "")) : 0;
                //        if (c > 0 && c < 100)
                //            sumEN += c;


                //        var d = item.respuestaGO != "NaN" ? float.Parse(item.respuestaGO.Replace("%", "")) : 0;
                //        if (d > 0 && d < 100)
                //            sumGO += d;
                //    }

                //}

                //var avgMUO = Math.Round(sumMUO / tablas.Count(), 2);
                //var avgPPC = Math.Round(sumPPC / tablas.Count(), 2);
                //var avgE = Math.Round(sumEN / tablas.Count(), 2);
                //var avgGO = Math.Round(sumGO / tablas.Count(), 2);

                //for (int i = 0; i < agrupadasDist.Count(); i++)
                //{
                //    avMUO.Add(avgMUO);
                //    avPPC.Add(avgPPC);
                //    avE.Add(avgE);
                //    avGO.Add(avgGO);
                //}

                //avggMUO = JsonConvert.SerializeObject(avMUO);
                //avggPPC = JsonConvert.SerializeObject(avPPC);
                //avggGO = JsonConvert.SerializeObject(avGO);
                //avggE = JsonConvert.SerializeObject(avE);
            }
            else
            {
                Response.Redirect("~/mainpage.aspx");
            }
        }

        [WebMethod]
        public static string traeListas()
        {
            DevExpress.Xpo.Session session = Util.getsession();

            var tablas = new XPQuery<TablaFinancieraResultados>(session).Where(x => (x.IdAuditoria != null || x.IdAuditoria == null) && x.completa == true || x.estatus == "Aprobada" && x.respuestaMUO != "" && x.respuestaPPC != "" && x.respuestaE != "" && x.respuestaGO != "").ToList();
            tablas = tablas.OrderBy(x => x.fechaRealizada).ToList();

            List<datos> listadoMUO = new List<datos>();
            List<datos> listadoPPC = new List<datos>();
            List<datos> listadoE = new List<datos>();
            List<datos> listadoGO = new List<datos>();

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
                }

                var resp = new
                {
                    muo = listadoMUO,
                    ppc = listadoPPC,
                    e = listadoE,
                    go = listadoGO
                };

                return JsonConvert.SerializeObject(resp);
            }

            return "-1";
        }

        [WebMethod]
        public static string promedios()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            //var tablas = new XPQuery<TablaFinancieraResultados>(session).Where(x => x.respuestaE != "" && x.respuestaGO != "" && x.respuestaMUO != "" && x.respuestaPPC != "" && x.completa == true).ToList();
            var fecha = DateTime.Parse("2024-01-01");

            var tablas = new XPQuery<TablaFinancieraResultados>(session)
                    .Where(x => (x.respuestaE != "" && !x.respuestaE.StartsWith("0"))
                    && (x.respuestaGO != "" && !x.respuestaGO.StartsWith("0"))
                    && (x.respuestaMUO != "" && !x.respuestaMUO.StartsWith("0"))
                    && (x.respuestaPPC != "" && !x.respuestaPPC.StartsWith("0"))
                    && (x.estatus == "Aprobada" || x.completa == true)
                    && (x.realizo != "PRUEBA" || x.realizo == null)
                    && (x.realizo != "admin" || x.realizo == null)
                    && (x.IdAuditoria.Distribuidor.nombredist != "PRUEBA" || x.IdAuditoria == null)
                    && x.fechaRealizada > fecha).ToList();

            tablas = tablas.Where(x => x.realizo != "RIMSA").ToList();
            
            var agrupadasDist = tablas.GroupBy(x => x.distribuidor);
            
            agrupadasDist = agrupadasDist.OrderBy(x => x.Key);
            List<promDist> lista = new List<promDist>();
            foreach (var item in agrupadasDist)
            {
                if (item.Key != "RIMSA" && item.Key != "PRUEBA" && item.Key != "")
                {
                    string distrivuidor = item.Key;
                    string promedio = "";
                    float aux = 0;

                    foreach (var x in item)
                    {
                        var r = x.respuestaMUO.Replace("%", "");
                        float parseo = float.Parse(r);
                        if(parseo > 0 && parseo < 100)
                            aux += parseo;
                    }

                    var avg = aux / item.Count();
                    promedio = Math.Round(avg, 2).ToString();
                    promDist obj = new promDist();
                    obj.dist = distrivuidor;
                    obj.prom = promedio;
                    obj.tipo = "muo";
                    obj.nombre = "Margen de utilidad operativa";
                    lista.Add(obj);

                    aux = 0;
                    promedio = "";

                    foreach (var x in item)
                    {
                        var r = x.respuestaPPC.Replace("%", "");
                        float parseo = float.Parse(r);
                        if(parseo > 0 && parseo < 100)
                            aux += parseo;
                    }

                    var avg2 = aux / item.Count();
                    promedio = Math.Round(avg2, 2).ToString();
                    promDist obj2 = new promDist();
                    obj2.dist = distrivuidor;
                    obj2.prom = promedio;
                    obj2.tipo = "ppc";
                    obj2.nombre = "Periodo promedio de cobro";
                    lista.Add(obj2);

                    aux = 0;
                    promedio = "";

                    foreach (var x in item)
                    {
                        var r = x.respuestaE.Replace("%", "");
                        float parseo = float.Parse(r);
                        if (parseo > 0 && parseo < 100)
                            aux += parseo;
                    }

                    var avg3 = aux / item.Count();
                    promedio = Math.Round(avg3, 2).ToString();
                    promDist obj3 = new promDist();
                    obj3.dist = distrivuidor;
                    obj3.prom = promedio;
                    obj3.tipo = "e";
                    obj3.nombre = "Endeudamiento";
                    lista.Add(obj3);

                    aux = 0;
                    promedio = "";

                    foreach (var x in item)
                    {
                        var r = x.respuestaGO.Replace("%", "");
                        float parseo = float.Parse(r);
                        if (parseo > 0 && parseo < 100)
                            aux += parseo;
                    }

                    var avg4 = aux / item.Count();
                    promedio = Math.Round(avg4, 2).ToString();
                    promDist obj4 = new promDist();
                    obj4.dist = distrivuidor;
                    obj4.prom = promedio;
                    obj4.tipo = "go";
                    obj4.nombre = "Gastos operativos";
                    lista.Add(obj4);
                }

            }

            var chartData = lista
                .GroupBy(d => d.tipo)
                .Select(g => new ChartData
                {
                    tipo = g.Key,
                    nombre = g.Select(d => d.nombre).ToList(),
                    Labels = g.Select(d => d.dist).ToList(),
                    Data = g.Select(d => d.prom).ToList()
                }).ToList();

            return JsonConvert.SerializeObject(chartData);
        }
    }

    public class promDist
    {
        public string dist { get; set; }
        public string prom { get; set; }
        public string tipo { get; set; }
        public string nombre { get; set; }
    }

    public class ChartData
    {
        public string tipo { get; set; }
        public List<string> nombre { get; set; }
        public List<string> Labels { get; set; }
        public List<string> Data { get; set; }
    }
}