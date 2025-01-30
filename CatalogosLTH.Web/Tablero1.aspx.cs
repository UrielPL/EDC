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
    public partial class Tablero1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            DevExpress.Xpo.Session session = Util.getsession();
            string permiso = Util.getPermiso().ToString();
            string nombreActual = Util.getusuario();
            string rol = Util.getrol();
            ViewState["rol"] = rol;

            List<mdl_distribuidor> DistribuidoresDeGerente = new List<mdl_distribuidor>();
                var distribuidores = new XPQuery<mdl_distribuidor>(session);

                ViewState["permiso"] = permiso;

            if (Util.getrol() == "DC Administrativo")
            {
                permiso = "DCAdmin";
            }

            if (permiso == "GerenteCuenta")
                {
                    //TRAE A LOS QUE SEAN SUS DEPENDIENTES
                    var listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName == nombreActual).ToList();
                var queryD = from us in listaDistribuidores
                          join ds in distribuidores on us.UserName equals ds.nombredist //into 
                             select new { Zona = ds.zonastr,
                                 Nombre = us.Nombre,
                                 UserName = ds.nombredist,
                                 Jefe = us.Jefe.Nombre,
                                 Resultado = ds.profesionalizacion.ToString("0.00"),
                                 Profesionalizacion = ds.nivelAct,
                                 NivelActividades = ds.nivelActividades,
                                 Cumplimiento = ds.cumplimientoNivel.ToString("0.00"),
                                 ProductosServicios = ds.avanceProd,
                                 Infraestructura = ds.avanceInfra,
                                 Administracion = ds.avanceAdministracion,
                                 Ejecucion = ds.avanceEjecucion,
                                 Planeacion = ds.avancePlaneacion };
                    //select ds;
                    ASPxGridView1.DataSource = queryD.ToList();

                }
            else if (permiso == "DCAdmin")
            {
                //TRAE A LOS QUE SEAN SUS DEPENDIENTES
                var listaDistribuidores = new XPQuery<Usuario>(session)
                            .FirstOrDefault(x => x.UserName == nombreActual)
                            .DistribuidoresSupervisa.ToList();

                var queryD = from us in listaDistribuidores
                             join ds in distribuidores on us.UserName equals ds.nombredist //into 
                             select new
                             {
                                 Zona = ds.zonastr,
                                 Nombre = us.Nombre,
                                 UserName = ds.nombredist,
                                 Jefe = us.Jefe.Nombre,
                                 Resultado = ds.profesionalizacion.ToString("0.00"),
                                 Profesionalizacion = ds.nivelAct,
                                 NivelActividades = ds.nivelActividades,
                                 Cumplimiento = ds.cumplimientoNivel.ToString("0.00"),
                                 ProductosServicios = ds.avanceProd,
                                 Infraestructura = ds.avanceInfra,
                                 Administracion = ds.avanceAdministracion,
                                 Ejecucion = ds.avanceEjecucion,
                                 Planeacion = ds.avancePlaneacion
                             };
                //select ds;
                ASPxGridView1.DataSource = queryD.ToList();

            }
            else if (permiso== "Distribuidor")
                {
                    var dist = new XPQuery<mdl_distribuidor>(session).Where(x => x.nombredist == nombreActual);                    
                    var usuario = new XPQuery<Usuario>(session).Where(y => y.UserName == nombreActual);
                    var queryD = from us in usuario
                                 join ds in dist on us.UserName equals ds.nombredist
                                 select new { Zona = ds.zonastr,
                                     Nombre = us.Nombre,
                                     UserName = ds.nombredist,
                                     Jefe = us.Jefe.Nombre,
                                     Resultado = ds.profesionalizacion.ToString("0.00"),
                                     Profesionalizacion = ds.nivelAct,
                                     NivelActividades = ds.nivelActividades,
                                     Cumplimiento = ds.cumplimientoNivel.ToString("0.00"),
                                     ProductosServicios = ds.avanceProd,
                                     Infraestructura = ds.avanceInfra,
                                     Administracion = ds.avanceAdministracion,
                                     Ejecucion = ds.avanceEjecucion,
                                     Planeacion = ds.avancePlaneacion };
                    ASPxGridView1.DataSource = queryD.ToList();
                }
           
            else if (permiso=="GerenteVenta"||permiso=="GerenteDesarrolloComercial")
                {//TRAE SOLO LO DE SU ZONA
                    Usuario actual = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
                    string z = actual.ZonaPertenece.zona;
                    var listaDistribuidores = new XPQuery<Usuario>(session).Where(c => c.ZonaPertenece.zona == z && c.TipoUsuario == TipoUsuario.Distribuidor);

                    var queryD = from us in listaDistribuidores
                             join ds in distribuidores on us.UserName equals ds.nombredist
                             select new { Zona = ds.zonastr,
                                 Nombre = us.Nombre,
                                 UserName = ds.nombredist,
                                 Jefe = us.Jefe.Nombre,
                                 Resultado = ds.profesionalizacion.ToString("0.00"),
                                 Profesionalizacion = ds.nivelAct,
                                 NivelActividades = ds.nivelActividades,
                                 Cumplimiento = ds.cumplimientoNivel.ToString("0.00"),
                                 ProductosServicios = ds.avanceProd,
                                 Infraestructura = ds.avanceInfra,
                                 Administracion = ds.avanceAdministracion,
                                 Ejecucion = ds.avanceEjecucion,
                                 Planeacion = ds.avancePlaneacion };
                ASPxGridView1.DataSource = queryD.ToList();
               }          
                else
                {
                    var usuarios = new XPQuery<Usuario>(session).Where(x=>x.TipoUsuario==TipoUsuario.Distribuidor);
                  /*   var quser = (from dx in usuarios
                            select new { Nombre = dx.UserName }).ToList();
                var qdist = (from dx in distribuidores
                             select new { Nombre = dx.nombredist }).ToList();
                int cersa = qdist.Count(x => x.Nombre == "ERSA");
                var kk = quser.Except(qdist);
                var kq = qdist.Except(quser);
                List < Usuario > lus = usuarios.ToList();
                List<mdl_distribuidor> ldi = distribuidores.ToList();
                int usus = usuarios.Count();
                int diss = distribuidores.Count();*/
                    var sqlDistribuidores= from us in usuarios
                         join ds in distribuidores on us.UserName equals ds.nombredist
                         select new { Zona = ds.zonastr, Nombre = us.Nombre, UserName = ds.nombredist, Jefe = us.Jefe.Nombre, Resultado = ds.profesionalizacion.ToString("0.00"), Profesionalizacion = ds.nivelAct, NivelActividades = ds.nivelActividades, Cumplimiento = ds.cumplimientoNivel.ToString("0.00"), ProductosServicios = ds.avanceProd, Infraestructura = ds.avanceInfra, Administracion = ds.avanceAdministracion, Ejecucion = ds.avanceEjecucion, Planeacion = ds.avancePlaneacion };

                /*    var sqlDistribuidores = from ds in distribuidores
                                            join us in usuarios on ds.nombredist equals us.UserName
                                            select new { Zona = ds.zonastr, Nombre = us.Nombre, UserName = ds.nombredist, Jefe = us.Jefe.Nombre, Resultado = ds.profesionalizacion, Profesionalizacion = ds.nivelAct, NivelActividades = ds.nivelActividades, Cumplimiento = ds.cumplimientoNivel.ToString("0.00"), ProductosServicios = ds.avanceProd, Infraestructura = ds.avanceInfra, Administracion = ds.avanceAdministracion, Ejecucion = ds.avanceEjecucion, Planeacion = ds.avancePlaneacion };
*/
                ASPxGridView1.DataSource = sqlDistribuidores.ToList();
                }
               

                ASPxGridView1.AutoGenerateColumns = true;
                ASPxGridView1.KeyFieldName = "idactplan";
                ASPxGridView1.DataBind();
                ASPxGridView1.EnableRowsCache = true;
                ASPxGridViewExporter1.ReportHeader = "Tablero 1";
                ASPxGridViewExporter1.GridViewID = "ASPxGridView1";
                ASPxGridViewExporter1.FileName = "EDC.Tab1_" + DateTime.Today.ToShortDateString();        
           
          
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