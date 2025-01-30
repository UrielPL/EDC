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
    //Gerente de cuenta	Zona	Numero de Distribuidores	Prof. Promedio	Act. Terminadas

    public partial class Reporte5 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DevExpress.Xpo.Session session = Util.getsession();
                int pos = 0;
                int cont = 0;
                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;
                if (month >= 10) { year++; }
                var periodos = session.Query<mdl_periodo>().ToList();

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
            llenaTabla();
        }

        public void llenaTabla()
        {
            DevExpress.Xpo.Session session = Util.getsession();

            string nombreActual = Util.getusuario();//Nombre usuario 
            string permiso = Util.getPermiso().ToString();//Permiso usuario
            ViewState["permiso"] = permiso;


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

            int selectedYear = Convert.ToInt32(cmbYear.SelectedItem.ToString());

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            if (month >= 10) { year++; }
            /*REPORTE DE ACTIVIDADES COMPLETADAS POR ZONA POR MES*/
            if (permiso == "Distribuidor" || permiso == "GerenteCuenta" || permiso == "GerenteVenta")
            {
                Usuario u = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
                string z = u.ZonaPertenece.zona;
                var zona = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == z && x.Periodo.Periodo == selectedYear);//Filtra registros mensuales por la zona del usuario logeado
                List<elementos> lst = new List<elementos>();
                var avg = from r in zona
                          group r by new { r.orden } into groups
                          select new elementos { grupo = groups.Key.orden, valor = groups.Sum(rec => rec.terminadas) };
                lst = avg.ToList();


                object[] values = new object[13];//Arreglo valores , se agrega una row
                values[0] = z;
                foreach (var item in avg)
                {
                    values[item.grupo] = item.valor.ToString();
                }

                dt.Rows.Add(values);
                ASPxGridView1.DataSource = dt;
            }
            else
            {
                var zonaN = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == "Norte" && x.Periodo.Periodo == selectedYear);
                var zonaS = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == "Sur" && x.Periodo.Periodo == selectedYear);
                var zonaC = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.zonastr == "CAM" && x.Periodo.Periodo == selectedYear);
                List<elementos> lstN = new List<elementos>();
                List<elementos> lstS = new List<elementos>();
                List<elementos> lstC = new List<elementos>();

                var avgN = from r in zonaN
                           group r by new { r.orden } into groups
                           select new elementos { grupo = groups.Key.orden, valor = groups.Sum(rec => rec.terminadas) };
                lstN = avgN.ToList();

                var avgS = from r in zonaS
                           group r by new { r.orden } into groups
                           select new elementos { grupo = groups.Key.orden, valor = groups.Sum(rec => rec.terminadas) };
                lstS = avgS.ToList();

                var avgC = from r in zonaC
                           group r by new { r.orden } into groups
                           select new elementos { grupo = groups.Key.orden, valor = groups.Sum(rec => rec.terminadas) };
                lstC = avgC.ToList();

                object[] valuesN = new object[13];//Arreglo valores NORTE, se agrega una row
                valuesN[0] = "Norte";
                foreach (var item in avgN)
                {
                    valuesN[item.grupo] = item.valor.ToString();
                }
                dt.Rows.Add(valuesN);
                //***********************
                object[] valuesS = new object[13];//Arreglo valores SUR, se agrega una row
                valuesS[0] = "Sur";
                foreach (var item in avgS)
                {
                    valuesS[item.grupo] = item.valor.ToString();
                }

                dt.Rows.Add(valuesS);
                //*********************
                object[] valuesC = new object[13];//Arreglo valores CAM, se agrega una row
                valuesC[0] = "CAM";
                foreach (var item in avgC)
                {
                    valuesC[item.grupo] = item.valor.ToString();
                }

                dt.Rows.Add(valuesC);

                ASPxGridView1.DataSource = dt;
            }
            ASPxGridView1.DataBind();
            ASPxGridViewExporter1.GridViewID = "ASPxGridView1";
            ASPxGridViewExporter1.FileName = "EDC.Rep5_" + DateTime.Today.ToShortDateString();
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.ReportHeader = "Actividades completadas por zona por mes";
            ASPxGridViewExporter1.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.ReportHeader = "Actividades completadas por zona por mes";
            ASPxGridViewExporter1.Landscape = true;
            ASPxGridViewExporter1.WritePdfToResponse();
        }

        protected void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
   
}