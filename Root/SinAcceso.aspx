<%@ Page Title="" Language="C#" MasterPageFile="~/SiteNA.Master" AutoEventWireup="true" CodeBehind="SinAcceso.aspx.cs" Inherits="Root.SinAcceso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
            <div class="modal d-block">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header badge-danger">
                            <h4>Acceso Denegado</h4>
                        </div>
                        <div class="modal-body">
                            <p>
                                No tiene los permisos necesarios para utilizar la aplicación.
                            </p>
                            <p>
                                Pongase en contacto con el departamento de Sistemas para requerir los permisos necesarios.
                            </p>
                        </div>
                    </div>
                </div>
            </div>
    </div>

</asp:Content>
