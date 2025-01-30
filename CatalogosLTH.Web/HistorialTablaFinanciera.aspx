<%@ Page Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="HistorialTablaFinanciera.aspx.cs" Inherits="CatalogosLTH.Web.HistorialTablaFinanciera" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdn.datatables.net/2.0.3/css/dataTables.dataTables.css" />

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container" style="margin-top: 40px">
        <div class="panel-body" style="margin-top: 8rem" id="contenido" runat="server">

            <div style="display:flex;">
                <h3>Tablas financieras</h3>
                <input type="button" value="Nueva tabla financiera" onclick="veATabla()" style="margin-left:auto"/>
                <input type="button" value="Historial Financiero" onclick="veAHistorial()" <%--style="margin-left:auto"--%>/>
            </div>
            <%if (permiso == "Admin" || username == "rodolfoivan.giacoman@clarios.com")
                { %>
            <div style="display:flex;">
                <input type="button" value="Historial financiero general" onclick="veHistorialGeneral()" style="margin-left:auto"/>
            </div>
            <%} %>
            <table id="myTable" class="display">
                <thead>
                    <tr>
                        <th style="text-align:center;">ID</th>
                        <th style="text-align:center;">Fecha</th>
                        <th style="text-align:center;">Autor</th>
                        <th style="text-align:center;">Distribuidor</th>
                        <th style="text-align:center;">Auto Auditoria</th>
                        <th style="text-align:center;">Estatus</th>
                        <th style="text-align:center;">Resultados Faltantes</th>
                    </tr>
                </thead>
                <tbody style="text-align:center;">
                </tbody>
            </table>
        </div>
    </div>


    <script src="https://cdn.datatables.net/2.0.3/js/dataTables.js"></script>
    <script>

        $(document).ready(function () {
            
            if("<%=permiso%>" == "Distribuidor"){
                traeXDist();
            }else{
                traeGenerales();
            }
             
        });

        function traeXDist(){
            $.ajax({
                url: "<%= ResolveUrl("HistorialTablaFinanciera.aspx/getTablasDist")%>",
                type: "POST",
                //cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = JSON.parse(response.d);

                    if (data == "-1") {
                        alert("Ocurrio un error al cargar la informacion")
                    } else {

                        $('#myTable').DataTable({
                            data: data,
                            columns: [
                                { data: 'ID' },
                                { data: 'Fecha' },
                                { data: 'Autor'},
                                { data: 'Distribuidor' },
                                { data: 'AutoAuditoria' },
                                { data: 'Estatus' },
                                { data: 'Resultados' }
                            ],
                            rowReorder: true,
                            order: [[0, "desc"]],
                            "createdRow": function (row, data, dataIndex) {
                                $(row).attr('onclick', 'redirect(' + data.ID + ')');
                            }
                        });
                    }

                }

             });
        }

        function traeGenerales(){
            $.ajax({
                url: "<%= ResolveUrl("HistorialTablaFinanciera.aspx/getTablas")%>",
                type: "POST",
                //cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = JSON.parse(response.d);

                    if (data == "-1") {
                        alert("Ocurrio un error al cargar la informacion")
                    } else {

                        $('#myTable').DataTable({
                            data: data,
                            columns: [
                                { data: 'ID' },
                                { data: 'Fecha' },
                                { data: 'Autor'},
                                { data: 'Distribuidor' },
                                { data: 'AutoAuditoria' },
                                { data: 'Estatus' },
                                { data: 'Resultados' }
                            ],
                            rowReorder: true,
                            order: [[0, "desc"]],
                            "createdRow": function (row, data, dataIndex) {
                                $(row).attr('onclick', 'redirect(' + data.ID + ')');
                            }
                        });
                    }

                }

             });
        }

        function redirect(id) {
            window.location.href = "TablaFinanciera.aspx?t=" + id;
        }

        function veATabla() {
            window.location.href = "TablaFinanciera.aspx";
        }

        function veAHistorial() {
            window.location.href = "AvanceFinanciero.aspx";
        }

        function veHistorialGeneral() {
            window.location.href = "AvanceFinancieroGeneral.aspx";
        }

    </script>

</asp:Content>
