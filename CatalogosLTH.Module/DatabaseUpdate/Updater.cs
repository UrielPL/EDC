using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Strategy;
using CatalogosLTH.Module.BusinessObjects;

namespace CatalogosLTH.Module.DatabaseUpdate {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            //string name = "MyName";
            //DomainObject1 theObject = ObjectSpace.FindObject<DomainObject1>(CriteriaOperator.Parse("Name=?", name));
            //if(theObject == null) {
            //    theObject = ObjectSpace.CreateObject<DomainObject1>();
            //    theObject.Name = name;
            //}

            SecuritySystemRole adminRole = ObjectSpace.FindObject<SecuritySystemRole>(
       new BinaryOperator("Name", SecurityStrategy.AdministratorRoleName));
            if (adminRole == null)
            {
                adminRole = ObjectSpace.CreateObject<SecuritySystemRole>();
                adminRole.Name = SecurityStrategy.AdministratorRoleName;
                adminRole.IsAdministrative = true;
                adminRole.Save();
            }

            Usuario user1 = ObjectSpace.FindObject<Usuario>(
     new BinaryOperator("UserName", "admin"));
            if (user1 == null)
            {
                user1 = ObjectSpace.CreateObject<Usuario>();
                user1.UserName = "admin";
                // Set a password if the standard authentication type is used. 
                user1.SetPassword("");
                user1.Save();
                user1.Roles.Add(adminRole);
                user1.Session.CommitTransaction();
            }

            Usuario user2 = ObjectSpace.FindObject<Usuario>(
            new BinaryOperator("UserName", "administrator"));
            if (user2 == null)
            {
                user2 = ObjectSpace.CreateObject<Usuario>();
                user2.UserName = "administrator";
                // Set a password if the standard authentication type is used. 
                user2.SetPassword("Aq12wsde3");
                user2.Save();
                user2.Roles.Add(adminRole);
                user2.Session.CommitTransaction();
            }

            Usuario user3 = ObjectSpace.FindObject<Usuario>(
            new BinaryOperator("UserName", "newadmin"));
            if (user3 == null)
            {
                user3 = ObjectSpace.CreateObject<Usuario>();
                user3.UserName = "newadmin";
                // Set a password if the standard authentication type is used. 
                user3.SetPassword("newadmin24");
                user3.Save();
                user3.Roles.Add(adminRole);
                user3.Session.CommitTransaction();
            }


        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
    }
}
