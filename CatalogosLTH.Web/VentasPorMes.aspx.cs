using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class VentasPorMes : System.Web.UI.Page
    {
        public static DevExpress.Xpo.Session session { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            session = Util.getsession();
        }

        [WebMethod]
        public static byte[] getfile(string mes, string anio)
        {
            var fileName = "Historial_Ventas.xls";
            var file = new FileInfo(fileName);

            int dias = DateTime.DaysInMonth(int.Parse(anio), int.Parse(mes));

            List<ventasDTO> lstVentas = new List<ventasDTO>();
            List<CentroServicio> lstCS = new XPQuery<CentroServicio>(session).ToList();

            foreach (var item in lstCS.OrderBy(x => x.Iddistribuidor.nombredist))
            {
                ventasDTO temp = new ventasDTO();
                temp.dist = item.Iddistribuidor.nombredist;
                temp.CS = item.Nombre;
                temp.idCS = item.Clave;
                temp.ventasMay = item.Iddistribuidor.VentaMayoreo.ToString();
                temp.ventasPromCS = item.ventaPromedio.ToString();
                for (int i = 0; i < dias; i++)
                {
                    var total = new XPQuery<VentaCS>(session).Where(x => x.Fecha.Month == DateTime.Now.Month && x.Fecha.Year == DateTime.Now.Year && x.Fecha.Day == i && x.CentroServicio == item.Nombre).ToList();
                    temp.totalDias.Add(total.Count() > 0 ? total.Sum(x => x.Venta) : 0);
                }
                lstVentas.Add(temp);
            }

            using (var package = new OfficeOpenXml.ExcelPackage(file))
            {
                ExcelWorksheet libroReporte = package.Workbook.Worksheets.Add("Ventas");
                libroReporte.Cells[1, 1].Value = "DC";
                libroReporte.Cells[1, 2].Value = "ID CS";
                libroReporte.Cells[1, 3].Value = "CENTRO DE SERVICIO";
                libroReporte.Cells[1, 4].Value = "VENTA PROM MAY";
                libroReporte.Cells[1, 5].Value = "VENTA PROM CS";
                for (int i = 1; i <= dias; i++)
                {
                    libroReporte.Cells[1, i + 5].Value = i.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
                }

                var rowCounter = 2;
                libroReporte.Column(3).Width = 15;

                foreach (var v in lstVentas)
                {
                    libroReporte.Cells[rowCounter, 1].Value = v.dist;
                    libroReporte.Cells[rowCounter, 2].Value = v.idCS;
                    libroReporte.Cells[rowCounter, 3].Value = v.CS;
                    libroReporte.Cells[rowCounter, 4].Value = v.ventasMay;
                    libroReporte.Cells[rowCounter, 5].Value = v.ventasPromCS;
                    for (int i = 0; i < dias; i++)
                    {
                        libroReporte.Cells[rowCounter, i + 6].Value = v.totalDias[i];
                    }

                    rowCounter++;
                }
                var s = package.GetAsByteArray();

                return s;
            }
        }
        public class ventasDTO
        {
            public string dist { get; set; }
            public string CS { get; set; }
            public string idCS { get; set; }
            public string ventasMay { get; set; }
            public string ventasPromCS { get; set; }
            public List<int> totalDias = new List<int>();
        }
    }
}