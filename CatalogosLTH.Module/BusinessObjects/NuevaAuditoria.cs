using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("NuevaAuditoria")]
    [XafDefaultProperty("Id")]
    public class NuevaAuditoria : XPLiteObject
    {
        public NuevaAuditoria() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public NuevaAuditoria(Session session) : base(session)
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

        private mdl_distribuidor distribuidor;
        [Association("Distribuidor-NuevaAuditoria")]
        [XafDisplayName("Distribuidor")]
        public mdl_distribuidor Distribuidor
        {
            get { return distribuidor; }
            set { SetPropertyValue("Distribuidor", ref distribuidor, value); }
        }

        public DateTime Fecha { get; set; }
        public DateTime Fecha_Cierre { get; set; }
        public string status { get; set; }

        [XafDisplayName("Autor")]
        public Usuario User_Apertura { get; set; }

        [XafDisplayName("Calificacion Total")]
        public double calificacionTotal { get; set; }

        [XafDisplayName("Pilar Administracion")]
        public double calificacionAdmin { get; set; }

        [XafDisplayName("Pilar Ejecucion")]
        public double calificacionEjec { get; set; }

        [XafDisplayName("Pilar Infraestructura")]
        public double calificacionInfra { get; set; }

        [XafDisplayName("Pilar Planeacion")]
        public double calificacionPlan { get; set; }

        [XafDisplayName("Pilar Productos y Servicios")]
        public double calificacionPyS { get; set; }

        [XafDisplayName("Calificacion Final")]
        public double calificacionFinal { get; set; }

        private bool _autoAuditoria;
        public bool autoAuditoria
        {
            get { return _autoAuditoria; }
            set { SetPropertyValue("autoAuditoria", ref _autoAuditoria, value); }
        }

        private bool _Lubricantes;
        public bool Lubricantes
        {
            get { return _Lubricantes; }
            set { SetPropertyValue("Lubricantes", ref _Lubricantes, value); }
        }

        [Association("NuevaAuditoria-NuevaAuditoriaDet")]
        public XPCollection<NuevaAuditoriaDetalle> NuevaAuditoriaDet
        {
            get
            {
                return GetCollection<NuevaAuditoriaDetalle>("NuevaAuditoriaDet");
            }
        }

        [Association("NuevaAuditoria-NuevaAuditoriaActividad")]
        public XPCollection<NuevaAuditoriaActividad> NuevaAuditoriaAct
        {
            get
            {
                return GetCollection<NuevaAuditoriaActividad>("NuevaAuditoriaAct");
            }
        }

        [Association("NuevaAuditoria-TablaFinancieraResultados")]
        public XPCollection<TablaFinancieraResultados> TablaFinancieraRes
        {
            get
            {
                return GetCollection<TablaFinancieraResultados>("TablaFinancieraRes");
            }
        }

    }

}