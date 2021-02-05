<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MyApp_1_.Login1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Form</title>

     <script src="https://www.google.com/recaptcha/api.js?render=6LdmHiQaAAAAAOTyjjnijZN54vw3QFPndeY3DjWl"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <fieldset>
                <legend>Login</legend>
                <p>Username : <asp:TextBox ID="tb_userid" runat="server" Height="25px" Width="137px" /> </p>
                <p>Password : <asp:TextBox ID="tb_pwd" runat="server" Height="24px" Width="137px" TextMode="Password" /> </p>
                <p><asp:Button ID="btnSubmit" runat="server" Text="Login" OnClick="LoginMe" Height="27px" Width="133px" /> 
                &nbsp; 
                <asp:Button ID="btn_reset" runat="server" Text="Reset Password" OnClick="Reset" Height="27px" Width="133px" /> 
                &nbsp;<asp:Button ID="btn_register" runat="server" Text="Register" OnClick="Register" Height="27px" Width="133px" /> 
                <br />
                <br />

                <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" >Error message here (lblMessage)</asp:Label>
                </p>
                <p>JSON Message:<asp:Label ID="lbl_gScore" runat="server"></asp:Label>
                </p>
            </fieldset>
        </div>
    </form>
    <script>
        grecaptcha.ready(function() {
           grecaptcha.execute('6LdmHiQaAAAAAOTyjjnijZN54vw3QFPndeY3DjWl', { action: 'Login' }).then(function (token) {
           document.getElementById("g-recaptcha-response").value = token;
           });
           });
    </script>
</body>
</html>