using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("CatPilar")]
    [XafDefaultProperty("idcatpilar")]
    public class mdl_catpilar : XPLiteObject
    {
        public mdl_catpilar(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        [Key(true)]
        public int idcatpilar { get; set; }

        private mdl_pilar idpilar;
        [Association("CatPilar-Pilar")]
        [XafDisplayName("TipoAuditoria")]
        public mdl_pilar Idpilar
        {
            get { return idpilar; }
            set { SetPropertyValue("Idpilar", ref idpilar, value); }
        }

        private mdl_catnivel idcatnivel;
        [Association("CatPilar-CatNivel")]
        [XafDisplayName("TipoAuditoria")]
        public mdl_catnivel Idcatnivel
        {
            get { return idcatnivel; }
            set { SetPropertyValue("Idcatnivel", ref idcatnivel, value); }
        }

        public int ponderacion { get; set; }

    }

}