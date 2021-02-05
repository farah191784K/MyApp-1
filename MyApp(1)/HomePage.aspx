<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="MyApp_1_.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <fieldset>
                <legend>HomePage</legend>
                <br />
                &nbsp
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" />
                <br />
                <br />
                &nbsp
                <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="LogoutMe" Visible="false" />
                
                <br />
                
                <br />
                
            </fieldset>
        </div>
        <br />
        <div id="lbl_title" style="font-weight: bold">
            Feedback<br />
            <br />
            <br />
            <asp:Label ID="lbComment" runat="server" Font-Bold="False" Text="Your comments"></asp:Label>
            <br />
            <asp:TextBox ID="tb_comments" runat="server" Height="100px" Width="600px"></asp:TextBox>
            <br />
            <br />
            <br />
            <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" Text="Submit" Width="150px" />
            <br />
            <br />
            <asp:Label ID="lbl_comments" runat="server" Font-Bold="False" Text="Comments"></asp:Label>
        </div>
    </form>
</body>
</html>