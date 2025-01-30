using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;

namespace CatalogosLTH.Module.BusinessObjects
{


    [DefaultClassOptions]
    [XafDisplayName("v_Area")]
    [XafDefaultProperty("Nombre")]
    public class v_Area : XPLiteObject
    {
        public v_Area() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public v_Area(Session session) : base(session)
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

        private v_Pilar pilar;
        [Association("Pilar-Areas")]
        public v_Pilar Pilar
        {
            get { return pilar; }
            set { SetPropertyValue("Pilar", ref pilar, value); }
        }
        public string Nombre { get; set; }
        public double Valor { get; set; }


        [Association("Area-Subtemas")]
        public XPCollection<v_Subtema> Subtemas
        {
            get
            {
                return GetCollection<v_Subtema>("Subtemas");
            }
        }
    }

}