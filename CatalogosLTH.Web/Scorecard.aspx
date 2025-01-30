<%@ Page Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="Scorecard.aspx.cs" Inherits="CatalogosLTH.Web.Scorecard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div style="margin-top: 16rem">
            <h2>Scorecard</h2>

            <table class="table">
                <thead>
                    <tr>
                        <th>Distribuidor</th>
                        <th>Fecha</th>
                        <th>Scorecard</th>
                        <th>SC Autoauditoria</th>
                        <th>SC Auditoria EDC-III</th>
                        <th>SC Tabla Financiera</th>
                        <th>SC Proyecto Desarrollo</th>
                        <th>SC Proyecto Etapa 2</th>
                        <th>SC Proyecto Etapa 3</th>
                        <th>Puntuacion Final</th>
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
                url: '<%=ResolveUrl("Scorecard.aspx/getScore")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                success: function (response) {
                    var data = JSON.parse(response.d);
                    //console.log(data);

                    if (data.length > 0) {
                        var html = '';
                        $.each(data, function (index, element) {
                            //console.log(index + " " + element.distribuidor);

                            html += '\
                                <tr> \
                                    <td>' + element.distribuidor +'</td> \
                                    <td>' + element.fecha + '</td>" \
                                    <td>' + element.scorecard + '%</td> \
                                    <td>' + element.sc_autoauditoria + '%</td> \
                                    <td>' + element.sc_auditedc3 + '%</td> \
                                    <td>' + element.sc_tablafinanciera + '%</td> \
                                    <td>' + element.sc_proyecto + '%</td> \
                                    <td>' + element.sc_proyecto2 + '%</td> \
                                    <td>' + element.sc_proyecto3 + '%</td> \
                                    <td><b>' + element.puntajeFinal + '%</b></td> \
                                </tr>\
                            ';
                        });

                        $("tbody").append(html);
                    } else {
                        console.log("Vacio");
                    }
                }
            });

        });
    </script>
</asp:Content>
