﻿<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="RevisionAutoAuditoria.aspx.cs" Inherits="CatalogosLTH.Web.RevisionAutoAuditoria" %>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="section  banner-actividades remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<h1>Revisión Auto Auditoría</h1>
				</div>
			</div>
		</div>
	</div>
    <div class="container">
          <div class="row">
                        <h3>Distribuidor</h3>
                        <dx:ASPxLabel ID="lblDist" runat="server" Text="ASPxLabel" Theme="ThemeLTH"></dx:ASPxLabel>
                    </div>
					<div class="row">
                        <h3>Tipo auditoría: </h3>
                        <dx:ASPxLabel ID="lblTipo" runat="server" Text="ASPxLabel" Theme="ThemeLTH"></dx:ASPxLabel>
                    </div>
                    <div class="row">
                        <h3>Área: </h3>
                        <dx:ASPxLabel ID="lblArea" runat="server" Text="ASPxLabel" Theme="ThemeLTH"></dx:ASPxLabel>
                    </div>
                    <div class="row">
                        <h3>Punto: </h3>
                        <dx:ASPxLabel ID="lblPunto" runat="server" Text="ASPxLabel" Theme="ThemeLTH"></dx:ASPxLabel>
                    </div>
                    <div class="row">
                        <h3>Evaluador: </h3>
                        <dx:ASPxLabel ID="lblEvaluador" runat="server" Text="ASPxLabel" Theme="ThemeLTH"></dx:ASPxLabel>
                    </div>
        
        <div class="row">
           <dx:ASPxTextBox ID="ASPxTextBox1" runat="server" Width="170px"></dx:ASPxTextBox>
        </div>
         <div class="row">
                 <dx:ASPxCheckBox ID="CheckAceptado" runat="server"  CheckState="Unchecked" Visible="True" >
                               <CheckedImage Height="36px" Url="~/Images/Toggle On-64.png" Width="120px">
                               </CheckedImage>
                               <UncheckedImage Height="36px" Url="~/Images/Toggle Off-64.png" Width="120px">
                               </UncheckedImage>
                           </dx:ASPxCheckBox>
            </div>
    </div>
</asp:Content>
