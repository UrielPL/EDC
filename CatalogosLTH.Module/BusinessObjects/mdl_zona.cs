using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp;
using System.Linq;

namespace CatalogosLTH.Module.BusinessObjects
{
    [DefaultClassOptions]
    [XafDisplayName("Zona")]
    [XafDefaultProperty("zona")]
    public class mdl_zona : XPLiteObject
    {
        
        [Action(Caption = "Actualizar Encargados")]
        public void ActualizarEncargados()
        {

            //XPQuery<mdl_auditoriaactividad> audacts =  this.Session.Query<mdl_auditoriaactividad>();
            //  var audacts = new XPQuery<mdl_auditoriaactividad>(this.Session).Where
            var audacts = new XPQuery<mdl_auditoriaactividad>(this.Session).Where(x => x.Idaud != null);

            XPQuery<Usuario> UsuarioDist = this.Session.Query<Usuario>();
            foreach (mdl_auditoriaactividad item in audacts)
            {                
                mdl_actividad idactividad = item.Idactividad;
                mdl_auditoria idauditoria = item.Idaud;//auditoria
                Usuario usuarioEncargado = null;
                string distribuidor = idauditoria.Iddistribuidor.nombredist;

                foreach (var UsuarioD in UsuarioDist)
                {                    
                    if (string.Equals(UsuarioD.UserName,distribuidor, StringComparison.OrdinalIgnoreCase))
                    {
                        usuarioEncargado = UsuarioD;
                    }                  
                }

                MascaraPermisos permiso = idactividad.Permisos;
                Usuario eval = null;

                if (permiso == MascaraPermisos.Particular)
                {
                    eval = idactividad.Encargado;
                }
                else if (permiso == MascaraPermisos.GerenteCuenta)
                {
                    /*  if (idactividad.Encargado != null)
                      {
                          eval = idactividad.Encargado.Jefe;
                      }*/
                      
                    if (usuarioEncargado!= null)
                    {
                        eval = usuarioEncargado.Jefe;
                    }
                }
                else if (permiso == MascaraPermisos.GerenteDesarrolloComercial)
                {
                    /* if (idactividad.Encargado != null)
                     {
                         eval = idactividad.Encargado.Jefe.Jefe;
                     }*/
                    if (usuarioEncargado != null)
                    {
                        if (usuarioEncargado.Jefe!=null)
                        {
                            eval = usuarioEncargado.Jefe.Jefe;
                        }                        
                    }
                }

                item.Evaluador = eval;
                item.Save();
                //return eval;
            }
            
            this.Session.CommitTransaction();
            
        }
       public mdl_zona(Session session) : base(session)
       {
          
       }
        [Key(true)]
        public int id { get; set; }

        public string zona { get; set; }
       

        Usuario encargado = null;
        public Usuario Encargado
        {
            get { return encargado; }
            set
            {
                if (encargado == value)
                    return;

                // Store a reference to the person's former house. 
                  Usuario prevEncargado = encargado;
                encargado = value;

                if (IsLoading) return;

                // Remove a reference to the house's owner, if the person is its owner. 
                  if (prevEncargado != null && prevEncargado.Zona == this)
                    prevEncargado.Zona = null;

                // Specify the person as a new owner of the house. 
                if (encargado != null)
                {
                    encargado.Zona = this;
                    encargado.ZonaPertenece = this;
                    if (encargado.TipoUsuario==TipoUsuario.GerenteDesarrolloComercial)
                    {
                        actualizaZona(encargado, this);
                    }
                }
                OnChanged("Encargado");
            }
        }

        [Association("Zona-Usuarios")]
        public XPCollection<Usuario> Usuarios
        {
            get
            {
                return GetCollection<Usuario>("Usuarios");
            }
        }

        public void actualizaZona(Usuario usuario, mdl_zona zona)
        {
            foreach (var item in usuario.Dependientes)
            {
                item.ZonaPertenece = zona;
                foreach (var dis in item.Dependientes)
                {
                    dis.ZonaPertenece = zona;
                }
            }
        }
    }
}