using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("CatNivel")]
    [XafDefaultProperty("idcatnivel")]
    public class mdl_catnivel : XPLiteObject
    {
        public mdl_catnivel(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        [Key(true)]
        public int idcatnivel { get; set; }

        private mdl_nivel idnivel;
        [Association("CatNivel-Nivel")]
        [XafDisplayName("Nivel")]
        public mdl_nivel Idnivel
        {
            get { return idnivel; }
            set { SetPropertyValue("Idnivel", ref idnivel, value); }
        }

        private mdl_tipoauditoria idtipoaud;
        [Association("CatNivel-TipoAuditoria")]
        [XafDisplayName("TipoAuditoria")]
        public mdl_tipoauditoria Idtipoaud
        {
            get { return idtipoaud; }
            set { SetPropertyValue("Idtipoaud", ref idtipoaud, value); }
        }

        public int ponderacion { get; set; }

        [Association("CatNivel-PondPilares")]
        public XPCollection<mdl_pondpilar> PondPilar
        {
            get
            {
                return GetCollection<mdl_pondpilar>("PondPilar");
            }
        }


            [Association("CatPilar-CatNivel")]
        public XPCollection<mdl_catpilar> CatPilar
        {
            get
            {
                return GetCollection<mdl_catpilar>("CatPilar");
            }
        }

      
    }

}