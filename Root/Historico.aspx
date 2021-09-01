<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Historico.aspx.cs" Inherits="Root.Historico" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="upProgress" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div style="position:absolute; top:0px; left:0px; cursor:wait; z-index:1000001; width:100%; height:100%; background-color: rgba(255, 255, 255,0.5);">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="upPanel" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="container-fluid"> 
                <table width="100%">
                    <tr>
                        <td colspan="2" align="left">
                            <h5>HISTORICO
                        </td>
                    </tr>
                    <tr>
                        <td class="float-left" align="left">
                            <div class="input-group">
                                <div class="input-group-prepend ">
                                    <span class="input-group-text">WorkFlow:</span>
                                </div>
                                <asp:DropDownList ID="ddlWorkflow" runat="server" ClientIDMode="Static" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlWorkflow_SelectedIndexChanged"></asp:DropDownList>
                                <div class="input-group-prepend ">
                                    <span class="input-group-text">Estatus:</span>
                                </div>
                                <asp:DropDownList ID="ddlEstatus" runat="server" ClientIDMode="Static" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                                <div class="input-group-prepend ">
                                    <span class="input-group-text">Folio:</span>
                                </div>
                                <asp:TextBox ID="txtFiltro" CssClass="form-control numeric" runat="server" placeholder="Folio" autocomplete="off"></asp:TextBox>
                                <div class="input-group-append ">
                                    <asp:Button ID="btnFiltro" runat="server" Text="Buscar" CssClass="btn btn-outline-dark" OnClick="btnFiltro_Click"/>
                                </div>
                            </div>
                        </td>
                        <td align="right" class="float-right">
                            
                            <div class="btn-toolbar">
                                <div class="btn-toolbar float-right">
                                    <div class="btn-group">
                                        
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnDetalle" runat="server" CssClass="btn btn-outline-dark" Text="Detalle" OnClick="btnDetalle_Click" Enabled="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        <asp:HiddenField ID="hdnDocumentoId" runat="server" ClientIDMode="Static" />
                            <asp:GridView ID="gvDocumentos" DataKeyNames="id" runat="server" CssClass="table table-sm table-hover" AllowPaging="true" PageSize="50" GridLines="None" AutoGenerateColumns="false" 
                                SelectedRowStyle-CssClass="border border-primary bg-light"
                                SelectedRowStyle-BorderStyle="Solid"
                                SelectedRowStyle-BorderWidth="1px"
                                SelectedRowStyle-Font-Underline="true"
                                SelectedRowStyle-ForeColor="#001BC5"
                                OnPageIndexChanging="gvDocumentos_PageIndexChanging"
                                OnSelectedIndexChanging="gvDocumentos_SelectedIndexChanging">
                                <Columns>
                                    <asp:BoundField HeaderText="Folio" DataField="id" />
                                    <asp:BoundField HeaderText="Workflow" DataField="workflow" Visible="false"/>
                                    <asp:BoundField HeaderText="Descripción" DataField="descripcion" />
                                    <asp:BoundField HeaderText="Fecha Creación" DataField="fecha" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
                                    <asp:BoundField HeaderText="Usuario Creación" DataField="usuario"  HeaderStyle-CssClass="border-right" ItemStyle-CssClass="border-right" />
                                    <asp:BoundField HeaderText="Estatus" DataField="estatus_desc" />
                                    <asp:BoundField HeaderText="Fecha Estatus" DataField="fecha_estatus" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
                                    <asp:BoundField HeaderText="Usuario Estatus" DataField="estatus_usuario" />
                                    <asp:BoundField HeaderText="Usuario Estatus Rol" DataField="estatus_rol" />
                                    <asp:BoundField HeaderText="Metodo" DataField="metodo_autorizacion" />
                                    <asp:TemplateField HeaderText="Detalle">
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
            
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
