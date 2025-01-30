<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListActividades.aspx.cs" MasterPageFile="~/edcMaster.Master" Inherits="CatalogosLTH.Web.ListActividades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://cdn.datatables.net/2.0.6/js/dataTables.js"></script>
    <script src="https://cdn.datatables.net/buttons/3.0.2/js/dataTables.buttons.js"></script>
    <script src="https://cdn.datatables.net/buttons/3.0.2/js/buttons.dataTables.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/3.0.2/js/buttons.html5.min.js"></script>

    <link rel="stylesheet" href="https://cdn.datatables.net/2.0.6/css/dataTables.dataTables.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/3.0.2/css/buttons.dataTables.css" />

    

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="section  banner-plan remove-top remove-bottom">
        <div class="container">
            <div class="row">
                <div class="twelve columns">
                    <h1>Plan de Negocio</h1>
                </div>
            </div>
        </div>
    </div>

    <div class="section  remove-bottom">
        <div class="container">
            <div class="row">
                <div class="six columns">
                    <p>Selecciona el Plan de Negocio:</p>
                </div>
            </div>
        </div>
    </div>

    <div class="container">
        <div class="section  remove-bottom remove-top">
            <div class="container">
                <div class="row">
                    <div class="four columns auditoria">
                        <label>
                            <p>Elige el nivel</p>
                        </label>
                        <select class="form-select" aria-label="Default select example" id="niveles" onchange="distChange()">
                        </select>
                    </div>
                    <div class="four columns auditoria">
                        <label>
                            <p>Distribuidor</p>
                        </label>
                        <select class="form-select" aria-label="Default select example" id="distribuidores" onchange="distChange()">
                        </select>
                    </div>
                    <div class="four columns auditoria search">
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="container spinner-border text-primary" role="status" id="spinner">
        <span class="visually-hidden">Cargando...</span>
    </div>
    <div class="container" id="listado">
        <div class="section">
            <table id="tbl_actividades" class="display">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Actividad</th>
                        <th>Duracion</th>
                        <th>Fecha Inicio</th>
                        <th>Fecha Fin</th>
                        <th>Estatus</th>
                        <th>Nivel</th>
                        <th>Evaluador</th>
                        <th>Pilar</th>
                        <th>Detalle</th>
                        <th>Distribuidor</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>

    </div>


    <script>
        var data = "";
        $(document).ready(function () {
            $('#spinner').hide();

            //$('#tbl_actividades').DataTable({
            //    select: true,
            //    data: data,
            //    columns: [
            //        { data: 'ID' },
            //        { data: 'Actividad' },
            //        { data: 'Duracion' },
            //        { data: 'FechaIn' },
            //        { data: 'FechaFin' },
            //        { data: 'Estatus' },
            //        { data: 'Nivel' },
            //        { data: 'Evaluador' },
            //        { data: 'Pilar' },
            //        { data: 'Vigencia' }
            //    ],
            //    rowReorder: true,
            //    processing: true,
            //    layout: {
            //        topStart: {
            //            buttons: [
            //                {
            //                    extend: 'excel',
            //                    text: 'Exportar Excel',
            //                    exportOptions: {
            //                        modifier: {
            //                            page: 'current'
            //                        }
            //                    }
            //                }
            //            ]
            //        }
            //    },
            //    order: [[0, "desc"]],
            //    "createdRow": function (row, data, dataIndex) {
            //        $(row).attr('onclick', 'redirect(' + data.ID + ')');
            //    }

            //});

            //LLENA NIVELES
            $.ajax({
                url: '<%=ResolveUrl("ListActividades.aspx/traeNiveles")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                success: function (response) {
                    var select = $('#niveles');
                    select.empty();
                    select.append($("<option></option>").attr("value", "Todos").text("Todos"));
                    var levels = JSON.parse(response.d);

                    $.each(levels, function (i) {
                        select.append($("<option></option>").attr("value", levels[i].idnivel).text(levels[i].nombreniv));
                    });


                }
            });

            //LLENA DISTRIBUIDORES
            $.ajax({
                url: '<%=ResolveUrl("ListActividades.aspx/traeDistribuidores")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                success: function (response) {
                    var select = $('#distribuidores');
                    select.empty();
                    var dist = JSON.parse(response.d);

                    $.each(dist, function (i) {
                        select.append($("<option></option>").attr("value", dist[i].id).text(dist[i].nombre));
                    });

                    if ('<%=permiso%>' == "Distribuidor") {
                        //$('#tbl_actividades').DataTable().destroy();
                        traeActividades('<%=user.Oid.ToString()%>', $('#niveles option:selected').text());
                    } else {
                        traeActividades(dist[0].id, $('#niveles option:selected').text());
                    }
                }
            });

            traeActividades($('#distribuidores').val(), $('#niveles option:selected').text());

        });

        function distChange() {
            var distribuidor = $('#distribuidores').val()
            var nivel = $('#niveles option:selected').text();
            //console.log(distribuidor);
            //console.log(nivel);

            $('#spinner').show();
            $('#listado').hide();
            traeActividades(distribuidor, nivel)
        }

        function redirect(id) {
            //console.log(id);
            window.location.href = "SelectActividad.aspx?id=" + id;
            //Funcion para redirigir al detalle de la actividad
        }

        function traeActividades(idDist, idNivel) {
            $.ajax({
                url: '<%=ResolveUrl("ListActividades.aspx/traeActividades")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                data: JSON.stringify({ idDist: idDist, idNivel: idNivel }),
                beforeSend: function () {
                    $('#spinner').show();
                    $('#listado').hide();
                },
                success: function (response) {
                    $('#spinner').hide();
                    $('#listado').show();
                    data = JSON.parse(response.d);
                    //console.log(data);

                    $('#tbl_actividades').DataTable({
                        //dom: 'Bfrtip',
                        destroy: true,
                        //select: true,
                        data: data,
                        columns: [
                            { data: 'idact' },
                            { data: 'Actividad' },
                            { data: 'Duracion' },
                            { data: 'FechaIn' },
                            { data: 'FechaFin' },
                            { data: 'Estatus' },
                            { data: 'Nivel' },
                            { data: 'Evaluador' },
                            { data: 'Pilar' },
                            { data: 'Vigencia' },
                            { data: 'Distribuidor' }
                        ],
                        processing: true,
                        pageLength: 7,
                        lengthMenu: [],
                        layout: {
                            topStart: {
                                buttons: [
                                    {
                                        extend: 'excel',
                                        text: 'Exportar Excel',
                                        exportOptions: {
                                            //modifier: {
                                            //    page: 'current'
                                            //}
                                        },
                                        filename: 'Actividades-' + $('#distribuidores option:selected').text()
                                    }
                                ]
                            }
                        },
                        //order: [[0, "desc"]],
                        "createdRow": function (row, data, dataIndex) {
                            $(row).attr('onclick', 'redirect(' + data.ID + ')');
                        }

                    });
                }
            });
        }



    </script>
</asp:Content>
