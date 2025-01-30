using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{

    public class dtoVentaCentro
    {
        public int ventaPromedio { get; set; }
        public int venta { get; set; }
        public string clave { get; set; }
        public DateTime Fecha { get; set; }
        public string nombre { get; set; }

    }
    public class dtoArray
    {
        public int valorProm { get; set; }
        public int valor { get; set; }
        public string clave { get; set; }
    }
    public partial class CargaVenta : System.Web.UI.Page
    {
        public static Usuario user { get; set; }
        public static DevExpress.Xpo.Session session { get; set; }
        public static List<CentroServicio> lstCentros { get; set; }
        public static string NombreDist { get; set; }
        public static DateTime FechaSelected { get; set; }
        public static List<dtoVentaCentro>lstVentas{get;set;}

        public double mayoreo { get; set; }
        public double promedio { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                session = Util.getsession();
                //string userRol = Util.getrol();
                NombreDist = Util.getusuario();
                dtFecha.Date = DateTime.Today;
                var dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == NombreDist);
                   
                if (dist == null)
                {
                    if (NombreDist == "admin" || NombreDist == "alonso.sierra.siller@clarios.com")
                    {
                        Response.Redirect("~/VentasPorMes.aspx");
                    } else
                    {
                        Response.Redirect("~/mainpage.aspx");
                    }
                }
                    promedio = dist.VentaPromedio;
                    mayoreo = dist.VentaMayoreo;
                lstCentros = dist.CentrosDeServicio.ToList();
            }
            GetVentas();
        }

        public void GetVentas()
        {
            var dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == NombreDist);
            promedio = dist.VentaPromedio;//Venta Promedio de mayoreo
            mayoreo = dist.VentaMayoreo;

            FechaSelected = dtFecha.Date;//selecciona fecha
            var ventas = new XPQuery<VentaCS>(session).Where(c => c.Iddistribuidor.nombredist == NombreDist && c.Fecha.Date == dtFecha.Date.Date);

            if (ventas.Count() > 0)/// si ya hay alguna venta este dia
            {
                lstVentas = (from cs in lstCentros
                             join vts in ventas on cs.Clave equals vts.ClaveCentro
                             select new dtoVentaCentro { clave = cs.Clave, nombre = cs.Nombre, ventaPromedio=cs.ventaPromedio, venta = vts.Venta }).ToList();

                if (ventas.Where(c => c.ClaveCentro == "Mayoreo").Count() >= 1)
                {
                    lstVentas.Add(
                        new dtoVentaCentro
                        {
                            clave = "Mayoreo",
                            nombre = "Venta Mayoreo",
                            ventaPromedio= ventas.First(c => c.ClaveCentro == "Mayoreo").VentaPromedio,
                            venta = ventas.First(c => c.ClaveCentro == "Mayoreo").Venta
                        });
                }
                /*Agrega el valor de Mayoreo desde la tabla de Distribuidor*/
            }
            else
            {
                lstVentas = (from cs in lstCentros
                             select new dtoVentaCentro { clave = cs.Clave, nombre = cs.Nombre,ventaPromedio=cs.ventaPromedio, venta = 0 }).ToList();
            }
        }

        protected void dtFecha_DateChanged(object sender, EventArgs e)
        {
            var ventas = new XPQuery<VentaCS>(session).Where(c => c.Iddistribuidor.nombredist == NombreDist && c.Fecha.Date==dtFecha.Date.Date);
        }

        [WebMethod]
        // public static string sendValor(string Nombres, string ApellidoP, string ApellidoM, string Genero, string Fecha)
        public static bool sendValor(string Data)
        {
            string val = "";

            var VentasInput = JsonConvert.DeserializeObject<List<dtoArray>>
                           (Data,
                                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }
                           );

            try
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    var ventasCS = new XPQuery<VentaCS>(session).Where(c => c.Iddistribuidor.nombredist == NombreDist && c.Fecha.Date == FechaSelected);

                    if (ventasCS.Count() == 0)//EN CASO DE QUE NO HAYA REGISTROS DE VENTAS para ese dia y para es distribuidor
                    {
                        if (VentasInput.Count() > 0)//En caso de que se envien todos los valores
                        {
                            foreach (var item in VentasInput)
                            {
                                if(item.clave== "VentaMayoreoTotal")//valores del distribuidor [venta mayoreo promedio]
                                {
                                    mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == NombreDist);

                                    if (dist != null)
                                    {
                                        dist.VentaMayoreo = item.valorProm;
                                        dist.VentaPromedio = item.valor;
                                        dist.Save();
                                    }

                                }
                                else if (item.valor > 0)//valores de venta de centros de servicio diario incluye clave de mayoreo
                                {
                                    VentaCS nuevaVenta = new VentaCS(session);
                                    nuevaVenta.ClaveCentro = item.clave;
                                    nuevaVenta.VentaPromedio = item.valorProm;
                                    nuevaVenta.Venta = item.valor;
                                    nuevaVenta.Fecha = FechaSelected;
                                    nuevaVenta.Iddistribuidor = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(v => v.nombredist == NombreDist);

                                    CentroServicio cs = new XPQuery<CentroServicio>(session).FirstOrDefault(c => c.Clave == item.clave);
                                    if (cs != null)//Busca la clave del CS y modifica su ventaPromedio para dejarla estatica 
                                    {
                                        cs.ventaPromedio = item.valorProm;
                                        cs.Save();
                                    }
                                    if (item.clave == "Mayoreo") nuevaVenta.Mayoreo = true;
                                    nuevaVenta.Save();
                                }
                            }
                        }
                    }
                    else// EN CASO DE QUE SE MODIFIQUE UNA VENTA REGISTRADA
                    {
                        foreach (var v in VentasInput)
                        {
                            VentaCS updVenta = new XPQuery<VentaCS>(session).FirstOrDefault(x => x.ClaveCentro == v.clave && x.Fecha.Date == FechaSelected.Date);

                            if (v.clave == "VentaMayoreoTotal")
                            {
                                mdl_distribuidor dist = new XPQuery<mdl_distribuidor>(session).FirstOrDefault(x => x.nombredist == NombreDist);

                                if (dist != null)
                                {
                                    dist.VentaMayoreo = v.valorProm;
                                    dist.VentaPromedio = v.valor;
                                    dist.Save();
                                }

                            }
                            else if (updVenta != null)
                            {
                                CentroServicio cs = new XPQuery<CentroServicio>(session).FirstOrDefault(c => c.Clave == v.clave);
                                if (cs != null)//Busca la clave del CS y modifica su ventaPromedio para dejarla estatica 
                                {
                                    cs.ventaPromedio = v.valorProm;
                                    cs.Save();
                                }
                                updVenta.VentaPromedio = v.valorProm;
                                updVenta.Venta = v.valor;
                                updVenta.Save();
                            }

                        }
                    }

                   
                    uow.CommitChanges();
                }
            }
            catch (Exception e)
            {
                return false;
            }


            return true;
        }
    }
}