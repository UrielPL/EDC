using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Export;
using DevExpress.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class ActividadesRevisar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Util.getPermiso()==TipoUsuario.Distribuidor)
                {
                    Response.Redirect("MainPage.aspx");
                }
                ASPxGridViewExporter1.GridViewID = "ASPxGridView1";
                ASPxGridView1.Styles.Header.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Middle;
                //llenagrid();
            }
            llenagrid();
        }

        public void llenagrid()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string usuario = Util.getusuario();
            TipoUsuario permiso = Util.getPermiso();
            //var audact= new XPQuery<mdl_auditoriaactividad>


            XPQuery<mdl_auditoriaactividad> audacts = session.Query<mdl_auditoriaactividad>();
            XPQuery<mdl_auditoria> auditorias = session.Query<mdl_auditoria>();
            XPQuery<mdl_actividad> actividades = session.Query<mdl_actividad>();
            XPQuery<mdl_distribuidor> distribuidores = session.Query<mdl_distribuidor>();

      /*      List<int> UltimasAuditorias = new List<int>();//Se declara lista de ids de auditorias
            List<mdl_auditoria> AudUltimasAuditorias = new List<mdl_auditoria>();//Se declara lista de auditorias
           // var distribuidores = new XPQuery<mdl_auditoria>(session).Select(x=>x.Iddistribuidor).Distinct();
            foreach (var item in distribuidores)
            {
                int ultAud = Util.getUltimaAuditoria(item.iddistribuidor);
                mdl_auditoria audit = new XPQuery<mdl_auditoria>(session).FirstOrDefault(x => x.idaud == ultAud);
                if (!AudUltimasAuditorias.Contains(audit))
                {
                    AudUltimasAuditorias.Add(audit);
                }
            }*/



            if (permiso==TipoUsuario.Admin)
            {
                var data2 = from aa in audacts
                            join ac in actividades on aa.Idactividad equals ac
                            join au in auditorias on aa.Idaud equals au                            //join ds in distribuidores on au equals ds.UltimaAuditoria
                            where aa.status == "En revisión" && aa.fechacomp == null// && aa.Idaud==ds.UltimaAuditoria
                            select new {aa.idactplan,
                                au.idaud,
                                ac.Code,
                                Actividad = ac.Texto,
                                Duración =aa.duracion,
                                Fecha_Inicio = aa.fechainicio,
                                Fecha_Completada = aa.fechafinal,
                                Estatus =  aa.status,
                                Evaluador = aa.Evaluador.Nombre,
                                Distribuidor =au.Iddistribuidor.nombredist,
                                EnRevision =aa.fechaEnvio,                                
                                Usuario = aa.Idaud.Iddistribuidor.nombreusuario
                            };

                /*var data2 = from ua in UltimasAuditorias
                            join aa in audacts on ua equals aa.Idaud.idaud
                            join ac in actividades on aa.Idactividad equals ac                            
                            join au in auditorias on ua equals au.idaud
                            where aa.status == "En revisión" && aa.fechacomp == null //&& aa.Idaud.idaud==Util.getUltimaAuditoria(aa.Idaud.Iddistribuidor.iddistribuidor)
                            select new { aa.idactplan, au.idaud, ac.Code, ac.Texto, aa.duracion, aa.fechainicio, aa.fechafinal, aa.comentario, aa.status, aa.Evaluador.Nombre, au.Iddistribuidor.nombredist };*/


                ASPxGridView1.DataSource = data2.ToList(); //Se agrega Data Source
            }
            else
            {
                var data = from aa in audacts
                           join ac in actividades on aa.Idactividad equals ac
                           join au in auditorias on aa.Idaud equals au                           
                           where aa.Evaluador.UserName == usuario && aa.status == "En revisión" && aa.fechacomp == null// && aa.Idaud.idaud == Util.getUltimaAuditoria(aa.Idaud.Iddistribuidor.iddistribuidor)
                           select new { aa.idactplan,
                               au.idaud,
                               ac.Code,
                               Actividad = ac.Texto,
                               Duración = aa.duracion,
                               Fecha_Inicio = aa.fechainicio,
                               Fecha_Completada = aa.fechafinal,
                               Estatus = aa.status,
                               Evaluador = aa.Evaluador.Nombre,
                               Distribuidor = au.Iddistribuidor.nombredist,
                               EnRevision = aa.fechaEnvio,
                               Usuario=aa.Idaud.Iddistribuidor.nombreusuario
                           };
                ASPxGridView1.DataSource = data.ToList();
            }
            
            //where aa.fechacomp == null && aa.Evaluador.UserName==usuario && aa.status=="Por realizar"
            


            //var audact = new XPQuery<mdl_auditoriaactividad>(session).Where(x => x.Evaluador.UserName == usuario && x.status == "Por realizar").OrderBy(p => p.Idaud);

          
            ASPxGridView1.AutoGenerateColumns = true;
            ASPxGridView1.KeyFieldName = "Code";
            ASPxGridView1.DataBind();
            ASPxGridView1.EnableRowsCache = false;

            foreach (GridViewDataColumn columna in ASPxGridView1.Columns)
            {
                string col2 = columna.FieldName.ToString();
                if (col2 == "idactplan" || col2=="idaud" )
                {
                    columna.Visible = false;
                }

            }
        }


        protected void ASPxGridView1_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)//HACE INVISIBLES ALGUNAS COLUMNAS
        {
            if (e.DataColumn.FieldName != "idaud") return;

            DevExpress.Xpo.Session session = Util.getsession();
            mdl_auditoria Auditoria = new XPQuery<mdl_auditoria>(session).FirstOrDefault(a => a.idaud.ToString() == e.CellValue.ToString());
            mdl_distribuidor Distribuidor = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(v => v.iddistribuidor.ToString() == e.CellValue.ToString());
            

            int idAud = Util.getUltimaAuditoria(Distribuidor.iddistribuidor);

            if (e.CellValue.ToString()!=idAud.ToString())
            {
                e.DataColumn.Visible = false;                
            }
        }

        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            int indice = ASPxGridView1.FocusedRowIndex;
            string codigo = ASPxGridView1.GetRowValues(indice, new string[] { "idactplan" }).ToString();
            Response.Redirect("~/SeleccionActividad.aspx?codigo=" + Cryptography.Encriptar(codigo));
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.WriteXlsToResponse(new XlsExportOptionsEx { ExportType =ExportType.WYSIWYG});
        }
    }
}