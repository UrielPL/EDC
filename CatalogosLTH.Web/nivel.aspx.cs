using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class nivel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            /*if (Util.getrol() == "Distribuidor")
            {
                Response.Redirect("NivelDistribuidor.aspx");
            }*/
            if (Util.getPermiso()!=TipoUsuario.Admin)
            {
                Response.Redirect("NivelDistribuidor.aspx");
            } 

            
            if (!Page.IsPostBack)
            {
                string nombreActual = Util.getusuario();

                XPCollection<mdl_distribuidor> dist = new XPCollection<mdl_distribuidor>(session);
                List<Usuario> listaDistribuidores = new List<Usuario>();

                string permiso = Util.getPermiso().ToString();
                //GerenteDesarrolloComercial,GerenteCuenta
                if (permiso== "GerenteCuenta")
                {
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName == nombreActual).ToList();
                }
                else if (permiso== "GerenteDesarrolloComercial")
                {
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.Jefe.UserName == nombreActual).ToList();
                }
                else if(permiso=="Admin")
                {
                    listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
                }
                // XPCollection<Usuario> listaDistribuidoresDependientes = new XPCollection<Usuario>(Util.getsession()).Where(x => x.Jefe.UserName.ToString() == Util.getusuario()).ToList();//n
                //if (Util.getPermiso()=="")
                //{
                  
                //}
                SortingCollection sortCollection = new SortingCollection();
                sortCollection.Add(new SortProperty("UserName", SortingDirection.Ascending));
                
                dist.Sorting = sortCollection;
                //LLENA DROPDOWNLIST DE DISTRIBUIDORES
                foreach (var itemDist in listaDistribuidores)
                {
                    dropDist.Items.Add(itemDist.UserName.ToString());
                }
                dropDist.SelectedIndex = 0;

                string IdAuditoria = Request.QueryString["IdAuditoria"];
                
                if (IdAuditoria==null||IdAuditoria=="")// Si el id de auditoria esta vacio, consigue el id del distribuidor seleccionado y se trae su ultima auditoria
                {
                    if (dropDist.SelectedItem!=null)
                    {
                        mdl_distribuidor distribuidorS = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == dropDist.SelectedItem.Value.ToString());
                        if (distribuidorS!=null){IdAuditoria = Util.getUltimaAuditoria(distribuidorS.iddistribuidor) + "";}
                    }                    
                }
                else
                {
                    IdAuditoria = Cryptography.Decripta(IdAuditoria);
                    int pos = 0;
                    int x = 0;
                    foreach (var item in dropDist.Items)
                    {
                        var a = new XPQuery<mdl_auditoria>(session).FirstOrDefault(w => w.idaud.ToString() == IdAuditoria.ToString());
                        var d = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(w => w.iddistribuidor.ToString() == a.Iddistribuidor.ToString());
                        if (item.ToString()==d.nombredist)
                        {
                            x = pos;
                        }
                        pos++;
                    }
                    dropDist.SelectedIndex = x;
                }
                ViewState["IdAuditoria"] = IdAuditoria;             
            }



        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            //query con vwactividad
            // XPQuery<vwactacertadas> auditorias = session.Query<vwactacertadas>();
            XPQuery<vwactividad> auditorias = session.Query<vwactividad>();
            XPQuery<mdl_actividad> actividades = session.Query<mdl_actividad>();

            string idaud = ViewState["IdAuditoria"] as string;
            int idaudInt = Convert.ToInt32(idaud);

            var sqlw = from a in auditorias
                       join c in actividades on a.IdActividad equals c.IdActividad
                       where a.idaud == idaudInt
                       orderby c.Secuencia ascending
                       select new { a.IdActividad, a.idaud, a.Code, a.secuencia, a.duracion, Actividad = c };
            //select new { a.IdActividad, a.idaud, a.Code, a.idpunto, c.Secuencia , c.Duracion,Actividad=c};



            mdl_auditoria audi = new XPQuery<mdl_auditoria>(session).FirstOrDefault(x => x.idaud.ToString() == idaud);
            if (audi.estatus == 0)
            {
                audi.estatus = 1;
                audi.fechaterm = DateTime.Now;
                audi.Save();
                DateTime fecha = DateTime.Now;
                DateTime fechafinal = DateTime.Now;
                foreach (var it in sqlw)
                {
                    int ist = it.IdActividad;
                    mdl_auditoriaactividad aa = new mdl_auditoriaactividad(session);
                    aa.Idaud = audi;
                    aa.Idactividad = it.Actividad;
                    aa.secuencia = it.secuencia;
                    aa.fechainicio = fecha;

                    aa.duracion = it.duracion;

                    fecha = fecha.AddDays(aa.duracion * 7);
                    aa.fechafinal = fecha;

                    aa.Save();
                    fecha = DateTime.Now;
                }
            }
            else
            {
                Response.Write("Auditoria ya cerrada");
            }

            ActualizarEncargados(audi.idaud, audi.Iddistribuidor.iddistribuidor);
            Util.ActualizarNivelProfesionalizacion(audi.Iddistribuidor.iddistribuidor);
            Response.Redirect("ListaActividades.aspx");

        }

        public void ActualizarEncargados(int idAud, int iddist)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            //XPQuery<mdl_auditoriaactividad> audacts =  session.Query<mdl_auditoriaactividad>();
            var audacts = new XPQuery<mdl_auditoriaactividad>(session).Where(c => c.Idaud.idaud == idAud);
            XPQuery<Usuario> UsuarioDist = session.Query<Usuario>();
            List<ReglaEvaluacion> ReglasDist = new XPQuery<ReglaEvaluacion>(session).Where(c => c.Distribuidor.iddistribuidor==iddist).ToList(); //TODAS LAS REGLAS CON EL DISTRIBUIDOR SELECCIONADO


            using (UnitOfWork varses = new UnitOfWork())
            {

                foreach (mdl_auditoriaactividad item in audacts)
                {
                   
                    mdl_actividad idactividad = item.Idactividad;
                    mdl_auditoria idauditoria = item.Idaud;//auditoria
                    Usuario usuarioEncargado = null;
                    string distribuidor = idauditoria.Iddistribuidor.nombredist;

                    foreach (var UsuarioD in UsuarioDist)
                    {
                        if (UsuarioD.UserName == distribuidor)
                        {
                            usuarioEncargado = UsuarioD;
                        }
                    }

                    var resultRegla = ReglasDist.Where(c => c.Actividad.IdActividad == item.Idactividad.IdActividad).ToList();
                    if (resultRegla.Count() > 0)//SI EXISTEN REGLAS PARA LA ACTIVIDAD N
                    {
                        item.Evaluador = resultRegla.FirstOrDefault().Evaluador;
                    }
                    else
                    {
                        MascaraPermisos permiso = idactividad.Permisos;
                        Usuario eval = null;

                        if (permiso == MascaraPermisos.Particular)
                        {
                            eval = idactividad.Encargado;
                        }
                        else if (permiso == MascaraPermisos.GerenteCuenta)
                        {
                            /*  if (idactividad.Encargado != null)
                              {
                                  eval = idactividad.Encargado.Jefe;
                              }*/

                            if (usuarioEncargado != null)
                            {
                                eval = usuarioEncargado.Jefe;
                            }
                        }
                        else if (permiso == MascaraPermisos.GerenteDesarrolloComercial)
                        {
                            /* if (idactividad.Encargado != null)
                             {
                                 eval = idactividad.Encargado.Jefe.Jefe;
                             }*/
                            if (usuarioEncargado != null)
                            {
                                if (usuarioEncargado.Jefe != null)
                                {
                                    eval = usuarioEncargado.Jefe.Jefe;
                                }
                            }
                        }

                        item.Evaluador = eval;
                    }
                    
                    item.Save();
                    //return eval;
                }
                varses.CommitChanges();
            }

        }


        protected void dropDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nombreActual = Util.getusuario();
            DevExpress.Xpo.Session session = Util.getsession();

            XPCollection<mdl_distribuidor> dist = new XPCollection<mdl_distribuidor>(session);
           // List<Usuario> listaDistribuidores = new List<Usuario>();
            // XPCollection<Usuario> listaDistribuidoresDependientes = new XPCollection<Usuario>(Util.getsession()).Where(x => x.Jefe.UserName.ToString() == Util.getusuario()).ToList();//n
         //   listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName == nombreActual).ToList();

           // SortingCollection sortCollection = new SortingCollection();
            //sortCollection.Add(new SortProperty("UserName", SortingDirection.Ascending));

            //dist.Sorting = sortCollection;
            //LLENA DROPDOWNLIST DE DISTRIBUIDORES
         //   foreach (var itemDist in listaDistribuidores)
          //  {
          //      dropDist.Items.Add(itemDist.UserName.ToString());
         //   }
         //   dropDist.SelectedIndex = 0;
          //  string IdAuditoria = Request.QueryString["IdAuditoria"];

           // if (IdAuditoria == null || IdAuditoria == "")
          //  {
                mdl_distribuidor distribuidorS = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == dropDist.SelectedItem.Value.ToString());
               string IdAuditoria = Util.getUltimaAuditoria(distribuidorS.iddistribuidor) + "";
          //  }
            ViewState["IdAuditoria"] = IdAuditoria;
        }
    }
}