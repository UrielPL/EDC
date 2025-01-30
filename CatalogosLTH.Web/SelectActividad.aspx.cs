using CatalogosLTH.Module.BusinessObjects;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CatalogosLTH.Web
{
    public partial class SelectActividad : System.Web.UI.Page
    {
        public List<Archivos> lstArchivos { get; set; }
        public NuevaAuditoriaActividad actividad { get; set; }
        public List<v_Condicion> condiciones { get; set; }
        public string usuario { get; set; }
        public string nomusuario { get; set; }
        public string id { get; set; }
        public string objetivo { get; set; }
        public string queHacer { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            DevExpress.Xpo.Session session = Util.getsession();

            id = Request.QueryString["id"];

            usuario = Util.getrol();
            nomusuario = Util.getusuario();

            if (id != null)
            {
                actividad = new XPQuery<NuevaAuditoriaActividad>(session).FirstOrDefault(x => x.Oid == int.Parse(id));

                objetivo = actividad.Idactividad.Objetivo;
                queHacer = actividad.Idactividad.QueHacer;

                condiciones = new XPQuery<v_Condicion>(session).Where(c => c.Punto == actividad.Idactividad.Punto).ToList();

                lstArchivos = new XPQuery<Archivos>(session).Where(x => x._auditoriaActividad == actividad).OrderByDescending(x => x.fecha).ToList();

                if (lstArchivos.Count() > 0)
                {
                    //var recentArchivo = lstArchivos.Where(x => x.usuario == "Distribuidor").First();
                    //if (recentArchivo.substatus == "En revisión" && usuario == "Distribuidor")
                    //{

                    //    btnUpload.Visible = false;
                    //}
                    if (actividad.status == "En revisión" && usuario == "Distribuidor")
                    {
                        btnUpload.Visible = false;
                    }

                    if (actividad.status == "Completada")
                    {
                        btnUpload.Visible = false;
                    }
                }
            }
        }



        protected void btnUpload_Click(object sender, EventArgs e)//GUARDAR EVALUADOR
        {
            //string usuario = Session["Usuario"] as string;

            //GuardaActividad(Util.getusuario());
            guardaTodo(comentarios.Value, subeFile);

            Response.Redirect(Request.RawUrl);
        }

        protected void btnUpload2_Click(object sender, EventArgs e)//GUARDAR DISTRIBUIDOR
        {
            //string usuario = Session["Usuario"] as string;

            //GuardaActividad(Util.getusuario());
            guardarSubida(comentarios.Value, subeFile);

            Response.Redirect(Request.RawUrl);
        }

        protected void btnDevolver(object sender, EventArgs e)
        {
            devuelve(comentarios.Value, subeFile);

            Response.Redirect(Request.RawUrl);
        }

        public void devuelve(string comentarios, FileUpload postedFile)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string usuario = Util.getrol();
            string nomusuario = Util.getusuario();
            string id = Request.QueryString["id"];

            Usuario userDist = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName.ToString() == actividad.Idaud.Distribuidor.nombredist.ToString());//Usuario distribuidor

            var act = new XPQuery<NuevaAuditoriaActividad>(session).FirstOrDefault(x => x.Oid.ToString() == id);

            string nuevoStatus = "Por realizar";

            if (act.status == "Por realizar")
            {
                Button1.Enabled = false;
                Response.Write("Actividad ya devuelta");
            }
            else
            {
                if (comentarios != null && comentarios != "")
                {
                    try
                    {
                        DevExpress.Persistent.BaseImpl.FileData dataF = null;
                        string clav = "";
                        dataF = new DevExpress.Persistent.BaseImpl.FileData(session);

                        //PARA LEER EL ARCHIVO CARGADO
                        if (postedFile.HasFile)
                        {

                            if (postedFile != null)
                            {
                                var fileStream = postedFile.FileContent;
                                dataF.LoadFromStream(postedFile.FileName, fileStream);

                                // Procesa el archivo aquí (por ejemplo, guarda el archivo en el servidor)
                                //string fileName = Path.GetFileName(postedFile.FileName);
                                //postedFile.SaveAs(HttpContext.Current.Server.MapPath("~/Uploads/" + fileName));
                            }

                        }

                        using (UnitOfWork uow = new UnitOfWork())
                        {
                            Archivos up = new Archivos(session);
                            up.comentario = comentarios;

                            if (dataF != null)
                            {
                                up.ArchivoImportar = dataF;
                            }

                            up.usuario = usuario;

                            if (actividad != null)
                            {
                                up._auditoriaActividad = act;
                            }

                            string status = actividad.status;

                            up.substatus = nuevoStatus;
                            up.Save();
                            uow.CommitChanges();
                            clav = up.Oid.ToString();
                        }

                        if (postedFile.HasFile)
                        {
                            using (UnitOfWork uow = new UnitOfWork())
                            {
                                Archivos up = new XPQuery<Archivos>(session).FirstOrDefault(x => x.Oid.ToString() == clav);
                                if (up.ArchivoImportar != null)
                                {
                                    up.ArchivoImportar.FileName = clav + up.ArchivoImportar.FileName;
                                }
                                up.Save();
                                uow.CommitChanges();
                            }

                            //guarda en disco
                            string fileName = dataF.FileName;
                            string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\archivos\\Evidencia\\";

                            FileStream fileStream1 = System.IO.File.Create(ruta + fileName);

                            dataF.SaveToStream(fileStream1);
                            fileStream1.Close();
                            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                        }

                        using (UnitOfWork uow2 = new UnitOfWork())
                        {
                            actividad.status = nuevoStatus;
                            actividad.Save();
                            uow2.CommitChanges();
                        }
                    }
                    catch (Exception e)
                    {
                        //manda correo de error
                    }
                }
                else
                {
                    //ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Debes agregar comentarios para devolver actividad.');", true);
                    errorDevolver.InnerText = "Debes llenar el campo de comentarios.";
                }

            }


        }

        public void guardaTodo(string comentarios, FileUpload postedFile)
        {
            //string comentarios = HttpContext.Current.Request.Form["comentarios"];
            //string checkEvidencia = HttpContext.Current.Request.Form["checkEvidencia"];
            //HttpPostedFile postedFile = HttpContext.Current.Request.Files["archivo"];
            string nuevostatus = "";

            DevExpress.Xpo.Session session = Util.getsession();
            string usuario = Util.getrol();
            string nomusuario = Util.getusuario();
            string id = Request.QueryString["id"];

            Usuario userDist = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName.ToString() == actividad.Idaud.Distribuidor.nombredist.ToString());//Usuario distribuidor

            var act = new XPQuery<NuevaAuditoriaActividad>(session).FirstOrDefault(x => x.Oid.ToString() == id);

            if (actividad != null)
            {
                nuevostatus = "Completada";

                try
                {
                    DevExpress.Persistent.BaseImpl.FileData dataF = null;
                    string clav = "";
                    dataF = new DevExpress.Persistent.BaseImpl.FileData(session);

                    //PARA LEER EL ARCHIVO CARGADO
                    if (postedFile.HasFile)
                    {

                        if (postedFile != null)
                        {
                            var fileStream = postedFile.FileContent;
                            dataF.LoadFromStream(postedFile.FileName, fileStream);

                            // Procesa el archivo aquí (por ejemplo, guarda el archivo en el servidor)
                            //string fileName = Path.GetFileName(postedFile.FileName);
                            //postedFile.SaveAs(HttpContext.Current.Server.MapPath("~/Uploads/" + fileName));
                        }

                    }

                    using (UnitOfWork uow = new UnitOfWork())
                    {
                        Archivos up = new Archivos(session);
                        up.comentario = comentarios;

                        if (dataF != null)
                        {
                            up.ArchivoImportar = dataF;
                        }

                        up.usuario = usuario;

                        if (actividad != null)
                        {
                            up._auditoriaActividad = act;
                        }

                        string status = actividad.status;

                        up.substatus = nuevostatus;
                        up.Save();
                        uow.CommitChanges();
                        clav = up.Oid.ToString();
                    }

                    if (postedFile.HasFile)
                    {
                        using (UnitOfWork uow = new UnitOfWork())
                        {
                            Archivos up = new XPQuery<Archivos>(session).FirstOrDefault(x => x.Oid.ToString() == clav);
                            if (up.ArchivoImportar != null)
                            {
                                up.ArchivoImportar.FileName = clav + up.ArchivoImportar.FileName;
                            }
                            up.Save();
                            uow.CommitChanges();
                        }

                        //guarda en disco
                        string fileName = dataF.FileName;
                        string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\archivos\\Evidencia\\";

                        FileStream fileStream1 = System.IO.File.Create(ruta + fileName);

                        dataF.SaveToStream(fileStream1);
                        fileStream1.Close();
                        Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                    }
                }
                catch (Exception e)
                {
                    //manda correo de error
                }

                if (actividad != null)
                {
                    using (UnitOfWork uow3 = new UnitOfWork())
                    {
                        //**Cambia estado de la actividad
                        actividad.fechacomp = DateTime.Now;
                        actividad.status = nuevostatus;

                        //SUMA PUNTOS DE ACTIVIDAD A LA CALIFICACION FINAL DE LA AUDITORIA
                        var punto = actividad.Idactividad.Punto;
                        var valPunto = punto.Valor;
                        var actsXPunto = new XPQuery<v_Actividad>(session).Where(x => x.Punto.Id == punto.Id).ToList();

                        var puntaje = valPunto / actsXPunto.Count;

                        var auditoria = new XPQuery<NuevaAuditoria>(session).Where(x => x.Id == actividad.Idaud.Id).FirstOrDefault();

                        auditoria.calificacionFinal += Math.Round(puntaje, 2);

                        auditoria.Save();
                        actividad.Save();
                        uow3.CommitChanges();
                        //**
                    }
                }
                else
                {
                    using (UnitOfWork uow2 = new UnitOfWork())
                    {
                        actividad.status = nuevostatus;
                        actividad.Save();
                        uow2.CommitChanges();
                    }
                }
            }
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Debes agregar comentarios para devolver actividad.');", true);
            //if(usuario == "Evaluador" || usuario == "AdministratorEDC" || usuario == "Admin")
            //{
            //    nuevostatus = "Por realizar";
            //    try
            //    {
            //        //codigo de mail
            //    }
            //    catch(Exception e)
            //    {
            //        //codigo de mail
            //    }
            //}
            //else
            //{
            //    nuevostatus = "En revisión";
            //    if (act.Evaluador != null && act.Evaluador.email != null)
            //    {
            //        string mailEvaluador = act.Evaluador.email;
            //        List<string> ev = new List<string>();
            //        ev.Add(mailEvaluador);

            //        string body = Util.correoActividadSubida(Util.getusuario(), "Act-" + act.Idactividad.Id + "  " + mailEvaluador + "  ", act.Evaluador.Nombre);
            //        if (ev.Count > 0)
            //        {
            //            Util.SendMail(mailEvaluador, "EDC-III - Actividad por evaluar EDCII", body, ev, "");
            //            Util.SendMail("uriel.perlop@gmail.com", "EDC-III - Actividad por evaluar EDCII", body, ev, "");
            //        }
            //        else Response.Write("No hay mail de evaluador");

            //        //if (correos.Count > 0)
            //        //{
            //        //    string body2 = Util.correoAvisaGerente(Util.getusuario(), audact.Idactividad.Code, audact.Evaluador.Nombre);
            //        //    Util.SendMail(mailEvaluador, "EDC-II - Actividad enviada a revisión por su Distribuidor", body2, correos, "");
            //        //}
            //        //else Response.Write("No hay mail de Gerente de cuenta");
            //    }
            //}
            //}
        }

        public string guardarSubida(string comentarios, FileUpload postedFile)
        {
            DevExpress.Xpo.Session session = Util.getsession();
            string usuario = Util.getrol();
            string nomusuario = Util.getusuario();
            string id = Request.QueryString["id"];
            string nuevostatus = "";

            Usuario userDist = new XPQuery<Usuario>(session).FirstOrDefault(x => x.UserName.ToString() == actividad.Idaud.Distribuidor.nombredist.ToString());//Usuario distribuidor

            var act = new XPQuery<NuevaAuditoriaActividad>(session).FirstOrDefault(x => x.Oid.ToString() == id);

            if (actividad != null && usuario == "Distribuidor")
            {
                nuevostatus = "En revisión";

                try
                {
                    DevExpress.Persistent.BaseImpl.FileData dataF = null;
                    string clav = "";

                    if (postedFile.HasFiles)
                    {
                        if (postedFile.PostedFiles.Count() > 3)
                        {
                            return "Excede numero de archivos.";
                        }
                        else
                        {
                            Archivos up = new Archivos(session);
                            up.comentario = comentarios;
                            up.usuario = usuario;
                            up._auditoriaActividad = act;
                            up.substatus = nuevostatus;

                            foreach (var file in postedFile.PostedFiles)
                            {
                                dataF = new DevExpress.Persistent.BaseImpl.FileData(session);
                                var fileStream = file.InputStream;
                                dataF.LoadFromStream(file.FileName, fileStream);

                                using (UnitOfWork uow = new UnitOfWork())
                                {
                                    switch (postedFile.PostedFiles.IndexOf(file))
                                    {
                                        case 0:
                                            up.ArchivoImportar = dataF;
                                            break;
                                        case 1:
                                            up.ArchivoImportar2 = dataF;
                                            break;
                                        case 2:
                                            up.ArchivoImportar3 = dataF;
                                            break;
                                    }
                                    up.Save();
                                    uow.CommitChanges();
                                    clav = up.Oid.ToString();
                                }

                                using (UnitOfWork uow = new UnitOfWork())
                                {
                                    //Archivos up = new XPQuery<Archivos>(session).FirstOrDefault(x => x.Oid.ToString() == clav);
                                    switch (postedFile.PostedFiles.IndexOf(file))
                                    {
                                        case 0:
                                            if (up.ArchivoImportar != null)
                                            {
                                                up.ArchivoImportar.FileName = clav + up.ArchivoImportar.FileName;
                                            }
                                            break;
                                        case 1:
                                            if (up.ArchivoImportar2 != null)
                                            {
                                                up.ArchivoImportar2.FileName = clav + up.ArchivoImportar2.FileName;
                                            }
                                            break;
                                        case 2:
                                            if (up.ArchivoImportar3 != null)
                                            {
                                                up.ArchivoImportar3.FileName = clav + up.ArchivoImportar3.FileName;
                                            }
                                            break;
                                    }
                                    uow.CommitChanges();
                                }

                                up.Save();
                                //guarda en disco
                                string fileName = dataF.FileName;
                                string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\archivos\\Evidencia\\";

                                FileStream fileStream1 = System.IO.File.Create(ruta + fileName);

                                dataF.SaveToStream(fileStream1);
                                fileStream1.Close();
                                Encoding iso = Encoding.GetEncoding("ISO-8859-1");

                                //return "";
                            }

                            if (actividad != null)
                            {
                                using (UnitOfWork uow3 = new UnitOfWork())
                                {
                                    //**Cambia estado de la actividad
                                    //actividad.fechacomp = DateTime.Now;
                                    actividad.status = nuevostatus;

                                    actividad.Save();
                                    uow3.CommitChanges();
                                    //**

                                    return "Ok";
                                }
                            }
                            else
                            {
                                using (UnitOfWork uow2 = new UnitOfWork())
                                {
                                    actividad.status = nuevostatus;
                                    actividad.Save();
                                    uow2.CommitChanges();

                                    return "Ok";
                                }
                            }
                        }
                    }
                    else
                    {
                        return "Sin archivos";
                    }

                    #region
                    //PARA LEER EL ARCHIVO CARGADO
                    //if (postedFile.HasFile)
                    //{

                    //    if (postedFile != null)
                    //    {
                    //        var fileStream = postedFile.FileContent;
                    //        dataF.LoadFromStream(postedFile.FileName, fileStream);

                    //        // Procesa el archivo aquí (por ejemplo, guarda el archivo en el servidor)
                    //        //string fileName = Path.GetFileName(postedFile.FileName);
                    //        //postedFile.SaveAs(HttpContext.Current.Server.MapPath("~/Uploads/" + fileName));
                    //    }

                    //}

                    //using (UnitOfWork uow = new UnitOfWork())
                    //{
                    //    Archivos up = new Archivos(session);
                    //    up.comentario = comentarios;

                    //    if (dataF != null)
                    //    {
                    //        up.ArchivoImportar = dataF;
                    //    }

                    //    up.usuario = usuario;

                    //    if (actividad != null)
                    //    {
                    //        up._auditoriaActividad = act;
                    //    }

                    //    string status = actividad.status;

                    //    up.substatus = nuevostatus;
                    //    up.Save();
                    //    uow.CommitChanges();
                    //    clav = up.Oid.ToString();
                    //}

                    //if (postedFile.HasFile)
                    //{
                    //    using (UnitOfWork uow = new UnitOfWork())
                    //    {
                    //        Archivos up = new XPQuery<Archivos>(session).FirstOrDefault(x => x.Oid.ToString() == clav);
                    //        if (up.ArchivoImportar != null)
                    //        {
                    //            up.ArchivoImportar.FileName = clav + up.ArchivoImportar.FileName;
                    //        }
                    //        up.Save();
                    //        uow.CommitChanges();
                    //    }

                    //    //guarda en disco
                    //    string fileName = dataF.FileName;
                    //    string ruta = AppDomain.CurrentDomain.BaseDirectory + "\\archivos\\Evidencia\\";

                    //    FileStream fileStream1 = System.IO.File.Create(ruta + fileName);

                    //    dataF.SaveToStream(fileStream1);
                    //    fileStream1.Close();
                    //    Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                    //}
                    #endregion
                }
                catch (Exception e)
                {
                    return e.Message;
                    //manda correo de error
                }

            }
            else
            {
                return "No Distribuidor";
            }
        }

        [WebMethod]
        public static string regresaActividad(string idact)
        {
            DevExpress.Xpo.Session session = Util.getsession();


            var acti = new XPQuery<NuevaAuditoriaActividad>(session).Where(x => x.Oid.ToString() == idact).FirstOrDefault();

            if (acti != null)
            {
                acti.status = "Por realizar";
                acti.Save();

                return "1";
            }
            else
            {
                return "-1";
            }
        }


    }
}