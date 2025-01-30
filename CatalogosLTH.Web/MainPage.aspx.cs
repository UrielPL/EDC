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
    public partial class MainPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            DevExpress.Xpo.Session session = Util.getsession();
            string permiso = Util.getPermiso().ToString();//Permiso usuario
            ViewState["permiso"] = permiso;

            string idsesion = (string)HttpContext.Current.Session["sessionID"];

            if(idsesion == null || idsesion == "")
            {
                Session["sessionID"] = Session.SessionID;
            }
            else if(idsesion != Session.SessionID.ToString())
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "err_msg", "alert('Sesion caduca. Vuelva a iniciar sesion.')", true);
            }



            //UPDATE DE ESTATUS AUDITORIAS
            //var fecha = DateTime.Now.AddMonths(-1);
            //var auditorias = new XPQuery<NuevaAuditoria>(session).Where(x => x.status == "INCOMPLETO" && x.Fecha <= fecha).ToList();

            //if(auditorias.Count() > 0)
            //{
            //   foreach(var item in auditorias)
            //    {
            //        item.status = "CANCELADO";
            //        item.Save();
            //    }
            //}
            //var fecha = DateTime.Parse("2024-01-01");

            //var puntostodos = new XPQuery<NuevaAuditoriaDetalle>(session).Where(x => x.Resultado == false && x.Auditoria.Distribuidor.nombredist != "PRUEBA" && x.Auditoria.Fecha >= fecha && x.Auditoria.status == "COMPLETADO" && x.Punto.Subtema.Area.Pilar.Nombre == "Ejecución").ToList();

            

            //var nuevaAcitividad = new XPQuery<NuevaAuditoriaActividad>(session).Where(x => x.Idactividad != null && x.Idaud != null && x.Idactividad.Punto != null).ToList();
            //foreach(var item in nuevaAcitividad)
            //{
            //    if (item.Idactividad.Punto.Evaluador != null)
            //    {
            //        item.Evaluador = item.Idactividad.Punto.Evaluador;
            //        item.Save();
            //    }
            //    else
            //    {
                    
            //        if ( item.Idaud.Distribuidor != null)
            //        {
            //            var dist = item.Idaud.Distribuidor;
            //            var user = new XPQuery<Usuario>(session).Where(x => x.UserName == dist.nombredist).FirstOrDefault();

            //            if (user != null && user.Jefe != null)
            //            {
            //                item.Evaluador = user.Jefe;

            //            }
            //            else
            //            {
            //                item.Evaluador = null;
            //            }

            //        }
            //        else
            //        {
            //            item.Evaluador = null;
            //        }

            //        item.Save();
            //    }
            //}

        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Nivel.aspx");
        }

        protected void ImgPlan_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/ListaActividades.aspx");
        }

        protected void ImgCompletas_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/ActividadesCompletadas.aspx");
        }
    }
}