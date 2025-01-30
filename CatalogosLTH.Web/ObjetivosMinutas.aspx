<%@ Page Title="" Language="C#" MasterPageFile="~/EDCClarios.Master" AutoEventWireup="true" CodeBehind="ObjetivosMinutas.aspx.cs" Inherits="CatalogosLTH.Web.ObjetivosMinutas" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .labelCampo {
            padding: 5px; 
            background-color: #6141b0; 
            color: white;
        }

        .labelCampoInv {
            padding: 5px;
            background-color: #00b0f0; 
            color: white;
        }

        .clarios-table tbody tr:nth-child(1) {
            font-weight: bold;
        }

        .clarios-table tbody tr:nth-child(1) td:last-child {
            color: #6141b0;
        }

        .clarios-table tbody tr td:last-child {
            font-weight: 800;
        }

        .clarios-table tbody tr:last-child td {
             font-weight: 800;
        }

        .clarios-table tbody tr:not(:first-child) td:first-child {
            color: #6141b0;
            font-weight: bold;
        }

        a.class, a.class:hover {
            text-decoration: none;
        }

        .btnBack {
            padding: 10px;
            background-color: #6141b0;
            color: white;
            font-size: 18px;
        }

        #ctl00_ContentPlaceHolder1_btnGuardarDist {
            margin-top: 0px!important;
        }

        .tarjeta{
            -webkit-box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
            -moz-box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
            box-shadow: 7px 7px 14px -1px rgba(0,0,0,0.75);
        }
    </style>
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 text-center" style="background-color: #6141b0; color: white; padding: 10px; font-size: 18px; font-weight: 600;">
                <span style="text-transform: uppercase;">CARGA DE OBJETIVOS</span>
            </div>
        </div>
        <br /><br />
        <div class="container">
            <div class="row">
                <% if (ViewState["cargaExcel"] != null && ViewState["cargaExcel"].ToString() == "1") { %>
                    <div id="alerta" class="col-md-12">
                        <div class="alert alert-success alert-dismissible fade show" role="alert">
                            Carga de Objetivos completada.
                            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                    </div>
                <% } %>
                <div class="col-md-2">
                    <% if(user.TipoUsuario == CatalogosLTH.Module.BusinessObjects.TipoUsuario.GerenteCuenta ||
                            user.TipoUsuario == CatalogosLTH.Module.BusinessObjects.TipoUsuario.Admin) { %>
                    <label>Cargar EXCEL:</label>
                    <asp:FileUpload ID="archivo" runat="server" />
                    <dx:ASPxButton ID="btnCargar" OnClick="btnCargar_Click" runat="server" Text="Cargar" Theme="ThemeLTH" SkinID="SkinClarios"></dx:ASPxButton>
                    <% } %>
                </div>
                <div class="col-md-6 offset-md-4">
                    <a class="class float-right tarjeta" href="MinutasIndex.aspx">
                        <span class="btnBack">Regresar</span>
                    </a>
                    <% if (user.TipoUsuario == CatalogosLTH.Module.BusinessObjects.TipoUsuario.GerenteCuenta) { %>
                        <br /><br /><br /><br />
                        <div class="row col-md-12">
                            <div class="col-md-3">
                                <label>Nombre de Distribuidor: </label>
                            </div>
                            <div class="col-md-5">
                                <dx:ASPxTextBox ID="txtDistMin" runat="server" Width="160px"></dx:ASPxTextBox>
                            </div>
                            <div class="col-md-4">
                                <dx:ASPxButton ID="btnGuardarDist" OnClick="btnGuardarDist_Click" runat="server" Text="Guardar" Theme="ThemeLTH" SkinID="SkinClarios"></dx:ASPxButton>
                            </div>
                        </div>
                    <% } %>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-6">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="labelCampo text-center">
                                <span>AÑO FISCAL</span>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <dx:ASPxComboBox ID="cbxFY" AutoPostBack="true" OnSelectedIndexChanged="cbxFY_SelectedIndexChanged" runat="server" Theme="ThemeLTH" ValueType="System.String"></dx:ASPxComboBox>
                        </div>
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-10 ">
                    <table class="table table-bordered clarios-table tarjeta">
                        <thead>
                            <tr class="text-center">
                                <th style="background-color: #6141b0; color: white;" scope="col" colspan="<%=lstDist.Count() + lstDistMin.Count() + 2 %>">OBJETIVOS DE MINUTAS POR DISTRIBUIDOR</th>
                            </tr>
                        </thead>
                        <tbody class="text-center">
                            <tr>
                                <td>MES</td>
                                <% for(int i = 0; i<lstDist.Count(); i++) { %>
                                    <td><a href="EdicionObjetivosMinutas.aspx?distribuidor=<%=lstDist[i].UserName%>&FY=<%=DateTime.Now.Year%>"><%=lstDist[i].UserName %></a></td>
                                <% } %>
                                <% for(int i = 0; i<lstDistMin.Count(); i++) { %>
                                    <td><a href="EdicionObjetivosMinutas.aspx?distribuidor=<%=lstDistMin[i].nombredist%>&FY=<%=DateTime.Now.Year%>"><%=lstDistMin[i].nombredist %></a"href=""></td>
                                <% } %>
                                <td>TOTAL</td>
                            </tr>
                            <tr>
                                <td>OCT</td>
                                <% for(int i = 0; i<lstDist.Count(); i++) { %>
                                    <td><%=lstObjetivos[i][0] %></td>
                                <% } %>
                                <% for(int i = 0; i<lstDistMin.Count(); i++) { %>
                                    <td><%=lstObjetivos[i + lstDist.Count()][0] %></td>
                                <% } %>
                                <td><%=lstTotalMes[0]%></td>
                            </tr>
                            <tr>
                                <td>NOV</td>
                                <% for(int i = 0; i<lstDist.Count(); i++) { %>
                                    <td><%=lstObjetivos[i][1] %></td>
                                <% } %>
                                 <% for(int i = 0; i<lstDistMin.Count(); i++) { %>
                                    <td><%=lstObjetivos[i + lstDist.Count()][1] %></td>
                                <% } %>
                                <td><%=lstTotalMes[1]%></td>
                            </tr>
                            <tr>
                                <td>DIC</td>
                                <% for(int i = 0; i<lstDist.Count(); i++) { %>
                                    <td><%=lstObjetivos[i][2] %></td>
                                <% } %>
                                 <% for(int i = 0; i<lstDistMin.Count(); i++) { %>
                                    <td><%=lstObjetivos[i + lstDist.Count()][2] %></td>
                                <% } %>
                                <td><%=lstTotalMes[2]%></td>
                            </tr>
                            <tr>
                                <td>ENE</td>
                                <% for(int i = 0; i<lstDist.Count(); i++) { %>
                                    <td><%=lstObjetivos[i][3] %></td>
                                <% } %>
                                 <% for(int i = 0; i<lstDistMin.Count(); i++) { %>
                                    <td><%=lstObjetivos[i + lstDist.Count()][3] %></td>
                                <% } %>
                                <td><%=lstTotalMes[3]%></td>
                            </tr>
                            <tr>
                                <td>FEB</td>
                                <% for(int i = 0; i<lstDist.Count(); i++) { %>
                                    <td><%=lstObjetivos[i][4] %></td>
                                <% } %>
                                 <% for(int i = 0; i<lstDistMin.Count(); i++) { %>
                                    <td><%=lstObjetivos[i + lstDist.Count()][4] %></td>
                                <% } %>
                                <td><%=lstTotalMes[4]%></td>
                            </tr>
                            <tr>
                                <td>MAR</td>
                                <% for(int i = 0; i<lstDist.Count(); i++) { %>
                                    <td><%=lstObjetivos[i][5] %></td>
                                <% } %>
                                 <% for(int i = 0; i<lstDistMin.Count(); i++) { %>
                                    <td><%=lstObjetivos[i + lstDist.Count()][5] %></td>
                                <% } %>
                                <td><%=lstTotalMes[5]%></td>
                            </tr>
                            <tr>
                                <td>ABR</td>
                                <% for(int i = 0; i<lstDist.Count(); i++) { %>
                                    <td><%=lstObjetivos[i][6] %></td>
                                <% } %>
                                 <% for(int i = 0; i<lstDistMin.Count(); i++) { %>
                                    <td><%=lstObjetivos[i + lstDist.Count()][6] %></td>
                                <% } %>
                                <td><%=lstTotalMes[6]%></td>
                            </tr>
                            <tr>
                                <td>MAY</td>
                                <% for(int i = 0; i<lstDist.Count(); i++) { %>
                                    <td><%=lstObjetivos[i][7] %></td>
                                <% } %>
                                 <% for(int i = 0; i<lstDistMin.Count(); i++) { %>
                                    <td><%=lstObjetivos[i + lstDist.Count()][7] %></td>
                                <% } %>
                                <td><%=lstTotalMes[7]%></td>
                            </tr>
                            <tr>
                                <td>JUN</td>
                                <% for(int i = 0; i<lstDist.Count(); i++) { %>
                                    <td><%=lstObjetivos[i][8] %></td>
                                <% } %>
                                 <% for(int i = 0; i<lstDistMin.Count(); i++) { %>
                                    <td><%=lstObjetivos[i + lstDist.Count()][8] %></td>
                                <% } %>
                                <td><%=lstTotalMes[8]%></td>
                            </tr>
                            <tr>
                                <td>JUL</td>
                                <% for(int i = 0; i<lstDist.Count(); i++) { %>
                                    <td><%=lstObjetivos[i][9] %></td>
                                <% } %>
                                 <% for(int i = 0; i<lstDistMin.Count(); i++) { %>
                                    <td><%=lstObjetivos[i + lstDist.Count()][9] %></td>
                                <% } %>
                                <td><%=lstTotalMes[9]%></td>
                            </tr>
                            <tr>
                                <td>AGO</td>
                                <% for(int i = 0; i<lstDist.Count(); i++) { %>
                                    <td><%=lstObjetivos[i][10] %></td>
                                <% } %>
                                 <% for(int i = 0; i<lstDistMin.Count(); i++) { %>
                                    <td><%=lstObjetivos[i + lstDist.Count()][10] %></td>
                                <% } %>
                                <td><%=lstTotalMes[10]%></td>
                            </tr>
                            <tr>
                                <td>SEP</td>
                                <% for(int i = 0; i<lstDist.Count(); i++) { %>
                                    <td><%=lstObjetivos[i][11] %></td>
                                <% } %>
                                 <% for(int i = 0; i<lstDistMin.Count(); i++) { %>
                                    <td><%=lstObjetivos[i + lstDist.Count()][11] %></td>
                                <% } %>
                                <td><%=lstTotalMes[11]%></td>
                            </tr>
                            <tr>
                                <td>TOTAL</td>
                                <% for(int i = 0; i<lstDist.Count(); i++) { %>
                                    <td><%=lstTotalDist[i] %></td>
                                <% } %>
                                <% for(int i = 0; i<lstDistMin.Count(); i++) { %>
                                    <td><%=lstTotalDist[i + lstDist.Count()] %></td>
                                <% } %>
                                <td><%=lstTotalMes.Sum()%></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>