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
    public partial class EvaluacionMovil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DevExpress.Xpo.Session session = Util.getsession();
               
                XPCollection<mdl_distribuidor> distribuidores = new XPCollection<mdl_distribuidor>(session);
                var lstdist = distribuidores.OrderBy(x => x.nombredist).ToList();
                foreach (var dist in lstdist)
                {
                    if (dist.UltimaAuditoria!=null)
                    {
                        cmbDist.Items.Add(dist.nombredist);
                    }
                }
                cmbDist.SelectedIndex = 0;               
            }
        }
        public void llenaCMB()
        {
           
        }
        protected void btnSig_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            
            mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == cmbDist.SelectedItem.Text);//dist seleccionado en cmb
            mdl_tipoauditoria tipo = dist.UltimaAuditoria.Idtipoaud;
            string userEvaluador = Util.getusuario();
            Usuario Evaluador = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == userEvaluador);
            List<mdl_AuditoriaMovil> lstAudMov = new List<mdl_AuditoriaMovil>();
            var areas = new XPQuery<mdl_Area>(session).Where(x => x.Idtipoaud == tipo);
            AuditoriaMovilGeneral amg = new AuditoriaMovilGeneral(session);
            amg.distribuidor = dist;
            amg.Evaluador = Evaluador;
            amg.Fecha = DateTime.Now;

            using (UnitOfWork uow = new UnitOfWork())
            {
                foreach (var item in areas)
                {
                    foreach (var p in item.Puntos)
                    {
                        mdl_AuditoriaMovil am = new mdl_AuditoriaMovil(session);
                        am.punto = p;
                        am.fecha = DateTime.Now;
                        am.evaluador = Evaluador;
                        am.distribuidor = dist;
                        am.auditoria = dist.UltimaAuditoria;
                        am.aceptada = false;//aceptada
                        am.status = false;//no aceptada
                        am.cerrada = false;
                        am.listaActividades.AddRange(p.Actividades.ToList());
                        amg.AudMDet.Add(am);
                    }
                }
                amg.Save();
                uow.CommitChanges();
            }


            int idAmg = amg.IdAudMovG;

            //using (UnitOfWork uow = new UnitOfWork())
            //{
            //    mdl_punto punto = new XPQuery<mdl_punto>(session).FirstOrDefault(x => x.texto == item.ToString() && x.Idtipoaud.Descripcion == cmbTipo.SelectedItem.Text);

            //    mdl_AuditoriaMovil audmov = new mdl_AuditoriaMovil(session);
            //    //mdl_auditoria auditoria = new XPQuery<mdl_auditoria>(session).FirstOrDefault(x => x.idaud.ToString() == idaud);
            //    //for (int i = 0; i < grdActividades.VisibleRowCount; i++)
            //    foreach (var acts in punto.Actividades)
            //    {
            //        audmov.listaActividades.Add(acts);
            //    }
            //    audmov.auditoria = ultAud;
            //    audmov.punto = punto;
            //    audmov.status = false;
            //    audmov.aceptada = false;
            //    audmov.cerrada = false;
            //    audmov.fecha = DateTime.Now;
            //    audmov.distribuidor = dist;
            //    //audmov.ArchivoImportar = dataF;
            //    audmov.evaluador = usuario;
            //    audmov.comentario = "Enviado en lista";
            //    audmov.Save();
            //    uow.CommitChanges();
            //}

            Response.Redirect("~/auditoria_movil.aspx?idamg=" + idAmg);

        }
        

        protected void cmbDist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
      
    }
}