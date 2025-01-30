<%@ Page Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="ProyectoDesarrollo.aspx.cs" Inherits="CatalogosLTH.Web.ProyectoDesarrollo" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <link rel="stylesheet" type="text/css" href="//cdn.datatables.net/1.12.1/css/jquery.dataTables.min.css">
    <script type="text/javascript" charset="utf8" src="//cdn.datatables.net/1.12.1/js/jquery.dataTables.min.js"></script>

    <style>
        .upld {
            text-align: center;
            cursor: pointer;
            background-color: #2C76F7;
            color: white;
            padding: 5px;
            border-radius: 5px;
            margin-bottom: 1rem;
        }

        .custom-link {
            color: white; /* Cambia el color del enlace */
            text-decoration: none; /* Sube línea */
            font-weight: bold; /* Texto en negrita */
            background-color: dodgerblue;
            padding: 5px;
            border-radius: 5px;
            cursor: pointer;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div style="margin-top: 16rem">
            <h3>Proyecto Desarrollo</h3>

            <div class="col-sm-2 upld" id="upload_widget_opener1">
                <p>Carga Archivo</p>
            </div>
            <table id="tbl_files" class="display">
                <thead>
                    <tr>
                        <td style="text-align: center;" >ID</td>
                        <td style="text-align: center;">Archivo</td>
                        <td style="text-align: center;">Compartió</td>
                        <td style="text-align: center;">Fecha de subida</td>
                        <td style="text-align: center;">Descargable</td>
                    </tr>
                </thead>
                <tbody></tbody>
            </table>
        </div>
    </div>

    <script src="https://upload-widget.cloudinary.com/latest/global/all.js" type="text/javascript"></script>
    <script>

        $(document).ready(function () {

            $.ajax({
                url: '<%=ResolveUrl("ProyectoDesarrollo.aspx/traeFiles")%>',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                type: 'POST',
                async: false,
                success: function (response) {
                    var json = JSON.parse(response.d);
                    //console.log(json);

                    if (json != -1) {
                        $('#tbl_files').DataTable({
                            data: json,
                            columns: [
                                { data: 'id' },
                                { data: 'nombre' },
                                { data: 'autor' },
                                { data: 'fecha' },
                                {
                                    data: 'url', // Indica que esta columna usará el campo 'url' del JSON
                                    render: function (data, type, row) {
                                        return '<a class="custom-link" href="' + data + '">Ver archivo</a>';
                                    }
                                }
                            ],
                            rowReorder: true
                        });
                    } else {
                        $('#tbl_files').DataTable();
                    }
                }
            });

        });

        //function open(url){
        //    window.open(url);
        //}

        var myUploadWidget;
        document.getElementById("upload_widget_opener1").addEventListener("click", function () {
            myUploadWidget = cloudinary.openUploadWidget({
                cloudName: "dpkyawi7g",
                uploadPreset: 'ml_default',
                //Step 3:  Add list of sources that should be added as tabs in the widget.
                sources: [
                    "local"
                ],
            },
                function (error, result) {

                    //console.log(result);
                    //Step 2.3:  Listen to 'success' event
                    if (result.event === "success") {
                        //Step 2.4:  Call the .close() method in order to close the widget
                        myUploadWidget.close();
                        //console.log(result.info.secure_url);
                        ///console.log(result.info.original_filename);
                        var file = result.info.secure_url;
                        var fileName = result.info.original_filename;

                        $.ajax({
                            url: "<%= ResolveUrl("ProyectoDesarrollo.aspx/guardaArchivo")%>",
                                type: "POST",
                                //cache: false,
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: true,
                                data: JSON.stringify({ imgUrl: file, name: fileName}),
                                success: function (data) {
                                    var resp = JSON.parse(data.d);

                                    if (resp == 1) {
                                        Swal.fire({
                                            title: 'Archivo guardado',
                                            text: 'Su archivo se subio correctamente.',
                                            icon: 'success',

                                            confirmButtonText:
                                              '<i class="fa fa-thumbs-up"></i> ok!',
                                            confirmButtonAriaLabel: 'Ok!',

                                        })
                                        location.reload();
                                    } else {
                                        Swal.fire({
                                            title: 'Archivo no guardado',
                                            text: 'Ocurrio un error al subir su archivo. Intente de nuevo mas tarde.',
                                            icon: 'info',

                                            confirmButtonText:
                                              '<i class="fa fa-thumbs-up"></i> ok!',
                                            confirmButtonAriaLabel: 'Ok!',

                                        })
                                    }
                                }
                            });
                        }
                    });
            }, false);

    </script>

</asp:Content>
