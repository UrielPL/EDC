<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="NuevoNivel.aspx.cs" Inherits="CatalogosLTH.Web.NuevoNivel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.6/css/jquery.dataTables.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div style="margin-top: 16rem" id="element">
            <div class="section">
                <div class="col-sm-2" id="dist1">
                    Distribuidor:
                </div>
                <div class="row" id="dist2">
                    <div class="col-sm-4">
                        <asp:DropDownList ID="drpDist" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                    </div>
                </div>

                <div class="col-sm-6" id="divnd">
                    <h3 id="nd"></h3>
                </div>
                <div class="row">
                    <div class="col-sm-6">
                        <h3>La calificación de esta auditoria es de: <%=_uAuditoria != null ? _uAuditoria.calificacionTotal.ToString() : "-"%></h3>
                    </div>
                </div>
                <br />
                <div style="display: flex;">
                    <h4>Total de puntos a evaluar segun su nivel en cada pilar</h4>
                    <h4 onclick="topdf()" id="descargaPDF" style="margin-left: 55%; cursor: pointer;">Descargar PDF</h4>
                </div>
                <table id="tbl_niveles" class="display">
                    <thead>
                        <tr>
                            <th style="text-align: center;">Nivel</th>
                            <th style="text-align: center;">Administración</th>
                            <th style="text-align: center;">Ejecución</th>
                            <th style="text-align: center;">Infraestructura</th>
                            <th style="text-align: center;">Planeación</th>
                            <th style="text-align: center;">Productos y servicios</th>
                            <th style="text-align: center;">Total</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <%--<div class="row">
                <div class="col-sm-10">
                    <h3>Revisa tu tabla financiera</h3>
                </div>
                <div class="col-sm-1">
                    <input type="button" class="btn btn-success" value="Tabla financiera" onclick="iratabla()" />
                </div>
            </div>--%>

            <br />

            <%--<h4>Calificación por Pilar</h4>
            <canvas id="myChart"></canvas>--%>

            <br />
            <h4>Puntos sin observaciones y con observaciones en la auditoria</h4>
            <div class="section">
                <table class="table">
                    <thead>
                        <tr>
                            <th style="text-align: center;">Resultado</th>
                            <th style="text-align: center;">Puntos</th>
                            <th style="text-align: center;">%</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Sin Observación</td>
                            <td><%=ptsCumplidos != null && ptsCumplidos.Count() > 0 ? ptsCumplidos.Count() : 0%></td>
                            <td><%=soPerc != 0 ? Math.Round(soPerc,2) : 0.0%>%</td>
                        </tr>
                        <tr>
                            <td>Observacion</td>
                            <td><%=ptsNoCumplidos != null && ptsNoCumplidos.Count() > 0 ? ptsNoCumplidos.Count() : 0%></td>
                            <td><%=obsPerc != 0 ? Math.Round(obsPerc,2) : 0.0%>%</td>
                        </tr>
                        <tr>
                            <td>Total</td>
                            <td><%=ptsNoCumplidos != null && ptsCumplidos != null ? ptsCumplidos.Count() + ptsNoCumplidos.Count() : 0.0%></td>
                            <td>100%</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <br />
            <div class="html2pdf__page-break"></div>

            <h4>Puntos por Pilar</h4>
            <canvas id="ptsPilar"></canvas>
            <br />
            <div class="html2pdf__page-break"></div>

            <h4>Puntos por Nivel</h4>
            <canvas id="ptsxnivel"></canvas>


            <br />
            <div class="html2pdf__page-break"></div>
            <div class="section">
                <h3>Puntos incorrectos</h3>
                <table id="tbl_resumen" class="display">
                    <thead>
                        <tr>
                            <th style="text-align: center;">Pilar</th>
                            <th style="text-align: center;">Punto</th>
                            <th style="text-align: center;">Comentarios</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <h3>Puntos correctos con comentarios</h3>
                <table id="tbl_correctos" class="display">
                    <thead>
                        <tr>
                            <th style="text-align: center;">Pilar</th>
                            <th style="text-align: center;">Punto</th>
                            <th style="text-align: center;">Comentarios</th>
                        </tr>
                    </thead>
                </table>
                <input id="tablaF" type="button" onclick="iratabla()" value="Ver Tabla Financiera">
            </div>
        </div>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/chartjs-plugin-datalabels/2.2.0/chartjs-plugin-datalabels.min.js" integrity="sha512-JPcRR8yFa8mmCsfrw4TNte1ZvF1e3+1SdGMslZvmrzDYxS69J7J49vkFL8u6u8PlPJK+H3voElBtUCzaXj+6ig==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script type="text/javascript" charset="utf8" src="//cdn.datatables.net/1.12.1/js/jquery.dataTables.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.10.1/html2pdf.bundle.min.js" integrity="sha512-GsLlZN/3F2ErC5ifS5QtgpiJtWd43JWSuIgh7mbzZ8zBps+dvLusV+eNQATqgA/HdeKFVgA5v3S/cIrLF7QnIg==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>

    <script>
        $('#divnd').hide();
        const labels = ['Ejecución', 'Administración', 'Infraestructura', 'Planeación', 'Prod. y Serv.']
        <%--const ctx = document.getElementById('myChart');
        
        const data = {
            labels: labels,
            datasets: [{
                label: 'Calificacion por Nivel',
                data: [<%=cEjecucion%>, <%=cAdministracion%>, <%=cInfraestructura%>,  <%=cPlaneacion%>,  <%=cProductos%>],
                backgroundColor: [
                  'rgba(255, 99, 132, 0.2)',
                  'rgba(255, 159, 64, 0.2)',
                  'rgba(255, 205, 86, 0.2)',
                  'rgba(75, 192, 192, 0.2)',
                  'rgba(54, 162, 235, 0.2)'
                ],
                borderColor: [
                  'rgb(255, 99, 132)',
                  'rgb(255, 159, 64)',
                  'rgb(255, 205, 86)',
                  'rgb(75, 192, 192)',
                  'rgb(54, 162, 235)'
                ],
                borderWidth: 1
            }]
        };

        new Chart(ctx, {
            type: 'bar',
            data: data,
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            },
            plugins: [ChartDataLabels]
        });--%>

        const tbl = document.getElementById('ptsPilar');
        const datos = {
            labels: labels,
            datasets: [{
                label: 'Sin Observacion',
                data: [<%=ptsEjec%>, <%=ptsAdmin%>, <%=ptsInfra%>,  <%=ptsPlan%>,  <%=ptsPYS%>],
                borderWidth: 1
            }, {
                type: 'bar',
                label: 'Con observacion',
                data: [<%=ejecNoCumplido%>, <%=adminNoCumplido%>, <%=infraNoCumplido%>, <%=planNoCumplido%>, <%=pysNoCumplido%>],
            }]
        };

        const optionsPilar = {
            scales:{
                y: {
                    beginAtZero: true
                }
            },
            plugins:{
                datalabels: {
                    labels: {
                        title: {
                            display: true,
                            color: 'gray', // Color de la etiqueta dentro de la barra
                            anchor: 'end',
                            align: 'top',
                            formatter:(value,context) =>{
                                //console.log(context.dataIndex);
                                //console.log(context.dataset.label);

                                var index = context.dataIndex;
                                var label = context.dataset.label;
                                if(index == 0 && label == "Sin Observacion"){
                                    var op =((value / 38) * 100);
                                    return parseFloat(op.toFixed(2)) + "%";
                                }
                                if(index == 1 && label == "Sin Observacion"){
                                    var op = ((value / 20) * 100);
                                    return parseFloat(op.toFixed(2))  + "%";
                                }
                                if(index == 2 && label == "Sin Observacion"){
                                    var op = ((value / 17) * 100)
                                    return parseFloat(op.toFixed(2)) + "%";
                                }
                                if(index == 3 && label == "Sin Observacion"){
                                    var op = ((value / 38) * 100)
                                    return parseFloat(op.toFixed(2)) + "%";
                                }
                                if(index == 4 && label == "Sin Observacion"){
                                    var op = ((value / 12) * 100)
                                    return parseFloat(op.toFixed(2)) + "%";
                                }

                                if(index == 0 && label == "Con observacion"){
                                    var op =((value / 38) * 100);
                                    return parseFloat(op.toFixed(2)) + "%";
                                }
                                if(index == 1 && label == "Con observacion"){
                                    var op = ((value / 20) * 100);
                                    return parseFloat(op.toFixed(2)) + "%";
                                }
                                if(index == 2 && label == "Con observacion"){
                                    var op = ((value / 17) * 100);
                                    return parseFloat(op.toFixed(2)) + "%";
                                }
                                if(index == 3 && label == "Con observacion"){
                                    var op = ((value / 38) * 100);
                                    return parseFloat(op.toFixed(2)) + "%";
                                }
                                if(index == 4 && label == "Con observacion"){
                                    var op = ((value / 12) * 100);
                                    return parseFloat(op.toFixed(2)) + "%";
                                }
                            },
                        },
                        value: {
                            display: true,
                            color: 'black', // Color de la etiqueta encima de la barra
                                    
                        },
                    },
                },
            },
        };
        

        new Chart(tbl, {
            type: 'bar',
            data: datos,
            options: optionsPilar,
            //options: {
            //    scales: {
            //        y: {
            //            beginAtZero: true
            //        }
            //    }
            //},
            plugins: [ChartDataLabels]
        });

        ///////////////////////////////////////////////////////////////////////////////////////
        var dist = $('#ctl00_ContentPlaceHolder1_drpDist').val();
        $.ajax({
            url: '<%=ResolveUrl("NuevoNivel.aspx/traePtsXNivel")%>',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            type: 'POST',
            async: false,
            data: JSON.stringify({dist : dist}),
            success: function (response) {
                var json = JSON.parse(response.d);
                console.log(json);
                const niveles = ['Básico', 'Intermedio', 'Avanzado', 'Sobresaliente']
                const grafo = document.getElementById('ptsxnivel');

                const data = {
                    labels: niveles,
                    datasets: [{
                        label: 'Cumplidos',
                        data: [json.basicoSi, json.interSi, json.avaSi, json.sobSi],
                        borderWidth: 1
                    }, {
                        type: 'bar',
                        label: 'No cumplidos',
                        data: [json.basicoNo, json.interNo, json.avaNo, json.sobNo],
                    }]
                };
                var counter = 1;
                const options ={
                    scales:{
                        y: {
                            beginAtZero: true
                        }
                    },
                    plugins:{
                        datalabels: {
                            labels: {
                                title: {
                                    display: true,
                                    color: 'gray', // Color de la etiqueta dentro de la barra
                                    anchor: 'end',
                                    align: 'top',
                                    formatter:(value,context) =>{
                                        //console.log(context.dataIndex);
                                        //console.log(context.dataset.label);

                                        var index = context.dataIndex;
                                        var label = context.dataset.label;
                                        if(index == 0 && label == "Cumplidos"){
                                            var op =((value / 44) * 100);
                                            return parseFloat(op.toFixed(2)) + "%";
                                        }
                                        if(index == 1 && label == "Cumplidos"){
                                            var op = ((value / 56) * 100);
                                            return parseFloat(op.toFixed(2))  + "%";
                                        }
                                        if(index == 2 && label == "Cumplidos"){
                                            var op = ((value / 23) * 100)
                                            return parseFloat(op.toFixed(2)) + "%";
                                        }
                                        if(index == 3 && label == "Cumplidos"){
                                            var op = ((value / 2) * 100)
                                            return parseFloat(op.toFixed(2)) + "%";
                                        }

                                        if(index == 0 && label == "No cumplidos"){
                                            var op =((value / 44) * 100);
                                            return parseFloat(op.toFixed(2)) + "%";
                                        }
                                        if(index == 1 && label == "No cumplidos"){
                                            var op = ((value / 56) * 100);
                                            return parseFloat(op.toFixed(2)) + "%";
                                        }
                                        if(index == 2 && label == "No cumplidos"){
                                            var op = ((value / 23) * 100);
                                            return parseFloat(op.toFixed(2)) + "%";
                                        }
                                        if(index == 3 && label == "No cumplidos"){
                                            var op = ((value / 2) * 100);
                                            return parseFloat(op.toFixed(2)) + "%";
                                        }
                                    },
                                },
                                value: {
                                    display: true,
                                    color: 'black', // Color de la etiqueta encima de la barra
                                    
                                },
                            },
                        },
                    },
                };
                new Chart(grafo, {
                    type: 'bar',
                    data: data,
                    options: options,
                    plugins: [ChartDataLabels]
                });
            }
        });

        

        $.ajax({
            url: '<%=ResolveUrl("NuevoNivel.aspx/traePtsXPilar")%>',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            type: 'POST',
            async: false,
            success: function (response) {
                var data = JSON.parse(response.d);
                //console.log(data);
                $('#tbl_niveles').DataTable({
                    data: data,
                    columns: [
                        { data: 'Nivel' },
                        { data: 'Administracion' },
                        { data: 'Ejecucion' },
                        { data: 'Infraestructura' },
                        { data: 'Planeacion' },
                        { data: 'ProductosYServicios' },
                        { data: 'Total' },
                    ],
                    paging: false,
                    searching: false,
                    ordering: false

                });
            }
        });

        function iratabla() {
            window.open("/tablafinanciera.aspx?a=<%=_uAuditoria != null ? _uAuditoria.Id.ToString() : ""%>");
}

            $.ajax({
                url: '<%=ResolveUrl("NuevoNivel.aspx/traePuntos")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                success: function (response) {
                var data = JSON.parse(response.d);
                //console.log(data);
                $('#tbl_resumen').DataTable({
                    data: data,
                    columns: [
                        { data: 'Pilar' },
                        { data: 'Punto' },
                        { data: 'Comentarios' },
                    ],
                    paging: false,
                    searching: false,
                    ordering: false,
                    "createdRow": function (row, data, dataIndex) {
                        $(row).css('text-align', 'start');
                    }
                });

            }
            });

        $.ajax({
             url: '<%=ResolveUrl("NuevoNivel.aspx/traePuntosCorrectos")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                success: function (response) {
                    var datos = JSON.parse(response.d);

                    $('#tbl_correctos').DataTable({
                        data: datos,
                        columns: [
                            { data: 'Pilar' },
                            { data: 'Punto' },
                            { data: 'Comentarios' },
                        ],
                        paging: false,
                        searching: false,
                        ordering: false,
                        "createdRow": function (row, data, dataIndex) {
                            $(row).css('text-align', 'start');
                        }
                    });
                }
        });

        function topdf() {
            $('#divnd').show();
            $('#dist1').hide();
            $('#dist2').hide();
            $('#tablaF').hide();
            $('#nd').text('<%:_uAuditoria != null ? _uAuditoria.Distribuidor.nombredist.ToString()  : "" %>       ');
            var name = "nivel-<%:_uAuditoria != null ? _uAuditoria.Id.ToString() : ""%>-<%:_uAuditoria != null ? _uAuditoria.Distribuidor.nombredist.ToString() : ""%>.pdf";
            console.log(name);
            $('#element').css('margin-top', '-2rem');
            var element = document.getElementById('element');
            //var element = $('#element');
            var opt = {
                margin: 0.4,
                filename: name,
                image: { type: 'jpeg', quality: 0.98 },
                html2canvas: { scale: 3 },
                jsPDF: { unit: 'in', format: 'tabloid', orientation: 'l' }
            };

            html2pdf(element, opt);

        }



    </script>

</asp:Content>
