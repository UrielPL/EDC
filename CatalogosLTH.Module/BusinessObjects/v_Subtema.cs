using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("v_Subtema")]
    [XafDefaultProperty("Nombre")]
    public class v_Subtema : XPLiteObject
    {
        public v_Subtema() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public v_Subtema(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
        }
        [Key(true)]
        public int Id { get; set; }

        private v_Area area;
        [Association("Area-Subtemas")]
        public v_Area Area
        {
            get { return area; }
            set { SetPropertyValue("Area", ref area, value); }
        }


        [Association("Subtema-Puntos")]
        public XPCollection<v_Punto> Puntos
        {
            get
            {
                return GetCollection<v_Punto>("Puntos");
            }
        }

        public string Nombre { get; set; }
        public double Valor { get; set; }

    }

}