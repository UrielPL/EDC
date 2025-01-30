using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("NuevaAuditoriaDetalle")]
    [XafDefaultProperty("Id")]
    public class NuevaAuditoriaDetalle : XPLiteObject
    {
        public NuevaAuditoriaDetalle() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public NuevaAuditoriaDetalle(Session session) : base(session)
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

        private NuevaAuditoria auditoria;
        [Association("NuevaAuditoria-NuevaAuditoriaDet")]
        [XafDisplayName("Auditoria")]
        public NuevaAuditoria Auditoria
        {
            get { return auditoria; }
            set { SetPropertyValue("Auditoria", ref auditoria, value); }
        }

        public v_Punto Punto { get; set; }
        public bool Resultado { get; set; }

        [XafDisplayName("No Aplica")]
        public bool n_a { get; set; }

        [Size(SizeAttribute.Unlimited)]
        public string Comentarios { get; set; }
        


    }

}