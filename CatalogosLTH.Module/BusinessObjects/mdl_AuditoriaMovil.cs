using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.BaseImpl;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Auditoría Móvil")]
    [XafDefaultProperty("IdAuditoriaMovil")]

    public class mdl_AuditoriaMovil : XPLiteObject
    {
        public mdl_AuditoriaMovil() : base(){ }

        public mdl_AuditoriaMovil(Session session) : base(session) {        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place here your initialization code.
            resultado = resultadov.Pendiente.ToString();
        }

        [Key(true)]
        public int IdAuditoriaMovil { get; set; }
        public mdl_punto punto { get; set;}
        public DateTime fecha { get; set; }
     //   public resultado resultado { get; set; }
        //[Association("AuditoriaMovil-Actividades")]
        //public XPCollection<mdl_auditoriaactividad> Actividades
        //{
        //    get
        //    {
        //        return GetCollection<mdl_auditoriaactividad>("Actividades");
        //    }
        //}

        [Association("AuditoriaMovil-listaActividades")]
        public XPCollection<mdl_actividad> listaActividades
        {
            get
            {
                return GetCollection<mdl_actividad>("listaActividades");
            }
        }
        public Usuario evaluador { get; set; }
        public mdl_distribuidor distribuidor { get; set; }
        [XafDisplayName("No aceptada")]
        public bool status { get; set; }
        [XafDisplayName("Aceptada")]
        public bool aceptada { get; set; }
        public bool cerrada { get; set; }
        public string comentario { get; set; }
        public string centroServicio { get; set; }
       
        public string resultado { get; set; }
        public mdl_auditoria auditoria { get; set; }

        private AuditoriaMovilGeneral auditoriaGeneral;
        [Association("Aud-Detalle")]
        public AuditoriaMovilGeneral AuditoriaGeneral
        {
            get { return auditoriaGeneral; }
            set { SetPropertyValue("AuditoriaGeneral", ref auditoriaGeneral, value); }
        }

        private FileData archivoImportar;
        [DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        [FileTypeFilter("DocumentFiles", 1, "*.txt", "*.csv")]
        [FileTypeFilter("AllFiles", 2, "*.*")]
        //[Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public FileData ArchivoImportar
        {
            get { return archivoImportar; }
            set
            {
                SetPropertyValue("ArchivoImportar", ref archivoImportar, value);

            }
        }

    }
    public enum resultadov
    {
        Pendiente,
        NoCumple,
        Cumple,
        Ignorado
    }

}