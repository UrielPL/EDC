using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Export;
using DevExpress.Xpo;
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
    public partial class Reporte8 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DevExpress.Xpo.Session session = Util.getsession();
                string permiso = Util.getPermiso().ToString();//Permiso usuario
                var periodos = session.Query<mdl_periodo>().ToList();

                ViewState["permiso"] = permiso;
                
                ASPxGridViewExporter1.GridViewID = "ASPxGridView1";
                ASPxGridViewExporter1.FileName = "EDC.Rep8_" + DateTime.Today.ToShortDateString();
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
            }

            llenatabla();
        }

        public void llenatabla()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string nombreActual = Util.getusuario();//Nombre usuario 
            string permiso = Util.getPermiso().ToString();//Permiso usuario
            DataTable dt = new DataTable();

            dt.Columns.Add("Zona");
            dt.Columns.Add("P.Oct");
            dt.Columns.Add("A.Oct");
            dt.Columns.Add("P.Nov");
            dt.Columns.Add("A.Nov");
            dt.Columns.Add("P.Dec");
            dt.Columns.Add("A.Dec");
            dt.Columns.Add("P.Jan");
            dt.Columns.Add("A.Jan");
            dt.Columns.Add("P.Feb");
            dt.Columns.Add("A.Feb");
            dt.Columns.Add("P.Mar");
            dt.Columns.Add("A.Mar");
            dt.Columns.Add("P.Apr");
            dt.Columns.Add("A.Apr");
            dt.Columns.Add("P.May");
            dt.Columns.Add("A.May");
            dt.Columns.Add("P.Jun");
            dt.Columns.Add("A.Jun");
            dt.Columns.Add("P.Jul");
            dt.Columns.Add("A.Jul");
            dt.Columns.Add("P.Ago");
            dt.Columns.Add("A.Ago");
            dt.Columns.Add("P.Sep");
            dt.Columns.Add("A.Sep");

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            if (month >= 10) { year++; }
            int selectedYear = Convert.ToInt32(cmbYear.SelectedItem.ToString());

            switch (permiso)
            {
                case "GerenteCuenta":
                case "GerenteDesarrolloComercial":
                case "Distribuidor":
                    Usuario u = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
                    string z = u.ZonaPertenece.zona;
                    var zona = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == z && x.Periodo.Periodo == selectedYear &&x.Distribuidor.EnDesarrollo==false);//Filtra registros mensuales por la zona del usuario logeado

                    List<elementosR8> lst = new List<elementosR8>();
                    var avg = from r in zona
                              group r by new { r.orden } into groups
                              select new elementosR8 { grupo = groups.Key.orden, acts = groups.Sum(rec => rec.terminadas), valor = groups.Average(r => r.resultado) };
                    lst = avg.ToList();

                    object[] values = new object[25];//Arreglo valores , se agrega una row
                    values[0] = z;

                    foreach (var item in avg)
                    {
                        values[(item.grupo * 2 - 1)] = item.valor.ToString("0.00");
                        values[(item.grupo * 2)] = item.acts.ToString();
                    }

                    dt.Rows.Add(values);
                    ASPxGridView1.DataSource = dt;

                    break;



                case "Admin":

                    var zonaN = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == "Norte" && x.Periodo.Periodo == selectedYear&&x.Distribuidor.EnDesarrollo==false);
                    var zonaS = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == "Sur" && x.Periodo.Periodo == selectedYear && x.Distribuidor.EnDesarrollo == false);
                    var zonaC = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == "CAM" && x.Periodo.Periodo == selectedYear && x.Distribuidor.EnDesarrollo == false);
                    List<elementosR8> lstN = new List<elementosR8>();
                    List<elementosR8> lstS = new List<elementosR8>();
                    List<elementosR8> lstC = new List<elementosR8>();

                    var avgN = from r in zonaN
                               group r by new { r.orden } into groups
                               select new elementosR8 { grupo = groups.Key.orden, acts = groups.Sum(rec => rec.terminadas), valor = groups.Average(r => r.resultado) };
                    lstN = avgN.ToList();

                    var avgS = from r in zonaS
                               group r by new { r.orden } into groups
                               select new elementosR8 { grupo = groups.Key.orden, acts = groups.Sum(rec => rec.terminadas), valor = groups.Average(r => r.resultado) };
                    lstS = avgS.ToList();

                    var avgC = from r in zonaC
                               group r by new { r.orden } into groups
                               select new elementosR8 { grupo = groups.Key.orden, acts = groups.Sum(rec => rec.terminadas), valor = groups.Average(r => r.resultado) };
                    lstC = avgC.ToList();

                    object[] valuesN = new object[25];//Arreglo valores NORTE, se agrega una row
                    valuesN[0] = "Norte";
                    foreach (var item in avgN)
                    {
                        // valuesN[item.grupo] = item.valor.ToString();
                        valuesN[(item.grupo * 2 - 1)] = item.valor.ToString("0.00");
                        valuesN[(item.grupo * 2)] = item.acts.ToString();
                    }
                    dt.Rows.Add(valuesN);
                    //***********************
                    object[] valuesS = new object[25];//Arreglo valores SUR, se agrega una row
                    valuesS[0] = "Sur";
                    foreach (var item in avgS)
                    {
                        //valuesS[item.grupo] = item.valor.ToString();
                        valuesS[(item.grupo * 2 - 1)] = item.valor.ToString("0.00");
                        valuesS[(item.grupo * 2)] = item.acts.ToString();
                    }

                    dt.Rows.Add(valuesS);
                    //*********************
                    object[] valuesC = new object[25];//Arreglo valores CAM, se agrega una row
                    valuesC[0] = "CAM";
                    foreach (var item in avgC)
                    {
                        valuesC[(item.grupo * 2 - 1)] = item.valor.ToString("0.00");
                        valuesC[(item.grupo * 2)] = item.acts.ToString();
                    }

                    dt.Rows.Add(valuesC);

                    ASPxGridView1.DataSource = dt;
                    break;

                default:
                    break;
            }

            ASPxGridView1.DataBind();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.ReportHeader = "Profesionalización mensual y actividades completadas";

            ASPxGridViewExporter1.WriteXlsToResponse(new XlsExportOptionsEx { ExportType = ExportType.WYSIWYG });
            //ASPxGridViewExporter1.WritePdfToResponse();
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.ReportHeader = "Profesionalización mensual y actividades completadas";
            ASPxGridViewExporter1.Landscape = true;
            ASPxGridViewExporter1.WritePdfToResponse();
        }

        protected void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}