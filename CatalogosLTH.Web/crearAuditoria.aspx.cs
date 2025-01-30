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
    public partial class crearAuditoria : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Util.getrol() == "Distribuidor")
            {
                Response.Redirect("mainpage.aspx");
            }
           

            if (!IsPostBack)
            {
                DevExpress.Xpo.Session session = Util.getsession();

                XPQuery<mdl_tipoauditoria> tipoAuditorias = session.Query<mdl_tipoauditoria>();
                var sqlTA = from ta in tipoAuditorias
                            select ta;
                cmbTipoAuditoria.DataSource = sqlTA;
                cmbTipoAuditoria.DataTextField = "Descripcion";
                cmbTipoAuditoria.DataValueField = "idTipoAuditoria";
                cmbTipoAuditoria.DataBind();


                XPQuery<mdl_distribuidor> distribuidores = session.Query<mdl_distribuidor>();
                var sqlDi = from di in distribuidores
                            orderby di.nombredist
                            select di;
                cmbDistribuidor.DataSource = sqlDi;
                cmbDistribuidor.DataTextField = "nombredist";
                cmbDistribuidor.DataValueField = "iddistribuidor";
                cmbDistribuidor.DataBind();
            }
        }

        protected void btnEmpezar_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
        //**Trae todas las auditorias del distribuidor seleccionado y verifica si hay que crear una nueva o continuar con alguna
            XPQuery<mdl_auditoria> auditorias = session.Query<mdl_auditoria>();
            var sqlA = from a in auditorias
                       where a.Iddistribuidor.iddistribuidor == int.Parse(cmbDistribuidor.SelectedValue)
                       orderby a.idaud descending
                       select a;
            bool empezar = false;
            if (sqlA == null || sqlA.Count() == 0)
                empezar = true;
            else if (sqlA.First<mdl_auditoria>().estatus == 1)
                empezar = true;
            else
                empezar = false;
            if (empezar)
                btnNueva.Visible = true;
            else {
                btnContinuar.Visible = true;
                mdl_auditoria audi = sqlA.First<mdl_auditoria>();
                ViewState["IdAuditoria"] = audi.idaud.ToString();
            }
            cmbDistribuidor.Enabled = false;
            cmbTipoAuditoria.Enabled = false;
            btnEmpezar.Enabled = false;
            
        }

        protected void btnNueva_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            mdl_auditoria audi = new mdl_auditoria(session);
            XPQuery<mdl_distribuidor> distribuidores = session.Query<mdl_distribuidor>();
            var sqlDi = from di in distribuidores
                        where di.iddistribuidor==int.Parse(cmbDistribuidor.SelectedValue)
                        select di;
            audi.Iddistribuidor = sqlDi.First<mdl_distribuidor>();
            XPQuery<mdl_tipoauditoria> tipoAuditorias = session.Query<mdl_tipoauditoria>();
            var sqlTA = from ta in tipoAuditorias
                        where ta.idTipoAuditoria == int.Parse(cmbTipoAuditoria.SelectedValue)
                        select ta;
            audi.Idtipoaud = sqlTA.First<mdl_tipoauditoria>();
            audi.fechaap = DateTime.Now;
            audi.estatus = 0;
            audi.idaud = 0;
            audi.Save();

            XPQuery<mdl_punto> puntos = session.Query<mdl_punto>();
            var sqlPuntos = from p in puntos
                            where p.Idtipoaud == audi.Idtipoaud
                            select p;
            foreach (var punto in sqlPuntos)
            {
                mdl_audidet ad = new mdl_audidet(session);
                ad.resultado = 1;
                ad.Idaud = audi;
                ad.Idpunto = punto;
                ad.id = 0;
                ad.Save();
            }
            btnNueva.Visible = false;
            btnContinuar.Visible = true;
            ViewState["IdAuditoria"] = audi.idaud.ToString();
        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            Response.Redirect("auditoriacali.aspx?IdAuditoria=" + ViewState["IdAuditoria"]);
        }
    }
}