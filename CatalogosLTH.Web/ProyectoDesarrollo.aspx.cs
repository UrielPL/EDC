using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class ProyectoDesarrollo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string permiso = Util.getPermiso().ToString();
            string username = Util.getusuario();

            Session["username"] = username;
            Session["permiso"] = permiso;
        }

        [WebMethod]
        public static string traeFiles()
        {
            DevExpress.Xpo.Session session = Util.getsession();

            string permiso = Util.getPermiso().ToString();
            string username = Util.getusuario();

            if (permiso == "Distribuidor")
            {
                var listaArchivos = new XPQuery<ProyectoDesarrolloFiles>(session).Where(x => x.autor == username).ToList();

                listaArchivos = listaArchivos.OrderByDescending(x => x.fechaCargada).ToList();

                if (listaArchivos.Count() > 0)
                {
                    List<pdfs> lista = new List<pdfs>();
                    foreach (var item in listaArchivos)
                    {
                        pdfs obj = new pdfs();
                        obj.id = item.Id.ToString();
                        obj.nombre = item.nombreFile;
                        obj.url = item.fileURL;
                        obj.fecha = item.fechaCargada.ToString("dd/MMMM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                        obj.autor = item.autor;

                        lista.Add(obj);
                    }

                    var json = JsonConvert.SerializeObject(lista);
                    return json;
                }

                return "-1";
            }

            if (permiso == "GerenteCuenta")
            {
                var usuario = new XPQuery<Usuario>(session).Where(x => x.UserName == username).FirstOrDefault();
                var dependientes = usuario.Dependientes.OrderByDescending(x => x.UserName).ToList();

                if (dependientes.Count() > 0)
                {
                    List<pdfs> lista = new List<pdfs>();
                    foreach (var dep in dependientes)
                    {
                        var listaArchivos = new XPQuery<ProyectoDesarrolloFiles>(session).Where(x => x.autor == dep.UserName).ToList();

                        if (listaArchivos.Count() > 0)
                        {
                            listaArchivos = listaArchivos.OrderByDescending(x => x.fechaCargada).ToList();
                            foreach (var item in listaArchivos)
                            {
                                pdfs obj = new pdfs();
                                obj.id = item.Id.ToString();
                                obj.nombre = item.nombreFile;
                                obj.url = item.fileURL;
                                obj.fecha = item.fechaCargada.ToString("dd/MMMM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                                obj.autor = item.autor;

                                lista.Add(obj);
                            }
                            
                        }

                    }

                    var json = JsonConvert.SerializeObject(lista);
                    return json;
                }
            }

            if (permiso == "Admin")
            {
                List<pdfs> lista = new List<pdfs>();

                var listaArchivos = new XPQuery<ProyectoDesarrolloFiles>(session).ToList();

                if (listaArchivos.Count() > 0)
                {
                    listaArchivos = listaArchivos.OrderByDescending(x => x.fechaCargada).ToList();
                    foreach (var item in listaArchivos)
                    {
                        pdfs obj = new pdfs();
                        obj.id = item.Id.ToString();
                        obj.nombre = item.nombreFile;
                        obj.url = item.fileURL;
                        obj.fecha = item.fechaCargada.ToString("dd/MMMM/yyyy", CultureInfo.CreateSpecificCulture("es-MX"));
                        obj.autor = item.autor;

                        lista.Add(obj);
                    }

                    var json = JsonConvert.SerializeObject(lista);
                    return json;
                }
                
            }


            return "-1";
        }

        [WebMethod]
        public static string guardaArchivo(string imgUrl, string name)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string permiso = Util.getPermiso().ToString();
            string username = Util.getusuario();

            ProyectoDesarrolloFiles nPdf = new ProyectoDesarrolloFiles(session);

            nPdf.autor = username;
            nPdf.fechaCargada = DateTime.Now;
            nPdf.fileURL = imgUrl;
            nPdf.nombreFile = name;

            nPdf.Save();

            return "1";
        }
    }


    public class pdfs
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public string url { get; set; }
        public string fecha { get; set; }
        public string autor { get; set; }
    }
}