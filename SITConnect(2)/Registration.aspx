<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="SITConnect_2_.Registration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://www.google.com/recaptcha/api.js?render=6Ld7JEUaAAAAAESYnfxBDdqz3pxFI8LaD5pDMrGN"></script>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6Ld7JEUaAAAAAESYnfxBDdqz3pxFI8LaD5pDMrGN', { action: 'Register' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
    <script type="text/javascript">
        function validate()
        {
            var str = document.getElementById('<%=tb_pwd.ClientID %>').value;


            if (str.length < 8) {
                document.getElementById('<%=lbl_pwdchecker.ClientID %>').innerHTML = "Password Length must be at least 8 characters";
                document.getElementById('<%=lbl_pwdchecker.ClientID %>').style.color = "Red";
                return ("too_short");
            }

            else if (str.search(/[0-9]/) == -1) {
                document.getElementById('<%=lbl_pwdchecker.ClientID %>').innerHTML = "Password require at least 1 number";
                document.getElementById('<%=lbl_pwdchecker.ClientID %>').style.color = "#ff6200";
                return ("no_number");
            }

            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById('<%=lbl_pwdchecker.ClientID %>').innerHTML = "Password require at least 1 upper case alphabet";
                document.getElementById('<%=lbl_pwdchecker.ClientID %>').style.color = "#ff9100";
                return ("no_uppercase");
            }

            else if (str.search(/[a-z]/) == -1) {
                document.getElementById('<%=lbl_pwdchecker.ClientID %>').innerHTML = "Password require at least 1 lower case alphabet";
                document.getElementById('<%=lbl_pwdchecker.ClientID %>').style.color = "#ffb700";
                return ("no_lowercase");
            }
            else if (str.search(/[!*@#$%^&+=]/) == -1) {
                document.getElementById('<%=lbl_pwdchecker.ClientID %>').innerHTML = "Password require at least 1 special character";
                document.getElementById('<%=lbl_pwdchecker.ClientID %>').style.color = "#ffd900";
                return ("no_specialchar");
            }
            document.getElementById('<%=lbl_pwdchecker.ClientID %>').innerHTML = "Excellent!"
            document.getElementById('<%=lbl_pwdchecker.ClientID %>').style.color = "#6fff00";
            return "good"


        }
    </script>
        <div>
            <p>First Name&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="tb_fname" runat="server"></asp:TextBox>
                <asp:Label ID="lbl_fname" runat="server" ForeColor="Red"></asp:Label>
            </p>
            <p>Last Name&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="tb_lname" runat="server"></asp:TextBox>
                <asp:Label ID="lbl_lname" runat="server" ForeColor="Red"></asp:Label>
            </p>
            <p>Credit Card No.&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="tb_ccno" runat="server" Width="407px" CssClass="auto-style1"></asp:TextBox>
                <asp:Label ID="lbl_ccno" runat="server" ForeColor="Red"></asp:Label>
            </p>
            <p>CVV<asp:TextBox ID="tb_cvv" runat="server"></asp:TextBox>
                <asp:Label ID="lbl_cvv" runat="server" ForeColor="Red"></asp:Label>
            </p>
            <p>Credit Card Expiry<asp:TextBox ID="tb_exp" runat="server" TextMode="Month">Month</asp:TextBox>
                <asp:Label ID="lbl_ccexp" runat="server" ForeColor="Red"></asp:Label>
            </p>
            <p>Email Address&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="tb_email" runat="server" Width="346px" TextMode="Email"></asp:TextBox>
                <asp:Label ID="lbl_email" runat="server" ForeColor="Red"></asp:Label>
            </p>
            <p>Password&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password" onkeyup="javascript:validate()"></asp:TextBox>
                <asp:Label ID="lbl_pwdchecker" runat="server" Text="pwdchecker"></asp:Label>
            </p>
            <p>
                <asp:Label ID="lbl_pwd" runat="server" ForeColor="Red"></asp:Label>
            </p>
            <p>Requirements: 1 uppercase alphabet,1 lowercase alphabet,1 number,1 special character</p>
            <!--onkeyup="javascript:validate()"-->
            <p>Date of Birth&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="tb_dob" runat="server" TextMode="Date"></asp:TextBox>
                <asp:Label ID="lbl_dob" runat="server" ForeColor="Red"></asp:Label>
            </p>
            <p>
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Submit" />
            </p>

            
        </div>
        
        <div class ="g-recaptcha" data-sitekey="6Lf1JkQaAAAAAAWYGvKP0QmJnBW6ocWu3uumQUpM">
        <input type="hidden" id="g-captcha-response" name="g-captcha-response" />
                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
            <br />
            <asp:Label ID="lbl_gScore" runat="server"></asp:Label>
    </div>

    </div>

</asp:Content>

    
