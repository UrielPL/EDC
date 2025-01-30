using CatalogosLTH.Module.BusinessObjects;
using DevExpress.ExpressApp;
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
    public partial class ListActividades : System.Web.UI.Page
    {
        public string permiso { get; set; }
        public string nombreActual { get; set; }
        public Usuario user { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            permiso = Util.getPermiso().ToString();
            nombreActual = Util.getusuario();
            user = (Usuario)SecuritySystem.CurrentUser;

        }

        [WebMethod]
        public static string traeNiveles()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            var lstNiveles = new XPQuery<mdl_nivel>(session).ToList();

            List<dtonivel> lstnivel = new List<dtonivel>();
            foreach(var item in lstNiveles)
            {
                dtonivel oNivel = new dtonivel();
                oNivel.idnivel = item.idnivel;
                oNivel.nombreniv = item.nombreniv;

                lstnivel.Add(oNivel);
            }
            
            return JsonConvert.SerializeObject(lstnivel);

        }

        [WebMethod]
        public static string traeActividades(string idDist, string idNivel)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            var user = (Usuario)SecuritySystem.CurrentUser;
            var nombreActual = Util.getusuario();
            var lstDTO = new List<dtoActividad>();
            if (user.TipoUsuario == TipoUsuario.Distribuidor)
            {

                mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist.ToString() == user.UserName.ToString());

                int auditoria = 0;
                if(nombreActual == "REPUESTOS DE ZACATECAS")
                {
                    var auditorias = new XPQuery<NuevaAuditoria>(session)
                                .Where(a => a.Distribuidor.iddistribuidor == dist.iddistribuidor && a.status == "COMPLETADO")
                                .OrderByDescending(a => a.Id)
                                .Select(a => a.Id)
                                .ToList();
                    if (auditorias.Count() > 0)
                        auditoria = auditorias[0];
                }
                else
                {
                    var auditorias = new XPQuery<NuevaAuditoria>(session)
                               .Where(a => a.Distribuidor.iddistribuidor == dist.iddistribuidor && a.status == "COMPLETADO" && a.autoAuditoria == false)
                               .OrderByDescending(a => a.Id)
                               .Select(a => a.Id)
                               .ToList();
                    if (auditorias.Count() > 0)
                        auditoria = auditorias[0];
                }

                //var auditorias = new XPQuery<NuevaAuditoria>(session)
                //                .Where(a => a.Distribuidor.iddistribuidor == dist.iddistribuidor && a.status == "COMPLETADO" && a.autoAuditoria == false)
                //                .OrderByDescending(a => a.Id)
                //                .Select(a => a.Id)
                //                .ToList();


                List<NuevaAuditoriaActividad> lstActividades = new List<NuevaAuditoriaActividad>();

                if(idNivel == "Todos")
                {
                    lstActividades = new XPQuery<NuevaAuditoriaActividad>(session).
                                    Where(c => c.Idactividad != null && c.Idactividad.Punto != null && c.Idaud.Id == auditoria && c.status != "COMPLETADA")
                                    .OrderByDescending(c => c.fechainicio)
                                    .ToList();
                }else
                {
                    lstActividades = new XPQuery<NuevaAuditoriaActividad>(session).Where(c => c.Idactividad != null && c.Idactividad.Punto != null && c.Idaud.Id == auditoria && c.Idactividad.NivelActividad.ToString() == idNivel && c.status != "COMPLETADA")
                                    .OrderByDescending(c => c.fechainicio)
                                    .ToList();
                }

                lstDTO = new List<dtoActividad>();
                foreach (var item in lstActividades)
                {
                    dtoActividad oAct = new dtoActividad();
                    oAct.ID = item.Oid;
                    oAct.Actividad = item.Idactividad.Nombre;
                    oAct.Duracion = item.duracion.ToString();
                    oAct.FechaFin = item.fechafinal.ToShortDateString();
                    oAct.FechaIn = item.fechainicio.ToShortDateString();
                    oAct.Estatus = item.status;
                    oAct.Nivel = item.Idactividad.NivelActividad.ToString();
                    oAct.Evaluador = item.Idactividad.Punto != null && item.Idactividad.Punto.Evaluador != null ? item.Idactividad.Punto.Evaluador.Nombre : item.Evaluador != null ? item.Evaluador.Nombre : "-";
                    oAct.Pilar = item.Idactividad.Punto.Subtema.Area.Pilar.Nombre;
                    oAct.Vigencia = item.vigencia;
                    oAct.idact = "Act-" + item.Idactividad.Punto.Id;
                    oAct.Distribuidor = item.Idaud.Distribuidor.nombredist;

                    lstDTO.Add(oAct);
                }

                return JsonConvert.SerializeObject(lstDTO);
            }
            else
            {
                Usuario usuario = new XPQuery<Usuario>(session).FirstOrDefault(x => x.Oid.ToString() == idDist);
                mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == usuario.UserName);

                int auditoria = 0;
                if (dist.nombredist == "REPUESTOS DE ZACATECAS")
                {
                    var auditorias = new XPQuery<NuevaAuditoria>(session)
                                .Where(a => a.Distribuidor.iddistribuidor == dist.iddistribuidor && a.status == "COMPLETADO")
                                .OrderByDescending(a => a.Id)
                                .Select(a => a.Id)
                                .ToList();
                    if (auditorias.Count() > 0)
                        auditoria = auditorias[0];
                }
                else
                {
                    var auditorias = new XPQuery<NuevaAuditoria>(session)
                               .Where(a => a.Distribuidor.iddistribuidor == dist.iddistribuidor && a.status == "COMPLETADO" && a.autoAuditoria == false)
                               .OrderByDescending(a => a.Id)
                               .Select(a => a.Id)
                               .ToList();
                    if (auditorias.Count() > 0)
                        auditoria = auditorias[0];
                }
                //var auditorias = new XPQuery<NuevaAuditoria>(session)
                //                .Where(a => a.Distribuidor.iddistribuidor == dist.iddistribuidor && a.status == "COMPLETADO" && a.autoAuditoria == false)
                //                .OrderByDescending(a => a.Id)
                //                .Select(a => a.Id)
                //                .ToList();
                //int auditoria = 0;

                //if (auditorias.Count() > 0)
                //auditoria = auditorias[0];

                List<NuevaAuditoriaActividad> lstActividades = new List<NuevaAuditoriaActividad>();
                
                if (idNivel == "Todos")
                {
                    lstActividades = new XPQuery<NuevaAuditoriaActividad>(session)
                                     .Where(c => c.Idactividad != null && c.Idactividad.Punto != null && c.Idaud.Id == auditoria && c.status != "Completada")
                                     .ToList();
                }
                else
                {
                  lstActividades = new XPQuery<NuevaAuditoriaActividad>(session).Where(c => c.Idactividad != null && c.Idactividad.Punto != null && c.Idaud.Id == auditoria && c.Idactividad.NivelActividad.ToString() == idNivel && c.status != "Completada").ToList();
                }
                
                lstDTO = new List<dtoActividad>();

                foreach (var item in lstActividades)
                {
                    dtoActividad oAct = new dtoActividad();
                    oAct.ID = item.Oid;
                    oAct.Actividad = item.Idactividad.Nombre;
                    oAct.Duracion = item.duracion.ToString();
                    oAct.FechaFin = item.fechafinal.ToShortDateString();
                    oAct.FechaIn = item.fechainicio.ToShortDateString();
                    oAct.Estatus = item.status;
                    oAct.Nivel = item.Idactividad.NivelActividad.ToString();
                    oAct.Evaluador = item.Idactividad.Punto != null && item.Idactividad.Punto.Evaluador != null ? item.Idactividad.Punto.Evaluador.Nombre : item.Evaluador != null ? item.Evaluador.Nombre : "-";
                    oAct.Pilar = item.Idactividad.Punto.Subtema.Area.Pilar.Nombre;
                    oAct.Vigencia = item.vigencia;
                    oAct.idact = "Act-" + item.Idactividad.Punto.Id;
                    oAct.Distribuidor = item.Idaud.Distribuidor.nombredist;

                    lstDTO.Add(oAct);
                }
            }

            return JsonConvert.SerializeObject(lstDTO);
        }

        [WebMethod]
        public static string traeDistribuidores()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string permiso = Util.getPermiso().ToString();
            string nombreActual = Util.getusuario();
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
            foreach(var item in listaDistribuidores)
            {
                dtodistr oDist = new dtodistr();
                oDist.id = item.Oid.ToString();
                oDist.nombre = item.UserName;

                lstdistr.Add(oDist);
            }

            lstdistr = lstdistr.OrderBy(x => x.nombre).ToList();

            return JsonConvert.SerializeObject(lstdistr);
        }
    }

    public class dtoActividad
    {
        public int ID { get; set; }
        public string Actividad { get; set; }
        public string Duracion { get; set; }
        public string FechaIn { get; set; }
        public string FechaFin { get; set; }
        public string Estatus { get; set; }
        public string Nivel { get; set; }
        public string Evaluador { get; set; }
        public string Pilar { get; set; }
        public string Vigencia { get; set; }
        public string idact { get; set; }
        public string Distribuidor { get; set; }
    }

    public class dtonivel
    {
        public int idnivel { get; set; }
        public string nombreniv { get; set; }
    }

    public class dtodistr
    {
        public string id { get; set; }
        public string nombre { get; set; }
    }
}