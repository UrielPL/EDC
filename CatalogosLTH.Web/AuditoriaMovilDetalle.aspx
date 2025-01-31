﻿<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="AuditoriaMovilDetalle.aspx.cs" Inherits="CatalogosLTH.Web.AuditoriaMovilDetalle" %>

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
        .v-center {
            position: relative;
            top: 50%;
            -webkit-transform: translateY(-50%);
            -ms-transform: translateY(-50%);
            transform: translateY(-50%);
        }
        .height100 {
            height: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="section  banner-actividades remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<h1>Actividades a revisar</h1>
				</div>
			</div>
		</div>
	</div>
    <div class="section  remove-bottom">
	    <div class="container">
            <div class="panel panel-default">
                <div class="panel-body">
                    <div class="row text-center">
                        <div class="col-sm-2" style="background-color:#0000E6;padding-left:5px;">
                            <h3 style="color:white">Distribuidor</h3>
                        </div>
                        <div class="col-sm-2" style="background-color:#0000E6;">
                            <h3 style="color:white">Tipo auditoría: </h3>
                        </div>
                        <div class="col-sm-2" style="background-color:#0000E6;">
                            <h3 style="color:white">Área: </h3>
                        </div>
                        <div class="col-sm-3" style="background-color:#0000E6;">
                            <h3 style="color:white">Punto: </h3>
                        </div>
                        <div class="col-sm-3" style="background-color:#0000E6;padding-right:5px;">
                            <h3 style="color:white">Evaluador: </h3>
                        </div>
                    </div>
                    <div class="row text-center" style="height: 13vh;">
                        <div class="col-sm-2 height100">
                            <dx:ASPxLabel ID="lblDist" CssClass="v-center" runat="server" Text="ASPxLabel" Theme="ThemeLTH" ></dx:ASPxLabel>
                        </div>
                        <div class="col-sm-2 height100">
                            <dx:ASPxLabel ID="lblTipo" CssClass="v-center" runat="server" Text="ASPxLabel" Theme="ThemeLTH"></dx:ASPxLabel>
                        </div>
                        <div class="col-sm-2 height100">
                            <dx:ASPxLabel ID="lblArea" CssClass="v-center" runat="server" Text="ASPxLabel" Theme="ThemeLTH"></dx:ASPxLabel>
                        </div>
                        <div class="col-sm-3 height100">
                            <dx:ASPxLabel ID="lblPunto" CssClass="v-center" runat="server" Text="ASPxLabel" Theme="ThemeLTH"></dx:ASPxLabel>
                        </div>
                        <div class="col-sm-3 height100">
                            <dx:ASPxLabel ID="lblEvaluador" CssClass="v-center" runat="server" Text="ASPxLabel" Theme="ThemeLTH"></dx:ASPxLabel>
                        </div>
                    </div>
                </div>
		    </div>
		   <%-- <div class="six columns green-button" align="right">
			    <!--<a href="#filtro" class="add-bottom fancybox">Descargar Excel</a>-->
		    </div>--%>
            <div class="col-sm-12" align="center">
                <dx:ASPxGridView ID="grdActividades" runat="server" Theme="Material"></dx:ASPxGridView>
            </div>
            <div class="col-sm-12" style="margin-top:10px;">
                <label>Nombre Centro de Servicio: </label>
                <dx:ASPxComboBox ID="cmbCS" runat="server" ValueType="System.String" xmlns:dx="devexpress.web" Theme="Material" Width="400px" ></dx:ASPxComboBox>
            </div>
            <div class="col-sm-12">
                <hr/>
                <label>Comentario:</label>
                <dx:ASPxTextBox ID="txtComentario" TextMode="MultiLine" EnableTheming="true" Theme="Material" runat="server" Width="500px"></dx:ASPxTextBox>
            </div>
            <div class="row"></div>
            <div class="col-sm-12">
                <asp:FileUpload EnableTheming="true" Theme="ThemeLTH" ID="FileUpload1" name="FileUpload1" runat="server" />
            </div>
            <div class="col-sm-12">
                <asp:Button ID="btnUpload" runat="server" Text="Enviar a revisión" OnClick="btnUpload_Click" />
                <asp:Button ID="Button1" runat="server" Text="Regresar" CssClass="btn btn-default" OnClick="Button1_Click" />
            </div>
            <div class ="row">
            </div>
	    </div>
    </div>
</asp:Content>
