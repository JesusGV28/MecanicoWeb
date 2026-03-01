<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="detalleServicio.aspx.cs" Inherits="TallerMecanicoWeb.Frontend.detalleServicio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card shadow">
            <div class="card-header bg-info text-white d-flex justify-content-between">
                <h4 class="mb-0">Detalles de la Orden de Servicio</h4>
                <asp:Label ID="lblFolio" runat="server" Font-Bold="true" />
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <asp:GridView ID="gvDetalles" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-bordered table-hover" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="IdServicio" HeaderText="ID" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                            <asp:BoundField DataField="Cantidad" HeaderText="Cant." />
                            <asp:BoundField DataField="Precio" HeaderText="Precio Unit." DataFormatString="{0:C}" />
                            <asp:BoundField DataField="Importe" HeaderText="Subtotal" DataFormatString="{0:C}" ItemStyle-Font-Bold="true" />
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="row mt-3 justify-content-end">
                    <div class="col-md-3">
                        <div class="border p-2 bg-light text-end">
                            <strong>Total de la Orden: </strong>
                            <asp:Label ID="lblTotal" runat="server" CssClass="h5 text-primary" Text="$0.00" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer text-muted">
                <asp:Button ID="btnVolver" runat="server" Text="Volver a la Lista" CssClass="btn btn-secondary" OnClick="btnVolver_Click" />
            </div>
        </div>
    </div>
</asp:Content>
