using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Export;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    
    public partial class Reporte10 : System.Web.UI.Page
    {

        public List<elementos> grupoTotal = new List<elementos>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

                DevExpress.Xpo.Session session = Util.getsession();
               // List<mdl_distribuidor> dist = new XPCollection<mdl_distribuidor>(session).OrderBy(x=>x.nombredist).ToList();
                List<Usuario> listaDistribuidores = new List<Usuario>();

                string nombreActual = Util.getusuario();
                string permiso = Util.getPermiso().ToString();
                string rol = Util.getrol();
                ViewState["rol"] = rol;

                if(nombreActual== "Alonso.sierra.siller@jci.com")
                {
                    permiso = "Admin";
                }
                if (Util.getrol() == "DC Administrativo") { permiso = "DCAdmin"; }

                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;

                var periodos = session.Query<mdl_periodo>().ToList();
                cmbYear.Items.AddRange(periodos.Select(x => x.Periodo).Distinct().ToList());
                cmbYear.SelectedIndex = periodos.Count-1;
                
                switch (permiso)
                {
                    case "GerenteCuenta":
                        listaDistribuidores = new XPQuery<Usuario>(session)
                            .Where(x => x.Jefe.UserName == nombreActual).ToList();
                        break;
                    case "GerenteDesarrolloComercial":
                        Usuario u = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
                        string z = u.ZonaPertenece.zona;
                        listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.ZonaPertenece.zona == z && x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
                        break;
                    case "Distribuidor":
                        listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.UserName == nombreActual).ToList();
                        break;
                    case "GerenteVenta":
                        Usuario us = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
                        string zn = us.ZonaPertenece.zona;
                        listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.ZonaPertenece.zona == zn && x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
                        break;
                    case "Admin":
                        listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
                        Usuario uTdo = new Usuario(session);
                        uTdo.Nombre = " Todos";
                        uTdo.UserName = " Todos";
                        listaDistribuidores.Add(uTdo);
                        break;

                    case "DCAdmin":
                        listaDistribuidores = new XPQuery<Usuario>(session)
                            .FirstOrDefault(x => x.UserName == nombreActual)
                            .DistribuidoresSupervisa.ToList();

                        break;
                }//FIN SWITCH

                dropDist.DataSource = listaDistribuidores.OrderBy(c=>c.UserName);
                dropDist.DataTextField = "UserName";
                dropDist.DataBind();
            }
            llenaGrid();

        }

        public void llenaGrid()
        {
            int[] arrPos = new int[] { 10, 11, 12, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
          
            object[] valuesR = new object[13];
            valuesR[0] = dropDist.SelectedItem.Text; 
            for(int x=1;x<13; x++) { valuesR[x] = 0; }

            DevExpress.Xpo.Session session = Util.getsession();
            int year = int.Parse(cmbYear.SelectedItem.Text);
            int month = DateTime.Now.Month;

            int selectedYear = Convert.ToInt32(cmbYear.SelectedItem.ToString());


            DataTable dt = new DataTable();
            dt.Columns.Add("Distribuidor");
            dt.Columns.Add("Oct"); dt.Columns.Add("Nov"); dt.Columns.Add("Dic"); dt.Columns.Add("Ene");
            dt.Columns.Add("Feb"); dt.Columns.Add("Mar"); dt.Columns.Add("Abr"); dt.Columns.Add("May");
            dt.Columns.Add("Jun"); dt.Columns.Add("Jul"); dt.Columns.Add("Ago"); dt.Columns.Add("Sep");

            if (dropDist != null && dropDist.Text != "" && dropDist.Text != " Todos")
            {
                mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist.ToString() == dropDist.SelectedItem.Text);
                List<Archivos> lstArch = new XPQuery<Archivos>(session).Where(x =>
                    x.IdAuditoriaActividad.Idaud.Iddistribuidor.iddistribuidor == dist.iddistribuidor &&
                    x.usuario == "Distribuidor" &&
                        (x.fecha.Date.Month < 10 && x.fecha.Date.Year == year ||
                         x.fecha.Date.Month >= 10 && x.fecha.Date.Year == (year - 1))).ToList();

                var totalf =
                    from p in lstArch
                    group p by new { p.IdAuditoriaActividad, p.fecha.Date.Month } into gr
                    select new elementosR8 { grupo = gr.Key.Month, acts = gr.Count(), valor = gr.FirstOrDefault().IdAuditoriaActividad.Idactividad.IdActividad};

                 grupoTotal =
                    (from p in totalf
                    group p by new { p.grupo } into gr
                    select new elementos { grupo = gr.Key.grupo, valor = gr.Count() }).ToList();

                int pos = -1;
                foreach (var item in grupoTotal)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        if (arrPos[i] == item.grupo) pos = i;
                    }
                    if(pos!=-1)
                    {
                        valuesR[pos+1] = item.valor.ToString("0");
                    }
                    pos = -1;
                }
                dt.Rows.Add(valuesR);
                gridAvanceActividades.DataSource = dt;
                gridAvanceActividades.DataBind();

                //lstArch.GroupBy(x => new { x.IdAuditoriaActividad, x.fecha.Date.Month }).Select(x => x.First());

            }
            else if(dropDist.Text == " Todos")
            {
                var lstDist = new XPQuery<mdl_distribuidor>(session).OrderBy(v=>v.nombredist).ToList();
                foreach (var dist in lstDist)
                {
                    if(dist.nombredist!="nohay")
                    {
                        for (int x = 1; x < 13; x++) { valuesR[x] = 0; }

                        valuesR[0] = dist.nombredist;

                        List<Archivos> lstArch = new XPQuery<Archivos>(session).Where(x =>
                       x.IdAuditoriaActividad.Idaud.Iddistribuidor.iddistribuidor == dist.iddistribuidor &&
                       x.usuario == "Distribuidor" &&
                           (x.fecha.Date.Month < 10 && x.fecha.Date.Year == year ||
                            x.fecha.Date.Month >= 10 && x.fecha.Date.Year == (year - 1))).ToList();

                        var totalf =
                            from p in lstArch
                            group p by new { p.IdAuditoriaActividad, p.fecha.Date.Month } into gr
                            select new elementosR8 { grupo = gr.Key.Month, acts = gr.Count(), valor = gr.FirstOrDefault().IdAuditoriaActividad.Idactividad.IdActividad };

                        grupoTotal =
                           (from p in totalf
                            group p by new { p.grupo } into gr
                            select new elementos { grupo = gr.Key.grupo, valor = gr.Count() }).ToList();

                        int pos = -1;
                        foreach (var item in grupoTotal)
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                if (arrPos[i] == item.grupo) pos = i;
                            }
                            if (pos != -1)
                            {
                                valuesR[pos + 1] = item.valor.ToString("0");
                            }
                            pos = -1;
                        }
                        dt.Rows.Add(valuesR);
                    }
                    
                }
               
                gridAvanceActividades.DataSource = dt;
                gridAvanceActividades.DataBind();

            }


        }

        protected void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenaGrid();
        }

        protected void dropDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenaGrid();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.GridViewID = "gridAvanceActividades";
            ASPxGridViewExporter1.FileName = "EDC.Rep10_" + DateTime.Today.ToShortDateString();
            ASPxGridViewExporter1.ReportHeader = "Profesionalización mensual y actividades completadas por distribuidor";
            ASPxGridViewExporter1.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }
    }
     
}