using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Nueva Auditoria Actividad")]
    [XafDefaultProperty("idactplan")]

    public class NuevaAuditoriaActividad:XPObject
    {
        public NuevaAuditoriaActividad(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        private NuevaAuditoria idaud;
        [Association("NuevaAuditoria-NuevaAuditoriaActividad")]
        [XafDisplayName("Auditoria")]
        public NuevaAuditoria Idaud
        {
            get { return idaud; }
            set { SetPropertyValue("Idaud", ref idaud, value); }
        }

        private v_Actividad idactividad;
        [Association("NuevaAuditoriaActividad-v_Actividad")]
        [XafDisplayName("Actividad")]
        public v_Actividad Idactividad
        {
            get { return idactividad; }
            set { SetPropertyValue("Idactividad", ref idactividad, value); }
        }

        public int duracion { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime fechafinal { get; set; }
        [DisplayName("Fecha Completado")]
        public DateTime fechacomp { get; set; }

        public string status { get; set; }

        private Usuario _Evaluador;
        public Usuario Evaluador
        {
            get { return _Evaluador; }
            set { SetPropertyValue("Evaluador", ref _Evaluador, value); }
        }

        public string vigencia { get; set; }
        public string comentario { get; set; }

        [Association("NuevaAuditoriaActividad-Archivos")]
        public XPCollection<Archivos> Archivo
        {
            get
            {
                return GetCollection<Archivos>("Archivo");
            }
        }

    }
}
