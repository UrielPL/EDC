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
    public partial class HistorialTablaFinanciera : System.Web.UI.Page
    {
        public string permiso { get; set; }
        public string username { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            permiso = Util.getPermiso().ToString();
            username = Util.getusuario();

            Session["username"] = username;
            Session["permiso"] = permiso;
        }

        [WebMethod]
        public static string getTablas()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string nombreActual = (string)HttpContext.Current.Session["username"];
            string permiso = (string)HttpContext.Current.Session["permiso"];

            if (permiso == "GerenteCuenta" || permiso == "Evaluador")
            {
                var usuario = new XPQuery<Usuario>(session).Where(x => x.UserName == nombreActual).FirstOrDefault();
                var dists = usuario.Dependientes.ToList();

                List<TablaFinancieraResultados> tablas = new List<TablaFinancieraResultados>();
                foreach (var user in dists)
                {
                    var tbls = new XPQuery<TablaFinancieraResultados>(session).Where(x => (x.IdAuditoria.Fecha > DateTime.Parse("01/01/2024") || x.fechaRealizada > DateTime.Parse("01/01/2024")) && (x.distribuidor == user.UserName || x.distribuidorSel == user.UserName)).ToList();

                    //tbls = tbls.Where(x => (x.IdAuditoria != null && x.IdAuditoria.Distribuidor.nombredist == user.UserName && x.IdAuditoria.status == "COMPLETADO") || x.distribuidor == user.UserName).ToList();

                    if (tbls.Count() > 0)
                    {
                        foreach (var i in tbls)
                        {
                            tablas.Add(i);
                        }
                    }
                }


                tablas = tablas.OrderByDescending(x => x.fechaRealizada).ToList();

                List<tablafinancieradto> list = new List<tablafinancieradto>();
                if (tablas.Count() > 0)
                {
                    foreach (var item in tablas)
                    {
                        tablafinancieradto tf = new tablafinancieradto();

                        tf.ID = item.Id;
                        tf.Distribuidor = item.IdAuditoria != null && item.IdAuditoria.Distribuidor != null ? item.IdAuditoria.Distribuidor.nombredist : item.distribuidorSel != "" ? item.distribuidorSel : "";
                        tf.Autor = item.realizo != null ? item.realizo : "";
                        if (item.fechaRealizada != null && item.fechaRealizada != DateTime.MinValue)
                        {
                            tf.Fecha = item.fechaRealizada.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            tf.Fecha = "-";
                        }

                        if (item.IdAuditoria != null && item.IdAuditoria.autoAuditoria == true)
                        {
                            tf.AutoAuditoria = "SI";
                        }
                        else
                        {
                            tf.AutoAuditoria = "NO";
                        }

                        //tf.Estatus = item.completa == true ? "Completa" : "Incompleta";
                        //if(item.respuestaMUO != null && item.respuestaMUO != "" &&
                        //   item.respuestaMUB != null && item.respuestaMUB != "" &&
                        //   item.respuestaLRR != null && item.respuestaLRR != "" &&
                        //   item.respuestaLRC != null && item.respuestaLRC != "" &&
                        //   item.respuestaRI != null && item.respuestaRI != "" &&
                        //   item.respuestaPPC != null && item.respuestaPPC != "" &&
                        //   item.respuestaPPP != null && item.respuestaPPP != "" &&
                        //   item.respuestaE != null && item.respuestaE != "" &&
                        //   item.respuestaGO != null && item.respuestaGO != "" &&
                        //   item.respuestaRAO != null && item.respuestaRAO != "")
                        //{
                        //    item.completa = true;
                        //    item.Save();
                        //}

                        if(item.estatus != null && item.estatus != "")
                        {
                            tf.Estatus = item.estatus;
                        }else
                        {
                            tf.Estatus = item.completa == true ? "Completa" : "Incompleta";
                        }
                        

                        string pts = "";
                        if (item.completa == false)
                        {
                            pts += item.respuestaMUO == null || item.respuestaMUO == "" ? "Margen de utilidad operativa, " : "";
                            pts += item.respuestaMUB == null || item.respuestaMUB == "" ? "Margen de utilidad bruta, " : "";
                            pts += item.respuestaLRR == null || item.respuestaLRR == "" ? "Liquidez (razon rapida), " : "";
                            pts += item.respuestaLRC == null || item.respuestaLRC == "" ? "Liquidez (razon circulante), " : "";
                            pts += item.respuestaRI == null || item.respuestaRI == "" ? "Rotacion de inventario, " : "";
                            pts += item.respuestaPPC == null || item.respuestaPPC == "" ? "Periodo promedio de cobro, " : "";
                            pts += item.respuestaPPP == null || item.respuestaPPP == "" ? "Periodo promedio de pago, " : "";
                            pts += item.respuestaE == null || item.respuestaE == "" ? "Endeudamiento, " : "";
                            pts += item.respuestaGO == null || item.respuestaGO == "" ? "Gastos operativos, " : "";
                            pts += item.respuestaRAO == null || item.respuestaRAO == "" ? "Rendimiento sobre activos operativos, " : "";
                        }
                        tf.Resultados = pts;


                        list.Add(tf);
                    }

                    return JsonConvert.SerializeObject(list);
                }
            }

            if (permiso == "Admin")
            {
                var tables = new XPQuery<TablaFinancieraResultados>(session).Where(x => (x.IdAuditoria.Fecha > DateTime.Parse("01/01/2024") || x.fechaRealizada > DateTime.Parse("01/01/2024")) && x.distribuidor != "PRUEBA" && (x.realizo != "PRUEBA" && x.realizo != "admin" && x.realizo != "GERENTE PRUEBA")).ToList();

                tables = tables.OrderByDescending(x => x.fechaRealizada).ToList();

                List<tablafinancieradto> lista = new List<tablafinancieradto>();
                if (tables.Count() > 0)
                {
                    foreach (var item in tables)
                    {
                        tablafinancieradto tf = new tablafinancieradto();

                        tf.ID = item.Id;
                        tf.Distribuidor = item.IdAuditoria != null && item.IdAuditoria.Distribuidor != null ? item.IdAuditoria.Distribuidor.nombredist : item.distribuidorSel != "" ? item.distribuidorSel : "";
                        tf.Autor = item.realizo != null ? item.realizo : "";
                        if (item.fechaRealizada != null && item.fechaRealizada != DateTime.MinValue)
                        {
                            tf.Fecha = item.fechaRealizada.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            tf.Fecha = "-";
                        }

                        if (item.IdAuditoria != null && item.IdAuditoria.autoAuditoria == true)
                        {
                            tf.AutoAuditoria = "SI";
                        }
                        else
                        {
                            tf.AutoAuditoria = "NO";
                        }

                        //if (item.respuestaMUO != null && item.respuestaMUO != "" &&
                        //    item.respuestaMUB != null && item.respuestaMUB != "" &&
                        //    item.respuestaLRR != null && item.respuestaLRR != "" &&
                        //    item.respuestaLRC != null && item.respuestaLRC != "" &&
                        //    item.respuestaRI != null && item.respuestaRI != "" &&
                        //    item.respuestaPPC != null && item.respuestaPPC != "" &&
                        //    item.respuestaPPP != null && item.respuestaPPP != "" &&
                        //    item.respuestaE != null && item.respuestaE != "" &&
                        //    item.respuestaGO != null && item.respuestaGO != "" &&
                        //    item.respuestaRAO != null && item.respuestaRAO != "")
                        //{
                        //    item.completa = true;
                        //    item.Save();
                        //}

                        if (item.estatus != null && item.estatus != "")
                        {
                            tf.Estatus = item.estatus;
                        }
                        else
                        {
                            tf.Estatus = item.completa == true ? "Completa" : "Incompleta";
                        }

                        string pts = "";
                        if (item.completa == false)
                        {
                            pts += item.respuestaMUO == null || item.respuestaMUO == "" ? "Margen de utilidad operativa, " : "";
                            pts += item.respuestaMUB == null || item.respuestaMUB == "" ? "Margen de utilidad bruta, " : "";
                            pts += item.respuestaLRR == null || item.respuestaLRR == "" ? "Liquidez (razon rapida), " : "";
                            pts += item.respuestaLRC == null || item.respuestaLRC == "" ? "Liquidez (razon circulante), " : "";
                            pts += item.respuestaRI == null || item.respuestaRI == "" ? "Rotacion de inventario, " : "";
                            pts += item.respuestaPPC == null || item.respuestaPPC == "" ? "Periodo promedio de cobro, " : "";
                            pts += item.respuestaPPP == null || item.respuestaPPP == "" ? "Periodo promedio de pago, " : "";
                            pts += item.respuestaE == null || item.respuestaE == "" ? "Endeudamiento, " : "";
                            pts += item.respuestaGO == null || item.respuestaGO == "" ? "Gastos operativos, " : "";
                            pts += item.respuestaRAO == null || item.respuestaRAO == "" ? "Rendimiento sobre activos operativos, " : "";
                        }
                        tf.Resultados = pts;


                        lista.Add(tf);
                    }

                    return JsonConvert.SerializeObject(lista);
                }

            }

            return "-1";

        }




        [WebMethod]
        public static string getTablasDist()
        {
            DevExpress.Xpo.Session session = Util.getsession();

            string nombredist = (string)HttpContext.Current.Session["username"];
            var tablas = new XPQuery<TablaFinancieraResultados>(session).Where(x => (x.IdAuditoria.Distribuidor.nombredist == nombredist || x.realizo == nombredist) && (x.IdAuditoria.status == "COMPLETADO" || x.IdAuditoria == null)).ToList();

            List<tablafinancieradto> list = new List<tablafinancieradto>();
            if (tablas.Count() > 0)
            {
                foreach (var item in tablas)
                {
                    tablafinancieradto tf = new tablafinancieradto();

                    tf.ID = item.Id;
                    if (item.IdAuditoria != null)
                    {
                        tf.Distribuidor = item.IdAuditoria.Distribuidor.nombredist;

                        if (item.IdAuditoria.autoAuditoria == true)
                        {
                            tf.AutoAuditoria = "SI";
                        }
                        else
                        {
                            tf.AutoAuditoria = "NO";
                        }
                    }
                    else if (item.distribuidorSel != null)
                    {
                        tf.Distribuidor = item.distribuidorSel;
                    }
                    else { tf.Distribuidor = ""; }


                    tf.Autor = item.realizo;
                    if (item.fechaRealizada != null && item.fechaRealizada != DateTime.MinValue)
                    {
                        tf.Fecha = item.fechaRealizada.ToString("dd/MM/yyyy");
                    }
                    else { tf.Fecha = "-"; }

                    //if (item.respuestaMUO != null && item.respuestaMUO != "" &&
                    //        item.respuestaMUB != null && item.respuestaMUB != "" &&
                    //        item.respuestaLRR != null && item.respuestaLRR != "" &&
                    //        item.respuestaLRC != null && item.respuestaLRC != "" &&
                    //        item.respuestaRI != null && item.respuestaRI != "" &&
                    //        item.respuestaPPC != null && item.respuestaPPC != "" &&
                    //        item.respuestaPPP != null && item.respuestaPPP != "" &&
                    //        item.respuestaE != null && item.respuestaE != "" &&
                    //        item.respuestaGO != null && item.respuestaGO != "" &&
                    //        item.respuestaRAO != null && item.respuestaRAO != "")
                    //{
                    //    item.completa = true;
                    //    item.Save();
                    //}

                    if (item.estatus != null && item.estatus != "")
                    {
                        tf.Estatus = item.estatus;
                    }
                    else
                    {
                        tf.Estatus = item.completa == true ? "Completa" : "Incompleta";
                    }

                    string pts = "";
                    if (item.completa == false)
                    {
                        pts += item.respuestaMUO == null || item.respuestaMUO == "" ? "Margen de utilidad operativa, " : "";
                        pts += item.respuestaMUB == null || item.respuestaMUB == "" ? "Margen de utilidad bruta, " : "";
                        pts += item.respuestaLRR == null || item.respuestaLRR == "" ? "Liquidez (razon rapida), " : "";
                        pts += item.respuestaLRC == null || item.respuestaLRC == "" ? "Liquidez (razon circulante), " : "";
                        pts += item.respuestaRI == null || item.respuestaRI == "" ? "Rotacion de inventario, " : "";
                        pts += item.respuestaPPC == null || item.respuestaPPC == "" ? "Periodo promedio de cobro, " : "";
                        pts += item.respuestaPPP == null || item.respuestaPPP == "" ? "Periodo promedio de pago, " : "";
                        pts += item.respuestaE == null || item.respuestaE == "" ? "Endeudamiento, " : "";
                        pts += item.respuestaGO == null || item.respuestaGO == "" ? "Gastos operativos, " : "";
                        pts += item.respuestaRAO == null || item.respuestaRAO == "" ? "Rendimiento sobre activos operativos, " : "";
                    }
                    tf.Resultados = pts;

                    list.Add(tf);
                }

                return JsonConvert.SerializeObject(list);
            }

            return "-1";
        }
    }

    public class tablafinancieradto
    {
        public int ID { get; set; }
        public string Fecha { get; set; }
        public string Autor { get; set; }
        public string Distribuidor { get; set; }
        public string AutoAuditoria { get; set; }
        public string Estatus { get; set; }
        public string Resultados { get; set; }

    }
}