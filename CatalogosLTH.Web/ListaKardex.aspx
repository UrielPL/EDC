﻿<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="ListaKardex.aspx.cs" Inherits="CatalogosLTH.Web.ListaKardex" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function encripta(plain) {
            window.location.href = 'Kardex.aspx?idKardex=' + plain;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="section  banner-nivel remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="col-md-12">
					<h1>Historial Kardex</h1>
				</div>
			</div>
		</div>
	</div>
    <div class="container">
        <div class="col-md-6 valign-wrapper">
            <dx:ASPxComboBox ID="listDist" OnSelectedIndexChanged="listDist_SelectedIndexChanged" runat="server" ValueType="System.String" Theme="ThemeLTH"></dx:ASPxComboBox>
        </div>
        <div class="col-md-12">
            <dx:ASPxGridView ID="grVWkardex" runat="server" Theme="ThemeLTH">
                <Settings ShowFilterBar="Visible" ShowHeaderFilterButton="True" />
                <ClientSideEvents RowDblClick="function(s, e){
                     s.GetRowValues(e.visibleIndex, 'Clave', function (data) {
                          encripta(data);                                       
                    });
                }" />
            </dx:ASPxGridView>
        </div>
        <div class="col-md-12">
            <a href="/NivelDistribuidor.aspx" class="btn btn-default">REGRESAR</a>
        </div>
    </div>
</asp:Content>
