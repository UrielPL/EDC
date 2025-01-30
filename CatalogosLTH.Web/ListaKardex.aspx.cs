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
    public partial class ListaKardex : System.Web.UI.Page
    {
        public static Usuario user { get; set; }
        public string nombreDist { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            nombreDist = Request.QueryString["nombreDist"];
            List<mdl_Kardex> lstKardex = new List<mdl_Kardex>();


            if (Util.getrol() == "Administrator" || Util.getusuario() == "alonso.sierra.siller@clarios.com") {
                lstKardex = new XPQuery<mdl_Kardex>(session).ToList();
            } else if (Util.getrol() == "Distribuidor") {
                lstKardex = new XPQuery<mdl_Kardex>(session).Where(x => x.Distribuidor.nombredist == Util.getusuario()).ToList();
            } else if (Util.getrol() == "Evaluador" && Util.getusuario() != "alonso.sierra.siller@clarios.com") {
                lstKardex = new XPQuery<mdl_Kardex>(session).Where(x => x.GerenteCuenta == Util.getusuario()).ToList();
            }
            var data = from aa in lstKardex
                       where aa.Distribuidor.nombredist == nombreDist
                       orderby aa.FechaVisita descending
                       select new
                       {
                           Clave = aa.Oid,
                           aa.AvanceAct,
                           aa.AvanceProf,
                           aa.FechaVisita,
                           aa.GerenteCuenta,
                           aa.UltAcceso,
                           Distribuidor = aa.Distribuidor.nombredist
                       };
            grVWkardex.KeyFieldName = "Clave";
            grVWkardex.DataSource = data;
            grVWkardex.EnableRowsCache = true;
            grVWkardex.DataBind();

            foreach (var itemDist in lstKardex.Select(x=>x.Distribuidor).Distinct())
            {
                listDist.Items.Add(itemDist.nombredist, itemDist.iddistribuidor);
            }
        }
        protected void listDist_SelectedIndexChanged(object sender, EventArgs e)
        {
            nombreDist = listDist.SelectedItem.Text;
            Response.Redirect("/ListaKardex.aspx?nombreDist=" + nombreDist);
        }
    }
}