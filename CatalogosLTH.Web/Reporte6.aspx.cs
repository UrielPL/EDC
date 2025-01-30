using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Export;
using DevExpress.Xpo;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class Reporte6 : System.Web.UI.Page
    {
        public List<elmGerentes> lstGerentes = new List<elmGerentes>();
        protected void Page_Load(object sender, EventArgs e)
        {
            //Gerente de cuenta	  Zona	Numero de Distribuidores	Prof. Promedio	Act. Terminadas
            DevExpress.Xpo.Session session = Util.getsession();
            string nombreActual = Util.getusuario();//Nombre usuario 
            string permiso = Util.getPermiso().ToString();//Permiso usuario
            var gerentes = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.GerenteCuenta);
            var distribuidores = new XPQuery<mdl_distribuidor>(session).Where(c=>c.EnDesarrollo==false);
            Usuario u = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
            string z = "";
            
            ViewState["permiso"] = permiso;

            if (permiso == "GerenteDesarrolloComercial")
            {
                z = u.ZonaPertenece.zona;
                gerentes = gerentes.Where(x => x.ZonaPertenece.zona == z);
            }
            else if (permiso== "GerenteCuenta")
            {
                z = u.ZonaPertenece.zona;
                gerentes = gerentes.Where(x => x.UserName == nombreActual);
            }
            else if (permiso=="Distribuidor")
            {
                z = u.ZonaPertenece.zona;
                Response.Redirect("tablero1.aspx");
            }

            foreach (var gerente in gerentes)
            {
                elmGerentes g = new elmGerentes();
           
                /*
                 var sql3 = from rg in registro
                           join us in usuarios on rg.Distribuidor.nombredist equals us.UserName
                           select new { Zona = rg.Distribuidor.zonastr, Nombre = us.Nombre, User = rg.Distribuidor.nombredist, Jefe = us.Jefe, Resultado = rg.resultado, Nivel = rg.nivel, Completadas = rg.terminadas };

                */

                var dependientes = from dp in gerente.Dependientes
                                   join dis in distribuidores on dp.UserName equals dis.nombredist
                                   select dis;

                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;
                if (month >= 10) { year++; }

                int sumaActs = 0;
                double promProf = 0;
                double avPS = 0;
                double avIn = 0;
                double avAd = 0;
                double avEj = 0;
                double avPl = 0;
                foreach (var item in dependientes)
                {
                    if (year==2017)
                    {
                        var actividades = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.nombredist == item.nombredist && x.Periodo.Periodo == year&&x.orden!=1);
                        sumaActs += actividades.Sum(x => x.terminadas);
                    }
                    else
                    {
                        var actividades = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.nombredist == item.nombredist && x.Periodo.Periodo == year);
                        sumaActs += actividades.Sum(x => x.terminadas);
                    }
                    
                }
                //if (gerente.Dependientes.Count>0)
                if(dependientes.Count()>0)
                {
                    promProf = dependientes.Average(x => x.profesionalizacion);
                    avPS = dependientes.Average(x => x.avanceProd);
                    avIn = dependientes.Average(x => x.avanceInfra);
                    avAd = dependientes.Average(x => x.avanceAdministracion);
                    avEj = dependientes.Average(x => x.avanceEjecucion);
                    avPl = dependientes.Average(x => x.avancePlaneacion);
                }
                               

                g.Nombre = gerente.Nombre;
                g.Zona = gerente.ZonaPertenece != null ? gerente.ZonaPertenece.zona : "";
                g.Distribuidores = gerente.Dependientes.Count;
                g.ProfPromedio = promProf.ToString("0.00");
                g.ActTerminadas = sumaActs;
                g.ProductosServicios = avPS.ToString("0.00");
                g.Infraestructura = avIn.ToString("0.00");
                g.Administracion = avAd.ToString("0.00");
                g.Ejecucion = avEj.ToString("0.00");
                g.Planeacion = avPl.ToString("0.00");
                lstGerentes.Add(g);
            }
            ASPxGridView1.DataSource = lstGerentes;
            ASPxGridView1.AutoGenerateColumns = true;
            ASPxGridView1.KeyFieldName = "nombre";
            ASPxGridView1.DataBind();

            ASPxGridViewExporter1.GridViewID = "ASPxGridView1";
            ASPxGridViewExporter1.FileName = "EDC.Rep6_" + DateTime.Today.ToShortDateString();
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.ReportHeader = "Profesionalización por gerente de cuenta";
            ASPxGridViewExporter1.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.ReportHeader = "Profesionalización por gerente de cuenta";
            ASPxGridViewExporter1.Landscape = true;
            ASPxGridViewExporter1.WritePdfToResponse();
        }
    }
    public class elmGerentes
    {
        public string Nombre { get; set; }
        public string Zona { get; set; }
        public int Distribuidores { get; set; }
        public string ProfPromedio { get; set; }
        public int ActTerminadas { get; set; }
        public string ProductosServicios { get; set; }
        public string Infraestructura { get; set; }
        public string Administracion { get; set; }
        public string Ejecucion { get; set; }
        public string Planeacion { get; set; }
    }
}