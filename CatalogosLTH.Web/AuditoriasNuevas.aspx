<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/edcMaster.Master" CodeBehind="AuditoriasNuevas.aspx.cs" Inherits="CatalogosLTH.Web.AuditoriasNuevas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="//cdn.datatables.net/1.12.1/css/jquery.dataTables.min.css">
    <script type="text/javascript" charset="utf8" src="//cdn.datatables.net/1.12.1/js/jquery.dataTables.min.js"></script>
    <style>
        .btnNueva {
            padding: 10px;
            background-color: green;
            color: white;
            border-radius: 3px;
            float: right;
            text-transform: none;
            cursor: pointer;
            text-decoration: none;
            margin-bottom: 1rem;
        }

        th {
            text-align: center;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container" style="margin-top: 40px">
        <div class="panel-body" style="margin-top: 8rem">

            <%if (rol == "Distribuidor" || auto==true)
                { %>
            <h3>Auto Auditorias</h3>
            <%}
                else
                {%>
            <h3>Auditorias Nuevas</h3>
            <%} %>

            <%if (auto == true && incompleto == false)
                { %>
            <a class="btnNueva" href="CreaAutoAuditoria.aspx">Nueva auditoria</a>
            <%}
            if (auto == false)
            {%>
            <a class="btnNueva" href="CreaAuditoria.aspx">Nueva auditoria</a>
            <%} %>
            <table class="table table-hover" id="tbl_auditorias">
                <thead>
                    <tr>
                        <th style="text-align:center;">ID</th>
                        <th style="text-align:center;">Distribuidor</th>
                        <th style="text-align:center;">Fecha</th>
                        <th style="text-align:center;">Fecha de Cierre</th>
                        <th style="text-align:center;">Estatus</th>
                        <th style="text-align:center;">Autoauditoria</th>
                        <th style="text-align:center;">Autor</th>
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

            $.ajax({
                url: '<%=ResolveUrl("AuditoriasNuevas.aspx/jsonAuditorias")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                success: function (response) {
                    data = JSON.parse(response.d);
                    //console.log(response);
                    //console.log(data);
                }
            });

            $('#tbl_auditorias').DataTable({
                data: data,
                columns: [
                    { data: 'ID' },
                    { data: 'Distribuidor' },
                    { data: 'Fecha' },
                    { data: 'FechaCierre' },
                    { data: 'Estatus' },
                    { data: 'Autoauditoria' },
                    { data: 'Autor' }
                ],
                rowReorder: true,
                order: [[0, "desc"]],
                "createdRow": function (row, data, dataIndex) {
                    $(row).attr('onclick', 'redirect(' + data.ID + ')');
                }
            });
        });

        function redirect(id) {
            if("<%=rol%>" == "Distribuidor" || <%=auto.ToString().ToLower()%> == true){
                window.location.href = "CreaAutoAuditoria.aspx?aud=" + id;
            }else{
                window.location.href = "CreaAuditoria.aspx?aud=" + id;
            }
            
        }

    </script>
</asp:Content>
