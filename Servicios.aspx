<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Servicios.aspx.cs" Inherits="TallerMecanicoWeb.Servicios" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="d-flex justify-content-between align-items-center mb-3">
            <h2 class="text-primary"><i class="fas fa-file-invoice"></i> Órdenes de Servicio</h2>
            <div>
                <asp:LinkButton ID="btnAgregar" runat="server" CssClass="btn btn-success" OnClick="btnAgregar_Click">
                    <i class="fas fa-plus"></i> Nueva Orden
                </asp:LinkButton>
            </div>
        </div>

        <div class="card shadow-sm">
            <div class="card-body p-0">
                <asp:GridView ID="GridViewDetalleOrdenServisio" runat="server" 
                    CssClass="table table-hover table-striped mb-0" GridLines="None" AutoGenerateColumns="False" OnSelectedIndexChanged="GridViewDetalleOrdenServisio_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="folio" HeaderText="Folio" />
                        <asp:BoundField DataField="fechaIngreso" HeaderText="Ingreso" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="numeroSerieVehiculo" HeaderText="Vehículo (VIN)" />
                        <asp:BoundField DataField="estado" HeaderText="Estado" />
                        <asp:BoundField DataField="costoTotal" HeaderText="Total" DataFormatString="{0:C}" />
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnVerDetalle" runat="server" 
                                    CssClass="btn btn-sm btn-success" 
                                    OnCommand="btnDetalleservicio_Click" 
                                    CommandArgument='<%# Eval("folio") %>'>
                                    <i class="fas fa-eye"></i> Ver Detalle
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="table-dark" />
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
