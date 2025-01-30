using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Export;
using DevExpress.Xpo;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
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
    public partial class Reporte3 : System.Web.UI.Page
    {
        public List<elementos> valoresN = new List<elementos>();
        public List<elementos> valoresS = new List<elementos>();
        public List<elementos> valoresC = new List<elementos>();
        protected void Page_Load(object sender, EventArgs e)
        {
           
            DevExpress.Xpo.Session session = Util.getsession();
            if (!IsPostBack)
            {
                var periodos = session.Query<mdl_periodo>().ToList();
                string nombreActual = Util.getusuario();//Nombre usuario 
                string permiso = Util.getPermiso().ToString();//Permiso usuario
                ViewState["permiso"] = permiso;


                int year = DateTime.Today.Year;
                if (DateTime.Today.Month > 9)
                {
                    year++;
                }
                int pos = 0;
                int cont = 0;
                foreach (var item in periodos)
                {
                    if (item.Periodo == year)
                    {
                        pos = cont;
                    }
                    comboYear.Items.Add(item.Periodo + "");
                    cont++;
                }
                comboYear.SelectedIndex = pos;
            }

            /***************************SE TERMINAN DE LLENAR COMBOS****************************/
            if (pnl1.Controls.Count > 0)
            { pnl1.Controls.RemoveAt(0); }
            if (pnl2.Controls.Count > 0)
            { pnl2.Controls.RemoveAt(0); }
            if (pnl3.Controls.Count > 0)
            { pnl3.Controls.RemoveAt(0); }
            llenatabla();
            ASPxGridViewExporter1.GridViewID = "ASPxGridView1";
            ASPxGridViewExporter1.FileName = "EDC.Rep3_" + DateTime.Today.ToShortDateString();
        }

        public void llenatabla()
        {
            DevExpress.Xpo.Session session = Util.getsession();

            DataTable dt = new DataTable();
            dt.Columns.Add("Zona");
            dt.Columns.Add("Oct");
            dt.Columns.Add("Nov");
            dt.Columns.Add("Dec");
            dt.Columns.Add("Jan");
            dt.Columns.Add("Feb");
            dt.Columns.Add("Mar");
            dt.Columns.Add("Apr");
            dt.Columns.Add("May");
            dt.Columns.Add("Jun");
            dt.Columns.Add("Jul");
            dt.Columns.Add("Ago");
            dt.Columns.Add("Sep");

            
            string nombreActual = Util.getusuario();//Nombre usuario 
            string permiso = Util.getPermiso().ToString();//Permiso usuario

            int selectedYear = Convert.ToInt32(comboYear.SelectedItem.ToString());

            if (permiso == "Distribuidor" || permiso == "GerenteCuenta" || permiso == "GerenteVenta")
            {
                Usuario u = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
                string z = u.ZonaPertenece.zona;
                var zona = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == z&&x.Periodo.Periodo==selectedYear&&x.Distribuidor.EnDesarrollo==false);

                List<elementos> lst = new List<elementos>();
                var avg = from r in zona
                          group r by new { r.orden } into groups
                          select new elementos { grupo = groups.Key.orden, valor = groups.Average(rec => rec.resultado) };
                lst = avg.ToList();


                object[] values = new object[13];//Arreglo valores , se agrega una row
                values[0] = z;
                foreach (var item in avg)
                {
                    if (item.valor == 0)
                    {
                        values[item.grupo] = item.valor.ToString();
                    }
                    else
                    {
                        values[item.grupo] = item.valor.ToString("0.00");
                    }

                }
                Graficas(lst, z);
                dt.Rows.Add(values);
                ASPxGridView1.DataSource = dt;
            }
            else
            {
                var zonaN = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == "Norte" && x.Periodo.Periodo == selectedYear && x.Distribuidor.EnDesarrollo == false);
                var zonaS = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == "Sur" && x.Periodo.Periodo == selectedYear && x.Distribuidor.EnDesarrollo == false);
                var zonaC = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == "CAM" && x.Periodo.Periodo == selectedYear && x.Distribuidor.EnDesarrollo == false);
                List<elementos> lstN = new List<elementos>();
                List<elementos> lstS = new List<elementos>();
                List<elementos> lstC = new List<elementos>();

                var avgN = from r in zonaN
                           group r by new { r.orden } into groups
                           select new elementos { grupo = groups.Key.orden, valor = groups.Average(rec => rec.resultado) };
                lstN = avgN.ToList();

                var avgS = from r in zonaS
                           group r by new { r.orden } into groups
                           select new elementos { grupo = groups.Key.orden, valor = groups.Average(rec => rec.resultado) };
                lstS = avgS.ToList();

                var avgC = from r in zonaC
                           group r by new { r.orden } into groups
                           select new elementos { grupo = groups.Key.orden, valor = groups.Average(rec => rec.resultado) };
                lstC = avgC.ToList();

                object[] valuesN = new object[13];//Arreglo valores NORTE, se agrega una row
                valuesN[0] = "Norte";
                foreach (var item in avgN)
                {
                    valuesN[item.grupo] = item.valor.ToString("0.00");
                }
                Graficas(lstN, "Norte");//Se mandan parametros para hacer la grafica
                dt.Rows.Add(valuesN);
                //***********************
                object[] valuesS = new object[13];//Arreglo valores SUR, se agrega una row
                valuesS[0] = "Sur";
                foreach (var item in avgS)
                {
                    valuesS[item.grupo] = item.valor.ToString("0.00");
                }
                Graficas(lstS, "Sur");//Se mandan parametros para hacer la grafica
                dt.Rows.Add(valuesS);
                //*********************
                object[] valuesC = new object[13];//Arreglo valores CAM, se agrega una row
                valuesC[0] = "CAM";
                foreach (var item in avgC)
                {
                    valuesC[item.grupo] = item.valor.ToString("0.00");
                }
                Graficas(lstC, "CAM");//Se mandan parametros para hacer la grafica
                dt.Rows.Add(valuesC);

                ASPxGridView1.DataSource = dt;

            }
            ASPxGridView1.DataBind();
        }

        protected void Excel(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.ReportHeader = "Profesionalización mensual por zona";
            ASPxGridViewExporter1.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        public void Graficas(List<elementos>valores, string z)
        {
          string[] meses = new string[12] { "Oct", "Nov", "Dec", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Ago", "Sep" };
            
            // Create a WebChartControl instance. 
            WebChartControl WebChartControl1 = new WebChartControl();

            // Create a line series and add points to it. 
            Series series1 = new Series("Profesionalización", ViewType.Line);

            ChartTitle titulo = new ChartTitle();
            // Add the chart to the form. 
            // Note that a chart isn't initialized until it's added to the form's collection of controls. 
            if (z=="Norte"){
                pnl1.Controls.Add(WebChartControl1);  titulo.Text = "Norte";
                valoresN = valores.OrderBy(x => x.grupo).ToList();
            }
            else if (z=="Sur"){
                pnl2.Controls.Add(WebChartControl1); titulo.Text = "Sur";
                valoresS = valores.OrderBy(x => x.grupo).ToList();
            }
            else if (z=="CAM"){
                pnl3.Controls.Add(WebChartControl1); titulo.Text = "CAM";
                valoresC = valores.OrderBy(x => x.grupo).ToList();
            }
            WebChartControl1.Titles.Add(titulo);
            valores = valores.OrderBy(x => x.grupo).ToList();
            
            for (int i = 0; i < valores.Count; i++)
            {
                series1.Points.Add(new SeriesPoint(meses[i], valores.ElementAt(i).valor));
            }
            WebChartControl1.Width = 420;
            // Add the series to the chart. 
            WebChartControl1.Series.Add(series1);
        }

        protected void comboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pnl1.Controls.Count > 0)
            { pnl1.Controls.RemoveAt(0); }
            if (pnl2.Controls.Count > 0)
            { pnl2.Controls.RemoveAt(0); }
            if (pnl3.Controls.Count > 0)
            { pnl3.Controls.RemoveAt(0); }
            llenatabla();
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.ReportHeader = "Profesionalización mensual por zona";

            ASPxGridViewExporter1.WritePdfToResponse();
        }
    }
    public class elementos
    {
        public int grupo;//Mes 
        public double valor;//Promedio profesionalizacion
    }
    public class elementosR8
    {
        public int grupo;
        public double valor;
        public int acts;
    }

}
