<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCambioPrecio.ascx.cs" Inherits="Root.UserControls.ucCambioPrecio" %>
<asp:Panel ID="pnlCambioPrecio" runat="server">
    <div class="modal d-block">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Cambio de Precio</h5>
                </div>
                <div class="modal-body p-0">
                    <table class="table table-sm table-borderless mt-0 mb-0">
                        <tr>
                            <td>Doc. Orden Compra: 
                                <asp:HiddenField ID="hdnCambioPrecioId" runat="server" ClientIDMode="Static" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocumentoOC" runat="server" ClientIDMode="Static" CssClass="form-control numeric" MaxLength="10" autocomplete="off"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>No. Parte:
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoParte" runat="server" MaxLength="50" ClientIDMode="Static" CssClass="form-control" placeholder=""></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Precio:
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrecio" runat="server" ClientIDMode="Static" CssClass="form-control numeric" autocomplete="off"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Moneda:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMoneda" runat="server" ClientIDMode="Static" CssClass="form-control" AutoPostBack="false"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Fecha Punto Quiebre:
                            </td>
                            <td>
                                <asp:TextBox ID="txtFechaPuntoQuiebre" runat="server" ClientIDMode="Static" CssClass="form-control datepicker" autocomplete="off"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Justificación:
                            </td>
                            <td>
                                <asp:TextBox ID="txtJustificacion" runat="server" ClientIDMode="Static" CssClass="form-control" autocomplete="off" MaxLength="5000" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
