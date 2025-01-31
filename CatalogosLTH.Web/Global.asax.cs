﻿using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Web;
using DevExpress.Web;
using DevExpress.Xpo.DB;

namespace CatalogosLTH.Web {
    public class Global : System.Web.HttpApplication {
        public Global() {
            InitializeComponent();
        }
        protected void Application_Start(Object sender, EventArgs e) {
            DevExpress.ExpressApp.FrameworkSettings.DefaultSettingsCompatibilityMode = DevExpress.ExpressApp.FrameworkSettingsCompatibilityMode.v20_1;
          //  JobScheduler.Start();
#if EASYTEST
            DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
#endif
        }
        protected void Session_Start(Object sender, EventArgs e) {
            WebApplication.SetInstance(Session, new CatalogosLTHAspNetApplication());
            WebApplication.Instance.SwitchToNewStyle();
           // WebApplication.Instance.ConnectionString =
        // MySqlConnectionProvider.GetConnectionString("172.93.106.146", "lth", "output", "lth2");
          if(ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
              WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
          }
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
            if (System.Diagnostics.Debugger.IsAttached && WebApplication.Instance.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                WebApplication.Instance.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
            WebApplication.Instance.Setup();
            WebApplication.Instance.Start();
            //if (Request.QueryString["u"]!=null)
            //{
            //    HttpContext.Current.Session["usuario"] = Request.QueryString["u"].ToString();
            //}
            //else
            //{
            //    HttpContext.Current.Session["usuario"] = ". ";
            //}
            //if (Request.QueryString["p"]!=null)
            //{
            //    HttpContext.Current.Session["pass"] = Request.QueryString["p"].ToString();
            //}
            //else
            //{
            //    HttpContext.Current.Session["pass"] = " ";
            //}

        }
        protected void Application_BeginRequest(Object sender, EventArgs e) {
        }
        protected void Application_EndRequest(Object sender, EventArgs e) {
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e) {
        }
        
        protected void Session_End(Object sender, EventArgs e) {
            WebApplication.LogOff(Session);
            WebApplication.DisposeInstance(Session);
        }
        protected void Application_End(Object sender, EventArgs e) {
        }
        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
        }
        #endregion
    }
}
