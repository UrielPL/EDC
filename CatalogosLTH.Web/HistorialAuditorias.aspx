<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/edcMaster.Master" CodeBehind="HistorialAuditorias.aspx.cs" Inherits="CatalogosLTH.Web.HistorialAuditorias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="//cdn.datatables.net/1.12.1/css/jquery.dataTables.min.css">
    <script type="text/javascript" charset="utf8" src="//cdn.datatables.net/1.12.1/js/jquery.dataTables.min.js"></script>
    <style>
        th {
            text-align: center;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container" style="margin-top: 40px">
        <div class="panel-body" style="margin-top: 8rem">
            <h3>Historial de auditorias</h3>

            <table class="table table-hover" id="tbl_historial">
                <thead>
                    <tr>
                        <th style="text-align:center;">ID</th>
                        <th style="text-align:center;">Distribuidor</th>
                        <th style="text-align:center;">Fecha</th>
                        <th style="text-align:center;">Fecha Cierre</th>
                        <th style="text-align:center;">Estatus</th>
                        <th style="text-align:center;">Calificación Final</th>
                        <th style="text-align:center;">Calificación en Desarrollo</th>
                        <th style="text-align:center;">Formato</th>
                        <th style="text-align:center;">Autor</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>

            </table>
        </div>
    </div>

    <script>

        $(document).ready(function () {
           
            $.ajax({
                url: '<%=ResolveUrl("HistorialAuditorias.aspx/traeAuditorias")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                success: function (response) {
                    data = JSON.parse(response.d);
                    //console.log(response);
                    //console.log(data);

                    $('#tbl_historial').DataTable({
                        data: data,
                        columns: [
                            { data: 'ID' },
                            { data: 'Distribuidor' },
                            { data: 'Fecha' },
                            { data: 'Fecha_Cierre' },
                            { data: 'Estatus' },
                            { data: 'CalificacionF' },
                            { data: 'CalificacionD' },
                            { data: 'Formato' },
                            { data: 'Autor' }
                        ],
                        rowReorder: true,
                        order: [[0, "desc"]],
                        "createdRow": function (row, data, dataIndex) {
                            $(row).attr('onclick', 'redirect(' + data.ID + ')');
                        }
                    });
                }
            });

        });

        function redirect(id) {
            window.location.href = "NuevoNivel.aspx?u=" + id;
        }

    </script>

</asp:Content>
