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
    public partial class ListaGeneralAuditoriaMovil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            List<AuditoriaMovilGeneral> audGeneral  = new XPQuery<AuditoriaMovilGeneral>(session).ToList();
            var auds = from au in audGeneral
                       select new
                       {
                           Id = au.IdAudMovG,
                           Evaluador = au.Evaluador.Nombre,
                           NivelDistribuidor=au.distribuidor.nombredist,
                           Fecha=au.Fecha
                       };

            //data = data.OrderByDescending(x => x.fecha);
            
            auds = auds.OrderByDescending(x => x.Fecha);
            grdAuditorias.DataSource = auds.ToList();
            grdAuditorias.AutoGenerateColumns = true;
            grdAuditorias.KeyFieldName = "Id";
            grdAuditorias.EnableRowsCache = true;
            grdAuditorias.DataBind();
        }
        public void llenagrid()
        {

        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            int indice = grdAuditorias.FocusedRowIndex;
            string codigo = grdAuditorias.GetRowValues(indice, new string[] { "Id" }).ToString();
            /*   mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == cmbDist.SelectedItem.Text);//dist seleccionado en cmb
               mdl_audidet auddet = new XPQuery<mdl_audidet>(session).FirstOrDefault(x => x.Idaud == dist.UltimaAuditoria && x.Idpunto.clavepunto == codigo);
               */
            Response.Redirect("~/listaauditoriasmoviles.aspx?codigo=" + Cryptography.Encriptar(codigo));
        }
    }
}