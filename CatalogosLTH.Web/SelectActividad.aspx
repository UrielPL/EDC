<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/edcMaster.Master" CodeBehind="SelectActividad.aspx.cs" Inherits="CatalogosLTH.Web.SelectActividad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <style>
        .card-revs {
            box-shadow: 5px 5px 5px #888888;
            margin-bottom: 2rem;
        }
    </style>

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container" style="margin-top: 12rem;">
        <div class="content">
            <div align="center">
                <h3>ACT-<%=actividad.Idactividad.Punto.Id%></h3>
                <h4><%=actividad.Idactividad.Nombre%></h4>
            </div>

            <div class="section">
                <h4>Condiciones</h4>
                <%foreach (var condicion in condiciones)
                    { %>
                <ul>
                    <li><%=condicion.NombreCondicion%></li>
                </ul>
                <%} %>
            </div>
            <div class="section">
                <h4>Objetivo</h4>
                <p>
                    <%=objetivo%>
                </p>
            </div>
            <div class="section">
                <h4>¿Qué tengo que hacer?</h4>
                <p>
                    <%=queHacer%>
                </p>
            </div>
            <hr />
            <div class="section">
                <div class="row">
                    <h4><b>Historial de revisiones</b></h4>
                    <br />
                    <%if (lstArchivos.Count() > 0)
                        {
                            foreach (var archivo in lstArchivos)
                            {%>
                    <label><%=archivo.usuario == "Evaluador" || archivo.usuario == "AdministratorEDC" ? archivo._auditoriaActividad.Evaluador.UserName : archivo._auditoriaActividad.Idaud.Distribuidor.nombredist%> || FECHA: <%:archivo.fecha.ToString("dd/MM/yyyy") %></label>
                    <div id="div<%:archivo.Oid%>" class="card-revs">
                        <div>
                            <p>Status: <%:archivo.substatus %><span></span></p>
                            <p>Comentarios: <%:archivo.comentario %></p>
                            <p>
                                Descarga:<br /><a download href="archivos/evidencia/<%:archivo.ArchivoImportar%>"><%:archivo.ArchivoImportar.FileName%></a> <br />
                                <%if (archivo.ArchivoImportar2 != null)
                                    { %>
                                <a download href="archivos/evidencia/<%:archivo.ArchivoImportar2%>"><%:archivo.ArchivoImportar2.FileName%></a><br />
                                <%} %>
                                <%if (archivo.ArchivoImportar3 != null)
                                    { %>
                                <a download href="archivos/evidencia/<%:archivo.ArchivoImportar3%>"><%:archivo.ArchivoImportar3.FileName%></a>
                                <%} %>
                            </p>
                        </div>
                    </div>
                    <%}
                        } %>
                </div>
                <hr />
                <%--<div class="row">
                    <div class="col-sm-4">
                        <label id="lblAceptado" for="aceptaEvidencia">Aceptado</label>
                        <input type="checkbox" id="aceptaEvidencia" runat="server"/>
                    </div>
                </div>--%>

                <div class="row">
                    <div class="col-sm-4">
                        <label for="comentarios">Comentario:</label>
                        <textarea class="form-control" id="comentarios" rows="3" runat="server"></textarea>
                    </div>

                </div>


            </div>
            <div class="row">
                <div class="col-sm-4">
                    <div class="input-group mb-3">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Archivo</span>
                        </div>
                        <div class="custom-file">
                            <%--<input type="file" id="archivo"/>--%>
                            <asp:FileUpload runat="server" ID="subeFile" name="subeFile" AllowMultiple="true" />
                            <label>Elegir archivo</label>
                        </div>
                    </div>
                </div>
            </div>
            <hr />
            <div class="row">
                <div class="col-sm-4">
                    <%if (usuario != "Distribuidor" && actividad.status == "En revisión" && actividad.Evaluador.UserName == nomusuario)
                        { %>
                    <%-- <input type="button" value="DEVOLVER ACTIVIDAD" onclick="regresaActi()"/>--%>
                    <asp:Button ID="Button1" Text="Devolver actividad" OnClick="btnDevolver" runat="server" />
                    <%} %>
                    <label id="errorDevolver" runat="server" style="color: red;"></label>
                    <%--<input type="button" value="GUARDAR" onclick="guardar()" />--%>
                    <%if (actividad.status == "Por realizar" && usuario == "Distribuidor")
                        { %>
                    <asp:Button ID="Button2" Text="Guardar actividad" OnClick="btnUpload2_Click" runat="server" />
                    <%} %>
                    <%if ((actividad.status == "En revisión" && actividad.Evaluador.UserName == nomusuario) || (usuario == "AdministratorEDC") /*(usuario == "Evaluador" || usuario == "GerenteCuenta" || usuario == "AdministratorEDC")*/)
                        { %>
                    <asp:Button ID="btnUpload" Text="Aceptar actividad" OnClick="btnUpload_Click" runat="server" />
                    <%} %>
                </div>
                <input type="button" value="REGRESAR A LISTADO" onclick="regresar()" style="float: right;" />
            </div>

        </div>
    </div>

    <script>

        $(document).ready(function () {
            if ("<%=usuario %>" == "Distribuidor") {
                $('#lblAceptado').hide();
                $("#ctl00_ContentPlaceHolder1_aceptaEvidencia").hide();
            }

            if ("<%=actividad.status%>" == "En revisión" || "<%=actividad.status%>" == "Completada") {
                $('#ctl00_ContentPlaceHolder1_archivo').attr('disabled', true);
                $('#ctl00_ContentPlaceHolder1_comentarios').text("<%=actividad.comentario%>");
            }

            if ("<%=actividad.status%>" == "Completada") {
                $('#ctl00_ContentPlaceHolder1_aceptaEvidencia').prop('checked', true);;
            }

        });

        function regresar() {
            window.location.href = "actividadesporrevisar.aspx";
        }

        function esconde(id) {
            $('#div' + id).hide();
        }

        function regresaActi() {
            $.ajax({
                url: "<%= ResolveUrl("SelectActividad.aspx/regresaActividad")%>",
                type: "POST",
                //cache: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ idact: <%=id%>}),
                success: function (response) {
                    var json = response.d;
                    console.log(json);

                    if (json == 1) {
                        Swal.fire({
                            title: 'Actividad devuelta',
                            text: 'La actividad se devolvio exitosamente.',
                            icon: 'success',

                            confirmButtonText:
                                '<i class="fa fa-thumbs-up"></i> ok!',
                            confirmButtonAriaLabel: 'Ok!',

                        })
                    }
                }
            });
        }


    </script>
</asp:Content>
