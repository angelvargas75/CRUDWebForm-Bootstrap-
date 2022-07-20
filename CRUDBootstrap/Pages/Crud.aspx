<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Crud.aspx.cs" Inherits="CRUDBootstrap.Pages.Crud" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    CRUD
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <br />
    <div class="mx-auto" style="width: 250px">
        <asp:Label ID="lblTitulo" runat="server" CssClass="h2"></asp:Label>
    </div>
    <form runat="server" class="h-100 d-flex align-items-center justify-content-center">
        <div>
            <div class="mb-3">
                <label class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="mb-3">
                <label class="form-label">Edad</label>
                <asp:TextBox ID="txtEdad" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="mb-3">
                <label class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="mb-3">
                <label class="form-label">Fecha de nacimiento</label>
                <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
            </div>
            <asp:Button ID="btnNuevo" runat="server" CssClass="btn btn-primary" Text="Nuevo" Visible="false" OnClick="btnNuevo_Click" />
            <asp:Button ID="btnActualizar" runat="server" CssClass="btn btn-primary" Text="Actualizar" Visible="false" OnClick="btnActualizar_Click" />
            <asp:Button ID="btnEliminar" runat="server" CssClass="btn btn-primary" Text="Eliminar" Visible="false" OnClick="btnEliminar_Click" />
            <asp:Button ID="btnVolver" runat="server" CssClass="btn btn-primary btn-dark" Text="Volver" Visible="true" OnClick="btnVolver_Click" />
        </div>
    </form>
</asp:Content>
