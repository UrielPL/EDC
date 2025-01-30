using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Auditoria Móvil General")]
    [XafDefaultProperty("IdAudMovG")]
    public class AuditoriaMovilGeneral : XPLiteObject
    {
        public AuditoriaMovilGeneral() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public AuditoriaMovilGeneral(Session session) : base(session)
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
        public int IdAudMovG { get; set; }
        public DateTime Fecha {get;set;}

        public Usuario Evaluador { get; set; }
        public mdl_distribuidor distribuidor { get; set; }

        public string ValidadoPor { get; set; }

        [Association("Aud-Detalle")]
        public XPCollection<mdl_AuditoriaMovil> AudMDet
        {
            get
            {
                return GetCollection<mdl_AuditoriaMovil>("AudMDet");
            }
        }
    }

}