<%@ Page Title="" Language="C#" MasterPageFile="~/EDCClarios.Master" AutoEventWireup="true" CodeBehind="MinutasIndex.aspx.cs" Inherits="CatalogosLTH.Web.MinutasIndex" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <head>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.3/Chart.bundle.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.3/Chart.min.js"></script>
        <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.3/Chart.min.css" />
    </head>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
        .seccionPagina {
            padding: 5px; 
            background-color: #6141b0; 
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
        .tarjeta{
            -webkit-box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
            -moz-box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
            box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
        }
    </style>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 text-center" style="background-color: #6141b0; color: white; padding: 10px; font-size: 18px; font-weight: 600;">
                <span style="text-transform: uppercase;">Minutas de visita a distribuidores</span>
            </div>
        </div>
        <div class="container">
            <br />
            <div class="row">
                <div class="col-md-1 offset-md-11">
                    <a class="class" href="mainpage.aspx">
                        <span class="btnBack tarjeta">Regresar</span>
                    </a>
                </div>
            </div>
            <div class="row">
                <div class="col-md-5 ">
                    <div class="col-md-12 text-center ">
                        <img style="padding: 20px 0px;" width="75px;" src="https://cdn0.iconfinder.com/data/icons/bicon/24/editor_clipboard_list_task_complete_finish-512.png" />
                    </div>
                    <% if(user.TipoUsuario == CatalogosLTH.Module.BusinessObjects.TipoUsuario.GerenteCuenta ||
                            user.TipoUsuario == CatalogosLTH.Module.BusinessObjects.TipoUsuario.GerenteDesarrolloComercial ||
                            user.TipoUsuario == CatalogosLTH.Module.BusinessObjects.TipoUsuario.GerenteVenta) { %>
                    <a href="CargaMinutas.aspx" class="class "><div class="col-md-12 text-center tarjeta" style="padding: 5px; background-color: #6141b0; color: white;">CARGA DE MINUTAS</div></a>
                    <br />
                    <% } %>
                    <a href="HistoricoMinutas.aspx" class="class"><div class="col-md-12 text-center seccionPagina tarjeta">HISTORICO DE MINUTAS</div></a>
                    <br />
                    <% if (user.TipoUsuario == CatalogosLTH.Module.BusinessObjects.TipoUsuario.GerenteVenta || user.TipoUsuario == CatalogosLTH.Module.BusinessObjects.TipoUsuario.GerenteDesarrolloComercial || user.TipoUsuario == CatalogosLTH.Module.BusinessObjects.TipoUsuario.Admin) { %>
                    <a href="VerificacionMinutas.aspx" class="class"><div class="col-md-12 text-center seccionPagina tarjeta">VERIFICACIÓN DE MINUTAS</div></a>
                    <br />
                    <% } %>
                    <a href="ObjetivosMinutas.aspx" class="class"><div class="col-md-12 text-center seccionPagina tarjeta">CARGA DE OBJETIVOS</div></a>
                    <br />
                    <a href="#" class="class"><div class="col-md-12 text-center seccionPagina tarjeta">REPORTES</div></a>
                </div>
                <div class="col-md-7">
                    <br />
                    <div class="col m12 tarjeta">
                        <canvas id="myChart" width="400" height="250"></canvas>
                    </div>
                    <div class="col-md-3 offset-md-9">
                        <img width="100px;" src="https://media-exp1.licdn.com/dms/image/C560BAQEo7dFAEtAq1A/company-logo_200_200/0?e=2159024400&v=beta&t=_CpLqKk7fjdLrSOEXbKfAXALPQDDzPDIdBKiqSOMJ0M"/>
                    </div>
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

        $(document).ready(function () {
            actualizaGrafica();
        });

        function actualizaGrafica() {
            var realizadas = [];
            $.ajax({
                type: "POST",
                url: "MinutasIndex.aspx/ActualizaGrafica",
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
                    myChart.data.datasets[0].data[12] = realizadas[12]; //TOTAL
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
            var FY = "FY" + new Date().getFullYear().toString().substring(2);
            $.ajax({
                type: "POST",
                url: "MinutasIndex.aspx/ActualizaGraficaObjetivos",
                contentType: "application/json; charset=utf-8",
                data: '{ "Dist" : "Todos", "FY" : "' + FY + '", "mes" : "Todos" }',
                dataType: "json",
                success: function (data) {
                    var json = JSON.parse(data.d);
                    $.each(json, function (index, value) {
                        objetivos[index] = value;
                    });
                    myChart.data.datasets[1].data = objetivos;
                    myChart.update();
                },
                failure: function (response) {
                    console.log(response.d);
                }
            });
        }
    </script>
</asp:Content>
