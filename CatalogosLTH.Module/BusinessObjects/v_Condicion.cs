using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]

    [XafDisplayName("v_Condicion")]
    [XafDefaultProperty("Nombre")]
    public class v_Condicion : XPLiteObject
    {
        public v_Condicion() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public v_Condicion(Session session) : base(session)
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

        private v_Punto punto;
        [Association("Punto-Condiciones")]
        public v_Punto Punto
        {
            get { return punto; }
            set { SetPropertyValue("Punto", ref punto, value); }
        }

        public string Nombre { get; set; }
        public double Valor { get; set; }
        public bool R { get; set; }

        [Size(SizeAttribute.Unlimited)]
        public string NombreCondicion { get; set; }

    }

}