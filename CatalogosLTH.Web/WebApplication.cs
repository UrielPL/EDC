using System;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.Web;
using System.Collections.Generic;
using DevExpress.ExpressApp.Xpo;
using System.Web;
using DevExpress.ExpressApp.Security;

namespace CatalogosLTH.Web {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/DevExpressExpressAppWebWebApplicationMembersTopicAll.aspx
    public partial class CatalogosLTHAspNetApplication : WebApplication {
        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule module2;
        private CatalogosLTH.Module.CatalogosLTHModule module3;
        private DevExpress.ExpressApp.FileAttachments.Web.FileAttachmentsAspNetModule fileAttachmentsAspNetModule1;
        private DevExpress.ExpressApp.Security.SecurityModule securityModule1;
        private DevExpress.ExpressApp.Security.SecurityStrategyComplex securityStrategyComplex1;
        private DevExpress.ExpressApp.HtmlPropertyEditor.Web.HtmlPropertyEditorAspNetModule htmlPropertyEditorAspNetModule1;
        private DevExpress.ExpressApp.ReportsV2.ReportsModuleV2 reportsModuleV21;
        private DevExpress.ExpressApp.ReportsV2.Web.ReportsAspNetModuleV2 reportsAspNetModuleV21;
        private AuthenticationStandard authenticationStandard1;
        private CatalogosLTH.Module.Web.CatalogosLTHAspNetModule module4;



        protected override void OnLoggedOn(LogonEventArgs args)
        {
            base.OnLoggedOn(args);
            ISecurityUserWithRoles currentUser = (ISecurityUserWithRoles)SecuritySystem.CurrentUser;
            if (currentUser.IsUserInRole("Distribuidor"))
            {
                WebApplication.Redirect("mainpage.aspx", true);
                //string newUrl = HttpContext.Current.Response.RedirectLocation;
                //HttpContext.Current.Response.RedirectLocation = newUrl.Replace("/default.aspx", "/listaactividades.aspx");
            }
            else if (currentUser.IsUserInRole("Evaluador"))
            {
                WebApplication.Redirect("mainpage.aspx", true);
                //string newUrl = HttpContext.Current.Response.RedirectLocation;
                //HttpContext.Current.Response.RedirectLocation = newUrl.Replace("/default.aspx", "/crearAuditoria.aspx");
            }
            else if (currentUser.IsUserInRole("AdministratorEDC"))
            {
                WebApplication.Redirect("mainpage.aspx", true);
                //string newUrl = HttpContext.Current.Response.RedirectLocation;
                //HttpContext.Current.Response.RedirectLocation = newUrl.Replace("/default.aspx", "/crearAuditoria.aspx");
            }
            else if (currentUser.IsUserInRole("DC Administrativo"))
            {
                WebApplication.Redirect("mainpage.aspx", true);
                //string newUrl = HttpContext.Current.Response.RedirectLocation;
                //HttpContext.Current.Response.RedirectLocation = newUrl.Replace("/default.aspx", "/crearAuditoria.aspx");
            }


            //if (SecuritySystem.CurrentUserId != null && SecuritySystem.CurrentUserId.ToString().Equals("Edgar"))


        }


