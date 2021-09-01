<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HistoricoDetalle.aspx.cs" Inherits="Root.HistoricoDetalle" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="upProgress" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div style="position:absolute; top:0px; left:0px; cursor:wait; z-index:1000001; width:100%; height:100%; background-color: rgba(255, 255, 255,0.5);">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="upPanel" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                $(document).ready(function() {
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
                    EndRequestHandler();
                    function EndRequestHandler(sender, args) {
                        $('.datepicker').datepicker({
                            dateFormat: 'yy/mm/dd', beforeShow: function () {
                                setTimeout(function () {
                                    $('.ui-datepicker').css('z-index', 99999999999999);
                                }, 0);
                            }
                        });
                        $('.numeric').numeric({decimal : ".",  decimalPlaces : 2 });
                    }
                });
            </script>
            <div class="container-fluid">
                    <table class="table table-sm table-borderless" border="0" width="100%">
                        <tr>
                            <td colspan="2" valign="top" class="w-25">
                                <h5><asp:Label ID="lblWorkflow" runat="server"></asp:Label> </h5>
                            </td>
                            <td rowspan="50" valign="top" align="right" class="w-50">
                                <asp:GridView ID="gvHistorico" runat="server" GridLines="None" Width="300px" CssClass="table" Caption="<h5>Historico</h5>"
                                    AutoGenerateColumns="false">
                                    <RowStyle Wrap="false" />
                                    <AlternatingRowStyle Wrap="false" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Estatus" DataField="Estatus" />
                                        <asp:BoundField HeaderText="Rol" DataField="Rol" />
                                        <asp:BoundField HeaderText="Usuario" DataField="Usuario" />
                                        <asp:BoundField HeaderText="Fecha Registro" DataField="Fecha" DataFormatString="{0: yyyy/MM/dd HH:mm}" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Folio:
                            </td>
                            <td>
                                <asp:TextBox ID="txtId" runat="server" ReadOnly="true" CssClass="form-control"/>
                                <asp:HiddenField ID="hdnDocumentoId" runat="server" />
                                <asp:HiddenField ID="hdnWorkflowId" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Workflow:
                            </td>
                            <td>
                                <asp:TextBox ID="txtWorkflow" runat="server" ReadOnly="true" CssClass="form-control"/>
                            </td>
                        </tr>
                        <tr class="d-none">
                            <td>
                                Documento:
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocumento" runat="server" ReadOnly="true" CssClass="form-control"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Descripción:
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescripcion" runat="server" ReadOnly="true" CssClass="form-control"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Fecha:
                            </td>
                            <td>
                                <asp:TextBox ID="txtFecha" runat="server" ReadOnly="true" CssClass="form-control"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Usuario:
                            </td>
                            <td>
                                <asp:TextBox ID="txtUsuario" runat="server" ReadOnly="true" CssClass="form-control"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Estatus:
                            </td>
                            <td>
                                <asp:TextBox ID="txtEstatus" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
            </div>            
            <div class="container-fluid">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <h5>Detalle de <asp:Label ID="lblWorkflow2" runat="server"></asp:Label></h5>    
                        </td>
                        <td align="right" class="float-right">

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="gvDocumentosDetalle" DataKeyNames="id" runat="server" CssClass="table" AllowPaging="false" PageSize="15" GridLines="None"  AutoGenerateColumns="true"
                                SelectedRowStyle-CssClass="border border-primary bg-light"
                                SelectedRowStyle-BorderStyle="Solid"
                                SelectedRowStyle-BorderWidth="1px" 
                                SelectedRowStyle-Font-Underline="true"
                                SelectedRowStyle-ForeColor="#001BC5"
                                OnPageIndexChanging="gvDocumentosDetalle_PageIndexChanging">
                                <%--<Columns>
                                    <asp:BoundField HeaderText="Id" DataField="id" />
                                    <asp:BoundField HeaderText="Partida" DataField="row_number" />
                                    <asp:BoundField HeaderText="DocumentoId" DataField="DocumentoId" Visible="false" />
                                    <asp:BoundField HeaderText="Documento OC" DataField="documento_oc" />
                                    <asp:BoundField HeaderText="No. Parte" DataField="no_parte" />
                                    <asp:BoundField HeaderText="Precio" DataField="precio" />
                                    <asp:BoundField HeaderText="Moneda" DataField="moneda" />
                                    <asp:BoundField HeaderText="Fecha PuntoQuiebre" DataField="fecha_punto_quiebre" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
                                    <asp:BoundField HeaderText="Justificacion" DataField="justificacion" />
                                    <asp:BoundField HeaderText="Usuario" DataField="usuario" />
                                    <asp:BoundField HeaderText="Fecha Registro" DataField="fecha_actualizacion" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
                                    <asp:BoundField HeaderText="Item No" DataField="ITEM_NO" />
                                    <asp:BoundField HeaderText="Unidad Medida" DataField="MEINS" />
                                    <asp:BoundField HeaderText="Precio Ant." DataField="NETPBR" />
                                </Columns>--%>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
