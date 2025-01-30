using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Web;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class CatalogoAuditorias : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            var auditorias = new XPQuery<mdl_auditoria>(session).Where(c => c.Iddistribuidor != null).ToList();
            
            var distribuidores = new XPQuery<mdl_distribuidor>(session).ToList();
            string nom = Util.getusuario();
            TipoUsuario tipoUsuario = Util.getPermiso();
            List<Usuario> listaDistribuidoresDependientes = new List<Usuario>();

            if (tipoUsuario==TipoUsuario.GerenteCuenta)
            {
                listaDistribuidoresDependientes = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName.ToString() == Util.getusuario()).ToList();//n
            }
            else if (tipoUsuario == TipoUsuario.GerenteDesarrolloComercial)
            {
                listaDistribuidoresDependientes = new XPQuery<Usuario>(session).Where(x => x.Jefe.Jefe.UserName.ToString() == Util.getusuario()).ToList();
                //List<Usuario> distribuidoresDependientes = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName.ToString() == Util.getusuario()).ToList();//n
            }
            else if (tipoUsuario==TipoUsuario.Distribuidor)
            {
                listaDistribuidoresDependientes = new XPQuery<Usuario>(session).Where(x => x.UserName.ToString() == Util.getusuario()).ToList();
            }
            else if (tipoUsuario==TipoUsuario.Admin)
            {
                listaDistribuidoresDependientes = new XPQuery<Usuario>(session).Where(x => x.TipoUsuario == TipoUsuario.Distribuidor).ToList();
            }

            

            List<mdl_auditoria> listaAuditorias = auditorias;
            List<mdl_distribuidor> listaDistribuidor = distribuidores;

            var data1 = from dc in distribuidores
                        orderby dc.nombredist ascending
                        join dd in listaDistribuidoresDependientes on dc.nombredist equals dd.UserName
                        select new { IdDistribuidor = dc.iddistribuidor, Nombre = dc.nombredist };

            var data = from au in listaAuditorias
                       orderby au.idaud ascending
                       join dc in data1 on au.Iddistribuidor.iddistribuidor equals dc.IdDistribuidor
                       select new { IdAuditoria = au.idaud, Distribuidor = au.Iddistribuidor.nombredist, Tipo = au.Idtipoaud.Descripcion, Fecha = au.fechaap, FechaFinal = au.fechaterm, Estatus = au.estatus };





            /* var data = from ba in listaAuditorias
                        orderby ba.idaud ascending
                        select new { IdAuditoria = ba.idaud, Distribuidor = ba.Iddistribuidor.nombredist, Tipo = ba.Idtipoaud.Descripcion, Fecha=ba.fechaap, FechaFinal=ba.fechaterm, Estatus=ba.estatus};
                        */

            ASPxGridView1.DataSource = data.ToList();
            ASPxGridView1.AutoGenerateColumns = true;
            ASPxGridView1.KeyFieldName = "IdAuditoria";
            ASPxGridView1.DataBind();                     
        }

        protected void btnSeleccionar_Click(object sender, EventArgs e)
        {
            int indice = ASPxGridView1.FocusedRowIndex;
            string codigo = ASPxGridView1.GetRowValues(indice, new string[] { "IdAuditoria" }).ToString();
            Response.Redirect("~/nivel.aspx?IdAuditoria=" + Cryptography.Encriptar(codigo));
        }

       
    }
}