        public CatalogosLTHAspNetApplication() {
            InitializeComponent();
           
        }
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            CreateXPObjectSpaceProvider(args.ConnectionString, args);
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
        }
        private void CreateXPObjectSpaceProvider(string connectionString, CreateCustomObjectSpaceProviderEventArgs e) {
            System.Web.HttpApplicationState application = (System.Web.HttpContext.Current != null) ? System.Web.HttpContext.Current.Application : null;
            IXpoDataStoreProvider dataStoreProvider = null;
            if(application != null && application["DataStoreProvider"] != null) {
                dataStoreProvider = application["DataStoreProvider"] as IXpoDataStoreProvider;
                e.ObjectSpaceProvider = new XPObjectSpaceProvider(dataStoreProvider, true);
            }
            else {
                if(!String.IsNullOrEmpty(connectionString)) {
                    connectionString = DevExpress.Xpo.XpoDefault.GetConnectionPoolString(connectionString);
                    dataStoreProvider = new ConnectionStringDataStoreProvider(connectionString, true);
                }
                else if(e.Connection != null) {
                    dataStoreProvider = new ConnectionDataStoreProvider(e.Connection);
                }
                if (application != null) {
                    application["DataStoreProvider"] = dataStoreProvider;
                }
                e.ObjectSpaceProvider = new XPObjectSpaceProvider(dataStoreProvider, true);
            }
        }
        private void CatalogosLTHAspNetApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e) {
#if EASYTEST
            e.Updater.Update();
            e.Handled = true;
#else
            if(System.Diagnostics.Debugger.IsAttached) {
                e.Updater.Update();
                e.Handled = true;
            }
            else {
                string message = "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application.\r\n" +
                    "This error occurred  because the automatic database update was disabled when the application was started without debugging.\r\n" +
                    "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " +
                    "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " +
                    "or manually create a database using the 'DBUpdater' tool.\r\n" +
                    "Anyway, refer to the following help topics for more detailed information:\r\n" +
                    "'Update Application and Database Versions' at http://help.devexpress.com/#Xaf/CustomDocument2795\r\n" +
                    "'Database Security References' at http://help.devexpress.com/#Xaf/CustomDocument3237\r\n" +
                    "If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/";

                if(e.CompatibilityError != null && e.CompatibilityError.Exception != null) {
                    message += "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
                }
                throw new InvalidOperationException(message);
            }
#endif
        }
        private void InitializeComponent() {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule();
            this.fileAttachmentsAspNetModule1 = new DevExpress.ExpressApp.FileAttachments.Web.FileAttachmentsAspNetModule();
            this.htmlPropertyEditorAspNetModule1 = new DevExpress.ExpressApp.HtmlPropertyEditor.Web.HtmlPropertyEditorAspNetModule();
            this.securityModule1 = new DevExpress.ExpressApp.Security.SecurityModule();
            this.module3 = new CatalogosLTH.Module.CatalogosLTHModule();
            this.module4 = new CatalogosLTH.Module.Web.CatalogosLTHAspNetModule();
            this.securityStrategyComplex1 = new DevExpress.ExpressApp.Security.SecurityStrategyComplex();
            this.reportsModuleV21 = new DevExpress.ExpressApp.ReportsV2.ReportsModuleV2();
            this.reportsAspNetModuleV21 = new DevExpress.ExpressApp.ReportsV2.Web.ReportsAspNetModuleV2();
            this.authenticationStandard1 = new DevExpress.ExpressApp.Security.AuthenticationStandard();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // securityStrategyComplex1
            // 
            this.securityStrategyComplex1.Authentication = this.authenticationStandard1;
            this.securityStrategyComplex1.RoleType = typeof(DevExpress.ExpressApp.Security.Strategy.SecuritySystemRole);
            this.securityStrategyComplex1.UserType = typeof(CatalogosLTH.Module.BusinessObjects.Usuario);
            // 
            // reportsModuleV21
            // 
            this.reportsModuleV21.EnableInplaceReports = true;
            this.reportsModuleV21.ReportDataType = typeof(DevExpress.Persistent.BaseImpl.ReportDataV2);
            // 
            // authenticationStandard1
            // 
            this.authenticationStandard1.LogonParametersType = typeof(DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters);
            // 
            // CatalogosLTHAspNetApplication
            // 
            this.ApplicationName = "CatalogosLTH";
            this.CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
            this.LinkNewObjectToParentImmediately = false;
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.securityModule1);
            this.Modules.Add(this.htmlPropertyEditorAspNetModule1);
            this.Modules.Add(this.reportsModuleV21);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.module4);
            this.Modules.Add(this.fileAttachmentsAspNetModule1);
            this.Modules.Add(this.reportsAspNetModuleV21);
            this.Security = this.securityStrategyComplex1;
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.CatalogosLTHAspNetApplication_DatabaseVersionMismatch);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        public void Logon(string userName, string password)
        {
            ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).UserName = userName;
            ((AuthenticationStandardLogonParameters)SecuritySystem.LogonParameters).Password = password;
            Logon(null);
        }
    }
}
