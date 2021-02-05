<%@ Page Title="" Language="C#" MasterPageFile="~/LoggedIn.Master" AutoEventWireup="true" CodeBehind="MyAccount.aspx.cs" Inherits="SITConnect_2_.MyAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <br />
    </p>
    <p>
        <asp:Button ID="btnLogout" runat="server" Text="Logout" onClick="LogoutMe"/>
    </p>
    <p>
        <asp:Button ID="btnChangePwd" runat="server" Text="Change Password" OnClick="btnChangePwd_Click" />
    </p>
</asp:Content>
