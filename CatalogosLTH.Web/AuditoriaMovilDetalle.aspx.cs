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
    public partial class AuditoriaMovilDetalle : System.Web.UI.Page
    {
        public List<CentroServicio> CSDist = new List<CentroServicio>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DevExpress.Xpo.Session session = Util.getsession();
                var codigo = Request.QueryString["codigo"];
                String grp = Request.Form["idAud"];

                string nombreActual = Util.getusuario();//Nombre usuario 
                mdl_audidet auddet = new mdl_audidet(session);
                //mdl_punto punto = new mdl_punto(session);
                List<mdl_actividad> actividades = new List<mdl_actividad>();

                if (codigo != null)
                {
                    codigo = Cryptography.Decripta(codigo.ToString());
                }
                if (codigo != null)
                {
                    auddet = new XPQuery<mdl_audidet>(session).FirstOrDefault(x => x.id.ToString() == codigo);
                    ViewState["idauditoria"] = auddet.Idaud.idaud.ToString();
                    ViewState["idpunto"] = auddet.Idpunto.idpunto.ToString();

                    //punto = new XPQuery<mdl_punto>(sessi7on).FirstOrDefault(x => x == auddet.Idpunto);
                    actividades = auddet.Idpunto.Actividades.ToList();

                    lblArea.Text = auddet.Idpunto.Areas.FirstOrDefault().Nombre;
                    lblDist.Text = auddet.Idaud.Iddistribuidor.nombredist;
                    lblEvaluador.Text = nombreActual;
                    lblPunto.Text = auddet.Idpunto.texto;
                    lblTipo.Text = auddet.Idaud.Idtipoaud.Descripcion;

                    CSDist = new XPQuery<CentroServicio>(session).Where(x => x.Iddistribuidor.iddistribuidor == auddet.Idaud.Iddistribuidor.iddistribuidor).ToList();

                    foreach (var item in CSDist)
                    {
                        cmbCS.Items.Add(item.Nombre, item.Nombre);
                    }

                    Session["cmbArea"] = lblArea.Text;
                }
                var acts = from act in actividades
                           select new { Actividad = act.Code, Descripción = act.Texto, Nivel = act.NivelID.nombreniv, Pilar = act.PilarID.nombrepil };
                grdActividades.DataSource = acts;
                grdActividades.AutoGenerateColumns = true;
                grdActividades.KeyFieldName = "IdActividad";
                grdActividades.EnableRowsCache = true;
                grdActividades.DataBind();
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Session session = Util.getsession();
            //traer auditoria general
            string idaud = ViewState["idauditoria"] as string;
            string idpunto = ViewState["idpunto"] as string;
            string codAMG = Session["amg"] as string;

            AuditoriaMovilGeneral amg = new XPQuery<AuditoriaMovilGeneral>(session).FirstOrDefault(x => x.IdAudMovG.ToString() == codAMG);

            mdl_AuditoriaMovil audmov = amg.AudMDet.FirstOrDefault(x => x.punto.idpunto.ToString() == idpunto && x.auditoria.idaud.ToString() == idaud);

                       

            DevExpress.Persistent.BaseImpl.FileData dataF = null;
            if (FileUpload1.HasFile)
            {
                var fileStream = FileUpload1.FileContent;
                dataF = new DevExpress.Persistent.BaseImpl.FileData(session);
                dataF.LoadFromStream(FileUpload1.FileName, fileStream);
            }

            using (UnitOfWork uow = new UnitOfWork())
            {
                //mdl_AuditoriaMovil audmov = new mdl_AuditoriaMovil(session);
                //mdl_punto punto = new XPQuery<mdl_punto>(session).FirstOrDefault(x => x.idpunto.ToString() == idpunto);
                //Usuario usuario = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == Util.getusuario());
                //mdl_auditoria auditoria = new XPQuery<mdl_auditoria>(session).FirstOrDefault(x => x.idaud.ToString() == idaud);
                //for (int i = 0; i < grdActividades.VisibleRowCount; i++)
                //{
                //    var rowValues = grdActividades.GetRowValues(i, "Actividad");
                //    mdl_actividad actividad = new XPQuery<mdl_actividad>(session).FirstOrDefault(x => x.Code == rowValues.ToString());
                //    audmov.listaActividades.Add(actividad);

                //}
                //        audmov.auditoria = auditoria;
                //        audmov.punto = punto;
                //        audmov.status = false;
                //        audmov.aceptada = false;
                //        audmov.cerrada = false;
                //        audmov.fecha = DateTime.Now;
                //        audmov.distribuidor = auditoria.Iddistribuidor;                        
                //audmov.evaluador = usuario;
                audmov.ArchivoImportar = dataF;
                if (txtComentario.Text != "")
                    audmov.comentario = txtComentario.Text;
                else
                    audmov.comentario = "Enviado";
                audmov.centroServicio = cmbCS.SelectedItem!=null? cmbCS.SelectedItem.Text:"Distribuidor";
                audmov.status = true;
                audmov.Save();
                uow.CommitChanges();                  
            }
            txtComentario.Text = "";
            btnUpload.Enabled = false;
            btnUpload.Text = "Auditoría Enviada";
            txtComentario.Enabled = false;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string idAmg = Session["amg"] as string;
            Response.Redirect("~/auditoria_movil.aspx?idamg=" + idAmg);
        }
    }
}