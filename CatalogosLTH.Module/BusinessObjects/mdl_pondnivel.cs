using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("PondNivel")]
    [XafDefaultProperty("idpondnivel")]
    public class mdl_pondnivel : XPLiteObject
    {
        public mdl_pondnivel(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        [Key(true)]
        public int idpondnivel { get; set; }

        private mdl_nivel nivelID;
        [Association("Nivel-PondNivel")]
        [XafDisplayName("Nivel")]
        public mdl_nivel NivelID
        {
            get { return nivelID; }
            set { SetPropertyValue("NivelID", ref nivelID, value); }
        }

        private mdl_tipoauditoria idtipoaud;
        [Association("TipoAuditoria-PondNivel")]
        [XafDisplayName("Tipo Auditoria")]
        public mdl_tipoauditoria Idtipoaud
        {
            get { return idtipoaud; }
            set { SetPropertyValue("Idtipoaud", ref idtipoaud, value); }
        }

        private mdl_auditoria idaud;
        [Association("Auditoria-PondNivel")]
        [XafDisplayName("Auditoria")]
        public mdl_auditoria Idaud
        {
            get { return idaud; }
            set { SetPropertyValue("Idaud", ref idaud, value); }
        }

        public int ponderacion { get; set; }
        public double profesionalizacion { get; set; }








    }

}