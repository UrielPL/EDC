using CatalogosLTH.Module.BusinessObjects;
using DevExpress.ExpressApp.Web;
using DevExpress.Xpo;
using Saml;
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
    public partial class AuthForm : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {//edwyn.quiroga@lth.com.mx
            DateTime v = DateTime.UtcNow;

            //http://172.93.106.146/LTH/authform.aspx?u=edwyn.quiroga@lth.com.mx&token=11f8da347965944234001861eaa3fd1f

            string uname = Request.QueryString["u"] as string;
            string pass = Request.QueryString["token"] as string;

            if ((!string.IsNullOrEmpty(uname)  && !string.IsNullOrEmpty(pass)) || (uname != null && pass != null))
            {
                DevExpress.Xpo.Session session = Util.getsession();
                Usuario user = new XPQuery<Usuario>(session).First(x => x.email.ToLower() == uname.ToLower());
                if (user != null)
                {
                    string name = user.UserName;
                    string mail = user.email;
                    Response.Write("Antes de entrar> " + DateTime.Now.TimeOfDay);
                        if (Entra(uname, pass))//valida token
                        {
                            string u = uname.ToUpper();
                            string p = user.email.ToLower();

                            ActividadUsuario actUs = new ActividadUsuario(session);

                            using (UnitOfWork varses = new UnitOfWork())
                                {
                                    actUs.Fecha = DateTime.Now;
                                    actUs.Usuario = user;
                                    actUs.Save();
                                    varses.CommitChanges();
                                }

                            ((CatalogosLTHAspNetApplication)WebApplication.Instance).Logon(name, p);
                            WebApplication.Redirect("Default.aspx");
                        }
                        else
                        {
                            Response.Write("Token vencido, ingresar nuevamente desde la Universidad \n\n\n");
                            Response.Write(imprime(uname, pass));
                        }
                }

            }
            else
            {
                var x = Request.Form["SAMLResponse"];
                if (x == null)
                {
                    Login();
                }
                else
                {

                }
            }
            Response.Write("No se enviaron parámetros adecuados, favor de volver a intentar ");



            //((CatalogosLTHAspNetApplication)WebApplication.Instance).Security.LogonParameters
            //  ((MainDemoWebApplication)WebApplication.Instance).Logon(Request.QueryString["UserName"], "");
            // WebApplication.Redirect("Default.aspx") 12:19;
        }
        public void Login()
        {
            //TODO: specify the SAML provider url here, aka "Endpoint"
            var samlEndpoint = "https://login.microsoftonline.com/74b72ba8-5684-402c-98da-e38799398d7d/saml2";

            var request = new AuthRequest(
                "https://edc-ii.com/Login.aspx", //TODO: put your app's "entity ID" here
                "https://edc-ii.com/Login.aspx" //TODO: put Assertion Consumer URL (where the provider should redirect users after authenticating)
                );

            //redirect the user to the SAML provider
            Response.Redirect(request.GetRedirectUrl(samlEndpoint));
        }
        public static string imprime(string u, string token)
        {
            string texto="  \n\n "+DateTime.Now +"<v> \n\n" ;
            string timestampToken = "";
            DateTime nx = new DateTime(1970, 1, 1); // UNIX epoch date
                                                    //   TimeSpan ts = DateTime.UtcNow - nx; // UtcNow, because timestamp is in GMT
            TimeSpan ts = DateTime.UtcNow - nx;
            string tiempo = ((int)ts.TotalSeconds).ToString();

            texto += "  \n\n TIEMPO:  " + tiempo + "<t> \n\n";
            double tiempoEvaluar = ((int)ts.TotalSeconds) + 1600; //variación de +- 100 segundos entre request 

            string sal = "P4r4D!50";

            for (int i = 0; i < 3200; i++)
            {
                string valorAEncriptar = u + (tiempoEvaluar - i) + sal;
                string cryptedx = GetMD5(valorAEncriptar);
                texto += "\n   " + cryptedx + "   --   " + (tiempoEvaluar - i) + "\n";
                if (cryptedx.Equals(token))
                {
                    timestampToken = "  ts:"+(tiempoEvaluar - i);
                }
                
            }
            texto += "  \n\n " + DateTime.Now + "<v> \n\n";
            return (timestampToken);
        }
        public static bool Entra(string u, string token)
        {
           
            bool entra = false;
            DateTime nx = new DateTime(1970, 1, 1); // UNIX epoch date
            TimeSpan ts = DateTime.UtcNow - nx; // UtcNow, because timestamp is in GMT
          //  TimeSpan ts = v - nx;
            string tiempo = ((int)ts.TotalSeconds).ToString();
            
            
            double tiempoEvaluar = ((int)ts.TotalSeconds) + 800; //variación de +- 100 segundos entre request 

            string sal = "P4r4D!50";
            //La mayor parte del tiempo,  la hora en el servidor de paradiso esta adelantada por 5 minutos a comparacion de la hora en server de edcii, por lo tanto unicamente se suma tiempo 
            //a la hora del servidor de edcii 
            for (int i = 0; i < 1100; i++)
            {
                string valorAEncriptar = u + (tiempoEvaluar - i)+sal;
                string cryptedx = GetMD5(valorAEncriptar);

                if (cryptedx == token)
                {
                    entra = true;
                }
            }
            
            return entra;
        }

        public void guardaViewState(string x)
        {
            ViewState["tiempo"] = x;
        }

        

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