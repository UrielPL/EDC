using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.DC;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [FileAttachment("archivoImportar")]
    public class Archivos : XPObject
    {
        public Archivos()
            : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public Archivos(Session session)
            : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            fecha = DateTime.Now;
            substatus = "Por realizar";
            // Place here your initialization code.
        }

        private mdl_auditoriaactividad idauditoriaactividad;
        [Association("AuditoriaActividad-Archivos")]
        [XafDisplayName("AuditoriaActividad")]
        public mdl_auditoriaactividad IdAuditoriaActividad
        {
            get { return idauditoriaactividad; }
            set { SetPropertyValue("Iddistribuidor", ref idauditoriaactividad, value); }
        }

        private NuevaAuditoriaActividad auditoriaActividad;
        [Association("NuevaAuditoriaActividad-Archivos")]
        [XafDisplayName("NuevaAuditoriaActividad")]
        public NuevaAuditoriaActividad _auditoriaActividad
        {
            get { return auditoriaActividad; }
            set { SetPropertyValue("_auditoriaActividad", ref auditoriaActividad, value); }
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

        private FileData archivoImportar2;
        [DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        [FileTypeFilter("DocumentFiles", 1, "*.txt", "*.csv")]
        [FileTypeFilter("AllFiles", 2, "*.*")]
        //[Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public FileData ArchivoImportar2
        {
            get { return archivoImportar2; }
            set
            {
                SetPropertyValue("ArchivoImportar2", ref archivoImportar2, value);

            }
        }

        private FileData archivoImportar3;
        [DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        [FileTypeFilter("DocumentFiles", 1, "*.txt", "*.csv")]
        [FileTypeFilter("AllFiles", 2, "*.*")]
        //[Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public FileData ArchivoImportar3
        {
            get { return archivoImportar3; }
            set
            {
                SetPropertyValue("ArchivoImportar3", ref archivoImportar3, value);

            }
        }

        public string Auditoria
        {
            get
            {
                string v = "";
                if (this.IdAuditoriaActividad != null)
                {
                    if (this.idauditoriaactividad.Idaud!=null)
                    {
                        v = this.IdAuditoriaActividad.Idaud.idaud.ToString();
                    }                    
                }
                return v;
            }
        }
        public string Secuencia
        {
            get
            {
                string v = "";
                if (this.IdAuditoriaActividad != null)
                {
                    v = this.IdAuditoriaActividad.secuencia.ToString();
                }
                return v;
            }
        }
        public string comentario { get; set; }
        
       public DateTime fecha { get; set; }
        public string usuario { get; set; }

        public string substatus { get; set; }

        protected override void OnSaving()
        {
            base.OnSaving();
            
        }

        protected override void OnSaved()
        {
            base.OnSaved();
/*   string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\Archivos\\Evidencia\\";
            string clave = this.Oid.ToString();
            if (this.ArchivoImportar != null)
            {

                if (!File.Exists(ruta + clave + this.ArchivoImportar.FileName))
                {
                    int pos = this.ArchivoImportar.FileName.IndexOf('.');
                    string ext = this.ArchivoImportar.FileName.Substring(pos, (this.ArchivoImportar.FileName.Length - pos));
                    string newname = clave + archivoImportar.FileName;
                    //string newname = ArchivoImportar.FileName + "-"+fecha.Day+fecha.Day + ext;
                    FileStream fileStream = System.IO.File.Create(ruta + newname);
                    this.ArchivoImportar.SaveToStream(fileStream);
                    fileStream.Close();

                }


            }*/
        }

        /* [Action(Caption = "Nombres")]
         public void nombres()
         {
             List<Archivos> todosArchivos = new XPQuery<Archivos>(this.Session).ToList();

            foreach (Archivos item in todosArchivos)
             {
                 if (item.ArchivoImportar!=null)
                 {
                     int cantOid =item.Oid.ToString().Length;
                     if (item.ArchivoImportar.FileName.Substring(0, cantOid)==item.Oid.ToString())
                     {
                         string f = "";
                     }
                     else
                     {
                         using (UnitOfWork uow = new UnitOfWork())
                         {
                             item.ArchivoImportar.FileName = item.Oid + item.ArchivoImportar.FileName;
                             item.Save();
                             uow.CommitChanges();
                         }
                     }

                 }

             }

         }*/
        [Action(Caption = "Importar")]
        public void Importar()
        {
            var ta= new XPQuery<Archivos>(this.Session);
            var todosArchivos = from ar in ta
                        orderby ar.fecha descending
                        select ar;
            string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\archivos\\evidencia\\";

            foreach (Archivos item in todosArchivos)
            {
                if (item.ArchivoImportar!=null)
                {
                    if (!File.Exists(ruta + item.ArchivoImportar.FileName))
                    {
                        FileStream fileStream = File.Create(ruta + item.ArchivoImportar.FileName);
                        item.ArchivoImportar.SaveToStream(fileStream);
                        fileStream.Close();
                    }
                }
                else if (item.ArchivoImportar2 != null)
                {
                    if (!File.Exists(ruta + item.ArchivoImportar2.FileName))
                    {
                        FileStream fileStream = File.Create(ruta + item.ArchivoImportar2.FileName);
                        item.ArchivoImportar2.SaveToStream(fileStream);
                        fileStream.Close();
                    }
                }
                else if (item.ArchivoImportar3 != null)
                {
                    if (!File.Exists(ruta + item.ArchivoImportar3.FileName))
                    {
                        FileStream fileStream = File.Create(ruta + item.ArchivoImportar3.FileName);
                        item.ArchivoImportar3.SaveToStream(fileStream);
                        fileStream.Close();
                    }
                }
            }
                       
        }

    }

}