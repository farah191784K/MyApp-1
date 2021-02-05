<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePwd.aspx.cs" Inherits="MyApp_1_.ForgotPwd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style2 {
            height: 42px;
            width: 174px;
        }
        .auto-style4 {
            height: 42px;
            width: 248px;
        }
        .auto-style5 {
            width: 176px;
        }
        .auto-style6 {
            width: 248px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>
                <br />
                Change Password<br />
                <br />
            </h2>
    <table class="style1">
        <tr>
            <td class="auto-style4">Email</td>
            <td class="auto-style2">
                <asp:TextBox ID="tb_email" runat="server" Height="36px" Width="280px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style4">Current Password</td>
            <td class="auto-style2">
                <asp:TextBox ID="tb_CurrPwd" runat="server" Height="36px" Width="280px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style4">New Password</td>
            <td class="auto-style2">
                <asp:TextBox ID="tb_pwd" runat="server" Height="32px" Width="281px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style4">Confirm Password</td>
            <td class="auto-style2">
                <asp:TextBox ID="tb_Cpwd" runat="server" Height="32px" Width="281px"></asp:TextBox>
            </td>
        </tr>
       
        <tr>
            <td class="auto-style4">&nbsp;</td>
            <td class="auto-style5">
                <asp:Button ID="btn_update" runat="server" Text="Update" Height="48px" 
                    Width="288px" OnClick="btn_submit_Click"/>
            </td>
        </tr>
        <tr>
        <td class="auto-style6">&nbsp;<asp:Label ID="lbl_pwdchecker" runat="server"></asp:Label></td>
            </tr>
    </table>
       
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <br />
        <br />
    
    </div>
    </form>
    </body>
</html>
