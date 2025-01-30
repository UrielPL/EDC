using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class NivelDistribuidor : System.Web.UI.Page
    {
        public static DevExpress.Xpo.Session session { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            session = Util.getsession();
            int idaud;
            if (Util.getrol() == "Distribuidor")
            {
                string usuario = Util.getusuario();

                mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist.ToString() == usuario);
                idaud = Util.getUltimaAuditoria(dist.iddistribuidor);
                ViewState["IdAuditoria"] = idaud;
                dropDist2.Items.Add(dist.nombredist);
                dropDist2.Enabled = false;
            }
            

            if (!Page.IsPostBack)
            {
                string nombreActual = Util.getusuario();
                string permiso = Util.getPermiso().ToString();
                string IdAuditoria = "";
                if(nombreActual == "alonso.sierra.siller@clarios.com")
                {
                    permiso = "Admin";
                }
                List<Usuario> listaDistribuidores = new List<Usuario>();
                btnGenKardex.Visible = false;
                btnGenKardex.Enabled = false;

                if(Util.getrol() == "DC Administrativo")
                {
                    permiso = "DCAdmin";
                }
              

                switch (permiso)
                {
                    case "GerenteCuenta":
                        listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName == nombreActual).ToList();
                        btnGenKardex.Visible = true;
                        btnGenKardex.Enabled = true;
                        break;
                    case "GerenteDesarrolloComercial":
                        listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.Jefe.UserName == nombreActual).ToList();
                        break;
                    case "Admin":
                        listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
                        break;
                    case "DCAdmin":
                        listaDistribuidores = new XPQuery<Usuario>(session)
                            .FirstOrDefault(x => x.UserName == nombreActual)
                            .DistribuidoresSupervisa.ToList();

                        btnLista.Enabled = false;
                        btnLista.Visible = false;
                        break;
                    case "GerenteVenta":
                        Usuario u = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
                        string z = u.ZonaPertenece.zona;
                        listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.ZonaPertenece.zona == z && x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
                        break;
                }

                listaDistribuidores = listaDistribuidores.OrderBy(x => x.UserName).ToList();
                foreach (var itemDist in listaDistribuidores)
                {
                    dropDist2.Items.Add(itemDist.UserName.ToString());
                }
                dropDist2.SelectedIndex = 0;


                if (dropDist2.SelectedItem != null)
                {
                    mdl_distribuidor distribuidorS = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == dropDist2.SelectedItem.Value.ToString());
                    if (distribuidorS != null) { IdAuditoria = Util.getUltimaAuditoria(distribuidorS.iddistribuidor) + ""; }
                }
                if (IdAuditoria != "") { ViewState["IdAuditoria"] = IdAuditoria; }
            }
        }

        protected void dropDist2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            mdl_distribuidor distribuidorS = new XPQuery<mdl_distribuidor>(session)
                .FirstOrDefault(x => x.nombredist == dropDist2.SelectedItem.Value.ToString());
            string IdAuditoria = Util.getUltimaAuditoria(distribuidorS.iddistribuidor) + "";
            ViewState["IdAuditoria"] = IdAuditoria;
        }

        protected void ASPxButton1_Click(object sender, EventArgs e)
        {
            int[] meses = new int[12] { 10, 11, 12, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            int mesActual = DateTime.Now.Month;
            int orden = Array.IndexOf(meses, mesActual) + 1;// se suma uno porque el index inicial es 0 y el orden en la BD empieza en 1
            int year = (mesActual > 9) ? DateTime.Now.Year + 1 : DateTime.Now.Year;

            mdl_Kardex newKardex = new mdl_Kardex(session);
            mdl_RegistroMensual tempNivel = new XPQuery<mdl_RegistroMensual>(session)
                .Where(x => x.Distribuidor.nombredist == dropDist2.SelectedItem.Text &&
                x.Periodo.Periodo == year && 
                x.orden == orden)
                .OrderByDescending(x=>x.IdRegistro).FirstOrDefault();

            newKardex.Profesionalizacion = tempNivel.resultado;
            newKardex.ActTerminadas = actTerminadas();
            newKardex.ActObjetivo = actObj();
            newKardex.ProfesionalizacionObjetivo = tempNivel.ObjetivoProf;
            newKardex.AvanceProf = tempNivel.resultado - tempNivel.ObjetivoProf;
            newKardex.AvanceAct = tempNivel.terminadas - tempNivel.ObjetivoAct;
            newKardex.Distribuidor = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x=>x.iddistribuidor == tempNivel.Distribuidor.iddistribuidor);
            newKardex.EjecutivoDC = tempNivel.Distribuidor.nombreusuario;
            newKardex.FechaVisita = DateTime.Now;
            newKardex.GerenteCuenta = Util.getusuario();
            newKardex.PilarAdministracion = tempNivel.Distribuidor.avanceAdministracion;
            newKardex.PilarEjecucion = tempNivel.Distribuidor.avanceEjecucion;
            newKardex.PilarInfraestructura = tempNivel.Distribuidor.avanceInfra;
            newKardex.PilarPlaneacion = tempNivel.Distribuidor.avancePlaneacion;
            newKardex.PilarPS = tempNivel.Distribuidor.avanceProd;
            ActividadUsuario ultAcceso = new XPQuery<ActividadUsuario>(session).OrderByDescending(x=>x.Fecha).FirstOrDefault(x => x.Usuario.UserName == tempNivel.Distribuidor.nombredist);
            newKardex.UltAcceso = ultAcceso.Fecha;
            newKardex.DiasSinAccesar = (DateTime.Now - newKardex.UltAcceso).Days;
            newKardex.ActRevisadasEjec = "";
            newKardex.AcuerdosRevision = "";
            using (UnitOfWork uow = new UnitOfWork())
            {
                newKardex.Save();
                uow.CommitChanges();
            }
            Response.Redirect(Page.ResolveClientUrl("/Kardex.aspx?idKardex=" + newKardex.Oid));
        }

        protected void btnLista_Click(object sender, EventArgs e)
        {
            Response.Redirect(Page.ResolveClientUrl("/ListaKardex.aspx?nombreDist=" + dropDist2.SelectedItem.Text));
        }

        public int actTerminadas()
        {
            int mesActual = DateTime.Now.Month;
            int year = (mesActual > 9) ? DateTime.Now.Year + 1 : DateTime.Now.Year;

            return new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.nombredist == dropDist2.SelectedItem.Text && x.Periodo.Periodo == year).Sum(x => x.terminadas);
        }

        public int actObj()
        {
            int mesActual = DateTime.Now.Month;
            int year = (mesActual > 9) ? DateTime.Now.Year + 1 : DateTime.Now.Year;

            return new XPQuery<mdl_RegistroMensual>(session).Where(x => x.Distribuidor.nombredist == dropDist2.SelectedItem.Text && x.Periodo.Periodo == year).Sum(x => x.ObjetivoAct);
        }
    }
}
