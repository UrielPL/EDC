using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class edcMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string nombre = Util.getusuario();
                ViewState["nombreUsuario"] = nombre;
                string rol = Util.getrol();
                if (rol == "Distribuidor")
                {
                    ViewState["rol"] = "Distribuidor";
                }                 
                else
                {
                    ViewState["rol"] = "Evaluador";
                }
                TipoUsuario permiso = Util.getPermiso();
                ViewState["permiso"] = permiso.ToString();
                if (rol == "DC Administrativo")
                {
                    ViewState["rol"] = "DCAdmin";
                   // ViewState["permiso"] = "DCAdmin";

                }

                Usuario user = new XPQuery<Usuario>(Util.getsession()).FirstOrDefault(x => x.UserName == nombre);

                //string nm = user.Nombre;
                string nm = user.email;
                string encriptado = GetMD5(nm);

                ViewState["u"] = nm;//EMAIL DE USUARIO
                ViewState["token"] = encriptado;//EMAIL ENCRIPTADO
                ViewState["token2"] = JWTGenerator(nm,user.TipoUsuario);//EMAIL ENCRIPTADO
                string eltk = JWTGenerator(nm, user.TipoUsuario);//EMAIL ENCRIPTADO

            }
        }

        protected string JWTGenerator(string usr,TipoUsuario tipoUsuario)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var now = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds) + 1200;
            var payload = new Dictionary<string, object>()
                {
                 { "exp", now },
                 { "Usuario", usr },
                 { "Tipo", tipoUsuario.ToString() }
                };
            
            return JWT.JsonWebToken.Encode(payload, "98AB17C3-0A8C-43E9-A319-1F6BB518EB63", JWT.JwtHashAlgorithm.HS256);
        }

        protected void salir(object sender, EventArgs e)
        {
            DevExpress.ExpressApp.Web.WebApplication.LogOff(Session);

        }

        /*protected void autoauditoria(object sender, EventArgs e)
        {
            
            string usuario = Util.getusuario();// consigue el UserName
            Usuario user = new XPQuery<Usuario>(Util.getsession()).FirstOrDefault(x => x.UserName == usuario);

            string nombre = user.Nombre;
            string encriptado = GetMD5(usuario);
            // Response.Redirect("http://172.93.106.146/edclth/authform.aspx?u="+usuario+"&token="+encriptado);
          // Response.Redirect("http://45.32.4.194/autoauditoria/authform.aspx?u=" + usuario + "&token=" + encriptado);
        }*/
        public static string GetMD5(string str)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();

        }
    }
}