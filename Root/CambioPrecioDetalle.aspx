<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CambioPrecioDetalle.aspx.cs" Inherits="Root.CambioPrecioDetalle" %>
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
                function Checkall () {
                    var checkboxes = $('#ContentPlaceHolder1_gvDocumentosDetalle').find(':enabled:checkbox');
                    checkboxes.prop('checked', 'checked');
                    return;
                }
                function DeselectAll() {
                    if (confirm('TODAS LAS PARTIDAS DEL DOCUMENTO SERÁN RECHAZADAS. ¿DESEA CONTINUAR?')) {
                        var checkboxes = $('#ContentPlaceHolder1_gvDocumentosDetalle').find(':enabled:checkbox');
                        checkboxes.prop('checked', false);
                        return true;
                    }
                    else {
                        return false;
                    }
                    
                }
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
            <asp:ModalPopupExtender ID="modalAgregar" runat="server" PopupControlID="pnlAgregar" TargetControlID="btnHidden" CancelControlID="btnHidden" BackgroundCssClass="modal-backdrop"></asp:ModalPopupExtender>
            <asp:ModalPopupExtender ID="modalLog" runat="server" PopupControlID="pnlLog" TargetControlID="btnHidden2" CancelControlID="btnHidden2" BackgroundCssClass="modal-backdrop"></asp:ModalPopupExtender>
            <div class="container-fluid">
                    <table class="table table-sm table-borderless" border="0" width="100%">
                        <tr>
                            <td colspan="2" valign="top" class="w-25">
                                <h5>CAMBIOS DE PRECIO</h5>
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
                            <td colspan="2">
                                <div class="input-group">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text border-dark">Rol de Usuario</span>
                                    </div>
                                    <asp:DropDownList ID="ddlRolesUsuario" runat="server" ClientIDMode="Static" CssClass="form-control border-dark" AutoPostBack="true" OnSelectedIndexChanged="ddlRolesUsuario_SelectedIndexChanged" ></asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Folio:
                            </td>
                            <td>
                                <asp:TextBox ID="txtId" runat="server" ReadOnly="true" CssClass="form-control"/>
                                <asp:HiddenField ID="hdnDocumentoId" runat="server" />
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
                        <tr>
                            <td colspan="2" align="right">
                                <div class="btn-toolbar float-right">
                                    <div class="btn-group mr-2">
                                        <asp:Button ID="btnValidarDocumento" Text="Validar Partidas" CssClass="btn btn-outline-success" OnClick="btnValidarDocumento_Click" runat="server" Visible="false"/>
                                        <asp:Button ID="btnVerLogValidaciones" Text="Log Validaciones" CssClass="btn btn-outline-dark" OnClick="btnVerLogValidaciones_Click" runat="server" Visible="true"/>
                                    </div>
                                    <div class="btn-group mr-2">
                                        <asp:Button ID="btnCancelarDocumento" Text="Cancelar" CssClass="btn btn-outline-dark" runat="server" OnClick="btnCancelarDocumento_Click" Visible="false"/>
                                        <asp:Button ID="btnGuardarDocumento" Text="Guardar" CssClass="btn btn-outline-dark" runat="server" OnClick="btnGuardarDocumento_Click" Visible="false" OnClientClick="return confirm('¿GUARDAR CAMBIOS Y TERMINAR CAPTURA?');"/>
                                    </div>
                                    <div class="btn-group">
                                        <asp:Button ID="btnAutorizar" runat="server" ClientIDMode="Static" Text="Autorizar" CssClass="btn btn-outline-dark" OnClick="btnAutorizar_Click" />
                                        <asp:Button ID="btnRechazar" runat="server" ClientIDMode="Static" Text="Rechazar" CssClass="btn btn-outline-dark" OnClick="btnRechazar_Click1" />  
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
            </div>            
            <div class="container-fluid">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <h5>Detalle de Cambio de Precio</h5>    
                        </td>
                        <td align="right" class="float-right">
                            <div class="btn-toolbar">
                                <div class="btn-group mr-2">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-outline-primary" Text="Eliminar" OnClientClick="return confirm('SE ELIMINARÁ EL REGISTRO SELECCIONADO ¿DESEA CONTINUAR?');" OnClick="btnDelete_Click" Enabled="false"/>
                                    <asp:Button ID="btnHidden" runat="server" ClientIDMode="Static" Text="" CssClass="btn d-none" />
                                    <asp:Button ID="btnHidden2" runat="server" ClientIDMode="Static" Text="" CssClass="btn d-none" />
                                    <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-outline-primary" Text="Editar" OnClick="btnEdit_Click" Enabled="false"/>
                                    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-outline-primary" Text="Agregar" OnClick="btnAdd_Click"/>                                    
                                </div>
                                <div class="btn-group mr-2">
                                    <asp:Button ID="btnDetalle" runat="server" CssClass="btn btn-outline-primary" Text="Detalle" OnClick="btnDetalle_Click" />
                                </div>
                            </div>
                        </td> 
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="gvDocumentosDetalle" DataKeyNames="id" runat="server" CssClass="table" AllowPaging="false" PageSize="15" GridLines="None"  AutoGenerateColumns="false"
                                SelectedRowStyle-CssClass="border border-primary bg-light"
                                SelectedRowStyle-BorderStyle="Solid"
                                SelectedRowStyle-BorderWidth="1px" 
                                SelectedRowStyle-Font-Underline="true"
                                SelectedRowStyle-ForeColor="#001BC5"
                                OnPageIndexChanging="gvDocumentosDetalle_PageIndexChanging"
                                OnSelectedIndexChanging="gvDocumentosDetalle_SelectedIndexChanging">
                                <Columns>
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
                                    <asp:BoundField HeaderText="Proveedor" DataField="NAME_VENDOR" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkSeleccionar" runat="server" Text="" CssClass="fa fa-arrow-left" CommandName="Select"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Panel ID="pnlAgregar" runat="server">
                <div class="modal d-block">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                             <div class="modal-header">
                                <h5 class="modal-title">Detalle de Partida</h5>
                             </div>                           
                            <div class="modal-body p-0">
                                <table class="table table-sm table-borderless mt-0 mb-0">
                                    <tr>
                                        <td>
                                            Documento OC:
                                            <asp:HiddenField ID="hdnCambioPrecioId" runat="server" ClientIDMode="Static" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDocumentoOC" runat="server" ClientIDMode="Static" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            No. Parte:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNoParte" runat="server" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Precio:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPrecio" runat="server" ClientIDMode="Static" CssClass="form-control numeric" autocomplete="off"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Moneda:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlMoneda" runat="server" ClientIDMode="Static" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Fecha Punto Quiebre:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFechaPuntoQuiebre" runat="server" ClientIDMode="Static" CssClass="form-control datepicker" autocomplete="off" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Justificación:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtJustificacion" runat="server" ClientIDMode="Static" CssClass="form-control" autocomplete="off" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <div class="modal-footer">
                                <div class="btn-group">
                                    <asp:Button ID="btnCancelar" runat="server" ClientIDMode="Static" Text="Cancelar" CssClass="btn btn-outline-dark" OnClick="btnCancelar_Click"/>
                                    <asp:Button ID="btnAgregar" runat="server" ClientIDMode="Static" Text="Guardar" CssClass="btn btn-outline-dark" OnClick="btnAgregar_Click"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlLog" runat="server">
                <div class="modal d-block">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                             <div class="modal-header">
                                <h5 class="modal-title">Log de Validaciones</h5>
                             </div>                           
                            <div class="modal-body p-0">
                                <asp:GridView ID="gvLogValidaciones" runat="server" GridLines="None" CssClass="table table-sm table-hover" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:BoundField HeaderText="Fecha" DataField="fecha_insert" DataFormatString="{0: yyyy/MM/dd HH:mm}" />
                                        <asp:BoundField HeaderText="ID" DataField="IDENTIFICADOR" Visible="false" />
                                        <asp:BoundField HeaderText="TIPO" DataField="TYPE" />
                                        <asp:BoundField HeaderText="NUMBER" DataField="NUMBER" Visible="false" />
                                        <asp:BoundField HeaderText="MENSAJE" DataField="MESSAGE" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="modal-footer">
                                <div class="btn-group">
                                    <asp:Button ID="btnCancelarLog" runat="server" ClientIDMode="Static" Text="Aceptar" CssClass="btn btn-outline-dark" OnClick="btnCancelarLog_Click"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlRolesUsuario" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
