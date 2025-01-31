﻿<%@ Page Language="C#"  Debug="true"  AutoEventWireup="true" Inherits="LoginPage" EnableViewState="false" CodeBehind="Login.aspx.cs" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.ExpressApp.Web.Templates.ActionContainers" TagPrefix="cc2" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.ExpressApp.Web.Templates.Controls" TagPrefix="tc" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.ExpressApp.Web.Controls" TagPrefix="cc4" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.ExpressApp.Web.Templates" TagPrefix="cc3" %>
<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>Logon</title>
    <style>
        .btnClase{

          padding: 10px;
          color: white;
          background-color: #906EA6;
          border: none;
        }
    </style>
</head>
<body class="Dialog">
    <div id="PageContent" class="PageContent DialogPageContent">
        <form id="form1" runat="server">
        <cc4:ASPxProgressControl ID="ProgressControl" runat="server" />
        <div id="Content" runat="server" />
        </form>
        <div >
            
        </div>
    </div>
    <script>
        
        var divelement = document.getElementById("Logon_PopupActions");

        const button = document.createElement('input');
        button.type = "button";
        button.classList.add('btnClase');
        button.value = 'CLARIOS ACCOUNT';


        divelement.appendChild(button);

        function loger() {
            window.location.replace("/authform.aspx");
        }

        document.querySelector('.btnClase').addEventListener('click', loger)
    </script>
</body>
</html>
