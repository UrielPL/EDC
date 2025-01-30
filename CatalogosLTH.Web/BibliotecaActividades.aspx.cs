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
    public partial class BibliotecaActividades : System.Web.UI.Page
    {
        public List<actividad> lista { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {

            DevExpress.Xpo.Session session = Util.getsession();

            var acts = new XPQuery<v_Actividad>(session).Where(x => x.Numero != null).ToList();

            acts = acts.OrderBy(x => int.Parse(x.Numero)).ToList();
            lista = new List<actividad>();

            if(acts.Count() > 0)
            {
                foreach(var item in acts)
                {
                    actividad obj = new actividad();

                    obj.id = item.Id;
                    obj.code = "ACT-" + item.Numero.ToString().PadLeft(3,'0') + "-" + item.Letra;
                    obj.nombre = item.Nombre;
                    obj.pilar = item.Punto.Subtema != null ? item.Punto.Subtema.Area.Pilar.Nombre : "-";
                    obj.nivel = item.NivelActividad.ToString();

                    lista.Add(obj);

                    
                }

                lista = lista.OrderBy(x => x.code).ToList();
            }

        }


    }

    public class actividad
    {
        public int id { get; set; }
        public string code { get; set; }
        public string nombre { get; set; }
        public string pilar { get; set; }
        public string nivel { get; set; }
    }

}