<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Autorizaciones.aspx.cs" Inherits="Root.Autorizaciones" %>
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
            <div class="container-fluid">
                <table width="100%">
                    <tr>
                        <td colspan="2" align="left">
                            <h5>AUTORIZACIONES PENDIENTES
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
                        <asp:HiddenField ID="hdnDocumentoId" runat="server" ClientIDMode="Static" />
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
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlRolesUsuario" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>