﻿<%@ Page Title="" Language="C#" MasterPageFile="~/edcMaster.Master" AutoEventWireup="true" CodeBehind="SeleccionActividad.aspx.cs" Inherits="CatalogosLTH.Web.SeleccionActividad" %>

<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Xpo.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Xpo" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxHtmlEditor.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxHtmlEditor" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.ASPxSpellChecker.v22.1, Version=22.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxSpellChecker" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <link rel="stylesheet" href="css/slicknav.css" />
      <script src="//ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
      <script src="js/jquery.bxslider/jquery.bxslider.min.js"></script>
      <script src="js/jquery.owl-carousel/owl.carousel.js"></script>
      <script src="js/jquery.fancybox/jquery.fancybox.pack.js"></script>
      <script src="js/master.js"></script>  
      <script src="js/jquery.slicknav.js"></script>
      <script src="http://cdnjs.cloudflare.com/ajax/libs/modernizr/2.6.2/modernizr.min.js"></script>

     <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <%@ Import Namespace="DevExpress.Xpo" %>
            <%@ Import Namespace="CatalogosLTH.Module.BusinessObjects" %>
            <%@ Import Namespace="CatalogosLTH.Web" %>
    <script type="text/javascript">  
        function Count() {  
  
            var i = document.getElementById("<%=TextBox1.ClientID%>").value.length;
            document.getElementById("display").innerHTML = (850 - i )+" caracteres restantes";


            if (i > 849)
            {
                var txt = document.getElementById("<%=TextBox1.ClientID%>").value;
                var subtxt = txt.substring(0, 849);
                document.getElementById("<%=TextBox1.ClientID%>").value = subtxt;
                //text.value = text.value.substring(0, maxlength);
                alert(" máximo 850 caracteres ");
            }
 
        }  
    </script>  

     <div class="section  banner-biblioteca remove-top remove-bottom">
		<div class="container">
			<div class="row">
				<div class="twelve columns">
					<h1>Actividad</h1>
				</div>
			</div>
		</div>
	</div>	
     <nav class="navbar navbar-default" style="background-color:#F8F8F8; border-bottom:solid; border-bottom-width:0.5px; border-color:#E7E7E7;margin-bottom:0 ">
          <div class="container">
              <div class="col-sm-12">
                  <div class="row">
                          <div align="center">
                              <%string codigoactividad = ViewState["codigoActividad"] as string;%>
                                <h1> <span class="label label-primary"><%:codigoactividad %></span></h1>
                          </div>                          
                  </div>
                  <div class="row">
                      <div align="center">
                          <%string nombreactividad = ViewState["nombreActividad"] as string;%>
                          <h3><%:nombreactividad %></h3>
                      </div>
                  </div>
                <div class="row">
                    <asp:Literal ID="literalInstruccion" runat="server"></asp:Literal>
                </div>
              
              </div>
                               
          </div>
      </nav> 

      <div class="container">

          <div class="row">
         
                  <div class="col-sm-4" >
                      <div class="row"  style="border: solid, 1px;border-color:lightgray; border-radius:5px">
                           <asp:Label ID="lblCode" runat="server" Text="Label" Font-Bold="True" Font-Names="Calibri" ForeColor="#2190C5"  ></asp:Label>
                            <br />
                            <asp:Label ID="lblTexto" runat="server" Text="Label" Visible="false" ></asp:Label>
                            <br />
                          <asp:Label ID="lblDist" runat="server" Font-Bold="True" Font-Names="Calibri" ForeColor="#2190C5" ></asp:Label>
                           <dx:ASPxCheckBox ID="CheckAceptado" runat="server"  CheckState="Unchecked" Visible="False" OnCheckedChanged="CheckAceptado_CheckedChanged">
                               <CheckedImage Height="36px" Url="~/Images/Toggle On-64.png" Width="120px">
                               </CheckedImage>
                               <UncheckedImage Height="36px" Url="~/Images/Toggle Off-64.png" Width="120px">
                               </UncheckedImage>
                           </dx:ASPxCheckBox>
                           <br />
                      </div>
                      <hr />
                      <div class="row">

                           <asp:RegularExpressionValidator ID="revMessageBoardContents" ControlToValidate="TextBox1"
                                    Text="Excede el tamaño de 850 caracteres" ValidationExpression="^[\s\S]{0,850}$" runat="server"/>

                          <div class="col-sm-3">
                              <p>Comentario</p>
                          </div>
                          <div class="col-sm-3">
                              <span class="label label-warning" id="display"> 850 caracteres restantes </span>
                          </div>
                            <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" onkeyup="Count()" spellcheck="false" CssClass="form-control" MaxLength="850" ></asp:TextBox>
                          <br />
                      </div>
                      <hr />
                      <div class="row">
                        <p>Archivo</p>
                        
                        <asp:FileUpload ID="FileUpload1" name="FileUpload1" runat="server" />
                      </div>
                      <hr />
                      <div class="row">
                          <div class="col-sm-5"></div>
                          <div class="col-sm-3">
                              <asp:Button ID="btnConfirm" runat="server" Text="Regresar"  CssClass="btn btn-success" PostBackUrl="~/listaactividades.aspx" />
                          </div>
                          <div class="col-sm-1"></div>
                          <div class="col-sm-3">
                              <asp:Button ID="btnUpload" runat="server" Text="Guardar" CssClass="btn btn-default"  OnClick="btnUpload_Click" />
                          </div>
                        
                      </div>
                 
                      <div class ="row">&nbsp;</div>
                      <div class ="row">
                         <!-- <a href="listaactividades.aspx">REGRESAR</a>-->
                      </div>
                  </div>
              <div class="col-sm-1" style="margin-bottom:60%"></div>

                  <div class="col-sm-7" style="margin-top:1%">
                           <asp:Label ID="lblHistorial" runat="server" Text="Historial de revisiones" Font-Bold="false" Font-Names="Calibri" ForeColor="#2190C5"  Font-Size="Large" ></asp:Label>
                           <div class ="row">&nbsp;</div>
                      <hr />
                       <div class="row">                     
                          
                                      <%
                                          foreach (var item in listaArchivos)
                                          { %>

                                             <div class="col-sm-12" style="margin-top:2%;margin-bottom:1%; border-radius:5px; border:solid, 2px; box-shadow:5px 5px 5px #888888;">
                                                 <div class ="row" style="background-color:#f7f7f7;"  data-toggle="collapse" data-target='#<%:item.Oid %>'> 
                                                      
                                                     <div class="col-sm-7" >
                                                         <%if (item.usuario == "Distribuidor")
                                                             {%>  
                                                         <p style="font-size:12px; font-family:Calibri;color:darkslategray;"><%:item.IdAuditoriaActividad.Idaud.Iddistribuidor.nombredist %> (ver comentario)</p> 
                                                          <%}
                                                                                                  else
                                                                                                  {  %>                                            
                                                        <p style="font-size:12px; font-family:Calibri;color:darkslateblue;"><%:item.IdAuditoriaActividad.Evaluador.Nombre%>  (ver comentario)</p> 
                                                         <%} %>

                                        
                                                     </div>
                                                     <div class="col-sm-1"></div>
                                                     <div class="col-sm-4">
                                                         <p style="color:grey;font-size:12px; font-family:Calibri"><%:item.fecha.ToShortDateString()+"    " %><%:item.fecha.ToShortTimeString() %></p>
                                                     </div>
                                                 </div>
                                                 <div class="collapse" id=<%:item.Oid%> >
                                                     <div class="col-sm-12" ><!--comentario-->
                                                         <div class="row">
                                                             <p style="color:#777777;font-size:12px; font-family:Calibri;text-decoration:underline">Status: <%:item.substatus %></p>
                                                         </div>
                                                         <%if (item.comentario.Length > 0)
                                                             { %>
                                                         <div class="row" style="margin:2%">
                                                            <p style="font-size:13px; font-family:Calibri"> <%:item.comentario %></p>
                                                         </div>                             
                                                         <%} %>        
                                                     </div>
                                                     <div class="row">

                                                         <%if (item.ArchivoImportar!=null){%>
                                                             <div class="col-sm-5" style="margin-bottom:1%">
                                                                 <asp:Label ID="Label1" runat="server" Text="Descarga: "></asp:Label>
                                                                 <button type="button" style="font-size:11px;color:cornflowerblue; text-decoration:underline; padding-top:1%;padding-bottom:1%;padding-right:3px;padding-left:3px;border:solid, 1px;"  name="<%:item.ArchivoImportar.FileName %>"  id="<%:item.Oid %>" onclick="file(this.id,this.name)" ><%:item.ArchivoImportar.FileName%> </button>
                                                                 <%--<a download href="archivos/Evidencia/<%:item.ArchivoImportar.FileName%>" target="_blank"><%:item.ArchivoImportar.FileName%></a>                         --%>
                                                              </div>
                                                    
                                                        <%}%>                                         
                                                     
                                                     </div>
                                                 </div>
                                             </div>

                                      <%
                                          };
                                      %>

                                        </div>

                      </div>

                           <div class ="row" style="visibility:hidden">                          
                             <dx:ASPxGridView ID="ASPxGridView1" runat="server" EnableTheming="True" Theme="ThemeLTH"  Style="" EnableRowsCache="False" Settings-UseFixedTableLayout="False">
                            <SettingsBehavior AllowFocusedRow="True" AllowSelectByRowClick="True" AllowSelectSingleRowOnly="True" />
                            <SettingsDataSecurity AllowEdit="False" AllowInsert="False" AllowDelete="False"></SettingsDataSecurity>
                       
                            <Styles AdaptiveDetailButtonWidth="22" FixedColumn-Wrap="Default" >
                            </Styles>
                            </dx:ASPxGridView>
                          
                              <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Ver Archivo" ></dx:ASPxButton>
                          </div>
                 

                      
                  </div><!--cierra div 6-->

              </div>

          </div>
    <script>


        function file(id,na) {

            //console.log("lawea: " + id);
            var nombre = getNombre(id);
            console.log("Nombre: "+na);

            $.ajax({
                type: "POST",
                url: "SeleccionActividad.aspx/getfile",
                contentType: "application/json; charset=utf-8",
                data: "{'id':\"" + id + "\" }",
                dataType: "Json",
                processData: false,
                success: function (data) {
                                     
                    data = data.d;

                    var byteArray = new Uint8Array(data);
                    var a = window.document.createElement('a');

                    a.href = window.URL.createObjectURL(new Blob([byteArray], { type: 'file/docx' }));

                    // supply your own fileName here...
                    // a.download = nombre;
                    //a.download = "Actividad.docx";
                    a.download = na;

                    document.body.appendChild(a)
                    a.click();
                    document.body.removeChild(a)

                }
            });
        }

        function getNombre(id) {
            var nombre = "";

            $.ajax({
                type: "POST",
                url: "SeleccionActividad.aspx/getnombre",
                contentType: "application/json; charset=utf-8",
                data: "{'id':\"" + id + "\" }",
                dataType: "Json",
                success: function (data) {

                    return data.d;
                  



                }
            });

            //return nombre;
        }


        function file2(id, name) {

            $file = '/path/to/your/dir/'.name;

            if (!$file) { // file does not exist
                die('file not found');
            } else {
                header("Cache-Control: public");
                header("Content-Description: File Transfer");
                header("Content-Disposition: attachment; filename=$file");
                header("Content-Type: application/zip");
                header("Content-Transfer-Encoding: binary");

                // read the file from disk
                readfile($file);
            }
        }
       
        function descarga(element) {
            console.log(element);

            var ruta = element.href;
            var nombre = element.text;

            var enlaceTemporal = document.createElement('a');
            enlaceTemporal.href = ruta;
            enlaceTemporal.download = nombre;

            // Añade el enlace temporal al cuerpo del documento
            document.body.appendChild(enlaceTemporal);

            // Simula un clic en el enlace para iniciar la descarga
            enlaceTemporal.click();

            // Elimina el enlace temporal del cuerpo del documento
            document.body.removeChild(enlaceTemporal);
        }

    </script>
   

</asp:Content>


