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
    public partial class reporte1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            var zonas = new XPQuery<mdl_zona>(session);
            string nombreActual = Util.getusuario();//Nombre usuario 
            string permiso = Util.getPermiso().ToString();//Permiso usuario
           
            if (permiso=="Distribuidor"||permiso== "GerenteCuenta"||permiso=="GerenteVenta")
            {
                //mdl_distribuidor d = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == nombreActual);
                Usuario u = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName == nombreActual);
                string z = u.ZonaPertenece != null ? u.ZonaPertenece.zona : "";
                int cont = 0;

                double[] prof = new double[1];
                string[] zona = new string[1];
                for (int i = 0; i < 1; i++)
                {
                    prof[i] = 0;
                }

                var dist = new XPQuery<mdl_distribuidor>(session).Where(x => x.zonastr == z&& x.EnDesarrollo == false).ToList();
               // var usuariosDist = new XPQuery<Usuario>(session).Where(p => p.TipoUsuario == TipoUsuario.Distribuidor);

                /**var distribuidores = from ud in usuariosDist
                                     join dis in dist on ud.UserName equals dis.nombredist
                                     where ud.Jefe!=null
                                     select new { Nombre = dis.nombredist, Nivel=dis.profesionalizacion, Zona=dis.zona};*/
                var distribuidores = from dis in dist
                                     select new { Nombre = dis.nombredist, Nivel = dis.profesionalizacion, Zona = dis.zonastr };


                //              var dist = new XPQuery<mdl_distribuidor>(session);


                var groupAverage = from r in distribuidores
                                   group r by new { r.Zona } into groups
                                   select new
                                   {
                                       Group = groups.Key.Zona,
                                       AverageValue = groups.Average(rec => rec.Nivel)
                                   };
                int registros= groupAverage.Count();
                for (int i = 0; i < registros; i++)
                {
                    prof[i] = groupAverage.ElementAt(i).AverageValue;
                    zona[i] = groupAverage.ElementAt(i).Group;
                }


                ViewState["profesionalizacion"] = prof;
                ViewState["nombrezona"] = zona;
            }
            else
            {
                int cont = 0;

                double[] prof = new double[zonas.Count()];
                string[] zona = new string[zonas.Count()];
                for (int i = 0; i < zonas.Count(); i++)
                {
                    prof[i] = 0;
                }

                var dist = new XPQuery<mdl_distribuidor>(session).Where(x=>x.EnDesarrollo==false).ToList();
                var usuariosDist = new XPQuery<Usuario>(session).Where(p => p.TipoUsuario == TipoUsuario.Distribuidor);

                /**var distribuidores = from ud in usuariosDist
                                     join dis in dist on ud.UserName equals dis.nombredist
                                     where ud.Jefe!=null
                                     select new { Nombre = dis.nombredist, Nivel=dis.profesionalizacion, Zona=dis.zona};*/
                var distribuidores = from dis in dist
                                     select new { Nombre = dis.nombredist, Nivel = dis.profesionalizacion, Zona = dis.zonastr };


                //              var dist = new XPQuery<mdl_distribuidor>(session);


                var groupAverage = from r in distribuidores
                                   group r by new { r.Zona } into groups
                                   select new
                                   {
                                       Group = groups.Key.Zona,
                                       AverageValue = groups.Average(rec => rec.Nivel)
                                   };

                for (int i = 0; i < prof.Length; i++)
                {
                    prof[i] = groupAverage.ElementAt(i).AverageValue;
                    zona[i] = groupAverage.ElementAt(i).Group;
                }


                ViewState["profesionalizacion"] = prof;
                ViewState["nombrezona"] = zona;
            }
           
        }
    }
}