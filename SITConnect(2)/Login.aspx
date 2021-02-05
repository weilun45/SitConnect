<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SITConnect_2_.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://www.google.com/recaptcha/api.js?render="></script>
    <script>
         grecaptcha.ready(function () {
             grecaptcha.execute('  ', { action: 'Login' }).then(function (token) {
         document.getElementById("g-recaptcha-response").value = token;
         });
         });
    </script>
    <p>
        &nbsp;</p>
    <p>
        Email Address
        <asp:TextBox ID="tb_email" runat="server" Height="22px"></asp:TextBox>
        <asp:Button ID="btnRegister" runat="server" OnClick="btnRegister_Click" Text="Register" Visible="False" />
    </p>
    <p>
        Password
        <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password"></asp:TextBox>
    </p>
    <p>
        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="LoginMe" />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    </p>
    <div class ="g-recaptcha" data-sitekey="6Le-CUgaAAAAAMnGLLi_D0PQzEfFjctXNk-FZKmN">
    <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
    <asp:Label ID="lbl_gScore" runat="server"></asp:Label>
</div>
</asp:Content>
