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
    public partial class AuditoriaMovil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DevExpress.Xpo.Session session = Util.getsession();
                XPCollection<mdl_tipoauditoria> tiposAud = new XPCollection<mdl_tipoauditoria>(session);
                foreach (var item in tiposAud)
                {
                    cmbTipo.Items.Add(item.Descripcion);
                }
                cmbTipo.SelectedIndex = 0;
                mdl_tipoauditoria tipo = new XPQuery<mdl_tipoauditoria>(session).FirstOrDefault(a => a.Descripcion == cmbTipo.SelectedItem.Text);

                List<mdl_auditoria> UltimasAudTipo = new List<mdl_auditoria>();
                List<mdl_distribuidor> distribuidoresValidos = new List<mdl_distribuidor>();
                XPCollection<mdl_distribuidor> distribuidores = new XPCollection<mdl_distribuidor>(session);
                foreach (var dist in distribuidores)
                {
                    if (dist.UltimaAuditoria != null)//Trae distribuidores con tipos de auditoria seleccionada
                    {
                        if (dist.UltimaAuditoria.Idtipoaud == tipo && dist.UltimaAuditoria != null)
                        {
                            UltimasAudTipo.Add(dist.UltimaAuditoria);
                            distribuidoresValidos.Add(dist);
                            cmbDist.Items.Add(dist.nombredist, dist.nombreusuario);
                        }
                    }

                }
                cmbDist.SelectedIndex = 0;
                int cs = 0;
                int cont = 0;
                var areasq = new XPQuery<mdl_Area>(session).Where(x => x.Idtipoaud == tipo);
                //XPCollection<mdl_Area> areas = new XPCollection<mdl_Area>(session);
                var areas = areasq.OrderBy(x => x.Nombre);
                foreach (var area in areas)
                {
                    cmbArea.Items.Add(area.Nombre);
                    if (area.Nombre == "Centro de Servicio")
                        cs = cont;
                    cont++;
                }
                cmbArea.SelectedIndex = cs;

            }
            llenagrid();

        }

        public void llenagrid()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            mdl_tipoauditoria tipo = new XPQuery<mdl_tipoauditoria>(session).FirstOrDefault(a => a.Descripcion == cmbTipo.SelectedItem.Text);//tipo seleccionado de aud en cmb
            mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == cmbDist.SelectedItem.Text);//dist seleccionado en cmb
            mdl_Area area = new XPQuery<mdl_Area>(session).FirstOrDefault(x => x.Nombre == cmbArea.SelectedItem.Text && x.Idtipoaud == tipo);//Area seleccionada
            mdl_auditoria ultAud = dist.UltimaAuditoria;
            //mdl_auditoriaactividad ax = new XPQuery<mdl_auditoriaactividad>(session).FirstOrDefault(z => z.idactplan == 5612);
            
            List<mdl_punto> lstPuntos = new XPQuery<mdl_punto>(session).Where(c => c.Idtipoaud == tipo).ToList();
            List<mdl_Area> lstAreas = new XPQuery<mdl_Area>(session).Where(d => d.Idtipoaud == tipo).ToList();
            List<actpunto> lstActPunto = new List<actpunto>();
           
            List<puntoarea> lstPuntoArea = new List<puntoarea>();
            foreach (var item in lstPuntos)
            {
                mdl_punto punto = new XPQuery<mdl_punto>(session).FirstOrDefault(x => x == item);//TODO CHANGE PREVIOUS PUNTOS LIST TO <PUNTOS> instead of <String>
                foreach (var pt in punto.Areas)
                {
                    puntoarea pa = new puntoarea();
                    pa.Punto = punto.texto;
                    pa.Area = pt.Nombre;
                    pa.ClavePunto = punto.clavepunto;
                    pa.IdArea = pt.IdArea.ToString();
                    lstPuntoArea.Add(pa);
                }
            }

            lstPuntoArea = lstPuntoArea.Where(x => x.Area == area.Nombre).ToList();//datasource
            
            ViewState["listaGrid"] = lstPuntoArea;
            
 
           

        }

        protected void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void cmbDist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void cmbArea_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnRevision_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            int IdAuditoria = int.Parse(ViewState["IdAuditoria"].ToString());
            int iddistribuidor = 0;
            XPQuery<mdl_audidet> audDetalles = session.Query<mdl_audidet>();
            var sql = from ad in audDetalles
                      where ad.Idaud.idaud == IdAuditoria
                      select ad;
            foreach (mdl_audidet item in sql)
            {
                int valor = 0;
                String grp = Request.Form["califa" + item.id.ToString()];
                if (grp != null && grp.Equals("on"))
                    valor = 1;
                item.resultado = valor;
                item.texto = Request.Form["coment" + item.id.ToString()];
                iddistribuidor = item.Idaud.Iddistribuidor.iddistribuidor;
                item.Save();
            }
        }
    }
}