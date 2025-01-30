<%@ Page Title="" Language="C#" MasterPageFile="~/EDCClarios.Master" AutoEventWireup="true" CodeBehind="VerificacionMinutas.aspx.cs" Inherits="CatalogosLTH.Web.VerificacionMinutas" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <head>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.3/Chart.bundle.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.3/Chart.min.js"></script>
        <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.3/Chart.min.css" />
    </head>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .labelCampo {
            padding: 5px;
            background-color: #6141b0;
            color: white;
        }

        .labelCampoInv {
            padding: 5px; 
            background-color: #00b0f0; 
            color: white;
        }

        a.class, a.class:hover {
            text-decoration: none;
        }

        .btnBack {
            padding: 10px;
            background-color: #6141b0;
            color: white;
            font-size: 18px;
        }
    </style>
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 text-center" style="background-color: #6141b0; color: white; padding: 10px; font-size: 18px; font-weight: 600;">
                <span style="text-transform: uppercase;">VERIFICACIÓN DE MINUTAS</span>
            </div>
        </div>
        <br /><br />
        <div class="container">
            <div class="row">
                <div class="col-md-1 offset-md-11">
                    <a class="class" href="MinutasIndex.aspx">
                        <span class="btnBack">Regresar</span>
                    </a>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-6">
                    <div class="row">
                        <div class="col-md-6">
                             <div class="labelCampo text-center">
                                <span>AÑO FISCAL</span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <select class="form-control" id="selAnio">
                                <option value="FY18">FY18</option>
                                <option value="FY19">FY19</option>
                                <option value="FY20">FY20</option>
                                <option value="FY21">FY21</option>
                                <option value="Todos">TODOS</option>
                            </select>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-6">
                             <div class="labelCampo text-center">
                                <span>GERENTE DE CUENTA</span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <select class="form-control" id="selGerente">
                                <% foreach (var item in lstGerentes) { %>
                                    <option value="<%=item.UserName %>"><%=item.Nombre %></option>
                                <% } %>
                                <option value="Todos">TODOS</option>
                            </select>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-6">
                             <div class="labelCampo text-center">
                                <span>MES</span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <select class="form-control" id="selMes">
                                <% foreach (var item in Meses) { %>
                                    <option value="<%=item %>"><%=item %></option>
                                <% } %>
                            </select>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-6">
                             <div class="labelCampo text-center">
                                <span>DISTRIBUIDOR</span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <select class="form-control" id="selDist">
                                <% foreach (var item in lstDist) { %>
                                    <option value="<%=item.UserName %>"><%=item.UserName %></option>
                                <% } %>
                                <% foreach (var item in lstDistMin) { %>
                                    <option value="<%=item.nombredist %>"><%=item.nombredist %></option>
                                <% } %>
                                <option value="Todos">TODOS</option>
                            </select>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-12 text-center">
                            <h5 id="distActual" style="font-weight: 700; color: #6141b0"></h5>
                        </div>
                    </div>
                    <canvas id="myChart" width="400" height="250"></canvas>
                </div>
                <div class="col-md-5 offset-md-1">
                    <table class="table table-striped">
                        <thead>
                            <tr class="text-center">
                                <th style="background-color: #6141b0; color: white;" scope="col" colspan="3">RECORD POR DISTRIBUIDOR</th>
                            </tr>
                        </thead>
                        <tbody id="registroMinutas">
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <script>
        // Color morado oscuro #6141b0 RGB(97, 65, 176);
        // Color morado/rosa #cc0099 RGB(204, 0, 153);
        var ctx = $('#myChart');
        var myChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ['Oct', 'Nov', 'Dec', 'Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Total'],
                datasets: [{
                    label: 'Realizadas',
                    data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                    backgroundColor: [
                        'rgba(97, 65, 176, 0.9)',
                        'rgba(97, 65, 176, 0.9)',
                        'rgba(97, 65, 176, 0.9)',
                        'rgba(97, 65, 176, 0.9)',
                        'rgba(97, 65, 176, 0.9)',
                        'rgba(97, 65, 176, 0.9)',
                        'rgba(97, 65, 176, 0.9)',
                        'rgba(97, 65, 176, 0.9)',
                        'rgba(97, 65, 176, 0.9)',
                        'rgba(97, 65, 176, 0.9)',
                        'rgba(97, 65, 176, 0.9)',
                        'rgba(97, 65, 176, 0.9)',
                        'rgba(97, 65, 176, 0.9)'
                    ]
                }, {
                    label: 'Objetivo',
                    data: [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                    backgroundColor: [
                        'rgba(204, 0, 153, 0.9)',
                        'rgba(204, 0, 153, 0.9)',
                        'rgba(204, 0, 153, 0.9)',
                        'rgba(204, 0, 153, 0.9)',
                        'rgba(204, 0, 153, 0.9)',
                        'rgba(204, 0, 153, 0.9)',
                        'rgba(204, 0, 153, 0.9)',
                        'rgba(204, 0, 153, 0.9)',
                        'rgba(204, 0, 153, 0.9)',
                        'rgba(204, 0, 153, 0.9)',
                        'rgba(204, 0, 153, 0.9)',
                        'rgba(204, 0, 153, 0.9)',
                        'rgba(204, 0, 153, 0.9)'
                    ]
                }]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }
        });

        // Variables para los valores actuales de los select
        var dist;
        var fy;
        var mes;

        $(document).ready(function () {

            actualizaTabla();

            $("#selDist").change(function () {
                dist = $("#selDist").val();
                fy = $("#selAnio").val();
                mes = $("#selMes").val();
                gte = $("#selGerente").val();

                actualizaTabla();
            });

            $("#selAnio").change(function () {
                dist = $("#selDist").val();
                fy = $("#selAnio").val();
                mes = $("#selMes").val();
                gte = $("#selGerente").val();

                actualizaTabla();
            });

            $("#selMes").change(function () {
                dist = $("#selDist").val();
                fy = $("#selAnio").val();
                mes = $("#selMes").val();
                gte = $("#selGerente").val();

                actualizaTabla();
            });

            $("#selGerente").change(function () {
                dist = $("#selDist").val();
                fy = $("#selAnio").val();
                mes = $("#selMes").val();
                gte = $("#selGerente").val();

                actualizaTabla();
            });
        });

        function actualizaTabla() {
            dist = $("#selDist").val();
            fy = $("#selAnio").val();
            mes = $("#selMes").val();
            gte = $("#selGerente").val();

            $("#distActual").text(dist);

            $.ajax({
                type: "POST",
                url: "VerificacionMinutas.aspx/ActualizaTabla",
                contentType: "application/json; charset=utf-8",
                data: '{ "Dist" : "' + dist + '", "FY" : "' + fy + '", "mes" : "' + mes + '", "Gte" : "'+gte+'" }',
                dataType: "json",
                success: function (data) {
                    $('#registroMinutas').empty();
                    var json = JSON.parse(data.d);
                    $.each(json, function (index, value) {
                        var link = '<a download href="Archivos/Minutas/' + value.archivo + '" style="font-size:12px!important" target="_blank">' + value.archivo + '</a>';
                        $("#registroMinutas").append("<tr><td>" + value.fecha + "</td><td>" + link + "</td><td>" + value.dist + "</td></tr>");
                    });
                    actualizaGrafica();
                },
                failure: function (response) {
                    console.log(response.d);
                }
            });
        }

        function actualizaGrafica() {
            var realizadas = [];
            $.ajax({
                type: "POST",
                url: "VerificacionMinutas.aspx/ActualizaGrafica",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var json = JSON.parse(data.d);
                    $.each(json, function (index, value) {
                        realizadas[index] = value;
                    });
                    myChart.data.datasets[0].data[3] = realizadas[0];
                    myChart.data.datasets[0].data[4] = realizadas[1];
                    myChart.data.datasets[0].data[5] = realizadas[2];
                    myChart.data.datasets[0].data[6] = realizadas[3];
                    myChart.data.datasets[0].data[7] = realizadas[4];
                    myChart.data.datasets[0].data[8] = realizadas[5];
                    myChart.data.datasets[0].data[9] = realizadas[6];
                    myChart.data.datasets[0].data[10] = realizadas[7];
                    myChart.data.datasets[0].data[11] = realizadas[8];
                    myChart.data.datasets[0].data[0] = realizadas[9];
                    myChart.data.datasets[0].data[1] = realizadas[10];
                    myChart.data.datasets[0].data[2] = realizadas[11];
                    myChart.data.datasets[0].data[12] = realizadas[12];
                    myChart.update();

                    actualizaGraficaObjetivos();
                },
                failure: function (response) {
                    console.log(response.d);
                }
            });
        }

        function actualizaGraficaObjetivos() {
            var objetivos = [];
            dist = $("#selDist").val();
            fy = $("#selAnio").val();
            mes = $("#selMes").val();
            $.ajax({
                type: "POST",
                url: "VerificacionMinutas.aspx/ActualizaGraficaObjetivos",
                contentType: "application/json; charset=utf-8",
                data: '{ "Dist" : "' + dist + '", "FY" : "' + fy + '", "mes" : "' + mes + '"}',
                dataType: "json",
                success: function (data) {
                    var json = JSON.parse(data.d);
                    $.each(json, function (index, value) {
                        objetivos[index] = value;
                    });
                    myChart.data.datasets[1].data = objetivos;
                    /*myChart.data.datasets[1].data[4] = objetivos[1];
                    myChart.data.datasets[1].data[5] = objetivos[2];
                    myChart.data.datasets[1].data[6] = objetivos[3];
                    myChart.data.datasets[1].data[7] = objetivos[4];
                    myChart.data.datasets[1].data[8] = objetivos[5];
                    myChart.data.datasets[1].data[9] = objetivos[6];
                    myChart.data.datasets[1].data[10] = objetivos[7];
                    myChart.data.datasets[1].data[11] = objetivos[8];
                    myChart.data.datasets[1].data[0] = objetivos[9];
                    myChart.data.datasets[1].data[1] = objetivos[10];
                    myChart.data.datasets[1].data[2] = objetivos[11];
                    myChart.data.datasets[1].data[12] = objetivos[12];*/ //TOTAL
                    myChart.update();
                },
                failure: function (response) {
                    console.log(response.d);
                }
            });
        }
    </script>
</asp:Content>
