﻿<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="ActividadesUsuario.aspx.cs" Inherits="CatalogosLTH.Web.ActividadesUsuario" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="section banner-reportes remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<h1>Actividades</h1>
				</div>
			</div>
		</div>
	</div>	

    <div class="container">
        <div class="col-md-12">
            <div class="card">
                <div class="card-content">
                    <div class="row">
                        <h2 class="card-title">Actividades de inicio de sesión</h2>
                        <div class="divider"></div>
                    </div>
                    <div class="row section">
                        <div class="col-md-4">
                            <dx:ASPxComboBox ID="selAnio" runat="server" ValueType="System.String" OnSelectedIndexChanged="selAnio_SelectedIndexChanged" AutoPostBack="True" Theme="ThemeLTH"></dx:ASPxComboBox>
                        </div>
                        <div class="col-md-4">
                            <dx:ASPxComboBox ID="selUser" runat="server" ValueType="System.String" OnSelectedIndexChanged="selUser_SelectedIndexChanged" AutoPostBack="True" Theme="ThemeLTH"></dx:ASPxComboBox>
                        </div>
                    </div>
                    <div class="row">
                        <table class="table table-hover">
                            <thead>
                                <tr>
                                    <th>Ene</th>
                                    <th>Feb</th>
                                    <th>Mar</th>
                                    <th>Abr</th>
                                    <th>Mayo</th>
                                    <th>Jun</th>
                                    <th>Jul</th>
                                    <th>Ago</th>
                                    <th>Sep</th>
                                    <th>Oct</th>
                                    <th>Nov</th>
                                    <th>Dic</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td id="0"><%:cantAct[0]%></td>
                                    <td id="1"><%:cantAct[1]%></td>
                                    <td id="2"><%:cantAct[2]%></td>
                                    <td id="3"><%:cantAct[3]%></td>
                                    <td id="4"><%:cantAct[4]%></td>
                                    <td id="5"><%:cantAct[5]%></td>
                                    <td id="6"><%:cantAct[6]%></td>
                                    <td id="7"><%:cantAct[7]%></td>
                                    <td id="8"><%:cantAct[8]%></td>
                                    <td id="9"><%:cantAct[9]%></td>
                                    <td id="10"><%:cantAct[10]%></td>
                                    <td id="11"><%:cantAct[11]%></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>

    </script>
</asp:Content>