<%@ Page Title="" Language="C#" MasterPageFile="~/EDCClarios.Master" AutoEventWireup="true" CodeBehind="VentasPorMes.aspx.cs" Inherits="CatalogosLTH.Web.VentasPorMes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        #btnExcel {
            color: white;
        }
        .btnBack {
            padding: 10px;
            background-color: #6141b0;
            color: white;
            font-size: 18px;
        }
        .tarjeta{
            -webkit-box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
            -moz-box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
            box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
        }
    </style>
    <div class="container">
        <div class="row">
            <div class="col-md-5">
                <div class="form-group">
                    <label>Selecciona el año</label>
                    <select id="selAnio" class="form-control">
                        <option value="2018">2018</option>
                        <option value="2019">2019</option>
                        <option value="2020" selected>2020</option>
                        <option value="2021">2021</option>
                    </select>
                </div>
            </div>
            <div class="col-md-5">
                <div class="form-group">
                    <label>Selecciona el mes</label>
                    <select id="selMes" class="form-control">
                        <option value="1">Enero</option>
                        <option value="2">Febrero</option>
                        <option value="3">Marzo</option>
                        <option value="4">Abril</option>
                        <option value="5">Mayo</option>
                        <option value="6">Junio</option>
                        <option value="7">Julio</option>
                        <option value="8">Agosto</option>
                        <option value="9">Septiembre</option>
                        <option value="10">Octubre</option>
                        <option value="11">Noviembre</option>
                        <option value="12">Diciembre</option>
                    </select>
                </div>
            </div>
            <div class="col-md-1 offset-md-1">
                <a class="class" href="mainpage.aspx">
                    <span class="btnBack tarjeta">Regresar</span>
                </a>
            </div>
            <div class="col-md-12">
                <button type="button" id="btnExcel" class="btn btn-success">Excel</button>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(() => {
            $('#btnExcel').on('click', () => {
                mes = $('#selMes').val();
                anio = $('#selAnio').val();
                var params = {};
                params["mes"] = mes;
                params["anio"] = anio;
                $.ajax({
                    type: "POST",
                    url: "VentasPorMes.aspx/getfile",
                    contentType: "application/json; charset=utf-8",
                    dataType: "Json",
                    data: JSON.stringify(params),
                    processData: false,
                    success: function (data) {
                        data = data.d;
                        console.log(data);
                        var byteArray = new Uint8Array(data);
                        if (window.navigator && window.navigator.msSaveOrOpenBlob) {
                            window.navigator.msSaveOrOpenBlob(new Blob([byteArray], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }), "Ventas.xlsx");
                        } else {
                            var a = window.document.createElement('a');
                            a.href = window.URL.createObjectURL(new Blob([byteArray], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }));
                            a.download = "Ventas.xlsx";
                            document.body.appendChild(a);
                            a.click();
                            document.body.removeChild(a);
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        console.log("Status: " + textStatus);
                        console.log("Error: " + errorThrown);
                        console.table(XMLHttpRequest);
                    }
                });
            });
        });
    </script>
</asp:Content>
