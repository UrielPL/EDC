using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("PondPilar")]
    [XafDefaultProperty("pondpilar")]
    public class mdl_pondpilar : XPLiteObject
    {
        public mdl_pondpilar(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        [Key(true)]
        public int pondpilar { get; set; }

        private mdl_pilar idpilar;
        [Association("Pilar-PondPilares")]
        [XafDisplayName("Pilar")]
        public mdl_pilar Idpilar
        {
            get { return idpilar; }
            set { SetPropertyValue("Idpilar", ref idpilar, value); }
        }

        private mdl_catnivel idcatnivel;
        [Association("CatNivel-PondPilares")]
        [XafDisplayName("Catnivel")]
        public mdl_catnivel Idcatnivel
        {
            get { return idcatnivel; }
            set { SetPropertyValue("Idcatnivel", ref idcatnivel, value); }
        }

        public int ponderacion { get; set; }
        public int nopuntostotales { get; set; }

        private mdl_auditoria idaud;
        [Association("Auditoria-PondPilares")]
        [XafDisplayName("Auditoria")]
        public mdl_auditoria Idaud
        {
            get { return idaud; }
            set { SetPropertyValue("Idaud", ref idaud, value); }
        }

        public int avanceaud { get; set; }
        public int avancedc { get; set; }
        public double avancetotal { get; set; }
        public int completadasaud { get; set; }
        public int completadasact { get; set; }

        



    }

}