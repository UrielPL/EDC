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
    public partial class Reporte9 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DevExpress.Xpo.Session session = Util.getsession();
                XPCollection<mdl_distribuidor> dist = new XPCollection<mdl_distribuidor>(session);
                ASPxGridViewExporter1.GridViewID = "TablaProfesionalizacion";

                SortingCollection sortCollection = new SortingCollection();
                sortCollection.Add(new SortProperty("nombredist", SortingDirection.Ascending));
                dist.Sorting = sortCollection;

                string nombreActual = Util.getusuario();
                string permiso = Util.getPermiso().ToString();
                string rol = Util.getrol();
                ViewState["rol"] = rol;

                if (Util.getrol() == "DC Administrativo")
                {
                    permiso = "DCAdmin";
                }

                List<Usuario> listaDistribuidores = new List<Usuario>();
                List<mdl_distribuidor> listaDistribuidoresDist = new List<mdl_distribuidor>();
                var periodos = session.Query<mdl_periodo>().ToList();

                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;
                int pos = 0;
                int cont = 0;
                if (month >= 10) { year++; }
                foreach (var item in periodos)
                {
                    if (item.Periodo == year)
                    {
                        pos = cont;
                    }
                    cmbYear.Items.Add(item.Periodo + "");
                    cont++;
                }
                cmbYear.SelectedIndex = pos;

                switch (permiso)
                {
                    case "GerenteCuenta":
                        listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName == nombreActual).ToList();
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
                        break;
                    case "DCAdmin":
                        listaDistribuidores = new XPQuery<Usuario>(session)
                            .FirstOrDefault(x => x.UserName == nombreActual)
                            .DistribuidoresSupervisa.ToList();

                        break;
                }

                listaDistribuidores = listaDistribuidores.OrderBy(x => x.UserName).ToList();

                dropDist.DataSource = listaDistribuidores;
                dropDist.DataTextField = "UserName";
                dropDist.DataBind();
                
                if (permiso== "Admin")
                {                 
                    dropDist.Items.Add("Todos");
                }
            } //Postback
            llenaGrid();


        }


        public void llenaGrid()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            List<elementosR8> lstR = new List<elementosR8>();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            if (month >= 10) { year++; }

            
            int selectedYear = Convert.ToInt32(cmbYear.SelectedItem.ToString());

            DataTable dt = new DataTable();
                    dt.Columns.Add("Distribuidor"); dt.Columns.Add("P.Oct"); dt.Columns.Add("A.Oct"); dt.Columns.Add("P.Nov"); dt.Columns.Add("A.Nov");
                    dt.Columns.Add("P.Dec"); dt.Columns.Add("A.Dec"); dt.Columns.Add("P.Jan"); dt.Columns.Add("A.Jan"); dt.Columns.Add("P.Feb");
                    dt.Columns.Add("A.Feb"); dt.Columns.Add("P.Mar"); dt.Columns.Add("A.Mar"); dt.Columns.Add("P.Apr"); dt.Columns.Add("A.Apr");
                    dt.Columns.Add("P.May"); dt.Columns.Add("A.May"); dt.Columns.Add("P.Jun"); dt.Columns.Add("A.Jun"); dt.Columns.Add("P.Jul");
                    dt.Columns.Add("A.Jul"); dt.Columns.Add("P.Ago"); dt.Columns.Add("A.Ago"); dt.Columns.Add("P.Sep"); dt.Columns.Add("A.Sep");

            int iddist = 62;
            string nombreDist = "";
            if (dropDist != null && dropDist.Text != "" &&dropDist.Text!="Todos")
            {
                mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist.ToString() == dropDist.SelectedItem.Text);
                iddist = dist.iddistribuidor;
                nombreDist = dist.nombredist;


                var regMens = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.iddistribuidor == iddist && x.Periodo.Periodo == selectedYear);

                var regM = from r in regMens
                           select new elementosR8 { grupo = r.orden, acts = r.terminadas, valor = r.resultado };
                lstR = regM.ToList();

                object[] valuesR = new object[25];
                valuesR[0] = nombreDist;

                foreach (var item in regM)
                {
                    // valuesN[item.grupo] = item.valor.ToString();
                    valuesR[(item.grupo * 2 - 1)] = item.valor.ToString("0.00");
                    valuesR[(item.grupo * 2)] = item.acts.ToString();
                }
                dt.Rows.Add(valuesR);
            }
            else if (dropDist.Text=="Todos")
            {
                XPQuery<mdl_distribuidor> distribuidores = session.Query<mdl_distribuidor>();
                foreach (var dist in distribuidores)
                {
                    var xregMens = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.iddistribuidor == dist.iddistribuidor && x.Periodo.Periodo == selectedYear);
                    var regM = from r in xregMens
                               select new elementosR8 { grupo = r.orden, acts = r.terminadas, valor = r.resultado };
                    lstR = regM.ToList();

                    object[] valuesR = new object[25];
                    valuesR[0] = dist.nombredist;

                    foreach (var item in regM)
                    {
                        // valuesN[item.grupo] = item.valor.ToString();
                        valuesR[(item.grupo * 2 - 1)] = item.valor.ToString("0.00");
                        valuesR[(item.grupo * 2)] = item.acts.ToString();
                    }
                    dt.Rows.Add(valuesR);

                }

            }




            TablaProfesionalizacion.DataSource = dt;
            TablaProfesionalizacion.DataBind();

        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.ReportHeader = "Profesionalización mensual y actividades completadas por distribuidor";
            ASPxGridViewExporter1.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void dropDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenaGrid();
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.ReportHeader = "Profesionalización mensual y actividades completadas por distribuidor";
            ASPxGridViewExporter1.Landscape = true;
            ASPxGridViewExporter1.WritePdfToResponse();
        }

        protected void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenaGrid();
        }
    }
}