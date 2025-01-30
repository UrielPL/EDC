using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Web;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class ActividadesUsuario : System.Web.UI.Page
    {
        public static DevExpress.Xpo.Session session { get; set; }
        public static List<ActividadUsuario> lstActs = new List<ActividadUsuario>();
        public static List<ActividadUsuario> lstActsTot = new List<ActividadUsuario>();
        public static List<string> cantAct = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                session = Util.getsession();
                cantAct.Clear();
                lstActsTot = new XPQuery<ActividadUsuario>(session).ToList();
                selAnio.Items.Clear();
                selUser.Items.Clear();
                foreach (var item in lstActsTot.Select(x => x.Fecha.Year).Distinct())
                {
                    selAnio.Items.Add(new ListEditItem(item.ToString(),item.ToString()));
                }
                foreach (var item in lstActsTot.Select(x=>x.Usuario.Nombre).Distinct())
                {
                    selUser.Items.Add(new ListEditItem(item,item));
                }

                selAnio.SelectedIndex = 0;
                selUser.SelectedIndex = 0;

                lstActs = new XPQuery<ActividadUsuario>(session).Where(x => x.Usuario.Nombre == selUser.Value.ToString() && x.Fecha.Year.ToString() == selAnio.Value.ToString()).ToList();
                lstActs = lstActs.OrderBy(x=>x.Fecha).ToList();
                int cont = 0, diaActual=99;
                for(int i=0; i < 12; i++) {
                    foreach (var item in lstActs)
                    {
                        if(item.Fecha.Month == i)
                        {
                            if (diaActual != item.Fecha.Day)
                            {
                                cont++;
                                diaActual = item.Fecha.Day;
                            }
                        }
                    }
                    cantAct.Add(cont.ToString());
                    cont = 0;
                }
            }
        }

        protected void selAnio_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstActs = new XPQuery<ActividadUsuario>(session).Where(x => x.Usuario.Nombre == selUser.Value.ToString() && x.Fecha.Year.ToString() == selAnio.Value.ToString()).ToList();
            cantAct.Clear();
            lstActs = lstActs.OrderBy(x => x.Fecha).ToList();
            int cont = 0, diaActual = 99;
            for (int i = 0; i < 12; i++)
            {
                foreach (var item in lstActs)
                {
                    if (item.Fecha.Month == i)
                    {
                        if (diaActual != item.Fecha.Day)
                        {
                            cont++;
                            diaActual = item.Fecha.Day;
                        }
                    }
                }
                cantAct.Add(cont.ToString());
                cont = 0;
            }
        }

        protected void selUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstActs = new XPQuery<ActividadUsuario>(session).Where(x => x.Usuario.Nombre == selUser.Value.ToString() && x.Fecha.Year.ToString() == selAnio.Value.ToString()).ToList();
            cantAct.Clear();
            lstActs = lstActs.OrderBy(x => x.Fecha).ToList();
            int cont = 0, diaActual = 99;
            for (int i = 0; i < 12; i++)
            {
                foreach (var item in lstActs)
                {
                    if (item.Fecha.Month == i)
                    {
                        if (diaActual != item.Fecha.Day)
                        {
                            cont++;
                            diaActual = item.Fecha.Day;
                        }
                    }
                }
                cantAct.Add(cont.ToString());
                cont = 0;
            }
        }
    }
}