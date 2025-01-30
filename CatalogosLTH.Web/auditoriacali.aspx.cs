using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class auditoriacali : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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

                string IdAuditoria = Request.QueryString["IdAuditoria"];
                ViewState["IdAuditoria"] = IdAuditoria;
                

                XPQuery<mdl_auditoria> auditorias = session.Query<mdl_auditoria>();
                var sqlA = from a in auditorias
                           where a.idaud==int.Parse(IdAuditoria) && a.estatus==0
                          
                           select a;
                if (sqlA == null || sqlA.Count() == 0)
                {
                    Response.Write("No existe una auditoria abierta");
                }
                else
                {

                    mdl_auditoria audi = sqlA.First<mdl_auditoria>();
                    cmbDistribuidor.SelectedValue = audi.Iddistribuidor.iddistribuidor.ToString();
                    cmbTipoAuditoria.SelectedValue = audi.Idtipoaud.idTipoAuditoria.ToString();
                    cmbTipoAuditoria.Enabled = false;
                    cmbDistribuidor.Enabled = false;
                    ViewState["empezar"] = "true";
                    
                }
            }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            
            
            
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            int IdAuditoria =int.Parse( ViewState["IdAuditoria"].ToString());
            int iddistribuidor=0;
            XPQuery<mdl_audidet> audDetalles = session.Query<mdl_audidet>();
            var sql = from ad in audDetalles
                      where ad.Idaud.idaud == IdAuditoria
                      select ad;
            foreach (mdl_audidet item in sql)
            {
                int valor = 0;
                String grp = Request.Form["califa"+item.id.ToString()];
                if (grp!= null && grp.Equals("on"))
                    valor = 1;
                item.resultado = valor;
                item.texto = Request.Form["coment" + item.id.ToString()];
                iddistribuidor = item.Idaud.Iddistribuidor.iddistribuidor;
                item.Save();
            }
            //Util.ActualizarNivelProfesionalizacion(iddistribuidor);


            // ActualizarEncargados(IdAuditoria);
        }

        protected void btnVerNivel_Click(object sender, EventArgs e)
        {
            Response.Redirect("nivel.aspx?IdAuditoria=" +Cryptography.Encriptar( ViewState["IdAuditoria"].ToString()));
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            int IdAuditoria = int.Parse(ViewState["IdAuditoria"].ToString());
            XPQuery<mdl_audidet> audDetalles = session.Query<mdl_audidet>();
            var sql = (from ad in audDetalles
                       where ad.Idaud.idaud == IdAuditoria
                       orderby ad.Idpunto.Idpilar.idpilar ascending
                       select ad).ToList();


            Response.Clear();
            var fileName = "Auditoria.xls";
            var file = new FileInfo(fileName);

            using (var package = new OfficeOpenXml.ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Reporte Ventas");
                worksheet.Cells[1, 1].Value = "PILAR";
                worksheet.Cells[1, 2].Value = "PUNTO";
                worksheet.Cells[1, 3].Value = "RESULTADO";
                worksheet.Cells[1, 4].Value = "COMENTARIO";
                
                var rowCounter =1 ;

                foreach (var v in sql)
                {
                    if (rowCounter<sql.Count())
                    {
                        worksheet.Cells[rowCounter+1, 1].Value = sql[rowCounter].Idpunto.Idpilar.nombrepil;
                        worksheet.Cells[rowCounter+1, 2].Value = sql[rowCounter].Idpunto.texto;
                        worksheet.Cells[rowCounter+1, 3].Value = sql[rowCounter].resultado;
                        worksheet.Cells[rowCounter+1, 4].Value = sql[rowCounter].texto;
                    }
                              

                    rowCounter++;
                }
                worksheet.Column(1).AutoFit();
                worksheet.Column(2).Width = 100;


                package.Workbook.Properties.Title = "Attempts";
                this.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                this.Response.AddHeader(
                          "content-disposition",
                          string.Format("attachment;  filename={0}", "Auditoria.xlsx"));
                this.Response.BinaryWrite(package.GetAsByteArray());
                Response.End();
                // System.Web.HttpContext.Current.Response.Write(package.GetAsByteArray());

                // System.Web.HttpContext.Current.Response.End();
            }

        }
    }
}