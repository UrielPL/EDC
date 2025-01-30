using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Punto")]
    [XafDefaultProperty("texto")]
    public class mdl_punto : XPLiteObject
    {
        public mdl_punto(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        [Key(true)]
        public int idpunto { get; set; }
        public string texto { get; set; }

        private mdl_pilar idpilar;
        [Association("Pilar-Puntos")]
        [XafDisplayName("Pilar")]
        public mdl_pilar Idpilar
        {
            get { return idpilar; }
            set { SetPropertyValue("Idpilar", ref idpilar, value); }
        }

        private mdl_nivel idnivel;
        [Association("Nivel-Puntos")]
        [XafDisplayName("Nivel")]
        public mdl_nivel Idnivel
        {
            get { return idnivel; }
            set { SetPropertyValue("Idnivel", ref idnivel, value); }
        }

        private mdl_tipoauditoria idtipoaud;
        [Association("TipoAuditoria-Puntos")]
        [XafDisplayName("Tipo Auditoria")]
        public mdl_tipoauditoria Idtipoaud
        {
            get { return idtipoaud; }
            set { SetPropertyValue("Idtipoaud", ref idtipoaud, value); }
        }

        public string clavepunto { get; set; }


        [Association("AuditDetalle-Puntos")]
        [XafDisplayName("Auditoria Detalle")]
        public XPCollection<mdl_audidet> idaudidet
        {
            get
            {
                return GetCollection<mdl_audidet>("idaudidet");
            }
        }

        [Association("Punto-Actividad")]
        public XPCollection<mdl_actividad> Actividades
        {
            get
            {
                return GetCollection<mdl_actividad>("Actividades");
            }
        }
        [Association("Puntos-Areas")]
        public XPCollection<mdl_Area> Areas
        {
            get
            {
                return GetCollection<mdl_Area>("Areas");
            }
        }



    }

}