<%@ Page Title="" Language="C#" MasterPageFile="~/LoggedIn.Master" AutoEventWireup="true" CodeBehind="ChangePwd.aspx.cs" Inherits="SITConnect_2_.ChangePwd" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Change Password</h3>
<p>Current Password:<asp:TextBox ID="tb_CurrentPwd" runat="server" TextMode="Password"></asp:TextBox>
</p>
<p>New Password:<asp:TextBox ID="tb_NewPwd" runat="server" TextMode="Password"></asp:TextBox>
</p>
<p>Requirements: 1 uppercase,1 lowercase, 1 number, 1 special character</p>
<p>
    <asp:Button ID="btn_Submit" runat="server" OnClick="btn_Submit_Click" Text="Submit" />
    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
    <asp:Label ID="lbl_Success" runat="server" ForeColor="Lime"></asp:Label>
</p>
</asp:Content>
