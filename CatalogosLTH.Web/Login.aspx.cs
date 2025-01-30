using System;
using System.Web.UI;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Templates;
using Saml;
using DevExpress.Xpo;
using CatalogosLTH.Module.BusinessObjects;
using CatalogosLTH.Web;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public partial class LoginPage : BaseXafPage
{
    public override System.Web.UI.Control InnerContentPlaceHolder
    {
        get
        {
            
            return Content;
        }
    }
    protected override void OnLoadComplete(EventArgs e)
    {

        base.OnLoadComplete(e);

        try
        {

            string samlCertificate = "-----BEGIN CERTIFICATE-----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-----END CERTIFICATE-----";     
                   
            string username = "";

            //var samlResp = "";
            var sml = Request.Form["SAMLResponse"];

            if (sml != null){
               // Response.Write(sml);
                // 2. Let's read the data - SAML providers usually POST it into the "SAMLResponse" var
                Saml.Response samlResponse = new Response(samlCertificate, sml);
               // Response.Write(samlResponse);

               // Response.Write(samlResponse.GetNameID() + " " + samlResponse.GetEmail() + " " + samlResponse.GetFirstName() + " " + samlResponse.GetLastName());
                var objetoUsuario = new XPQuery<Usuario>(Util.getsession()).FirstOrDefault(x => x.email == samlResponse.GetEmail());

                if (objetoUsuario != null)
                {
                    if (!string.IsNullOrEmpty(objetoUsuario.Nombre))
                    {
                        var usname = objetoUsuario.email;
                        //Response.Write(usname);
                        Entra(samlResponse.GetEmail());
                        //((CatalogosLTHAspNetApplication)WebApplication.Instance).Logon(usname, samlResponse.GetEmail().ToLower());
                        //WebApplication.Redirect("Default.aspx");



                        // 3. We're done!
                        if (samlResponse.IsValid())
                        {
                            username = samlResponse.GetNameID();
                           // Response.Write(username);

                        }
                    }
                   
                }
               
                   
                //Response.Redirect("~/AuthForm.aspx?ider=" + username);
            }            


           
        }
        catch(Exception ex)
        {
          //  Response.Redirect("~/AuthForm.aspx?ider="+ex.Message);
        }

        

        var s=Request.Form["ok"];
    }

    public  void Entra(string u)
    {

        bool entra = false;
        DateTime nx = new DateTime(1970, 1, 1); // UNIX epoch date
        TimeSpan ts = DateTime.UtcNow - nx; // UtcNow, because timestamp is in GMT
                                            //  TimeSpan ts = v - nx;
        string tiempo = ((int)ts.TotalSeconds).ToString();


        double tiempoEvaluar = ((int)ts.TotalSeconds); //variación de +- 100 segundos entre request 

        string sal = "P4r4D!50";
        //La mayor parte del tiempo,  la hora en el servidor de paradiso esta adelantada por 5 minutos a comparacion de la hora en server de edcii, por lo tanto unicamente se suma tiempo 
        //a la hora del servidor de edcii 
       
            string valorAEncriptar = u + (tiempoEvaluar ) + sal;
            string cryptedx = GetMD5(valorAEncriptar);



        Response.Redirect("~/authform.aspx?u=" + u + "&token=" + cryptedx);
        //return entra;
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
