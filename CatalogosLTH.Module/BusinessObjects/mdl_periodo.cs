using System;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.DC;
using System.Linq;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Periodo")]
    [XafDefaultProperty("Periodo")]
    public class mdl_periodo : XPLiteObject
    {
        public mdl_periodo() : base()
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public mdl_periodo(Session session) : base(session)
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here.
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Periodo = DateTime.Today.Year;
            // Place here your initialization code.
        }

        
        [Action(Caption = "Crear Registros")]
        public void crearRegistros()
        {
            List<string> meses = new List<string>();
            
            meses.Add("OCTUBRE");
            meses.Add("NOVIEMBRE");
            meses.Add("DICIEMBRE");
            meses.Add("ENERO");
            meses.Add("FEBRERO");
            meses.Add("MARZO");
            meses.Add("ABRIL");
            meses.Add("MAYO");
            meses.Add("JUNIO");
            meses.Add("JULIO");
            meses.Add("AGOSTO");
            meses.Add("SEPTIEMBRE");

            if (Registro.Count==0)
            {
                XPQuery<mdl_distribuidor> distribuidores = this.Session.Query<mdl_distribuidor>();

                using (UnitOfWork varses = new UnitOfWork())
                {
                    int cont = 0;
                    foreach (var mes in meses)
                    {
                        cont++;
                        foreach (var dis in distribuidores)
                        {
                            mdl_RegistroMensual registro = new mdl_RegistroMensual(this.Session);

                            registro.Periodo = this;
                            registro.orden = cont;
                            registro.Distribuidor = dis;
                            registro.Mes = mes;

                            registro.resultado = 0;
                            registro.terminadas = 0;
                            registro.Save();
                        }
                    }
                                        
                    varses.CommitChanges();
                }

            }
            else
            {
                throw new System.ArgumentException("Ya se crearon los registros para este periodo");

            }


        }

        [Action(Caption = "Quien")]
        public void quienEs()
        {
            UtilidadesBO.generaRegistrosMensuales();
            var distSinRegistros = new XPQuery<mdl_distribuidor>(this.Session).Where(x => x.Registro.Count() == 0&&x.nombredist!="nohay").ToList();
            var s = distSinRegistros;                

        }


        [Key(true)]
        public int IdPeriodo { get; set; }
        public int Periodo { get; set; }

        [Association("Periodo-Registro")]
        public XPCollection<mdl_RegistroMensual> Registro
        {
            get
            {
                return GetCollection<mdl_RegistroMensual>("Registro");
            }
        }

        protected override void OnSaving()
        {
            var periodos = new XPQuery<mdl_periodo>(this.Session).Where(x => x.Periodo == this.Periodo).Count();
            //listaDistribuidores = new XPQuery<Usuario>(session).Where(x => x.Jefe.UserName == nombreActual).ToList();
            //XPQuery<mdl_periodo> periodos = this.Session.Query<mdl_periodo>();
            if (periodos>0)
            {
                throw new System.ArgumentException("Ya existe un periodo registrado para este mismo año: "+this.Periodo);
            }
            else
            {
                crearRegistros();
            }
            base.OnSaving();
        }

        protected override void OnSaved()
        {
            base.OnSaved();
        }

    }

}