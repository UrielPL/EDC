<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/edcMaster.Master" CodeBehind="BibliotecaActividades.aspx.cs" Inherits="CatalogosLTH.Web.BibliotecaActividades" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdn.datatables.net/2.0.3/css/dataTables.dataTables.css" />
    

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container" style="margin-top: 40px">

        <div class="panel-body" style="margin-top: 8rem" id="contenido" runat="server">
            <h3>Biblioteca de actividades</h3>

            <table id="tbl_actividades" class="display">
                <thead>
                    <tr>
                        <th style="text-align:center;">Code</th>
                        <th style="text-align:center;">Nombre</th>
                        <th style="text-align:center;">Pilar</th>
                        <th style="text-align:center;">Nivel</th>
                    </tr>
                </thead>
                <tbody>
                    <%foreach (var item in lista)
                        { %>
                        <tr onclick="redirect(<%=item.id%>)">
                            <td><%=item.code %></td>
                            <td><%=item.nombre %></td>
                            <td><%=item.pilar %></td>
                            <td><%=item.nivel %></td>
                        </tr>
                    <%} %>
                </tbody>
            </table>
        </div>

    </div>


    
    <script src="https://cdn.datatables.net/2.0.3/js/dataTables.js"></script>
    <script>

        $(document).ready(function () {

            $('#tbl_actividades').DataTable({
                rowReorder: true,
                order: [[0, "asc"]]
                
            });

        });

        function alerta(objeto) {
            console.log(objeto);
        }

        function redirect(id) {
            window.open("DetallesActividad.aspx?act=" + id, "_blank");
        }

    </script>
</asp:Content>