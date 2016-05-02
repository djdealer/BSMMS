<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="InstaFollow.LicenseGen.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Your License</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>

                <div style="font-family: Arial, Helvetica, sans-serif;font-size: 14px;text-align:center; width: 400px;margin: auto;text-align:center">
                <br/><br/><br/>
                <h2>Please provide your data:</h2>
                <label>First name:</label><br/>
                <asp:TextBox runat="server" ID="tbFirstName"></asp:TextBox><br/><br/>
        
                <label>Last name:</label><br/>
                <asp:TextBox runat="server" ID="tbLastName"></asp:TextBox><br/><br/>
        
                <label>Paypal Email:</label><br/>
                <asp:TextBox runat="server" ID="tbEmail"></asp:TextBox><br/><br/>
        
                <asp:Button runat="server" ID="btnGetLicense" OnClick="OnClickBtnGetLicense" Text="Get Licence" />
                <h2>Your BETA license key:</h2>
                <asp:TextBox runat="server" ID="tbLicense" Width="450"></asp:TextBox><br/><br/>
                <asp:Label runat="server" Text="Please paste this into your application."></asp:Label>
            </div>

    </form>
</body>
</html>
