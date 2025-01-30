<%@ Page Title="" Language="C#" MasterPageFile="~/EDCClarios.Master" AutoEventWireup="true" CodeBehind="HistoricoMinutas.aspx.cs" Inherits="CatalogosLTH.Web.HistoricoMinutas" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <head>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.3/Chart.bundle.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.3/Chart.min.js"></script>
        <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.9.3/Chart.min.css" />
        <script src="https://unpkg.com/ag-grid-community/dist/ag-grid-community.min.js"></script>
        <link rel="stylesheet" href="https://unpkg.com/ag-grid-community/dist/styles/ag-grid.css">
        <link rel="stylesheet" href="https://unpkg.com/ag-grid-community/dist/styles/ag-theme-balham.css">
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
         .tarjeta{
            -webkit-box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
            -moz-box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
            box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
        }
    </style>

    <!-- MODALS -->
    <div id="modalCorreo" class="modal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Enviar correo</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div id="correoEnviado" class="alert alert-success">
                        <strong>Hecho!</strong> El correo se ha enviado correctamente.
                    </div>
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text" id="basic-addon1">@</span>
                        </div>
                        <input type="text" id="txtCorreo" class="form-control" placeholder="Correo..." aria-label="Username" aria-describedby="basic-addon1" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnCerrarModal" type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                    <button id="btnEnviarCorreo" type="button" class="btn btn-primary">Enviar correo</button>
                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 text-center" style="background-color: #6141b0; color: white; padding: 10px; font-size: 18px; font-weight: 600;">
                <span style="text-transform: uppercase;">HISTORICO DE MINUTAS</span>
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
                                <span>DISTRIBUIDOR</span>
                            </div>
                        </div>
                        <div class="col-md-6 form-group">
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
                    <div class="row ">
                        <div class="col-md-12 text-center">
                            <h5 id="distActual" style="font-weight: 700; color: #6141b0"></h5>
                        </div>
                    
                        <canvas class="tarjeta" id="myChart" width="400" height="250"></canvas>
                    </div>
                </div>
                <div class="col-md-5 offset-md-1">
                     <div id="myGrid" style="height: 600px; width:100%;" class="ag-theme-balham"></div>
                </div>
            </div>
        </div>
    </div>

    <script></script>
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

        // Variables del archivo seleccionado para enviar por mail
        var name;
        var URL;
        var usuario;
        var dist;
        var fecha;

        $(document).ready(function () {
            actualizaTabla();
            $("#correoEnviado").hide();

            $("#selDist").change(function () {
                dist = $("#selDist").val();
                fy = $("#selAnio").val();
                mes = $("#selMes").val();

                actualizaTabla();
            });

            $("#selAnio").change(function () {
                dist = $("#selDist").val();
                fy = $("#selAnio").val();
                mes = $("#selMes").val();

                actualizaTabla();
            });

            $("#selMes").change(function () {
                dist = $("#selDist").val();
                fy = $("#selAnio").val();
                mes = $("#selMes").val();

                actualizaTabla();
            });

            $("#btnEnviarCorreo").on("click", function () {
                enviarCorreo($("#txtCorreo").val());
            });

            $("#btnCerrarModal").on("click", function () {
                $("#correoEnviado").hide();
            });
        });

        function downloadURI(uri, name) {
            var link = document.createElement("a");
            link.download = name;
            link.href = uri;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            delete link;
        }

        function abrirModalCorreo(pUrl, pName, pFecha, pDist) {
            $('#modalCorreo').modal('show');

            URL = pUrl;
            name = pName;
            fecha = pFecha;
            dist = pDist;
        }

        function enviarCorreo(correo) {
            $.ajax({
                type: "POST",
                url: "HistoricoMinutas.aspx/enviarCorreo",
                contentType: "application/json; charset=utf-8",
                data: '{ "name" : "' + name + '", "URL" : "' + URL + '", "correo" : "' + correo + '", "fecha" : "' + fecha + '", "distr" : "' + dist + '" }',
                dataType: "json",
                success: function (data) {
                    $("#correoEnviado").show();
                }
            });
        }

        function actualizaTabla() {
            dist = $("#selDist").val();
            fy = $("#selAnio").val();
            mes = $("#selMes").val();

            $("#distActual").text(dist);

            $.ajax({
                type: "POST",
                url: "HistoricoMinutas.aspx/ActualizaTabla",
                contentType: "application/json; charset=utf-8",
                data: '{ "Dist" : "'+dist+'", "FY" : "'+fy+'", "mes" : "'+mes+'" }',
                dataType: "json",
                success: function (data) {
                    $('#registroMinutas').empty();
                        
                    var dataRow = JSON.parse(data.d);

                    var cellRenderer = function (params) {
                        var eDiv = document.createElement('div');
                        eDiv.innerHTML = '<span class="my-css-class"><i class="btnDescargar">Descargar</i></span>';
                        var eButton = eDiv.querySelectorAll('.btnDescargar')[0];

                        eButton.addEventListener('click', function (datas) {
                            var focusedCell = gridOptions.api.getFocusedCell();
                            var row = gridOptions.api.getDisplayedRowAtIndex(focusedCell.rowIndex).data;

                            downloadURI('Archivos/Minutas/' + row.archivo, row.archivo);
                        });
                        return eDiv;
                    }

                    var cellRenderer2 = function (params) {
                        var eDiv = document.createElement('div');
                        eDiv.innerHTML = '<span class="my-css-class"><i class="btnEnviar">Enviar</i></span>';
                        var eButton = eDiv.querySelectorAll('.btnEnviar')[0];

                        eButton.addEventListener('click', function (datas) {
                            var focusedCell = gridOptions.api.getFocusedCell();
                            var row = gridOptions.api.getDisplayedRowAtIndex(focusedCell.rowIndex).data;

                            abrirModalCorreo('Archivos/Minutas/' + row.archivo, row.archivo, row.fecha, row.dist);
                        });
                        return eDiv;
                    }

                    // specify the columns
                    var columnDefs = [
                        { headerName: "Archivo", cellRenderer: cellRenderer, width: 75 },
                        { headerName: "Correo", cellRenderer: cellRenderer2, width: 75 },
                        { headerName: "Fecha", field: "fecha", sortable: true, sort: 'desc', filter: true, width: 100 },
                        { headerName: "Nombre", field: "archivo", sortable: true, filter: true, width: 125 },
                        { headerName: "Distribuidor", field: "dist", sortable: true, filter: true, width: 150 }
                    ];

                    JsonParaExportar = dataRow;
                    // let the grid know which columns and what data to use
                    var gridOptions = {
                        columnDefs: columnDefs,
                        rowData: dataRow,
                        rowSelection: 'multiple',
                        pagination: true,
                        paginationPageSize: 50
                    };

                    // lookup the container we want the Grid to use
                    var eGridDiv = document.querySelector('#myGrid');
                    eGridDiv.innerHTML = '';
                    // create the grid passing in the div to use together with the columns & data we want to use
                    new agGrid.Grid(eGridDiv, gridOptions);
                    //document.getElementById("btnExcelExport").style.display = "block";
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
                url: "HistoricoMinutas.aspx/ActualizaGrafica",
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
            dist = $("#selDist").val();
            fy = $("#selAnio").val();
            mes = $("#selMes").val();
            $.ajax({
                type: "POST",
                url: "HistoricoMinutas.aspx/ActualizaGraficaObjetivos",
                contentType: "application/json; charset=utf-8",
                data: '{ "Dist" : "' + dist + '", "FY" : "' + fy + '", "mes" : "' + mes +'" }',
                dataType: "json",
                success: function (data) {
                    var json = JSON.parse(data.d);
                    $.each(json, function (index, value) {
                        objetivos[index] = value;
                    });
                    console.log(objetivos);
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
