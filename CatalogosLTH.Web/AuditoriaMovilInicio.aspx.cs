using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Web;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class AuditoriaMovilInicio : System.Web.UI.Page
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
                    if (dist.UltimaAuditoria!=null)
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

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            int indice = gridPuntoArea.FocusedRowIndex;
            string codigo = gridPuntoArea.GetRowValues(indice, new string[] { "ClavePunto" }).ToString();
            mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == cmbDist.SelectedItem.Text);//dist seleccionado en cmb
            mdl_audidet auddet = new XPQuery<mdl_audidet>(session).FirstOrDefault(x => x.Idaud == dist.UltimaAuditoria && x.Idpunto.clavepunto == codigo);

            Response.Redirect("~/auditoriamovildetalle.aspx?codigo=" + Cryptography.Encriptar(auddet.id.ToString()));
        }

        public void llenagrid()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            mdl_tipoauditoria tipo = new XPQuery<mdl_tipoauditoria>(session).FirstOrDefault(a => a.Descripcion == cmbTipo.SelectedItem.Text);//tipo seleccionado de aud en cmb
            mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == cmbDist.SelectedItem.Text);//dist seleccionado en cmb
            mdl_Area area = new XPQuery<mdl_Area>(session).FirstOrDefault(x => x.Nombre == cmbArea.SelectedItem.Text&& x.Idtipoaud==tipo);//Area seleccionada
            mdl_auditoria ultAud = dist.UltimaAuditoria;
            //mdl_auditoriaactividad ax = new XPQuery<mdl_auditoriaactividad>(session).FirstOrDefault(z => z.idactplan == 5612);
            List<mdl_auditoriaactividad> audact = ultAud.auditoriaActividad.Where(s => s.fechacomp == DateTime.MinValue).ToList();// actividades sin completar
            List<mdl_punto> lstPuntos = new XPQuery<mdl_punto>(session).Where(c => c.Idtipoaud == tipo).ToList();
            List<mdl_Area> lstAreas = new XPQuery<mdl_Area>(session).Where(d => d.Idtipoaud == tipo).ToList();
            List<actpunto> lstActPunto = new List<actpunto>();
            /**var distribuidores = from ud in usuariosDist
                                   join dis in dist on ud.UserName equals dis.nombredist
                                   where ud.Jefe!=null
                                   select new { Nombre = dis.nombredist, Nivel=dis.profesionalizacion, Zona=dis.zona};*/
          
                               
            /*

            
             List<string> puntos = new List<string>();//LISTA DE PUNTOS
               foreach (var item in audact)//recorre actividades sin completar
              {                
                  string idactividad = item.Idactividad.IdActividad.ToString();
                  foreach (var punto in item.Idactividad.Puntos.Where(x=>x.Idtipoaud==tipo))
                  {                    
                      actpunto a1 = new actpunto();
                      a1.idact = idactividad;
                      a1.clavepunto = punto.clavepunto;
                      lstActPunto.Add(a1);
                      if (!puntos.Contains(punto.clavepunto))
                      {
                          puntos.Add(punto.clavepunto);
                      }
                  }              
              }*/



              List < puntoarea > lstPuntoArea = new List<puntoarea>();
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

            lstPuntoArea = lstPuntoArea.Where(x => x.Area == area.Nombre).ToList();
            
            gridPuntoArea.DataSource = lstPuntoArea;
            gridPuntoArea.AutoGenerateColumns = true;
            gridPuntoArea.KeyFieldName = "ClavePunto";
            gridPuntoArea.EnableRowsCache = true;
            
            gridPuntoArea.DataBind();

            int index = 0;
            foreach (var columna in gridPuntoArea.Columns)
            {
                if (index<3)
                {
                    GridViewDataColumn c = (GridViewDataColumn)columna;
                    string col2 = c.FieldName.ToString();
                    if (col2 == "IdArea")
                    {
                        c.Visible = false;
                    }
                }
                index++;
            }

        }
        protected void gridPuntoArea_DataBound(object sender, EventArgs e)
        {

            ASPxGridView grid = sender as ASPxGridView;
                int cont = grid.GetSelectedFieldValues("ClavePunto").Count;
                if (cont>1)
                {
                    btnAceptar.Enabled = false;
                }

            if (grid.Columns.IndexOf(grid.Columns["Completa"]) != -1)
                return;
            GridViewCommandColumn col = new GridViewCommandColumn();
            col.Name = "Completa";
            col.ShowSelectCheckbox = true;
            col.VisibleIndex = 3;
            col.Caption = "Completa";

            GridViewCommandColumn col2 = new GridViewCommandColumn();
            col2.Name = "Incompleta";
            col2.ShowSelectCheckbox = true;
            col2.VisibleIndex = 4;
            col2.Caption = "Incompleta";
            grid.Columns.Add(col);
            grid.Columns.Add(col2);

        }
        public void llenaDist()
        {
            cmbDist.Items.Clear();
            DevExpress.Xpo.Session session = Util.getsession();

            mdl_tipoauditoria tipo = new XPQuery<mdl_tipoauditoria>(session).FirstOrDefault(a => a.Descripcion == cmbTipo.SelectedItem.Text);           

            List<mdl_auditoria> UltimasAudTipo = new List<mdl_auditoria>();
            List<mdl_distribuidor> distribuidoresValidos = new List<mdl_distribuidor>();
            XPCollection<mdl_distribuidor> distribuidores = new XPCollection<mdl_distribuidor>(session);
            foreach (var dist in distribuidores)
            {
                if (dist.UltimaAuditoria.Idtipoaud == tipo && dist.UltimaAuditoria != null)
                {
                    UltimasAudTipo.Add(dist.UltimaAuditoria);
                    distribuidoresValidos.Add(dist);
                    cmbDist.Items.Add(dist.nombredist, dist.nombreusuario);
                }
            }
            cmbDist.SelectedIndex = 0;
            llenagrid();
        }

        protected void btnRevision_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            mdl_tipoauditoria tipo = new XPQuery<mdl_tipoauditoria>(session).FirstOrDefault(a => a.Descripcion == cmbTipo.SelectedItem.Text);//tipo seleccionado de aud en cmb
            Usuario usuario = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == Util.getusuario());
            mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == cmbDist.SelectedItem.Text);//dist seleccionado en cmb
            mdl_Area area = new XPQuery<mdl_Area>(session).FirstOrDefault(x => x.Nombre == cmbArea.SelectedItem.Text && x.Idtipoaud == tipo);//Area seleccionada

            List<mdl_AuditoriaMovil> lstAudMov = new List<mdl_AuditoriaMovil>();
            List<mdl_punto> lstPunto = new List<mdl_punto>();
            
            mdl_auditoria ultAud = dist.UltimaAuditoria;

            foreach (var item in ASPxListBox1.Items)
            {

                using (UnitOfWork uow = new UnitOfWork())
                {
                    mdl_punto punto = new XPQuery<mdl_punto>(session).FirstOrDefault(x => x.texto == item.ToString()&&x.Idtipoaud.Descripcion==cmbTipo.SelectedItem.Text);

                    mdl_AuditoriaMovil audmov = new mdl_AuditoriaMovil(session);
                    //mdl_auditoria auditoria = new XPQuery<mdl_auditoria>(session).FirstOrDefault(x => x.idaud.ToString() == idaud);
                    //for (int i = 0; i < grdActividades.VisibleRowCount; i++)
                    foreach (var acts in punto.Actividades)                    
                    {                       
                        audmov.listaActividades.Add(acts);
                    }
                    audmov.auditoria = ultAud;
                    audmov.punto = punto;
                    audmov.status = false;
                    audmov.aceptada = false;
                    audmov.cerrada = false;
                    audmov.fecha = DateTime.Now;
                    audmov.distribuidor = dist;
                    //audmov.ArchivoImportar = dataF;
                    audmov.evaluador = usuario;
                    audmov.comentario = "Enviado en lista";
                    audmov.Save();
                    uow.CommitChanges();
                }
            }
            

//            var lstPuntos = new XPQuery<mdl_punto>
        }
        protected void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
        {


            llenaDist();

        }

        protected void cmbDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenagrid();


        }

        protected void cmbArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenagrid();
        }

        
    }

    public class actaudact
    {
        actaudact()
        {

        }
        string idactplan { get; set; }
        string idact { get; set; }
    }

    public class actpunto
    {
        public actpunto()
        {

        }
        public string idact { get; set; }
        public string clavepunto { get; set; }

    }
    [Serializable]
    public class puntoarea
    {

        public puntoarea()
        {

        }
        public string ClavePunto { get; set; }
        public string IdArea { get; set; }
        public string Punto { get; set; }
        public string Area { get; set; }
        public string Comentario { get; set; }
        public bool aceptada { get; set; }
        public bool noaceptada { get; set; }

    }
}
