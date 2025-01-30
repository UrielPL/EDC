using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Pilar")]
    [XafDefaultProperty("nombrepil")]

    public class mdl_pilar : XPLiteObject
    {
        public mdl_pilar(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        [Key(true)]
        public int idpilar { get; set; }

        public string nombrepil { get; set; }

         [Association("Pilar-Actividades")]
        public XPCollection<mdl_actividad> Actividad
        {
            get
            {
                return GetCollection<mdl_actividad>("Actividad");
            }
        }

        [Association("CatPilar-Pilar")]
        public XPCollection<mdl_catpilar> CatPilar
        {
            get
            {
                return GetCollection<mdl_catpilar>("CatPilar");
            }
        }

        [Association("Pilar-PondPilares")]
        public XPCollection<mdl_pondpilar> PondPilar
        {
            get
            {
                return GetCollection<mdl_pondpilar>("PondPilar");
            }
        }
        [Association("Pilar-Puntos")]
        public XPCollection<mdl_punto> Punto
        {
            get
            {
                return GetCollection<mdl_punto>("Punto");
            }
        }

        [Association("Pilar - ActividadesNuevas")]
        public XPCollection<ActividadNueva> ActividadNueva
        {
            get
            {
                return GetCollection<ActividadNueva>("ActividadNueva");
            }
        }

        [Association("Pilar - ActividadesKpi")]
        public XPCollection<ActividadKpi> ActividadKpi
        {
            get
            {
                return GetCollection<ActividadKpi>("ActividadKpi");
            }
        }
    }

}