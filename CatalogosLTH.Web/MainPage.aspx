﻿<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="MainPage.aspx.cs" Inherits="CatalogosLTH.Web.MainPage" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" style="background-color:#E0E0E0">
    <html dir="ltr" lang="es-mx" xml:lang="es-mx" class="yui3-js-enabled">
          <script src="js/animations.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" style="background-color:#E0E0E0;margin:0;padding:0;">
      <%string permiso = ViewState["permiso"] as string; %> 
       <div class="section banner remove-top remove-bottom" id="bibliotecas">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<div class="parallax left">
						<h1>Consulta la Biblioteca de Actividades</h1>
						<p>CAMINO A LA <italic>EXCELENCIA</italic></p>
					</div>
				</div>
			</div>
		</div>
	</div>	
	
	<div class="section remove-top remove-bottom" id="profesionalizacion">
			<div class="row">
				<div class="special columns sector1 parallax background first left">
					<div class="parallax text left">
						<img src="img/icono1.png" class="icono"/>
                         <%if (permiso != "Evaluador"){%>	 
						<h1>Consulta tu Nivel de Profesionalización</h1>
						<p>Ubica tu conocimiento e identifica tu nivel de profesionalización.</p>
						<a href="niveldistribuidor.aspx" class="button add-bottom">CONOCER MI NIVEL</a>
                         <%}%>
                         <%if (permiso == "Evaluador"){%>	 
						<h1>Evalúa las actividades disponibles</h1>
						<p>Revisa las actividades enviadas por los distribuidores.</p>
						<a href="actividadesrevisar.aspx" class="button add-bottom">EVALUAR</a>
                         <%}%>
					</div>
				</div>
				<div class="special columns parallax right second">
					<img src="img/Home 02.jpg" class="u-full-width" style="display:block; height: 398.55px;"/>
				</div>
			</div>
	</div>
	
	<div class="section remove-top remove-bottom" id="plan-negocio">
			<div class="row">
				<div class="special columns parallax first left">
					<img src="img/Home 03.jpg" class="u-full-width" style="display:block; height:398.55px;"/>
				</div>
				<div class="special columns sector2 parallax right second">
					<div class="parallax text right">
						<img src="img/icono2.png" class="icono"/>
						<h1>Conoce el Plan de Negocio</h1>
						<p>Para estar en sintonía <br/>compartamos el mismo plan.</p>
						<a href="listaactividades.aspx" class="button add-bottom">DESCUBRIR EL PLAN</a>
					</div>
				</div>
			</div>
	</div>
	
	<div class="section remove-top remove-bottom" id="actividades">
		<div class="row">
			<div class="special columns sector3 parallax left first">
				<div class="parallax text left">
					<img src="img/icono3.png" class="icono"/>
					<h1>Revisa las Actividades Completadas</h1>
					<p>Consulta las acciones que has desarrollado.</p>
					<a href="actividadescompletadas.aspx" class="button add-bottom">VER AVANCES</a>
				</div>
			</div>
			<div class="special columns parallax right second">
				<img src="img/img3.png" class="u-full-width" style="display:block; height: 398.55px;"/>
			</div>
		</div>
	</div>
	
</asp:Content>