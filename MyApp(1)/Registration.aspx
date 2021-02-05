<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="MyApp_1_.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 174px;
        }
        .auto-style2 {
            height: 42px;
            width: 174px;
        }
        .auto-style3 {
            width: 352px;
        }
        .auto-style4 {
            height: 42px;
            width: 352px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>
                <br />
                <asp:Label ID="Label1" runat="server" Text="Account Registration"></asp:Label>
                <br />
                <br />
            </h2>
    <table class="style1">
        <tr>
            <td class="auto-style4">First Name</td>
            <td class="auto-style2">
                <asp:TextBox ID="tb_FName" runat="server" Height="36px" Width="280px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style4">Last Name</td>
            <td class="auto-style2">
                <asp:TextBox ID="tb_LName" runat="server" Height="32px" Width="281px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style4">Credit Card Number</td>
            <td class="auto-style2">
                <asp:TextBox ID="tb_CCinfo" runat="server" Height="32px" Width="281px"></asp:TextBox>
            </td>
        </tr>
        <%--<tr>
            <td class="auto-style4">CVV</td>
            <td class="auto-style2">
                <asp:TextBox ID="tb_CVV" runat="server" Height="32px" Width="281px"></asp:TextBox>
            </td>
        </tr>--%>
        <%--<tr>
            <td class="auto-style4">Expiry Date</td>
            <td class="auto-style2">
                <asp:TextBox ID="tb_expDate" runat="server" Height="32px" Width="281px"></asp:TextBox>
            </td>
        </tr>--%>
        <tr>
            <td class="auto-style4">Email</td>
            <td class="auto-style7">
                <asp:TextBox ID="tb_email" runat="server" Height="32px" Width="281px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style3">Password</td>
            <td class="auto-style1">
                <asp:TextBox ID="tb_password" runat="server" Height="32px" Width="281px" TextMode="Password"></asp:TextBox>
                &nbsp;<asp:Label ID="lbl_pwdchecker" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="auto-style3">Date of Birth</td>
            <td class="auto-style1">
                <asp:TextBox ID="tb_DOB" runat="server" Height="32px" Width="281px" TextMode="Date"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style4">&nbsp;</td>
            <td class="auto-style5">
                <asp:Button ID="btn_submit" runat="server" Text="Register" Height="48px" 
                    Width="288px" OnClick="btn_submit_Click"/>
            </td>
        </tr>
        <br />
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td class="auto-style4">&nbsp;</td>
            <td class="auto-style5">
                <asp:Button ID="btnLogin" runat="server" OnClick="btn_login" Text="Login"  Height="48px" 
                    Width="288px" />

            </td>
        </tr>
    </table>
        &nbsp;<br />
        <asp:Label ID="lb_error1" runat="server"></asp:Label>
        <br />
        <asp:Label ID="lb_error2" runat="server"></asp:Label>
    <br />
        <br />
    
    </div>
    </form>
    </body>
</html>
