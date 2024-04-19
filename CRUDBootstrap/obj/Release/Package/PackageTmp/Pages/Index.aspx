<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="CRUDBootstrap.Pages.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    INDEX
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <form runat="server">
        <div class="d-flex" role="search" style="padding: 15px 255px 5px 255px;">
            <asp:TextBox ID="txtFilter" runat="server" CssClass="form-control me-2" />
            <asp:Button ID="btnBuscar" runat="server" class="btn btn-outline-success" OnClick="btnBuscar_Click1" Text="Buscar" />
        </div>

        <br />
        <div class="mx-auto" style="width: 300px;">
            <h2>Listado de registro</h2>
        </div>
        <br />
        <div class="container">
            <div class="row">
                <div class="col align-self-end">
                    <asp:Button ID="btnNuevo" runat="server" CssClass="btn btn-success form-control" Text="Nuevo" OnClick="btnNuevo_Click" />
                </div>
            </div>
        </div>
        <br />

        <%-- <div class="container row">--%>
        <div class="table small" style="padding: 20px;">
            <asp:GridView ID="dgvListado" runat="server" class="table table-borderless table-hover">
                <Columns>
                    <asp:TemplateField HeaderText="Opciones del administrador">
                        <ItemTemplate>
                            <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" CssClass="btn form-control-sm btn-outline-success" OnClick="btnActualizar_Click" />
                            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn form-control-sm btn-outline-danger" OnClick="btnEliminar_Click" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <%--      </div>--%>
    </form>
</asp:Content>

