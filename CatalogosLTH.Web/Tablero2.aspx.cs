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
    public partial class Tablero2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string permiso = Util.getPermiso().ToString();//Permiso usuario
            ViewState["permiso"] = permiso;
            string rol = Util.getrol();
            ViewState["rol"] = rol;
            if (!IsPostBack)
            {
               
                    // XPCollection<mdl_distribuidor> dist = new XPCollection<mdl_distribuidor>(session);
                    //var periodos = session.Query<mdl_periodo>().ToList();
                    var periodos = new XPQuery<mdl_periodo>(session).ToList();                    

                   
                    comboPeriodos.Items.Add("OCTUBRE");
                    comboPeriodos.Items.Add("NOVIEMBRE");
                    comboPeriodos.Items.Add("DICIEMBRE");
                    comboPeriodos.Items.Add("ENERO");
                    comboPeriodos.Items.Add("FEBRERO");
                    comboPeriodos.Items.Add("MARZO");
                    comboPeriodos.Items.Add("ABRIL");
                    comboPeriodos.Items.Add("MAYO");
                    comboPeriodos.Items.Add("JUNIO");
                    comboPeriodos.Items.Add("JULIO");
                    comboPeriodos.Items.Add("AGOSTO");
                    comboPeriodos.Items.Add("SEPTIEMBRE");
                    comboPeriodos.SelectedIndex = 0;
                
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

                ASPxGridViewExporter1.ReportHeader = "Tablero 2";
                ASPxGridViewExporter1.GridViewID = "ASPxGridView1";
                ASPxGridViewExporter1.FileName = "EDC.Tab2_" + DateTime.Today.ToShortDateString();
            }

            llenaTabla();



        }

        protected void comboPeriodos_SelectedIndexChanged(object sender, EventArgs e)
        {
           llenaTabla();
        }
        public void llenaTabla()
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string permiso = Util.getPermiso().ToString();//Permiso usuario
            string nombreActual = Util.getusuario();//Nombre usuario 
            int mesactual = DateTime.Today.Month;

            if (Util.getrol() == "DC Administrativo")
            {
                permiso = "DCAdmin";
            }

            List<int> mes = new List<int>();
            mes.Add(10);
            mes.Add(11);
            mes.Add(12);
            mes.Add(1);
            mes.Add(2);
            mes.Add(3);
            mes.Add(4);
            mes.Add(5);
            mes.Add(6);
            mes.Add(7);
            mes.Add(8);
            mes.Add(9);

            int selectedYear = Convert.ToInt32(comboYear.SelectedItem.ToString());
            int messeleccionado = comboPeriodos.SelectedIndex + 1;
            int mesReal = mes.ElementAt(messeleccionado - 1);//Mes SELECCIONADO REAL
            int year = DateTime.Today.Year;
            if (mesactual > 9)
            {
                year++;
            }
            if (mesactual!=mesReal||year!=selectedYear)//SE DESHABILITA EL BOTON PARA NO MODIFICAR REGISTROS DE MESES ANTERIORES O FUTUROS
            {
                ASPxButton1.Enabled = false;                
            }
            else
            {
                ASPxButton1.Enabled = true;
            }

            
            mdl_periodo periodo = new XPQuery<mdl_periodo>(session).FirstOrDefault(x => x.Periodo == selectedYear);//Periodo actual

            var registro = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Periodo == periodo && x.orden == messeleccionado); //Registros del periodo y mes seleccionados

            if (permiso == "GerenteCuenta")
            {
                var usuarios = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor && x.Jefe.UserName == nombreActual).ToList();
                var sql3 = from us in usuarios
                           join rg in registro on us.UserName equals rg.Distribuidor.nombredist
                           select new { zona = rg.Distribuidor.zonastr, Nombre = rg.Distribuidor.nombredist, Resultado = rg.resultado.ToString("0.00"), Nivel = rg.nivel, Completadas = rg.terminadas };
                if (sql3 != null && sql3.Count() > 0)
                {
                    ASPxGridView1.DataSource = sql3.ToList();

                }
            }
            else if (permiso == "DCAdmin")
            {
                var usuarios = new XPQuery<Usuario>(session)
                    .FirstOrDefault(x => x.UserName == nombreActual)
                            .DistribuidoresSupervisa.ToList();

                var sql3 = from us in usuarios
                           join rg in registro on us.UserName equals rg.Distribuidor.nombredist
                           select new { zona = rg.Distribuidor.zonastr, Nombre = rg.Distribuidor.nombredist, Resultado = rg.resultado.ToString("0.00"), Nivel = rg.nivel, Completadas = rg.terminadas };
                if (sql3 != null && sql3.Count() > 0)
                {
                    ASPxGridView1.DataSource = sql3.ToList();

                }
            }
            else if (permiso == "Distribuidor")
            {
                var usuarios = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor && x.UserName == nombreActual).ToList();
                var sql3 = from us in usuarios
                           join rg in registro on us.UserName equals rg.Distribuidor.nombredist
                           select new { zona = rg.Distribuidor.zonastr, Nombre = rg.Distribuidor.nombredist, Resultado = rg.resultado.ToString("0.00"), Nivel = rg.nivel, Completadas = rg.terminadas };
                if (sql3 != null && sql3.Count() > 0)
                {
                    ASPxGridView1.DataSource = sql3.ToList();

                }
            }
            else if (permiso == "GerenteVenta" || permiso=="GerenteDesarrolloComercial")
            {
                Usuario actual = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
                string z = actual.ZonaPertenece.zona;
                var listaDistribuidores = new XPQuery<Usuario>(session).Where(c => c.ZonaPertenece.zona == z && c.TipoUsuario == TipoUsuario.Distribuidor).ToList();

                var sql3 = from us in listaDistribuidores
                           join rg in registro on us.UserName equals rg.Distribuidor.nombredist
                           select new { zona = rg.Distribuidor.zonastr, Nombre = rg.Distribuidor.nombredist, Resultado = rg.resultado.ToString("0.00"), Nivel = rg.nivel, Completadas = rg.terminadas };
                if (sql3 != null && sql3.Count() > 0)
                {
                    ASPxGridView1.DataSource = sql3.ToList();

                }
            }

            else
            {

                var usuarios = new XPQuery<Usuario>(session);

                var sql3 = from rg in registro
                           join us in usuarios on rg.Distribuidor.nombredist equals us.UserName
                           select new { Zona = rg.Distribuidor.zonastr, Nombre = us.Nombre, User = rg.Distribuidor.nombredist, Jefe = us.Jefe, Resultado = rg.resultado.ToString("0.00"), Nivel = rg.nivel, Completadas = rg.terminadas };

                var sql2 = from reg in registro
                           select new { zona = reg.Distribuidor.zonastr, Nombre = reg.Distribuidor.nombredist, Resultado = reg.resultado.ToString("0.00"), Nivel = reg.nivel, Completadas = reg.terminadas };

                int xi = sql3.Count();
                if (sql3 != null && sql3.Count() > 0)
                {
                    ASPxGridView1.DataSource = sql2;

                }
            }
           
            ASPxGridView1.AutoGenerateColumns = true;
            ASPxGridView1.KeyFieldName = "Nombre";
            ASPxGridView1.DataBind();
            ASPxGridView1.EnableRowsCache = false;

        }

        protected void ASPxButton1_Click(object sender, EventArgs e)//CALCULAR
        {
            DevExpress.Xpo.Session session = Util.getsession();

            List<int> mes = new List<int>();
            mes.Add(10);
            mes.Add(11);
            mes.Add(12);
            mes.Add(1);
            mes.Add(2);
            mes.Add(3);
            mes.Add(4);
            mes.Add(5);
            mes.Add(6);
            mes.Add(7);
            mes.Add(8);
            mes.Add(9);

            int messeleccionado = comboPeriodos.SelectedIndex+1;
            int mesReal = mes.ElementAt(messeleccionado-1);

            int year = DateTime.Today.Year;
            if (mesReal>9)
            {
                year++;
            }
            
            var registros = new XPQuery<mdl_RegistroMensual>(session).Where(x => x.orden == messeleccionado && x.Periodo.Periodo== year);//TRAE  REGISTROS DEL MES SELECCIONADO Y PERIODO SELECCIONADO
            XPCollection<mdl_distribuidor> dist = new XPCollection<mdl_distribuidor>(session);
            //var distribx = new XPQuery<mdl_distribuidor>(session).Where(x => x.nombredist == "MASTER ELECTRONIC");
            int TotalActividades = 0;
            using (UnitOfWork varses = new UnitOfWork())
            {
                int cont = 0;
                foreach (var reg in registros)
                {
                    cont++;
                    foreach (var dis in dist)
                    {
                        if (reg.Distribuidor==dis)
                        {
                            int actTerminadas = new XPQuery<mdl_auditoriaactividad>(session).Where(x => x.Idaud == dis.UltimaAuditoria && x.fechacomp.Month == mesReal && x.fechacomp.Year == DateTime.Now.Year).Count();
                            reg.resultado = dis.profesionalizacion;
                            reg.nivel = dis.nivelAct;
                            reg.terminadas = actTerminadas;
                            reg.Save();
                        }                      
                    }
                }
                varses.CommitChanges();
            }
        }

        protected void Excel(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.WriteXlsxToResponse(new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
        }

        protected void comboYear_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            ASPxGridViewExporter1.WritePdfToResponse();
        }
    }
}