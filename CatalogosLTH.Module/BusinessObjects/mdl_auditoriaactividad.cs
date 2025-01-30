using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using System.Linq;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Auditoria Actividad")]
    [XafDefaultProperty("idactplan")]
    public class mdl_auditoriaactividad : XPLiteObject
    {
      
        public mdl_auditoriaactividad(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }
        [Key(true)]
        public int idactplan { get; set; }

        private mdl_auditoria idaud;
        [Association("AuditoriaActividad-Auditoria")]
        [XafDisplayName("Auditoria")]
        public mdl_auditoria Idaud
        {
            get { return idaud; }
            set { SetPropertyValue("Idaud", ref idaud, value); }
        }

        private mdl_actividad idactividad;
        [Association("AuditoriaActividad-Actividad")]
        [XafDisplayName("Actividad")]
        public mdl_actividad Idactividad
        {
            get { return idactividad; }
            set { SetPropertyValue("Idactividad", ref idactividad, value); }
        }

        public int secuencia { get; set; }
        public int duracion { get; set; }
        public DateTime fechainicio { get; set; }
        public DateTime fechafinal { get; set; }
        public DateTime fechacomp { get; set; }
        public DateTime fechaEnvio { get; set; }
        [Association("AuditoriaActividad-Archivos")]
        public XPCollection<Archivos> Archivo
        {
            get
            {
                return GetCollection<Archivos>("Archivo");
            }
        }

        public string status { get; set; }
        public string comentario { get; set; }

        
        public override void AfterConstruction()
        {
 	        base.AfterConstruction();
            status="Por realizar";            
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            string status = this.status;
            
            if (this.status.ToLower().Equals("por realizar"))
            {
                List<Archivos> listaArch = this.Archivo.ToList();
                if (listaArch.Count>0)
                {
                    var raras = listaArch.OrderByDescending(x => x.fecha).First();
                    if (raras.usuario == "Distribuidor")
                    {
                        string v = "stepedo";
                    }
                }
               
            }
           
        }



        public Usuario Evaluador{ get; set; }
        //[XafDisplayName("Auditorías móviles")]
        //[Association("AuditoriaMovil-Actividades")]
        //public XPCollection<mdl_AuditoriaMovil> Auditoriamovil
        //{
        //    get
        //    {
        //        return GetCollection<mdl_AuditoriaMovil>("Auditoriamovil");
        //    }
        //}

    }
    
}