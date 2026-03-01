<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmServicios.aspx.cs" Inherits="TallerMecanicoWeb.Frontend.frmServicios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4 mb-5">
        <div class="card border-primary shadow">
            <div class="card-header bg-primary text-white">
                <h4 class="mb-0"><i class="fas fa-file-invoice"></i> Registro de Orden de Servicio</h4>
            </div>
            <div class="card-body">
                
                <h5 class="text-secondary border-bottom pb-2 mb-3">Información General</h5>
                <div class="row g-3 mb-4">
                    <div class="col-md-4">
                        <asp:Label ID="lblCliente" runat="server" CssClass="form-label" Text="Seleccionar Cliente"></asp:Label>
                        <asp:DropDownList ID="ddlClientes" runat="server" CssClass="form-select" 
                            AutoPostBack="true" OnSelectedIndexChanged="ddlClientes_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        <asp:Label ID="Label3" runat="server" CssClass="form-label" Text="Vehículo"></asp:Label>
                        <asp:DropDownList ID="ddlVehiculos" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        <asp:Label ID="Label4" runat="server" CssClass="form-label" Text="Estado de la Orden"></asp:Label>
                        <asp:DropDownList ID="DropDownList2" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Abierta" Value="abierta"></asp:ListItem>
                            <asp:ListItem Text="En Proceso" Value="en proceso"></asp:ListItem>
                            <asp:ListItem Text="Finalizada" Value="finalizada"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        <asp:Label ID="Label5" runat="server" CssClass="form-label" Text="Fecha Entrega Estimada"></asp:Label>
                        <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                </div>

                <h5 class="text-secondary border-bottom pb-2 mb-3">Detalle del Servicio</h5>
                <div class="bg-light p-3 rounded mb-4 border">
                    <div class="row g-3">
                        <div class="col-md-6">
                            <asp:Label ID="Label9" runat="server" CssClass="form-label" Text="Servicio a Realizar"></asp:Label>
                            <asp:DropDownList ID="ddlServicios" runat="server" CssClass="form-select"></asp:DropDownList>
                        </div>
                        <div class="col-md-6">
                            <asp:Label ID="Label6" runat="server" CssClass="form-label" Text="Descripción Adicional"></asp:Label>
                            <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" placeholder="Ej. Cambio de aceite sintético"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <asp:Label ID="Label7" runat="server" CssClass="form-label" Text="Cantidad"></asp:Label>
                            <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCantidad" runat="server" 
                                ControlToValidate="TextBox2" ErrorMessage="Requerido" 
                                ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-md-4">
                            <asp:Label ID="Label8" runat="server" CssClass="form-label" Text="Precio Unitario"></asp:Label>
                            <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPrecio" runat="server" 
                                ControlToValidate="TextBox3" ErrorMessage="Ingrese precio" 
                                ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-4 d-flex align-items-end">
                            <button type="button" class="btn btn-outline-secondary w-100">
                                <i class="fas fa-plus-circle"></i> Agregar a la Lista
                            </button>
                        </div>
                    </div>
                </div>

                <div class="d-flex justify-content-end gap-2 p-3">
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-outline-danger px-4" CausesValidation="false" OnClick="btnCancelar_Click" />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Orden de Servicio" CssClass="btn btn-primary px-4" OnClick="btnGuardar_Click" />
                </div>

            </div> 
        </div> 
    </div> 
</asp:Content>
