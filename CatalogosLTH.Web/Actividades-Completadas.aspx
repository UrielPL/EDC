<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/edcMaster.Master" CodeBehind="Actividades-Completadas.aspx.cs" Inherits="CatalogosLTH.Web.Actividades_Completadas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link rel="stylesheet" type="text/css" href="//cdn.datatables.net/1.12.1/css/jquery.dataTables.min.css">
    <script type="text/javascript" charset="utf8" src="//cdn.datatables.net/1.12.1/js/jquery.dataTables.min.js"></script>--%>
    <script src="https://cdn.datatables.net/2.0.6/js/dataTables.js"></script>
    <script src="https://cdn.datatables.net/buttons/3.0.2/js/dataTables.buttons.js"></script>
    <script src="https://cdn.datatables.net/buttons/3.0.2/js/buttons.dataTables.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/3.0.2/js/buttons.html5.min.js"></script>

    <link rel="stylesheet" href="https://cdn.datatables.net/2.0.6/css/dataTables.dataTables.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/3.0.2/css/buttons.dataTables.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container" style="margin-top: 40px">
        <div class="panel-body" style="margin-top: 8rem">
            <h3>Actividades completadas</h3>
            <%if (rol != "Admin" && rol != "GerenteCuenta" && rol != "Evaluador")
                { %>
            <div class="row">
                <div class="col-sm-4">
                    <select id="drop_dist" onchange="changeDist()">
                    </select>
                </div>
            </div>
            <br />
            <%} %>
            <table class="table table-hover" id="tbl_actividades">
                <thead>
                    <tr>
                        <th style="text-align: center;">ID</th>
                        <th style="text-align: center;">Actividad</th>
                        <th style="text-align: center;">Duracion</th>
                        <th style="text-align: center;">Fecha Inicio</th>
                        <th style="text-align: center;">Fecha Completada</th>
                        <th style="text-align: center;">Estatus</th>
                        <th style="text-align: center;">Evaluador</th>
                        <th style="text-align: center;">Distribuidor</th>
                        <%--<th style="text-align:center;">En revision</th>--%>
                        <th style="text-align: center;">Autor Auditoria</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>

            </table>
        </div>
    </div>


    <script>
        $(document).ready(function () {
            changeDist();
            <%--$.ajax({
                url: '<%=ResolveUrl("Actividades-Completadas.aspx/getDistribuidores")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                success: function (response) {
                    data = JSON.parse(response.d);

                    //console.log(data);
                    if (data != -1 && data != 1) {
                        var select = $('#drop_dist');
                        select.empty();

                        $.each(data, function (i) {
                            if (i == 0) {
                                select.append($("<option selected></option>").attr("value", "").text(data[i]));
                            } else {
                                select.append($("<option></option>").attr("value", "").text(data[i]));
                            }
                            
                        });

                        changeDist();
                    } else {
                        actsAdmin();
                    }
                    
                }
            });--%>
        });


        function changeDist() {
            //var distSelected = $('#drop_dist option:selected').text();
            //console.log(distSelected);
            $.ajax({
                url: '<%=ResolveUrl("Actividades-Completadas.aspx/getActs")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                //data: JSON.stringify({distname: distSelected}),
                success: function (response) {
                    data = JSON.parse(response.d);
                    if (data == -1) { alert("Ocurrio un error inesperado. Intentelo de nuevo mas tarde"); }
                    if (data == -2) { alert("Error en Id actividad. Contacte a soporte por favor"); }
                    if (data == -3) { alert("Sin actividades"); }
                    if (data != -1 && data != -2 && data != -3) {
                        $('#tbl_actividades').DataTable({
                            destroy: true,
                            data: data,
                            columns: [
                                { data: 'id' },
                                { data: 'nombre' },
                                { data: 'duracion' },
                                { data: 'fecha_inicio' },
                                { data: 'fecha_completada' },
                                { data: 'estatus' },
                                { data: 'evaluador' },
                                { data: 'distribuidor' },
                                { data: 'usuario' }
                            ],
                            rowReorder: true,
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
                                            filename: 'Pendientes'
                                        }
                                    ]
                                }
                            },
                            order: [[0, "desc"]],
                            "createdRow": function (row, data, dataIndex) {
                                $(row).attr('onclick', 'redirect(' + data.idob +')');
                            }
                        });
                    }
                    
                }
            });
        }

        function redirect(id) {
            //console.log(id);
            //var split = id.split("-");
            //var oid = split[1];
            window.location.href = "SelectActividad.aspx?id=" + id;

        }

        function actsAdmin() {
             $.ajax({
                url: '<%=ResolveUrl("Actividades-Completadas.aspx/getActividades")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                data: JSON.stringify({distname: 'todos'}),
                success: function (response) {
                    data = JSON.parse(response.d);

                    //console.log(data);
                    if (data != -1) {
                        $('#tbl_actividades').DataTable({
                            destroy: true,
                            data: data,
                            columns: [
                                { data: 'id' },
                                { data: 'nombre' },
                                { data: 'duracion' },
                                { data: 'fecha_inicio' },
                                { data: 'fecha_completada' },
                                { data: 'estatus' },
                                { data: 'evaluador' },
                                { data: 'distribuidor' },
                                { data: 'usuario' }
                            ],
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
                                            filename: 'Pendientes-' + $('#drop_dist option:selected').text()
                                        }
                                    ]
                                }
                            },
                            order: [[0, "desc"]],
                            "createdRow": function (row, data, dataIndex) {
                                $(row).attr('onclick', 'redirect(' + data.id.split('-')[1] + ')');
                            }
                        });
                    } else {
                        alert("sin registros");
                    }
                    
                }
            });
        }
    </script>
</asp:Content>
