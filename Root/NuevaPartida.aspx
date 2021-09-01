<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NuevaPartida.aspx.cs" Inherits="Root.NuevaPartida" %>
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
            <asp:ModalPopupExtender ID="modalAgregar" runat="server" PopupControlID="pnlAgregar" TargetControlID="btnHidden" CancelControlID="btnHidden" BackgroundCssClass="modal-backdrop"></asp:ModalPopupExtender>
            <div class="container-fluid">
                <table width="100%">
                    <tr>
                        <td colspan="2" align="left">
                            <h5>NUEVAS PARTIDA
                        </td>
                    </tr>
                    <tr>
                        <td class="float-left" align="left">
                            <div class="input-group">
                                <div class="input-group-prepend ">
                                    <span class="input-group-text">Rol de Usuario:</span>
                                </div>
                                <div class="input-group-append ">
                                    <asp:DropDownList ID="ddlRolesUsuario" runat="server" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlRolesUsuario_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>
                        </td>
                        <td align="right" class="float-right">
                            
                            <div class="btn-toolbar">
                                <div class="input-group mr-2">
                                    <div class="input-group-prepend">
                                        <asp:Button ID="btnHidden" runat="server" ClientIDMode="Static" Text="" CssClass="btn d-none" />
                                        <asp:TextBox ID="txtFiltro" CssClass="form-control numeric" runat="server" placeholder="Folio" autocomplete="off"></asp:TextBox>
                                    </div>
                                    <div class="input-group-append">
                                        <asp:Button ID="btnFiltro" runat="server" Text="Buscar" CssClass="btn btn-outline-dark" OnClick="btnFiltro_Click"/>
                                    </div>
                                </div>
                                <div class="btn-toolbar float-right">
                                    <div class="btn-group">
                                        <asp:Button ID="btnDetalle" runat="server" CssClass="btn btn-outline-dark" Text="Detalle" OnClick="btnDetalle_Click" Enabled="false" Visible="false"/>
                                    </div>
                                    <div class="btn-group ">
                                        <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-outline-dark" Text="Eliminar" Visible="false" OnClick="btnDelete_Click" Enabled="false" OnClientClick="return confirm('SE ELIMINARÁ EL REGISTRO SELECCIONADO ¿DESEA CONTINUAR?');"/>
                                        <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-outline-dark" Text="Detalle" Visible="false" OnClick="btnEdit_Click" Enabled="false"/>
                                        <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-outline-dark" Text="Agregar" Visible="false" OnClick="btnAdd_Click"/>
                                    </div>
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
                            <asp:GridView ID="gvDocumentos" DataKeyNames="id" runat="server" CssClass="table" AllowPaging="true" PageSize="15" GridLines="None" AutoGenerateColumns="false" 
                                SelectedRowStyle-CssClass="border border-primary bg-light"
                                SelectedRowStyle-BorderStyle="Solid"
                                SelectedRowStyle-BorderWidth="1px"
                                SelectedRowStyle-Font-Underline="true"
                                SelectedRowStyle-ForeColor="#001BC5"
                                OnPageIndexChanging="gvDocumentos_PageIndexChanging"
                                OnSelectedIndexChanging="gvDocumentos_SelectedIndexChanging">
                                <Columns>
                                    <asp:BoundField HeaderText="Folio" DataField="id" />
                                    <asp:BoundField HeaderText="Workflow" DataField="workflow"/>
                                    <asp:BoundField HeaderText="Documento" DataField="documento" Visible="false" />
                                    <asp:BoundField HeaderText="Descripción" DataField="descripcion" />
                                    <asp:BoundField HeaderText="Fecha" DataField="fecha" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
                                    <asp:BoundField HeaderText="Usuario" DataField="usuario" />
                                    <asp:BoundField HeaderText="Estatus" DataField="Estatus" />
                                    <asp:TemplateField HeaderText="">
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
            <asp:Panel ID="pnlAgregar" runat="server" >
                <div class="modal d-block">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                             <div class="modal-header">
                                <h5 class="modal-title">Nuevo Documento</h5>
                             </div>                           
                            <div class="modal-body p-0">
                                <table class="table mt-0 mb-0">
                                    <tr>
                                        <td>
                                            Workflow:
                                        </td>
                                        <td>
                                            <asp:HiddenField ID="hdnDocumentoId" runat="server" ClientIDMode="Static" />
                                            <asp:HiddenField ID="hdnUsuarioId" runat="server" ClientIDMode="Static" />
                                            <asp:HiddenField ID="hdnLastRolAccepted" runat="server" ClientIDMode="Static" />
                                            <asp:DropDownList ID="ddlWorkflow" runat="server" ClientIDMode="Static" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="d-none">
                                        <td>
                                            Documento:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDocumento" runat="server" CssClass="form-control numeric" MaxLength="10" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Descripción:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" MaxLength="5000" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Fecha:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFecha" runat="server" ClientIDMode="Static" ReadOnly="true" Enabled="false" CssClass="form-control" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Usuario:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUsuario" runat="server" ReadOnly="true" Enabled="false" CssClass="form-control"></asp:TextBox>
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
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlRolesUsuario" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
