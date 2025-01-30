using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace CatalogosLTH.Web
{
    public partial class HistorialAuditorias : System.Web.UI.Page
    {
        public static string rol { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            string permiso = Util.getPermiso().ToString();
            string nombreActual = Util.getusuario();
            Session["username"] = nombreActual;
            Session["permiso"] = permiso;

            switch (permiso)
            {

                case "Distribuidor":

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
        }

        [WebMethod]
        public static string traeAuditorias()
        {
            DevExpress.Xpo.Session session = Util.getsession();

            string s = (string)HttpContext.Current.Session["username"];
            string p = (string)HttpContext.Current.Session["permiso"];

            if (p == "Distribuidor")
            {
                var lista = new XPQuery<NuevaAuditoria>(session).Where(x => x.Distribuidor.nombredist == s && x.status == "COMPLETADO").ToList();

                if (lista.Count > 0)
                {
                    List<aud_row> lsttemp = new List<aud_row>();
                    foreach (var item in lista)
                    {
                        aud_row obj = new aud_row();
                        obj.ID = item.Id.ToString();
                        obj.Distribuidor = item.Distribuidor.nombredist;
                        obj.Estatus = item.status;
                        obj.Fecha = item.Fecha.ToString("dd/MM/yyyy");
                        obj.CalificacionF = item.calificacionTotal.ToString();
                        obj.CalificacionD = item.calificacionFinal.ToString();
                        obj.Autor = item.User_Apertura != null ? item.User_Apertura.UserName : "";
                        obj.Fecha_Cierre = item.Fecha_Cierre > DateTime.MinValue ? item.Fecha_Cierre.ToString("dd/MM/yyyy") : "";
                        if (item.autoAuditoria)
                        {
                            obj.Formato = "Autoauditoria";
                        }
                        else
                        {
                            obj.Formato = "Auditoria";
                        }

                        lsttemp.Add(obj);
                    }

                    return JsonConvert.SerializeObject(lsttemp);
                }
            }
            else if (p == "GerenteCuenta" || p == "Evaluador")
            {
                var usuario = new XPQuery<Usuario>(session).Where(x => x.UserName == s).FirstOrDefault();

                if (usuario != null)
                {
                    var dependientes = usuario.Dependientes.ToList();

                    if (dependientes.Count() > 0)
                    {
                        List<aud_row> lstAud = new List<aud_row>();
                        foreach (var dep in dependientes)
                        {
                            var audis = new XPQuery<NuevaAuditoria>(session).Where(x => x.Distribuidor.nombredist == dep.UserName && x.status == "COMPLETADO").ToList();
                            if (audis.Count() > 0)
                            {
                                foreach (var a in audis)
                                {
                                    aud_row obj = new aud_row();
                                    obj.ID = a.Id.ToString();
                                    obj.Distribuidor = a.Distribuidor.nombredist;
                                    obj.Estatus = a.status;
                                    obj.Fecha = a.Fecha.ToString("dd/MM/yyyy");
                                    obj.CalificacionF = a.calificacionTotal.ToString();
                                    obj.CalificacionD = a.calificacionFinal.ToString();
                                    obj.Autor = a.User_Apertura != null ? a.User_Apertura.UserName : "";
                                    obj.Fecha_Cierre = a.Fecha_Cierre > DateTime.MinValue ? a.Fecha_Cierre.ToString("dd/MM/yyyy") : "";
                                    if (a.autoAuditoria)
                                    {
                                        obj.Formato = "Autoauditoria";
                                    }
                                    else
                                    {
                                        obj.Formato = "Auditoria";
                                    }

                                    lstAud.Add(obj);
                                }
                            }

                        }

                        return JsonConvert.SerializeObject(lstAud);
                    }
                }
            }
            else
            {
                var audis = new XPQuery<NuevaAuditoria>(session).Where(x => x.status == "COMPLETADO").ToList();
                audis = audis.OrderByDescending(x => x.Fecha).ToList();
                List<aud_row> lstAud = new List<aud_row>();

                foreach (var a in audis)
                {

                    aud_row obj = new aud_row();
                    obj.ID = a.Id.ToString();
                    obj.Distribuidor = a.Distribuidor.nombredist;
                    obj.Estatus = a.status;
                    obj.Fecha = a.Fecha.ToString("dd/MM/yyyy");
                    obj.CalificacionF = a.calificacionTotal.ToString();
                    obj.CalificacionD = a.calificacionFinal.ToString();
                    obj.Autor = a.User_Apertura != null ? a.User_Apertura.UserName : "";
                    obj.Fecha_Cierre = a.Fecha_Cierre > DateTime.MinValue ? a.Fecha_Cierre.ToString("dd/MM/yyyy") : "";
                    if (a.autoAuditoria)
                    {
                        obj.Formato = "Autoauditoria";
                    }
                    else
                    {
                        obj.Formato = "Auditoria";
                    }

                    lstAud.Add(obj);

                }

                return JsonConvert.SerializeObject(lstAud);

            }


            return "-1";
        }
    }

    public class aud_row
    {
        public string ID { get; set; }
        public string Distribuidor { get; set; }
        public string Fecha { get; set; }
        public string Fecha_Cierre { get; set; }
        public string Estatus { get; set; }
        public string CalificacionF { get; set; }
        public string CalificacionD { get; set; }
        public string Formato { get; set; }
        public string Autor { get; set; }
    }
}