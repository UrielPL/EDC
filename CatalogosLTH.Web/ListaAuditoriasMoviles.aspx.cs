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
    public partial class ListaAuditoriasMoviles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CmbStatus.Items.Add(resultadov.Cumple.ToString());
                CmbStatus.Items.Add(resultadov.Ignorado.ToString());
                CmbStatus.Items.Add(resultadov.NoCumple.ToString());
                CmbStatus.Items.Add(resultadov.Pendiente.ToString());
                CmbStatus.Items.Add("Todos");
                CmbStatus.SelectedIndex = 3;
                var codigo = Request.QueryString["codigo"];
                if (codigo != null)
                {
                    codigo = Cryptography.Decripta(codigo.ToString());
                    Session["lstidamg"] = codigo;
                }
                
            }
            //List<mdl_AuditoriaMovil> auditorias = new XPQuery<mdl_AuditoriaMovil>(session).Where(x => x.cerrada == false).ToList();
            llenaGrid(); 
        }

        public void llenaGrid()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string codigo = Session["lstidamg"] as string;
            if (!string.IsNullOrEmpty(codigo))
            {
                if (CmbStatus.SelectedIndex == 4)
                {
                    List<mdl_AuditoriaMovil> auditorias = auditorias = new XPQuery<mdl_AuditoriaMovil>(session).Where(x => x.AuditoriaGeneral.IdAudMovG.ToString() == codigo).ToList();
                    var auds = from au in auditorias
                               where au.status == true || au.aceptada == true
                               select new
                               {
                                   IdAuditoriaMovil = au.IdAuditoriaMovil,
                                   Area = (au.punto != null) ? au.punto.Areas.FirstOrDefault().Nombre : "NA",
                                   Punto = (au.punto != null) ? au.punto.texto : "NA",
                                   Distribuidor = (au.distribuidor != null) ? au.distribuidor.nombredist : "NA",
                                   TipoAuditoria = (au.punto != null) ? au.punto.Idtipoaud.Descripcion : "NA",
                                   Evaluador = (au.evaluador != null) ? au.evaluador.Nombre : "NA",
                                   Status = au.resultado,
                                   FechaAbierta = au.fecha,
                                   Cerrada = au.cerrada
                               };

                    //data = data.OrderByDescending(x => x.fecha);
                    auds = auds.OrderByDescending(x => x.FechaAbierta);
                    gridAuditorias.DataSource = auds.ToList();
                    gridAuditorias.AutoGenerateColumns = true;
                    gridAuditorias.KeyFieldName = "IdAuditoriaMovil";
                    gridAuditorias.EnableRowsCache = true;
                    gridAuditorias.DataBind();
                    // auditorias = new XPQuery<mdl_AuditoriaMovil>(session).ToList();
                }
                else
                {
                    List<mdl_AuditoriaMovil> auditorias2 = new XPQuery<mdl_AuditoriaMovil>(session).Where(x => x.resultado == CmbStatus.SelectedItem.ToString()&&x.AuditoriaGeneral.IdAudMovG.ToString()==codigo).ToList();
                    var auds2 = from au in auditorias2
                                where au.status == true || au.aceptada == true
                                select new
                                {
                                    IdAuditoriaMovil = au.IdAuditoriaMovil,
                                    Area = (au.punto != null) ? au.punto.Areas.FirstOrDefault().Nombre : "NA",
                                    Punto = (au.punto != null) ? au.punto.texto : "NA",
                                    Distribuidor = (au.distribuidor != null) ? au.distribuidor.nombredist : "NA",
                                    TipoAuditoria = (au.punto != null) ? au.punto.Idtipoaud.Descripcion : "NA",
                                    Evaluador = (au.evaluador != null) ? au.evaluador.Nombre : "NA",
                                    Status = au.resultado,
                                    FechaAbierta = au.fecha,
                                    Cerrada = au.cerrada
                                };

                    //data = data.OrderByDescending(x => x.fecha);
                    auds2 = auds2.OrderByDescending(x => x.FechaAbierta);
                    gridAuditorias.DataSource = auds2.ToList();
                    gridAuditorias.AutoGenerateColumns = true;
                    gridAuditorias.KeyFieldName = "IdAuditoriaMovil";
                    gridAuditorias.EnableRowsCache = true;
                    gridAuditorias.DataBind();
                }

            }


        }
      

        protected void btnSeleccion_Click1(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            int indice = gridAuditorias.FocusedRowIndex;
            string codigo = gridAuditorias.GetRowValues(indice, new string[] { "IdAuditoriaMovil" }).ToString();
            /*   mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == cmbDist.SelectedItem.Text);//dist seleccionado en cmb
               mdl_audidet auddet = new XPQuery<mdl_audidet>(session).FirstOrDefault(x => x.Idaud == dist.UltimaAuditoria && x.Idpunto.clavepunto == codigo);
               */
            Response.Redirect("~/RevisionAuditoriaMovil.aspx?codigo=" + Cryptography.Encriptar(codigo));
        }

        protected void CmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenaGrid();
        }
    }
}