using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Auditoria Detalle")]
    [XafDefaultProperty("texto")]
    public class mdl_audidet : XPLiteObject
    {
        public mdl_audidet(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        
        [Key(true)]
        public int id { get; set; }

        /*[Association("AuditDetalle-Puntos")]
        [XafDisplayName("Puntos")]
        public XPCollection<mdl_punto> idpunto
        {
            get
            {
                return GetCollection<mdl_punto>("idpunto");
            }
        }*/

        private mdl_punto idpunto;
        [Association("AuditDetalle-Puntos")]
        [XafDisplayName("Punto")]
        public mdl_punto Idpunto
        {
            get { return idpunto; }
            set { SetPropertyValue("Idpunto", ref idpunto, value); }
        }
        public string pilar
        {
            get
            {
                string v = "";
                if (this.Idpunto!=null)
                {
                    v = this.Idpunto.Idpilar.nombrepil;
                }
                return v;
            }
        }

        public string texto { get; set; }
        public DateTime fecha { get; set; }
        public int resultado { get; set; }

        private mdl_auditoria idaud;
        [Association("Auditoria-AuditDetalles")]
        [XafDisplayName("Auditoria")]
        public mdl_auditoria Idaud
        {
            get { return idaud; }
            set { SetPropertyValue("Idaud", ref idaud, value); }
        }

        

     
    }

}