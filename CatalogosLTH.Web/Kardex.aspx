﻿<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="Kardex.aspx.cs" Inherits="CatalogosLTH.Web.Kardex" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <style>
        .table tr td:first-child {
            color: white;
            background-color: #0076CA;
        }
        .tagBanner {
            color :white;
            background-color: #00B259;
        }
        .avance .col-md-4:nth-child(2) h5 {
            border-left: 1px solid gray;
            border-right: 1px solid gray;
        }
        .avance .col-md-4 {
            padding: 10px;
        }
        .informe div:not(:first-child){
            padding: 7px;
        }
        .vcenter {
            display: inline-block;
            vertical-align: middle;
            float: none;
        }
    </style>
    <div class="section  banner-nivel remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="col-md-12">
					<h1>Consulta kardex</h1>
				</div>
			</div>
		</div>
	</div>
    <div class="container">
        <div class="row">
            <div class="col-md-8">
                <table class="table table-bordered">
                    <tr>
                        <td>Distribuidor:</td>
                        <td><%=actualKardex.Distribuidor.nombredist %></td>
                    </tr>
                    <tr>
                        <td>Ejectuvio de Desarrollo Comercial:</td>
                        <td><%=actualKardex.Distribuidor.nombreusuario %></td>
                    </tr>
                    <tr>
                        <td>Fecha de Visita Gte. Cuenta:</td>
                        <td><%=actualKardex.FechaVisita.ToShortDateString() %></td>
                    </tr>
                    <tr>
                        <td>Gerente de Cuenta:</td>
                        <td><%=actualKardex.GerenteCuenta %></td>
                    </tr>
                </table>
            </div>
            <div class="col-md-4">
                <img width="100%" src="img/logo.png" />
            </div>
            <div class="tagBanner col-md-12 text-center">
                <h4>AVANCE AL MOMENTO</h4>
            </div>
            <div class="col-md-12 text-center avance">
                <div class="col-md-4">
                    <h5>Profesionalizacion</h5>
                    <strong><%=Math.Round(actualKardex.Profesionalizacion,2) %></strong>
                </div>
                <div class="col-md-4">
                    <h5>Profesionalizacion Objetivo</h5>
                    <strong><%=actualKardex.ProfesionalizacionObjetivo %></strong>
                </div>
                <div class="col-md-4">
                    <h5>AVANCE</h5>
                    <strong style="background-color: yellow;padding:10px 45%";><%=Math.Round(actualKardex.AvanceProf,2) %>%</strong>
                </div>
                <div class="col-md-4">
                    <h5>Actividades Terminadas</h5>
                    <strong><%=actualKardex.ActTerminadas %></strong>
                </div>
                <div class="col-md-4">
                    <h5>Actividades Objetivo</h5>
                    <strong><%=actualKardex.ActObjetivo %></strong>
                </div>
                <div class="col-md-4">
                    <h5>AVANCE</h5>
                    <strong style="background-color: yellow;padding:10px 45%";><%=actualKardex.AvanceAct %></strong>
                </div>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-4 text-center" style="margin-top: 10vh;">
                <strong>Pilares Estratégicos</strong>
            </div>
            <div class="col-md-8">
                <div id="chart_div"></div>
            </div>
        </div>
        <br />
        <div class="row informe">
            <div class="tagBanner col-md-12 text-center">
                <h4>INFORME DE VISITA</h4>
            </div>
            <div style="background-color:#0076CA;" class="col-md-4">
                <strong style="color:white;">Ultimo acceso del ejecutivo</strong>
            </div>
            <div class="col-md-3 text-center">
                <strong><%=actualKardex.UltAcceso.ToShortDateString() %></strong>
            </div>
            <div style="background-color:#0076CA;" class="col-md-3">
                <strong style="color:white;">Dias sin accesar del ejecutivo</strong>
            </div>
            <div class="col-md-2 text-center">
                <strong>7</strong>
            </div>
            <div style="background-color:#0076CA;margin-top:40px;" class="col-md-4">
                <strong style="color:white;">Actividades revisadas con ejecutivo</strong>
            </div>
            <div class="col-md-12" style="padding: 50px 0;">
                <strong><%=actualKardex.ActRevisadasEjec %></strong>
            </div>
            <div class="col-md-8 editGerente">
                <dx:ASPxTextBox ID="txtAct" style="display:inline-block;" runat="server" Width="550px" Theme="ThemeLTH"></dx:ASPxTextBox>
                <dx:ASPxButton ID="btnActualizar1" OnClick="btnActualizar1_Click" runat="server" Text="Guardar" Theme="ThemeLTH"></dx:ASPxButton>
            </div>
            <div class="col-md-12"></div>
            <div style="background-color:#0076CA;margin-top:20px;" class="col-md-4">
                <strong style="color:white;">Acuerdos de la revisión</strong>
            </div>
            <div class="col-md-12" style="padding: 50px 0;">
                <strong><%=actualKardex.AcuerdosRevision %></strong>
            </div>
            <div class="col-md-8 editGerente">
                <dx:ASPxTextBox ID="txtAcu" style="display:inline-block;" runat="server" Width="550px" Theme="ThemeLTH"></dx:ASPxTextBox>
                <dx:ASPxButton ID="btnActualizar2" OnClick="btnActualizar2_Click" runat="server" Text="Guardar" Theme="ThemeLTH"></dx:ASPxButton>
            </div>
            <div class="col-md-12 text-center">
                <dx:ASPxButton ID="btnCerrar" OnClick="btnCerrar_Click" runat="server" Text="Cerrar" Theme="ThemeLTH"></dx:ASPxButton>
            </div>
        </div>
    </div>
    <script>
        $('document').ready(function () {
            
        });

        google.charts.load('current', { packages: ['corechart', 'bar'] });
        google.charts.setOnLoadCallback(drawBasic);

        function drawBasic() {

            var data = google.visualization.arrayToDataTable([
              ['Pilar', '% Avance', ],
              ['INFRAESTRUCTURA', <%=actualKardex.PilarInfraestructura%>],
              ['ADMINISTRACION', <%=actualKardex.PilarAdministracion%>],
              ['PLANEACION', <%=actualKardex.PilarPlaneacion%>],
              ['EJECUCION', <%=actualKardex.PilarEjecucion%>],
              ['PRODUCTOS Y SERVICIOS', <%=actualKardex.PilarPS%>]
            ]);

            var options = {
                title: 'AVANCE POR PILARES',
                chartArea: { width: '50%' },
                hAxis: {
                    title: '% Avance',
                    minValue: 0
                },
                vAxis: {
                    title: 'Pilar'
                }
            };

            var chart = new google.visualization.BarChart(document.getElementById('chart_div'));

            chart.draw(data, options);
        }
    </script>
</asp:Content>
