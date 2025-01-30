using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;

namespace CatalogosLTH.Web
{
    public class MyAuthenticationStandard : AuthenticationStandard
    {
        public MyAuthenticationStandard() : base() { }
        public MyAuthenticationStandard(Type userType, Type logonParametersType) : base(userType, logonParametersType) { }
        private AuthenticationStandardLogonParameters parameters = new AuthenticationStandardLogonParameters();
        public override bool AskLogonParametersViaUI
        {
            get
            {
                return false;
            }
        }
        public override object LogonParameters
        {
            get
            {
                string user = Util.getUserLogin();
                string pass = Util.getPassLogin();

//              string user = Session["user"] as string;
                parameters.UserName = user;
                parameters.Password = pass;
                return parameters;
            }
        }

        public override void Logoff()
        {
            base.Logoff();
            parameters = new AuthenticationStandardLogonParameters();

            //parameters;
            //   customLogonParameters = new CustomLogonParameters();
        }
        public override bool IsLogoffEnabled
        {
            get { return true; }
        }
        public override object Authenticate(IObjectSpace objectSpace)
        {
            //return base.Authenticate(objectSpace);
            if (string.IsNullOrEmpty(parameters.UserName))
                throw new ArgumentException(SecurityExceptionLocalizer.GetExceptionMessage(SecurityExceptionId.UserNameIsEmpty));
            IAuthenticationStandardUser user = (IAuthenticationStandardUser)objectSpace.FindObject(UserType, CriteriaOperator.Parse("UserName=?", parameters.UserName));

            if(user==null||!user.ComparePassword(parameters.Password))
            {
                throw new AuthenticationException(parameters.UserName, SecurityExceptionLocalizer.GetExceptionMessage(SecurityExceptionId.RetypeTheInformation));
            }
            return user;


        }
        /* public override object Authenticate(DevExpress.ExpressApp.ObjectSpace objectSpace)
         {
             if (string.IsNullOrEmpty(parameters.UserName))
                 throw new ArgumentException(SecurityExceptionLocalizer.GetExceptionMessage(SecurityExceptionId.UserNameIsEmpty));

             IAuthenticationStandardUser user = (IAuthenticationStandardUser)objectSpace.FindObject(UserType, CriteriaOperator.Parse("UserName = ?", parameters.UserName));

             if (user == null || !user.ComparePassword(parameters.Password))
             {
                 throw new AuthenticationException(parameters.UserName, SecurityExceptionLocalizer.GetExceptionMessage(SecurityExceptionId.RetypeTheInformation));
             }
             return user;
         }*/
    }
    public class MyAuthenticationStandard<UserType, LogonParametersType> : MyAuthenticationStandard
    {
        public MyAuthenticationStandard() : base(typeof(UserType), typeof(LogonParametersType)) { }
    }
    public class MyAuthenticationStandard<UserType> : MyAuthenticationStandard<UserType, AuthenticationStandardLogonParameters>
        where UserType : IAuthenticationStandardUser
    {
    }
}