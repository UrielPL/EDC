using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Security.Strategy;
using System.IO;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]

    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [XafDefaultProperty("Nombre")]
    public class Usuario : SecuritySystemUser
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        [Action(Caption = "GenerarReglas")]
        public void GenerarReglas()
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

                using (UnitOfWork uow = new UnitOfWork())
                {
                    int i = 0;

                    while (!reader.EndOfStream)//LEE EL ARCHIVO
                    {
                        i++;
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        for (int j = 0; j < 8; j++)
                        {
                            ReglaEvaluacion verifica = new XPQuery<ReglaEvaluacion>(this.Session).FirstOrDefault(x => x.Distribuidor.nombredist == values[1] && x.Evaluador.Nombre == values[0] && x.Actividad.Code.Contains(values[j + 2]));
                            ReglaEvaluacion repite = new XPQuery<ReglaEvaluacion>(this.Session).FirstOrDefault(x => x.Distribuidor.nombredist == values[1] && x.Actividad.Code.Contains(values[j + 2]));
                            if (verifica != null)
                            {

                            } else if (repite != null)
                            {
                                repite.Evaluador = new XPQuery<Usuario>(this.Session).FirstOrDefault(x=>x.Nombre == values[0]);
                                repite.Save();
                            } else {
                                if (values[j + 2].Length == 1)
                                {
                                    string temp = "00";
                                    values[j + 2] = temp + values[j + 2];
                                }
                                else if (values[j + 2].Length == 2)
                                {
                                    string temp = "0";
                                    values[j + 2] = temp + values[j + 2];
                                }
                                ReglaEvaluacion newRegla = new ReglaEvaluacion(this.Session);
                                newRegla.Distribuidor = new XPQuery<mdl_distribuidor>(this.Session).FirstOrDefault(x => x.nombredist == values[1]);
                                if (newRegla.Distribuidor == null)
                                {
                                    newRegla.Comentario = "No coincidió el distribuidor(" + values[1] + ").";
                                }
                                newRegla.Evaluador = new XPQuery<Usuario>(this.Session).FirstOrDefault(x => x.Nombre == values[0]);
                                if (newRegla.Evaluador == null)
                                {
                                    if (newRegla.Comentario == null)
                                        newRegla.Comentario = "No coincidió el nombre del evaluador(" + values[0] + ").";
                                    else
                                        newRegla.Comentario = newRegla.Comentario + ". No coincidió el nombre del evaluador(" + values[0] + ").";
                                }
                                newRegla.Actividad = new XPQuery<mdl_actividad>(this.Session).FirstOrDefault(x => x.Code.Contains(values[j + 2].ToString()));
                                if (newRegla.Actividad == null)
                                {
                                    if (newRegla.Comentario == null)
                                        newRegla.Comentario = "No coincidió el No. de activdad(" + values[j + 2] + ").";
                                    else
                                        newRegla.Comentario = newRegla.Comentario + ". No coincidió el No. de activdad(" + values[j + 2] + ").";
                                }
                                newRegla.Save();
                            }
                        }
                    }
                    uow.CommitChanges();
                }
            }
        }

        [Action(Caption = "Actualiza Actividades a Evaluar")]
        public void Importar()
        {
            if (TipoUsuario.ToString()== "GerenteCuenta")
            {
                if (Dependientes.Count != 0)
                {                   
                    List<mdl_auditoria> ultimasAud = new List<mdl_auditoria>();
                    List<mdl_auditoriaactividad> lstAudact = new List<mdl_auditoriaactividad>();
                    foreach (Usuario item in Dependientes)
                    {
                        mdl_distribuidor dis = new XPQuery<mdl_distribuidor>(this.Session).FirstOrDefault(x => x.nombredist == item.UserName);
                        ultimasAud.Add(dis.UltimaAuditoria);
                    }

                    foreach (mdl_auditoria item in ultimasAud)
                    {
                        var actEv = new XPQuery<mdl_auditoriaactividad>(this.Session).Where(x => x.Idaud.idaud == item.idaud&&x.Idactividad.Permisos.ToString()== "GerenteCuenta");//&&x.Idactividad.Permisos==TipoUsuario
                        foreach (mdl_auditoriaactividad itemact in actEv)
                        {
                            lstAudact.Add(itemact);
                        }
                    }

                    foreach (mdl_auditoriaactividad audact in lstAudact)
                    {
                        if (audact!=null)
                        {
                            using (UnitOfWork varses = new UnitOfWork())
                            {
                                audact.Evaluador= this;
                                audact.Save();
                                varses.CommitChanges();
                            }
                        }
                    }



                    //FileStream fileStream = System.IO.File.Create(ruta + nombre);

                    //ArchivoImportar.SaveToStream(fileStream);
                    //fileStream.Close();
                    //Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                    //var reader = new StreamReader(File.OpenRead(ruta + nombre), iso);
                    //var line = reader.ReadLine();
                    ////Session varses = new Session();

                    //using (UnitOfWork varses = new UnitOfWork())
                    //{

                    //    int i = 0;

                    //    while (!reader.EndOfStream)
                    //    {
                    //        i++;
                    //        line = reader.ReadLine();
                    //        var values = line.Split(',');

                    //        foreach (mdl_auditoriaactividad item in auditoriaActividad)
                    //        {
                    //            if (item.Idactividad.Code.Equals(values[0].Trim()))
                    //            {
                    //                item.fechacomp = DateTime.Now;
                    //                item.status = "Completada";
                    //                item.Save();
                    //            }
                    //        }
                    //    }
                    //    varses.CommitChanges();
                    //}


                }
            }
           
        }
        public Usuario(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
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
        public string Nombre { get; set; }
        public string email { get; set; }
         public TipoUsuario TipoUsuario { get; set; }

        [Association("Jefe-Dependientes")]
        public XPCollection<Usuario> Dependientes
        {
            get
            {
                return GetCollection<Usuario>("Dependientes");
            }
        }

        private Usuario jefe;
        [Association("Jefe-Dependientes")]
        public Usuario Jefe
        {
            get { return jefe; }
            set { SetPropertyValue("Jefe", ref jefe, value); }
        }


        [Association("Supervisor-supervisado")]
        public XPCollection<Usuario> DistribuidoresSupervisa
        {
            get
            {
                return GetCollection<Usuario>("DistribuidoresSupervisa");
            }
        }

        private Usuario supervisor;
        [Association("Supervisor-supervisado")]
        public Usuario Supervisor
        {
            get { return supervisor; }
            set { SetPropertyValue("Supervisor", ref supervisor, value); }
        }

        [Association("Usuario-Actividades")]
        public XPCollection<mdl_actividad> Actividades
        {
            get
            {
                return GetCollection<mdl_actividad>("Actividades");
            }
        }

        [Association("Usuario - Minutas")]
        public XPCollection<Minutas> Minutas
        {
            get
            {
                return GetCollection<Minutas>("Minutas");
            }
        }

        [Association("Usuario - DistribuidorMinutas")]
        public XPCollection<mdl_DistribuidoresMinutas> DistribuidoresMinutas
        {
            get
            {
                return GetCollection<mdl_DistribuidoresMinutas>("DistribuidoresMinutas");
            }
        }

        mdl_zona zona = null;
        public mdl_zona Zona
        {
            get { return zona; }
            set
            {
                if (zona == value)
                    return;

                // Store a reference to the former owner. 
                mdl_zona prevZona = zona;
                zona = value;

                if (IsLoading) return;

                // Remove an owner's reference to this building, if exists. 
                if (prevZona != null && prevZona.Encargado == this)
                    prevZona.Encargado = null;

                // Specify that the building is a new owner's house. 
                if (zona != null)
                    zona.Encargado = this;
                OnChanged("Zona");
            }
        }

        private mdl_zona zonaPertenece;//Zona a la que pertenece el usuario, se define automaticamente 
        [Association("Zona-Usuarios")]
        public mdl_zona ZonaPertenece
        {
            get { return zonaPertenece; }
            set { SetPropertyValue("ZonaPertenece", ref zonaPertenece, value); }
        }

        [Association("Regla - Evaluador")]
        public XPCollection<ReglaEvaluacion> Reglas
        {
            get
            {
                return GetCollection<ReglaEvaluacion>("Reglas");
            }
        }

        [Association("Actividad-Usuario")]
        public XPCollection<ActividadUsuario> Logs
        {
            get
            {
                return GetCollection<ActividadUsuario>("Logs");
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (this.TipoUsuario==TipoUsuario.Distribuidor)
            {
                this.UserName = this.UserName.ToUpper();
            }
        }

    }
    //Ernesto Franco, ernesto.franco@jci.com, Director General
    //Mario Moreno, mario.a.moreno@jci.com, Gerente de venta zona sur
    //Jaime Adriaenséns, jesus.j.adriaensens @jci.com,
    public enum TipoUsuario {Admin,Evaluador,Distribuidor,GerenteDesarrolloComercial,GerenteCuenta,GerenteVenta,DirectorGeneral}

}