<%@ Page Title="" Language="C#" MasterPageFile="~/lthMasterPage.Master" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="CatalogosLTH.Web.Reportes" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

      <div class="container" style="margin-top:3%">
        <div class="dropdown">
        <button class="btn btn-default dropdown-toggle" type="button" id="menu1" data-toggle="dropdown">REPORTES
        <span class="caret"></span></button>
        <ul class="dropdown-menu" role="menu" aria-labelledby="menu1">
          <li role="presentation" class="dropdown-header">Tableros</li>
          <li role="presentation"><a role="menuitem" tabindex="-1" href="/tablero1.aspx">Tablero 1</a></li>
          <li role="presentation"><a role="menuitem" tabindex="-1" href="/tablero2.aspx">Tablero 2</a></li>
          <li role="presentation" class="divider"></li>
          <li role="presentation" class="dropdown-header">Reportes</li>
          <li role="presentation"><a role="menuitem" tabindex="-1" href="/reporte1.aspx">Reporte profesionalización</a></li>
          <li role="presentation"><a role="menuitem" tabindex="-1" href="/reporte2.aspx">Nivel de profesionalización por zona</a></li>
          <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Profesionalización mensual por zona</a></li>
          <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Reporte actividades completadas</a></li>
          <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Actividades completadas por zona por mes</a></li>
          <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Profesionalización por gerente de cuenta</a></li>
          <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Profesionalización por pilar </a></li>
        </ul>
      </div>
  </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
</asp:Content>
