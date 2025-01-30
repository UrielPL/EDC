using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDefaultProperty("nombreniv")]
    [XafDisplayName("Nivel")]
    public class mdl_nivel : XPLiteObject
    {
        public mdl_nivel(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        [Key(true)]
        public int idnivel { get; set; }

        public string nombreniv { get; set; }

        [Association("CatNivel-Nivel")]
        public XPCollection<mdl_catnivel> CatNivel
        {
            get
            {
                return GetCollection<mdl_catnivel>("CatNivel");
            }
        }

        [Association("Nivel-Actividades")]
        public XPCollection<mdl_actividad> Actividad
        {
            get
            {
                return GetCollection<mdl_actividad>("Actividad");
            }
        }

        [Association("Nivel-PondNivel")]
        public XPCollection<mdl_pondnivel> PondNivel
        {
            get
            {
                return GetCollection<mdl_pondnivel>("PondNivel");
            }
        }

        [Association("Nivel-Puntos")]
        public XPCollection<mdl_punto> Punto
        {
            get
            {
                return GetCollection<mdl_punto>("Punto");
            }
        }

        [Association("Nivel - ActividadesNuevas")]
        public XPCollection<ActividadNueva> ActividadNueva
        {
            get
            {
                return GetCollection<ActividadNueva>("ActividadNueva");
            }
        }

        [Association("Nivel - ActividadesKpi")]
        public XPCollection<ActividadKpi> ActividadKpi
        {
            get
            {
                return GetCollection<ActividadKpi>("ActividadKpi");
            }
        }


    }

}