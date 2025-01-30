using System;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace CatalogosLTH.Module.BusinessObjects
{
    
    [DefaultClassOptions]
    [XafDisplayName("Actividad")]
    [XafDefaultProperty("Code")]
    public class mdl_actividad : XPLiteObject
    {
        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            mdl_actividad act = this;
            if (newValue != null)
            {
                if (propertyName == "Permisos")
                {
                    if (newValue.ToString()=="Particular"&&act.Encargado==null)
                    {
                        

                       /* if (oldValue.ToString() == "Particular") { this.Permisos = MascaraPermisos.Particular; }
                        else if (oldValue.ToString() == "GerenteCuenta") {this.Permisos = MascaraPermisos.GerenteCuenta;}
                        else if (oldValue.ToString() == "GerenteDesarrolloComercial") {this.Permisos = MascaraPermisos.GerenteDesarrolloComercial;}
                        this.Save();
                        throw new System.ArgumentException("DEBES ASIGNAR PRIMERO UN ENCARGADO");*/

                    }
                    else
                    {
                        var distribuidores = new XPQuery<mdl_distribuidor>(this.Session);                        
                        List<mdl_auditoria> ultimasAuditorias = new List<mdl_auditoria>();
                        var audact = new XPQuery<mdl_auditoriaactividad>(this.Session);

                        foreach (var item in distribuidores)
                        {
                            int c = item.UltimaAuditoria.idaud;
                            ultimasAuditorias.Add(item.UltimaAuditoria);
                        }

                        var data1 = from au in ultimasAuditorias
                                    join aa in audact on au equals aa.Idaud
                                    where aa.Idactividad == act
                                    select new { aa };

                        foreach (var item in data1)//auditorias actividades
                        {
                            Usuario eval = null;//Usuario que se le asignará el evaluador dependiendo del permiso seleccionado
                            Usuario usuarioDistribuidor;//traer usuario del mismo nombre que el distribuidor de la auditoria
                            usuarioDistribuidor = new XPQuery<Usuario>(this.Session).FirstOrDefault(x => x.UserName == item.aa.Idaud.Iddistribuidor.nombredist);
                            eval = item.aa.Evaluador;
                            
                            if (newValue.ToString() == "Particular")
                            {
                                if (act.Encargado != null)
                                {
                                    item.aa.Evaluador = act.Encargado;
                                }
                                else
                                {
                                    //throw new DevExpress.ExpressApp.UserFriendlyException("DEBES ASIGNAR PRIMERO UN ENCARGADO");
                                    this.Permisos = (MascaraPermisos)oldValue;

                                    //throw new System.ArgumentException("DEBES ASIGNAR PRIMERO UN ENCARGADO");
                                }
                            }
                            else if (newValue.ToString() == "GerenteCuenta")
                            {
                                if (item.aa.Idaud.Iddistribuidor != null)
                                {
                                    eval = usuarioDistribuidor.Jefe;
                                }
                            }
                            else if (newValue.ToString() == "GerenteDesarrolloComercial")
                            {
                                if (item.aa.Idaud.Iddistribuidor != null)
                                {
                                    if (usuarioDistribuidor.Jefe != null)
                                    {
                                        eval = usuarioDistribuidor.Jefe.Jefe;
                                    }
                                }
                            }
                            item.aa.Evaluador = eval;
                            item.aa.Save();
                        }
                        this.Session.CommitTransaction();
                    }

                }
            }
        }
        public mdl_actividad(Session session)
            : base(session) {        }
        [Key(true)]
        public int IdActividad { get; set; }
        public string Code { get; set; }
        [Size(SizeAttribute.Unlimited)]
        public string Texto { get; set; }
        //public int NivelID { get; set; }

        private FileData archivoImportar;
        [DevExpress.Xpo.Aggregated, ExpandObjectMembers(ExpandObjectMembers.Never)]
        public FileData ArchivoImportar
        {
            get { return archivoImportar; }
            set
            {
                SetPropertyValue("ArchivoImportar", ref archivoImportar, value);

            }
        }

        [Action(Caption = "Eliminar Actividades")]
        public void GenerarObjetivos()
        {

            if (ArchivoImportar != null)
            {
                string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\archivos\\";
                string nombre = ArchivoImportar.FileName;

                FileStream fileStream = System.IO.File.Create(ruta + nombre);

                ArchivoImportar.SaveToStream(fileStream);
                fileStream.Close();
                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                var reader = new StreamReader(File.OpenRead(ruta + nombre), iso);
                var line = reader.ReadLine();
                List<mdl_distribuidor> lstDist = new XPQuery<mdl_distribuidor>(this.Session).ToList();
                //Session varses = new Session();

                using (UnitOfWork varses = new UnitOfWork())
                {

                    int i = 0;

                    while (!reader.EndOfStream)
                    {
                        i++;
                        line = reader.ReadLine();
                        var values = line.Split(',');

                        mdl_actividad actTemp = new XPQuery<mdl_actividad>(this.Session).FirstOrDefault(x=>x.Code == values[0]);
                        if (actTemp != null)
                        {
                            actTemp.Delete();
                        }

                    }
                    varses.CommitChanges();
                }

            }
        }

        private mdl_nivel nivelID;
        [Association("Nivel-Actividades")]
        [XafDisplayName("Nivel")]
        public mdl_nivel NivelID

        {
            get { return nivelID; }
            set { SetPropertyValue("NivelID", ref nivelID, value); }
        }


        //public int PilarID { get; set; }

        private mdl_pilar pilarID;
        [Association("Pilar-Actividades")]
        [XafDisplayName("Pilar")]
        public mdl_pilar PilarID

        {
            get { return pilarID; }
            set { SetPropertyValue("PilarID", ref pilarID, value); }
        }

        public int Duracion { get; set; }
        [Size(SizeAttribute.Unlimited)]
        public string Instruccion { get; set; }
        public int Secuencia { get; set; }

        [Association("Punto-Actividad")]
        public XPCollection<mdl_punto> Puntos
        {
            get
            {
                return GetCollection<mdl_punto>("Puntos");
            }
        }

        [Association("AuditoriaActividad-Actividad")]
        public XPCollection<mdl_auditoriaactividad> AuditoriaActividad
        {
            get
            {
                return GetCollection<mdl_auditoriaactividad>("AuditoriaActividad");
            }
        }

        [XafDisplayName("Auditorías móviles")]
        [Association("AuditoriaMovil-listaActividades")]
        public XPCollection<mdl_AuditoriaMovil> Auditoriamovil
        {
            get
            {
                return GetCollection<mdl_AuditoriaMovil>("Auditoriamovil");
            }
        }

        //public MascaraPermisos Permisos { get; set; }
        private MascaraPermisos permisos;
        public MascaraPermisos Permisos
        {
            get { return permisos; }
            set { SetPropertyValue("Permisos", ref permisos, value); }
        }

        private Usuario encargado;
        [Association("Usuario-Actividades")]
        [XafDisplayName("Encargado")]
        public Usuario Encargado

        {
            get { return encargado; }
            set { SetPropertyValue("Encargado", ref encargado, value); }
        }



        FileData muestra1;
        [ImmediatePostData]
        public FileData Muestra1
        {
            get { return muestra1; }
            set { SetPropertyValue("Muestra1", ref muestra1, value); }
        }
        FileData muestra2;
        [ImmediatePostData]
        public FileData Muestra2
        {
            get { return muestra2; }
            set { SetPropertyValue("Muestra2", ref muestra2, value); }
        }

        [Association("Regla - Actividad")]
        public XPCollection<ReglaEvaluacion> Reglas
        {
            get
            {
                return GetCollection<ReglaEvaluacion>("Reglas");
            }
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            if (this.Permisos.ToString() == "Particular" && this.Encargado == null)
            {
                throw new System.ArgumentException("DEBES ASIGNAR PRIMERO UN ENCARGADO");
            //if (newValue.ToString() == "Particular" && act.Encargado == null)

            }

                string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\Archivos\\";
            
            if (Muestra1 != null)
            {
                int pos = Muestra1.FileName.IndexOf('.');
                string ext = Muestra1.FileName.Substring(pos, (Muestra1.FileName.Length - pos));
                string newname = IdActividad + "Muestra1" + ext;
                FileStream fileStream = System.IO.File.Create(ruta + newname);
                Muestra1.SaveToStream(fileStream);
                fileStream.Close();
            }
            if (Muestra2 != null)
            {
                int pos2 = Muestra2.FileName.IndexOf('.');
                string ext2 = Muestra2.FileName.Substring(pos2, (Muestra2.FileName.Length - pos2));
                string newname2 = IdActividad + "Muestra2" + ext2;
                FileStream fileStream2 = System.IO.File.Create(ruta + newname2);
                Muestra2.SaveToStream(fileStream2);
                fileStream2.Close();
            }
        }
       



    }
    
    public enum MascaraPermisos {GerenteDesarrolloComercial, GerenteCuenta, Particular }
}