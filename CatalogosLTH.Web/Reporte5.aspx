﻿<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="Reporte5.aspx.cs" Inherits="CatalogosLTH.Web.Reporte5" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link rel="stylesheet" href="css/slicknav.css" />
      <script src="//ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
      <script src="js/jquery.bxslider/jquery.bxslider.min.js"></script>
      <script src="js/jquery.owl-carousel/owl.carousel.js"></script>
      <script src="js/jquery.fancybox/jquery.fancybox.pack.js"></script>
      <script src="js/master.js"></script>  
      <script src="js/jquery.slicknav.js"></script>
      <script src="http://cdnjs.cloudflare.com/ajax/libs/modernizr/2.6.2/modernizr.min.js"></script>   
      <style>
        li{
            font-size:12px; 
        }
    </style>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
     <div class="section  banner-reportes remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<h1>Reportes</h1>
				</div>
			</div>
		</div>
	</div>	
      <%string permiso = ViewState["permiso"] as string; %> 
    <div class="container-fluid" style="padding:3% 0% 3% 0%">  
  <ul class="nav nav-pills nav-justified">
    <li><a href="tablero1.aspx">Tablero 1</a></li>
    <li><a href="tablero2.aspx">Tablero 2</a></li>
    <li><a href="reporte1.aspx">Reporte profesionalización</a></li>
    <li><a href="reporte2.aspx">Nivel de profesionalización por zona</a></li>
    <li><a href="reporte3.aspx">Profesionalización mensual por zona</a></li>
    <%if (permiso != "Distribuidor"){%> 
    <li><a href="reporte4.aspx">Reporte actividades completadas</a></li><%} %>  
    <li class="active"><a href="reporte5.aspx">Actividades completadas por zona por mes</a></li> 
    <%if (permiso != "Distribuidor"){%>           
    <li><a href="reporte6.aspx">Profesionalización por gerente de cuenta</a></li>
    <li><a href="reporte7.aspx">Profesionalización por pilar </a></li><%} %>
    <%if (permiso != "h" && permiso!= "y"){%>                      
    <li><a href="reporte8.aspx">Profesionalización mensual y actividades completadas por zona</a></li>
    <li><a href="reporte9.aspx">Profesionalización mensual y actividades completadas por distribuidor</a></li>
    <li class=""><a href="reporte10.aspx">Actividades mensuales por distribuidor</a></li>

    <%} %>
  </ul>
</div>

    <div class="container">
        <div class="page-header">
            <h4>Actividades completadas por zona por mes</h4>
        </div>
        <div class="row">
            
            <div class="col-sm-12">
                <div class="row">
                    <dx:ASPxComboBox ID="cmbYear" runat="server" OnSelectedIndexChanged="cmbYear_SelectedIndexChanged" AutoPostBack="true" Theme="ThemeLTH" ValueType="System.String"></dx:ASPxComboBox>
                </div>
                <div class="row" align="center">
                    <dx:ASPxGridView ID="ASPxGridView1" runat="server" Width="90%" Theme="ThemeLTH"></dx:ASPxGridView>
                </div>
                <div class="row">
                    <div class="col-sm-3">
                    <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Exporta a Excel" CssClass="btn btn-default" OnClick="ASPxButton1_Click" Theme="ThemeLTH"></dx:ASPxButton>
                </div>
                <div class="col-sm-2">
                    <div class="col-sm-1"><dx:ASPxButton ID="btnPdf" runat="server" Text="Exportar a PDF"  CssClass="btn btn-success" OnClick="btnPdf_Click" HorizontalAlign="Right" Theme="ThemeLTH"></dx:ASPxButton></div>
                </div>   
                </div>
                
            </div>
            
        </div>
        <div class="row">
            <dx:ASPxGridViewExporter ID="ASPxGridViewExporter1" runat="server"></dx:ASPxGridViewExporter>
        </div>
    </div>
</asp:Content>
