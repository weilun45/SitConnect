<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/LoggedIn.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SITConnect_2_._Home" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="BannerPwdExpired" Visible="false" runat="server" CssClass="alert alert-dismissable alert-warning">
      <button type="button" class="close" data-dismiss="alert">
      <span aria-hidden="true">&times;</span>
      </button>
      <asp:Label ID="lbPassExpired" runat="server">Please change your <a href="ChangePwd.aspx">password</a> as soon as possible. </asp:Label>
    </asp:Panel>
    
    
    <p>
        <br />
    </p>
    <h2>Welcome to SiTConnect!</h2>
<p>
    <asp:TextBox ID="tb_Search" runat="server"></asp:TextBox>
    <asp:Button ID="btn_Search" runat="server" Text="Search" />
</p>
    <p>
        &nbsp;</p>
    <p>
        <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="LogoutMe" Visible="false" />
    </p>

    

</asp:Content>
