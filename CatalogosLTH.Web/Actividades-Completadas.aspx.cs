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
    public partial class Actividades_Completadas : System.Web.UI.Page
    {
        public string rol { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            string permiso = Util.getPermiso().ToString();
            string nombreActual = Util.getusuario();

            Session["username"] = nombreActual;
            Session["permiso"] = permiso;
            rol = permiso;

            //if (permiso == "Distribuidor")
            //{
            //    Response.Redirect("mainpage.aspx");
            //}
        }


        [WebMethod]
        public static string getDistribuidores()
        {
            DevExpress.Xpo.Session session = Util.getsession();

            string username = (string)HttpContext.Current.Session["username"];
            string permiso = (string)HttpContext.Current.Session["permiso"];

            if (permiso == "GerenteCuenta" || permiso == "Evaluador")
            {
                var user = new XPQuery<Usuario>(session).Where(x => x.UserName == username).FirstOrDefault();
                var lst = user.Dependientes.ToList();
                if (lst.Count() > 0)
                {
                    List<string> distribuidores = new List<string>();
                    lst = lst.OrderBy(x => x.UserName).ToList();
                    foreach (var item in lst)
                    {
                        distribuidores.Add(item.UserName);
                    }

                    return JsonConvert.SerializeObject(distribuidores);
                }
            }
            else
            {
                if (permiso == "Admin")
                {
                    return "1";
                    //var dists = new XPQuery<mdl_distribuidor>(session).ToList();
                    //dists = dists.OrderBy(b => b.nombredist).ToList();
                    //List<string> distsnames = new List<string>();
                    //foreach(var item in dists)
                    //{
                    //    distsnames.Add(item.nombredist);
                    //}

                    //return JsonConvert.SerializeObject(distsnames);
                }
            }


            return "-1";

        }

        [WebMethod]
        public static string getActividades(string distname)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            if (distname != "todos")
            {
                var user = new XPQuery<Usuario>(session).Where(x => x.UserName == distname).FirstOrDefault();
                var auditorias = new XPQuery<NuevaAuditoria>(session).Where(x => x.Distribuidor.nombredist == user.UserName && x.status == "COMPLETADO").ToList();
                auditorias = auditorias.OrderByDescending(x => x.Id).ToList();

                var ultima_auditoria = auditorias.First();

                var listado = new XPQuery<NuevaAuditoriaActividad>(session).Where(x => x.Idaud.Id == ultima_auditoria.Id && x.status == "Completada")
                                                                                  .OrderByDescending(x => x.fechacomp)
                                                                                  .ToList();

                if (listado.Count() > 0)
                {
                    List<actividadobj> lst = new List<actividadobj>();

                    foreach (var item in listado)
                    {
                        actividadobj a = new actividadobj();
                        a.id = "Act-" + item.Idactividad.Punto.Id;
                        a.nombre = item.Idactividad.Nombre;
                        a.duracion = item.Idactividad.Vigencia.ToString();
                        a.fecha_inicio = item.fechainicio.ToString("dd/MM/yyyy");
                        a.fecha_completada = item.fechacomp != null && item.fechacomp != DateTime.MinValue ? item.fechacomp.ToString("dd/MM/yyyy") : "";
                        a.estatus = item.status;
                        a.evaluador = item.Evaluador != null ? item.Evaluador.Nombre : "";
                        a.distribuidor = distname;
                        a.usuario = item.Idaud.User_Apertura.Nombre;
                        a.idob = item.Oid;

                        lst.Add(a);
                    }

                    lst = lst.OrderByDescending(x => x.idob).ToList();
                    return JsonConvert.SerializeObject(lst);
                }

                return JsonConvert.SerializeObject(listado);
            }
            else
            {
                var listado = new XPQuery<NuevaAuditoriaActividad>(session).Where(x => x.status == "Completada" && x.Idaud.status != "CANCELADO" && x.Idaud.Distribuidor.nombredist != "PRUEBA")
                                                                            .OrderByDescending(x => x.fechacomp)
                                                                            .ToList();
                if (listado.Count() > 0)
                {
                    List<actividadobj> lst = new List<actividadobj>();

                    foreach (var item in listado)
                    {
                        actividadobj a = new actividadobj();
                        a.id = "Act-" + item.Idactividad.Punto.Id;
                        a.nombre = item.Idactividad.Nombre;
                        a.duracion = item.Idactividad.Vigencia.ToString();
                        a.fecha_inicio = item.fechainicio.ToString("dd/MM/yyyy");
                        a.fecha_completada = item.fechacomp != null && item.fechacomp != DateTime.MinValue ? item.fechacomp.ToString("dd/MM/yyyy") : "";
                        a.estatus = item.status;
                        a.evaluador = item.Evaluador != null ? item.Evaluador.Nombre : "";
                        a.distribuidor = item.Idaud.Distribuidor.nombredist;
                        a.usuario = item.Idaud.User_Apertura != null ? item.Idaud.User_Apertura.Nombre : "";
                        a.idob = item.Oid;

                        lst.Add(a);
                    }

                    lst = lst.OrderByDescending(x => x.idob).ToList();
                    return JsonConvert.SerializeObject(lst);
                }
            }

            return "-1";

        }

        [WebMethod]
        public static string getActs()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string permiso = Util.getPermiso().ToString();
            string nombreActual = Util.getusuario();

            if (permiso == "Admin")
            {
                var listado = new XPQuery<NuevaAuditoriaActividad>(session).Where(x => x.status == "Completada" && x.Idaud.status != "CANCELADO" && x.Idaud.Distribuidor.nombredist != "PRUEBA").OrderByDescending(x => x.fechacomp).ToList();
                if (listado.Count() > 0)
                {
                    List<actividadobj> lst = new List<actividadobj>();

                    foreach (var item in listado)
                    {
                        actividadobj a = new actividadobj();
                        a.id = "Act-" + item.Idactividad.Punto.Id;
                        a.nombre = item.Idactividad.Nombre;
                        a.duracion = item.Idactividad.Vigencia.ToString();
                        a.fecha_inicio = item.fechainicio.ToString("dd/MM/yyyy");
                        a.fecha_completada = item.fechacomp != null && item.fechacomp != DateTime.MinValue ? item.fechacomp.ToString("dd/MM/yyyy") : "";
                        a.estatus = item.status;
                        a.evaluador = item.Evaluador != null ? item.Evaluador.Nombre : "";
                        a.distribuidor = item.Idaud.Distribuidor.nombredist;
                        a.usuario = item.Idaud.User_Apertura != null ? item.Idaud.User_Apertura.Nombre : "";
                        a.idob = item.Oid;

                        lst.Add(a);
                    }

                    lst = lst.OrderByDescending(x => x.idob).ToList();
                    return JsonConvert.SerializeObject(lst);
                }
                else { return "-3"; }
            }
            else if (permiso == "GerenteCuenta" || permiso == "Evaluador")
            {
                var usuario = new XPQuery<Usuario>(session).Where(u => u.UserName == nombreActual).FirstOrDefault();
                var dependientes = usuario.Dependientes.Select(x => x.UserName);
                //var actspend = new XPQuery<NuevaAuditoriaActividad>(session).Where(x => x.Idactividad != null && x.status == "Completada" && x.Idaud.status != "CANCELADO" && dependientes.Contains(x.Idaud.Distribuidor.nombredist)).OrderByDescending(x => x.fechacomp).ToList();
                var actspend = new XPQuery<NuevaAuditoriaActividad>(session).Where(x => x.Idactividad != null && x.status == "Completada" && x.Idaud.status != "CANCELADO" && x.Evaluador.UserName == nombreActual).OrderByDescending(x => x.fechacomp).ToList();

                if (actspend.Count() > 0)
                {
                    List<actividadobj> lst = new List<actividadobj>();

                    foreach (var item in actspend)
                    {
                        if (item.Idactividad != null)
                        {
                            actividadobj a = new actividadobj();
                            a.id = "Act-" + item.Idactividad.Punto.Id;
                            a.nombre = item.Idactividad.Nombre;
                            a.duracion = item.Idactividad.Vigencia.ToString();
                            a.fecha_inicio = item.fechainicio.ToString("dd/MM/yyyy");
                            a.fecha_completada = item.fechacomp != null && item.fechacomp != DateTime.MinValue ? item.fechacomp.ToString("dd/MM/yyyy") : "";
                            a.estatus = item.status;
                            a.evaluador = item.Evaluador != null ? item.Evaluador.Nombre : "";
                            a.distribuidor = item.Idaud.Distribuidor.nombredist;
                            a.usuario = item.Idaud.User_Apertura != null ? item.Idaud.User_Apertura.Nombre : "";
                            a.idob = item.Oid;

                            lst.Add(a);
                        }
                        else { return "-2"; }

                    }

                    lst = lst.OrderByDescending(x => x.idob).ToList();
                    return JsonConvert.SerializeObject(lst);
                }
                else
                { return "-3"; }
            }
            else if (permiso == "Distribuidor")
            {
                string usuario = Util.getusuario();
                var actividades = new XPQuery<NuevaAuditoriaActividad>(session)
                                    .Where(x => x.Idactividad != null && x.status == "Completada" && x.Idaud.status != "CANCELADO" && x.Idaud.Distribuidor.nombredist == usuario)
                                    .ToList();
                if(actividades.Count() > 0)
                {
                    List<actividadobj> listado = new List<actividadobj>();
                    foreach(var item in actividades)
                    {
                        actividadobj a = new actividadobj();
                        a.id = "Act-" + item.Idactividad.Punto.Id;
                        a.nombre = item.Idactividad.Nombre;
                        a.duracion = item.Idactividad.Vigencia.ToString();
                        a.fecha_inicio = item.fechainicio.ToString("dd/MM/yyyy");
                        a.fecha_completada = item.fechacomp != null && item.fechacomp != DateTime.MinValue ? item.fechacomp.ToString("dd/MM/yyyy") : "";
                        a.estatus = item.status;
                        a.evaluador = item.Evaluador != null ? item.Evaluador.Nombre : "";
                        a.distribuidor = item.Idaud.Distribuidor.nombredist;
                        a.usuario = item.Idaud.User_Apertura != null ? item.Idaud.User_Apertura.Nombre : "";
                        a.idob = item.Oid;

                        listado.Add(a);
                    }

                    listado = listado.OrderByDescending(x => x.idob).ToList();
                    var json = JsonConvert.SerializeObject(listado);
                    return json;
                }
                else
                {
                    return "-3";
                }
            }
            else
            {
                return "-1";
            }
        }
    }
}