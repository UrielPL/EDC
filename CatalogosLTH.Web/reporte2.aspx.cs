using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Export;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
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
    public partial class reporte2 : System.Web.UI.Page
    {
        public List<double> valNorte = new List<double>();
        public List<string> lblNorte = new List<string>();
        public List<double> valSur = new List<double>();
        public List<string> lblSur = new List<string>();
        public List<double> valCam = new List<double>();
        public List<string> lblCam = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string nombreActual = Util.getusuario();//Nombre usuario 
            string permiso = Util.getPermiso().ToString();//Permiso usuario
            ViewState["permiso"] = permiso;
            if (permiso=="Distribuidor"||permiso=="GerenteCuenta"||permiso=="GerenteVenta")
            {
                Usuario u = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
                string z = u.ZonaPertenece.zona;

                var niveles = new XPQuery<mdl_nivel>(session).ToList();
                var zonas = new XPQuery<mdl_zona>(session).Where(x => x.zona == z).ToList();

                List<string> listaNiveles = new List<string>();
                List<string> listaZonas = new List<string>();

                foreach (var item in niveles) { listaNiveles.Add(item.nombreniv); }
                foreach (var item in zonas) { listaZonas.Add(item.zona); }

                DataTable dt = new DataTable();
                dt.Columns.Add("Zona");
                foreach (var itemN in listaNiveles)
                {
                    dt.Columns.Add(itemN.ToString());
                }
                dt.Columns.Add("Total");


                foreach (var itemZ in listaZonas)
                {
                    object[] values = new object[listaNiveles.Count + 2];
                    values[0] = itemZ;

                    int total = 0;
                    for (int i = 0; i < listaNiveles.Count; i++)
                    {
                        var itemN = listaNiveles[i];
                        int cantidad = new XPQuery<mdl_distribuidor>(session).Where(x => x.zonastr == itemZ.ToString() && x.nivelAct == itemN.ToString()).Count();
                        values[i + 1] = cantidad.ToString();
                        total += cantidad;
                    }
                    values[values.Length - 1] = total;

                    graficas(values, itemZ, listaNiveles);

                    dt.Rows.Add(values);
                }
                ASPxGridView1.DataSource = dt;
            }
            else
            {
                var niveles = new XPQuery<mdl_nivel>(session).ToList();
                var zonas = new XPQuery<mdl_zona>(session).ToList();

                List<string> listaNiveles = new List<string>();
                List<string> listaZonas = new List<string>();

                foreach (var item in niveles) { listaNiveles.Add(item.nombreniv); }
                foreach (var item in zonas) { listaZonas.Add(item.zona); }

                DataTable dt = new DataTable();
                dt.Columns.Add("Zona");
                foreach (var itemN in listaNiveles)
                {
                    dt.Columns.Add(itemN.ToString());
                }
                dt.Columns.Add("Total");


                foreach (var itemZ in listaZonas)
                {
                    object[] values = new object[listaNiveles.Count + 2];
                    values[0] = itemZ;

                    int total = 0;
                    for (int i = 0; i < listaNiveles.Count; i++)
                    {
                        var itemN = listaNiveles[i];
                        int cantidad = new XPQuery<mdl_distribuidor>(session).Where(x => x.zonastr == itemZ.ToString() && x.nivelAct == itemN.ToString()).Count();
                        values[i + 1] = cantidad.ToString();
                        total += cantidad;
                    }
                    values[values.Length - 1] = total;

                    graficas(values, itemZ, listaNiveles);

                    dt.Rows.Add(values);
                }
                ASPxGridView1.DataSource = dt;
            }
           
            
            
           
            //ASPxGridView1.AutoGenerateColumns = true;
            //ASPxGridView1.KeyFieldName = "idactplan";
            ASPxGridView1.DataBind();
            //ASPxGridView1.EnableRowsCache = true;
            ASPxGridViewExporter1.ReportHeader = "Nivel de profesionalización por zona";            
            ASPxGridViewExporter1.GridViewID = "ASPxGridView1";
            ASPxGridViewExporter1.FileName = "EDC.Rep2_" + DateTime.Today.ToString();
        }

        public void graficas(object[]values, string zona, List<string> niveles)
        {
            // Create an empty chart.
          //  ChartControl pieChart = new ChartControl();
            WebChartControl chartElemento = new WebChartControl();
            // Create a pie series.
            Series series1 = new Series(zona, ViewType.Pie);

            // Populate the series with points.
            for (int i = 0; i < niveles.Count; i++)
            {
                double valor = Convert.ToDouble(values[i+1].ToString());
                double total = Convert.ToDouble(values[values.Length - 1].ToString());
                double puntos = valor * 100 / total;
                series1.Points.Add(new SeriesPoint(niveles.ElementAt(i), puntos));
                if (zona == "Norte")
                {
                    valNorte.Add(puntos);
                    lblNorte.Add(niveles.ElementAt(i));
                }
                if (zona == "Sur")
                {
                    valSur.Add(puntos);
                    lblSur.Add(niveles.ElementAt(i));
                }
                if (zona == "CAM")
                {
                    valCam.Add(puntos);
                    lblCam.Add(niveles.ElementAt(i));
                }
                    
            }
           

            // Add the series to the chart.
            chartElemento.Series.Add(series1);

            // Format the the series labels.
            series1.Label.TextPattern = "{A}: {VP:p0}";

            // Adjust the position of series labels. 
            ((PieSeriesLabel)series1.Label).Position = PieSeriesLabelPosition.TwoColumns;

            // Detect overlapping of series labels.
            ((PieSeriesLabel)series1.Label).ResolveOverlappingMode = ResolveOverlappingMode.Default;

            // Access the view-type-specific options of the series.
            PieSeriesView myView = (PieSeriesView)series1.View;

            // Show a title for the series.
            myView.Titles.Add(new SeriesTitle());
            myView.Titles[0].Text = series1.Name;

            // Specify a data filter to explode points.
            myView.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Value_1,
                DataFilterCondition.GreaterThanOrEqual, 9));
            myView.ExplodedPointsFilters.Add(new SeriesPointFilter(SeriesPointKey.Argument,
                DataFilterCondition.NotEqual, "Others"));
            myView.ExplodeMode = PieExplodeMode.UseFilters;
            myView.ExplodedDistancePercentage = 30;
            myView.RuntimeExploding = true;
            myView.HeightToWidthRatio = 0.75;

            // Hide the legend (if necessary).
            chartElemento.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            // Add the chart to the form.
           // chartNorte.Dock = DockStyle.Fill;
            Panel1.Controls.Add(chartElemento);

        }

        protected void Excel(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.WritePdfToResponse();
        }
    }
}