﻿<%@ Page Title="" Language="C#" MasterPageFile="~/lthMasterPage.Master" AutoEventWireup="true" CodeBehind="auditoriacali.aspx.cs" Inherits="CatalogosLTH.Web.auditoriacali" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register TagPrefix="dx" Namespace="DevExpress.Xpo" Assembly="DevExpress.Xpo.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="//ajax.googleapis.com/ajax/libs/jquery/2.2.4/jquery.min.js"></script>
<script src="//cdn.rawgit.com/rainabba/jquery-table2excel/1.1.0/dist/jquery.table2excel.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Auditoria</h1>
    Tipo de Auditoria:
    <asp:DropDownList ID="cmbTipoAuditoria" runat="server"></asp:DropDownList>
    Distribuidor:
    <asp:DropDownList ID="cmbDistribuidor" runat="server"></asp:DropDownList>
    <asp:Button ID="btnConsultar" runat="server" Text="Consultar" class="btn btn-primary" OnClick="btnConsultar_Click" />
    <%@ Import Namespace="DevExpress.Xpo" %>
    <%@ Import Namespace="CatalogosLTH.Module.BusinessObjects" %>
    <%@ Import Namespace="CatalogosLTH.Web" %>
    <%
        if (ViewState["empezar"].Equals("true"))
        {



    %>
    <div class="panel-body">
                           
                            <div class="table-responsive">
                                <table id="resultados"  class="table table-striped">
                                   <col width="60%">
                                   <col width="10%">
                                   <col width="30%">
                                    <thead>
                                        <tr>
                                            <th>Punto</th>
                                            <th>Acreditado</th>
                                            <th>Observacion</th>
                                        </tr>
                                    </thead>
                                    <tbody>
        <%
            DevExpress.Xpo.Session session = Util.getsession();
            string IdAuditoria = ViewState["IdAuditoria"].ToString();

            XPQuery<mdl_auditoria> auditorias = session.Query<mdl_auditoria>();
            XPQuery<mdl_audidet> audDetalles = session.Query<mdl_audidet>();
            XPQuery<mdl_punto> puntos = session.Query<mdl_punto>();
            var sqlA = from a in auditorias
                       where a.idaud == int.Parse(IdAuditoria) && a.estatus == 0

                       select a;
            mdl_auditoria audi = null;
            foreach (var item in sqlA)
            {
                audi = item;

            }


            var sql = from ad in audDetalles
                      join p in puntos on ad.Idpunto equals p
                      where ad.Idaud.idaud == audi.idaud
                      orderby ad.resultado descending
                      orderby p.Idpilar,p.texto
                      select new { Punto = p.texto, Resultado = ad.resultado,Pilar=p.Idpilar.nombrepil,Id=ad.id,Comentario=ad.texto };
            int num = sql.Count();
            string pilar = "";
            foreach (var item in sql)
            {
                %>
                                        <%
                                            if (!item.Pilar.Equals(pilar))
                                            {
                                                pilar = item.Pilar;
                                                %>
                                        <tr><td><h2 data-toggle="collapse" data-target="#<%:pilar %> " ><%:pilar %>  <button type="button" class="btn btn-primary" >Expandir</button></h2> </td></tr>
                                        <%
                                            }
                                             %>
        <tr  id="<%:pilar %>" class="collapse">
            
            <td><%:item.Punto %></td>
            
              <td>
                <div class="switch">
                <input name="califa<%:item.Id %>" id="cmn-toggle-<%:item.Id %>" class="cmn-toggle cmn-toggle-round" type="checkbox" <%:(item.Resultado==1)?"checked":"" %>>
                <label for="cmn-toggle-<%:item.Id %>" name="califal<%:item.Id %>"></label>
                </div>                
             </td>
            <td><input class="form-control input-lg" name="coment<%:item.Id %>" type="text" value="<%:item.Comentario %>"></td>
        </tr>
        <%
    }
             %>
    </table>
                                </div>
                            <!-- /.table-responsive -->
                            
                        </div>
                        <!-- /.panel-body -->
    <%} %>
    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" class="btn btn-primary" />
    <asp:Button ID="btnVerNivel" runat="server" Text="Nivel" OnClick="btnVerNivel_Click" class="btn btn-primary" />
    <asp:Button ID="btnExport" runat="server" Text="Exporta Excel" OnClick="btnExport_Click" class="btn btn-success" />


    
</asp:Content>
