using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("v_Pilar")]
    [XafDefaultProperty("Nombre")]
    public class v_Pilar : XPLiteObject
    {
        
        public v_Pilar() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public v_Pilar(Session session) : base(session)
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

        [Association("Pilar-Areas")]
        public XPCollection<v_Area> Areas
        {
            get
            {
                return GetCollection<v_Area>("Areas");
            }
        }

        public string Nombre { get; set; }
        public double Valor { get; set; }

    }

}