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
    public partial class AuditoriasNuevas : System.Web.UI.Page
    {
        //public static List<NuevaAuditoria> lstAuditorias { get; set; }
        //public static List<auditoriaDTO> audi { get; set; }
        public string rol { get; set; }
        public bool auto { get; set; }
        public bool incompleto { get; set; }
        public string nomactual { get; set; }
        //public auditoriaDTO[] audis { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            //DevExpress.Xpo.Session session = Util.getsession();

            //lstAuditorias = new XPCollection<NuevaAuditoria>(session).ToList();
            //audi = new List<auditoriaDTO>();
            //audis = new auditoriaDTO[lstAuditorias.Count()];

            //if (lstAuditorias.Count() > 0)
            //{
            //    foreach (var item in lstAuditorias)
            //    {
            //        auditoriaDTO oAuditoria = new auditoriaDTO();
            //        oAuditoria.ID = item.Id;
            //        if(item.Distribuidor != null)
            //        {
            //            oAuditoria.Distribuidor = item.Distribuidor.nombredist;
            //        }else
            //        {
            //            oAuditoria.Distribuidor = "-";
            //        }
            //        oAuditoria.Fecha = item.Fecha.ToShortDateString().ToString();
            //        if (item.User_Apertura != null)
            //        {
            //            oAuditoria.Usuario = item.User_Apertura.Nombre;
            //        }
            //        else
            //        {
            //            oAuditoria.Usuario = "-";
            //        }


            //        audi.Add(oAuditoria);
            //    }

            //    audis = audi.ToArray();
            //}
            incompleto = false;
            auto = false;
            if (Request.QueryString["a"] != null)
                auto = true;

            string permiso = Util.getPermiso().ToString();
            string nombreActual = Util.getusuario();
            Session["username"] = nombreActual;
            Session["permiso"] = permiso;
            nomactual = nombreActual;
            switch (permiso)
            {
               
                case "Distribuidor":
                    auto = true;
                    break;
                case "GerenteVenta":
                  
                    break;
                case "Admin":
                   
                    break;
                case "Evaluador":
                   
                    break;

                case "DCAdmin":
                   
                    break;
            }

            rol = permiso;

            checaPendiente();
        }

        [WebMethod]
        public static string jsonAuditorias()
        {
            DevExpress.Xpo.Session session = Util.getsession();

            string s = (string)HttpContext.Current.Session["username"];

            string p = (string)HttpContext.Current.Session["permiso"];

            List<NuevaAuditoria> lstAuditorias = new List<NuevaAuditoria>();

            if (p == "Distribuidor")
            {

                var auds = new XPQuery<NuevaAuditoria>(session).Where(x => x.Distribuidor.nombredist == s && x.autoAuditoria == true && x.status != "CANCELADO").ToList();

                var ultima = auds.OrderByDescending(x => x.Id).First();

                if (ultima.status == "INCOMPLETO")
                {
                    lstAuditorias = auds;

                }

                if(ultima.status == "COMPLETADO")
                {
                    lstAuditorias = auds.Where(x => x.status == "COMPLETADO").ToList();
                }

            }
            else if (p == "GerenteCuenta" || p == "Evalaudor")
            {
                var usuario = new XPQuery<Usuario>(session).Where(x => x.UserName == s).FirstOrDefault();
                var dependientes = usuario.Dependientes.Select(x => x.UserName).ToList();
                var auds = new XPQuery<NuevaAuditoria>(session).Where(x => x.status != "CANCELADO" && dependientes.Contains(x.Distribuidor.nombredist)).ToList();

                lstAuditorias = auds;

            }
            else
            {
                lstAuditorias = new XPQuery<NuevaAuditoria>(session).Where(x => x.Distribuidor.nombredist != "PRUEBA" && x.status != "CANCELADO").ToList();

            }

            List<auditoriaDTO> audi = new List<auditoriaDTO>();
            //audis = new auditoriaDTO[lstAuditorias.Count()];

            if (lstAuditorias.Count() > 0)
            {
                foreach (var item in lstAuditorias)
                {
                    auditoriaDTO oAuditoria = new auditoriaDTO();
                    oAuditoria.ID = item.Id;
                    if (item.Distribuidor != null)
                    {
                        oAuditoria.Distribuidor = item.Distribuidor.nombredist;
                    }
                    else
                    {
                        oAuditoria.Distribuidor = "-";
                    }
                    oAuditoria.Fecha = item.Fecha.ToString("dd/MM/yyyy");
                    oAuditoria.FechaCierre = item.Fecha_Cierre != null && item.Fecha_Cierre != DateTime.MinValue ? item.Fecha_Cierre.ToString("dd/MM/yyyy") : "Sin dato";
                    if (item.status != null)
                    {
                        oAuditoria.Estatus = item.status;
                    }
                    else
                    {
                        oAuditoria.Estatus = "-";
                    }

                    if(item.autoAuditoria == true)
                    {
                        oAuditoria.Autoauditoria = "Si";
                    }else
                    {
                        oAuditoria.Autoauditoria = "No";
                    }

                    if(item.User_Apertura != null)
                    {
                        oAuditoria.Autor = item.User_Apertura.Nombre;
                    }else
                    {
                        oAuditoria.Autor = "-";
                    }

                    audi.Add(oAuditoria);
                }
                return JsonConvert.SerializeObject(audi);
                
            }

            return JsonConvert.SerializeObject(audi);
        }

        public void checaPendiente()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            if (rol == "Distribuidor")
            {
               
                string s = (string)HttpContext.Current.Session["username"];

                string p = (string)HttpContext.Current.Session["permiso"];

                List<NuevaAuditoria> lstAuditorias = new List<NuevaAuditoria>();
                
                lstAuditorias = new XPQuery<NuevaAuditoria>(session).Where(x => x.Distribuidor.nombredist == s && x.autoAuditoria == true).ToList();

                if(lstAuditorias.Count() > 0)
                {
                    var ultimaId = lstAuditorias.Max(x => x.Id);
                    var ultima = new XPQuery<NuevaAuditoria>(session).Where(x => x.Id == ultimaId).FirstOrDefault();

                    if(ultima.status == "INCOMPLETO")
                    {
                        incompleto = true;
                    }else
                    {
                        if(ultima.status == "COMPLETADO")
                        {
                            incompleto = false;
                        }
                    }

                }

            }
        }
    }

    public class auditoriaDTO
    {
        public int ID { get; set; }
        public string Distribuidor { get; set; }
        public string Fecha { get; set; }
        public string FechaCierre { get; set; }
        public string Estatus { get; set; }
        public string Autoauditoria { get; set; }
        public string Autor { get; set; }
    }
}